import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService, AdminService } from "app/services";
import { FileListComponent } from "app/admin/file-list/file-list.component";

@Component({
  selector: 'app-static-file',
  templateUrl: './static-file.component.html',
  styleUrls: ['./static-file.component.css']
})
export class StaticFileComponent implements OnInit {
  @ViewChild('fileList') fileList: FileListComponent;
  dataInfo: any = {};
  constructor(private route: ActivatedRoute, public appServ: AppService, private adminServ: AdminService) { }
  pathChange(ev) {
    this.dataInfo.path = ev;
    this.appServ.goRouter("/index/staticFile", { id: this.dataInfo.id, path: ev });
  }
  itemDelete(ev) {
    this.fileList.fileDelete();
  }
  fileUpload(ev) {
    if (ev.Code == 0) {
      this.adminServ.StaticFileUpload({
        file: ev.Data,
        id: this.dataInfo.id,
        path: this.dataInfo.path,
      }).then(data => {
        if (data.Code == 0) {
          this.fileList.getFileList();
        }
      });
    } else {
      this.appServ.showToaster(ev.Msg);
    }
  }
  createDir(ev) {
    this.fileList.showDialog("");
  }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        this.dataInfo = {
          id: +params['id'] || 0,
          path: params['path'] || "",
          stype: 1,
          title: this.appServ.pageTitle,
        };
      });
  }

}
