namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加权限Path : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsPowers", "Path", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsPowers", "Path");
        }
    }
}
