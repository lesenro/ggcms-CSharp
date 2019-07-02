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
      <el-table-column prop="TaskName" label="任务名称"></el-table-column>
      <el-table-column prop="TaskType" label="任务类型">
        <template slot-scope="scope">{{TaskType.find(x=>x.value==scope.row.TaskType).label}}</template>
      </el-table-column>

      <el-table-column prop="StatusName" label="状态">
        <template slot-scope="scope">{{TaskStatus.find(x=>x.value==scope.row.Status).label}}</template>
      </el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              icon="el-icon-edit"
              size="mini"
              @click="handleEdit(scope.$index, scope.row)"
            >编辑</el-button>
            <el-button
              icon="el-icon-position"
              size="mini"
              type="success"
              @click="onRunNow(scope.row.Id)"
            >立即执行</el-button>
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
    <el-dialog title="任务编辑" width="70%" :visible.sync="dialogFormVisible" @open="dialogOpened">
      <el-row :gutter="10">
        <el-col :span="12">
          <form-generator
            :value="value"
            @change="onFormCtrlChange"
            ref="form"
            :settings="formSettings"
          ></form-generator>
        </el-col>
        <el-col :span="12">
          <form-generator
            v-if="value.TaskType==3"
            :value="static_task"
            @change="onFormCtrlChange"
            ref="static_task_form"
            :settings="static_task_form"
          ></form-generator>
        </el-col>
      </el-row>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import formSettings, {
  defaultValue,
  StaticTask,
  StaticTaskForm,
  TaskType,
  TaskStatus
} from "./form_settings";
import _ from "lodash";
import { mapActions, mapState } from "vuex";
export default {
  name: "task-list",
  data() {
    return {
      dialogFormVisible: false,
      formSettings: {},
      value: new defaultValue(),
      data_list: [],
      pageInfo: {},
      select_ids: [],
      static_task: new StaticTask(),
      static_task_form: {},
      category_tree: [],
      category_list: [],
      TaskType: TaskType,
      TaskStatus: TaskStatus
    };
  },
  computed: {
    ...mapState("global", ["page_sizes"]),
    ...mapState("dict", ["loading"])
  },
  async created() {
    let settings = Object.assign({}, formSettings);
    let cates = await this.getCategory({
      PageSize: 0,
      OrderBy: "OrderId asc"
    });
    this.formSettings = settings;
    //设置分类选项
    let staticForm = new StaticTaskForm();

    this.category_list = cates.Records;
    this.category_tree = this.generateTree(cates.Records, 0);
    let item = staticForm.layouts[0].controls.find(x => x.key == "Categories");

    if (item) {
      item.options = this.category_tree;
    }

    this.static_task_form = staticForm;
    this.pageInfo = Object.assign({}, this.$store.state.global.defaultPageInfo);
    this.pageInfo.QueryString = "";
    this.pageInfo.OrderBy = "Id desc";
    this.dataLoad();
  },

  methods: {
    ...mapActions("category", {
      getCategory: "getList"
    }),
    ...mapActions("task", ["getList", "save", "del", "getById","runNow"]),
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
    onRunNow(id){
      this.runNow(id);
      this.getList(this.pageInfo);
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
        x.Switch = x.Switch == 1 ? true : false;
        x = Object.assign({}, x, JSON.parse(x.PlanOptions || "{}"));
        
        if(x.TaskType==3){
          this.static_task = JSON.parse(x.TaskConfigs || "{}");
          this.static_task.Categories = this.static_task.CategorieTree;
        }
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
      this.value = new defaultValue();
      this.dialogFormVisible = true;
      this.static_task = new StaticTask();
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
      this.PlanTypeChange(1);
      this.onFormCtrlChange({
        key: "All",
        value: this.static_task.All
      });
      form.setValues(Object.assign({}, this.value));
      let staticform = this.$refs["static_task_form"];
      staticform.updateValue("Categories", this.static_task.Categories);
    },
    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      if(vals.Switch){
        vals.Status= 4;
      }
      vals.Switch = vals.Switch ? 1 : 0;

      vals.PlanOptions = JSON.stringify({
        SpecificDate: vals.SpecificDate,
        StartDate: vals.StartDate,
        EndDate: vals.EndDate,
        IntervalMinute: vals.IntervalMinute,
        WeekDays: vals.WeekDays,
        MonthDays: vals.MonthDays
      });
      delete vals.SpecificDate;
      delete vals.StartDate;
      delete vals.EndDate;
      delete vals.IntervalMinute;
      delete vals.WeekDays;
      delete vals.MonthDays;
      if (vals.TaskType == 3) {
        let form = this.$refs["static_task_form"];
        let staticVal = form.formSubmit();
        if (!staticVal) {
          return;
        }
        staticVal.CategorieTree = staticVal.Categories;
        staticVal.Categories = _.union(_.flattenDeep(staticVal.Categories));
        vals.TaskConfigs = JSON.stringify(staticVal);
      }
      this.save(vals).then(x => {
        if (x.Id > 0) {
          this.dialogFormVisible = false;
          this.dataLoad();
        }
      });
    },

    //表单项改动事件
    onFormCtrlChange(ev) {
      if (ev.key == "PlanType") {
        this.PlanTypeChange(ev.value);
      } else if (ev.key == "All") {
        let form = this.$refs["static_task_form"];
        form.setItemProps("Categories", { class: "hidden" });
        if (!ev.value) {
          form.setItemProps("Categories", { class: false });
        }
      }
    },
    PlanTypeChange(type) {
      let form = this.$refs["form"];
      [
        "IntervalMinute",
        "WeekDays",
        "MonthDays",
        "StartDate",
        "EndDate"
      ].forEach(x => {
        form.setItemProps(x, { class: "hidden" });
      });
      let show_list = [];
      switch (type) {
        case 2:
          show_list = ["StartDate", "EndDate"];
          break;
        case 3:
          show_list = ["StartDate", "EndDate", "WeekDays"];
          break;
        case 4:
          show_list = ["StartDate", "EndDate", "MonthDays"];
          break;
        case 5:
          show_list = ["StartDate", "EndDate", "IntervalMinute"];
          break;
      }
      show_list.forEach(x => {
        form.setItemProps(x, { class: false });
      });
    }
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
