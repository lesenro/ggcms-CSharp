import { Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { AdminService, AppService } from 'app/services';
import { OverlayPanel } from 'primeng/primeng';

@Component({
  selector: 'file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.css'],
  inputs: ['title', 'stype', 'id', 'path'],
  outputs: ['onPathChange']
})
export class FileListComponent implements OnInit {
  @ViewChild('imgPreview') imgPreview: OverlayPanel;
  onPathChange = new EventEmitter<any>();
  title: string = "";
  stype: number = 0;
  id: number = 0;
  path: string = "";
  dataList: any[] = [];
  imgUrl = "";
  severPath = "";
  editLink: string = "";
  newName: string = "";
  dialogInfo = {
    display: false,
    title: "",
    type: "",
    newName: "",
    oldName: "",
  };
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService) { }
  //类型初始化
  private fileTypeInit(list: any[]): any[] {
    for (var item of list) {
      let fullPath = this.severPath + "/" + item.name;
      fullPath = fullPath.replace("//", "/");
      fullPath = this.adminServ.ServerUrl + fullPath;
      item.fullPath = fullPath;
      if (item.type == "dir") {
        item.typeName = "文件夹";
        item.typeIcon = "fa-folder-o";
      } else {
        item.typeName = "文件";
        var ext = item.name.substr(item.name.lastIndexOf(".")).toLowerCase();
        item.ext = ext;
        switch (ext) {
          case ".js":
            item.typeIcon = "fa-code";
            break;
          case ".css":
            item.typeIcon = "fa-css3";
            break;
          case ".html":
            item.typeIcon = "fa-html5";
            break;
          case ".jpg":
            item.typeIcon = "fa-image";
            break;
          case ".gif":
            item.typeIcon = "fa-image";
            break;
          case ".jpeg":
            item.typeIcon = "fa-image";
            break;
          case ".png":
            item.typeIcon = "fa-image";
            break;
          case ".bmp":
            item.typeIcon = "fa-image";
            break;
          default:
            item.typeIcon = "fa-file-o";
            break;
        }
      }
    }
  
    return list;
  }

  showDialog(name) {
    this.dialogInfo.oldName = name;
    this.dialogInfo.newName = name;
    this.dialogInfo.display = true;
    if (name) {
      this.dialogInfo.type = "rename";
      this.dialogInfo.title = "重命名";
    } else {
      this.dialogInfo.type = "newdir";
      this.dialogInfo.title = "新建文件夹";
    }
  }
  closeDialog(ev) {
    this.dialogInfo.display = false;
    //新建文件夹
    if (this.dialogInfo.type == "newdir") {
      if (this.stype == 1) {
        this.newDirStaticFiles();
      }
    }
    //重命名
    else if (this.dialogInfo.type == "rename") {
      if (this.stype == 1) {
        this.renameStaticFiles();
      } else if (this.stype == 2) {
        this.renameTemplateFiles();
      }
    }

  }

  imagePreview(ev, item) {
    this.imgUrl = item.fullPath;
    this.imgPreview.toggle(ev);
  }
  changeAll(ev) {
    var checked = ev.target.checked;
    for (let item of this.dataList) {
      item.checked = checked;
    }
  }

  fileDelete() {
    let fns: any[] = [];
    for (let item of this.dataList) {
      if (item.checked) {
        fns.push({
          name: item.name,
          type: item.type,
        });
      }
    }
    if (fns.length == 0) {
      this.appServ.showToaster("请先选择要删除的文件");
      return;
    }
    this.appServ.MessageBox("是否删除：" + fns.length + " 文件", "删除提醒",
      {
        accept: () => {
          if (this.stype == 1) {
            this.deleteStaticFiles(fns);
          } else if (this.stype == 2) {
            this.deleteTemplateFiles(fns)
          }
        },
      });
  }
  //路径切换
  openFolder(ev, item) {
    //返回上级
    if (item.type == "up") {
      this.path = this.path.substr(0, this.path.lastIndexOf("/"));
    }
    //打开下级
    else {
      this.path = (this.path == "" ? item.name : this.path + "/" + item.name);
    }
    this.onPathChange.emit(this.path);
    this.getFileList();
  }
  getFileList() {
    if (this.stype == 1) {
      this.getStaticFileList();
    } else if (this.stype == 2) {
      this.getTemplateList();
    }
  }
  ngOnInit() {
    //风格静态文件
    if (this.stype == 1) {
      this.editLink = "/index/staticFileEdit";
    } else if (this.stype == 2) {
      this.editLink = "/index/templateEdit";
    }
    this.getFileList();
  }
  //风格相关
  newDirStaticFiles() {
    var sendData = {
      id: this.id,
      path: this.path,
      newName: this.dialogInfo.newName,
    };
    this.adminServ.StaticFileNewDir(sendData).then(data => {
      if (data.Code == 0) {
        this.getFileList();
      } else {
        this.appServ.showToaster(data.Msg, "失败", "error");
      }
    });
  }
  renameStaticFiles() {
    var sendData = {
      id: this.id,
      path: this.path,
      newName: this.dialogInfo.newName,
      oldName: this.dialogInfo.oldName,
    };
    this.adminServ.StaticFileReName(sendData).then(data => {
      if (data.Code == 0) {
        this.getFileList();
      } else {
        this.appServ.showToaster(data.Msg, "失败", "error");
      }
    });
  }
  deleteStaticFiles(fns) {
    var deldata = {
      id: this.id,
      path: this.path,
      files: fns,
    };
    this.adminServ.StaticFileDelete(deldata).then(data => {
      if (data.Code == 0) {
        this.appServ.showToaster("删除成功", "成功");
        this.getFileList();
      }
    });
  }
  getStaticFileList() {
    this.adminServ.GetStaticFile({
      id: this.id,
      path: this.path,
    }).then((data) => {
      if (data.Code == 0) {
        this.severPath = data.Data.path;
        this.dataList = this.fileTypeInit(data.Data.files);
        if (this.path != "") {
          this.dataList.unshift({
            name: "..",
            type: "up",
            typeIcon: "fa-level-up",
            typeName: "返回上级",
            fullPath: ""
          });
        }
      }
    });
  }
  //模板相关

  renameTemplateFiles() {
    var sendData = {
      id: this.id,
      newName: this.dialogInfo.newName,
      oldName: this.dialogInfo.oldName,
    };
    this.adminServ.TemplateFileReName(sendData).then(data => {
      if (data.Code == 0) {
        this.getFileList();
      } else {
        this.appServ.showToaster(data.Msg, "失败", "error");
      }
    });
  }
  deleteTemplateFiles(fns) {
    var deldata = {
      id: this.id,
      files: fns,
    };
    this.adminServ.TemplateFileDelete(deldata).then(data => {
      if (data.Code == 0) {
        this.appServ.showToaster("删除成功", "成功");
        this.getFileList();
      }
    });
  }
  getTemplateList() {
    this.adminServ.GetTemplateList(this.id).then((data) => {
      if (data.Code == 0) {
        this.severPath = data.Data.path;
        this.dataList = this.fileTypeInit(data.Data.files);
      }
    });
  }
}
