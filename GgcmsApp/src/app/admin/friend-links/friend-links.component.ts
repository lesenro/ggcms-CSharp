import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService, PageData } from "app/services";
@Component({
  selector: 'app-friend-links',
  templateUrl: './friend-links.component.html',
  styleUrls: ['./friend-links.component.css']
})
export class FriendLinksComponent implements OnInit {
  pdata: PageData = new PageData();
  dataList: any[] = [];
  itemDelete() {
    let ids: number[] = [];
    for (let item of this.dataList) {
      if (item.checked) {
        ids.push(item.Id);
      }
    }
    if (ids.length == 0) {
      this.appServ.showToaster("请先选择要删除的记录");
      return;
    }
    this.appServ.MessageBox("是否删除：" + ids.length + " 条记录", "删除提醒",
      {
        accept: () => {
          this.adminServ.DelFriendLinks(ids).then(data => {
            if (data.Code == 0) {
              this.appServ.showToaster("删除成功");
              this.dataLoad();
            }
          });
        },
      });
  }
  changeAll(ev) {
    var checked = ev.target.checked;
    for (let item of this.dataList) {
      item.checked = checked;
    }
  }
  pageChange(ev) {
    this.appServ.goRouter('/index/links', { page: ev });
  }
  dataLoad() {
    this.adminServ.GetFriendLinksList(this.pdata.pagenum).then(data => {
      if (data.Code == 0) {
        this.dataList = data.Data.List;
        this.pdata.count = data.Data.Count;
        this.pdata.totalPage = Math.ceil(this.pdata.count / this.pdata.limit);
      }
    });
  }
  constructor( private route: ActivatedRoute,public appServ: AppService, private adminServ: AdminService) { }

  ngOnInit() {
    this.route
    .queryParams
    .subscribe(params => {
      // Defaults to 0 if no query param provided.
      this.pdata.pagenum = +params['page'] || 1;
      this.dataLoad();
    });
  }

}
