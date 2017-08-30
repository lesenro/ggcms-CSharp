namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 修改字典类型DictType为字符型 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GgcmsDictionaries", "DictType", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GgcmsDictionaries", "DictType", c => c.Int(nullable: false));
        }
    }
}
