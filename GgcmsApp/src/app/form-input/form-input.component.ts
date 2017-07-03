
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { AppService, AdminService } from "app/services";

@Component({
  selector: 'form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.css'],
  inputs: ['option', 'value', 'name', 'placeholder', 'itemFormGroup'],
  outputs: ['valueChange', 'uploaded', 'inputChange'],
})

export class FormInputComponent implements OnInit {

  valueChange = new EventEmitter<any>();
  inputChange = new EventEmitter<any>();
  uploaded = new EventEmitter<any>();
  option: FormInputOption = new FormInputOption();
  value: any = '';
  placeholder: string = "";
  name: string = "test";
  dataSource: any;
  itemFormGroup: FormGroup = new FormGroup({ test: new FormControl() });
  onChange(ev) {
    this.valueChange.emit(ev);
    this.inputChange.emit({
      name: this.name,
      value: ev,
      option: this.option,
    });
    if (this.option.datasource == "style" && this.option.type == "select") {
      if (this.dataSource) {
        var styleinfo = this.dataSource.find(x => x.Folder == ev);
        styleinfo = styleinfo || { Id: '' };
        this.appServ.globalSubject.next({
          msgType: "styleChange", msgData: {
            name: this.name,
            value: styleinfo.Id,
            option: this.option,
          }
        });
      }
    }
  };

  fileSelect(ev) {
    if (ev.Code == 0) {
      this.adminServ.fileUpload(ev.Data).then(data => {
        if (data.Code == 0) {
          this.value = data.link;
          this.uploaded.emit({
            "filePath": data.Data[0].url,
            "fileType": 0,
            "propertyName": this.name,
          });
          this.valueChange.emit(data.link);
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }
  richeditor = {
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
        this.uploaded.emit({
          "filePath": data.Data[0].url,
          "fileType": 1,
          "propertyName": this.name,
        });
      }
    }
    //charCounterMax: 200, // 最大输入字数,目前只支持英文字母
  };
  simpleditor = {
    heightMax: 200,
    toolbarButtons: ['bold', 'italic', 'underline', 'fontSize', 'color', 'clearFormatting', 'insertImage', 'html'],
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
        this.uploaded.emit({
          "filePath": data.Data[0].url,
          "fileType": 1,
          "propertyName": this.name,
        });
      }
    }
    //charCounterMax: 200, // 最大输入字数,目前只支持英文字母
  };


  constructor(private adminServ: AdminService, public appServ: AppService) {

  }
  ngOnInit() {
    if (this.option.datasource == "sysdict") {
      this.adminServ.GetDictionaryList(1, true, "SysFlag:1,DictType:-1").then(data => {
        if (data.Code == 0) {
          this.dataSource = data.Data.List;
        }
      });
    }
    if (this.option.datasource == "style") {
      this.adminServ.GetAllStylesList().then(data => {
        if (data.Code == 0) {
          this.dataSource = data.Data.List;
          this.onChange(this.value);
        }
      });
    }
    if (this.option.type == "switch") {
      this.value = (this.value.toLowerCase() == "true");
    }
    //全局消息提醒，对话框
    this.appServ.globalObservable.subscribe(data => {
      if (data.msgType == "styleChange") {//风格修改
        if (this.option.datasource == "template" && this.option.type == "select") {
          if (data.msgData.value) {
            this.adminServ.GetTemplateList(data.msgData.value).then(data => {
              if (data.Code == 0) {
                this.dataSource = data.Data.files.filter(x => x.name.startsWith(this.option.egroup));
              }
            });
          } else {
            this.dataSource = [];
          }
        }
      }
    });
  }
  static buildItem(item: any) {
    let itemValidator: any = {};
    let vs: any[] = [];
    if (item.option.required) {
      vs.push(Validators.required);
    }
    itemValidator[item.CfgName] = new FormControl(item.CfgValue, vs);
    return new FormGroup(itemValidator);
  }
}
export class FormInputOption {
  type: string = "text";
  required: boolean = false;
  min: number = 0;
  max: number = 0;
  minLength: number = 0;
  maxLength: number = 0;
  pattern: string = "";
  message: string = "";
  requiredMessage: string = "";
  minMessage: string = "";
  maxMessage: string = "";
  minLengthMessage: string = "";
  maxLengthMessage: string = "";
  patternMessage: string = "";
  helpMessage: string = "";
  preview: boolean = false;
  datasource: string = "";
  egroup: string = "";
  multiple: boolean = false;
  onColor: string = "info";
  offColor: string = "default";
  onText: string = "ON";
  offText: string = "OFF";
  minDate: string = "";
  minDateMessage: string = "";
  maxDate: string = "";
  maxDateMessage: string = "";
  extension: string = "";
  extensionMessage: string = "";
  fileSize: number = 0;
  fileSizenMessage: string = "";
  dependent: string = "";
}