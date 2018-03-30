import { Component, OnInit } from '@angular/core';
import { AppService, AdminService } from "../../services";

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  options = {
    displayField: 'CategoryName',
    idField: "Id",
    allowDrag: true,
    allowDrop: true,
  };
  SortChange: boolean = false;
  categorys: any[] = [];
  CategorysInit(pid: any, mlist: any[]): any[] {
    let menus: any[] = [];
    for (let item of mlist) {
      if (item.ParentId == pid) {
        item.children = this.CategorysInit(item.Id, mlist);
        item.isExpanded = true;
        menus.push(item);
      }
    }
    return menus;
  }
  sortData: any[] = [];
  getCategorysSort(data: any[], pid) {
    for (let item of data) {
      let sdata = {
        Id: item.Id,
        OrderId: this.sortData.length,
        ParentId: pid,
      }
      this.sortData.push(sdata);
      if (item.children && item.children.length > 0) {
        this.getCategorysSort(item.children, item.Id);
      }
    }
  }
  SortSave() {
    this.sortData = [];
    this.getCategorysSort(this.categorys, 0);
    this.adminServ.CategorySortSave(this.sortData).then(data => {
      if (data.Code == 0) {
        this.appServ.showToaster("分类导航排序保存成功", "恭喜", "success");
        this.SortChange = false;
      }
    });
  }
  itemDelete(item) {
    this.appServ.MessageBox("是否删除：" + item.CategoryName, "删除提醒",
      {
        accept: () => {
          this.adminServ.DelCategoryInfo(item.Id).then(data => {
            if (data.Code == 0) {
              this.dataLoad();
            }
          });
        },
      });
  }
  dataLoad() {
    this.adminServ.GetCategoryList().then(data => {
      if (data.Code == 0) {
        this.categorys = this.CategorysInit(0, data.Data.List);
      }
    });
  }
  constructor(public appServ: AppService, private adminServ: AdminService) {
    this.dataLoad();
  }

  ngOnInit() {
  }

}
