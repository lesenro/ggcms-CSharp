import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, URLSearchParams } from "@angular/http";
import { AppService, LocalStorageService } from "app/services";

const ServerUrl = window["GgcmsServerConfig"].serviceUrl + window["GgcmsServerConfig"].apiPath;
const ServerBaseUrl = window["GgcmsServerConfig"].serviceUrl;
const cacheDict = {
  CategoryList: "CategoryList",
  PowersList: "PowersList",
  DictionaryType: "DictionaryType",
  SiteSettings: "SiteSettings",
  AllStyleList: "AllStyleList",
  TemplateList: "TemplateList",
};
export class PageData {
  public columns: string = "";
  public limit: number = 10;
  public offset: number = 0;
  public pagenum: number = 1;
  public order: string = "desc";
  public sortby: string = "Id";
  public query: string = "";
  public count: number = 0;
  public maxSize: number = 5;
  public totalPage: number = 0;
}
@Injectable()
export class AdminService {
  private localServ: LocalStorageService = new LocalStorageService();
  private options: RequestOptions;
  ServerUrl: string = ServerBaseUrl;
  constructor(private http: Http, private appServ: AppService) {
    this.options = new RequestOptions();
    this.options.headers = new Headers({ 'Content-Type': 'application/json' });

    //处理跨域问题
    let _build = (<any>http)._backend._browserXHR.build;
    (<any>http)._backend._browserXHR.build = () => {
      let _xhr = _build();
      _xhr.withCredentials = true;
      return _xhr;
    };
  }
  //错误处理
  private handleError(error: any): Promise<any> {
    //this.appServ.showToaster("访问服务器失败!", "系统错误", "error");
    console.error('An error occurred', error); // for demo purposes only
    if (error.status === 401) {
      this.appServ.goRouter("/login");
    }
    return Promise.reject(error.message || error);
  }
  private comPages(pdata: PageData): PageData {
    pdata.offset = (pdata.pagenum - 1) * pdata.limit;
    return pdata;
  }
  //文件上传
  fileUpload(file) {
    var url = ServerUrl + "Common/fileUpload";
    let formData: FormData = new FormData();
    formData.append('file', file);
    formData.append('serverUrl', ServerUrl);
    return this.http.post(url, formData)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //用户登录
  login(data: any): Promise<any> {
    var url = ServerUrl + "Login/PostLogin";
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //用户登录
  logout(): Promise<any> {
    var url = ServerUrl + "Login/GetLogout";
    return this.http.get(url)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //获取登录用户
  GetLoginUser(): Promise<any> {
    var url = ServerUrl + "Login/GetLoginUser";
    return this.http.get(url)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //保存分类信息
  CategorySave(data: any): Promise<any> {
    var action = data.Id > 0 ? "Edit" : "Add";
    var url = ServerUrl + "GgcmsCategories/" + action;
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.CategoryList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //保存分类排序信息
  CategorySortSave(list: any[]): Promise<any> {
    var url = ServerUrl + "GgcmsCategories/CategorySortSave";
    return this.http.post(url, list, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.CategoryList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  DelCategoryInfo(id): Promise<any> {
    var url = ServerUrl + "GgcmsCategories/Delete/" + id;
    return this.http.delete(url)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.CategoryList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  GetCategoryInfo(id): Promise<any> {
    var url = ServerUrl + "GgcmsCategories/GetInfo/" + id;
    return this.http.get(url)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取网站导航菜单
  GetCategoryList(): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.CategoryList);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsCategories/GetList";
    var pdata = new PageData();
    pdata.query = "CategoryType:0";
    pdata.sortby = "OrderId";
    pdata.order = "asc";
    pdata.limit = 1000;
    pdata.columns = "Id,CategoryName,OrderId,ParentId";
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.CategoryList, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取权限菜单
  GgcmsPowersList(): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.PowersList);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsPowers/GetList";
    var pdata = new PageData();
    pdata.query = "PowerType:0";
    pdata.sortby = "OrderId";
    pdata.order = "asc";
    pdata.limit = 1000;
    pdata.columns = "Id,PowerName,OrderId,ParentId,PowerTag,IconClass,ShowInSidebar,PowerType,PowerParams,Path";
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.PowersList, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  ///文章相关
  //保存文章信息
  ArticleSave(data: any): Promise<any> {
    var action = data.Id > 0 ? "Edit" : "Add";
    var url = ServerUrl + "GgcmsArticles/" + action;
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  DelArticleInfo(ids: number[]): Promise<any> {
    let params: URLSearchParams = new URLSearchParams();
    params.append("query", "Id.in:" + ids.join("|"));
    var url = ServerUrl + "GgcmsArticles/MultDelete";
    var opt = new RequestOptions({
      search: params
    });
    return this.http.get(url, opt)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  GetArticleInfo(id): Promise<any> {
    var url = ServerUrl + "GgcmsArticles/GetInfo/" + id;
    return this.http.get(url)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取网站文章列表
  GetArticleList(pn: number, query?: string): Promise<any> {
    var url = ServerUrl + "GgcmsArticles/GetList";
    var pdata = new PageData();
    pdata.query = query || "Category_Id.gt:0";
    pdata.columns = "Id,Title,CreateTime,Category_Id";
    pdata = this.comPages(pdata);
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  ///系统字典相关
  //保存系统字典
  DictionarySave(data: any): Promise<any> {
    var action = data.Id > 0 ? "Edit" : "Add";
    var url = ServerUrl + "GgcmsDictionaries/" + action;
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  DelDictionaryInfo(ids: number[]): Promise<any> {
    let params: URLSearchParams = new URLSearchParams();
    params.append("query", "Id.in:" + ids.join("|"));
    var url = ServerUrl + "GgcmsDictionaries/MultDelete";
    var opt = new RequestOptions({
      search: params
    });
    return this.http.get(url, opt)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  GetDictionaryInfo(id): Promise<any> {
    var url = ServerUrl + "GgcmsDictionaries/GetInfo/" + id;
    return this.http.get(url)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  GetDictionaryType(): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.DictionaryType);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsDictionaries/GetList";
    var pdata = new PageData();
    pdata.query = "SysFlag:1,DictType:1";
    pdata.limit = 10000;
    pdata.sortby = "OrderID";
    pdata.order = "asc";
    pdata.columns = "Id,Title,describe";
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.DictionaryType, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取系统字典列表
  GetDictionaryList(pn: number, all: boolean = false, query?: string): Promise<any> {
    var url = ServerUrl + "GgcmsDictionaries/GetList";
    var pdata = new PageData();
    pdata.sortby = "DictType,OrderID";
    pdata.order = "asc,asc";
    pdata.query = query || "SysFlag:0";
    pdata.columns = "Id,Title,Value,DictType,OrderID,describe";
    if (all) {
      pdata.limit = 1000;
    } else {
      pdata = this.comPages(pdata);
    }
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  // 获取设置
  getSettings(): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.SiteSettings);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsSysConfigs/GetList";
    var pdata = new PageData();
    pdata.sortby = "OrderId";
    pdata.order = "asc";
    pdata.limit = 1000;
    pdata.query = "GroupId.gt:0";
    pdata.columns = "Id,CfgName,CfgValue,Descrip,GroupId,Options,OrderId";
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.SiteSettings, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  // 保存设置
  SettingsSave(list: any[], files: any[]): Promise<any> {
    var url = ServerUrl + "GgcmsSysConfigs/SettingsSave";
    var data = {
      list: list,
      files: files,
    };
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.SiteSettings);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //保存风格模板
  StylesSave(data: any): Promise<any> {
    var action = data.Id > 0 ? "Edit" : "Add";
    var url = ServerUrl + "GgcmsStyles/" + action;
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.AllStyleList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  DelStylesInfo(ids: number[]): Promise<any> {
    let params: URLSearchParams = new URLSearchParams();
    params.append("query", "Id.in:" + ids.join("|"));
    var url = ServerUrl + "GgcmsStyles/MultDelete";
    var opt = new RequestOptions({
      search: params
    });
    return this.http.get(url, opt)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.AllStyleList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  GetStylesInfo(id): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/GetInfo/" + id;
    return this.http.get(url)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取风格列表
  GetStylesList(pn: number, query?: string): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/GetList";
    var pdata = new PageData();
    pdata.query = query || "Id.gt:0";
    pdata.columns = "Id,StyleName,Folder,Descrip";
    pdata = this.comPages(pdata);
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //获取所有风格列表
  GetAllStylesList(): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.AllStyleList);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsStyles/GetList";
    var pdata = new PageData();
    pdata.query = "Id.gt:0";
    pdata.columns = "Id,StyleName,Folder,Descrip";
    pdata.limit = 1000;
    var params = this.appServ.objectToParams(pdata);
    return this.http.get(url, {
      search: params
    })
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.AllStyleList, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取模板文件列表
  GetTemplateList(id): Promise<any> {
    let cacheData = this.localServ.getObject(cacheDict.TemplateList + "_" + id);
    if (cacheData) {
      return new Promise(function (resolve, reject) {
        resolve(cacheData);
      });
    }
    var url = ServerUrl + "GgcmsStyles/GetTemplateList/" + id;
    //TemplateList
    return this.http.get(url)
      .toPromise()
      .then(response => {
        var jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.setObject(cacheDict.TemplateList + "_" + id, jsonData);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //获取模板文件内容
  GetTemplateInfo(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/GetTemplateInfo";
    var params = this.appServ.objectToParams(data);
    return this.http.get(url, {
      search: params,
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //保存模板文件
  TemplateFileSave(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/TemplateFileSave";
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.TemplateList + "_" + data.id);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //删除模板文件
  TemplateFileDelete(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/TemplateFileDelete";
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.TemplateList + "_" + data.id);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  //模板上传
  TemplateFileUpload(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/TemplateFileUpload";
    let formData: FormData = new FormData();
    formData.append('file', data.file);
    formData.append('id', data.id);
    return this.http.post(url, formData)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.TemplateList + "_" + data.id);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }

  //模板重命名
  TemplateFileReName(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/TemplateFileReName";
    let formData: FormData = new FormData();
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code === 0) {
          this.localServ.remove(cacheDict.TemplateList + "_" + data.id);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }

  //获取静态文件列表
  GetStaticFile(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/GetStaticFile";
    var params = this.appServ.objectToParams(data);
    return this.http.get(url, {
      search: params,
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //获取静态文件内容
  GetStaticFileInfo(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/GetStaticFileInfo";
    var params = this.appServ.objectToParams(data);
    return this.http.get(url, {
      search: params,
    })
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //保存静态文件
  StaticFileSave(data: any): Promise<any> {
    var url = ServerUrl + "GgcmsStyles/StaticFileSave";
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //删除静态文件
  StaticFileDelete(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsStyles/StaticFileDelete';
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //上传
  StaticFileUpload(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsStyles/StaticFileUpload';
    let formData: FormData = new FormData();
    formData.append('file', data.file);
    formData.append('id', data.id);
    formData.append('path', data.path);
    return this.http.post(url, formData)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //新建文件夹
  StaticFileNewDir(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsStyles/StaticFileNewDir';
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //重命名
  StaticFileReName(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsStyles/StyleImport';
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //修改密码
  ModifyPassword(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsMembers/ModifyPassword';
    return this.http.post(url, data, this.options)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  //上传
  StyleImport(data: any): Promise<any> {
    var url = ServerUrl + 'GgcmsStyles/StyleImport';
    let formData: FormData = new FormData();
    formData.append('file', data.file);
    return this.http.post(url, formData)
      .toPromise()
      .then(response => {
        let jsonData = response.json();
        if (jsonData.Code == 0) {
          this.localServ.remove(cacheDict.AllStyleList);
        }
        return jsonData;
      })
      .catch(err => this.handleError(err));
  }
  ClearCache(){
    var url = ServerUrl + 'Common/ClearCache';
    sessionStorage.clear();
    localStorage.clear();
    return this.http.get(url)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
  AppRestart(){
    var url = ServerUrl + 'Common/AppRestart';
    sessionStorage.clear();
    localStorage.clear();
    return this.http.get(url)
      .toPromise()
      .then(response => response.json())
      .catch(err => this.handleError(err));
  }
}
