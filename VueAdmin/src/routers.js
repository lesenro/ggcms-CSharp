/* eslint-disable no-unused-vars */
import Vue from 'vue'
import App from './App.vue'
import {
  Login, Layout,
  Error404,
  Dictionary,
  Settings,
  Advert,
  Links,
  Category,
  Article
} from './pages'
import CachePages from './components/CachePages'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const Home = {
  name: "admin-home",
  template: `<div>{{$t('message.hello')}}
  <el-button @click="setLanguage('cn')">中文</el-button>
  <el-button @click="setLanguage('en')">English</el-button>
  <el-button @click="getMsg()">getMsg</el-button>
  </div>`,
  created() {
  },
  methods: {
    setLanguage(lang) {
      this.$store.commit("global/setLanguage", lang);
      this.$i18n.locale = lang;
    },
    getMsg() {
      console.log(this.$t("iot_menus.home"));
    }
  }
}
const Building = {
  name: "building",
  template: `<div :style="styles">功能建设中....  </div>`,
  data() {
    return {
      styles: {
        "font-size": "50px"
      }
    };
  },
  created() {
  },
  methods: {
    setLanguage(lang) {
      this.$store.commit("global/setLanguage", lang);
      this.$i18n.locale = lang;
    },
    getMsg() {
      console.log(this.$t("iot_menus.home"));
    }
  }
}
const routes = [
  {
    path: '/',
    component: Home,
    tabHidden: true,
    no_login: true,
    redirect: "/error-404",
  },
  { path: '/login/:role', no_login: true, component: Login, name: "登录" },
  // { path: '/province-city', component: provinceCity, name: "登录" },
  {
    path: '/admin', component: Layout,
    redirect: "/admin/home",
    tabHidden: true,
    children: [
      {
        // 当 /user/:id/profile 匹配成功，
        // UserProfile 会被渲染在 User 的 <router-view> 中
        icon: 'el-icon-house',
        path: 'home',
        iconColor: "#2196f3",
        component: Home,
        name: "管理首页",
        closable: false,
        isHome: true,
        powers: [
          {
            name: "浏览",
            keys: ["menu_21"],
            isMenu: true,
          },
        ]
      },
      {
        icon: 'el-icon-setting',
        path: 'sys',
        iconColor: "#8bc34a",
        component: CachePages,
        redirect: "/admin/sys/power",
        name: "系统管理",
        children: [
          {
            icon: 'el-icon-circle-check',
            path: 'power',
            iconColor: "#18ffff",
            component: Building,
            name: "权限管理",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_16", "/meterBaseInfo/selectPage", "/sysDictionary/selectAll", "/companyMeterRelation/selectByComId"],
                isMenu: true,
              },
              {
                name: "详情",
                keys: ["/meterBaseInfo/selectById", "/sysFactoryInfo/getById", "/companyEndBase/select4Meter"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-connection',
            path: 'tool',
            iconColor: "#ffa726",
            component: Building,
            name: "系统工具",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-files',
            path: 'file',
            iconColor: "#2d6e63",
            component: Building,
            name: "文件管理",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-user',
            path: 'user',
            iconColor: "#ff5722",
            component: Building,
            name: "用户管理",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-fork-spoon',
            path: 'role',
            iconColor: "#ffeb3b",
            component: Building,
            name: "角色管理",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-notebook-2',
            path: 'dict',
            iconColor: "#76ff03",
            component: Dictionary,
            name: "系统字典",
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
        ]

      },
      {
        icon: 'el-icon-star-off',
        iconColor: "#ff5722",
        path: 'web',
        component: CachePages,
        redirect: "/admin/web/info",
        name: "网站设置",
        children: [
          {
            icon: 'el-icon-postcard',
            path: 'info',
            iconColor: "#4fc3f7",
            component: Settings,
            name: "基本信息",
            powers: [
              {
                name: "查询",
                keys: ["menu_18", "/companyUser/selectPage", "/companyDepart/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyUser/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyUser/update", "/companyUser/selectById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyUser/del"],
                isMenu: false,
              },
              {
                name: "角色分配",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId", "/sysUserRoleRelation/add"],
                isMenu: false,
              },
              {
                name: "角色查询",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-magic-stick',
            path: 'ad',
            iconColor: "#7c4dff",
            component: Advert,
            name: "广告管理",
            powers: [
              {
                name: "查询",
                keys: ["menu_19", "/sysRole/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/sysRole/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/sysRole/update", "/sysRole/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/sysRole/del"],
                isMenu: false,
              },
              {
                name: "权限查询",
                keys: [
                  "/sysRoleResourcesRelation/getByRoleId",
                  "/sysResources/select4Com"
                ],
                isMenu: false,
              },
              {
                name: "权限编辑",
                keys: ["/sysRoleResourcesRelation/add"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-connection',
            path: 'link',
            iconColor: "#ff4081",
            component: Links,
            name: "友情链接",
            powers: [
              {
                name: "查询",
                keys: ["menu_20", "/companyDepart/getByPid"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyDepart/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyDepart/update", "/companyDepart/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyDepart/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-price-tag',
            path: 'tag',
            iconColor: "#00bcd4",
            component: Building,
            name: "全站标签",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-chat-line-square',
            path: 'code',
            iconColor: "#66bb6a",
            component: Building,
            name: "代码片段",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },
        ]

      },
      {
        icon: 'el-icon-reading',
        iconColor: "#e040fb",
        path: 'content',
        component: CachePages,
        redirect: "/admin/system/data",
        name: "分类内容",
        children: [
          {
            icon: 'el-icon-help',
            path: 'category',
            iconColor: "#eeff41",
            component: Category,
            name: "分类导航",
            powers: [
              {
                name: "查询",
                keys: ["menu_18", "/companyUser/selectPage", "/companyDepart/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyUser/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyUser/update", "/companyUser/selectById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyUser/del"],
                isMenu: false,
              },
              {
                name: "角色分配",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId", "/sysUserRoleRelation/add"],
                isMenu: false,
              },
              {
                name: "角色查询",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-guide',
            path: 'topic',
            iconColor: "#ffa726",
            component: Building,
            name: "专题导航",
            sideHidden: true,
            powers: [
              {
                name: "查询",
                keys: ["menu_19", "/sysRole/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/sysRole/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/sysRole/update", "/sysRole/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/sysRole/del"],
                isMenu: false,
              },
              {
                name: "权限查询",
                keys: [
                  "/sysRoleResourcesRelation/getByRoleId",
                  "/sysResources/select4Com"
                ],
                isMenu: false,
              },
              {
                name: "权限编辑",
                keys: ["/sysRoleResourcesRelation/add"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-tickets',
            path: 'single',
            iconColor: "#ff8a65",
            component: Building,
            sideHidden: true,
            name: "单页文章",
            powers: [
              {
                name: "查询",
                keys: ["menu_20", "/companyDepart/getByPid"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyDepart/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyDepart/update", "/companyDepart/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyDepart/del"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-document-copy',
            path: 'article',
            iconColor: "#ffeb3b",
            component: Article,
            name: "文章管理",
            powers: [
              {
                name: "查询",
                keys: ["menu_17", "/sysCountry/getByPid", "/endAddress/getByPid", "/endAddress/getById", "/companyMeterRelation/selectByComId", "/companyEndBase/select4Clinet"],
                isMenu: true,
              },
              {
                name: "表具信息绑定",
                keys: ["/companyEndBase/add"],
                isMenu: false,
              },
              {
                name: "绑定修改",
                keys: ["/companyEndBase/update"],
                isMenu: false,
              },
              {
                name: "删除绑定",
                keys: ["/companyEndBase/del"],
                isMenu: false,
              },
            ]
          },

        ]

      },
      {
        icon: 'el-icon-picture-outline',
        path: 'style',
        iconColor: "#e91e63",
        component: Building,
        name: "风格模板",
        sideHidden: true,
        powers: [
          {
            name: "查询",
            keys: ["menu_18", "/companyUser/selectPage", "/companyDepart/selectPage"],
            isMenu: true,
          },
          {
            name: "添加",
            keys: ["/companyUser/add"],
            isMenu: false,
          },
          {
            name: "修改",
            keys: ["/companyUser/update", "/companyUser/selectById"],
            isMenu: false,
          },
          {
            name: "删除",
            keys: ["/companyUser/del"],
            isMenu: false,
          },
          {
            name: "角色分配",
            keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId", "/sysUserRoleRelation/add"],
            isMenu: false,
          },
          {
            name: "角色查询",
            keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId"],
            isMenu: false,
          },
        ]
      },
      {
        icon: 'el-icon-s-operation',
        iconColor: "#009688",
        path: 'task',
        component: CachePages,
        sideHidden: true,
        redirect: "/admin/system/data",
        name: "任务管理",
        children: [
          {
            icon: 'el-icon-truck',
            path: 'run',
            iconColor: "#69f0ae",
            component: Building,
            name: "任务执行",
            powers: [
              {
                name: "查询",
                keys: ["menu_18", "/companyUser/selectPage", "/companyDepart/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyUser/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyUser/update", "/companyUser/selectById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyUser/del"],
                isMenu: false,
              },
              {
                name: "角色分配",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId", "/sysUserRoleRelation/add"],
                isMenu: false,
              },
              {
                name: "角色查询",
                keys: ["/sysResources/select4Com", "/sysUserRoleRelation/getByUserId"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-finished',
            path: 'sql',
            iconColor: "#00e5ff",
            component: Building,
            name: "SQL语句",
            powers: [
              {
                name: "查询",
                keys: ["menu_19", "/sysRole/selectPage"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/sysRole/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/sysRole/update", "/sysRole/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/sysRole/del"],
                isMenu: false,
              },
              {
                name: "权限查询",
                keys: [
                  "/sysRoleResourcesRelation/getByRoleId",
                  "/sysResources/select4Com"
                ],
                isMenu: false,
              },
              {
                name: "权限编辑",
                keys: ["/sysRoleResourcesRelation/add"],
                isMenu: false,
              },
            ]
          },
          {
            icon: 'el-icon-set-up',
            path: 'rule',
            component: Building,
            iconColor: "#7c4dff",
            name: "采集规则",
            powers: [
              {
                name: "查询",
                keys: ["menu_20", "/companyDepart/getByPid"],
                isMenu: true,
              },
              {
                name: "添加",
                keys: ["/companyDepart/add"],
                isMenu: false,
              },
              {
                name: "修改",
                keys: ["/companyDepart/update", "/companyDepart/getById"],
                isMenu: false,
              },
              {
                name: "删除",
                keys: ["/companyDepart/del"],
                isMenu: false,
              },
            ]
          },
        ]
      },
    ]
  },
  {
    path: '*',
    no_login: true,
    component: Error404
  }
];
let route_list = [];
function getRouteList(list, p) {
  list.forEach(r => {
    let path = r.path;
    if (p) {
      path = (p.fullpath || p.path) + '/' + r.path;
    }
    r.fullpath = path;
    route_list.push({
      fullpath: path,
      path: r.path,
      name: r.name,
      icon: r.icon,
      iconColor: r.iconColor,
      closable: r.closable == undefined ? true : r.closable,
      component_name: r.component.name,
      tabHidden: r.tabHidden,
      sideHidden: r.sideHidden,
      no_login: r.no_login || false,
    });
    if (r.children && r.children.length > 0) {
      getRouteList(r.children, r);
    }
  });
}
getRouteList(routes, null);

let router = new VueRouter({
  routes // (缩写) 相当于 routes: routes
});
router.beforeEach((to, from, next) => {
  let store = router.app.$store;
  let login_status = store.state.global.login_status;
  let login_name = store.state.global.login_name;
  // console.log(login_status,login_name);
  let r = route_list.find(x => x.fullpath == to.matched[0].path);
  if (r) {
    if (r.no_login || login_status) {
      next();
      return;
    }
    if (login_name) {
      next({ path: "/login/" + login_name });
    }
  }
  next();
});
router.afterEach((to, from) => {
  let store = router.app.$store;
  let win_list = store.state.global.open_windows;
  let r = route_list.find(x => x.fullpath == to.path);

  let w = win_list.find(x => x.fullpath == to.path);
  if (r) {
    if (!w) {
      if (!r.tabHidden) {
        win_list.push(r);
        store.commit('global/setOpenWindows', win_list);
      }
    }
    store.commit('global/setActiveWindow', r.fullpath);
    store.commit('global/setDocTitle', r.name);
  }
});
export default router;
