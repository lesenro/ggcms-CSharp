namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class totext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsDictionaries", "GroupKey", c => c.String(maxLength: 100));
            AddColumn("dbo.GgcmsDictionaries", "OtherProperty", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.GgcmsAdverts", "Content", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.GgcmsArticlePages", "Content", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.GgcmsArticles", "Content", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.GgcmsCategories", "Content", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsCategories", "Content", c => c.String());
            AlterColumn("dbo.GgcmsArticles", "Content", c => c.String());
            AlterColumn("dbo.GgcmsArticlePages", "Content", c => c.String());
            AlterColumn("dbo.GgcmsAdverts", "Content", c => c.String());
            DropColumn("dbo.GgcmsDictionaries", "OtherProperty");
            DropColumn("dbo.GgcmsDictionaries", "GroupKey");
        }
    }
}
