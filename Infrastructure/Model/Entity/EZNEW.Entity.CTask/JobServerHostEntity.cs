﻿using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    [Serializable]
    [Entity("CTask_JobServerHost", "CTask", "工作承载节点")]
    public class JobServerHostEntity : BaseEntity<JobServerHostEntity>
    {
        #region	字段

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
        /// 任务
        /// </summary>
        [EntityField(Description = "任务", PrimaryKey = true)]
        public string Job
        {
            get { return valueDict.GetValue<string>("Job"); }
            set { valueDict.SetValue("Job", value); }
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

        #endregion
    }
}