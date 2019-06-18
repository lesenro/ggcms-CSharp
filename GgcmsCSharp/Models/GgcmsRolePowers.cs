namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsRolePowers
    {
        public int Id { get; set; }

        public int Role_Id { get; set; }

        public int Power_Id { get; set; }
    }
}
