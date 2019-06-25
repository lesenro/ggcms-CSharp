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
      <el-table-column prop="UserName" label="用户名"></el-table-column>
      <el-table-column prop="Email" label="电邮"></el-table-column>
      <el-table-column prop="Phone" label="手机"></el-table-column>
      <el-table-column prop="Roles_Id" label="角色">
        <template slot-scope="scope">{{getRoleName(scope.row.Roles_Id)}}</template>
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
    <el-dialog title="友情链接" :visible.sync="dialogFormVisible" @open="dialogOpened">
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
  name: "user-list",
  data() {
    return {
      dialogFormVisible: false,
      formSettings: {},
      value: Object.assign({}, defaultValue),
      data_list: [],
      pageInfo: {},
      roles: [],
      select_ids: [],
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  async created() {
    let settings = Object.assign({}, formSettings);

    this.formSettings = settings;
    let roles = await this.getDictList({
      QueryString: 'GroupKey=="user_role" and DictType==0 and DictStatus=1',
      PageSize: 0,
      OrderBy: "OrderId asc"
    });
    if (roles.Records.length > 0) {
      this.roles = roles.Records.map(x => {
        return {
          label: x.DictName,
          value: x.Id
        };
      });
    }
    this.pageInfo = Object.assign({}, defaultValue);
    this.pageInfo.QueryString = "";
    this.pageInfo.OrderBy = "Id asc";
    this.dataLoad();
  },

  methods: {
    ...mapActions("dict", {
      getDictList: "getList"
    }),
    ...mapActions("user", ["getList", "save", "del", "getById","modifyPassword"]),
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
    getRoleName(gkey) {
      let role = this.roles.find(x => x.value == gkey);
      if (role) {
        return role.label;
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
      form.resetForm();
      form.setOptions("Roles_Id", this.roles);
      if(this.value.Id>0){
        form.setRules("PassWord",[{required:false}]);
      }else{
        form.setRules("PassWord",[{required:true}]);
      }
      form.setValues(Object.assign({}, this.value));
    },
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    },

    //表单项改动事件
    onFormCtrlChange(ev) {}
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
