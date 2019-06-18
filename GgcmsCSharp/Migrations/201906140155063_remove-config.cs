namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeconfig : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.GgcmsSysConfigs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GgcmsSysConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CfgName = c.String(maxLength: 50),
                        CfgValue = c.String(),
                        Descrip = c.String(maxLength: 255),
                        GroupId = c.Int(nullable: false),
                        Options = c.String(),
                        OrderId = c.Int(nullable: false),
                        Protection = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
