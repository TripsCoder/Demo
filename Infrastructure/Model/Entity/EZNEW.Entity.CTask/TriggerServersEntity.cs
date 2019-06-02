using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerServer", "CTask", "服务节点执行计划")]
    public class TriggerServerEntity : BaseEntity<TriggerServerEntity>
    {
        #region	字段

        /// <summary>
        /// 触发器
        /// </summary>
        [EntityField(Description = "触发器", PrimaryKey = true)]
        public string Trigger
        {
            get { return valueDict.GetValue<string>("Trigger"); }
            set { valueDict.SetValue("Trigger", value); }
        }

        /// <summary>
        /// 服务
        /// </summary>
        [EntityField(Description = "服务", PrimaryKey = true)]
        public string Server
        {
            get { return valueDict.GetValue<string>("Server"); }
            set { valueDict.SetValue("Server", value); }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        [EntityField(Description = "运行状态")]
        public int RunStatus
        {
            get { return valueDict.GetValue<int>("RunStatus"); }
            set { valueDict.SetValue("RunStatus", value); }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        [EntityField(Description = "上次执行时间")]
        public DateTime LastFireDate
        {
            get { return valueDict.GetValue<DateTime>("LastFireDate"); }
            set { valueDict.SetValue("LastFireDate", value); }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [EntityField(Description = "下次执行时间")]
        public DateTime NextFireDate
        {
            get { return valueDict.GetValue<DateTime>("NextFireDate"); }
            set { valueDict.SetValue("NextFireDate", value); }
        }

        #endregion
    }
}