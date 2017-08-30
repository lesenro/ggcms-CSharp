using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GgcmsCSharp.Utils;

namespace GgcmsCSharp.Models
{
    public class GgcmsAdverts
    {
        public int Id { get; set; }


        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(100)]
        public string GroupKey { get; set; }

        public string Content { get; set; }

        public int OrderID { get; set; }

        public int Status { get; set; }


        [StringLength(255)]
        public string Describe { get; set; }
        public static GgcmsAdverts Clone(dynamic data)
        {
            GgcmsAdverts info = new GgcmsAdverts();
            if (Tools.IsPropertyExist(data, "Id"))
            {
                info.Id = data.Id;
            }
            if (Tools.IsPropertyExist(data, "Title"))
            {
                info.Title = data.Title;
            }
            if (Tools.IsPropertyExist(data, "Url"))
            {
                info.Url = data.Url;
            }
            if (Tools.IsPropertyExist(data, "Image"))
            {
                info.Image = data.Image;
            }
            if (Tools.IsPropertyExist(data, "GroupKey"))
            {
                info.GroupKey = data.GroupKey;
            }
            if (Tools.IsPropertyExist(data, "Content"))
            {
                info.Content = data.Content;
            }
            if (Tools.IsPropertyExist(data, "OrderID"))
            {
                info.OrderID = data.OrderID;
            }
            if (Tools.IsPropertyExist(data, "Status"))
            {
                info.Status = data.Status;
            }
            return info;
        }
    }
}