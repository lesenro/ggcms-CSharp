namespace GgcmsCSharp.Models
{
    using GgcmsCSharp.Utils;
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
        [StringLength(100)]
        public string LinkType { get; set; }

        public int RelationId { get; set; }

        public string ExtAttrib { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        public static GgcmsFriendLink Clone(dynamic data)
        {
            GgcmsFriendLink info = new GgcmsFriendLink();
            if(Tools.IsPropertyExist(data, "Id"))
            {
                info.Id = data.Id;
            }
            if (Tools.IsPropertyExist(data, "OrderId"))
            {
                info.OrderId = data.OrderId;
            }
            if (Tools.IsPropertyExist(data, "Url"))
            {
                info.Url = data.Url;
            }
            if (Tools.IsPropertyExist(data, "WebName"))
            {
                info.WebName = data.WebName;
            }
            if (Tools.IsPropertyExist(data, "LogoImg"))
            {
                info.LogoImg = data.LogoImg;
            }
            if (Tools.IsPropertyExist(data, "LinkType"))
            {
                info.LinkType = data.LinkType;
            }
            if (Tools.IsPropertyExist(data, "RelationId"))
            {
                info.RelationId = data.RelationId;
            }
            if (Tools.IsPropertyExist(data, "ExtAttrib"))
            {
                info.ExtAttrib = data.ExtAttrib;
            }
            return info;
        }
    }
}
