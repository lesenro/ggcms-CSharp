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
      <el-table-column prop="ModuleName" label="模型名称"></el-table-column>
      <el-table-column prop="Describe" label="模型描述"></el-table-column>
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
    <el-dialog width="75%" title="数据模型编辑" :visible.sync="dialogFormVisible" @open="dialogOpened">
      <el-row class="module-editor" :gutter="15">
        <el-col :span="8">
          <form-generator
            :value="value"
            @change="onFormCtrlChange"
            ref="form"
            :settings="formSettings"
          ></form-generator>
        </el-col>
        <el-col :span="16">
          <el-table
            :data="model_list"
            stripe
            row-key="Id"
            ref="table"
            @selection-change="handleSelectionChange"
          >
            <el-table-column prop="ModuleName" label="模型名称"></el-table-column>
            <el-table-column prop="Describe" label="模型描述"></el-table-column>
            <el-table-column label="操作">
              <template slot-scope="scope">
                <el-button-group>
                  <el-button
                    icon="el-icon-close"
                    size="mini"
                    @click="handleEdit(scope.$index, scope.row)"
                  >删除</el-button>
                </el-button-group>
              </template>
            </el-table-column>
          </el-table>
        </el-col>
      </el-row>

      <div slot="footer" class="dialog-footer">
        <el-button type="primary" icon="el-icon-plus" @click="columnsAdd()">添加字段</el-button>
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
      </div>
    </el-dialog>
    <el-dialog
      width="80%"
      append-to-body
      title="字段编辑"
      :visible.sync="columnsVisible"
      @open="columnsOpened"
    >
      <el-row :gutter="15">
        <el-col :span="8">
          <form-generator :value="col_value" ref="col-form" :settings="columnModuleForm"></form-generator>
        </el-col>

        <el-col :span="16">
          <el-tabs value="rule_code">
            <el-tab-pane label="验证代码" name="rule_code">
              <el-row :gutter="15">
                <el-col :span="12">
                  <form-generator
                    @change="onRuleFormChange"
                    :value="rule_value"
                    ref="rule-form"
                    :settings="controlForm"
                  ></form-generator>
                  <el-divider>操作</el-divider>
                  <div class="float-right">
                    <el-button type="primary" @click="ruleAdd" size="mini">添加</el-button>
                    <el-button type="danger" @click="rule_code='[]'" size="mini">清空</el-button>
                  </div>
                </el-col>
                <el-col :span="12">
                  <codemirror name="rule_code" v-model="rule_code"></codemirror>
                </el-col>
              </el-row>
            </el-tab-pane>
            <el-tab-pane label="属性代码" name="prop_code">
              <el-row :gutter="15">
                <el-col :span="12">
                  <form-generator
                    @change="onPropsFormChange"
                    :value="prop_value"
                    ref="props-form"
                    :settings="propsForm"
                  ></form-generator>
                  <el-divider>操作</el-divider>
                  <div class="float-right">
                    <el-button type="primary" @click="propsAdd" size="mini">添加</el-button>
                    <el-button type="danger" @click="prop_code='{}'" size="mini">清空</el-button>
                  </div>
                </el-col>
                <el-col :span="12">
                  <codemirror name="prop_code" v-model="prop_code"></codemirror>
                </el-col>
              </el-row>
            </el-tab-pane>
          </el-tabs>
        </el-col>
      </el-row>

      <div slot="footer" class="dialog-footer">
        <el-button @click="columnsVisible = false">取 消</el-button>
        <el-button type="primary" @click="onColumnSubmit">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import formSettings, {
  defaultValue,
  moduleColumns,
  moduleForm,
  controlForm,
  validataOptions,
  controlValue,
  propsOptions,
  propsValue,
  propsForm
} from "./form_settings";
import { mapActions, mapState } from "vuex";
export default {
  name: "data-model",
  data() {
    return {
      dialogFormVisible: false,
      columnsVisible: false,
      formSettings: Object.assign({}, formSettings),
      value: Object.assign({}, defaultValue),
      columnModuleForm: new moduleForm(),
      controlForm: new controlForm(),
      propsForm: new propsForm(),
      col_value: new moduleColumns(),
      data_list: [],
      pageInfo: {},
      select_ids: [],
      model_list: [],
      rule_value: new controlValue(),
      prop_value: new propsValue(),
      rule_code: "[]",
      prop_code: "{}"
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
    ...mapActions("extMod", ["getList", "save", "del", "getById", "getValues"]),
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
        this.value.Status = x.Status == 1 ? true : false;
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
    onFormCtrlChange(ev) {},
    columnsOpened() {
      let form = this.$refs["rule-form"];
      if (!form) {
        setTimeout(() => {
          this.columnsOpened();
        }, 100);
        return;
      }
      form.resetForm();
      this.onRuleFormChange({
        key: "validata",
        value: "required"
      });
      this.rule_value = new controlValue();
      form.setValues(Object.assign({}, this.rule_value));

      let pform = this.$refs["props-form"];
      pform.resetForm();
      this.prop_value = new propsValue();
      pform.setValues(Object.assign({}, this.prop_value));
      this.onPropsFormChange({
        key: "propType",
        value: "size"
      });
    },
    onRuleFormChange(ev) {
      if (ev.key == "validata") {
        let all = validataOptions.find(x => x.value == "all");
        let item = validataOptions.find(x => x.value == ev.value);
        let form = this.$refs["rule-form"];
        all.hidden.forEach(x => {
          form.setItemProps(x, { class: false });
        });
        item.hidden.forEach(x => {
          form.setItemProps(x, { class: "hidden" });
        });
      }
    },
    ruleAdd() {
      let rules = JSON.parse(this.rule_code);
      let form = this.$refs["rule-form"];
      let vals = form.formSubmit();
      let r = { trigger: "blur" };
      switch (vals.validata) {
        case "required":
          r.required = true;
          r.message = vals.message;
          break;
        case "regexp":
          r.pattern = vals.pattern;
          r.message = vals.message;
          break;
        case "range":
          r.min = vals.min;
          r.max = vals.max;
          r.message = vals.message;
          break;
        case "min":
          r.min = vals.min;
          r.message = vals.message;
          break;
        case "max":
          r.max = vals.max;
          r.message = vals.message;
          break;
      }
      rules.push(r);
      this.rule_code = JSON.stringify(rules, null, "\t");
    },

    onPropsFormChange(ev) {
      if (ev.key == "propType") {
        let all = propsOptions.find(x => x.value == "all");
        let item = propsOptions.find(x => x.value == ev.value);
        let form = this.$refs["props-form"];
        all.hidden.forEach(x => {
          form.setItemProps(x, { class: false });
        });
        item.hidden.forEach(x => {
          form.setItemProps(x, { class: "hidden" });
        });
      }
    },
    propsAdd() {
      let props = JSON.parse(this.prop_code);
      let form = this.$refs["props-form"];
      let vals = form.formSubmit();
      let item = propsOptions.find(x => x.value == vals.propType);
      let p = {};
      switch (vals.propType) {
        case "size":
          p.size = vals.size;
          break;
        case "label-width":
          p["label-width"] = vals["label-width"];
          break;
        case "placeholder":
          p.placeholder = vals.value;
          break;
        case "lenRange":
          p.minlength = vals.min;
          p.maxlength = vals.max;
          break;
        case "range":
          p.min = vals.min;
          p.max = vals.max;
          break;
        case "minlength":
          p.minlength = vals.min;
          break;
        case "maxlength":
          p.maxlength = vals.max;
          break;
        case "min":
          p.min = vals.min;
          break;
        case "max":
          p.max = vals.max;
          break;
      }
      if (item.itemType == "item") {
        props.itemProps = Object.assign({}, props.itemProps, p);
      } else if (item.itemType == "control") {
        props.controlProps = Object.assign({}, props.controlProps, p);
      }
      this.prop_code = JSON.stringify(props, null, "\t");
    },
    onColumnSubmit() {
      let form = this.$refs["col-form"];
      let vals = form.formSubmit();
      let rules = JSON.parse(this.rule_code);
      let props = JSON.parse(this.prop_code);
      props.rules = rules;
      props.type = vals.inputType;
      vals.Options = JSON.stringify(props);
      console.log(vals);
    },
    columnsAdd(info) {
      this.columnsVisible = true;
    }
  }
};
</script>

<style lang="scss">
.data-table::before {
  height: 0;
}
</style>
