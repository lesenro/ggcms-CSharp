namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsKeyword
    {
        public int Id { get; set; }

        
        [StringLength(50)]
        public string Keyword { get; set; }

        
        [StringLength(255)]
        public string Url { get; set; }

        
        [StringLength(255)]
        public string Describe { get; set; }

        public int Status { get; set; }
    }
}
