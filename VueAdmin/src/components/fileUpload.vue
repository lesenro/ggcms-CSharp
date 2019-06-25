<template>
  <div class="file-upload">
    <el-input readonly :value="fileUrl">
      <el-button slot="append" icon="el-icon-upload2"></el-button>
    </el-input>
  </div>
</template>

<script>
export default {
  name: "file-upload",
  props: ["image"],
  computed: {
    fileUrl() {
      return this.image;
    }
  },
  data() {
    return {};
  },
  methods: {}
};
</script>
<style lang="scss">

</style>