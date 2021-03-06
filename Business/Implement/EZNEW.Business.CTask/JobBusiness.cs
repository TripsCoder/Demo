﻿using EZNEW.Develop.CQuery;
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
using EZNEW.Query.CTask;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Domain.CTask.Service;
using EZNEW.Framework.Response;
using EZNEW.Framework.IoC;

namespace EZNEW.Business.CTask
{
    /// <summary>
    /// 工作任务业务
    /// </summary>
    public class JobBusiness : IJobBusiness
    {
        IJobService jobService = ContainerManager.Resolve<IJobService>();

        public JobBusiness()
        {
        }

        #region 保存工作任务

        /// <summary>
        /// 保存工作任务
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<JobDto> SaveJob(SaveJobCmdDto saveInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (saveInfo == null || saveInfo.Job == null)
                {
                    return Result<JobDto>.FailedResult("任务保存信息不完整");
                }
                var job = saveInfo.Job.MapTo<Job>();
                jobService.SaveJob(job);
                Result<JobDto> result = null;
                var commitResult = businessWork.Commit();
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<JobDto>.SuccessResult("保存成功");
                    result.Data = job.MapTo<JobDto>();
                }
                else
                {
                    result = Result<JobDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取工作任务

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobDto GetJob(JobFilterDto filter)
        {
            var job = jobService.GetJob(CreateQueryObject(filter));
            return job.MapTo<JobDto>();
        }

        #endregion

        #region 获取工作任务列表

        /// <summary>
        /// 获取工作任务列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobDto> GetJobList(JobFilterDto filter)
        {
            var jobList = jobService.GetJobList(CreateQueryObject(filter));
            return jobList.Select(c => c.MapTo<JobDto>()).ToList();
        }

        #endregion

        #region 获取工作任务分页

        /// <summary>
        /// 获取工作任务分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobDto> GetJobPaging(JobFilterDto filter)
        {
            var jobPaging = jobService.GetJobPaging(CreateQueryObject(filter));
            return jobPaging.ConvertTo<JobDto>();
        }

        #endregion

        #region 删除工作任务

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJob(DeleteJobCmdDto deleteInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.JobIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的工作任务");
                }

                #endregion

                //删除逻辑
                jobService.DeleteJob(deleteInfo.JobIds);
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改工作任务运行状态

        /// <summary>
        /// 修改工作任务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyJobRunState(ModifyJobRunStatusCmdDto stateInfo)
        {
            if (stateInfo == null || stateInfo.Jobs.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要修改的任务信息");
            }
            using (var businessWork = WorkFactory.Create())
            {
                jobService.ModifyJobRunState(stateInfo.Jobs.Select(c => c.MapTo<Job>()));
                var commitResult = businessWork.Commit();
                Result result = commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
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
        public IQuery CreateQueryObject(JobFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<JobQuery>(filter);

            #region 筛选条件

            if (!filter.Ids.IsNullOrEmpty())
            {
                query.In<JobQuery>(c => c.Id, filter.Ids);
            }
            if (!filter.Group.IsNullOrEmpty())
            {
                query.Equal<JobQuery>(c => c.Group, filter.Group);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Like<JobQuery>(c => c.Name, filter.Name);
            }
            if (filter.Type.HasValue)
            {
                query.Equal<JobQuery>(c => c.Type, filter.Type.Value);
            }
            if (filter.RunType.HasValue)
            {
                query.Equal<JobQuery>(c => c.RunType, filter.RunType.Value);
            }
            if (filter.Status.HasValue)
            {
                query.Equal<JobQuery>(c => c.Status, filter.Status.Value);
            }
            if (!filter.Description.IsNullOrEmpty())
            {
                query.Equal<JobQuery>(c => c.Description, filter.Description);
            }
            if (filter.UpdateDate.HasValue)
            {
                query.Equal<JobQuery>(c => c.UpdateDate, filter.UpdateDate.Value);
            }
            if (!filter.JobPath.IsNullOrEmpty())
            {
                query.Equal<JobQuery>(c => c.JobPath, filter.JobPath);
            }
            if (!filter.JobFileName.IsNullOrEmpty())
            {
                query.Equal<JobQuery>(c => c.JobFileName, filter.JobFileName);
            }
            #endregion

            #region 数据加载

            if (filter.LoadGroup)
            {
                query.SetLoadPropertys<Job>(true, c => c.Group);
            }

            #endregion

            return query;
        }

        #endregion
    }
}
