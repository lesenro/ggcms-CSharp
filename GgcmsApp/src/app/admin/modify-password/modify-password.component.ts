import { Component, OnInit } from '@angular/core';
import { Md5 } from 'ts-md5/dist/md5';
import { AppService, AdminService } from "app/services";

@Component({
  selector: 'app-modify-password',
  templateUrl: './modify-password.component.html',
  styleUrls: ['./modify-password.component.css']
})
export class ModifyPasswordComponent implements OnInit {
  dataInfo = {
    oldPassword: "",
    newPassword: "",
    rePassword: "",
  };
  constructor(public appServ: AppService, private adminServ: AdminService) { }
  dataSave() {
    var sendData = {
      oldPassword: Md5.hashStr(this.dataInfo.oldPassword).toString(),
      newPassword: Md5.hashStr(this.dataInfo.newPassword).toString(),
      rePassword: Md5.hashStr(this.dataInfo.rePassword).toString(),
    };
    this.adminServ.ModifyPassword(sendData).then(data => {
      if (data.Code == 0) {
        this.appServ.MessageBox("恭喜，密码修改成功,请重新登录。", "操作成功", {
          accept: x => {
            this.appServ.goRouter("/login");
          },
          btns:"ok",
        });
      } else {
        this.appServ.showToaster(data.Msg, "操作失败", "error");
      }
    });
  }
  ngOnInit() {
  }

}
