import { Component, OnInit } from '@angular/core';
import { AppService, AdminService } from "../../services";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  sideMenuData: any[] = [];
  sideMenuInit(pid: any, path, mlist: any[]): any[] {
    let menus: any[] = [];
    for (let item of mlist) {
      if (item.ParentId == pid && item.ShowInSidebar) {
        item.fullpath = path + "/" + item.PowerTag;
        item.children = this.sideMenuInit(item.Id, item.fullpath, mlist);
        let subMenus: string[] = [];
        for (let sub of mlist) {
          if (sub.ParentId == item.Id && !sub.ShowInSidebar) {
            subMenus.push(sub.PowerTag);
          }
        }
        item.sub = "," + subMenus.join(",") + ",";
        menus.push(item);
      }
    }
    return menus;
  }

  constructor(public appServ: AppService, private adminServ: AdminService) { }

  ngOnInit() {
    this.adminServ.GgcmsPowersList().then(data => {
      if (data.Code == 0) {
        this.sideMenuData = this.sideMenuInit(0, "", data.Data.List);
      }
    });
  }

}
