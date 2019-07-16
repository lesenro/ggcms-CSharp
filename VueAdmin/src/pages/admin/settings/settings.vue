<template>
  <div class="page-panel" v-loading="loading">
    <div class="header-bar">
      <el-button-group>
        <el-button
          icon="el-icon-document-checked"
          size="mini"
          type="primary"
          @click="onInfoSubmit"
        >保存</el-button>
      </el-button-group>
    </div>

    <form-generator
      v-if="value"
      :value="value"
      @change="onFormCtrlChange"
      ref="form"
      :settings="formSettings"
    ></form-generator>
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
  name: "settings",
  data() {
    return {
      formSettings: {},
      uploadTypes: [],
      value: null,
      styles: [],
      configs: [],
      files: []
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  async created() {
    let styles = await this.getStyles();
    this.styles = styles.Records.map(s => {
      return {
        value: s.Folder,
        label: s.StyleName,
        id: s.Id
      };
    });
    let uploadTypes = await this.getList({
      QueryString: 'GroupKey=="upload_type"'
    });
    this.uploadTypes = uploadTypes.Records.map(x => {
      return {
        label: x.DictName,
        value: x.DictKey
      };
    });
    this.dataLoad();
  },

  methods: {
    ...mapActions("dict", ["getList", "settingsSave"]),
    ...mapActions("global", ["fileUpload"]),
    ...mapActions("styles", {
      getStyles: "getList",
      getTmplList: "getTmplList"
    }),

    styleChange(folderName) {
      let s = this.styles.find(x => x.value == folderName);
      if (s) {
        this.getTmplList(s.id).then(t => {
          if (t.Code == 0) {
            let files = t.Data.files;
            let form = this.$refs["form"];
            form.setOptions(
              "cfg_template_home",
              files
                .where(f => f.name.startsWith("index_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
            form.setOptions(
              "cfg_template_list",
              files
                .where(f => f.name.startsWith("list_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
            form.setOptions(
              "cfg_template_view",
              files
                .where(f => f.name.startsWith("view_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
            form.setOptions(
              "cfg_template_m_home",
              files
                .where(f => f.name.startsWith("m_index_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
            form.setOptions(
              "cfg_template_m_list",
              files
                .where(f => f.name.startsWith("m_list_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
            form.setOptions(
              "cfg_template_m_view",
              files
                .where(f => f.name.startsWith("m_view_"))
                .map(f => {
                  return {
                    label: f.name,
                    value: f.name
                  };
                })
            );
          }
        });
      }
    },
    dataLoad() {
      let pageInfo = Object.assign(
        {},
        this.$store.state.global.defaultPageInfo
      );
      pageInfo.QueryString = 'DictType==0 and GroupKey=="system_configs"';
      pageInfo.PageSize = 0;
      this.getList(pageInfo).then(result => {
        let list = result.Records;
        this.configs = list;
        let settings = {
          props: {
            "label-width": "350px"
          },
          buttons: {
            hidden: true
          },
          layouts: [
            {
              key: "div",
              name: "div",
              type: "tabs",
              props: {
                value: "tab-1"
              },
              layouts: [
                {
                  key: "tab-1",
                  name: "基本设置",
                  type: "tab",
                  controls: this.getCtrls(list, "cfg_base")
                },
                {
                  key: "tab-2",
                  name: "界面设置",
                  type: "tab",
                  controls: this.getCtrls(list, "cfg_ui")
                },
                {
                  key: "tab-3",
                  name: "内容相关",
                  type: "tab",
                  controls: this.getCtrls(list, "cfg_content")
                },
                {
                  key: "tab-4",
                  name: "其他设置",
                  type: "tab",
                  controls: this.getCtrls(list, "cfg_other")
                }
              ]
            }
          ]
        };
        this.$set(this, "formSettings", settings);
        let val = {};
        list.forEach(item => {
          val[item.DictKey] = item.DictValue;
          if (item.DictKey == "cfg_mob_enable") {
            val[item.DictKey] = item.DictValue.toLowerCase() == "true";
          }
          if (item.DictKey == "cfg_artkey_enable") {
            val[item.DictKey] = item.DictValue.toLowerCase() == "true";
          }
          if (item.DictKey == "cfg_default_style") {
            this.styleChange(item.DictValue);
          }
        });
        this.$set(this, "value", val);
        setTimeout(() => {
          const form = this.$refs["form"];
          let ctrl = form.getControl("cfg_powerby");
          ctrl.setValue(val.cfg_powerby);
        }, 200);
      });
    },
    getCtrls(list, pkey) {
      return list
        .where(x => x.ParentKey == pkey)
        .orderBy(x => x.OrderId)
        .map(x => {
          try {
            let item = JSON.parse(x.OtherProperty);
            item.key = x.DictKey;
            item.name = `${x.DictName}(${x.DictKey})`;
            if (item.key == "cfg_logo") {
              if (!item.controlProps) {
                item.controlProps = {};
              }
              item.controlProps.action = "/";
              item.controlProps.httpRequest = ev =>
                this.onFileSelect(ev, item.key);
            }
            if (item.key == "cfg_default_style") {
              item.options = this.styles;
            }
            if (item.key == "cfg_uploadmode") {
              item.options = this.uploadTypes;
            }
            if (item.key == "cfg_powerby") {
              item.controlProps = {
                config: {
                  height: 100,
                  toolbarButtons: {
                    moreText: {
                      buttons: [
                        "bold",
                        "italic",
                        "underline",
                        "strikeThrough",
                        "subscript",
                        "superscript",
                        "fontFamily",
                        "fontSize",
                        "textColor",
                        "backgroundColor",
                        "inlineClass",
                        "inlineStyle",
                        "clearFormatting"
                      ]
                    },
                    moreParagraph: {
                      buttons: [
                        "alignLeft",
                        "alignCenter",
                        "alignRight",
                        "alignJustify",
                        "formatOLSimple"
                      ]
                    },
                    moreRich: {
                      buttons: ["insertLink", "insertHR"]
                    },
                    moreMisc: {
                      buttons: ["undo", "redo", "selectAll", "html"],
                      align: "right",
                      buttonsVisible: 2
                    }
                  }
                }
              };
            }
            return item;
          } catch (ex) {
            return null;
          }
        });
    },
    onFileSelect(ev, key) {
      const form = this.$refs["form"];
      let ctrl = form.getControl(key);
      if (ev.file) {
        if (!ev.file.type.startsWith("image")) {
          this.$message({
            type: "error",
            message: "必须上传图片"
          });
          ctrl.clearFiles();
          return;
        }
        this.fileUpload({
          type: "config",
          file: ev.file
        }).then(x => {
          let file = this.files.find(f => f.propertyName == key);
          if (file) {
            file.filePath = x.Data[0].url;
            file.propertyName = key;
            file.fileType = 3;
          } else {
            this.files.push({
              filePath: x.Data[0].url,
              propertyName: key,
              fileType: 3
            });
          }
          ctrl.clearFiles();
          form.setValue(key, x.link);
        });
      }
      //
    },
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      let cfgs = [];
      this.configs.forEach(x => {
        cfgs.push({
          Id: x.Id,
          DictKey: x.DictKey,
          DictValue: vals[x.DictKey]
        });
      });
      this.settingsSave({
        list: cfgs,
        files: this.files
      }).then(x => {
        if (x.length > 0) {
          this.$message({
            type: "success",
            message: "保存成功"
          });
          this.dataLoad();
        }
      });
    },

    //表单项改动事件
    onFormCtrlChange(ev) {
      if (ev.key == "cfg_default_style") {
        this.styleChange(ev.value);
      }
    }
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
