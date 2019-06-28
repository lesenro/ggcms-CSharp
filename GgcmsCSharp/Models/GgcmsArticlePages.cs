namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;

    public partial class GgcmsArticlePages
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        public int Article_Id { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        [NotMapped]
        public EntityState state { get; set; }
    }
}
