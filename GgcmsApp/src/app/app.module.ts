import { NgArrayPipesModule } from 'angular-pipes/src';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NgpModule } from "app/ngp-module/ngp-module.module";
import { AdminService, AppService, LocalStorageService } from "app/services";
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { HeaderComponent } from './admin/header/header.component';
import { FooterComponent } from './admin/footer/footer.component';
import { SidebarComponent } from './admin/sidebar/sidebar.component';
import { PageHeaderComponent } from './admin/page-header/page-header.component';
import { SideMenusComponent } from './admin/side-menus/side-menus.component';
import { AdvertsComponent,AdvertsEditComponent,LoginComponent, ArticleEditComponent, ArticleComponent, HomeComponent, IndexComponent, CategoryComponent, CategoryEditComponent } from "app/admin";
import { TreeModule } from 'angular-tree-component';

import { DictionaryComponent } from './admin/dictionary/dictionary.component';
import { DictionaryEditComponent } from './admin/dictionary-edit/dictionary-edit.component';
import { FormInputComponent } from './form-input/form-input.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { StylesComponent } from './admin/styles/styles.component';
import { StylesEditComponent } from './admin/styles-edit/styles-edit.component';
import { FileListComponent } from './admin/file-list/file-list.component';
import { FileEditComponent } from './admin/file-edit/file-edit.component';
import { CodemirrorModule } from 'ng2-codemirror';
import { StaticFileComponent } from './admin/static-file/static-file.component';
import { StaticFileEditComponent } from './admin/static-file-edit/static-file-edit.component';
import { TemplateComponent } from './admin/template/template.component';
import { TemplateEditComponent } from './admin/template-edit/template-edit.component';
import { ModifyPasswordComponent } from './admin/modify-password/modify-password.component';
// import { NgArrayPipesModule, NgStringPipesModule } from 'angular-pipes';
import { EqualValidator } from './services/equal-validator.directive';
import { TinyEditorComponent } from './tiny-editor/tiny-editor.component';
import { FriendLinksComponent } from './admin/friend-links/friend-links.component';
import { FriendLinksEditComponent } from './admin/friend-links-edit/friend-links-edit.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    IndexComponent,
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    PageHeaderComponent,
    SideMenusComponent,
    CategoryComponent,
    CategoryEditComponent,
    FileUploadComponent,
    ArticleComponent,
    ArticleEditComponent,
    DictionaryComponent,
    DictionaryEditComponent,
    FormInputComponent,
    SettingsComponent,
    StylesComponent,
    StylesEditComponent,
    FileListComponent,
    FileEditComponent,
    StaticFileComponent,
    StaticFileEditComponent,
    TemplateComponent,
    TemplateEditComponent,
    ModifyPasswordComponent,
    EqualValidator,
    TinyEditorComponent,
    FriendLinksComponent,
    FriendLinksEditComponent,
    AdvertsComponent,
    AdvertsEditComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    NgpModule,
    AppRoutingModule,
    TreeModule,
    ReactiveFormsModule,
    NgArrayPipesModule,
    CodemirrorModule,
    // NgStringPipesModule,
    NgbModule.forRoot()
  ],
  providers: [
    AdminService,
    AppService,
    LocalStorageService,
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
