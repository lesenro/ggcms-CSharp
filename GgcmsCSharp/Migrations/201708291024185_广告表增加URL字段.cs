namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 广告表增加URL字段 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsAdverts", "Url", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsAdverts", "Url");
        }
    }
}
