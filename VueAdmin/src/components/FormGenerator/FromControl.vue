<template>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-if="d_settings.type=='text'">
    <el-input ref="ctrl" @change="onChange" v-bind="controlProps" v-model="value[d_settings.key]"></el-input>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='password'">
    <el-input
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
      show-password
    ></el-input>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='textarea'">
    <el-input
      ref="ctrl"
      @change="onChange"
      type="textarea"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-input>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='datetime-picker'"
  >
    <el-date-picker
      ref="ctrl"
      @change="onChange"
      type="datetime"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-date-picker>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='date-picker'"
  >
    <el-date-picker
      ref="ctrl"
      @change="onChange"
      type="date"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-date-picker>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='time-picker'"
  >
    <el-time-picker
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-time-picker>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='number'">
    <el-input-number
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-input-number>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='switch'">
    <el-switch ref="ctrl" @change="onChange" v-bind="controlProps" v-model="value[d_settings.key]"></el-switch>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='checkbox'">
    <el-checkbox
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    >{{d_settings.name}}</el-checkbox>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='checkbox-group'"
  >
    <el-checkbox-group
      ref="ctrl"
      @change="onChangeArray"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    >
      <el-checkbox
        v-for="item in options||[]"
        :key="item.value"
        :label="item.label"
        :value="item.value"
      ></el-checkbox>
    </el-checkbox-group>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='radio-group'"
  >
    <el-radio-group
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    >
      <el-radio v-for="item in options||[]" :key="item.value" :label="item.value">{{item.label}}</el-radio>
    </el-radio-group>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='select'">
    <el-select ref="ctrl" @change="onChange" v-bind="controlProps" v-model="value[d_settings.key]">
      <el-option
        v-for="item in options||[]"
        :key="item.value"
        :label="item.label"
        :value="item.value"
        v-bind="optionProps"
      ></el-option>
    </el-select>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='slider'">
    <el-slider ref="ctrl" @change="onChange" v-bind="controlProps" v-model="value[d_settings.key]"></el-slider>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='rate'">
    <el-rate ref="ctrl" @change="onChange" v-bind="controlProps" v-model="value[d_settings.key]"></el-rate>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='address'">
    <address-select
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></address-select>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='cascader'">
    <el-cascader
      ref="ctrl"
      @change="onChange"
      :options="options||[]"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    ></el-cascader>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='image-upload'"
  >
    <el-upload ref="ctrl" v-bind="controlProps" :on-success="onFileUploadSuccess">
      <image-upload ref="subCtrl" :image="value[d_settings.key]" />
    </el-upload>
    <el-button
      v-if="itemProps.showClear&&value[d_settings.key]"
      type="danger"
      class="btn-clear"
      icon="el-icon-close"
      @click="value[d_settings.key]=''"
    >清除</el-button>
  </el-form-item>
  <el-form-item
    :prop="d_settings.key"
    v-bind="itemProps"
    v-else-if="d_settings.type=='file-upload'"
  >
    <el-upload ref="ctrl" v-bind="controlProps" :on-success="onFileUploadSuccess">
      <file-upload ref="subCtrl" :image="value[d_settings.key]" />
    </el-upload>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='button'">
    <el-button ref="ctrl" @click="onChange" v-bind="controlProps">{{d_settings.name}}</el-button>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='message'">
    <el-alert ref="ctrl" v-bind="controlProps"></el-alert>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='editor'">
    <tinymce
      @editorInit="editorInit"
      :style="{'margin-right':'4px'}"
      :other_options="editorOptions"
      :id="d_settings.key"
      ref="ctrl"
      v-model="value[d_settings.key]"
      @editorChange="onEditorChange"
      v-bind="controlProps"
    ></tinymce>
  </el-form-item>
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='code'">
    <codemirror
      :name="'code_'+d_settings.key"
      v-model="value[d_settings.key]"
      v-bind="controlProps"
      @input="onChange"
    ></codemirror>
  </el-form-item>
  <sub-component
    v-bind="itemProps"
    :prop="d_settings.key"
    ref="ctrl"
    v-else-if="d_settings.type=='component'"
  />
  <el-form-item :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='position'">
    <get-positon
      ref="ctrl"
      @change="onChange"
      :value="value[d_settings.key]"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    />
  </el-form-item>
  <div :prop="d_settings.key" v-bind="itemProps" v-else-if="d_settings.type=='article'">
    <article-content
      ref="ctrl"
      @change="onChange"
      v-bind="controlProps"
      v-model="value[d_settings.key]"
    />
  </div>
</template>

<script>
// language js
// import "codemirror/mode/javascript/javascript.js";
// import "codemirror/mode/xml/xml.js";
// theme css
import { mapState, mapActions } from "vuex";
import articleContent from "@/components/articleContent";
import imageUpload from "@/components/imageUpload";
import fileUpload from "@/components/fileUpload";

export default {
  name: "form-control",
  created() {
    if (this.settings.component) {
      this.$options.components[this.vueName] = this.settings.component;
    }
  },
  updated() {
    if (this.d_settings.type == "editor") {
      // this.value[this.d_settings.key] = this.d_val;
    }
  },
  props: ["settings", "value"],
  data() {
    let self = this;
    return {
      vueName: "sub-component",
      d_settings: this.settings,
      d_options: (this.settings.options || []).map(x => x),
      d_itemProps: this.settings.itemProps || {},
      d_controlProps: this.settings.controlProps || {},
      editorOptions:Object.assign({},{
        selector: "#"+this.settings.key,
        file_browser_callback_types: "image",
        // file_browser_callback_types: "file image media",
        language_url: "/assets/js/zh_CN.js",
        plugins:['advlist autolink lists link image charmap print preview hr anchor pagebreak', 'searchreplace visualblocks visualchars code fullscreen', 'insertdatetime media nonbreaking save table contextmenu directionality','template paste textcolor colorpicker textpattern imagetools toc help emoticons hr codesample'],
        file_picker_callback: function(callback, value, meta) {
          // Provide image and alt text for the image dialog
          // if (meta.filetype == "image") {
          //   callback("myimage.jpg", { alt: "My alt text" });
          // }
          let ctrl= self.$refs["ctrl"];

          if (meta.filetype == "image") {
            let file=document.createElement("input");
            file.type="file"
            file.onchange=(ev)=>{
              if(ctrl.imageAdded){
                ctrl.imageAdded(file.files[0],callback);
              }
            };
            file.click();
            // callback("myimage.jpg", { alt: "My alt text" });
          }
        }
      }, (this.settings.controlProps||{}).configs)
    };
  },
  computed: {
    ...mapState("global", ["appcfg"]),
    options() {
      return this.d_options;
    },
    optionProps() {
      return this.settings.optionProps || {};
    },
    itemProps() {
      let itemProps = this.d_itemProps;
      itemProps.label = itemProps.label || this.settings.name;
      if (itemProps.labelHidden) {
        itemProps.label = "";
        itemProps["label-width"] = "0";
      }
      return itemProps;
    },
    controlProps() {
      return this.d_controlProps;
    }
  },
  methods: {
    setOptions(key, opts) {
      if (key != this.settings.key) {
        return false;
      }
      this.d_options = opts;
      return true;
    },
    setItemProps(key, prop) {
      if (key != this.settings.key) {
        return false;
      }
      this.d_itemProps = Object.assign({}, this.d_itemProps, prop);
      return true;
    },
    setControlProps(key, prop) {
      if (key != this.settings.key) {
        return false;
      }
      this.d_controlProps = Object.assign({}, this.d_controlProps, prop);
      if (["number", "select"].indexOf(this.d_settings.type) != -1) {
        this.d_controlProps.disabled = prop.readonly;
      }
      return true;
    },
    updateValue(key, val) {
      if (key != this.settings.key) {
        return false;
      }
      this.$set(this.value, key, val);
      this.$forceUpdate();
      return true;
    },
    onChange(ev) {
      if (this.d_settings.type == "button") {
        ev.preventDefault();
      }
      this.$emit("change", {
        key: this.d_settings.key,
        value: ev
      });
    },
    editorInit(ev) {
      this.originalEditor=ev;
      this.value[this.d_settings.key]=this.value[this.d_settings.key]+" ";
    },
    onEditorChange(ev) {
      this.$emit("change", {
        key: this.d_settings.key,
        value: this.value[this.d_settings.key],
        event: ev
      });
    },
    onChangeArray(ev) {
      let list = this.options
        .filter(x => ev.find(l => l == x.label))
        .map(x => x.value);
      this.$emit("change", {
        key: this.d_settings.key,
        value: list
      });
    },
    onFileUploadSuccess(ev) {},
    getControl() {
      return this.$refs["ctrl"];
    },
    getValue() {
      return this.value[this.d_settings.key];
    },
    setRules(key, rule) {
      if (key != this.settings.key) {
        return false;
      }
      let rules = this.d_itemProps.rules || [];
      rule.forEach(r => {
        let item = rules.find(x => x.name == r.name);
        if (item) {
          for (let n in r) {
            item[n] = r[n];
          }
        }
      });
      this.d_itemProps.rules = rules;
    }
  },
  components: { articleContent, imageUpload, fileUpload }
};
</script>
<style lang="scss">
.btn-clear {
  width: 130px;
}
</style>
