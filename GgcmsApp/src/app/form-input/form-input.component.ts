import { Component, EventEmitter, Input, OnInit, Output, AfterViewInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { AppService, AdminService } from "../services";
import { FormInputOption, messageModel } from '../BaseModules';
import { async } from 'q';
import { Subscription } from 'rxjs';
import { debounce } from 'rxjs/operators/debounce';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.css'],
  inputs: ['option', 'value', 'name', 'placeholder', 'itemFormGroup'],
  outputs: ['valueChange', 'uploaded', 'inputChange'],
})

export class FormInputComponent implements OnInit,AfterViewInit {

  valueChange = new EventEmitter<any>();
  inputChange = new EventEmitter<any>();
  uploaded = new EventEmitter<any>();
  option: FormInputOption = new FormInputOption();
  value: any = '';
  placeholder: string = "";
  name: string = "test";
  dataSource: any;
  itemFormGroup: FormGroup = new FormGroup({ test: new FormControl() });
  inputObservable:Subscription;
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
    }else{
      this.appServ.globalSubject.next({
        msgType: "smartInputChanged", msgData: {
          name: this.name,
          event: ev,
          option: this.option,
          timeStamp:this.appServ.timeStamp()
        }
      });
    }
    
  };

  fileSelect(ev) {
    if (ev.Code == 0) {
      this.adminServ.fileUpload(ev.Data).then(data => {
        if (data.Code == 0) {
          this.value = data.link;
          this.uploaded.emit({
            "filePath": data.Data[0].url,
            "fileType": 3,
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
  };
  simpleditor = {
    plugins: [
      'advlist autolink lists link image charmap print preview hr anchor pagebreak',
      'searchreplace wordcount visualblocks visualchars code fullscreen',
      'insertdatetime media nonbreaking save table contextmenu directionality',
      'emoticons template paste textcolor colorpicker textpattern imagetools toc help'
    ],
    toolbar1: 'insert link | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent ',
    toolbar2: false,
    height: 150,
  };


  constructor(private adminServ: AdminService, public appServ: AppService) {

  }
  ngOnInit() {
    if (this.option.datasource == "sysdict") {
      this.adminServ.GetDictionaryList(1, true, "SysFlag:1,DictType:"+this.option.egroup).then(data => {
        if (data.Code == 0) {
          this.dataSource = data.Data.List;
        }
      });
    }
    if (this.option.type == "single-select-dict") {
      this.adminServ.GetDictionaryList(1, true, "DictType:"+this.option.egroup).then(data => {
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
    this.inputObservable=this.appServ.globalObservable.subscribe((data) => {
      
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
      }else if(data.msgType=="smartInputChanged"){
        if(this.option.targetName!=data.msgData.name){
          return;
        }
        if(this.option.type=="single-select-reldict"){
          if(!data.msgData.event){
            //this.value="";
            this.onChange("");
            this.dataSource=[];
            return;
          }
          this.adminServ.GetDictionaryList(1, true, "DictType:"+data.msgData.event).then(data => {
            if (data.Code == 0) {
              this.dataSource = data.Data.List;
              let found=false;
              this.dataSource.forEach(x=>{
                if(x.Value==this.value){
                  found=true;
                  return;
                }
              });
              if(!found){
                this.value="";
                this.onChange("");
              }
            }
          });  
        }
      }
    });
  }
  ngAfterViewInit() {
  }
  ngOnDestroy(){
   this.inputObservable.unsubscribe();
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
