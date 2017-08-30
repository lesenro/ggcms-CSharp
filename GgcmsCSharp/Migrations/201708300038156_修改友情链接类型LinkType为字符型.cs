namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 修改友情链接类型LinkType为字符型 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsFriendLinks", "LinkType", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsFriendLinks", "LinkType", c => c.Int(nullable: false));
        }
    }
}
