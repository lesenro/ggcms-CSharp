namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 广告表增加Image字段 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsAdverts", "Image", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsAdverts", "Image");
        }
    }
}
