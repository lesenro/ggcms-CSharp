import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';

@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.css']
})
export class ArticleEditComponent implements OnInit {
  styleList: any[] = [];
  articleTmpl: any[] = [];
  mobArticleTmpl: any[] = [];
  dataInfo = {
    Id: 0,
    Content: "",
    Title: "",
    Hits: 0,
    CreateTime: new Date(),
    TitleImg: "",
    TitleThumbnail: "",
    MemberId: 0,
    RedirectUrl: "",
    Source: "",
    SourceUrl: "",
    Keywords: "",
    Description: "",
    TmplName: "",
    StyleName: "",
    PageTitle: "",
    ExtModelId: 0,
    MobileTmplName: "",
    ShowType: 0,
    ShowLevel: 0,
    Author: "",
    Category_Id: 0,
    files: [],
  };
  onStyleChange(ev) {
    var styleinfo = this.styleList.find(x => x.Folder == ev);
    if (styleinfo) {
      this.tmplLoad(styleinfo.Id);
    } else {
      this.articleTmpl = [];
      this.mobArticleTmpl = [];
    }
  };
  tmplLoad(styleId) {
    this.adminServ.GetTemplateList(styleId).then(data => {
      if (data.Code == 0) {
        var files = data.Data.files;
        this.articleTmpl = files.filter(x => x.name.startsWith('view_'));
        this.mobArticleTmpl = files.filter(x => x.name.startsWith('m_view_'));
      }
    });
  };
  editorOption = {
    language: "zh_cn", //配置语言
    placeholderText: "请输入内容", // 文本框提示内容
    charCounterCount: true, // 是否开启统计字数
    imageUploadURL: this.adminServ.ServerUrl + "api/Common/fileUpload",
    fileUploadURL: this.adminServ.ServerUrl + "api/Common/fileUpload",
    imageUploadParams: { serverUrl: this.adminServ.ServerUrl },
    events: {
      'froalaEditor.image.uploaded': (e, editor, response) => {
        e.preventDefault();
        let data = JSON.parse(response);
        this.dataInfo.files.push({
          "filePath": data.Data[0].url,
          "fileType": 1,
          "propertyName": "Content",
        });
      }
    }
    //charCounterMax: 200, // 最大输入字数,目前只支持英文字母
  };
  showCategoryTree: boolean = false;
  selectedCategorys: any = { CategoryName: "", label: "", Id: 0, data: 0 };
  categorys: any[] = [];
  CategorysInit(pid: any, mlist: any[]): any[] {
    let menus: any[] = [];
    for (let item of mlist) {
      if (item.Id == this.dataInfo.Id) {
        continue;
      }
      if (item.Id == this.dataInfo.Category_Id) {
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
    this.dataInfo.Category_Id = ev.node.Id;
    this.showCategoryTree = false;
  }
  CategoryLoad() {
    this.adminServ.GetCategoryList().then(data => {
      if (data.Code == 0) {
        this.categorys = this.CategorysInit(0, data.Data.List);
      }
    });
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
            "propertyName": "TitleImg",
          });
          this.dataInfo.TitleImg = data.link;
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }
  dataSave() {
    this.adminServ.ArticleSave(this.dataInfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        let id = +params['id'] || 0;
        if (id > 0) {
          this.adminServ.GetArticleInfo(id).then(data => {
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
