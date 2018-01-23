import { Component, OnInit, EventEmitter, ViewChild } from '@angular/core';

import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { ModulesOptionsComponent } from '../modules-options/modules-options.component';
import { AppService } from 'app/services';
@Component({
  selector: 'modules-columns',
  templateUrl: './modules-columns.component.html',
  styleUrls: ['./modules-columns.component.css'],
  inputs:["columns"],
  outputs:["columnsChange"]

})
export class ModulesColumnsComponent implements OnInit {

  columnsChange = new EventEmitter<any>();
  columns=[];
  closeResult: string;
  columnItem=null;
  
  onChange(ev){
    this.columnsChange.emit(this.columns);
  }
  deleteColumns(item){
    this.columns=this.columns.filter(x=>x.Id!=item.Id);
  }
  open(content,item) {
    this.columnItem=item||{isnew:true};
    this.modalService.open(content,{size:"lg" }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      let rtype=typeof result;
      if(rtype==="object"){
        let item=result.columnItem;
        item.Options=result.optionsStringify();
        console.log(item.Options);
        if(item.isnew){
          if(this.columns.filter(x=>x.ColTitle==item.ColTitle)){
            this.appServ.showToaster("字段名不能重复","输入错误","warn");
            return false;
          }
          delete item.isnew;
          item.Id=this.appServ.timeStamp();
          this.columns.push(item);
        }else{
          if(this.columns.filter(x=>x.ColTitle==item.ColTitle&&x.Id!=item.Id).length>0){
            this.appServ.showToaster("字段名不能重复","输入错误","warn");
            return false;
          }
          for(var i in this.columns){
            if(this.columns[i].Id==item.Id){
              this.columns[i]=item;
              break;
            }
          }
        }
        this.onChange(item);
      }      
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }
  constructor(private modalService: NgbModal,private appServ: AppService) { }

  ngOnInit() {
    
  }

}
