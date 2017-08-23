import { Component, OnInit, EventEmitter,ElementRef,ViewChild } from '@angular/core';

@Component({
  selector: 'file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
  inputs: ['fileType', 'fileMaxSize', 'fileTypeErr', 'fileMaxSizeErr'],
  outputs: ['fileSelect']
})
export class FileUploadComponent implements OnInit {
  @ViewChild('fileInput')  fileInputElement: ElementRef;
  fileType: string = "";
  fileMaxSize: string = "";
  fileTypeErr: string = "上传文件类型错误";
  fileMaxSizeErr: string = "上传文件不能超过{size}";
  fileSelect = new EventEmitter<any>();
  regex: RegExp;
  maxSize: number;
  constructor() { }
  onClick(ev){
    this.fileInputElement.nativeElement.click();
  }
  fileSelected(ev) {
    ev.preventDefault();
    let fdata = {
      Code: 1,
      Msg: "",
      Data: {},
    }
    let fileList: FileList = ev.target.files;
    if (fileList.length > 0) {
      if (this.fileType != "") {
        if (!this.regex.test(fileList[0].type)) {
          fdata.Msg = this.fileTypeErr;
          this.fileSelect.emit(fdata);
          return;
        }
      }
      if (this.fileMaxSize != "") {
        if (this.maxSize < fileList[0].size) {
          fdata.Msg = this.fileMaxSizeErr.replace("{size}", this.fileMaxSize);
          this.fileSelect.emit(fdata);
          return;
        }
      }
      this.fileSelect.emit({
        Code: 0,
        Msg: fileList[0].name,
        Data: fileList[0],
      });
    }
  }
  ngOnInit() {
    this.regex = new RegExp(this.fileType);
    this.maxSize = parseInt(this.fileMaxSize);
    this.fileMaxSize = this.fileMaxSize.toLowerCase();
    if (this.fileMaxSize.endsWith("kb")) {
      this.maxSize = this.maxSize * 1024;
    } else if (this.fileMaxSize.endsWith("mb")) {
      this.maxSize = this.maxSize * 1024 * 1024;
    } else if (this.fileMaxSize.endsWith("gb")) {
      this.maxSize = this.maxSize * 1024 * 1024 * 1024;
    }
  }

}
