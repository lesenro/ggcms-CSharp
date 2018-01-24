namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsArticle
    {
        public int Id { get; set; }


        public string Content { get; set; }


        [StringLength(255)]
        public string Title { get; set; }

        public int Hits { get; set; }

        public DateTime CreateTime { get; set; }


        [StringLength(255)]
        public string TitleImg { get; set; }


        [StringLength(255)]
        public string TitleThumbnail { get; set; }

        public int MemberId { get; set; }


        [StringLength(255)]
        public string RedirectUrl { get; set; }


        [StringLength(255)]
        public string Source { get; set; }


        [StringLength(255)]
        public string SourceUrl { get; set; }


        [StringLength(255)]
        public string Keywords { get; set; }


        [StringLength(255)]
        public string Description { get; set; }


        [StringLength(50)]
        public string TmplName { get; set; }


        [StringLength(50)]
        public string StyleName { get; set; }
        [StringLength(255)]
        public string PageTitle { get; set; }

        public int ExtModelId { get; set; }


        [StringLength(50)]
        public string MobileTmplName { get; set; }

        public int ShowType { get; set; }

        public int ShowLevel { get; set; }


        [StringLength(50)]
        public string Author { get; set; }

        public int Category_Id { get; set; }
        [NotMapped]
        public List<UpFileClass> files { get; set; }
        [NotMapped]
        public List<GgcmsAttachment> attachments { get; set; }
        [NotMapped]
        public GgcmsModule ModuleInfo { get; set; }

        public GgcmsArticle Clone()
        {
            var info = new GgcmsArticle
            {
                Author = Author,
                Category_Id = Category_Id,
                CreateTime = CreateTime,
                Description = Description,
                ExtModelId = ExtModelId,
                Hits = Hits,
                Id = Id,
                Keywords = Keywords,
                MobileTmplName = MobileTmplName,
                PageTitle = PageTitle,
                RedirectUrl = RedirectUrl,
                Source = Source,
                SourceUrl = SourceUrl,
                StyleName = StyleName,
                Title = Title,
                TitleImg = TitleImg,
                TitleThumbnail = TitleThumbnail,
                TmplName = TmplName,
                ShowLevel = ShowLevel,
                ShowType = ShowType,
                Content=Content,
                MemberId=MemberId,
            };

            return info;
        }
        public static GgcmsArticle Clone(dynamic dinfo)
        {
            var info = new GgcmsArticle
            {
                Author = dinfo.Author,
                Category_Id = dinfo.Category_Id,
                CreateTime = dinfo.CreateTime,
                Description = dinfo.Description,
                ExtModelId = dinfo.ExtModelId,
                Hits = dinfo.Hits,
                Id = dinfo.Id,
                Keywords = dinfo.Keywords,
                MobileTmplName = dinfo.MobileTmplName,
                PageTitle = dinfo.PageTitle,
                RedirectUrl = dinfo.RedirectUrl,
                Source = dinfo.Source,
                SourceUrl = dinfo.SourceUrl,
                StyleName = dinfo.StyleName,
                Title = dinfo.Title,
                TitleImg = dinfo.TitleImg,
                TitleThumbnail = dinfo.TitleThumbnail,
                TmplName = dinfo.TmplName,
                ShowLevel = dinfo.ShowLevel,
                ShowType = dinfo.ShowType,
            };

            return info;
        }
    }
}
