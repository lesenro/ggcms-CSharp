namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsFriendLink
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        
        [StringLength(255)]
        public string Url { get; set; }

        
        [StringLength(100)]
        public string WebName { get; set; }

        
        [StringLength(255)]
        public string LogoImg { get; set; }

        public int Status { get; set; }

        public int LinkType { get; set; }

        public int RelationId { get; set; }

        
        public string ExtAttrib { get; set; }
    }
}
