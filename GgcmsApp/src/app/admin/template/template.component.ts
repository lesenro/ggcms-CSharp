import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService, AdminService } from "app/services";
import { FileListComponent } from "app/admin/file-list/file-list.component";

@Component({
  selector: 'app-template',
  templateUrl: './template.component.html',
  styleUrls: ['./template.component.css']
})
export class TemplateComponent implements OnInit {
  @ViewChild('fileList') fileList: FileListComponent;
  dataInfo: any = {};

  itemDelete(ev) {
    this.fileList.fileDelete();
  }
  fileUpload(ev) {
    if (ev.Code == 0) {
      this.adminServ.TemplateFileUpload({
        file: ev.Data,
        id: this.dataInfo.id,
      }).then(data => {
        if (data.Code == 0) {
          this.fileList.getFileList();
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }

  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        this.dataInfo = {
          id: +params['id'] || 0,
          stype: 2,
          title: this.appServ.pageTitle,
        };
      });
  }

}
