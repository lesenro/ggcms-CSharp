namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加管理菜单属性 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsPowers", "ShowInSidebar", c => c.Boolean(nullable: false));
            AddColumn("dbo.GgcmsPowers", "PowerType", c => c.Int(nullable: false));
            AddColumn("dbo.GgcmsPowers", "PowerParams", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsPowers", "PowerParams");
            DropColumn("dbo.GgcmsPowers", "PowerType");
            DropColumn("dbo.GgcmsPowers", "ShowInSidebar");
        }
    }
}
