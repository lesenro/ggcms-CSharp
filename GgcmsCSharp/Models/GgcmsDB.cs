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

        public virtual DbSet<GgcmsAdverts> GgcmsAdverts { get; set; }
        public virtual DbSet<GgcmsArticlePages> GgcmsArticlePages { get; set; }
        public virtual DbSet<GgcmsArticles> GgcmsArticles { get; set; }
        public virtual DbSet<GgcmsAttachments> GgcmsAttachments { get; set; }
        public virtual DbSet<GgcmsCategories> GgcmsCategories { get; set; }
        public virtual DbSet<GgcmsDictionaries> GgcmsDictionaries { get; set; }
        public virtual DbSet<GgcmsFriendLinks> GgcmsFriendLinks { get; set; }
        public virtual DbSet<GgcmsKeywords> GgcmsKeywords { get; set; }
        public virtual DbSet<GgcmsMembers> GgcmsMembers { get; set; }
        public virtual DbSet<GgcmsModuleColumns> GgcmsModuleColumns { get; set; }
        public virtual DbSet<GgcmsModules> GgcmsModules { get; set; }
        public virtual DbSet<GgcmsPowers> GgcmsPowers { get; set; }
        public virtual DbSet<GgcmsRolePowers> GgcmsRolePowers { get; set; }
        public virtual DbSet<GgcmsStyles> GgcmsStyles { get; set; }
        public virtual DbSet<GgcmsTasks> GgcmsTasks { get; set; }
        public virtual DbSet<GgcmsTopics> GgcmsTopics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
