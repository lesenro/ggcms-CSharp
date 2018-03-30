import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService, AdminService } from "../../services";

@Component({
  selector: 'app-static-file-edit',
  templateUrl: './static-file-edit.component.html',
  styleUrls: ['./static-file-edit.component.css']
})
export class StaticFileEditComponent implements OnInit {
  dataInfo: any = {};
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        this.dataInfo = {
          id: +params['id'] || 0,
          stype: 1,
          path: params['path'] || "",
          filename: params['filename'] || "",
          types: ['javascript', 'css', 'html'],
        };
      });
  }

}
