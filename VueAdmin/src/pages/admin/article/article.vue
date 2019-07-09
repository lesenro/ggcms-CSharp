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
        <el-button-group class="float-left">
          <el-button
            :type="value.attachments.length>0?'primary':'info'"
            @click="attachmentVisible=true"
          >附件管理</el-button>
          <el-button v-if="extModelInfo" type="success" @click="modelInputVisible=true">数据模型</el-button>
        </el-button-group>
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
    <el-dialog title="附件管理" width="70%" append-to-body :visible.sync="attachmentVisible">
      <el-scrollbar ref="scrollbar">
        <div class="attr-list" v-if="attrCount>0">
          <div class="attr-item" :key="item.key" v-for="item  in value.attachments">
            <form-generator class="attr-form" :value="item" ref="attrForm" :settings="item.form"></form-generator>
            <el-button size="mini" type="danger" @click="attrDel(item)" icon="el-icon-delete"></el-button>
          </div>
        </div>
        <div class="no-data" text-center v-if="attrCount==0">
          <span>暂无附件</span>
        </div>
      </el-scrollbar>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" class="float-left" icon="el-icon-plus" @click="attachmentAdd">添加</el-button>
        <el-button type="primary" @click="attachmentVisible=false">确 定</el-button>
      </div>
    </el-dialog>
    <el-dialog
      title="模型数据输入"
      append-to-body
      :visible.sync="modelInputVisible"
      @open="modelInputOpened"
    >
      <el-scrollbar ref="scrollbar">
        <div class="attr-list">
          <model-input
            @onSubmit="modelDataSubmit"
            @onCancel="modelInputVisible=false"
            ref="model-input"
          />
        </div>
      </el-scrollbar>
    </el-dialog>
  </div>
</template>

<script>
import {
  ArticleForm,
  defaultValue,
  GgcmsAttachment,
  GgcmsAttachmentFrom
} from "./form_settings";
import modelInput from "./model-input";
import { mapActions, mapState } from "vuex";
import { GgcmsArticlePages, EntityState } from "@/components/articleContent";
export default {
  name: "article-list",
  data() {
    return {
      dialogFormVisible: false,
      attachmentVisible: false,
      modelInputVisible: false,
      formSettings: {},
      value: new defaultValue(),
      data_list: [],
      pageInfo: {},
      display_modes: [],
      top_levels: [],
      select_ids: [],
      category_list: [],
      category_tree: [],
      styles: [],
      files: [],
      extModelInfo: null
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"]),
    attrCount() {
      return this.value.attachments.length;
    }
  },
  async created() {
    let settings = new ArticleForm();
    //设置标题图上传
    let upload = settings.layouts[0].layouts[0].controls.find(
      x => x.key == "TitleImg"
    );
    upload.controlProps.httpRequest = ev => this.imageUpload(ev, "TitleImg");
    let cates = await this.getCategory({
      PageSize: 0,
      OrderBy: "OrderId asc"
    });
    //设置分类选项
    this.category_list = cates.Records;
    this.category_tree = this.generateTree(cates.Records, 0);
    let item = settings.layouts[0].layouts[0].controls.find(
      x => x.key == "CategoryId"
    );
    if (item) {
      item.options = this.category_tree;
    }
    //设置文章级别
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
          value: Number(x.DictValue)
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
            value: Number(x.DictValue)
          };
        }
      );
      this.top_levels.unshift({
        label: "未选择",
        value: 0
      });
    }
    item = settings.layouts[0].layouts[2].controls.find(
      x => x.key == "ShowType"
    );
    if (item) {
      item.options = this.display_modes;
    }
    item = settings.layouts[0].layouts[2].controls.find(
      x => x.key == "ShowLevel"
    );
    if (item) {
      item.options = this.top_levels;
    }
    //设置风格
    let styles = await this.getStyles();
    this.styles = styles.Records.map(s => {
      return {
        value: s.Folder,
        label: s.StyleName,
        id: s.Id
      };
    });
    item = settings.layouts[0].layouts[2].controls.find(
      x => x.key == "StyleName"
    );
    if (item) {
      item.options = this.styles;
    }
    this.formSettings = settings;

    this.pageInfo = Object.assign({}, this.$store.state.global.defaultPageInfo);
    this.pageInfo.QueryString = "";
    this.pageInfo.OrderBy = "Id desc";

    this.pageInfo.QueryString = "Category_Id>0";

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
    ...mapActions("article", [
      "getList",
      "save",
      "del",
      "getById",
      "getValues"
    ]),
    ...mapActions("global", ["fileUpload"]),
    ...mapActions("extMod", {
      getModelById: "getById"
    }),

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
      this.dataLoad(pageInfo);
    },
    handleSizeChange(ev) {
      let pageInfo = this.pageInfo;
      pageInfo.PageSize = ev;
      this.dataLoad(pageInfo);
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
    attachmentAdd() {
      let attr = new GgcmsAttachment();
      if (this.value.attachments.length == 0) {
        attr.key = 0;
      } else {
        attr.key = this.value.attachments.max(x => x.key) || 0;
      }
      attr.key++;
      attr.form = new GgcmsAttachmentFrom();
      let upload = attr.form.layouts[0].layouts[1].controls.find(
        x => x.key == "AttaUrl"
      );
      upload.controlProps.httpRequest = ev =>
        this.attrUpload(ev, "AttaUrl", attr.key);
      this.value.attachments.push(attr);
    },
    attrDel(item) {
      //删除文件
      let file = this.files.find(x => x.filePath == item.RealName);
      if (file) {
        let fidx = this.files.indexOf(file);
        if (fidx != -1) {
          this.files.splice(fidx, 1);
        }
      }
      //删除附件
      let idx = this.value.attachments.indexOf(item);
      if (idx != -1) {
        this.value.attachments.splice(idx, 1);
      }
    },
    dataLoad() {
      this.getList(this.pageInfo).then(x => {
        this.data_list=x.Records;
        let pinfo = this.pageInfo;
        pinfo.total = x.Count;
        this.pageInfo = pinfo;
      });
    },
    onFileSelect(ev, key, ftype = 0) {
      const form = this.$refs["form"];
      if (ev.file) {
        if (ftype != 2 && !ev.file.type.startsWith("image")) {
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
        let page1 = new GgcmsArticlePages();
        page1.Content = x.Content || "";
        page1.Title = x.PageTitle || "";
        page1.Article_Id = x.Id;
        x.pages.unshift(page1);
        this.value = x;
        this.value.CategoryId = [];
        this.findCategoryIds(x.Category_Id);
        this.onFormCtrlChange({
          key: "CategoryId",
          value: this.value.CategoryId
        });
        this.value.attachments.forEach(a => {
          a.key = a.Id;
          a.form = new GgcmsAttachmentFrom();
          let upload = a.form.layouts[0].layouts[1].controls.find(
            x => x.key == "AttaUrl"
          );
          upload.controlProps.httpRequest = ev =>
            this.attrUpload(ev, "AttaUrl", a.key);
        });
        this.dialogFormVisible = true;
      });
    },
    findCategoryIds(cid) {
      if (cid > 0) {
        this.value.CategoryId.unshift(cid);
        let category = this.category_list.find(x => x.Id == cid);
        if(category){
          this.findCategoryIds(category.ParentId);
        }
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
      this.value = new defaultValue();
      this.value.pages.push(new GgcmsArticlePages());
      this.extModelInfo = null;
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
      form.resetForm();
      form.setLayoutProps("div", { value: "tab-1" });
      if (this.value.StyleName) {
        this.styleChange(this.value.StyleName);
      }
      form.setValues(Object.assign({}, this.value));
      form.updateValue("CategoryId", this.value.CategoryId);
      //强制更新分页
      let pagesCtrl = form.getControl("pages");
      pagesCtrl.setValues(this.value.pages, this.value.Id);
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
      let page1 = vals.pages.find(x => x.OrderId == 1);
      vals.Content = page1.Content;
      vals.PageTitle = page1.Title;
      vals.files.push(...page1.files);
      if (page1.Id == 0) {
        let idx = vals.pages.indexOf(page1);
        vals.pages.splice(idx, 1);
      } else {
        page1.state = EntityState.Deleted;
      }
      vals.Category_Id = vals.CategoryId[vals.CategoryId.length - 1];
      vals.ModuleInfo = this.extModelInfo;
      vals.attachments = vals.attachments
        .filter(x => x.AttaUrl)
        .map(x => {
          return {
            Id: x.Id,
            AttaTitle: x.AttaTitle,
            AttaUrl: x.AttaUrl,
            Describe: x.Describe,
            RealName: x.RealName
          };
        });
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    },
    // async editorImageAdded(file, Editor, cursorLocation, resetUploader) {
    //   let result = await this.onFileSelect({ file: file }, "Content", 1);
    //   if (result.Code == 0) {
    //     Editor.insertEmbed(cursorLocation, "image", result.link);
    //     resetUploader();
    //   }
    // },
    async imageUpload(ev, key) {
      const form = this.$refs["form"];
      let ctrl = form.getControl(key);
      ctrl.clearFiles();
      let result = await this.onFileSelect(ev, key);
      form.setValue(key, result.link);
    },
    async attrUpload(ev, key, id) {
      const form = this.$refs["attrForm"].find(x => x.value.key == id);
      let ctrl = form.getControl(key);
      ctrl.clearFiles();
      let result = await this.onFileSelect(ev, "attachments", 3);
      form.setValue(key, result.link);
      form.value.RealName = result.Data[0].url;
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
    async onFormCtrlChange(ev) {
      if (ev.key == "StyleName") {
        this.styleChange(ev.value);
      } else if (ev.key == "CategoryId") {
        let cid = 0;
        if (ev.value.length > 0) {
          cid = ev.value[ev.value.length - 1];
        }

        let category = this.category_list.find(x => x.Id == cid);
        if (category && category.ExtModelId > 0) {
          //分类有扩展模型信息
          this.extModelInfo = await this.getModelById(category.ExtModelId);
          if (this.value.Id > 0) {
            //文章扩展模型信息
            let vals = await this.getValues({
              aid: this.value.Id,
              mid: category.ExtModelId
            });
            this.extModelInfo.Columns.forEach(x => {
              x.Value = vals[x.ColName];
            });
          }
        } else {
          this.extModelInfo = null;
        }
      }
    },
    modelInputOpened() {
      let editor = this.$refs["model-input"];
      if (!editor) {
        setTimeout(() => {
          this.modelInputOpened();
        }, 100);
        return;
      }
      editor.setValues(this.extModelInfo);
    },
    modelDataSubmit(vals) {
      this.extModelInfo.Columns.forEach(x => {
        x.Value = vals[x.ColName];
      });
      this.modelInputVisible = false;
    }
  },
  components: {
    modelInput
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
.attr-list {
  max-height: 300px;
  .attr-item {
    .attr-form {
      display: inline-block;
      vertical-align: top;
      margin-right: 15px;
    }
  }
}
</style>
