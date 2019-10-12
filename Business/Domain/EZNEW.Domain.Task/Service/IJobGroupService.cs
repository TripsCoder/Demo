using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 任务分组服务
    /// </summary>
    public interface IJobGroupService
    {
        #region 删除任务分组

        /// <summary>
        /// 删除任务分组
        /// </summary>
        /// <param name="jobGroups">任务分组信息</param>
        void DeleteJobGroup(IEnumerable<JobGroup> jobGroups);

        /// <summary>
        /// 删除任务分组
        /// </summary>
        /// <param name="groupIds">任务分组编号</param>
        void DeleteJobGroup(IEnumerable<string> groupIds);

        #endregion

        #region 保存工作分组

        /// <summary>
        /// 保存工作分组
        /// </summary>
        /// <param name="jobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        void SaveJobGroup(JobGroup jobGroup);

        #endregion

        #region 获取工作分组

        /// <summary>
        /// 获取工作分组
        /// </summary>
        /// <param name="query">筛选信息</param>
        /// <returns></returns>
        JobGroup GetJobGroup(IQuery query);

        #endregion

        #region 获取工作分组列表

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        List<JobGroup> GetJobGroupList(IQuery query);

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="groupCodes">分组编码信息</param>
        /// <returns></returns>
        List<JobGroup> GetJobGroupList(IEnumerable<string> groupCodes);

        #endregion

        #region 获取工作分组分页

        /// <summary>
        /// 获取工作分组分页
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        IPaging<JobGroup> GetJobGroupPaging(IQuery query);

        #endregion
    }
}
