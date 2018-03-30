import { forEach } from '@angular/router/src/utils/collection';
import { Component, OnInit, OnChanges, HostBinding, EventEmitter } from '@angular/core';
import { AppService } from "../../services";

@Component({
  selector: 'side-menus',
  templateUrl: './side-menus.component.html',
  styleUrls: ['./side-menus.component.css'],
  inputs: ['menuData', 'isRoot', 'isOpen'],
  outputs: ['myHeight']
})
export class SideMenusComponent implements OnInit, OnChanges {
  @HostBinding('style.height') menuHeight: any = "auto";
  myHeight = new EventEmitter<any>();
  submenuHeight: number = 0;
  openMenu: any;
  menuData: any[] = [];
  isOpen: boolean = false;
  isRoot: boolean = true;
  cssClass: string = "sub-menu";
  constructor(public appServ: AppService) {
    this.appServ.globalObservable.subscribe(data => {
      //导航完毕后获取当前菜单
      if (data.msgType == "NavigationEnd") {
        //分隔带参数的url
        let path: string = data.msgData.split("?")[0];
        let menuTags = path.split("/");
        let mtag = menuTags[menuTags.length - 1];
        for (let item of this.menuData) {
          if (path.endsWith("/" + item.Path) || item.sub.indexOf("," + mtag + ",") != -1) {
            item.active = true;
            item.opend = true;
            //通知上级菜单改变激活状态
            this.appServ.globalSubject.next({ msgType: "SidebarMenuActive", msgData: item });
          } else {
            item.active = false;
          }
        }
      }
      //改变上级菜单激活状态
      else if (data.msgType == "SidebarMenuActive") {
        let fullpath = data.msgData.fullpath + "/";
        for (let item of this.menuData) {
          let path: string = data.msgData;
          if (fullpath.indexOf("/" + item.PowerTag + "/") != -1) {
            item.opend = true;
            item.active = true;
          } else {
            item.active = false;
          }
        }
      }
    });
  }
  childrenHeightChange(ev) {
    if (this.isRoot) {
      return;
    }
    this.submenuHeight = ev;
    let nh = parseInt(this.menuHeight);
    setTimeout(() => {
      if (nh > 0) {
        if (ev > 0) {
          this.menuHeight = ((this.menuData.length * 33 + this.menuData.length + 15) + ev) + "px";
        } else {
          this.menuHeight = (this.menuData.length * 33 + this.menuData.length + 15) + "px";
        }
        this.myHeight.emit(parseInt(this.menuHeight));
      }
    }, 100);
  }
  ngOnChanges(ev) {
    if (!this.isRoot) {
      if (this.isOpen) {
        this.menuHeight = (this.menuData.length * 33 + this.menuData.length + this.submenuHeight + 15) + "px";
      } else {
        this.menuHeight = "0";
      }
      this.myHeight.emit(parseInt(this.menuHeight));
    }
  }
  ngOnInit() {
    if (this.isRoot) {
      this.cssClass = "page-sidebar-menu  page-header-fixed";
    }
  }

}
