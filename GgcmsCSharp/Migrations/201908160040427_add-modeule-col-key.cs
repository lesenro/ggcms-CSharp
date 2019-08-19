namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmodeulecolkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsModuleColumns", "ColKey", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsModuleColumns", "ColKey");
        }
    }
}
