using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Framework.ValueType;
using EZNEW.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    public class JobServerHost : AggregationRoot<JobServerHost>
    {
        #region	字段

        /// <summary>
        /// 服务
        /// </summary>
        protected LazyMember<ServerNode> server;

        /// <summary>
        /// 任务
        /// </summary>
        protected LazyMember<Job> job;

        /// <summary>
        /// 运行状态
        /// </summary>
        protected JobServerStatus runStatus;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化工作承载节点对象
        /// </summary>
        /// <param name="server">编号</param>
        internal JobServerHost()
        {
            server = new LazyMember<ServerNode>(LoadServer);
            job = new LazyMember<Job>(LoadJob);
            repository = this.Instance<IJobServerHostRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 服务
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
        /// 任务
        /// </summary>
        public Job Job
        {
            get
            {
                return job.Value;
            }
            protected set
            {
                job.SetValue(value, false); ;
            }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerStatus RunStatus
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

        #endregion

        #region 方法

        #region 功能方法

        #region 设置工作任务

        /// <summary>
        /// 设置工作任务
        /// </summary>
        /// <param name="job">工作信息</param>
        /// <param name="init">是否初始化，初始化后将不会自动加载数据</param>
        public void SetJob(Job job, bool init = true)
        {
            this.job.SetValue(job, init);
        }

        #endregion

        #region 设置服务信息

        /// <summary>
        /// 设置服务信息
        /// </summary>
        /// <param name="server">服务信息</param>
        /// <param name="init">是否初始化，初始化后将不会自动加载数据</param>
        public void SetServer(ServerNode server, bool init = true)
        {
            this.server.SetValue(server, init);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载服务

        /// <summary>
        /// 加载服务
        /// </summary>
        /// <returns></returns>
        ServerNode LoadServer()
        {
            if (!AllowLazyLoad(r => r.Server))
            {
                return server.CurrentValue;
            }
            if (server.CurrentValue == null || server.CurrentValue.Id.IsNullOrEmpty())
            {
                return server.CurrentValue;
            }
            return this.Instance<IServerNodeRepository>().Get(QueryFactory.Create<ServerNodeQuery>(r => r.Id == server.CurrentValue.Id));
        }

        #endregion

        #region 加载工作信息

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
            if (job.CurrentValue == null || job.CurrentValue.Id.IsNullOrEmpty())
            {
                return job.CurrentValue;
            }
            return this.Instance<IJobRepository>().Get(QueryFactory.Create<JobQuery>(r => r.Id == job.CurrentValue.Id));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return (job.Value?.Id.IsNullOrEmpty() ?? true) || (server.Value?.Id.IsNullOrEmpty() ?? true);
        }

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建任务&服务承载信息
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="serverId">服务编号</param>
        /// <returns></returns>
        public static JobServerHost CreateJobServerHost(string jobId, string serverId)
        {
            var jobServerHost = new JobServerHost();
            jobServerHost.SetJob(Job.CreateJob(jobId));
            jobServerHost.SetServer(ServerNode.CreateServerNode(serverId));
            return jobServerHost;
        }

        protected override string GetIdentityValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }

    public class JobServerHostCompare : IEqualityComparer<JobServerHost>
    {
        public bool Equals(JobServerHost x, JobServerHost y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Server?.Id == y.Server?.Id && x.Job?.Id == y.Job?.Id;
        }

        public int GetHashCode(JobServerHost obj)
        {
            return 0;
        }
    }
}