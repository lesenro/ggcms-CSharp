<template>
  <div class="article-content">
    <el-form size="mini" ref="form" :model="info" label-width="100px">
      <el-form-item>
        <div class="page-toolbar">
          <el-button-group>
            <el-tooltip placement="top" content="追加">
              <el-button size="mini" @click="addPage" icon="el-icon-plus" type="success"></el-button>
            </el-tooltip>
            <el-tooltip placement="top" content="插入">
              <el-button size="mini" @click="insPage" icon="el-icon-download" type="primary"></el-button>
            </el-tooltip>
            <el-tooltip placement="top" content="删除">
              <el-button size="mini" @click="delPage" icon="el-icon-close" type="danger"></el-button>
            </el-tooltip>
            <el-tooltip placement="top" content="左移">
              <el-button size="mini" @click="movePage(-1)" icon="el-icon-d-arrow-left"></el-button>
            </el-tooltip>
            <el-tooltip placement="top" content="右移">
              <el-button size="mini" @click="movePage(1)" icon="el-icon-d-arrow-right"></el-button>
            </el-tooltip>
          </el-button-group>
          <el-pagination
            background
            layout="prev, pager, next"
            :page-count="pageCount"
            @current-change="currentChange"
            :current-page="cur_page"
          ></el-pagination>
        </div>
      </el-form-item>
      <el-form-item label="分页标题">
        <el-input v-model="info.Title"></el-input>
      </el-form-item>
      <el-form-item label="内容">
        <vue-editor
          ref="editor"
          v-model="info.Content"
          :config="editorOptions"
          @imageBeforeUpload="imageUpload"
        />
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";

export class GgcmsArticlePages {
  Id = 0;
  OrderId = 1;
  Content = "";
  Title = "";
  Article_Id = "";
  files = [];
  state = EntityState.Detached;
  isReady = true;
}
export const EntityState = {
  Detached: 1,
  Unchanged: 2,
  Added: 4,
  Deleted: 8,
  Modified: 16
};
export default {
  name: "article-content",
  mounted() {},
  updated() {},
  props: {
    value: Array
  },
  data() {
    let self = this;
    return {
      info: new GgcmsArticlePages(),
      d_value: this.value,
      cur_page: 1,
      articleId: 0,
      editorOptions: {
        height: 300
      }
    };
  },
  computed: {
    pageCount() {
      return this.d_value.where(x => x.state != EntityState.Deleted).length;
    }
  },
  methods: {
    ...mapActions("article", ["getPageInfoById"]),
    ...mapActions("global", ["fileUpload"]),
    editorInit(ev) {},
    setValues(val, aid) {
      this.articleId = aid;
      this.$set(this, "d_value", val);
      // this.d_value = val;
      this.currentChange(1);
    },
    async imageUpload(files, editor, callback) {
      let file = files[0];
      if (!file.type.startsWith("image")) {
        this.$message({
          type: "error",
          message: "必须上传图片"
        });
        return;
      }
      let result = await this.fileUpload({
        type: "article",
        file: file
      });

      if (result.Code != 0) {
        return;
      }
      callback(result.link, file);
      if (!this.info.files) {
        this.info.files = [];
      }

      this.info.files.push({
        filePath: result.Data[0].url,
        propertyName: "Content",
        fileType: 1
      });
    },
    currentChange(ev) {
      this.cur_page = ev;
      let info = this.d_value.find(
        x => x.state != EntityState.Deleted && x.OrderId == ev
      );
      if (!info) {
        return;
      }
      if (info.Id > 0 && !info.isReady) {
        this.getPageInfoById({
          pid: info.Id
        }).then(x => {
          info = Object.assign(info, x);
          info.files = [];
          info.OrderId = ev;
          info.isReady = true;
          info.state = EntityState.Modified;
          this.info = info;
        });
      } else {
        this.info = info;
      }
      // this.$set(this.info,"Content",info.Content);
    },
    addPage() {
      let info = new GgcmsArticlePages();
      this.info = info;
      this.d_value.push(info);
      this.info.OrderId = this.pageCount;
      this.cur_page = this.pageCount;
    },
    delPage() {
      if (this.pageCount == 1) {
        return;
      }
      let info = this.d_value.find(
        x => x.state != EntityState.Deleted && x.OrderId == this.cur_page
      );
      if (info.Id > 0) {
        info.state = EntityState.Deleted;
      } else {
        let idx = this.d_value.indexOf(info);
        this.d_value.splice(idx, 1);
      }
      this.d_value
        .where(x => x.state != EntityState.Deleted && x.OrderId > this.cur_page)
        .forEach(page => {
          page.OrderId = page.OrderId - 1;
          if (page.Id > 0 && page.state == EntityState.Detached) {
            page.state = EntityState.Unchanged;
          }
        });
      if (this.pageCount < this.cur_page) {
        this.currentChange(this.pageCount);
      } else {
        this.currentChange(this.cur_page);
      }
    },
    insPage() {
      this.d_value
        .where(
          x => x.state != EntityState.Deleted && x.OrderId >= this.cur_page
        )
        .forEach(page => {
          page.OrderId = page.OrderId + 1;
          if (page.Id > 0 && page.state == EntityState.Detached) {
            page.state = EntityState.Unchanged;
          }
        });
      let info = new GgcmsArticlePages();
      this.info = info;
      this.info.OrderId = this.cur_page;
      this.d_value.splice(this.cur_page - 1, 0, info);
    },
    movePage(step) {
      let target = this.cur_page + step;
      if (target < 1 || target > this.pageCount) {
        return;
      }
      let info = this.d_value.find(
        x => x.state != EntityState.Deleted && x.OrderId == this.cur_page
      );
      let tInfo = this.d_value.find(
        x => x.state != EntityState.Deleted && x.OrderId == target
      );
      tInfo.OrderId = this.cur_page;
      info.OrderId = target;
      this.cur_page = target;
      this.currentChange(target);
    }
  }
};
</script>

<style lang="scss">
.page-toolbar {
  .el-pagination {
    display: inline-block;
    line-height: 25px;
  }
}
</style>
