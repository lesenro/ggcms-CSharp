<template>
  <el-form :model="d_value" :rules="rules" ref="editForm" v-bind="settings.props">
    <slot name="top"></slot>
    <form-layout
      :key="layout.key"
      :value="d_value"
      :settings="layout"
      v-for="layout in settings.layouts"
      :ref="`l_${layout.key}`"
      @change="onChange"
    ></form-layout>
    <el-form-item v-if="settings.buttons&&!settings.buttons.hidden">
      <el-button type="primary" @click="formSubmit">提交</el-button>
      <el-button @click="resetForm">重置</el-button>
    </el-form-item>
    <slot></slot>
  </el-form>
</template>

<script>
import FormLayout from "./FormLayout.vue";
const formName = "editForm";
export default {
  name: "form-generator",
  props: ["settings", "value"],
  data() {
    return {
      d_value: this.value,
      d_rules: this.settings.rules
    };
  },
  computed: {
    rules() {
      return this.d_rules;
    }
  },
  methods: {
    formSubmit() {
      let value = false;
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$emit("on-submit", this.d_value);
          value = Object.assign({}, this.d_value);
        } else {
          console.error("form error!");
        }
      });
      return value;
    },
    resetForm() {
      this.$refs[formName].resetFields();
    },
    setOptions(key, opts) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        if (layout.setOptions(key, opts)) {
          return true;
        }
      }
      return false;
    },
    setItemProps(key, prop) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        if (layout.setItemProps(key, prop)) {
          return true;
        }
      }
      return false;
    },
    setControlProps(key, prop) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        if (layout.setControlProps(key, prop)) {
          return true;
        }
      }
      return false;
    },
    setAllControlProps(prop) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        layout.setAllControlProps(prop);
      }
      return false;
    },
    setLayoutProps(key, prop) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        if (layout.setLayoutProps(key, prop)) {
          return true;
        }
      }
      return false;
    },
    updateValue(key, val) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        if (layout.updateValue(key, val)) {
          return true;
        }
      }
      return false;
    },
    setValue(key, val) {
      this.d_value[key] = val;
    },
    setValues(val) {
      for (let item in val) {
        this.setValue(item, val[item]);
      }
    },
    onChange(ev) {
      this.$emit("change", ev);
    },
    getControl(key) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        const ctrl = layout.getControl(key);
        if (ctrl) {
          return ctrl;
        }
      }
    },
    getFormItem(key) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        const item = layout.getFormItem(key);
        if (item) {
          return item;
        }
      }
    },
    getValue(key) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        const ctrl = layout.getValue(key);
        if (ctrl) {
          return ctrl;
        }
      }
    },
    setRules(key, rule) {
      for (let x in this.$refs) {
        if (x == formName) {
          continue;
        }
        let layout = this.$refs[x][0];
        layout.setRules(key, rule);
      }
    }
  },
  components: { FormLayout }
};
</script>

<style>
</style>
