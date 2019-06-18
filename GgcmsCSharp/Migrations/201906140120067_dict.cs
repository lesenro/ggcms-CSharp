namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dict : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsDictionaries", "DictName", c => c.String(maxLength: 50));
            AddColumn("dbo.GgcmsDictionaries", "GroupKey", c => c.String(maxLength: 50));
            AddColumn("dbo.GgcmsDictionaries", "ParentKey", c => c.String(maxLength: 50));
            AddColumn("dbo.GgcmsDictionaries", "DictKey", c => c.String(maxLength: 50));
            AddColumn("dbo.GgcmsDictionaries", "DictValue", c => c.String());
            AddColumn("dbo.GgcmsDictionaries", "DictDescribe", c => c.String(maxLength: 255));
            AddColumn("dbo.GgcmsDictionaries", "DictStatus", c => c.Int());
            AlterColumn("dbo.GgcmsDictionaries", "DictType", c => c.Int());
            AlterColumn("dbo.GgcmsDictionaries", "OrderId", c => c.Int());
            DropColumn("dbo.GgcmsDictionaries", "Pid");
            DropColumn("dbo.GgcmsDictionaries", "Title");
            DropColumn("dbo.GgcmsDictionaries", "SysFlag");
            DropColumn("dbo.GgcmsDictionaries", "describe");
            DropColumn("dbo.GgcmsDictionaries", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GgcmsDictionaries", "Value", c => c.String(maxLength: 100));
            AddColumn("dbo.GgcmsDictionaries", "describe", c => c.String(maxLength: 255));
            AddColumn("dbo.GgcmsDictionaries", "SysFlag", c => c.Int(nullable: false));
            AddColumn("dbo.GgcmsDictionaries", "Title", c => c.String(maxLength: 100));
            AddColumn("dbo.GgcmsDictionaries", "Pid", c => c.Int(nullable: false));
            AlterColumn("dbo.GgcmsDictionaries", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.GgcmsDictionaries", "DictType", c => c.String(maxLength: 100));
            DropColumn("dbo.GgcmsDictionaries", "DictStatus");
            DropColumn("dbo.GgcmsDictionaries", "DictDescribe");
            DropColumn("dbo.GgcmsDictionaries", "DictValue");
            DropColumn("dbo.GgcmsDictionaries", "DictKey");
            DropColumn("dbo.GgcmsDictionaries", "ParentKey");
            DropColumn("dbo.GgcmsDictionaries", "GroupKey");
            DropColumn("dbo.GgcmsDictionaries", "DictName");
        }
    }
}
