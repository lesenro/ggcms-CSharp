import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService, AdminService } from "app/services";
import 'codemirror/mode/javascript/javascript';
import 'codemirror/mode/css/css';
import 'codemirror/mode/htmlmixed/htmlmixed';
import { CodemirrorComponent } from "ng2-codemirror/lib";
import { Location } from '@angular/common';

@Component({
  selector: 'file-edit',
  templateUrl: './file-edit.component.html',
  styleUrls: ['./file-edit.component.css'],
  inputs: ['title', 'stype', 'id', 'path', 'types', 'filename']
})
export class FileEditComponent implements OnInit {
  @ViewChild('content') codemirror: CodemirrorComponent;
  config = {
    lineNumbers: true,
    mode: "htmlmixed"
  };
  dataInfo: any = {
    content: '',
    filename: '',
    ftype: '',
  };
  title: string = "";
  stype: number = 0;
  //此id是风格id，文件没有id
  id: number = 0;
  path: string = "";
  types: string[] = [];
  filename: string = "";
  onTypeChange(ev) {
    if (ev == "html") {
      this.config.mode = "htmlmixed";
    } else {
      this.config.mode = ev;
    }
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }
  dataSave() {
    if (this.stype == 1) {
      this.saveStaticFile();
    } else if (this.stype == 2) {
      this.saveTemplate();
    }
  }
  private getFilePath(): string {
    var ext = this.dataInfo.ftype;
    if (ext != "") {
      ext = "." + ext;
    }
    if (ext == ".javascript") {
      ext = ".js";
    }
    var file = this.dataInfo.filename + ext;
    file = this.path == "" ? file : this.path + "/" + file;
    return file;
  }
  ngOnInit() {
    this.dataInfo.filename = this.filename;
    //风格静态文件
    if (this.stype == 1) {
      this.getStaticFileInfo();
    } else if (this.stype == 2) {
      this.getTemplateInfo();
    }
  }
  getTemplateInfo() {
    if (this.dataInfo.filename) {
      this.adminServ.GetTemplateInfo({
        id: this.id,
        filename: this.dataInfo.filename,
      }).then((data) => {
        this.dataInfo.content = data.Data;
      });
    }
  }
  saveTemplate() {
    var file = this.dataInfo.filename;
    if (file.indexOf(".") == -1) {
      file = file + ".cshtml";
    }
    this.adminServ.TemplateFileSave({
      id: this.id,
      filename: file,
      content: this.dataInfo.content,
    }).then((data) => {
      if (data.Code == 0) {
        this._location.back();
      } else {
        this.appServ.showToaster(data.Msg, "保存失败", "error");
      }
    });
  }
  getStaticFileInfo() {
    let file = this.getFilePath();
    if (this.dataInfo.filename) {
      var ext = this.dataInfo.filename.substr(this.dataInfo.filename.lastIndexOf(".")).toLowerCase();
      ext = ext.replace(".", "");
      if (ext == "js") {
        ext = "javascript";
      }
      this.onTypeChange(ext);
      this.adminServ.GetStaticFileInfo({
        id: this.id,
        path: file,
      }).then((data) => {
        this.dataInfo.content = data.Data;
      });
    }
  }
  saveStaticFile() {
    var file = this.getFilePath();
    this.adminServ.StaticFileSave({
      id: this.id,
      path: file,
      content: this.dataInfo.content,
    }).then((data) => {
      if (data.Code == 0) {
        this._location.back();
      } else {
        this.appServ.showToaster(data.Msg, "保存失败", "error");
      }
    });
  }
}
