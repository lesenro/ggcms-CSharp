namespace GgcmsCSharp.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class GgcmsCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = "";

        public int OrderId { get; set; } = 0;

        [StringLength(255)]
        public string LogoImg { get; set; } = "";

        [StringLength(50)]
        public string StyleName { get; set; } = "";

        public int ParentId { get; set; } = 0;

        [StringLength(50)]
        public string TmplName { get; set; } = "";

        [StringLength(50)]
        public string MobileTmplName { get; set; } = "";

        [StringLength(50)]
        public string ArticleTmplName { get; set; } = "";

        [StringLength(50)]
        public string ArticleMobileTmplName { get; set; } = "";

        [StringLength(255)]
        public string RedirectUrl { get; set; } = "";

        public int PageSize { get; set; } = 0;

        public int ImgWidth { get; set; } = 0;

        public int ImgHeight { get; set; } = 0;

        [StringLength(255)]
        public string RssFeed { get; set; } = "";

        [StringLength(255)]
        public string Keywords { get; set; } = "";

        [StringLength(255)]
        public string Description { get; set; } = "";

        public string Content { get; set; } = "";

        public string ExtAttrib { get; set; } = "";

        public int ExtModelId { get; set; } = 0;

        public int CategoryType { get; set; } = 0;
        public int ArticleTotal { get; set; } = 0;
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        [NotMapped]
        public List<GgcmsCategory> subCategory { get; set; }
        public static List<GgcmsCategory> GetCategoryList(int pid, List<GgcmsCategory> categorys,string prefix)
        {
            var list = (from c in categorys
                       where c.ParentId == pid
                       select c).ToList();
            foreach(var item in list)
            {
                item.RedirectUrl = string.IsNullOrEmpty(item.RedirectUrl.Trim()) ? prefix+"/Category/" + item.Id.ToString() : item.RedirectUrl;
                item.subCategory = GgcmsCategory.GetCategoryList(item.Id, categorys, prefix);
            }
            return list as List<GgcmsCategory>;
        }
        public static GgcmsCategory GetCategoryById(int id, List<GgcmsCategory> list)
        {
            foreach (var item in list)
            {
                if (item.Id == id)
                {
                    return item;
                }
                var info = GgcmsCategory.GetCategoryById(id, item.subCategory);
                if (info != null)
                {
                    return info;
                }
            }
            return null;
        }
        public static int[] GetCategoryIds(GgcmsCategory category)
        {
            List<int> ids = new List<int>();
            ids.Add(category.Id);
            if (category.subCategory.Count > 0)
            {
                foreach(var item in category.subCategory)
                {
                    int[] tmps = GetCategoryIds(item);
                    ids.AddRange(tmps);
                }
            }
            return ids.ToArray();
        }
    }
}
