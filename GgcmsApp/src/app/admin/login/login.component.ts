import { Component, OnInit } from '@angular/core';
import { Md5 } from 'ts-md5/dist/md5';
import { AdminService, AppService, LocalStorageService } from "app/services";
@Component({
  selector: 'admin-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  CaptchaUrl: string;
  loginInfo = {
    "username": "",
    "password": "",
    "captcha": "",
  };
  onSubmit(ev) {
    ev.preventDefault();
    var ldata = Object.assign({}, this.loginInfo);
    ldata.password = Md5.hashStr(ldata.password).toString();
    ldata.password = Md5.hashStr(ldata.password + ldata.captcha).toString();
    this.adminServ.login(ldata).then(data => {
      if (data.Code == 0) {
        this.localServ.setObject("LoginUser", data.Data);
        this.appServ.LoginUser = data.Data;
        this.appServ.goRouter("/index");
      } else {
        this.getCaptchaUrl();
        this.appServ.showToaster(data.Msg, "错误");
      }
    }, err => {
      this.getCaptchaUrl();
      this.appServ.showToaster(err);
    });
  }
  getCaptchaUrl() {
    this.loginInfo.captcha = "";
    this.CaptchaUrl = this.adminServ.ServerBaseUrl + "getCaptcha?" + this.appServ.randCode();
  }
  constructor(public adminServ: AdminService, private appServ: AppService, private localServ: LocalStorageService) {
    this.getCaptchaUrl();
  }
  tstart(ev) {
    console.log(ev);
  }
  ngOnInit() {

  }

}
