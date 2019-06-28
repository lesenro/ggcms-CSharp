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
          id="editor"
          useCustomImageHandler
          ref="editor"
          v-model="info.Content"
          @imageAdded="editorImageAdded"
        ></vue-editor>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { VueEditor } from "vue2-editor";
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
    value: Array,
    aid: Number
  },
  data() {
    return {
      info: new GgcmsArticlePages(),
      d_value: this.value,
      cur_page: 1
    };
  },
  computed: {
    articleId() {
      return this.aid;
    },
    pageCount() {
      return this.d_value.where(x => x.state != EntityState.Deleted).length;
    }
  },
  methods: {
    ...mapActions("article", ["getPageInfo"]),
    setValues(val) {
      this.$set(this, "d_value", val);
      // this.d_value = val;
      this.currentChange(1);
    },
    async editorImageAdded(file, Editor, cursorLocation, resetUploader) {},
    currentChange(ev) {
      this.cur_page = ev;
      let info = this.d_value.find(
        x => x.state != EntityState.Deleted && x.OrderId == ev
      );
      if (!info) {
        return;
      }
      this.info = info;
      if (this.info.Id > 0 && !this.info.isReady) {
      }
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
        x => x.state != EntityState.Deleted && x.OrderId == ev
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
  },
  components: { VueEditor }
};
</script>

<style lang="scss">
.page-toolbar {
  .el-pagination {
    display: inline-block;
    line-height: 25px;
  }
}
#editor {
  height: 250px;
}
</style>
