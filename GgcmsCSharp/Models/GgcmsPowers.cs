namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsPowers
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string PowerName { get; set; }

        public int OrderId { get; set; }

        public int ParentId { get; set; }

        [StringLength(255)]
        public string PowerTag { get; set; }

        [StringLength(255)]
        public string IconClass { get; set; }

        public bool ShowInSidebar { get; set; }

        public int PowerType { get; set; }

        [StringLength(255)]
        public string PowerParams { get; set; }

        [StringLength(255)]
        public string Path { get; set; }
    }
}
