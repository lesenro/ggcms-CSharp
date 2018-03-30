import { Component, OnInit,ViewChild } from '@angular/core';
import { AdminService, AppService } from "../../services";
import { FormInputComponent } from "../../form-input/form-input.component";
import { FormGroup, FormControl, FormBuilder, Validators, FormArray } from '@angular/forms';
import { FormInputOption } from '../../BaseModules';
@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settings: any[] = [];
  files: any[] = [];
  itemFormGroups: FormArray;
  constructor(private fb: FormBuilder, public appServ: AppService, private adminServ: AdminService) {
  }
  public editFormGroup: FormGroup = new FormGroup({});

  uploaded(ev) {
    if (ev.fileType == 3) {
      var idx = this.files.findIndex(item => item.propertyName == ev.propertyName);
      if (idx != -1) {
        this.files.splice(idx, 1);
      }
    }
    this.files.push(ev);
  }
  dataSave($event) {
    let cfgs: any[] = [];
    for (let item of this.settings) {
      cfgs.push({
        Id: item.Id,
        CfgName: item.CfgName,
        CfgValue: item.CfgValue,
      });
    }
    this.adminServ.SettingsSave(cfgs, this.files).then(data => {
      if (data.Code == 0) {
        for (let nitem of data.Data) {
          for (let sitem of this.settings) {
            if (nitem.CfgName == sitem.CfgName) {
              sitem.CfgValue = nitem.CfgValue;
            }
          }
        }
        this.appServ.showToaster("设置更新成功");
      }
    });
  }
  ngOnInit() {
    this.adminServ.getSettings().then(data => {
      if (data.Code == 0) {
        this.itemFormGroups = new FormArray([]);
        for (let item of data.Data.List) {
          item.option = new FormInputOption();
          item.Options = item.Options.trim() || "{}";
          let o = eval("(" + item.Options + ")");
          item.option = Object.assign(item.option, o);
          item.formGroup = FormInputComponent.buildItem(item);
          this.itemFormGroups.push(item.formGroup);
        }
        this.settings = data.Data.List;

        this.editFormGroup = this.fb.group(
          { items: this.itemFormGroups }
        );
      }
    });

  }

}
