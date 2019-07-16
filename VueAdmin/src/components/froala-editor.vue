<template>
  <div id="froala-editor" class="froala-editor"></div>
</template>

<script>
import FroalaEditor from "froala-editor/js/froala_editor.pkgd.min.js";

export default {
  name: "froala-editor",
  props: ["config", "value"],
  mounted() {
    const self = this;
    const default_config = {
      language: "zh_cn",
      heightMax: 300,
      pluginsEnabled: [
        "align",
        // "charCounter",
        "codeBeautifier",
        "codeView",
        "colors",
        "draggable",
        "embedly",
        // "emoticons",
        "entities",
        // "file",
        "fontAwesome",
        "fontFamily",
        "fontSize",
        "fullscreen",
        "image",
        "imageTUI",
        // "imageManager",
        "inlineStyle",
        "inlineClass",
        "lineBreaker",
        "lineHeight",
        "link",
        "lists",
        "paragraphFormat",
        "paragraphStyle",
        "quickInsert",
        "quote",
        // "save",
        "table",
        "url",
        // "video",
        "wordPaste"
      ],
      events: {
        initialized: function() {
          self.$emit("initialized", this);
        },
        "image.beforeUpload": function(files) {
          // Do something here.
          // this is the editor instance.
          self.$emit("imageBeforeUpload", files, this, self.imageBeforeUpload);
          return false;
        },
        "image.inserted": function($img, response) {
          // Do something here.
          // this is the editor instance.
          if($img.length>0){
            console.log($img[0]);
            $img[0].style.width="";
            $img[0].style.height="";
          }
          this.image.hideProgressBar();
        },
        contentChanged: function() {
          self.$emit("input", this.html.get(true));
          self.$emit("changed", this.html.get(true), this);
        }
      }
    };
    const config = Object.assign({}, default_config, this.config || {});
    this.editor = new FroalaEditor("#froala-editor", config);
    if (this.d_val) {
      this.editor.html.set(this.d_val);
    }
  },

  watch: {
    value(v) {
      this.setValue(v);
    }
  },
  computed: {},
  data() {
    let self = this;
    return {
      editor: null,
      d_val: ""
    };
  },
  methods: {
    imageBeforeUpload(link, file) {
      this.editor.image.insert(link, true, {
        name: file.name
      });
      window.editor = this.editor;
    },
    setValue(val) {
      this.d_val = val;
      this.editor.html.set(this.d_val);
    }
  },
  components: {}
};
</script>

<style lang="scss">
</style>
