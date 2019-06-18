<template>
  <div class="page-panel" v-loading="loading">
    <div class="header-bar">
      <el-button-group>
        <el-button icon="el-icon-plus" size="mini" type="primary" @click="handleAdd">添加</el-button>
        <el-button icon="el-icon-delete" size="mini" type="danger" @click="handleDelete">删除</el-button>
      </el-button-group>
    </div>
    <el-table
      :data="data_list"
      stripe
      row-key="Id"
      ref="table"
      @selection-change="handleSelectionChange"
    >
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column prop="Title" label="文章标题"></el-table-column>
      <el-table-column prop="Category_Id" label="所属分类">
        <template slot-scope="scope">{{getCategoryName(scope.row.Category_Id)}}</template>
      </el-table-column>
      <el-table-column prop="Author" label="作者"></el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              icon="el-icon-edit"
              size="mini"
              @click="handleEdit(scope.$index, scope.row)"
            >编辑</el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
    <div class="footer-bar" text-right>
      <el-pagination
        class="pagination"
        @size-change="handleSizeChange"
        @current-change="currentChange"
        :current-page="pageInfo.PageNum"
        :page-sizes="page_sizes"
        :page-size="pageInfo.PageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="pageInfo.total"
      ></el-pagination>
    </div>
    <el-dialog title="文章管理" :visible.sync="dialogFormVisible" @open="dialogOpened">
      <form-generator :value="value" @change="onFormCtrlChange" ref="form" :settings="formSettings"></form-generator>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import formSettings, { defaultValue } from "./form_settings";
import { mapActions, mapState } from "vuex";
export default {
  name: "article-list",
  data() {
    return {
      dialogFormVisible: false,
      formSettings: {},
      value: Object.assign({}, defaultValue),
      data_list: [],
      pageInfo: {},
      display_modes: [],
      top_levels: [],
      select_ids: [],
      category_list: [],
      category_tree: [],
      styles: [],
      files: [],
      article_type: 0
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  async created() {
    let settings = Object.assign({}, formSettings);
    let upload = settings.layouts[0].layouts[0].controls.find(
      x => x.key == "TitleImg"
    );
    upload.controlProps.httpRequest = ev => this.imageUpload(ev, "TitleImg");
    this.formSettings = settings;
    let cates = await this.getCategory({
      PageSize: 0,
      OrderBy: "OrderId asc"
    });
    this.category_list = cates.Records;
    this.category_tree = this.generateTree(cates.Records, 0);
    let grps = await this.getDictList({
      QueryString:
        '(GroupKey=="display_mode" or GroupKey=="top_level") and DictType==0 and DictStatus=1',
      PageSize: 0,
      OrderBy: "OrderId asc"
    });
    if (grps.Records.length > 0) {
      this.display_modes = grps.Records.where(
        x => x.GroupKey == "display_mode"
      ).map(x => {
        return {
          label: x.DictName,
          value: x.DictKey
        };
      });
      this.display_modes.unshift({
        label: "未选择",
        value: 0
      });
      this.top_levels = grps.Records.where(x => x.GroupKey == "top_level").map(
        x => {
          return {
            label: x.DictName,
            value: x.DictKey
          };
        }
      );
      this.top_levels.unshift({
        label: "未选择",
        value: 0
      });
    }

    let styles = await this.getStyles();
    this.styles = styles.Records.map(s => {
      return {
        value: s.Folder,
        label: s.StyleName,
        id: s.Id
      };
    });
    this.pageInfo = Object.assign({}, this.$store.state.global.defaultPageInfo);
    this.pageInfo.QueryString = "";
    this.pageInfo.OrderBy = "Id asc";

    if (this.$route.path == "/admin/content/article") {
      this.article_type = 0;
      this.pageInfo.QueryString = "Category_Id>0";
    } else if (this.$route.path == "/admin/content/review") {
      this.article_type = 1;
      this.pageInfo.QueryString = "Category_Id<0";
    } else if (this.$route.path == "/admin/content/single") {
      this.article_type = 2;
      this.pageInfo.QueryString = "Category_Id==0";
    }
    this.dataLoad();
  },

  methods: {
    ...mapActions("styles", {
      getStyles: "getList",
      getTmplList: "getTmplList"
    }),
    ...mapActions("dict", {
      getDictList: "getList"
    }),
    ...mapActions("category", {
      getCategory: "getList"
    }),
    ...mapActions("article", ["getList", "save", "del", "getById"]),
    ...mapActions("global", ["fileUpload"]),
    generateTree(list, pid) {
      let nodes = [];
      list
        .filter(x => x.ParentId == pid)
        .forEach(n => {
          let node = {
            value: n.Id,
            label: n.CategoryName
          };
          if (list.filter(x => x.ParentId == n.Id).length > 0) {
            node.children = this.generateTree(list, n.Id);
          }
          nodes.push(node);
        });

      return nodes;
    },
    currentChange(ev) {
      let pageInfo = this.pageInfo;
      pageInfo.PageNum = ev;
      this.getList(pageInfo);
    },
    handleSizeChange(ev) {
      let pageInfo = this.pageInfo;
      pageInfo.PageSize = ev;
      this.getList(pageInfo);
    },
    getCategoryName(cid) {
      let c = this.category_list.find(x => x.Id == cid);
      if (c) {
        return c.CategoryName;
      }
      return "";
    },
    handleSelectionChange(rows) {
      this.select_ids = rows.map(x => x.Id);
    },
    dataLoad() {
      this.getList(this.pageInfo).then(x => {
        this.data_list = x.Records;
        let pinfo = this.pageInfo;
        pinfo.total = x.Count;
        this.pageInfo = pinfo;
      });
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
          type: "article",
          file: ev.file
        }).then(x => {
          let file = this.files.find(f => f.propertyName == key);
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
    },
    handleEdit(index, row) {
      this.getById(row.Id).then(x => {
        this.value = x;
        this.value.Status = x.Status == 1 ? true : false;
        this.value.CategoryId = [];
        this.dialogFormVisible = true;
        this.findCategoryIds(x.Category_Id);
      });
    },
    findCategoryIds(cid) {
      if (cid > 0) {
        this.value.CategoryId.unshift(cid);
        let category = this.category_list.find(x => x.Id == cid);
        this.findCategoryIds(category.ParentId);
      }
    },
    handleDelete(index, row) {
      if (this.select_ids.length == 0) {
        this.$message({
          type: "error",
          message: "请先选择要删除的数据"
        });
        return;
      }
      this.$confirm(`是否要删除(${this.select_ids.length})条记录吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.del(this.select_ids).then(result => {
            if (result > 0) {
              this.$message({
                type: "success",
                message: "删除成功!"
              });
              this.dataLoad();
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
    handleAdd() {
      this.value = Object.assign({}, defaultValue);
      this.dialogFormVisible = true;
    },
    dialogOpened() {
      let form = this.$refs["form"];
      if (!form) {
        setTimeout(() => {
          this.dialogOpened();
        }, 100);
        return;
      }
      this.files = [];
      form.setOptions("StyleName", this.styles);
      if (this.value.StyleName) {
        this.styleChange(this.value.StyleName);
      }
      form.setOptions("ShowLevel", this.top_levels);
      form.setOptions("ShowType", this.display_modes);
      form.setOptions("CategoryId", this.category_tree);
      form.resetForm();
      let editor = form.getControl("Content");
      if (!editor.finished) {
        editor.finished = true;
        editor.$on("imageAdded", this.editorImageAdded);
      }
      form.setValues(Object.assign({}, this.value));
    },
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      vals.files = this.files;
      if (vals.CategoryId.length == 0) {
        return;
      }
      vals.Category_Id = vals.CategoryId[vals.CategoryId.length - 1];
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    },
    async editorImageAdded(file, Editor, cursorLocation, resetUploader) {
      let result = await this.onFileSelect({ file: file }, "Content", 1);
      if (result.Code == 0) {
        Editor.insertEmbed(cursorLocation, "image", result.link);
        resetUploader();
      }
    },
    async imageUpload(ev, key) {
      const form = this.$refs["form"];
      let ctrl = form.getControl(key);
      ctrl.clearFiles();
      let result = await this.onFileSelect(ev, key);
      form.setValue(key, result.link);
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
    //表单项改动事件
    onFormCtrlChange(ev) {
      if (ev.key == "StyleName") {
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
