import { Component, OnInit, EventEmitter } from '@angular/core';
import { FormInputOption } from 'app/form-input/form-input.component';


@Component({
  selector: 'modules-options',
  templateUrl: './modules-options.component.html',
  styleUrls: ['./modules-options.component.css'],
  inputs:["columnItem"],
})
export class ModulesOptionsComponent implements OnInit {
  options :FormInputOption;
  columnItem : defaultColumn ;
  columnTypes=[
    "nvarchar","int","bigint","datetime","decimal"
  ];
  constructor() { }

  ngOnInit() {
    this.columnItem=Object.assign({},defaultColumn,this.columnItem);
    let options=JSON.parse(this.columnItem.Options||"{}");
    this.options = Object.assign(new FormInputOption() ,options);
  }
}

class defaultColumn {
  ColName :string= "";
  ColTitle:string ="";
  ColType:any ="";
  Length:number =0;
  ColDecimal:number =0;
  OrderId:number  =0;
  Options:string ="";
  Module_Id:number =0;
}
