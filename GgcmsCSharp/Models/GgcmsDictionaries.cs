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
        /// ������Ϣ
        /// ϵͳ��������ָ���ķ�����Ϣ
        /// </summary>
        [StringLength(50)]
        public string GroupKey { get; set; }

        /// <summary>
        /// �����ϼ���key
        /// �����¼���ϵ���ֵ䣬key��������ʱ������
        /// </summary>
        [StringLength(50)]
        public string ParentKey { get; set; }

        /// <summary>
        /// �ֵ���Ŀ����
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
