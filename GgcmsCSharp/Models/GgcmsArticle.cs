namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsArticle
    {
        public int Id { get; set; }


        public string Content { get; set; }


        [StringLength(255)]
        public string Title { get; set; }

        public int Hits { get; set; }

        public DateTime CreateTime { get; set; }


        [StringLength(255)]
        public string TitleImg { get; set; }


        [StringLength(255)]
        public string TitleThumbnail { get; set; }

        public int MemberId { get; set; }


        [StringLength(255)]
        public string RedirectUrl { get; set; }


        [StringLength(255)]
        public string Source { get; set; }


        [StringLength(255)]
        public string SourceUrl { get; set; }


        [StringLength(255)]
        public string Keywords { get; set; }


        [StringLength(255)]
        public string Description { get; set; }


        [StringLength(50)]
        public string TmplName { get; set; }


        [StringLength(50)]
        public string StyleName { get; set; }
        [StringLength(255)]
        public string PageTitle { get; set; }

        public int ExtModelId { get; set; }


        [StringLength(50)]
        public string MobileTmplName { get; set; }

        public int ShowType { get; set; }

        public int ShowLevel { get; set; }


        [StringLength(50)]
        public string Author { get; set; }

        public int Category_Id { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        [NotMapped]
        public List<GgcmsAttachment> attachments { get; set; }
    }
}
