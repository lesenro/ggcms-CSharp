namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dictpid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GgcmsDictionaries", "Pid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GgcmsDictionaries", "Pid");
        }
    }
}
