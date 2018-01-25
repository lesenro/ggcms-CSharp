import {Component, OnInit, AfterViewInit, ViewChild, ElementRef} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {AppService, AdminService} from "app/services";
import {Location} from '@angular/common';
import {OverlayPanel} from 'primeng/primeng';
import {NgbTabset, NgbTab} from '@ng-bootstrap/ng-bootstrap';
import {FormGroup, FormArray, FormBuilder} from '@angular/forms';
import {FormInputOption, GgcmsAttachment} from 'app/BaseModules';
import {FormInputComponent} from 'app/form-input/form-input.component';
import { forEach } from '@angular/router/src/utils/collection';
@Component({selector: 'app-article-edit', templateUrl: './article-edit.component.html', styleUrls: ['./article-edit.component.css']})

export class ArticleEditComponent implements OnInit,
AfterViewInit {

  @ViewChild('imgPreview')overlayPanel : OverlayPanel;
  @ViewChild('preVideo')preVideo : ElementRef;
  @ViewChild('tabset')tabset : NgbTabset;
  tabModel : NgbTab;
  styleList : any[] = [];
  articleTmpl : any[] = [];
  mobArticleTmpl : any[] = [];
  dataInfo:any = {
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
    ModuleInfo:{
      Id:0,
      Columns:[],
    }
  };
  //风格改变
  onStyleChange(ev) {
    var styleinfo = this
      .styleList
      .find(x => x.Folder == ev);
    if (styleinfo) {
      this.tmplLoad(styleinfo.Id);
    } else {
      this.articleTmpl = [];
      this.mobArticleTmpl = [];
    }
  };
  //根据风格模板读取模板
  tmplLoad(styleId) {
    this
      .adminServ
      .GetTemplateList(styleId)
      .then(data => {
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
    this
      .dataInfo
      .attachments
      .push(attachInfo);
  };
  //删除附件
  delAttachment(idx) {
    this
      .dataInfo
      .attachments
      .splice(idx, 1);
  }
  preViewUrl = "";
  preViewVideo = "";
  showCategoryTree : boolean = false;
  selectedCategorys : any = {
    CategoryName: "",
    label: "",
    Id: 0,
    data: 0
  };
  categorys : any[] = [];
  categoryList : any[] = [];
  moduleInfo : any = {
    Columns: []
  };
  editFormGroup : FormGroup = new FormGroup({});
  itemFormGroups : FormArray;
  CategorysInit(pid : any, mlist : any[]) : any[] {
    let menus : any[] = [];
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
      this
        .appServ
        .showToaster("此文件无法预览");
      return;
    }
    this
      .overlayPanel
      .toggle(ev);
  }

  moduleInit(id) {
    this
      .adminServ
      .GetModules(id)
      .then(data => {
        if (data.Code == 0) {
          this.itemFormGroups = new FormArray([]);
          for (let item of data.Data.Columns) {
            item.option = new FormInputOption();
            item.Options = item.Options.trim() || "{}";
            let o = eval("(" + item.Options + ")");
            item.option = Object.assign(item.option, o);
            item.CfgName=item.ColName;
            item.CfgValue=item.Value;
            item.formGroup = FormInputComponent.buildItem(item);
            this.itemFormGroups.push(item.formGroup);
          }
          this.moduleInfo = data.Data;
          this.tabModel.title = data.Data.ModuleName;
          this.tabModel.disabled = false;

          this.editFormGroup = this
            .fb
            .group({items: this.itemFormGroups});
          this.GetModuleValue();
        }
      });
  }
  moduleDisabled() {
    this.tabModel.disabled = true;
    this.tabModel.title = "";
    this.moduleInfo={
      Columns: []
    };
  }
  categorySelect(ev) {
    this.dataInfo.Category_Id = ev.node.Id;
    this.showCategoryTree = false;
    if (ev.node.ExtModelId > 0) {
      this.moduleInit(ev.node.ExtModelId);
    } else {
      this.moduleDisabled();
    }
  }
  CategoryLoad() {
    this
      .adminServ
      .GetCategoryList()
      .then(data => {
        if (data.Code == 0) {
          this.categoryList = data.Data.List;
          this.categorys = this.CategorysInit(0, data.Data.List);
          if (this.dataInfo.Id > 0) {
            let item = this
              .categoryList
              .find(x => x.Id == this.dataInfo.Category_Id);
            if (item && item.ExtModelId > 0) {
              this.moduleInit(item.ExtModelId);
            } else {
              this.moduleDisabled();
            }
          }
        }
      });
  }
  moduleUpload(ev) {}
  editUpload(ev, name) {
    if (ev.Code == 0) {
      this
        .dataInfo
        .files
        .push({"filePath": ev.Data[0].url, "fileType": 1, "propertyName": name});
    }
  }
  fileSelect(ev, type, name) {
    if (ev.Code == 0) {
      this
        .adminServ
        .fileUpload(ev.Data)
        .then(data => {
          if (data.Code == 0) {
            if (type === 0) {
              let idx = this
                .dataInfo
                .files
                .indexOf(data.Data.url);
              if (idx != -1) {
                this
                  .dataInfo
                  .files
                  .splice(idx, 1);
              }
              this
                .dataInfo
                .files
                .push({"filePath": data.Data[0].url, "fileType": type, "propertyName": name});
              this.dataInfo[name] = data.link;
            } else if (type === 2) {
              let idx = this
                .dataInfo
                .files
                .indexOf(data.Data.url);
              if (idx != -1) {
                this
                  .dataInfo
                  .files
                  .splice(idx, 1);
              }
              this
                .dataInfo
                .files
                .push({"filePath": data.Data[0].url, "fileType": type, "propertyName": "attachments"});
              this.dataInfo.attachments[name].AttaUrl = data.link;
              this.dataInfo.attachments[name].RealName = data.Data[0].url;
            }
          }
        });
    } else {
      this
        .appServ
        .showToaster(ev.Msg);
    }
  }
  dataSave() {
    if(this.moduleInfo.Id){
      this.dataInfo.ModuleInfo={
        Id:this.moduleInfo.Id,
        Columns:[],
      };
      this.moduleInfo.Columns.forEach(item => {
        let { Id, Value } = item;
        this.dataInfo.ModuleInfo.Columns.push({
          Id,Value
        });
      });
    }
    //console.log(this.moduleInfo,this.dataInfo);
    this
      .adminServ
      .ArticleSave(this.dataInfo)
      .then(data => {
        if (data.Code == 0) {
          this
            ._location
            .back();
        }
      });
  }
  constructor(private fb : FormBuilder, private route : ActivatedRoute, public appServ : AppService, private adminServ : AdminService, private _location : Location) {}
  GetModuleValue(){
    if(this.dataInfo.Id>0){
      this.adminServ.GetGgcmsModuleValue(this.dataInfo.Id,this.dataInfo.ExtModelId)
        .then(data => {
          if (data.Code == 0) {
            data.Data=data.Data||{};
            for (let idx in this.moduleInfo.Columns) {
              let name=this.moduleInfo.Columns[idx].ColName;
              this.moduleInfo.Columns[idx].Value=data.Data[name]||"";
            }
          }
        });
      }
  }
  ngOnInit() {
    this
      .route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        let id = +params['id'] || 0;
        if (id > 0) {
          this
            .adminServ
            .GetArticleInfo(id)
            .then(data => {
              if (data.Code == 0) {
                this.dataInfo = data.Data;
                this.dataInfo.files = [];
              }
              this.CategoryLoad();
              this.GetModuleValue();
              return this
                .adminServ
                .GetAllStylesList();
            })
            .then(data => {
              if (data.Code == 0) {
                this.styleList = data.Data.List;
                this.onStyleChange(this.dataInfo.StyleName);
              }
            });
        } else {
          this.CategoryLoad();
          this
            .adminServ
            .GetAllStylesList()
            .then(data => {
              if (data.Code == 0) {
                this.styleList = data.Data.List;
              }
            });
        }
      });
  }
  ngAfterViewInit() {
    this.tabModel = this
      .tabset
      .tabs
      .find(x => x.id === "tabModel");
  }
}
