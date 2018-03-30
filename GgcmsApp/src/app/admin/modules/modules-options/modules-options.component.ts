import {Component, OnInit, EventEmitter} from '@angular/core';
import {AppService, AdminService} from '../../../services';
import {GgcmsDictionary, FormInputOption, GgcmsModuleColumn} from '../../../BaseModules';

const inputTypes = [
  {
    name: "单行文本",
    key: "text"
  }, {
    name: "多行文本",
    key: "textarea"
  }, {
    name: "URL地址",
    key: "url"
  }, {
    name: "Email",
    key: "email"
  }, {
    name: "密码",
    key: "password"
  }, {
    name: "单选字典",
    key: "single-select-dict"
  }, {
    name: "单选联动字典",
    key: "single-select-reldict"
  }, {
    name: "文件上传",
    key: "file"
  }
];
@Component({
  selector: 'modules-options',
  templateUrl: './modules-options.component.html',
  styleUrls: ['./modules-options.component.css'],
  inputs: ["columnItem", "columns"]
})
export class ModulesOptionsComponent implements OnInit {
  columns : any[] = [];
  options : FormInputOption;
  columnItem : GgcmsModuleColumn;
  columnTypes = ["nvarchar", "int", "bigint", "datetime", "decimal"];
  inputTypes = inputTypes;
  dictTypes : GgcmsDictionary[] = [];
  defaultDictTypes : GgcmsDictionary[] = [];
  constructor(public appServ : AppService, private adminServ : AdminService) {}
  optionsStringify() {
    let options = Object.assign({}, this.options);
    if (typeof options.egroup == "object") {
      options.egroup = options.egroup.Value || "";
    }
    if (typeof options.targetName == "object") {
      options.targetName = options.targetName.ColName || "";
    }
    return JSON.stringify(options);
  }
  ngOnInit() {
    this.columnItem = Object.assign({}, GgcmsModuleColumn, this.columnItem);
    let options = JSON.parse(this.columnItem.Options || "{}");
    this.options = Object.assign(new FormInputOption(), options);
    if (options.egroup) {
      this
        .adminServ
        .GetDictionaryList(1, true, "Value:" + options.egroup)
        .then(data => {
          if (data.Code == 0 && data.Data.Count > 0) {
            this.dictTypes = data.Data.List;
            this.options.egroup = data.Data.List[0];
          }
        });
    }
    this
      .adminServ
      .GetDictionaryType()
      .then(data => {
        if (data.Code == 0) {
          this.defaultDictTypes = data.Data.List;
        }
      });
  }
  search(event) {
    if (event.query.trim()) {
      this
        .adminServ
        .GetDictionaryList(1, true, "Title.contains:" + event.query + ";Value.contains:" + event.query)
        .then(data => {
          if (data.Code == 0 && data.Data.Count > 0) {
            this.dictTypes = data.Data.List;
          } else {
            this.dictTypes = this
              .defaultDictTypes
              .slice();
          }
        });
    } else {
      this.dictTypes = this
        .defaultDictTypes
        .slice();
      if (this.options.egroup && this.dictTypes.filter(x => x.Value === this.options.egroup.Value).length === 0) {
        this
          .dictTypes
          .push(this.options.egroup);
      }
    }

  }
}
