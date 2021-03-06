﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.BusinessContract.CTask;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Framework.Paging;
using EZNEW.Framework.Response;
using EZNEW.AppServiceContract.CTask;

namespace EZNEW.AppService.CTask
{
    /// <summary>
    /// 工作分组服务
    /// </summary>
    public class JobGroupAppService : IJobGroupAppService
    {
        IJobGroupBusiness jobGroupBusiness = null;

        public JobGroupAppService(IJobGroupBusiness jobGroupBusiness)
        {
            this.jobGroupBusiness = jobGroupBusiness;
        }

        #region 保存工作分组

        /// <summary>
        /// 保存工作分组
        /// </summary>
        /// <param name="jobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        public Result<JobGroupDto> SaveJobGroup(JobGroupCmdDto jobGroup)
        {
            return jobGroupBusiness.SaveJobGroup(jobGroup);
        }

        #endregion

        #region 获取工作分组

        /// <summary>
        /// 获取工作分组
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public JobGroupDto GetJobGroup(JobGroupFilterDto filter)
        {
            return jobGroupBusiness.GetJobGroup(filter);
        }

        #endregion

        #region 获取工作分组列表

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public List<JobGroupDto> GetJobGroupList(JobGroupFilterDto filter)
        {
            return jobGroupBusiness.GetJobGroupList(filter);
        }

        #endregion

        #region 获取工作分组分页

        /// <summary>
        /// 获取工作分组分页
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public IPaging<JobGroupDto> GetJobGroupPaging(JobGroupFilterDto filter)
        {
            return jobGroupBusiness.GetJobGroupPaging(filter);
        }

        #endregion

        #region 删除工作分组

        /// <summary>
        /// 删除工作分组
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobGroup(DeleteJobGroupCmdDto deleteInfo)
        {
            return jobGroupBusiness.DeleteJobGroup(deleteInfo);
        }

        #endregion

        #region 修改工作分组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>执行结果</returns>
        public Result ModifySortIndex(ModifyJobGroupSortCmdDto sortInfo)
        {
            return jobGroupBusiness.ModifySortIndex(sortInfo);
        }

        #endregion

    }
}
