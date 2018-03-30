import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "../../services";
import { Location } from '@angular/common';
import { GgcmsDictionary } from '../../BaseModules';
@Component({
  selector: 'app-dictionary-edit',
  templateUrl: './dictionary-edit.component.html',
  styleUrls: ['./dictionary-edit.component.css']
})

export class DictionaryEditComponent implements OnInit {
  dataInfo :GgcmsDictionary=new GgcmsDictionary();
  dictTypes: GgcmsDictionary[] = [];
  defaultDictTypes: GgcmsDictionary[] = [];
  dataSave() {
    let data=Object.assign({},this.dataInfo);
    if(data.DictType.Value){
      data.DictType=data.DictType.Value;
    }
    
    this.adminServ.DictionarySave(data).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }
  search(event) {
    if(event.query.trim()){
      this.adminServ.GetDictionaryList(1,true,"Title.contains:"+event.query+";Value.contains:"+event.query).then(data => {
        if (data.Code == 0&&data.Data.Count>0) {
          this.dictTypes=data.Data.List;
        }else{
          this.dictTypes= this.defaultDictTypes.slice();
        }
      });
    }else{
      this.dictTypes= this.defaultDictTypes.slice();
      if(this.dataInfo.DictType&&this.dictTypes.filter(x=>x.Value===this.dataInfo.DictType.Value).length===0){
        this.dictTypes.push(this.dataInfo.DictType);
      }
    }
    
  }
  ngOnInit() {

    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        let id = +params['id'] || 0;
        if (id > 0) {
          this.adminServ.GetDictionaryInfo(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
              this.adminServ.GetDictionaryList(1,true,"Value:"+data.Data.DictType).then(data => {
                if (data.Code == 0&&data.Data.Count>0) {
                  this.dictTypes=data.Data.List;
                  this.dataInfo.DictType=data.Data.List[0];
                }
              });
            }
          });
        }
      });
    this.adminServ.GetDictionaryType().then(data => {
      if (data.Code == 0) {
        this.defaultDictTypes = data.Data.List;
      }
    });
  }
}
