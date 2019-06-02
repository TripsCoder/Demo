using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.CTask;
using System.Collections.Generic;
using EZNEW.Framework.Code;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    public class TriggerServer : AggregationRoot<TriggerServer>
    {
        #region	字段

        /// <summary>
        /// 触发器
        /// </summary>
        protected Trigger trigger;

        /// <summary>
        /// 服务
        /// </summary>
        protected ServerNode server;

        /// <summary>
        /// 运行状态
        /// </summary>
        protected TaskTriggerServerRunStatus runStatus;

        /// <summary>
        /// 上次执行时间
        /// </summary>
        protected DateTime lastFireDate;

        /// <summary>
        /// 下次执行时间
        /// </summary>
        protected DateTime nextFireDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化服务节点执行计划对象
        /// </summary>
        /// <param name="trigger">编号</param>
        internal TriggerServer()
        {
            repository = this.Instance<ITriggerServerRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public Trigger Trigger
        {
            get
            {
                return trigger;
            }
            protected set
            {
                trigger = value;
            }
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNode Server
        {
            get
            {
                return server;
            }
            protected set
            {
                server = value;
            }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public TaskTriggerServerRunStatus RunStatus
        {
            get
            {
                return runStatus;
            }
            set
            {
                runStatus = value;
            }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get
            {
                return lastFireDate;
            }
            protected set
            {
                lastFireDate = value;
            }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get
            {
                return nextFireDate;
            }
            protected set
            {
                nextFireDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        public override async Task SaveAsync()
        {
            if (IsNew)
            {
                LastFireDate = DateTime.Now;
                NextFireDate = DateTime.Now;
            }
            await repository.SaveAsync(this);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return trigger == null || server == null;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个服务节点执行计划编号

        /// <summary>
        /// 生成一个服务节点执行计划编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateTriggerServerId()
        {
            return SerialNumber.GetSerialNumber("").ToString();
        }

        #endregion

        #region 创建服务节点执行计划

        /// <summary>
        /// 创建一个服务节点执行计划对象
        /// </summary>
        /// <param name="trigger">编号</param>
        /// <returns></returns>
        public static TriggerServer CreateTriggerServer(string triggerId, string serverId, TaskTriggerServerRunStatus runState = TaskTriggerServerRunStatus.运行)
        {
            return CreateTriggerServer(Trigger.CreateTrigger(triggerId), ServerNode.CreateServerNode(serverId), runState);
        }

        /// <summary>
        /// 创建一个计划服务对象
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <param name="server">服务信息</param>
        /// <param name="runState">运行状态</param>
        /// <returns></returns>
        public static TriggerServer CreateTriggerServer(Trigger trigger, ServerNode server, TaskTriggerServerRunStatus runState = TaskTriggerServerRunStatus.运行)
        {
            return new TriggerServer()
            {
                server = server,
                trigger = trigger,
                runStatus = runState
            };
        }

        protected override string GetIdentityValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #endregion
    }

    public class TriggerServerCompare : IEqualityComparer<TriggerServer>
    {
        public bool Equals(TriggerServer x, TriggerServer y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Server?.Id == y.Server?.Id && x.Trigger?.Id == y.Trigger?.Id;
        }

        public int GetHashCode(TriggerServer obj)
        {
            return 0;
        }
    }
}