<template>
  <div id="login" :style="bgInfo.style">
    <div class="panel">
      <div
        v-for="item in list"
        :key="item.value"
        :style="item.style"
        class="ui-item"
        :class="item.className"
      >
        <img :src="item.picture" alt v-if="item.type=='picture'">
        <el-card v-else-if="item.type=='login_panel'" class="login-form">
          <div slot="header" text-center class="clearfix">
            <div>登录</div>
          </div>
          <form-generator :value="value" @change="onChange" ref="form" :settings="formSettings"></form-generator>
        </el-card>
        <div class="custom" v-else-if="item.type=='custom'" v-html="item.content"></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import FormGenerator from "../../components/FormGenerator/FormGenerator";
import Captcha from "../../components/Captcha.vue";
import { AppTools } from "@/utils/appTools";
const defaultStyle = {
  position: "absolute"
};
const adminLogin = [
  {
    label: "背景",
    value: 1,
    type: "background",
    style: JSON.stringify({
      background: "linear-gradient(#03a9f4, #259b27)"
    }),
    content: "",
    picture: "",
    animate: "",
    active: false
  },
  {
    label: "新组件",
    value: 2,
    type: "login_panel",
    style: JSON.stringify({
      top: "300px",
      height: "100%",
      width: "100%"
    }),
    content: "",
    picture: "",
    animate: "fadeInUpBig",
    active: false
  },
  {
    label: "新组件",
    value: 3,
    type: "custom",
    style: JSON.stringify({
      "font-size": "50px",
      "text-align": "center",
      width: "100%",
      color: "#fff",
      top: "200px"
    }),
    content: "管理员登录",
    picture: "",
    animate: "zoomInDown",
    active: true
  }
];
const formSettings = {
  props: {
    "label-width": "80px"
  },
  buttons: {
    hidden: true
  },
  layouts: [
    {
      key: "div",
      name: "div",
      type: "div",
      controls: [
        {
          key: "username",
          name: "账号",
          type: "text",
          itemProps: {},
          controlProps: {
            "prefix-icon": "el-icon-user"
          }
        },
        {
          key: "password",
          name: "密码",
          type: "password",
          itemProps: {},
          controlProps: {
            "prefix-icon": "el-icon-key"
          }
        }
      ]
    },
    {
      key: "row",
      name: "row",
      type: "row",
      props: {
        gutter: 15
      },
      layouts: [
        {
          key: "col-code",
          name: "col-code",
          type: "col",
          props: {
            span: 16
          },
          controls: [
            {
              key: "captcha",
              name: "验证码",
              type: "text",
              itemProps: {},
              controlProps: {
                "prefix-icon": "el-icon-finished"
              }
            }
          ]
        },
        {
          key: "col-img",
          name: "col-img",
          type: "col",
          props: {
            span: 8
          },
          controls: [
            {
              key: "img",
              name: "img",
              type: "component",
              component: Captcha
            }
          ]
        }
      ]
    },
    {
      key: "div1",
      name: "div1",
      type: "div",
      props: {
        "padding-horizontal": true
      },
      controls: [
        {
          key: "submit",
          name: "登录",
          type: "button",
          itemProps: {
            labelHidden: true
          },
          controlProps: {
            type: "primary",
            round: true,
            class: "btn-block",
            "native-type": "submit"
          }
        }
      ]
    }
  ],
  rules: {
    username: [
      { required: true, message: "请输入账号名称", trigger: "blur" },
      {
        min: 3,
        max: 20,
        message: "长度在 3 到 20 个字符",
        trigger: "blur"
      }
    ],
    password: [
      { required: true, message: "请输入账号密码", trigger: "blur" },
      {
        min: 3,
        max: 20,
        message: "长度在 3 到 20 个字符",
        trigger: "blur"
      }
    ],
    captcha: [
      { required: true, message: "请输入验证码", trigger: "blur" },
      { len: 4, message: "长度是 4 个字符", trigger: "blur" }
    ]
  }
};
export default {
  name: "login-form",
  data() {
    let self = this;
    return {
      list: [],
      value: {
        username: "",
        password: "",
        captcha: ""
      },
      formSettings: formSettings,
      bgInfo: {}
    };
  },
  created() {
    this.uiShow(adminLogin);
  },
  activated() {
    this.uiShow(adminLogin);
  },
  computed: {
    ui_list() {
      return this.list;
    }
  },
  methods: {
    ...mapActions("login", ["login"]),
    uiShow(list) {
      let bg = list.find(x => x.type == "background");
      this.$store.commit("global/setDocTitle", "登录");

      if (bg) {
        if (bg.style && typeof bg.style == "string") {
          try {
            bg.style = JSON.parse(bg.style);
          } catch (ex) {
            bg.style = {};
          }
        } else if (typeof bg.style != "object") {
          bg.style = {};
        }
        if (bg.picture) {
          bg.style["background-image"] = `url(${bg.picture})`;
        }
        this.bgInfo = bg;
      }
      list
        .filter(x => x.type != "background")
        .forEach(item => {
          item.className = item.animate ? "animated " + item.animate : "";
          let style = Object.assign({}, defaultStyle);
          if (item.style && typeof item.style == "string") {
            try {
              let tmp = JSON.parse(item.style);
              style = Object.assign({}, style, tmp);
            } catch (ex) {
              console.error(ex);
            }
            item.style = style;
          } else if (typeof item.style != "object") {
            item.style = style;
          }
        });
      this.list = list.filter(x => x.type != "background");
    },
    onChange(ev) {
      const self = this;
      if (ev.key == "submit") {
        let form = this.$refs["form"];
        let vals = form[0].formSubmit();
        if (vals) {
          this.login(vals).then(x => {
            if (!x.Id) {
              this.$set(this.value, "captcha", "");
              form.setValue("captcha", "");
              return;
            }
            this.$message({
              type: "success",
              message: "登录成功!"
            });
            this.$store.commit("global/setLoginUser", x);
            self.$router.push("/admin/home");
          });
        }
      }
    }
  }
};
</script>

<style lang="scss">
#login {
  background-size: cover;
  background-repeat: no-repeat;
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  z-index: 100;
  .ui-item {
    position: absolute;
    img {
      max-width: 100%;
    }
  }

  .login-form {
    width: 350px;
    .el-card__header {
      border-bottom: none;
      padding-top: 30px;
      padding-bottom: 5px;
      font-size: 18px;
    }
    margin-left: auto;
    margin-right: auto;
  }
  .panel {
    position: relative;
    height: 100%;
    width: 100%;
  }
}

@media only screen and (max-width: 767px) {
  #login {
    .el-col-offset-3 {
      margin-left: 0;
    }
  }
}
</style>
