<template>
  <el-container class="root">
    <div :class="{ collapse: isCollapse}" class="side-menu-container">
      <div class="side-logo">
        <el-button
          size="medium"
          v-if="!isCollapse"
          type="text"
          @click="collapseChange"
          border-none
          class="btn-collapse hidden-sm-and-up"
          plain
        >
          <i class="el-icon-s-fold"></i>
        </el-button>

        <img alt="logo" :src="'./assets/images/logo.png'">
      </div>
      <el-scrollbar class="layout-scrollbar">
        <side-menu :collapse="isCollapse" ref="sideMenu"/>
      </el-scrollbar>
    </div>
    <div :class="{ collapse: isCollapse}" class="main-container">
      <admin-header :collapse="isCollapse" @on-collapse="collapseChange"/>
      <el-container>
        <transition name="slide">
          <el-main class="layout-main">
            <keep-alive :include="cache_pages">
              <router-view></router-view>
            </keep-alive>
          </el-main>
        </transition>
      </el-container>
    </div>
  </el-container>
</template>

<script>
import adminHeader from "./header";
import sideMenu from "./sidemenu";
import { mapState, mapGetters, mapActions } from "vuex";

export default {
  mounted() {
    // let smenu = this.$refs["sideMenu"];
    // smenu.menusInit();
  },
  data() {
    return {
      collapse: false,

      editableTabsValue: "2",
      editableTabs: [
        {
          title: "Tab 1",
          name: "1",
          content: "Tab 1 content"
        },
        {
          title: "Tab 2",
          name: "2",
          content: "Tab 2 content"
        }
      ],
      tabIndex: 2
    };
  },

  created: function() {
    if (this.screenSize == "xs") {
      this.setCollapse(true);
    }
    this.$store.commit("global/getLoginUser");
    this.getPowers();
  },
  computed: {
    isCollapse: function() {
      return this.collapse;
    },
    ...mapGetters("global", ["screenSize"]),
    ...mapState("global", ["login_user"]),
    ...mapState({
      cache_pages(state) {
        let list = state.global.open_windows.map(x => x.component_name);
        list.push("admin-home");
        return list;
      }
    })
  },
  methods: {
    ...mapActions("login", ["getPowers"]),
    collapseChange() {
      this.collapse = !this.collapse;
    },
    setCollapse(c) {
      this.collapse = c;
    }
  },
  components: { adminHeader, sideMenu }
};
</script>

<style lang="scss">
@import "../../element-variables.scss";

.el-header {
  background-color: $--header-background-color;
  color: $--header-color;
  line-height: $--header-height / 2;
  position: fixed;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  right: 0;
  left: $aside-width;
  top: 0;
  z-index: 100;
}

.side-menu-container {
  display: flex;
  flex-direction: column;
  white-space: nowrap;
  max-height: 100vh;
  flex: 0 0 $aside-width;
  max-width: $aside-width;
  min-width: $aside-width;
  width: $aside-width;
  z-index: 101;
  box-shadow: 2px 0 6px rgba(0, 21, 41, 0.1);
  transition: all 0.2s;
  background-color: $side-menu-background;
  .layout-scrollbar {
    height: 100%;
  }
  .el-menu {
    border-right: 0px transparent;
    background-color: transparent;
  }
  .el-menu-item.is-active {
    background-color: $--color-primary !important;
  }
  .side-logo {
    padding: 15px 20px;
    .btn-collapse {
      float: right;
      font-size: 1.2em;
      margin-top: -2px;
      color: #ccc;
    }
    img {
      height: 36px;
    }
  }
  .el-scrollbar__wrap {
    overflow-x: hidden !important;
  }
}
.side-menu-container.collapse {
  flex: 0 0 $aside-collapse-width;
  max-width: $aside-collapse-width;
  min-width: $aside-collapse-width;
  width: $aside-collapse-width;
  background-color: $side-menu-background;
  overflow: hidden;
  .side-logo {
    img {
      height: 26px;
    }
  }
}

.main-container {
  max-height: 100vh;
  overflow-x: hidden;
  flex: auto;
  flex-direction: column;
  min-height: 0;
  background: #f0f0f0;
  .el-container {
    height: 100vh;
    .layout-main {
      padding: 15px;
      margin-top: $--header-height;
      width: 100%;
      display: flex;
      flex-direction: column;
    }
  }
}
.main-container.collapse {
  .el-header {
    left: $aside-collapse-width;
  }
}

@media only screen and (max-width: $--sm - 1) {
  .side-menu-container {
    position: fixed !important;
    left: 0;
    top: 0;
    bottom: 0;
    z-index: 200;
  }
  .side-menu-container.collapse {
    width: 0;
    min-width: 0;
  }
  .el-header {
    left: 0 !important;
  }
  .el-main {
    .el-col {
      margin-bottom: 15px;
    }
  }
}
</style>
