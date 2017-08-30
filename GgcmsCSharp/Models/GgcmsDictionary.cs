namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsDictionary
    {
        public int Id { get; set; }

        
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        [StringLength(100)]
        public string DictType { get; set; }

        public int OrderID { get; set; }

        public int SysFlag { get; set; }

        
        [StringLength(255)]
        public string describe { get; set; }
    }
}
