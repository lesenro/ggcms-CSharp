namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlepagetitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsArticles", "PageTitle", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsArticles", "PageTitle", c => c.Long(nullable: false));
        }
    }
}
