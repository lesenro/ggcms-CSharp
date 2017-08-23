import { Component, OnInit } from '@angular/core';
import { AppService, AdminService } from "app/services";
import { ActivatedRoute } from "@angular/router";
import { Location } from '@angular/common';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css']
})
export class CategoryEditComponent implements OnInit {
  styleList: any[] = [];
  listTmpl: any[] = [];
  articleTmpl: any[] = [];
  mobListTmpl: any[] = [];
  mobArticleTmpl: any[] = [];
  dataInfo = {
    Id: 0,
    ParentId: 0,
    CategoryName: "",
    LogoImg: "",
    StyleName: "",
    TmplName: "",
    ArticleTmplName: "",
    MobileTmplName: "",
    ArticleMobileTmplName: "",
    RedirectUrl: "",
    Keywords: "",
    Description: "",
    Content: "",
    ExtAttrib: "",
    OrderId: 0,
    PageSize: 10,
    ImgWidth: 0,
    ImgHeight: 0,
    CategoryType: 0,
    files: [],
  };

  showCategoryTree: boolean = false;
  selectedCategorys: any = { CategoryName: "顶级菜单", label: "顶级菜单", Id: 0, data: 0 };
  categorys: any[] = [];
  onStyleChange(ev) {
    var styleinfo = this.styleList.find(x => x.Folder == ev);
    if (styleinfo) {
      this.tmplLoad(styleinfo.Id);
    } else {
      this.listTmpl = [];
      this.articleTmpl = [];
      this.mobListTmpl = [];
      this.mobArticleTmpl = [];
    }
  };
  tmplLoad(styleId) {
    this.adminServ.GetTemplateList(styleId).then(data => {
      if (data.Code == 0) {
        var files = data.Data.files;
        this.listTmpl = files.filter(x => x.name.startsWith('list_'));
        this.articleTmpl = files.filter(x => x.name.startsWith('view_'));
        this.mobListTmpl = files.filter(x => x.name.startsWith('m_list_'));
        this.mobArticleTmpl = files.filter(x => x.name.startsWith('m_view_'));
      }
    });
  };
  CategorysInit(pid: any, mlist: any[]): any[] {
    let menus: any[] = [];
    for (let item of mlist) {
      if (item.Id == this.dataInfo.Id) {
        continue;
      }
      if (item.Id == this.dataInfo.ParentId) {
        this.selectedCategorys = item;
      }
      if (item.ParentId == pid) {
        item.label = item.CategoryName;
        item.data = item.Id;
        item.children = this.CategorysInit(item.Id, mlist);
        menus.push(item);
      }
    }
    return menus;
  }
  categorySelect(ev) {
    this.dataInfo.ParentId = ev.node.Id;
    this.showCategoryTree = false;
  }
  CategoryLoad() {
    this.adminServ.GetCategoryList().then(data => {
      if (data.Code == 0) {
        this.categorys = [{
          Id: 0,
          CategoryName: "顶级菜单",
          label: "顶级菜单",
          data: 0,
          expanded: true,
          children: this.CategorysInit(0, data.Data.List)
        }];
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) {

  }
  fileSelect(ev) {
    if (ev.Code == 0) {
      this.adminServ.fileUpload(ev.Data).then(data => {
        if (data.Code == 0) {
          let idx = this.dataInfo.files.indexOf(data.Data.url);
          if (idx != -1) {
            this.dataInfo.files.splice(idx, 1);
          }
          this.dataInfo.files.push({
            "filePath": data.Data[0].url,
            "fileType": 0,
            "propertyName": "LogoImg",
          });
          this.dataInfo.LogoImg = data.link;
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }
  dataSave() {
    this.adminServ.CategorySave(this.dataInfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        let id = +params['id'] || 0;
        if (id > 0) {
          this.adminServ.GetCategoryInfo(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
              this.dataInfo.files = [];
            }
            this.CategoryLoad();
            return this.adminServ.GetAllStylesList();
          }).then(data => {
            if (data.Code == 0) {
              this.styleList = data.Data.List;
              this.onStyleChange(this.dataInfo.StyleName);
            }
          });
        } else {
          this.CategoryLoad();
          this.adminServ.GetAllStylesList().then(data => {
            if (data.Code == 0) {
              this.styleList = data.Data.List;
            }
          });
        }
      });
  }

}
