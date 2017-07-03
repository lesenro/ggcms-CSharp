namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsTask
    {
        public int Id { get; set; }

        
        [StringLength(50)]
        public string TaskName { get; set; }

        public int TaskType { get; set; }

        
        public string TaskConfigs { get; set; }

        public int Status { get; set; }

        public int Switch { get; set; }

        public int PlanType { get; set; }

        public DateTime RunTime { get; set; }

        
        public string PlanOptions { get; set; }
    }
}
