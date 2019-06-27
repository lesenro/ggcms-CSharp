<template>
  <div class="column-edit">
    <el-row :gutter="15">
      <el-col :span="8">
        <form-generator :value="col_value" ref="col-form" :settings="columnModuleForm"></form-generator>
      </el-col>

      <el-col :span="16">
        <el-tabs value="rule_code">
          <el-tab-pane label="验证代码" name="rule_code">
            <rules-edit @change="rulesChange" ref="rules-edit"/>
          </el-tab-pane>
          <el-tab-pane label="属性代码" name="prop_code">
            <props-edit @change="propsChange" ref="props-edit"/>
          </el-tab-pane>
        </el-tabs>
      </el-col>
    </el-row>
    <div slot="footer" class="dialog-footer margin-top text-right">
      <el-button @click="onCancel">取 消</el-button>
      <el-button type="primary" @click="onColumnSubmit">确 定</el-button>
    </div>
  </div>
</template>

<script>
import { moduleColumns, moduleForm } from "./form_settings";
import rulesEdit from "./rules-edit";
import propsEdit from "./props-edit";
export default {
  name: "column-edit",
  data() {
    return {
      dialogFormVisible: false,
      columnsVisible: false,
      columnModuleForm: new moduleForm(),
      col_value: new moduleColumns(),
      rule_code: "[]",
      prop_code: "{}"
    };
  },
  computed: {},
  methods: {
    setValues(o) {
      let val = Object.assign({}, o);

      //初始化验证和属性
      if (!val.Options) {
        this.rule_code = "[]";
        this.prop_code = "{}";
      } else {
        let option = JSON.parse(o.Options);
        if (option.rules) {
          this.rule_code = JSON.stringify(option.rules, null, "\t");
          delete option.rules;
        }
        val.inputType = option.type;
        delete option.type;
        this.prop_code = JSON.stringify(option, null, "\t");
      }
      let redit = this.$refs["rules-edit"];
      let pedit = this.$refs["props-edit"];
      redit.setValues(this.rule_code);
      pedit.setValues(this.prop_code);
      let form = this.$refs["col-form"];
      form.resetForm();
      form.setValues(val);
      this.col_value = val;
    },
    rulesChange(code) {
      this.rule_code = code;
    },
    propsChange(code) {
      this.prop_code = code;
    },
    onCancel() {
      this.$emit("onCancel", true);
    },

    onColumnSubmit() {
      let form = this.$refs["col-form"];
      let vals = form.formSubmit();
      if (!vals) {
        return;
      }
      let rules = JSON.parse(this.rule_code);
      let props = JSON.parse(this.prop_code);
      props.rules = rules;
      props.type = vals.inputType;
      vals.Options = JSON.stringify(props);
      this.$emit("onSubmit", Object.assign({}, vals));
    }
  },
  components: { rulesEdit, propsEdit }
};
</script>

<style lang="scss">
</style>
