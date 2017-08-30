namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加广告表GgcmsAdverts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GgcmsAdverts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        GroupKey = c.String(maxLength: 100),
                        Content = c.String(),
                        OrderID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Describe = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GgcmsAdverts");
        }
    }
}
