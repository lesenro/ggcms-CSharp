import { BehaviorSubject } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { URLSearchParams } from '@angular/http';
import { Title } from '@angular/platform-browser';

import 'rxjs/Rx';
import { Routes, Router } from "@angular/router";
class messageModel {
  msgType: string;
  msgData: any;
}
@Injectable()
export class AppService {
  routes: Routes;
  //取随机码
  randCode() {
    return "rnd_" + this.timeStamp();
  }
  //取时间缀
  timeStamp() {
    return ((new Date()).getTime());
  };
  pageTitle: string = "";
  setPageTitle(t: string) {
    this.pTitle.setTitle(t);
  };
  //消息对话框
  MessageBox(msg: string, title?: string, opts?: any) {
    opts = Object.assign({}, opts);
    //this.globalSubject.next({ msgType: "dialog", msgData: {} });
    let defaultOption: any = {
      message: msg || "",
      header: title || "提示",
      icon: opts.icon || "fa fa-exclamation-circle",
      btns: opts.btns || "ok|cancel",
      //curbtn: opts.curbtn||{html:"<i class='fa fa-car'></i> 自定义",func:()=>{console.log(222222222222222)}},
      curbtn: opts.curbtn,
      width: 500,
      positionTop: opts.positionTop || 20,
      accept: () => { },
      reject: () => { },
    };
    defaultOption = Object.assign(defaultOption, opts);
    this.globalSubject.next({ msgType: "dialog", msgData: defaultOption });
  }
  //显示通知
  showToaster(content: string, header?: string, type?: string, life?) {
    life = life || 3000;
    let toasterObj = { severity: type || "info", summary: header || "", detail: content, life: life };
    this.globalSubject.next({ msgType: "toaster", msgData: toasterObj });
  }
  //路由跳转
  goRouter(rname, params?: any) {
    //console.log(rname);
    if (params) {
      this.router.navigate([rname], { queryParams: params });
    } else {
      this.router.navigateByUrl(rname);
    }
    return true;
  };
  //对象转参数
  objectToParams(data: any): string {
    let params: URLSearchParams = new URLSearchParams();
    for (var key in data) {
      if (data.hasOwnProperty(key)) {
        let val = data[key];
        params.append(key, val);
      }
    }
    return params.toString();
  };
  sidebarClosed: boolean = false;
  LoginUser: any;
  //全局通知-提交消息 
  globalSubject = new BehaviorSubject<messageModel>({ msgData: {}, msgType: "" });
  //监听订阅
  globalObservable: Observable<messageModel>;
  constructor(private pTitle: Title, private router: Router) {
    this.globalObservable = this.globalSubject.asObservable();
  }
}
