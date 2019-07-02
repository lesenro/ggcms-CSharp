namespace GgcmsCSharp.Models
{
    using GgcmsCSharp.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

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
        public T GetTaskConfig<T>()
        {
            return Utils.Tools.JsonDeserialize<T>(this.TaskConfigs);
        }
        public void SetNextRuntime()
        {
            if (this.TaskState == TaskStatus.Executing || this.TaskState == TaskStatus.disable) {
                return;
            }
            PlanOption option = Tools.JsonDeserialize<PlanOption>(PlanOptions);

            DateTime now = DateTime.Now;
            if (option.StartDate > now)
            {
                this.TaskState = TaskStatus.NotStarted;
                return;
            }
            if (option.EndDate < now)
            {
                this.TaskState = TaskStatus.IsOver;
                return;
            }
            this.TaskState = TaskStatus.Ready;
            DateTime rtime;
            switch (EPlanType)
            {
                case Models.PlanType.once:
                    this.RunTime = option.SpecificDate;
                    return;
                case Models.PlanType.daily:
                    rtime = new DateTime(now.Year, now.Month, now.Day, option.SpecificDate.Hour, option.SpecificDate.Minute, option.SpecificDate.Second);
                    if (rtime < now)
                    {
                        rtime = rtime.AddDays(1);
                    }
                    RunTime = rtime;
                    return;
                case Models.PlanType.TimeInterval:
                    rtime = new DateTime(now.Year, now.Month, now.Day, option.SpecificDate.Hour, option.SpecificDate.Minute, option.SpecificDate.Second);
                    if (rtime < now)
                    {
                        rtime = rtime.AddMinutes(option.IntervalMinute);
                    }
                    RunTime = rtime;
                    return;
                case Models.PlanType.weekly:
                    rtime = new DateTime(now.Year, now.Month, now.Day, option.SpecificDate.Hour, option.SpecificDate.Minute, option.SpecificDate.Second);
                    while (!option.WeekDays.ToList().Contains((int)rtime.DayOfWeek))
                    {
                        rtime = rtime.AddDays(1);
                    }
                    RunTime = rtime;
                    return;
                case Models.PlanType.monthly:
                    rtime = new DateTime(now.Year, now.Month, now.Day, option.SpecificDate.Hour, option.SpecificDate.Minute, option.SpecificDate.Second);
                    while (!option.MonthDays.ToList().Contains(rtime.Day))
                    {
                        rtime = rtime.AddDays(1);
                    }
                    RunTime = rtime;
                    return;
            }
        }
        [NotMapped]
        public TaskStatus TaskState
        {
            get
            {
                return (TaskStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }
        [NotMapped]
        public PlanType EPlanType
        {
            get
            {
                return (PlanType)this.PlanType;
            }
        }

        [NotMapped]
        public TaskType TaskModel
        {
            get
            {
                return (TaskType)this.TaskType;
            }
        }
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
        /// ׼������
        /// </summary>
        Ready = 3,
        /// <summary>
        /// ����
        /// </summary>
        disable=4,
        /// <summary>
        /// ִ����
        /// </summary>
        Executing = 5,

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
