<template>
  <div class="image-upload">
    <div class="avatar-uploader">
      <img v-if="imageUrl" :src="imageUrl" class="avatar">
      <i v-else class="el-icon-plus avatar-uploader-icon"></i>
    </div>
  </div>
</template>

<script>
export default {
  name: "image-upload",
  props: ["image"],
  computed: {
    imageUrl() {
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
.image-upload {
  .avatar-uploader {
    border: 1px dashed #d9d9d9;
    background-color: #eeeeee;
    border-radius: 6px;
    width: 128px;
    height: 128px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .avatar-uploader:hover {
    border-color: #409eff;
  }
  .avatar-uploader-icon {
    font-size: 28px;
    color: #8c939d;
    line-height: 128px;
    text-align: center;
  }
  .avatar-uploader-icon:hover {
    color: #409eff;
  }
  .avatar {
    max-width: 100%;
    max-height: 100%;
    display: block;
    margin-top: 50%;
    transform: translateY(-50%);
  }
}
</style>