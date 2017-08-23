import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';
import {
  OverlayPanel,
} from 'primeng/primeng';
@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.css']
})

export class ArticleEditComponent implements OnInit {

  @ViewChild('imgPreview') overlayPanel: OverlayPanel;
  @ViewChild('preVideo') preVideo: ElementRef;
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
    attachments: [],
  };
  //风格改变
  onStyleChange(ev) {
    var styleinfo = this.styleList.find(x => x.Folder == ev);
    if (styleinfo) {
      this.tmplLoad(styleinfo.Id);
    } else {
      this.articleTmpl = [];
      this.mobArticleTmpl = [];
    }
  };
  //根据风格模板读取模板
  tmplLoad(styleId) {
    this.adminServ.GetTemplateList(styleId).then(data => {
      if (data.Code == 0) {
        var files = data.Data.files;
        this.articleTmpl = files.filter(x => x.name.startsWith('view_'));
        this.mobArticleTmpl = files.filter(x => x.name.startsWith('m_view_'));
      }
    });
  };
  //添加附件
  addAttachment() {
    var attachInfo = new GgcmsAttachment();
    this.dataInfo.attachments.push(attachInfo);
  };
  //删除附件
  delAttachment(idx) {
    this.dataInfo.attachments.splice(idx, 1);
  }
  preViewUrl = "";
  preViewVideo = "";
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
  filePreview(ev, url) {
    this.preViewUrl = "";
    this.preViewVideo = "";
    var picRgx = /(.jpg|.JPG|.gif|.GIF)$/i;
    var videoRgx = /(.mp4|.ogg)$/i;
    if (picRgx.test(url)) {
      this.preViewUrl = url;
    } else if (videoRgx.test(url)) {
      this.preViewVideo = url;
    } else {
      this.appServ.showToaster("此文件无法预览");
      return;
    }
    this.overlayPanel.toggle(ev);
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
  editUpload(ev, name) {
    if (ev.Code == 0) {
      this.dataInfo.files.push({
        "filePath": ev.Data[0].url,
        "fileType": 1,
        "propertyName": name,
      });
    }
  }
  fileSelect(ev, type, name) {
    if (ev.Code == 0) {
      this.adminServ.fileUpload(ev.Data).then(data => {
        if (data.Code == 0) {
          if (type === 0) {
            let idx = this.dataInfo.files.indexOf(data.Data.url);
            if (idx != -1) {
              this.dataInfo.files.splice(idx, 1);
            }
            this.dataInfo.files.push({
              "filePath": data.Data[0].url,
              "fileType": type,
              "propertyName": name,
            });
            this.dataInfo[name] = data.link;
          } else if (type === 2) {
            let idx = this.dataInfo.files.indexOf(data.Data.url);
            if (idx != -1) {
              this.dataInfo.files.splice(idx, 1);
            }
            this.dataInfo.files.push({
              "filePath": data.Data[0].url,
              "fileType": type,
              "propertyName": "attachments",
            });
            this.dataInfo.attachments[name].AttaUrl = data.link;
            this.dataInfo.attachments[name].RealName = data.Data[0].url;
          }
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
class GgcmsAttachment {
  Id = 0;
  AttaTitle = "";
  AttaUrl = "";
  Describe = "";
  RealName = "";
}