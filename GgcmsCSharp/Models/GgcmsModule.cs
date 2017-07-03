namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsModule
    {

        public int Id { get; set; }

        
        [StringLength(50)]
        public string ModuleName { get; set; }

        
        [StringLength(255)]
        public string Describe { get; set; }

        
        [StringLength(50)]
        public string TableName { get; set; }

        
        [StringLength(50)]
        public string ViewName { get; set; }

    }
}
