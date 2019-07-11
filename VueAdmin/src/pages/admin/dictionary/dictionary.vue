<template>
  <div class="page-panel" v-loading="loading">
    <el-row type="flex" class="header-bar" justify="space-between">
      <el-col :span="8">
        <el-button-group>
          <el-button icon="el-icon-plus" size="mini" type="primary" @click="handleAdd">添加</el-button>
          <el-button icon="el-icon-delete" size="mini" type="danger" @click="handleDelete">删除</el-button>
        </el-button-group>
      </el-col>
      <el-col :span="8">
        <el-form size="mini" :inline="true" class="float-right">
          <el-form-item required>
            <el-input v-model="searchKey" clearable @clear="clearSearch" placeholder="查询关键词"></el-input>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="onSearch">查询</el-button>
          </el-form-item>
        </el-form>
      </el-col>
    </el-row>    
    <el-table
      :data="data_list"
      stripe
      row-key="Id"
      ref="table"
      @selection-change="handleSelectionChange"
    >
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column prop="DictName" label="字典名称"></el-table-column>
      <el-table-column prop="DictKey" label="关键字"></el-table-column>
      <el-table-column prop="DictValue" label="字典值"></el-table-column>
      <el-table-column prop="GroupKey" label="类型">
        <template slot-scope="scope">
          <el-button
            size="mini"
            type="primary"
            plain
            @click="searchByGroup(scope.row.GroupKey)"
          >{{getGroupName(scope.row.GroupKey)}}</el-button>
        </template>
      </el-table-column>
      <el-table-column prop="DictStatus" label="状态">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.DictStatus==1" type="success">正常</el-tag>
          <el-tag v-if="scope.row.DictStatus==0" type="danger">禁用</el-tag>
        </template>
      </el-table-column>
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
    <el-dialog title="系统字典" :visible.sync="dialogFormVisible" @open="dialogOpened">
      <form-generator :value="value" @change="onFormCtrlChange" ref="form" :settings="formSettings"></form-generator>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import formSettings from "./dict_form";
import { mapActions, mapState } from "vuex";
export default {
  name: "dict-list",
  data() {
    return {
      dialogFormVisible: false,
      formSettings: Object.assign({}, formSettings),
      value: Object.assign({}, formSettings.value),
      data_list: [],
      pageInfo: {},
      dict_groups: [],
      select_ids: [],
      searchKey:""
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  created() {
    this.getList({
      QueryString: 'DictType==0 and GroupKey=="base_dict"',
      PageSize: 0,
      OrderBy: "OrderId asc"
    }).then(x => {
      if (x.Records.length > 0) {
        this.dict_groups = x.Records.map(d => {
          return {
            label: d.DictName,
            value: d.DictKey
          };
        });
      }
    });
    this.pageInfo = Object.assign({}, this.$store.state.global.defaultPageInfo);
    this.pageInfo.QueryString = "DictType==1";
    this.pageInfo.OrderBy = "OrderId asc";
    this.dataLoad();
  },

  methods: {
    ...mapActions("dict", ["getList", "save", "del", "getById"]),
    currentChange(ev) {
      let pageInfo = this.pageInfo;
      pageInfo.PageNum = ev;
      this.dataLoad();
    },
    handleSizeChange(ev) {
      let pageInfo = this.pageInfo;
      pageInfo.PageSize = ev;
      this.dataLoad();
    },
    getGroupName(gkey) {
      let grp = this.dict_groups.find(x => x.value == gkey);
      if (grp) {
        return grp.label;
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
    handleEdit(index, row) {
      this.getById(row.Id).then(x => {
        this.value = x;
        this.value.DictStatus = x.DictStatus == 1 ? true : false;
        this.dialogFormVisible = true;
      });
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
      this.value = Object.assign({}, formSettings.value);

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
      form.resetForm();
      form.setOptions("GroupKey", this.dict_groups);
      form.setValues(Object.assign({}, this.value));
    },
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      vals.DictStatus = vals.DictStatus ? 1 : 0;
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    },

    //表单项改动事件
    onFormCtrlChange(ev) {},
    searchByGroup(gid) {
      if (!gid) {
        return;
      }
      this.searchKey = "g:" + this.getGroupName(gid);
      this.onSearch();
    },
    onSearch() {
      if (!this.searchKey) {
        return;
      }
      let rule = /^g:/gi;
      if (rule.test(this.searchKey)) {
        let group = this.searchKey.replace(rule, "");
        let citem = this.dict_groups.find(
          x => x.label.indexOf(group) != -1
        );
        if (citem) {
          this.pageInfo.QueryString = `DictType==1 and GroupKey=="${citem.value}"`;
        } else {
          return;
        }
      } else {
        this.pageInfo.QueryString = `DictType==1 and (DictKey.Contains("${this.searchKey}") or GroupKey.Contains("${this.searchKey}") or DictName.Contains("${this.searchKey}"))`;
      }
      this.pageInfo.PageNum = 1;
      this.dataLoad();
    },
    clearSearch() {
      this.searchKey = "";
      this.pageInfo.QueryString = `DictType==1`;
      this.pageInfo.PageNum = 1;
      this.dataLoad();
    }
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
