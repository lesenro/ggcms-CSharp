<template>
  <div class="form-layout" v-bind="layoutProps" v-if="settings.type=='div'">
    <slot name="top"></slot>
    <transition-group v-if="layouts&&layouts.length>0">
      <form-layout
        :value="value"
        :key="layout.key"
        :settings="layout"
        v-for="layout in layouts"
        :ref="`l_${layout.key}`"
      ></form-layout>
    </transition-group>
    <transition-group v-if="settings.controls&&settings.controls.length>0">
      <form-control
        :value="value"
        :key="control.key"
        :settings="control"
        v-for="control in settings.controls"
        :ref="`c_${control.key}`"
        @change="onChange"
      ></form-control>
    </transition-group>
    <slot></slot>
  </div>
  <el-card class="form-layout" v-bind="layoutProps" v-else-if="settings.type=='card'">
    <slot name="top"></slot>
    <transition-group v-if="layouts&&layouts.length>0">
      <form-layout
        :value="value"
        :key="layout.key"
        :settings="layout"
        v-for="layout in layouts"
        :ref="`l_${layout.key}`"
      ></form-layout>
    </transition-group>
    <transition-group v-if="settings.controls&&settings.controls.length>0">
      <form-control
        :value="value"
        :key="control.key"
        :settings="control"
        v-for="control in settings.controls"
        :ref="`c_${control.key}`"
        @change="onChange"
      ></form-control>
    </transition-group>
    <slot></slot>
  </el-card>
  <el-tabs class="form-layout" v-bind="layoutProps" v-else-if="settings.type=='tabs'">
    <el-tab-pane
      v-bind="layout.props"
      :key="layout.key"
      :settings="layout"
      v-for="layout in layouts"
      class="form-layout"
      :name="layout.key"
      :label="layout.name"
      :ref="`s_${layout.key}`"
    >
      <slot name="top"></slot>
      <transition-group v-if="layout.layouts&&layout.layouts.length>0">
        <form-layout
          :key="sub_layout.key"
          :settings="sub_layout"
          v-for="sub_layout in layout.layouts"
          :value="value"
          :ref="`l_${layout.key}`"
        ></form-layout>
      </transition-group>
      <transition-group v-if="layout.controls&&layout.controls.length>0">
        <form-control
          :value="value"
          :key="control.key"
          :settings="control"
          v-for="control in layout.controls"
          :ref="`c_${control.key}`"
          @change="onChange"
        ></form-control>
      </transition-group>
      <slot></slot>
    </el-tab-pane>
  </el-tabs>
  <el-row class="form-layout" v-bind="layoutProps" v-else-if="settings.type=='row'">
    <el-col
      v-bind="layout.props"
      :key="layout.key"
      :settings="layout"
      v-for="layout in layouts"
      class="form-layout"
      :name="layout.key"
      :label="layout.name"
      :ref="`s_${layout.key}`"
    >
      <slot name="top"></slot>
      <transition-group v-if="layout.layouts&&layout.layouts.length>0">
        <form-layout
          :key="sub_layout.key"
          :settings="sub_layout"
          v-for="sub_layout in layout.layouts"
          :value="value"
          :ref="`l_${layout.key}`"
        ></form-layout>
      </transition-group>
      <transition-group v-if="layout.controls&&layout.controls.length>0">
        <form-control
          :value="value"
          :key="control.key"
          :settings="control"
          v-for="control in layout.controls"
          :ref="`c_${control.key}`"
          @change="onChange"
        ></form-control>
      </transition-group>
      <slot></slot>
    </el-col>
  </el-row>
</template>

<script>
import FormControl from "./FromControl.vue";
export default {
  name: "form-layout",

  props: ["settings", "value"],
  data() {
    return {
      d_layoutProps: this.settings.props,
      d_layouts: this.settings.layouts
    };
  },
  computed: {
    layoutProps() {
      return this.d_layoutProps;
    },
    layouts() {
      return this.d_layouts;
    }
  },
  methods: {
    setOptions(key, opts) {
      for (let x in this.$refs) {
        if (x.startsWith("s_")) {
          continue;
        }
        let item = this.$refs[x][0];
        if (item.setOptions(key, opts)) {
          return true;
        }
      }
      return false;
    },
    setItemProps(key, prop) {
      for (let x in this.$refs) {
        let item = this.$refs[x][0];
        if (item.setItemProps(key, prop)) {
          return true;
        }
      }
      return false;
    },
    setControlProps(key, prop) {
      for (let x in this.$refs) {
        let item = this.$refs[x][0];
        if (item.setControlProps(key, prop)) {
          return true;
        }
      }
      return false;
    },
    setAllControlProps(prop) {
      for (let x in this.$refs) {
        if (x.startsWith("s_")) {
          continue;
        }
        let item = this.$refs[x][0];
        if (x.startsWith("c_")) {
          item.setControlProps(x.substr(2), prop);
        } else {
          console.log(x);
          item.setAllControlProps(prop);
        }
      }
      return false;
    },
    setLayoutProps(key, prop) {
      if (key == this.settings.key) {
        this.d_layoutProps = Object.assign({}, this.d_layoutProps, prop);
        return true;
      }
      for (let x in this.$refs) {
        if (x.startsWith("l_")) {
          let item = this.$refs[x][0];
          if (item.setLayoutProps(key, prop)) {
            return true;
          }
        }
      }
      if (["tabs", "row"].indexOf(this.settings.type) != -1) {
        for (let x in this.d_layouts) {
          let layout = this.d_layouts[x];
          if (layout.key == key) {
            let props = Object.assign({}, layout.props, prop);
            this.$set(layout, "props", props);
            return true;
          }
        }
      }
      return false;
    },
    onChange(ev) {
      this.$emit("change", ev);
    },
    getControl(key) {
      for (let x in this.$refs) {
        if (x.startsWith("l_") || x == "c_" + key) {
          let item = this.$refs[x][0];
          let ctrlItem = item.getControl(key);
          if (ctrlItem) {
            return ctrlItem;
          }
        }
      }
    },
    getFormItem(key) {
      for (let x in this.$refs) {
        if (x.startsWith("l_")) {
          let item = this.$refs[x][0];
          let ctrlItem = item.getFormItem(key);
          if (ctrlItem) {
            return ctrlItem;
          }
        } else if (x == "c_" + key) {
          let item = this.$refs[x][0];
          return item;
        }
      }
    },
    getValue(key) {
      for (let x in this.$refs) {
        if (x.startsWith("l_") || x == "c_" + key) {
          let item = this.$refs[x][0];
          let ctrlItem = item.getValue(key);
          if (ctrlItem) {
            return ctrlItem;
          }
        }
      }
    },
    setRules(key, rule) {
      for (let x in this.$refs) {
        if (x.startsWith("l_") || x == "c_" + key) {
          let item = this.$refs[x][0];
          if (item.setRules(key, rule)) {
            return true;
          }
        }
      }
      return false;
    }
  },
  components: { FormControl }
};
</script>

<style>
</style>
