<template>
  <div class="vue-editor">
    <textarea v-model="content" :id="editorId" ref="editor"></textarea>
  </div>
</template>

<script>
import { mapState } from "vuex";
const simpleConfig = { menubar: false, height: 150 };
const fullConfig = {
  plugins:
    "print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount imagetools textpattern help",
  toolbar:
    "formatselect | bold italic strikethrough forecolor backcolor permanentpen | link image media pageembed | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent | removeformat | addcomment",
  image_advtab: true,
  height: 350,
  file_picker_types: "image",
  images_upload_url: "upload",
  automatic_uploads: false
};
export default {
  name: "vue-editor",
  props: ["config", "value", "mode"],
  created() {},
  mounted() {
    if (window["tinymce"]) {
      this.editorInit();
    } else {
      let script = document.createElement("script");
      script.type = "text/javascript";
      script.src = "https://cdn.bootcss.com/tinymce/5.0.12/tinymce.min.js";
      if (script.readyState) {
        script.onreadystatechange = () => {
          if (
            script.readyState == "loaded" ||
            script.readyState == "complete"
          ) {
            script.onreadystatechange = null;
            this.editorInit();
          }
        };
      } else {
        script.onload = () => {
          this.editorInit();
        };
      }
      document.body.appendChild(script);
      // let langs = ["zh-cn"];
      // langs.forEach(l => {
      //   let lang_script = document.createElement("script");
      //   lang_script.type = "text/javascript";
      //   lang_script.src = `https://cdn.bootcss.com/ckeditor/4.12.1/${l}.js`;
      //   document.body.appendChild(lang_script);
      // });
    }
  },
  watch: {
    value(val) {
      let now = new Date();
      let delay = now - this.lastInput;
      if (this.editor && delay > 200) {
        this.setValue(val);
        this.content = val;
      }
    }
  },
  computed: {
    ...mapState("global", ["appcfg"])
  },
  data() {
    let ts = new Date().getTime();
    return {
      editorId: `edit_${ts}`,
      tinymce: null,
      editor: null,
      lastInput: (new Date()).getTime()-5000,
      content: this.value,
      d_config:this.config||{}
    };
  },
  methods: {
    editorInit() {
      let self = this;
      this.tinymce = window["tinymce"];
      let el = this.$refs["editor"];
      let cfg = {
        language_url: this.appcfg.WebRoot + "/assets/js/langs/zh_CN.js",
        language: "zh_CN",
        selector: "#" + this.editorId,
        setup: function(editor) {
          self.editor = editor;
          self.setValue(self.value);
          editor.on("init", function(e) {
            self.$emit("init", e, editor);
          });
        },
        init_instance_callback: function(editor) {
          self.editor = editor;
          editor.on("focus", function(e) {
            self.$emit("focus", e, editor);
          });
          editor.on("blur", function(e) {
            self.$emit("blur", e, editor);
          });
          editor.on("Change", function(e) {
            self.onEditorInput(e.level.content, e);
          });
        },
        images_upload_handler: function(blobInfo, success, failure) {
          self.$emit(
            "imageBeforeUpload",
            [blobInfo.blob()],
            self.editor,
            (url, file) => {
              success(url);
            }
          );
        }
      };
      if (this.mode == "simple") {
        this.tinymce.init(Object.assign({}, cfg, simpleConfig,this.d_config));
      } else {
        this.tinymce.init(Object.assign({}, cfg, fullConfig,this.d_config));
      }

      // this.editor.on("change", this.onEditorInput);

      // this.setValue(this.value);
      // console.log(this.editor);
    },
    setValue(val) {
      this.content = val;
      this.editor.setContent(val);
    },
    onEditorReady(ev) {
      this.editor = ev;
    },
    onEditorInput(ctx, ev) {
      this.content = ctx;
      this.lastInput = new Date();
      this.$emit("change", ctx);
      this.$emit("input", ctx);
    }
  },
  components: {}
};
</script>

<style lang="scss">
div.tox.tox-silver-sink {
  z-index: 5000;
}
</style>
