namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlepagetitle255 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsArticles", "PageTitle", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsArticles", "PageTitle", c => c.String());
        }
    }
}
