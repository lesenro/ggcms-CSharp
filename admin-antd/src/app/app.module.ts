import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AppRoutingModule } from './app-routing';
import { AppComponent } from './app.component';
import { LoginComponent } from './routes/login/login.component';
import { FooterComponent } from './component/footer/footer.component';
import { AdminService, AppService, LocalStorageService } from "./services";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,
    ReactiveFormsModule ,
    NgZorroAntdModule.forRoot(),
    AppRoutingModule,
  ],
  providers: [AdminService,
    AppService,
    LocalStorageService,
    { provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
