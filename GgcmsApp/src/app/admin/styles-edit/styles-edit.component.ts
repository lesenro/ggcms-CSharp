import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AppService, AdminService } from "../../services";
import { Location } from '@angular/common';
@Component({
  selector: 'app-styles-edit',
  templateUrl: './styles-edit.component.html',
  styleUrls: ['./styles-edit.component.css']
})
export class StylesEditComponent implements OnInit {

  dataInfo = {
    Id: 0,
    StyleName: "",
    Descrip: "",
  };
  dictTypes: any[] = [];

  dataSave() {
    this.adminServ.StylesSave(this.dataInfo).then(data => {
      if (data.Code == 0) {
        this._location.back();
      } else {
        this.appServ.showToaster(data.Msg, "保存失败", "error");
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
          this.adminServ.GetStylesInfo(id).then(data => {
            if (data.Code == 0) {
              this.dataInfo = data.Data;
            }
          });
        }
      });

  }

}
