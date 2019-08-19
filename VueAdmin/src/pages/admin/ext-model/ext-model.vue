<template>
  <div class="page-panel" v-loading="loading">
    <div class="header-bar">
      <el-button-group>
        <el-button icon="el-icon-plus" size="mini" type="primary" @click="handleAdd">添加</el-button>
      </el-button-group>
    </div>
    <el-table
      :data="data_list"
      stripe
      row-key="Id"
      ref="table"
    >
      <el-table-column prop="ModuleName" label="模型名称"></el-table-column>
      <el-table-column prop="Describe" label="模型描述"></el-table-column>
      <el-table-column width="200" label="操作">
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              icon="el-icon-edit"
              size="mini"
              @click="handleEdit(scope.$index, scope.row)"
            >编辑</el-button>
            <el-button
              icon="el-icon-delete"
              size="mini"
              type="danger"
              @click="handleDelete(scope.$index, scope.row)"
            >删除</el-button>
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
    <el-dialog width="75%" title="数据模型编辑" :visible.sync="dialogFormVisible" @open="dialogOpened">
      <model-edit @onSubmit="onInfoSubmit" @onCancel="dialogFormVisible=false" ref="model-edit"></model-edit>
    </el-dialog>
  </div>
</template>

<script>
import { defaultValue, propsValue } from "./form_settings";
import modelEdit from "./model-edit";
import { mapActions, mapState } from "vuex";
export default {
  name: "data-model",
  data() {
    return {
      dialogFormVisible: false,
      columnsVisible: false,
      value: Object.assign({}, defaultValue),
      data_list: [],
      pageInfo: {},
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  async created() {
    this.pageInfo = Object.assign({}, this.$store.state.global.defaultPageInfo);
    this.pageInfo.QueryString = "";
    this.pageInfo.OrderBy = "Id asc";
    this.dataLoad();
  },

  methods: {
    ...mapActions("extMod", ["getList", "save", "del", "getById"]),
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


    dataLoad() {
      this.getList(this.pageInfo).then(x => {
        this.data_list = x.Records;
        let pinfo = this.pageInfo;
        pinfo.total = x.Count;
        this.pageInfo = pinfo;
      });
    },

    handleEdit(index, row) {
      this.getById(row.Id).then(x => {
        this.value = x;
        this.value.Status = x.Status == 1 ? true : false;
        this.dialogFormVisible = true;
      });
    },
    handleDelete(index, row) {
      this.$confirm(`是否要删除(${row.ModuleName})吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.del(row.Id).then(result => {
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
      let editor = this.$refs["model-edit"];
      if (!editor) {
        setTimeout(() => {
          this.dialogOpened();
        }, 100);
        return;
      }
      editor.setValues(Object.assign({}, this.value));
    },
    onInfoSubmit(vals) {
      if(vals.Columns.groupBy(x=>x.ColKey).where(x=>x.length>1).length>0){
        this.$message({
          type: "error",
          message: "字段关键字重复!"
        });
        return;
      }
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    }
  },
  components: { modelEdit }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
