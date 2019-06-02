using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    [Serializable]
    [Entity("CTask_ErrorLog", "CTask", "任务异常日志")]
    public class ErrorLogEntity : BaseEntity<ErrorLogEntity>
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
        /// 服务节点
        /// </summary>
        [EntityField(Description = "服务节点")]
        public string Server
        {
            get { return valueDict.GetValue<string>("Server"); }
            set { valueDict.SetValue("Server", value); }
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
        /// 错误消息
        /// </summary>
        [EntityField(Description = "错误消息")]
        public string Message
        {
            get { return valueDict.GetValue<string>("Message"); }
            set { valueDict.SetValue("Message", value); }
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        [EntityField(Description = "错误描述")]
        public string Description
        {
            get { return valueDict.GetValue<string>("Description"); }
            set { valueDict.SetValue("Description", value); }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        [EntityField(Description = "错误类型")]
        public int Type
        {
            get { return valueDict.GetValue<int>("Type"); }
            set { valueDict.SetValue("Type", value); }
        }

        /// <summary>
        /// 时间
        /// </summary>
        [EntityField(Description = "时间")]
        public DateTime Date
        {
            get { return valueDict.GetValue<DateTime>("Date"); }
            set { valueDict.SetValue("Date", value); }
        }

        #endregion
    }
}