using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GgcmsCSharp.Models
{
    public class Pagination
    {
        //是否显示边界链接
        public bool boundaryLinks { get; set; } = false;
        //显示页数集合
        public int collectionSize { get; set; } = 5;
        //显示上一页下一页
        public bool directionLinks { get; set; } = true;
        //超出页集合时，是否显示省略号
        public bool ellipses { get; set; } = true;
        //总页数
        public int maxSize { get; set; } = 0;
        //当前页
        public int page { get; set; } = 0;
        //每页显示记录数
        public int pageSize { get; set; } = 0;
        //基本链接
        public string baseLink { get; set; } = "";
        //开始页
        public int start { get; set; } = 0;
        //结束页
        public int end { get; set; } = 0;
        public int getSkip()
        {
            return pageSize * (page - 1);
        }
        public void setMaxSize(int count)
        {
            maxSize = (int)Math.Ceiling((double)count / pageSize);
            
            if (maxSize<= collectionSize)
            {
                start = 1;
                end = maxSize;
            }
            else
            {
                int pad = (int)Math.Floor(collectionSize / 2d);
                start = page > pad ? page - pad : 1;
                end = start + collectionSize - 1;
                if (end > maxSize)
                {
                    end = maxSize;
                    start = end - collectionSize+1;
                }
            }
        }
    }
}