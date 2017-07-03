namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 添加受保护的设置 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsSysConfigs", "Protection", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsSysConfigs", "Protection");
        }
    }
}
