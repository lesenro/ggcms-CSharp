namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 字典表增加value字段 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsDictionaries", "Value", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsDictionaries", "Value");
        }
    }
}
