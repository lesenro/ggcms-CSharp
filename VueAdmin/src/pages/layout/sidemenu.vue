<template>
  <el-menu
    :background-color="styles.side_menu_background"
    :text-color="styles.side_menu_color"
    :active-text-color="styles.side_menu_active_color"
    :collapse="isCollapse"
    :default-active="active_window"
  >
    <route-menu :key="r.fullpath" v-for="r in routers" :item="r" @menu-click="go_route"/>
  </el-menu>
</template>

<script>
import styles from "@/element-variables.scss";
import routeMenu from "../../components/menus";
import { mapState } from "vuex";
export default {
  name: "side-menu",
  data() {
    return {
      d_routers: [],
      styles: styles
    };
  },
  mounted() {
    this.menusInit();
  },
  computed: {
    ...mapState({
      active_window: state => state.global.active_window
    }),
    isCollapse: function() {
      return this.collapse;
    },
    routers() {
      return this.d_routers.filter(x=>!x.sideHidden);
    }
  },
  props: ["collapse"],
  methods: {
    go_route(path) {
      this.$router.push(path);
    },
    menusInit() {
      let paths = this.$router.history.current.fullPath
        .split("/")
        .filter(x => x);
      if (paths.length == 0) {
        return;
      }
      let menus = this.$router.options.routes.find(
        x => x.path == `/${paths[0]}`
      );
      if (!menus) {
        this.d_routers = [];
      }
      if (!menus.children) {
        this.d_routers = [menus];
      }
      // if (paths[0] == "iot") {
      //   this.$store.dispatch("global/getMyPowers").then(powers => {
      //     this.d_routers = this.getMenus(menus.children.map(x => x), powers);
      //   });
      // } else {
      //   this.d_routers = menus.children.map(x => x);
      // }
      this.d_routers = menus.children.map(x => x);
    },
    getMenus(routes, power_list) {
      let powers = [];
      //遍历菜单项
      routes.forEach(item => {
        const p = {
          closable: item.closable,
          component: item.component,
          fullpath: item.fullpath,
          iconColor: item.iconColor,
          icon: item.icon,
          isHome: item.isHome,
          name: item.name,
          path: item.path
        };
        if (item.children && item.children.length > 0) {
          p.children = this.getMenus(item.children, power_list);
        }
        let children = [];
        //判断菜单是否包含权限
        if (item.powers && item.powers.length > 0) {
          //遍历菜单权限
          item.powers.forEach(x => {
            //检索该公司是否拥有此权限
            for (let i = 0; i < x.keys.length; i++) {
              let power = power_list.find(p => p.resCode == x.keys[i]);
              if (!power) {
                return;
              }
            }

            children.push({
              label: x.name + (x.isMenu ? "-菜单" : ""),
              value: x.keys,
              icon: x.isMenu ? "lightbulb" : "user-check",
              iconColor: x.isMenu ? "#ff5722" : "#ffb74d",
              id: this.getPowerId(x.keys, power_list),
              pid: p.id,
              isMenu: x.isMenu,
              type: 1
            });
          });
        }
        if ((p.children && p.children.length > 0) || children.length > 0) {
          powers.push(p);
        }
      });
      return powers;
    },
    getPowerId(keys, power_list) {
      let ids = power_list
        .filter(x => keys.indexOf(x.resCode) != -1)
        .map(x => x.id);
      return ids.join(",");
    }
  },
  components: { routeMenu }
};
</script>
