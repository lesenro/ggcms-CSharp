import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {FooterComponent} from "../../component"
import {AdminService} from '../../services';
import { Md5 } from 'blueimp-md5';
@Component({selector: 'app-login', templateUrl: './login.component.html', styleUrls: ['./login.component.css']})
export class LoginComponent implements OnInit {
  validateForm : FormGroup;
  CaptchaUrl : string = "";
  _submitForm() {
    var errs = [];
    for (const i in this.validateForm.controls) {
      this
        .validateForm
        .controls[i]
        .markAsDirty();
    }
    if (this.validateForm.invalid) {
      return false;
    }
    var ldata = {
      username:this.validateForm.controls["userName"].value,
      password:this.validateForm.controls["password"].value,
      captcha:this.validateForm.controls["captcha"].value,
    };
    ldata.password = Md5(ldata.password).toString();
    ldata.password = Md5(ldata.password + ldata.captcha).toString();
    console.log(ldata);
    this.adminServ.login(ldata).then(data => {
    }, err => {
      this.captchaChange();
    });
  }
  captchaChange() {
    this.CaptchaUrl = this
      .adminServ
      .getCaptchaUrl();
  }
  constructor(private fb : FormBuilder, private adminServ : AdminService) {
    this.captchaChange();
  }

  ngOnInit() {
    this.validateForm = this
      .fb
      .group({
        userName: [
          null,
          [Validators.required]
        ],
        password: [
          null,
          [Validators.required]
        ],
        captcha: [
          null,
          [Validators.required,Validators.maxLength(4)]
        ]
      });
  }

}
