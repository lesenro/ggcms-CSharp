namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsSysConfig
    {
        public int Id { get; set; }

        
        [StringLength(50)]
        public string CfgName { get; set; }

        
        public string CfgValue { get; set; }

        
        [StringLength(255)]
        public string Descrip { get; set; }

        public int GroupId { get; set; }

        
        public string Options { get; set; }

        public int OrderId { get; set; }
        public bool Protection { get; set; }
    }
}
