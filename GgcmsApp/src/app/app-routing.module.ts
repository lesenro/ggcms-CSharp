import { NgModule, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes, Router, NavigationEnd, NavigationStart, ActivatedRoute } from '@angular/router';
import { AppService } from "app/services";
import { TemplateComponent,FriendLinksEditComponent, FriendLinksComponent, TemplateEditComponent, LoginComponent, ArticleEditComponent, ArticleComponent, HomeComponent, IndexComponent, CategoryComponent, CategoryEditComponent, DictionaryComponent, DictionaryEditComponent, SettingsComponent, StylesComponent, StylesEditComponent, StaticFileComponent, StaticFileEditComponent, ModifyPasswordComponent } from "app/admin";
import { Observable } from 'rxjs/Rx';

let routes: Routes = [
  {
    path: '', redirectTo: '/index', pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      "title": "登录"
    },
  },
  {
    path: 'index',
    component: IndexComponent,
    children: [
      {
        path: '', redirectTo: 'home', pathMatch: 'full'
      },
      {
        path: 'home',
        component: HomeComponent,
        data: {
          "title": "首页"
        },
      },
      {
        path: 'power',
        component: HomeComponent,
        data: {
          "title": "权限管理"
        },
      },
      {
        path: 'category',
        component: CategoryComponent,
        data: {
          "title": "分类导航"
        },
      },
      {
        path: 'categoryEdit',
        component: CategoryEditComponent,
        data: {
          "title": "分类导航编辑"
        },
      },
      {
        path: 'topic',
        component: HomeComponent,
        data: {
          "title": "专题管理"
        },
      },
      {
        path: 'article',
        component: ArticleComponent,
        data: {
          "title": "文章列表"
        },
      },
      {
        path: 'articleEdit',
        component: ArticleEditComponent,
        data: {
          "title": "文章编辑"
        },
      },
      {
        path: 'dictionary',
        component: DictionaryComponent,
        data: {
          "title": "系统字典列表"
        },
      },
      {
        path: 'dictionaryEdit',
        component: DictionaryEditComponent,
        data: {
          "title": "系统字典编辑"
        },
      },
      {
        path: 'settings',
        component: SettingsComponent,
        data: {
          "title": "基本信息设置"
        },
      },
      {
        path: 'styles',
        component: StylesComponent,
        data: {
          "title": "风格模板管理"
        },
      },
      {
        path: 'stylesEdit',
        component: StylesEditComponent,
        data: {
          "title": "风格模板设置"
        },
      },
      {
        path: 'staticFile',
        component: StaticFileComponent,
        data: {
          "title": "风格静态文件列表"
        },
      },
      {
        path: 'staticFileEdit',
        component: StaticFileEditComponent,
        data: {
          "title": "风格静态文件编辑"
        },
      },
      {
        path: 'template',
        component: TemplateComponent,
        data: {
          "title": "模板文件列表"
        },
      },
      {
        path: 'templateEdit',
        component: TemplateEditComponent,
        data: {
          "title": "模板文件编辑"
        },
      },
      {
        path: 'subsub',
        component: HomeComponent,
        data: {
          "title": "文章管理"
        },
      },
      {
        path: 'modifyPassword',
        component: ModifyPasswordComponent,
        data: {
          "title": "密码修改"
        },
      },
      {
        path: 'links',
        component: FriendLinksComponent,
        data: {
          "title": "友情链接"
        },
      },
      {
        path: 'linksEdit',
        component: FriendLinksEditComponent,
        data: {
          "title": "友情链接编辑"
        },
      }
    ]
  },];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule {
  constructor(private router: Router,
    private actRoute: ActivatedRoute,
    private appServ: AppService) {
    this.appServ.routes = routes;
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (event.id == 1) {
          //第一次需要延时
          var delay = setTimeout(() => {
            this.appServ.globalSubject.next({ msgType: "NavigationEnd", msgData: event.url });
            clearTimeout(delay);
          }, 1000);
        } else {
          this.appServ.globalSubject.next({ msgType: "NavigationEnd", msgData: event.url });
        }
        this.navigationEnd();
      }
    });
  }
  private navigationEnd() {

    //获取当前路由，和上级路由
    let currentRoute = this.actRoute.root;

    while (currentRoute.children[0] !== undefined) {
      currentRoute = currentRoute.children[0];
    }
    let parentPath: string = "";
    if (currentRoute.snapshot.parent.routeConfig) {
      parentPath = currentRoute.snapshot.parent.routeConfig.path || "index";
    }
    let currentPath: string = currentRoute.snapshot.routeConfig.path || "";
    //获取当前路由标题
    let ptitle = currentRoute.snapshot.data.title || "";
    //设置浏览器标题
    this.appServ.pageTitle = ptitle;
    this.appServ.setPageTitle(ptitle);
    //未登录->登录
    // if (!this.appGlobal.StudentInfo) {
    //   this.appSvr.goRouter("/login");
    // }
  }
}