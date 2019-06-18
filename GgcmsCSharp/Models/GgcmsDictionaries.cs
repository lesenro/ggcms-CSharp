namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsDictionaries
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string DictName { get; set; }

        /// <summary>
        /// 分组信息
        /// 系统可以事先指定的分组信息
        /// </summary>
        [StringLength(50)]
        public string GroupKey { get; set; }

        /// <summary>
        /// 保存上级的key
        /// 有上下级关系的字典，key是在运行时产生的
        /// </summary>
        [StringLength(50)]
        public string ParentKey { get; set; }

        /// <summary>
        /// 字典条目主键
        /// </summary>
        [StringLength(50)]
        public string DictKey { get; set; }

        public string DictValue { get; set; }

        [Column(TypeName = "text")]
        public string OtherProperty { get; set; }

        [StringLength(255)]
        public string DictDescribe { get; set; }

        public int? DictType { get; set; }

        public int? OrderId { get; set; }

        public int? DictStatus { get; set; }
    }
}
