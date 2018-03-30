import { Component, OnInit } from '@angular/core';
import { AppService, AdminService } from "../../services";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  logout() {
    this.adminServ.logout().then((data) => {
      if (data.Code == 0) {
        this.appServ.goRouter("/login");
      }
    });
  };
  clearCache() {
    this.appServ.MessageBox("是否要清理系统缓存?", "清理提醒",
      {
        accept: () => {
          this.adminServ.ClearCache().then(data => {
            if (data.Code == 0) {
              this.appServ.showToaster("清理成功");
            }
          });
        },
      });
  }
  appRestart() {
    this.appServ.MessageBox("重启应用需要重新登录，是否继续?", "清理提醒",
      {
        accept: () => {
          this.adminServ.AppRestart().then(data => {
            if (data.Code == 0) {
              this.appServ.showToaster("请重新登录","重启成功");
              this.appServ.goRouter("/login");
            }
          });
        },
      });
  }
  sidebarSwitch() {
    this.appServ.globalSubject.next({ msgType: "sidebarToggler", msgData: {} })
  };
  constructor(public adminServ: AdminService, public appServ: AppService) { }

  ngOnInit() {
  }

}
