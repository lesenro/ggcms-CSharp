namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsAttachment
    {
        public int Id { get; set; }

        
        [StringLength(255)]
        public string AttaTitle { get; set; }

        
        [StringLength(255)]
        public string AttaUrl { get; set; }

        
        [StringLength(255)]
        public string Describe { get; set; }

        public long AttaSize { get; set; }

        
        [StringLength(255)]
        public string RealName { get; set; }

        public DateTime CreateTime { get; set; }

        public int Articles_Id { get; set; }
    }
}
