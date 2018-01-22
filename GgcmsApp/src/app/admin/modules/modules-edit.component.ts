import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';

@Component({
  selector: 'app-modules-edit',
  templateUrl: './modules-edit.component.html',
  styleUrls: ['./modules-edit.component.css']
})
export class ModulesEditComponent implements OnInit {
  dataInfo = {
    Id: 0,
    ModuleName: "",
    Describe: "",
    TableName: "",
    ViewName: "",
    Columns:[],
  };
  dataSave() {
    let dinfo=Object.assign({},this.dataInfo);
    let d=this.appServ.timeStamp()-1000*60*60*24*100;
    for(var i in dinfo.Columns){

      if(dinfo.Columns[i].Id>1516608277456){
        dinfo.Columns[i].Id=0;
      }
    }
    this.adminServ.ModulesSave(dinfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }

  ngOnInit() {
    let id = -1;
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        id = +params['id'] || 0
        if (id > 0) {
          this.adminServ.GetModules(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
            }
          });
        }
      });
  }

}
