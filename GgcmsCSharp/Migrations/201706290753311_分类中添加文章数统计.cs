namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 分类中添加文章数统计 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsCategories", "ArticleTotal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsCategories", "ArticleTotal");
        }
    }
}
