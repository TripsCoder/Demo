using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 任务服务
    /// </summary>
    public interface IJobService
    {
        #region 删除工作任务

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="jobs">工作信息</param>
        void DeleteJob(IEnumerable<Job> jobs);

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="jobIds">任务编号</param>
        void DeleteJob(IEnumerable<string> jobIds);

        #endregion

        #region 修改工作任务运行状态

        /// <summary>
        /// 修改工作任务运行状态
        /// </summary>
        /// <param name="jobs">工作信息</param>
        void ModifyJobRunState(IEnumerable<Job> jobs);

        #endregion

        #region 获取工作任务

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        Job GetJob(IQuery query);

        /// <summary>
        /// 根据工作编号获取工作任务
        /// </summary>
        /// <param name="id">工作编号</param>
        /// <returns>工作任务信息</returns>
        Job GetJob(string id);

        #endregion

        #region 获取工作任务列表

        /// <summary>
        /// 获取工作任务列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        List<Job> GetJobList(IQuery query);

        /// <summary>
        /// 根据工作编号获取工作任务
        /// </summary>
        /// <param name="ids">工作编号</param>
        /// <returns>工作任务信息</returns>
        List<Job> GetJobList(IEnumerable<string> ids);

        #endregion

        #region 获取工作任务分页

        /// <summary>
        /// 获取工作任务分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<Job> GetJobPaging(IQuery query);

        #endregion

        #region 保存工作任务

        /// <summary>
        /// 保存工作任务
        /// </summary>
        /// <param name="job">任务信息</param>
        /// <returns>执行结果</returns>
        void SaveJob(Job job);

        #endregion
    }
}
