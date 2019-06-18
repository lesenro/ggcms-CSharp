namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class GgcmsCategories
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public int OrderId { get; set; }

        [StringLength(255)]
        public string LogoImg { get; set; }

        [StringLength(50)]
        public string StyleName { get; set; }

        public int ParentId { get; set; }

        [StringLength(50)]
        public string TmplName { get; set; }

        [StringLength(50)]
        public string MobileTmplName { get; set; }

        [StringLength(50)]
        public string ArticleTmplName { get; set; }

        [StringLength(50)]
        public string ArticleMobileTmplName { get; set; }

        [StringLength(255)]
        public string RedirectUrl { get; set; }

        public int PageSize { get; set; }

        public int ImgWidth { get; set; }

        public int ImgHeight { get; set; }

        [StringLength(255)]
        public string RssFeed { get; set; }

        [StringLength(255)]
        public string Keywords { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }

        public string ExtAttrib { get; set; }

        public int ExtModelId { get; set; }

        public int CategoryType { get; set; }

        public int ArticleTotal { get; set; }

        [StringLength(255)]
        public string RouteKey { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        [NotMapped]
        public List<GgcmsCategories> subCategory { get; set; }
        public static List<GgcmsCategories> GetCategoryList(int pid, List<GgcmsCategories> categorys, string prefix)
        {
            var list = (from c in categorys
                        where c.ParentId == pid
                        select c).ToList();
            foreach (var item in list)
            {
                string key = string.IsNullOrWhiteSpace(item.RouteKey) ? item.Id.ToString() : item.RouteKey.Trim();
                string rurl = item.RedirectUrl ?? "";
                item.RedirectUrl = string.IsNullOrEmpty(rurl.Trim()) ? prefix + "/Category/" + key : rurl;
                item.subCategory = GetCategoryList(item.Id, categorys, prefix);
            }
            return list as List<GgcmsCategories>;
        }
        public static GgcmsCategories GetCategoryById(int id, List<GgcmsCategories> list)
        {
            foreach (var item in list)
            {
                if (item.Id == id)
                {
                    return item;
                }
                var info = GetCategoryById(id, item.subCategory);
                if (info != null)
                {
                    return info;
                }
            }
            return null;
        }
        public static GgcmsCategories GetCategoryByKey(string key, List<GgcmsCategories> list)
        {
            foreach (var item in list)
            {
                if (item.RouteKey == key)
                {
                    return item;
                }
                var info = GetCategoryByKey(key, item.subCategory);
                if (info != null)
                {
                    return info;
                }
            }
            return null;
        }
        public static int[] GetCategoryIds(GgcmsCategories category)
        {
            List<int> ids = new List<int>();
            ids.Add(category.Id);
            if (category.subCategory.Count > 0)
            {
                foreach (var item in category.subCategory)
                {
                    int[] tmps = GetCategoryIds(item);
                    ids.AddRange(tmps);
                }
            }
            return ids.ToArray();
        }
    }
}
