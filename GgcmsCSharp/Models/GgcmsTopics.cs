namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsTopics
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string TopicName { get; set; }

        public DateTime CreateTime { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        public int PageSize { get; set; }

        [StringLength(50)]
        public string TmplName { get; set; }

        [StringLength(50)]
        public string MobileTmplName { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Keywords { get; set; }

        [StringLength(255)]
        public string LogoImg { get; set; }

        [StringLength(255)]
        public string RedirectUrl { get; set; }

        [StringLength(50)]
        public string StyleName { get; set; }

        public string ExtAttrib { get; set; }

        public string TopicIds { get; set; }
    }
}
