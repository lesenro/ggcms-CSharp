namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removegroupkey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsDictionaries", "OtherProperty", c => c.String(maxLength: 100));
            DropColumn("dbo.GgcmsDictionaries", "GroupKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GgcmsDictionaries", "GroupKey", c => c.String(maxLength: 100));
            AlterColumn("dbo.GgcmsDictionaries", "OtherProperty", c => c.String(unicode: false, storeType: "text"));
        }
    }
}
