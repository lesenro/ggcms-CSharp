import { Component, HostBinding } from '@angular/core';
import { Observable, BehaviorSubject } from "rxjs/Rx";
import { AppService, AdminService, LocalStorageService } from "app/services";
import { ConfirmationService } from "primeng/primeng";

@Component({
  selector: 'body',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  @HostBinding('class') public cssClass = "";
  private baseCss = 'page-header-fixed page-sidebar-closed-hide-logo page-content-white page-md';
  msgs: string[];
  value = new Date();
  toasterLife = 3000;
  dialogOption: any = {
    btns: "",
    width: 500,
  };
  constructor(public appServ: AppService,
    private dialogServ: ConfirmationService
  ) {
    this.cssClass = this.baseCss;
    //全局消息提醒，对话框
    this.appServ.globalObservable.subscribe(data => {
      if (data.msgType == "toaster") {//消息
        this.msgs = [];
        this.toasterLife = data.msgData.life;
        this.msgs.push(data.msgData);
      } else if (data.msgType == "dialog") {//对话框
        this.dialogOption = data.msgData;
        this.dialogServ.confirm(data.msgData);
      } else if (data.msgType == "sidebarToggler") {//侧边菜单
        this.appServ.sidebarClosed = !this.appServ.sidebarClosed;
        if (this.appServ.sidebarClosed) {
          this.cssClass = this.baseCss + " page-sidebar-closed";
        } else {
          this.cssClass = this.baseCss;
        }
      }
    });
  }
}
