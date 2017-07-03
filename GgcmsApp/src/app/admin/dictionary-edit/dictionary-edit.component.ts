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
  dataInfo = {
    Id: 0,
    Title: "",
    DictType: 0,
    OrderID: 0,
    SysFlag: 0,
    describe: "",
    Value:""
  };
  dictTypes:any[]=[];

  dataSave() {
    this.adminServ.DictionarySave(this.dataInfo).then(data => {
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
          this.adminServ.GetDictionaryInfo(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
            }
          });
        }
      });
    this.adminServ.GetDictionaryType().then(data => {
      if (data.Code == 0) {
        this.dictTypes = data.Data.List;
      }
    });
  }
}
