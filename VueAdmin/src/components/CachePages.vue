<template>
  <transition name="slide">
    <keep-alive :include="cache_pages">
      <router-view></router-view>
    </keep-alive>
  </transition>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "cache-pages",
  computed: {
    ...mapState({
      cache_pages(state) {
        let list = state.global.open_windows.map(x => x.component_name);
        list.push("admin-home");
        return list;
      }
    })
  }
};
</script>

<style lang="scss">
</style>
