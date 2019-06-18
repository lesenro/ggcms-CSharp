namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class totext1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsDictionaries", "OtherProperty", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsDictionaries", "OtherProperty", c => c.String(maxLength: 100));
        }
    }
}
