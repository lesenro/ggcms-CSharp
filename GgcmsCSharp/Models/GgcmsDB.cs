namespace GgcmsCSharp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GgcmsDB : DbContext
    {
        public GgcmsDB()
            : base("name=GgcmsDB")
        {
        }

        public virtual DbSet<GgcmsArticlePage> GgcmsArticlePages { get; set; }
        public virtual DbSet<GgcmsArticle> GgcmsArticles { get; set; }
        public virtual DbSet<GgcmsAttachment> GgcmsAttachments { get; set; }
        public virtual DbSet<GgcmsCategory> GgcmsCategories { get; set; }
        public virtual DbSet<GgcmsDictionary> GgcmsDictionaries { get; set; }
        public virtual DbSet<GgcmsFriendLink> GgcmsFriendLinks { get; set; }
        public virtual DbSet<GgcmsKeyword> GgcmsKeywords { get; set; }
        public virtual DbSet<GgcmsMember> GgcmsMembers { get; set; }
        public virtual DbSet<GgcmsModuleColumn> GgcmsModuleColumns { get; set; }
        public virtual DbSet<GgcmsModule> GgcmsModules { get; set; }
        public virtual DbSet<GgcmsPower> GgcmsPowers { get; set; }
        public virtual DbSet<GgcmsRolePower> GgcmsRolePower { get; set; }
        public virtual DbSet<GgcmsStyle> GgcmsStyles { get; set; }
        public virtual DbSet<GgcmsSysConfig> GgcmsSysConfigs { get; set; }
        public virtual DbSet<GgcmsTask> GgcmsTasks { get; set; }
        public virtual DbSet<GgcmsTopic> GgcmsTopics { get; set; }
        public virtual DbSet<GgcmsAdverts> GgcmsAdverts { get; set; }
        

    }
}
