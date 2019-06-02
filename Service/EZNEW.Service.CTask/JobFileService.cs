using EZNEW.BusinessContract.CTask;
using EZNEW.Framework.Paging;
using EZNEW.Framework.Response;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.ServiceContract.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Service.CTask
{
    /// <summary>
    /// 任务工作文件服务
    /// </summary>
    public class JobFileService : IJobFileService
    {
        IJobFileBusiness jobFileBusiness = null;

        public JobFileService(IJobFileBusiness jobFileBusiness)
        {
            this.jobFileBusiness = jobFileBusiness;
        }

        #region 保存任务工作文件

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<JobFileDto> SaveJobFile(SaveJobFileCmdDto saveInfo)
        {
            return jobFileBusiness.SaveJobFile(saveInfo);
        }

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobFileDto GetJobFile(JobFileFilterDto filter)
        {
            return jobFileBusiness.GetJobFile(filter);
        }

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobFileDto> GetJobFileList(JobFileFilterDto filter)
        {
            return jobFileBusiness.GetJobFileList(filter);
        }

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobFileDto> GetJobFilePaging(JobFileFilterDto filter)
        {
            return jobFileBusiness.GetJobFilePaging(filter);
        }

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobFile(DeleteJobFileCmdDto deleteInfo)
        {
            return jobFileBusiness.DeleteJobFile(deleteInfo);
        }

        #endregion

    }
}
