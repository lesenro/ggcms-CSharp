namespace GgcmsCSharp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 数据库初始化 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GgcmsArticlePages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Content = c.String(),
                        Title = c.String(maxLength: 255),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Title = c.String(maxLength: 255),
                        Hits = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        TitleImg = c.String(maxLength: 255),
                        TitleThumbnail = c.String(maxLength: 255),
                        MemberId = c.Int(nullable: false),
                        RedirectUrl = c.String(maxLength: 255),
                        Source = c.String(maxLength: 255),
                        SourceUrl = c.String(maxLength: 255),
                        Keywords = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                        TmplName = c.String(maxLength: 50),
                        StyleName = c.String(maxLength: 50),
                        PageTitle = c.Long(nullable: false),
                        ExtModelId = c.Int(nullable: false),
                        MobileTmplName = c.String(maxLength: 50),
                        ShowType = c.Int(nullable: false),
                        ShowLevel = c.Int(nullable: false),
                        Author = c.String(maxLength: 50),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttaTitle = c.String(maxLength: 255),
                        AttaUrl = c.String(maxLength: 255),
                        Describe = c.String(maxLength: 255),
                        AttaSize = c.Long(nullable: false),
                        RealName = c.String(maxLength: 255),
                        CreateTime = c.DateTime(nullable: false),
                        Articles_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        OrderId = c.Int(nullable: false),
                        LogoImg = c.String(maxLength: 255),
                        StyleName = c.String(maxLength: 50),
                        ParentId = c.Int(nullable: false),
                        TmplName = c.String(maxLength: 50),
                        MobileTmplName = c.String(maxLength: 50),
                        ArticleTmplName = c.String(maxLength: 50),
                        ArticleMobileTmplName = c.String(maxLength: 50),
                        RedirectUrl = c.String(maxLength: 255),
                        PageSize = c.Int(nullable: false),
                        ImgWidth = c.Int(nullable: false),
                        ImgHeight = c.Int(nullable: false),
                        RssFeed = c.String(maxLength: 255),
                        Keywords = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                        Content = c.String(),
                        ExtAttrib = c.String(),
                        ExtModelId = c.Int(nullable: false),
                        CategoryType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsDictionaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        DictType = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        SysFlag = c.Int(nullable: false),
                        describe = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsFriendLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Url = c.String(maxLength: 255),
                        WebName = c.String(maxLength: 100),
                        LogoImg = c.String(maxLength: 255),
                        Status = c.Int(nullable: false),
                        LinkType = c.Int(nullable: false),
                        RelationId = c.Int(nullable: false),
                        ExtAttrib = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsKeywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Keyword = c.String(maxLength: 50),
                        Url = c.String(maxLength: 255),
                        Describe = c.String(maxLength: 255),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        PassWord = c.String(maxLength: 50),
                        Sex = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 100),
                        Scores = c.Int(nullable: false),
                        Avatar = c.String(maxLength: 255),
                        JoinTime = c.DateTime(nullable: false),
                        Level = c.Int(nullable: false),
                        Phone = c.String(maxLength: 20),
                        Roles_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsModuleColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColName = c.String(maxLength: 50),
                        ColTitle = c.String(maxLength: 50),
                        ColType = c.String(maxLength: 50),
                        Length = c.Int(nullable: false),
                        ColDecimal = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Options = c.String(),
                        Module_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(maxLength: 50),
                        Describe = c.String(maxLength: 255),
                        TableName = c.String(maxLength: 50),
                        ViewName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsPowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PowerName = c.String(maxLength: 50),
                        OrderId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        PowerTag = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsRolePowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role_Id = c.Int(nullable: false),
                        Power_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsStyles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StyleName = c.String(maxLength: 50),
                        Folder = c.String(maxLength: 50),
                        Descrip = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskName = c.String(maxLength: 50),
                        TaskType = c.Int(nullable: false),
                        TaskConfigs = c.String(),
                        Status = c.Int(nullable: false),
                        Switch = c.Int(nullable: false),
                        PlanType = c.Int(nullable: false),
                        RunTime = c.DateTime(nullable: false),
                        PlanOptions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GgcmsTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicName = c.String(maxLength: 255),
                        CreateTime = c.DateTime(nullable: false),
                        Content = c.String(storeType: "ntext"),
                        PageSize = c.Int(nullable: false),
                        TmplName = c.String(maxLength: 50),
                        MobileTmplName = c.String(maxLength: 50),
                        Title = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                        Keywords = c.String(maxLength: 255),
                        LogoImg = c.String(maxLength: 255),
                        RedirectUrl = c.String(maxLength: 255),
                        StyleName = c.String(maxLength: 50),
                        ExtAttrib = c.String(),
                        TopicIds = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GgcmsTopics");
            DropTable("dbo.GgcmsTasks");
            DropTable("dbo.GgcmsSysConfigs");
            DropTable("dbo.GgcmsStyles");
            DropTable("dbo.GgcmsRolePowers");
            DropTable("dbo.GgcmsPowers");
            DropTable("dbo.GgcmsModules");
            DropTable("dbo.GgcmsModuleColumns");
            DropTable("dbo.GgcmsMembers");
            DropTable("dbo.GgcmsKeywords");
            DropTable("dbo.GgcmsFriendLinks");
            DropTable("dbo.GgcmsDictionaries");
            DropTable("dbo.GgcmsCategories");
            DropTable("dbo.GgcmsAttachments");
            DropTable("dbo.GgcmsArticles");
            DropTable("dbo.GgcmsArticlePages");
        }
    }
}
