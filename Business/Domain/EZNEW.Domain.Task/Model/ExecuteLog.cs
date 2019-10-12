using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Framework.ValueType;
using EZNEW.Domain.CTask.Service;
using EZNEW.Framework.Code;
using System.Threading.Tasks;
using EZNEW.CTask;
using EZNEW.Application.CTask;
using EZNEW.Framework.IoC;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    public class ExecuteLog : AggregationRoot<ExecuteLog>
    {
        static IJobService jobService = ContainerManager.Resolve<IJobService>();//工作服务
        static ITriggerService triggerService = ContainerManager.Resolve<ITriggerService>();//触发器服务
        static IServerNodeService serverNodeService = ContainerManager.Resolve<IServerNodeService>();//服务节点服务

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long id;

        /// <summary>
        /// 工作任务
        /// </summary>
        protected LazyMember<Job> job;

        /// <summary>
        /// 执行计划
        /// </summary>
        protected LazyMember<Trigger> trigger;

        /// <summary>
        /// 执行服务
        /// </summary>
        protected LazyMember<ServerNode> server;

        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime beginTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime endTime;

        /// <summary>
        /// 记录时间
        /// </summary>
        protected DateTime recordTime;

        /// <summary>
        /// 执行状态
        /// </summary>
        protected ExecuteLogStatus status;

        /// <summary>
        /// 消息
        /// </summary>
        protected string message;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务执行日志对象
        /// </summary>
        /// <param name="id">编号</param>
        internal ExecuteLog(long id = 0)
        {
            this.id = id;
            job = new LazyMember<Model.Job>(LoadJob);
            trigger = new LazyMember<Model.Trigger>(LoadTrigger);
            server = new LazyMember<ServerNode>(LoadServer);
            repository = this.Instance<IExecuteLogRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public Job Job
        {
            get
            {
                return job.Value;
            }
            protected set
            {
                job.SetValue(value, false);
            }
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public Trigger Trigger
        {
            get
            {
                return trigger.Value;
            }
            protected set
            {
                trigger.SetValue(value, false);
            }
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public ServerNode Server
        {
            get
            {
                return server.Value;
            }
            protected set
            {
                server.SetValue(value, false);
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get
            {
                return beginTime;
            }
            protected set
            {
                beginTime = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            protected set
            {
                endTime = value;
            }
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get
            {
                return recordTime;
            }
            protected set
            {
                recordTime = value;
            }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecuteLogStatus Status
        {
            get
            {
                return status;
            }
            protected set
            {
                status = value;
            }
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            protected set
            {
                message = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 设置服务信息

        /// <summary>
        /// 设置服务节点
        /// </summary>
        /// <param name="server">服务信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetServer(ServerNode server, bool init = true)
        {
            this.server.SetValue(server, init);
        }

        #endregion

        #region 设置工作任务

        /// <summary>
        /// 设置工作任务
        /// </summary>
        /// <param name="job">工作信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetJob(Job job, bool init = true)
        {
            this.job.SetValue(job, init);
        }

        #endregion

        #region 设置执行计划

        /// <summary>
        /// 设置执行计划
        /// </summary>
        /// <param name="trigger">执行计划信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetTrigger(Trigger trigger, bool init = true)
        {
            this.trigger.SetValue(trigger, init);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitIdentityValue()
        {
            base.InitIdentityValue();
            id = GenerateExecuteLogId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载工作

        /// <summary>
        /// 加载工作信息
        /// </summary>
        /// <returns></returns>
        Job LoadJob()
        {
            if (!AllowLazyLoad(r => r.Job))
            {
                return job.CurrentValue;
            }
            if (job.CurrentValue == null)
            {
                return job.CurrentValue;
            }
            return jobService.GetJob(job.CurrentValue.Id);
        }

        #endregion

        #region 加载执行计划

        Trigger LoadTrigger()
        {
            if (!AllowLazyLoad(r => r.Trigger))
            {
                return trigger.CurrentValue;
            }
            if (trigger.CurrentValue == null)
            {
                return trigger.CurrentValue;
            }
            return triggerService.GetTrigger(trigger.CurrentValue.Id);
        }

        #endregion

        #region 加载服务

        ServerNode LoadServer()
        {
            if (!AllowLazyLoad(r => r.Server))
            {
                return server.CurrentValue;
            }
            if (server.CurrentValue == null)
            {
                return server.CurrentValue;
            }
            return serverNodeService.GetServerNode(server.CurrentValue.Id);
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return id <= 0;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个任务执行日志编号

        /// <summary>
        /// 生成一个任务执行日志编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateExecuteLogId()
        {
            return SerialNumber.GetSerialNumber(CTaskApplicationHelper.GetIdGroupCode(TaskIdGroup.执行日志));
        }

        #endregion

        #region 创建任务执行日志

        /// <summary>
        /// 创建一个任务执行日志对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static ExecuteLog CreateExecuteLog(long id = 0)
        {
            id = id <= 0 ? GenerateExecuteLogId() : id;
            return new ExecuteLog(id);
        }

        protected override string GetIdentityValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #endregion
    }
}