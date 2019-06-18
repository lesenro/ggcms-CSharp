<template>
  <el-header :height="'90px'">
    <el-row>
      <el-col :span="4">
        <el-button type="text" size="mini" @click="collapseChange" class="btn-collapse">
          <i class="el-icon-s-unfold" v-if="collapse"></i>
          <i class="el-icon-s-fold" v-else></i>
        </el-button>
      </el-col>
      <el-col :span="20" text-right>
        <el-dropdown @command="handleCommand">
          <span class="el-dropdown-link">
            用户 : {{login_user.userName}}
            <i class="el-icon-arrow-down el-icon--right"></i>
          </span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="logout" icon="el-icon-circle-check-outline">退出</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-col>
    </el-row>
    <div class="top-bar">
      <div class="menus">
        <el-scrollbar ref="scrollbar" :vertical="false">
          <el-tag
            class="win-tag"
            size="medium"
            :type="active_window==item.fullpath?'':'info'"
            :closable="item.closable"
            cursor-pointer
            :key="item.fullpath"
            v-for="(item) in open_windows"
            @click="go_route(item.fullpath)"
            @close="tag_tab_close(item.fullpath)"
          >
          <i
            :style="{color:active_window==item.fullpath?item.iconColor:''}"
            :class="item.icon"
          ></i>
          {{ item.name }}</el-tag>
        </el-scrollbar>
      </div>
      <div class="action">
        <el-button @click="closeAll" icon="el-icon-close" size="mini" plain>关闭所有</el-button>
      </div>
    </div>
  </el-header>
</template>

<script>
import { mapState, mapGetters, mapActions } from "vuex";
const homePath = "/admin/home";
export default {
  name: "admin-header",
  data() {
    return {
      login_user: {}
    };
  },
  props: ["collapse"],
  mounted() {
    this.$store.dispatch("global/getLoginUser").then(x => {
      this.login_user = x || {
        userName: "未知用户"
      };
    });
  },
  updated() {
    let scrollbar = this.$refs["scrollbar"];
    scrollbar.update();
  },
  computed: {
    ...mapState({
      open_windows(state) {
        let list = state.global.open_windows.map(x => x);
        let paths = this.$router.history.current.fullPath
          .split("/")
          .filter(x => x);
        if (paths.length > 0) {
          let menus = this.$router.options.routes.find(
            x => x.path == `/${paths[0]}`
          );
          if (menus.children) {
            let home = menus.children.find(x => x.isHome);
            let path = `/${paths[0]}/${home.path}`;
            if (!list.find(x => x.fullpath == path)) {
              list.unshift({
                fullpath: path,
                path: home.path,
                name: home.name,
                icon: home.icon,
                iconColor: home.iconColor,
                isHome: home.isHome,
                closable: home.closable == undefined ? true : home.closable,
                component_name: home.component.name,
                tabHidden: home.tabHidden
              });
              this.$store.commit("global/setOpenWindows", list);
            }
          }
        }
        return list;
      },
      active_window: state => state.global.active_window
    }),
    ...mapGetters("global", ["login_name"])
  },
  methods: {
    ...mapActions("login", ["logout"]),
    collapseChange() {
      this.$emit("on-collapse");
    },
    tag_tab_close(path) {
      let list = this.open_windows.filter(x => x.fullpath != path);
      this.$store.commit("global/setOpenWindows", list);
      if (path == this.active_window) {
        if (list.length == 0) {
          this.$router.push(homePath);
        } else {
          this.$router.push(list[list.length - 1].fullpath);
        }
      }
    },
    go_route(path) {
      this.$router.push(path);
    },
    handleCommand(cmd) {
      switch (cmd) {
        case "logout":
          this.user_logout();
          break;

        default:
          break;
      }
    },
    getHomePath() {
      let home = this.open_windows.find(x => !x.closable);
      return home.fullpath;
    },
    closeAll() {
      let homePath = this.getHomePath();
      let list = this.open_windows.filter(x => !x.closable);
      this.$store.commit("global/setOpenWindows", list);
      this.$router.push(homePath);
    },
    user_logout() {
      this.$confirm(`确定要退出登录吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      }).then(() => {
        this.logout().then(x => {
          if (x.code == 10000) {
            this.$store.commit("global/setMyPowers", null);
            this.$store.commit("global/setOpenWindows", []);
            this.$store.commit("global/setLoginUser", null);
            this.login_name.then(n => {
              if (n) {
                this.$router.push("/login/" + n);
              }
            });
          }
        });
      });
    }
  }
};
</script>

<style lang="scss">
@import "../../element-variables.scss";
.btn-collapse {
  font-size: 1.2em !important;
}
.win-tag {
  margin: 2px 5px;
}
.win-tag:first-child {
  margin-left: 0;
}
.win-tag:last-child {
  margin-right: 0;
}
.top-bar {
  display: flex;
  .menus {
    flex-grow: 1;
    white-space:nowrap;
    overflow: hidden;
  }
  .action {
    padding-left: 10px;
  }
}
</style>
