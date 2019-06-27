<template>
  <div class="rules-edit">
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
  </div>
</template>

<script>
import  {
  controlForm,
  validataOptions,
  controlValue,
} from "./form_settings";
export default {
  name: "rules-edit",
  data() {
    return {
      controlForm: new controlForm(),
      rule_value: new controlValue(),
      rule_code: "[]",
    };
  },
  computed: {},
  methods: {
    setValues(code) {
      this.rule_code = code;
      this.onRuleFormChange({
        key: "validata",
        value: "required"
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
      let rules = JSON.parse(this.rule_code);
      //过滤重复项
      rules = rules.filter(x => {
        switch (vals.validata) {
          case "required":
            if (x.required) {
              return false;
            }
            break;
          case "regexp":
            if (x.pattern !== undefined) {
              return false;
            }
            break;
          case "range":
            if (x.min !== undefined && x.max !== undefined) {
              return false;
            }
            break;
          case "min":
            if (x.min !== undefined && x.max === undefined) {
              return false;
            }
            break;
          case "max":
            if (x.max !== undefined && x.min === undefined) {
              return false;
            }
            break;
        }
        return true;
      });
      rules.push(r);
      this.rule_code = JSON.stringify(rules, null, "\t");
      this.$emit("change", this.rule_code);
    },

  }
};
</script>

<style lang="scss">
</style>
