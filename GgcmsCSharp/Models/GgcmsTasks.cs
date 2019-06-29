namespace GgcmsCSharp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GgcmsTasks
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string TaskName { get; set; }

        public int TaskType { get; set; }

        public string TaskConfigs { get; set; }

        public int Status { get; set; }

        public int Switch { get; set; }

        public int PlanType { get; set; }

        public DateTime RunTime { get; set; }

        public string PlanOptions { get; set; }
    }
    public enum TaskStatus
    {
        /// <summary>
        /// δ��ʼ
        /// </summary>
        NotStarted = 1,
        /// <summary>
        /// �ѽ���
        /// </summary>
        IsOver = 2,
        /// <summary>
        /// ִ����
        /// </summary>
        Executing = 3
    }
    public enum TaskType
    {
        SqlTask = 1,
        GatherTask = 2,
        StaticTask = 3
    }
    public class StaticTaskInfo
    {
        //��������
        public bool All { get; set; }
        //Ҫ���ɵķ����б�
        public int[] Categories { get; set; }
        //Ҫ���ɵ�ר���б�
        public int[] Topics { get; set; }
    }
    public enum PlanType
    {
        /// <summary>
        /// ����һ��
        /// </summary>
        once = 1,
        /// <summary>
        /// ÿ��ִ��
        /// </summary>
        daily = 2,
        /// <summary>
        /// ÿ��ִ��
        /// </summary>
        weekly = 3,
        /// <summary>
        /// ÿ��
        /// </summary>
        monthly = 4,
        /// <summary>
        /// ÿ���ʱ���ִ��
        /// </summary>
        TimeInterval = 5,
    }
    public class PlanOption
    {
        /// <summary>
        /// �ض�������
        /// </summary>
        public DateTime SpecificDate { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public int IntervalMinute { get; set; }
        /// <summary>
        /// ��ִ����
        /// </summary>
        public int[] WeekDays { get; set; }
        /// <summary>
        /// ��ִ����
        /// </summary>
        public int[] MonthDays { get; set; }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
