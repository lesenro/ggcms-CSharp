<template>
  <div class="page-panel" v-loading="loading" :style="{flex:'1 0'}">
    <div class="toolbar">
      <el-button-group>
        <el-button size="mini" type="primary" icon="el-icon-plus" @click="handleAdd">添加</el-button>
        <el-button size="mini" type="primary" icon="el-icon-sort" @click="handleSortSave">保存排序</el-button>
        <el-button size="mini" type="success" icon="el-icon-refresh" @click="refresh">刷新</el-button>
      </el-button-group>
    </div>
    <el-scrollbar class="tree-scrollbar" ref="scrollbar">
      <div class="tree-view" padding-vertical>
        <el-tree
          :data="treeData"
          :props="props"
          default-expand-all
          node-key="Id"
          :expand-on-click-node="false"
          ref="treeView"
          draggable
        >
          <div class="tree-node" slot-scope="{ node, data }">
            <span>{{ node.label }}</span>
            <span class="right">
              <el-button-group class="btns" float-right>
                <el-button
                  size="mini"
                  type="primary"
                  icon="el-icon-plus"
                  @click="handleAdd($event,data)"
                >添加下级</el-button>
                <el-button
                  size="mini"
                  type="success"
                  icon="el-icon-edit"
                  @click="handleEdit(data)"
                >编辑</el-button>
                <el-button
                  v-if="node.isLeaf"
                  size="mini"
                  type="danger"
                  icon="el-icon-close"
                  @click="handleDelete(data)"
                >删除</el-button>
              </el-button-group>
            </span>
          </div>
        </el-tree>
      </div>
    </el-scrollbar>

    <el-dialog
      :title="'分类导航 : '+dataInfo.CategoryName||'添加'"
      :visible.sync="editFormVisble"
      @open="editFormOpen"
      width="75%"
    >
      <form-generator :value="value" @change="onChange" ref="form" :settings="formSettings"></form-generator>
      <div slot="footer" class="dialog-footer">
        <el-button @click="editFormVisble = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import formSettings, { defaultValue } from "./form_settings";
import { mapActions, mapState } from "vuex";
let orderid = 0;
export default {
  name: "category-list",
  data() {
    return {
      formSettings: {},
      editFormVisble: false,
      dataInfo: {},
      value: Object.assign({}, defaultValue),
      treeData: [],
      data_list: [],
      files: [],
      styles: [],
      model_list: [],
      props: {
        label: "CategoryName"
      }
    };
  },

  computed: {
    ...mapState("category", ["loading"])
  },
  created() {
    this.getStyles().then(x => {
      this.styles = x.Records.map(s => {
        return {
          value: s.Folder,
          label: s.StyleName,
          id: s.Id
        };
      });
    });
    this.getExtModList().then(result => {
      this.model_list = result.Records.map(x => {
        return {
          label: x.ModuleName,
          value: x.Id
        };
      });
      this.model_list.unshift({
        label: "未选择",
        value: 0
      });
    });
    let settings = Object.assign({}, formSettings);
    let upload = settings.layouts[0].layouts[0].controls.find(
      x => x.key == "LogoImg"
    );
    upload.controlProps.httpRequest = ev => this.imageUpload(ev, "LogoImg");
    this.formSettings = settings;
  },
  activated() {
    this.refresh();
  },
  methods: {
    ...mapActions("styles", {
      getStyles: "getList",
      getTmplList: "getTmplList"
    }),
    ...mapActions("extMod", {
      getExtModList: "getList"
    }),
    ...mapActions("category", [
      "getList",
      "save",
      "del",
      "getById",
      "sortSave"
    ]),
    ...mapActions("global", ["fileUpload"]),
    handleEdit(row) {
      this.dataInfo = row;
      this.getById(row.Id).then(x => {
        if (x.Id > 0) {
          this.value = Object.assign({}, x);
          this.editFormVisble = true;
        }
      });
    },

    handleDelete(row) {
      this.$confirm(`是否要删除(${row.CategoryName})?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.del(row.Id).then(x => {
            if (x.Id > 0) {
              let tree = this.$refs["treeView"];
              tree.remove(row.Id);
              this.$message({
                type: "success",
                message: "删除成功!"
              });
            }
          });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消删除"
          });
        });
    },
    handleAdd(ev, row) {
      this.value = Object.assign({}, defaultValue);
      if (row) {
        this.dataInfo = Object.assign({}, row);
        this.dataInfo.CategoryName = "(添加下级)" + this.dataInfo.CategoryName;
        this.value.ParentId = row.Id;
      } else {
        this.dataInfo = {
          CategoryName: "添加"
        };
      }
      this.editFormVisble = true;
    },

    async editorImageAdded(file,callback) {
      let result = await this.onFileSelect({ file: file }, "Content", 1);
      if (result.Code == 0) {
        callback(result.link, { alt: file.name });
      }
    },
    async imageUpload(ev, key) {
      const form = this.$refs["form"];
      let ctrl = form.getControl(key);
      ctrl.clearFiles();
      let result = await this.onFileSelect(ev, key);
      form.setValue(key, result.link);
    },
    onFileSelect(ev, key, ftype = 0) {
      const form = this.$refs["form"];
      if (ev.file) {
        if (!ev.file.type.startsWith("image")) {
          this.$message({
            type: "error",
            message: "必须上传图片"
          });
          return;
        }
        return this.fileUpload({
          type: "category",
          file: ev.file
        }).then(x => {
          let file = this.files.find(f => f.propertyName == key && ftype == 0);
          if (file) {
            file.filePath = x.Data[0].url;
            file.propertyName = key;
            file.fileType = ftype;
          } else {
            this.files.push({
              filePath: x.Data[0].url,
              propertyName: key,
              fileType: ftype
            });
          }
          return x;
        });
      }
      //
    },
    //信息编辑窗口打开
    editFormOpen() {
      let form = this.$refs["form"];
      if (!form) {
        setTimeout(() => {
          this.editFormOpen();
        }, 100);
        return;
      }
      this.files = [];
      form.setLayoutProps("div", {
        value: "tab-1"
      });
      form.setOptions("StyleName", this.styles);
      form.setOptions("ExtModelId", this.model_list);
      if (this.value.StyleName) {
        this.styleChange(this.value.StyleName);
      }
      form.resetForm();
      let editor = form.getControl("Content");
      if (!editor.finished) {
        editor.finished = true;
        editor.imageAdded=this.editorImageAdded;
      }
      form.setValues(Object.assign({}, this.value));
    },
    handleSortSave() {
      orderid = 1;
      let treeView = this.$refs["treeView"];
      let data = this.getSortData(treeView.children, 0);
      this.sortSave(data).then(result => {
        this.$message({
          type: "success",
          message: "保存成功"
        });
        this.refresh();
      });
    },
    getSortData(list, pid) {
      let data = [];
      list.forEach(x => {
        data.push({
          Id: x.Id,
          ParentId: pid,
          OrderId: orderid++
        });
        if (x.children && x.children.length > 0) {
          let sub = this.getSortData(x.children, x.Id);
          data.push(...sub);
        }
      });
      return data;
    },
    //
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      vals.files = this.files;
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.editFormVisble = false;
          this.refresh();
        } else {
          this.$message({
            type: "error",
            message: x.msg
          });
        }
      });
    },

    generateTree(list, pid) {
      let nodes = [];
      list
        .filter(x => x.ParentId == pid)
        .forEach(n => {
          let node = {
            ...n
          };
          if (list.filter(x => x.ParentId == n.Id).length > 0) {
            node.children = this.generateTree(list, n.Id);
          }
          nodes.push(node);
        });

      return nodes;
    },
    async refresh() {
      let result = await this.getList({
        PageNum: 1,
        OrderBy: "OrderId asc"
      });
      this.treeData = this.generateTree(result.Records, 0);
    },
    styleChange(folderName) {
      let s = this.styles.find(x => x.value == folderName);
      if (s) {
        this.getTmplList(s.id).then(t => {
          if (t.Code == 0) {
            let files = t.Data.files;
            let form = this.$refs["form"];
            form.setOptions(
              "TmplName",
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
              "ArticleTmplName",
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
              "MobileTmplName",
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
              "ArticleMobileTmplName",
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
    onChange(ev) {
      if (ev.key == "StyleName") {
        this.styleChange(ev.value);
      }
    }
  },
  components: {}
};
</script>

<style lang="scss">
.tree-scrollbar {
  height: 100%;
  .el-scrollbar__wrap {
    overflow-x: hidden;
  }
}
</style>
