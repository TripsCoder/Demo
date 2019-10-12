using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.Domain.CTask.Repository;
using EZNEW.BusinessContract.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Model;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.CTask.Service;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Query.CTask;
using EZNEW.Framework.Response;
using EZNEW.Framework.IoC;

namespace EZNEW.Business.CTask
{
    /// <summary>
    /// 工作承载节点业务
    /// </summary>
    public class JobServerHostBusiness : IJobServerHostBusiness
    {
        IJobServerHostService jobServerHostService = ContainerManager.Resolve<IJobServerHostService>();

        public JobServerHostBusiness()
        {
        }

        #region 保存工作承载节点

        /// <summary>
        /// 保存工作承载节点
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveJobServerHost(SaveJobServerHostCmdDto saveInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (saveInfo == null || saveInfo.JobServerHosts.IsNullOrEmpty())
                {
                    return Result.FailedResult("工作承载保存信息为空");
                }
                List<JobServerHost> jobServerHostList = saveInfo.JobServerHosts.Select(c => { var jobServer = JobServerHost.CreateJobServerHost(c.Job?.Id, c.Server?.Id); jobServer.RunStatus = c.RunStatus; return jobServer; }).ToList();
                jobServerHostService.SaveJobServerHost(jobServerHostList);
                var commitResult = businessWork.Commit();
                var result = commitResult.ExecutedSuccess ? Result.SuccessResult("保存成功") : Result.FailedResult("保存失败");
                return result;
            }
        }

        #endregion

        #region 获取工作承载节点

        /// <summary>
        /// 获取工作承载节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobServerHostDto GetJobServerHost(JobServerHostFilterDto filter)
        {
            var jobServerHost = jobServerHostService.GetJobServerHost(CreateQueryObject(filter));
            return jobServerHost.MapTo<JobServerHostDto>();
        }

        #endregion

        #region 获取工作承载节点列表

        /// <summary>
        /// 获取工作承载节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobServerHostDto> GetJobServerHostList(JobServerHostFilterDto filter)
        {
            var jobServerHostList = jobServerHostService.GetJobServerHostList(CreateQueryObject(filter));
            return jobServerHostList.Select(c => c.MapTo<JobServerHostDto>()).ToList();
        }

        #endregion

        #region 获取工作承载节点分页

        /// <summary>
        /// 获取工作承载节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobServerHostDto> GetJobServerHostPaging(JobServerHostFilterDto filter)
        {
            var jobServerHostPaging = jobServerHostService.GetJobServerHostPaging(CreateQueryObject(filter));
            return jobServerHostPaging.ConvertTo<JobServerHostDto>();
        }

        #endregion

        #region 删除工作承载节点

        /// <summary>
        /// 删除工作承载节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobServerHost(DeleteJobServerHostCmdDto deleteInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (deleteInfo == null || deleteInfo.JobServerHosts.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的信息");
                }
                List<JobServerHost> serverHosts = deleteInfo.JobServerHosts.Select(c => JobServerHost.CreateJobServerHost(c.Job?.Id, c.Server?.Id)).ToList();
                jobServerHostService.DeleteJobServerHost(serverHosts);
                var commitResult = businessWork.Commit();
                var result = commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
                return result;
            }
        }

        #endregion

        #region 修改承载服务运行状态

        /// <summary>
        /// 修改承载服务运行状态
        /// </summary>
        /// <param name="modifyInfo">修改信息</param>
        /// <returns></returns>
        public Result ModifyRunState(ModifyJobServerHostRunStatusCmdDto modifyInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (modifyInfo == null || modifyInfo.JobServerHosts.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要修改的信息");
                }
                var jobServerHostList = modifyInfo.JobServerHosts.Select(c => c.MapTo<JobServerHost>());
                jobServerHostService.ModifyRunState(jobServerHostList);
                var commitResult = businessWork.Commit();
                var result = commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
                return result;
            }
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(JobServerHostFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create(filter);
            if (!filter.Servers.IsNullOrEmpty())
            {
                query.In<JobServerHostQuery>(c => c.Server, filter.Servers);
            }
            if (!filter.Jobs.IsNullOrEmpty())
            {
                query.In<JobServerHostQuery>(c => c.Job, filter.Jobs);
            }
            if (filter.RunStatus.HasValue)
            {
                query.Equal<JobServerHostQuery>(c => c.RunStatus, filter.RunStatus.Value);
            }
            if (!filter.ServerKey.IsNullOrEmpty())
            {
                IQuery serverQuery = QueryFactory.Create<ServerNodeQuery>();
                serverQuery.And<ServerNodeQuery>(QueryOperator.OR, CriteriaOperator.Like, filter.ServerKey, null, c => c.Name, c => c.Host);
                serverQuery.AddQueryFields<ServerNodeQuery>(c => c.Id);
                query.And<JobServerHostQuery>(c => c.Server, CriteriaOperator.In, serverQuery);
            }
            if (!filter.JobKey.IsNullOrEmpty())
            {
                IQuery jobQuery = QueryFactory.Create<JobQuery>();
                jobQuery.And<JobQuery>(QueryOperator.OR, CriteriaOperator.Like, filter.JobKey, null, c => c.Name);
                jobQuery.AddQueryFields<JobQuery>(c => c.Id);
                query.And<JobServerHostQuery>(c => c.Job, CriteriaOperator.In, jobQuery);
            }
            return query;
        }

        #endregion
    }
}
