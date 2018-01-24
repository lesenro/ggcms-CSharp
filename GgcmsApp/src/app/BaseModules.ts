export class GgcmsDictionary {
  Id : number = 0;
  Title : string = "";
  DictType : any = "";
  OrderID : number = 0;
  SysFlag : number = 0;
  describe : string = "";
  Value : string = "";
}
export class FormInputOption {
  type : string = "text";
  required : boolean = false;
  min : number = 0;
  max : number = 0;
  minLength : number = 0;
  maxLength : number = 0;
  pattern : string = "";
  message : string = "";
  requiredMessage : string = "";
  minMessage : string = "";
  maxMessage : string = "";
  minLengthMessage : string = "";
  maxLengthMessage : string = "";
  patternMessage : string = "";
  helpMessage : string = "";
  preview : boolean = false;
  datasource : string = "";
  egroup : any = "";
  targetName : any = "";
  multiple : boolean = false;
  onColor : string = "info";
  offColor : string = "default";
  onText : string = "ON";
  offText : string = "OFF";
  minDate : string = "";
  minDateMessage : string = "";
  maxDate : string = "";
  maxDateMessage : string = "";
  extension : string = "";
  extensionMessage : string = "";
  fileSize : number = 0;
  fileSizenMessage : string = "";
  dependent : string = "";
}

export class GgcmsModuleColumn {
  ColName : string = "";
  ColTitle : string = "";
  ColType : any = "";
  Length : number = 0;
  ColDecimal : number = 0;
  OrderId : number = 0;
  Options : string = "";
  Module_Id : number = 0;
  Value:any="";
}
export class GgcmsAttachment {
  Id = 0;
  AttaTitle = "";
  AttaUrl = "";
  Describe = "";
  RealName = "";
}