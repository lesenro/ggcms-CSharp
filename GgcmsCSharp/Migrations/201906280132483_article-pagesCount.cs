namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlepagesCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsArticles", "pagesCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsArticles", "pagesCount");
        }
    }
}
