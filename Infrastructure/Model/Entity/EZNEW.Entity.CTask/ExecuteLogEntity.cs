using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    [Serializable]
    [Entity("CTask_ExecuteLog", "CTask", "任务执行日志")]
    public class ExecuteLogEntity : BaseEntity<ExecuteLogEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public long Id
        {
            get { return valueDict.GetValue<long>("Id"); }
            set { valueDict.SetValue("Id", value); }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        [EntityField(Description = "工作任务")]
        public string Job
        {
            get { return valueDict.GetValue<string>("Job"); }
            set { valueDict.SetValue("Job", value); }
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        [EntityField(Description = "执行计划")]
        public string Trigger
        {
            get { return valueDict.GetValue<string>("Trigger"); }
            set { valueDict.SetValue("Trigger", value); }
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        [EntityField(Description = "执行服务")]
        public string Server
        {
            get { return valueDict.GetValue<string>("Server"); }
            set { valueDict.SetValue("Server", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EntityField(Description = "开始时间")]
        public DateTime BeginTime
        {
            get { return valueDict.GetValue<DateTime>("BeginTime"); }
            set { valueDict.SetValue("BeginTime", value); }
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
        /// 记录时间
        /// </summary>
        [EntityField(Description = "记录时间")]
        public DateTime RecordTime
        {
            get { return valueDict.GetValue<DateTime>("RecordTime"); }
            set { valueDict.SetValue("RecordTime", value); }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        [EntityField(Description = "执行状态")]
        public int Status
        {
            get { return valueDict.GetValue<int>("Status"); }
            set { valueDict.SetValue("Status", value); }
        }

        /// <summary>
        /// 消息
        /// </summary>
        [EntityField(Description = "消息")]
        public string Message
        {
            get { return valueDict.GetValue<string>("Message"); }
            set { valueDict.SetValue("Message", value); }
        }

        #endregion
    }
}