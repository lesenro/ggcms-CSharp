import { Component, OnInit } from '@angular/core';
import { AppService, AdminService } from "app/services";

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
  sidebarSwitch() {
    this.appServ.globalSubject.next({ msgType: "sidebarToggler", msgData: {} })
  };
  constructor(public adminServ: AdminService, public appServ: AppService) { }

  ngOnInit() {
  }

}
