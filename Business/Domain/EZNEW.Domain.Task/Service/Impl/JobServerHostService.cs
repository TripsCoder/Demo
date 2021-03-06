﻿using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.IoC;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Framework.Paging;

namespace EZNEW.Domain.CTask.Service.Impl
{
    /// <summary>
    /// 工作承载服务
    /// </summary>
    public class JobServerHostService : IJobServerHostService
    {
        static IJobServerHostRepository jobServerHostRepository = ContainerManager.Resolve<IJobServerHostRepository>();

        #region 保存工作承载信息

        /// <summary>
        /// 保存工作承载信息
        /// </summary>
        /// <param name="jobServerHosts">工作承载信息</param>
        public void SaveJobServerHost(IEnumerable<JobServerHost> jobServerHosts)
        {
            if (jobServerHosts.IsNullOrEmpty())
            {
                throw new Exception("没有指定任何要保存的信息");
            }
            IEnumerable<string> jobIds = jobServerHosts.Select(c => c.Job?.Id).Distinct();
            IEnumerable<string> serverIds = jobServerHosts.Select(c => c.Server?.Id).Distinct();
            IEnumerable<JobServerHost> nowServerHosts = jobServerHostRepository.GetList(QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job) && serverIds.Contains(c.Server)));
            jobServerHosts = jobServerHosts.Except(nowServerHosts, new JobServerHostCompare()).ToList();
            if (jobServerHosts.IsNullOrEmpty())
            {
                return;
            }
            jobServerHostRepository.Save(jobServerHosts.ToArray());
        }

        #endregion

        #region 删除工作承载信息

        /// <summary>
        /// 删除工作承载信息
        /// </summary>
        /// <param name="jobServerHosts"></param>
        public void DeleteJobServerHost(IEnumerable<JobServerHost> jobServerHosts)
        {
            if (jobServerHosts.IsNullOrEmpty())
            {
                throw new Exception("没有指定要删除的信息");
            }
            jobServerHostRepository.Remove(jobServerHosts.ToArray());
        }

        #endregion

        #region 修改工作承载运行状态

        /// <summary>
        /// 修改工作承载运行状态
        /// </summary>
        /// <param name="newStateJobServerHosts">新的状态信息</param>
        public void ModifyRunState(IEnumerable<JobServerHost> newStateJobServerHosts)
        {
            if (newStateJobServerHosts.IsNullOrEmpty())
            {
                throw new Exception("没有指定任何要修改的工作承载信息");
            }
            IQuery query = QueryFactory.Create<JobServerHostQuery>();
            foreach (var serverHost in newStateJobServerHosts)
            {
                if (serverHost == null || serverHost.Server == null || serverHost.Job == null)
                {
                    continue;
                }
                query.Or<JobServerHostQuery>(c => c.Job == serverHost.Job.Id && c.Server == serverHost.Server.Id);
            }
            List<JobServerHost> nowServerHostList = jobServerHostRepository.GetList(query);
            var jobServerHostCompare = new JobServerHostCompare();
            if (!newStateJobServerHosts.Except(nowServerHostList, jobServerHostCompare).IsNullOrEmpty())
            {
                throw new Exception("请指定正确的操作信息");
            }
            nowServerHostList.ForEach(c =>
            {
                var newStateObj = newStateJobServerHosts.FirstOrDefault(r => jobServerHostCompare.Equals(c, r));
                if (newStateObj != null)
                {
                    c.RunStatus = newStateObj.RunStatus;
                }
            });
            jobServerHostRepository.Save(nowServerHostList.ToArray());
        }

        #endregion

        #region 获取工作承载节点

        /// <summary>
        /// 获取工作承载节点
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public JobServerHost GetJobServerHost(IQuery query)
        {
            var jobServerHost = jobServerHostRepository.Get(query);
            return jobServerHost;
        }

        #endregion

        #region 获取工作承载节点列表

        /// <summary>
        /// 获取工作承载节点列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public List<JobServerHost> GetJobServerHostList(IQuery query)
        {
            var jobServerHostList = jobServerHostRepository.GetList(query);
            return jobServerHostList;
        }

        #endregion

        #region 获取工作承载节点分页

        /// <summary>
        /// 获取工作承载节点分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public IPaging<JobServerHost> GetJobServerHostPaging(IQuery query)
        {
            var jobServerHostPaging = jobServerHostRepository.GetPaging(query);
            return jobServerHostPaging;
        }

        #endregion
    }
}
