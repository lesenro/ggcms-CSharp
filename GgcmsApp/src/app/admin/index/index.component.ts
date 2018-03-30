import { Component, OnInit } from '@angular/core';
import { AdminService, LocalStorageService, AppService } from "../../services";

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  constructor(public appServ: AppService,
    private adminServ: AdminService,
    private localServ: LocalStorageService) {
    this.appServ.LoginUser = localServ.getObject("LoginUser");
    //判断用户是否登录
    adminServ.GetLoginUser().then(data => {
      if (data.Code == 0) {
        localServ.setObject("LoginUser", data.Data);
        this.appServ.LoginUser = data.Data;
      }else{
        localServ.remove("LoginUser");
        this.appServ.LoginUser = null;
        this.appServ.goRouter("/login");
      }
    });
  }

  ngOnInit() {
  }

}
