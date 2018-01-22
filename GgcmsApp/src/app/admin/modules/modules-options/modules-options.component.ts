import { Component, OnInit, EventEmitter } from '@angular/core';

const defaultColumn={
  ColName : "",
  ColTitle:"",
  ColType:"",
  Length:0,
  ColDecimal:0,
  OrderId:0,
  Options:"",
  Module_Id:0,
}
@Component({
  selector: 'modules-options',
  templateUrl: './modules-options.component.html',
  styleUrls: ['./modules-options.component.css'],
  inputs:["columnItem"],
})
export class ModulesOptionsComponent implements OnInit {
  columnItem={};
  columnTypes=[
    "nvarchar","int","bigint","datetime","decimal"
  ];
  constructor() { }

  ngOnInit() {
    this.columnItem=Object.assign({},defaultColumn,this.columnItem);
  }

}
