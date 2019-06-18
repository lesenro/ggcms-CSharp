namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsMembers
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        public bool Sex { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int Scores { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        public DateTime JoinTime { get; set; }

        public int Level { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public int Roles_Id { get; set; }
    }
}
