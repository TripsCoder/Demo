using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using EZNEW.Framework.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 任务工作文件服务
    /// </summary>
    public interface IJobFileService
    {
        #region 保存

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="jobFile">任务工作文件信息</param>
        /// <returns></returns>
        Result<JobFile> SaveJobFile(JobFile jobFile);

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        JobFile GetJobFile(IQuery query);

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        List<JobFile> GetJobFileList(IQuery query);

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<JobFile> GetJobFilePaging(IQuery query);

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="jobFiles">要删出的信息</param>
        /// <returns>执行结果</returns>
        Result DeleteJobFile(IEnumerable<JobFile> jobFiles);

        #endregion
    }
}
