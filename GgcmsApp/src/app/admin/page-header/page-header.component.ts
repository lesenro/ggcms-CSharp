import { Component, OnInit } from '@angular/core';
import {AppService,AdminService } from "../../services";
@Component({
  selector: 'page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.css']
})
export class PageHeaderComponent implements OnInit {

  constructor(public appServ: AppService, private adminServ:AdminService) { }

  ngOnInit() {
  }

}
