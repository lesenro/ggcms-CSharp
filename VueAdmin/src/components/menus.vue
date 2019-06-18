<template>
  <transition>
    <template v-if="item.children">
      <el-submenu :index="item.fullpath||item.path">
        <template slot="title">
          <i :class="item.icon" :style="{color:item.iconColor||''}"></i>

          <span>{{item.name}}</span>
        </template>
        <route-menu :key="r.fullpath" v-for="r in item.children.filter(m=>!m.sideHidden)" :item="r" @menu-click="menuClick"/>
      </el-submenu>
    </template>
    <template v-if="!item.children">
      <el-menu-item :index="item.fullpath" @click="menuClick(item.fullpath)">
        <i :class="item.icon" :style="{color:item.iconColor||''}"></i>
        <span>{{item.name}}</span>
      </el-menu-item>
    </template>
  </transition>
</template>
<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "route-menu",
  props: ["item"],
  computed: {
    ...mapState({
      open_windows(state) {
        let list = state.global.open_windows.map(x => x);
        return list;
      },
      active_window: state => state.global.active_window
    })
  },
  methods: {
    menuClick(path) {
      this.$emit("menu-click", path);
    }
  }
};
</script>

<style lang="scss">
@import "../element-variables.scss";
.el-icon-none {
  display: inline-block;
  text-align: center;
  transform: translateY(-1px);
}
.mg-r-5 {
  margin-right: 5px;
}
</style>
