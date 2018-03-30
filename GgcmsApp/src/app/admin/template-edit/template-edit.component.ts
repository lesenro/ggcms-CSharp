import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService, AdminService } from "../../services";

@Component({
  selector: 'app-template-edit',
  templateUrl: './template-edit.component.html',
  styleUrls: ['./template-edit.component.css']
})
export class TemplateEditComponent implements OnInit {

  dataInfo: any = {};
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        this.dataInfo = {
          id: +params['id'] || 0,
          stype: 2,
          filename: params['filename'] || "",
        };
      });
  }

}
