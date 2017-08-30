import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';

@Component({
  selector: 'app-adverts-edit',
  templateUrl: './adverts-edit.component.html',
  styleUrls: ['./adverts-edit.component.css']
})
export class AdvertsEditComponent implements OnInit {
  dataInfo = {
    Id: 0,
    Title: "",
    GroupKey: "",
    Content: "",
    OrderID: "0",
    Status: 0,
    Describe: "",
    Url: "",
    Image: ""
  };
  AdsGroups = [];
  dataSave() {
    this.adminServ.AdvertsSave(this.dataInfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }
  config = {
    lineNumbers: true,
    mode: "htmlmixed"
  };
  ngOnInit() {
    let id = -1;
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        id = +params['id'] || 0
        if (id > 0) {
          this.adminServ.GetAdverts(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
            }
          });
        }
      });
    this.adminServ.GetDictionaryList(1, true, "SysFlag:0,DictType:ads_group").then(data => {
      if (data.Code == 0) {
        this.AdsGroups = data.Data.List;
        if (this.AdsGroups.length > 0 && id === 0) {
          this.dataInfo.GroupKey = this.AdsGroups[0].Value;
        }
      }
    });
  }

}
