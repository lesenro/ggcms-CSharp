import { Component, OnInit } from '@angular/core';
import { TreeNode } from "primeng/primeng";
import { AdminService } from "../../services";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  welcomeMessage = "";

  constructor(private adminServ: AdminService) {
    this.welcomeMessage = window["GgcmsServerConfig"].welcomeMessage || "欢迎使用GGCMS内容管理系统";
  }
  keyupHandlerFunction(ev) {

  }
  ngOnInit() {

  }

}
