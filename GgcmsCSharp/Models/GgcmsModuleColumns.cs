namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsModuleColumns
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ColName { get; set; }

        [StringLength(50)]
        public string ColTitle { get; set; }

        [StringLength(50)]
        public string ColType { get; set; }

        public int Length { get; set; }

        public int ColDecimal { get; set; }

        public int OrderId { get; set; }

        public string Options { get; set; }

        public int Module_Id { get; set; }
        [NotMapped]
        public dynamic Value { get; set; }
    }
}
