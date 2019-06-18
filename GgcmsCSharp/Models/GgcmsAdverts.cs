namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsAdverts
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(100)]
        public string GroupKey { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }

        public int OrderID { get; set; }

        public int Status { get; set; }

        [StringLength(255)]
        public string Describe { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
    }
}
