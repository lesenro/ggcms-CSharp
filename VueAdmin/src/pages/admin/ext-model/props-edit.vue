<template>
  <div class="props-edit">
    <el-row :gutter="15">
      <el-col :span="12">
        <form-generator
          @change="onPropsFormChange"
          :value="prop_value"
          ref="props-form"
          :settings="propsForm"
        ></form-generator>
        <el-divider>操作</el-divider>
        <div class="float-right">
          <el-button type="primary" @click="propsAdd" size="mini">添加</el-button>
          <el-button type="danger" @click="codeClear" size="mini">清空</el-button>
        </div>
      </el-col>
      <el-col :span="12">
        <codemirror name="prop_code" @input="codeInput" v-model="prop_code"></codemirror>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import {
  propsOptions,
  propsValue,
  propsForm
} from "./form_settings";
export default {
  name: "props-edit",
  data() {
    return {
      propsForm: new propsForm(),
      prop_value: new propsValue(),
      prop_code: "{}"
    };
  },
  computed: {
  },
  methods: {
    setValues(code) {
      this.prop_code = code;
      this.onPropsFormChange({
        key: "propType",
        value: "size"
      });
    },
    codeInput(ev){
      this.$emit("change", this.prop_code);
    },
    codeClear(){
      this.prop_code="{}";
      this.$emit("change", this.prop_code);
    },
    onPropsFormChange(ev) {
      if (ev.key == "propType") {
        let all = propsOptions.find(x => x.value == "all");
        let item = propsOptions.find(x => x.value == ev.value);
        let form = this.$refs["props-form"];
        all.hidden.forEach(x => {
          form.setItemProps(x, { class: false });
        });
        item.hidden.forEach(x => {
          form.setItemProps(x, { class: "hidden" });
        });
      }
    },
    propsAdd() {
      let props = JSON.parse(this.prop_code);
      let form = this.$refs["props-form"];
      let vals = form.formSubmit();
      let item = propsOptions.find(x => x.value == vals.propType);
      let p = {};
      switch (vals.propType) {
        case "size":
          p.size = vals.size;
          break;
        case "label-width":
          p["label-width"] = vals["label-width"];
          break;
        case "placeholder":
          p.placeholder = vals.value;
          break;
        case "lenRange":
          p.minlength = vals.min;
          p.maxlength = vals.max;
          break;
        case "range":
          p.min = vals.min;
          p.max = vals.max;
          break;
        case "minlength":
          p.minlength = vals.min;
          break;
        case "maxlength":
          p.maxlength = vals.max;
          break;
        case "min":
          p.min = vals.min;
          break;
        case "max":
          p.max = vals.max;
          break;
      }
      if (item.itemType == "item") {
        props.itemProps = Object.assign({}, props.itemProps, p);
      } else if (item.itemType == "control") {
        props.controlProps = Object.assign({}, props.controlProps, p);
      }
      this.prop_code = JSON.stringify(props, null, "\t");
      this.$emit("change", this.prop_code);
    },
  }
};
</script>

<style lang="scss">
</style>
