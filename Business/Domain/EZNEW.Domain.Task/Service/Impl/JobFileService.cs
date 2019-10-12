using EZNEW.Domain.CTask.Model;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework;
using EZNEW.Framework.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.Framework.Response;

namespace EZNEW.Domain.CTask.Service.Impl
{
    /// <summary>
    /// 任务工作文件服务
    /// </summary>
    public class JobFileService : IJobFileService
    {
        static IJobFileRepository jobFileRepository = ContainerManager.Container.Resolve<IJobFileRepository>();

        #region 保存

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="jobFile">任务工作文件信息</param>
        /// <returns></returns>
        public Result<JobFile> SaveJobFile(JobFile jobFile)
        {
            return null;
        }

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public JobFile GetJobFile(IQuery query)
        {
            var jobFile = jobFileRepository.Get(query);
            return jobFile;
        }

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public List<JobFile> GetJobFileList(IQuery query)
        {
            var jobFileList = jobFileRepository.GetList(query);
            return jobFileList;
        }

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public IPaging<JobFile> GetJobFilePaging(IQuery query)
        {
            var jobFilePaging = jobFileRepository.GetPaging(query);
            return jobFilePaging;
        }

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="jobFiles">要删出的信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobFile(IEnumerable<JobFile> jobFiles)
        {
            #region 参数判断

            if (jobFiles.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要删除的任务工作文件");
            }

            #endregion

            //删除逻辑
            return Result.SuccessResult("删除成功");
        }

        #endregion
    }
}
