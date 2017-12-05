import { Component, OnInit, OnDestroy, AfterViewInit, EventEmitter } from '@angular/core';
import { AdminService } from "app/services";
import 'tinymce/plugins/advlist';
import 'tinymce/plugins/autolink';
import 'tinymce/plugins/lists';
import 'tinymce/plugins/link';
import 'tinymce/plugins/image';
import 'tinymce/plugins/charmap';
import 'tinymce/plugins/print';
import 'tinymce/plugins/preview';
import 'tinymce/plugins/hr';
import 'tinymce/plugins/anchor';
import 'tinymce/plugins/pagebreak';
import 'tinymce/plugins/searchreplace';
import 'tinymce/plugins/wordcount';
import 'tinymce/plugins/visualblocks';
import 'tinymce/plugins/visualchars';
import 'tinymce/plugins/code';
import 'tinymce/plugins/codesample';
import 'tinymce/plugins/fullscreen';
import 'tinymce/plugins/insertdatetime';
import 'tinymce/plugins/media';
import 'tinymce/plugins/nonbreaking';
import 'tinymce/plugins/save';
import 'tinymce/plugins/table';
import 'tinymce/plugins/contextmenu';
import 'tinymce/plugins/directionality';
import 'tinymce/plugins/emoticons';
import 'tinymce/plugins/template';
import 'tinymce/plugins/paste';
import 'tinymce/plugins/textcolor';
import 'tinymce/plugins/colorpicker';
import 'tinymce/plugins/textpattern';
import 'tinymce/plugins/imagetools';
import 'tinymce/plugins/toc';
import 'tinymce/plugins/help';
import 'assets/plugins/tinymce/languages/zh_CN';
declare var tinymce: any;
@Component({
  selector: 'tiny-editor',
  templateUrl: './tiny-editor.component.html',
  styleUrls: ['./tiny-editor.component.css'],
  inputs: ['elementId', 'value', 'options'],
  outputs: ['valueChange','fileUploaded'],
})

export class TinyEditorComponent implements AfterViewInit, OnDestroy {
  elementId: String;
  value: String = "";
  valueChange = new EventEmitter<any>();
  fileUploaded = new EventEmitter<any>();
  options = {};
  editor;

  constructor(public adminServ: AdminService) { }
  ngAfterViewInit() {
    var defaultOptions = {
      selector: '#' + this.elementId,
      height: 300,
      plugins: [
        'advlist autolink lists link image charmap print preview hr anchor pagebreak',
        'searchreplace wordcount visualblocks visualchars code fullscreen',
        'insertdatetime media nonbreaking save table contextmenu directionality',
        'emoticons template paste textcolor colorpicker textpattern imagetools toc help code codesample'
      ],
      toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent ',
      toolbar2: 'link image media | forecolor backcolor | preview print code codesample',
      image_advtab: true,
      skin_url: 'assets/plugins/tinymce/skins/lightgray',
      images_upload_url: this.adminServ.getFileUploadUrl(),
      images_upload_handler: (blobInfo, success, failure) => {
        this.adminServ.fileUpload(blobInfo.blob()).then(resp => {
          success(resp.link);
          this.fileUploaded.emit(resp);
        });
      },

      images_upload_credentials: true,
      automatic_uploads: true,
      file_picker_types: 'image',
      setup: editor => {
        this.editor = editor;
        // editor.on('keyup', () => {
        //   const content = editor.getContent();
        //   this.valueChange.emit(content);
        // });
        editor.contentCSS.push("assets/plugins/tinymce/plugins/codesample/prism.css");
        editor.on('change', () => {
          const content = editor.getContent();
          this.valueChange.emit(content);
        });
      },
      init_instance_callback: editor => {
        editor.setContent(this.value);
        //editor.setContentCSS("aaa.css");
      },
      languages: "zh_CN",
      menubar: false,
      contentCSS:"aaa.css"
    };
    var option = Object.assign({}, defaultOptions, this.options);
    tinymce.init(option);
  }

  ngOnDestroy() {
    tinymce.remove(this.editor);
  }
}