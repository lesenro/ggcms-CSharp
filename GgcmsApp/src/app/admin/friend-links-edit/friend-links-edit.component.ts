import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "app/services";
import { Location } from '@angular/common';
@Component({
  selector: 'app-friend-links-edit',
  templateUrl: './friend-links-edit.component.html',
  styleUrls: ['./friend-links-edit.component.css']
})
export class FriendLinksEditComponent implements OnInit {
  dataInfo = {
    Id: 0,
    WebName: "",
    Url: "",
    OrderId: "0",
    Status: 0,
    LogoImg: "",
    ExtAttrib: "",
    LinkType: "",
    files: []
  };
  linkTypes = [];
  dataSave() {
    this.adminServ.FriendLinksSave(this.dataInfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      }
    });
  }
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService, private _location: Location) { }
  fileSelect(ev) {
    if (ev.Code == 0) {
      this.adminServ.fileUpload(ev.Data).then(data => {
        if (data.Code == 0) {
          let idx = this.dataInfo.files.indexOf(data.Data.url);
          if (idx != -1) {
            this.dataInfo.files.splice(idx, 1);
          }
          this.dataInfo.files.push({
            "filePath": data.Data[0].url,
            "fileType": 0,
            "propertyName": "LogoImg",
          });
          this.dataInfo.LogoImg = data.link;
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }
  ngOnInit() {
    let id = -1;
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        id = +params['id'] || 0;
        if (id > 0) {
          this.adminServ.GetFriendLinks(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
              this.dataInfo.files = [];              
            }
          });
        }
      });
    this.adminServ.GetDictionaryList(1, true, "SysFlag:0,DictType:link_type").then(data => {
      if (data.Code == 0) {
        this.linkTypes = data.Data.List;
        if (this.linkTypes.length > 0 && id === 0) {
          this.dataInfo.LinkType = this.linkTypes[0].Value;
        }
      }
    });

  }

}
