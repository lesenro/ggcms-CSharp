<template>
  <div class="model-input">
    <div class="model-body">
      <form-generator :value="value" @change="onFormCtrlChange" ref="form" :settings="formSettings"></form-generator>
    </div>
    <div slot="footer" class="dialog-footer margin-top text-right">
      <el-button @click="onCancel">取 消</el-button>
      <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
    </div>
  </div>
</template>

<script>
export default {
  name: "model-input",
  data() {
    return {
      formSettings: {},
      value: {}
    };
  },
  computed: {},

  methods: {
    setValues(modelInfo) {
      let ctrls = [];
      let val = {};
      modelInfo.Columns.orderBy(x => x.OrderId).forEach(x => {
        let item = JSON.parse(x.Options);
        item.key = x.ColName;
        item.name = x.ColTitle;
        val[item.key] = x.Value || "";
        this.$set(this.value, item.key, x.Value || "");
        ctrls.push(item);
      });
      let sets = {
        props: {
          "label-width": "150px"
        },
        buttons: {
          hidden: true
        },
        layouts: [
          {
            key: "div",
            name: "div",
            type: "div",
            controls: ctrls
          }
        ]
      };
      this.$set(this, "formSettings", sets);

      let form = this.$refs["form"];
      form.resetForm();
      form.setValues(val);
    },
    onCancel() {
      this.$emit("onCancel", true);
    },

    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      this.$emit("onSubmit", vals);
    },

    //表单项改动事件
    onFormCtrlChange(ev) {}
  }
};
</script>

<style lang="scss">
</style>
