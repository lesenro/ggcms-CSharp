<template>
  <div class="edit-root">
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
        <el-table :data="value.Columns" stripe row-key="tmpId" ref="table" max-height="300px">
          <el-table-column prop="ColTitle" label="字段名称"></el-table-column>
          <el-table-column prop="ColType" label="字段类型"></el-table-column>
          <el-table-column label="操作">
            <template slot-scope="scope">
              <el-button-group>
                <el-button icon="el-icon-edit" size="mini" @click="columnEdit(scope.row)">编辑</el-button>
                <el-button
                  icon="el-icon-close"
                  type="danger"
                  size="mini"
                  @click="columnDelete(scope.row)"
                >删除</el-button>
              </el-button-group>
            </template>
          </el-table-column>
        </el-table>
      </el-col>
    </el-row>
    <div slot="footer" class="dialog-footer margin-top text-right">
      <el-button type="primary" icon="el-icon-plus" @click="columnsAdd()">添加字段</el-button>
      <el-button @click="onCancel">取 消</el-button>
      <el-button type="primary" @click="onInfoSubmit">确 定</el-button>
    </div>
    <el-dialog
      width="80%"
      append-to-body
      title="字段编辑"
      :visible.sync="columnsVisible"
      @open="columnsOpened"
    >
      <column-edit @onCancel="columnsVisible=false" @onSubmit="onColumnSubmit" ref="column-edit"></column-edit>
    </el-dialog>
  </div>
</template>

<script>
import formSettings, { moduleColumns, defaultValue } from "./form_settings";
import columnEdit from "./column-edit";
export default {
  name: "model-edit",
  data() {
    return {
      columnsVisible: false,
      formSettings: Object.assign({}, formSettings),
      value: Object.assign({}, defaultValue),
      col_value: new moduleColumns()
    };
  },
  computed: {},

  methods: {
    setValues(val) {
      let form = this.$refs["form"];
      form.resetForm();
      let v = Object.assign({}, val);
      form.setValues(v);
      this.value = v;
      let id = 1;
      this.value.Columns.forEach(x => {
        x.tmpId = id++;
      });
    },
    onCancel() {
      this.$emit("onCancel", true);
    },

    onInfoSubmit() {
      let vals = this.$refs["form"].formSubmit();
      if (!vals) {
        return;
      }
      this.$emit("onSubmit", vals);
    },

    //表单项改动事件
    onFormCtrlChange(ev) {},
    columnsOpened() {
      //字段编辑表单
      let editor = this.$refs["column-edit"];
      if (!editor) {
        setTimeout(() => {
          this.columnsOpened();
        }, 100);
        return;
      }
      editor.setValues(this.col_value);
    },

    onColumnSubmit(vals) {
      let col = this.value.Columns.find(x => (x.tmpId == vals.tmpId));
      if (col) {
        col = Object.assign(col, vals);
      } else {
        this.value.Columns.push(vals);
      }

      this.columnsVisible = false;
    },
    columnsAdd() {
      this.col_value = new moduleColumns();
      let maxid = 0;
      if (this.value.Columns.length > 0) {
        maxid = this.value.Columns.max(x => x.tmpId);
      }
      this.col_value.tmpId = maxid + 1;
      this.columnsVisible = true;
    },
    columnEdit(row) {
      this.col_value = row;
      this.columnsVisible = true;
    },
    columnDelete(row) {
      let idx = this.value.Columns.indexOf(row);
      this.value.Columns.splice(idx, 1);
    }
  },
  components: { columnEdit }
};
</script>

<style lang="scss">
</style>
