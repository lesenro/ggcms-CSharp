import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';
@Component({
  selector: 'app-dictionary-edit',
  templateUrl: './dictionary-edit.component.html',
  styleUrls: ['./dictionary-edit.component.css']
})

export class DictionaryEditComponent implements OnInit {
  dataInfo :Dictionary=new Dictionary();
  dictTypes: Dictionary[] = [];
  defaultDictTypes: Dictionary[] = [];
  dataSave() {
    let data=Object.assign({},this.dataInfo);
    data.DictType=data.DictType.Value;
    this.adminServ.DictionarySave(data).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }
  search(event) {
    if(event.query){
      this.adminServ.GetDictionaryList(1,true,"Title:"+event.query+",Value:"+event.query).then(data => {
        if (data.Code == 0) {
          
        }else{
          this.dictTypes= this.defaultDictTypes.slice();
        }
      });
    }else{
      this.dictTypes= this.defaultDictTypes.slice();
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
class Dictionary{
  Id:number=0;
  Title:string="";
  DictType:any="";
  OrderID:number=0;
  SysFlag:number=0;
  describe:string="";
  Value:string="";
}