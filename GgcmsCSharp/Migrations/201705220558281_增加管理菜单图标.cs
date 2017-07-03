namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加管理菜单图标 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsPowers", "IconClass", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsPowers", "IconClass");
        }
    }
}
