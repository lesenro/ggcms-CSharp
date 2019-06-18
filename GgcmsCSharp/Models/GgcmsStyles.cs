namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsStyles
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string StyleName { get; set; }

        [StringLength(50)]
        public string Folder { get; set; }

        [StringLength(255)]
        public string Descrip { get; set; }
    }
}
