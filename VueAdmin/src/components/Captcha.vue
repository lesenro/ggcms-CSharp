<template>
  <div class="captcha">
    <el-tooltip content="看不清? 点击换一张" placement="top">
      <img alt="captcha code" @click="codeChange" :src="code">
    </el-tooltip>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "captcha",
  mounted() {
    this.$store.commit("login/getCodeImg");
  },
  computed: {
    ...mapState("login", ["codeImgUrl"]),
    code() {
      return (
        this.codeImgUrl ||""
      );
    }
  },
  data() {
    let self = this;
    return {};
  },
  methods: {
    codeChange(ev) {
      this.$store.commit("login/getCodeImg");
    }
  },
  components: {}
};
</script>

<style>
.captcha {
  padding-top: 7px;
}
.captcha img {
  max-width: 100%;
}
</style>
