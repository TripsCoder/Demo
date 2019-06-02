using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务执行计划
    /// </summary>
    [Serializable]
    [Entity("CTask_Trigger", "CTask", "任务执行计划")]
    public class TriggerEntity : BaseEntity<TriggerEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Id
        {
            get { return valueDict.GetValue<string>("Id"); }
            set { valueDict.SetValue("Id", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityField(Description = "名称")]
        public string Name
        {
            get { return valueDict.GetValue<string>("Name"); }
            set { valueDict.SetValue("Name", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Description
        {
            get { return valueDict.GetValue<string>("Description"); }
            set { valueDict.SetValue("Description", value); }
        }

        /// <summary>
        /// 所属任务
        /// </summary>
        [EntityField(Description = "所属任务")]
        public string Job
        {
            get { return valueDict.GetValue<string>("Job"); }
            set { valueDict.SetValue("Job", value); }
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        [EntityField(Description = "应用到对象")]
        public int ApplyTo
        {
            get { return valueDict.GetValue<int>("ApplyTo"); }
            set { valueDict.SetValue("ApplyTo", value); }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        [EntityField(Description = "上次执行时间")]
        public DateTime PrevFireTime
        {
            get { return valueDict.GetValue<DateTime>("PrevFireTime"); }
            set { valueDict.SetValue("PrevFireTime", value); }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [EntityField(Description = "下次执行时间")]
        public DateTime NextFireTime
        {
            get { return valueDict.GetValue<DateTime>("NextFireTime"); }
            set { valueDict.SetValue("NextFireTime", value); }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        [EntityField(Description = "优先级")]
        public int Priority
        {
            get { return valueDict.GetValue<int>("Priority"); }
            set { valueDict.SetValue("Priority", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return valueDict.GetValue<int>("Status"); }
            set { valueDict.SetValue("Status", value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        [EntityField(Description = "类型")]
        public int Type
        {
            get { return valueDict.GetValue<int>("Type"); }
            set { valueDict.SetValue("Type", value); }
        }

        /// <summary>
        /// 额外条件类型
        /// </summary>
        [EntityField(Description = "额外条件类型")]
        public int ConditionType
        {
            get { return valueDict.GetValue<int>("ConditionType"); }
            set { valueDict.SetValue("ConditionType", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EntityField(Description = "开始时间")]
        public DateTime StartTime
        {
            get { return valueDict.GetValue<DateTime>("StartTime"); }
            set { valueDict.SetValue("StartTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EntityField(Description = "结束时间")]
        public DateTime EndTime
        {
            get { return valueDict.GetValue<DateTime>("EndTime"); }
            set { valueDict.SetValue("EndTime", value); }
        }

        /// <summary>
        /// 执行失败操作类型
        /// </summary>
        [EntityField(Description = "执行失败操作类型")]
        public int MisFireType
        {
            get { return valueDict.GetValue<int>("MisFireType"); }
            set { valueDict.SetValue("MisFireType", value); }
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        [EntityField(Description = "总触发次数")]
        public int FireTotalCount
        {
            get { return valueDict.GetValue<int>("FireTotalCount"); }
            set { valueDict.SetValue("FireTotalCount", value); }
        }

        #endregion
    }
}