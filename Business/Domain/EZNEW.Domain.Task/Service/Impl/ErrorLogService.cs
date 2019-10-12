using EZNEW.Domain.CTask.Model;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;

namespace EZNEW.Domain.CTask.Service.Impl
{
    /// <summary>
    /// 错误日志服务信息
    /// </summary>
    public class ErrorLogService : IErrorLogService
    {
        static IErrorLogRepository errorLogRepository = ContainerManager.Resolve<IErrorLogRepository>();
        static IServerNodeService serverNodeService = ContainerManager.Resolve<IServerNodeService>();
        static IJobService jobService = ContainerManager.Resolve<IJobService>();
        static ITriggerService triggerService = ContainerManager.Resolve<ITriggerService>();

        #region 保存错误日志

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="logs">日志信息</param>
        public void SaveErrorLog(IEnumerable<ErrorLog> logs)
        {
            if (logs.IsNullOrEmpty())
            {
                return;
            }
            errorLogRepository.Save(logs.ToArray());
        }

        #endregion

        #region 获取任务异常日志

        /// <summary>
        /// 获取任务异常日志
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ErrorLog GetErrorLog(IQuery query)
        {
            var errorLog = errorLogRepository.Get(query);
            return errorLog;
        }

        #endregion

        #region 获取任务异常日志列表

        /// <summary>
        /// 获取任务异常日志列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public List<ErrorLog> GetErrorLogList(IQuery query)
        {
            var errorLogList = errorLogRepository.GetList(query);
            errorLogList = LoadOtherObjectData(errorLogList, query);
            return errorLogList;
        }

        #endregion

        #region 获取任务异常日志分页

        /// <summary>
        /// 获取任务异常日志分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public IPaging<ErrorLog> GetErrorLogPaging(IQuery query)
        {
            var errorLogPaging = errorLogRepository.GetPaging(query);
            var logList = LoadOtherObjectData(errorLogPaging, query);
            return new Paging<ErrorLog>(errorLogPaging.Page, errorLogPaging.PageSize, errorLogPaging.TotalCount, logList);
        }

        #endregion

        #region 数据列表加载额外的对象数据

        /// <summary>
        /// 数据列表加载额外的对象数据
        /// </summary>
        /// <param name="datas">数据信息</param>
        /// <param name="query">查询对象</param>
        List<ErrorLog> LoadOtherObjectData(IEnumerable<ErrorLog> datas, IQuery query)
        {
            #region 参数判断

            if (datas.IsNullOrEmpty())
            {
                return new List<ErrorLog>(0);
            }
            if (query == null || query.LoadPropertys == null || query.LoadPropertys.Count <= 0)
            {
                return datas.ToList();
            }

            #endregion

            #region 服务信息

            List<ServerNode> serverList = null;
            if (query.AllowLoad<ErrorLog>(c => c.Server))
            {
                List<string> serverIds = datas.Select(c => c.Server?.Id).Distinct().ToList();
                serverList = serverNodeService.GetServerNodeList(serverIds);
            }

            #endregion

            #region 工作信息

            List<Job> jobList = null;
            if (query.AllowLoad<ErrorLog>(c => c.Job))
            {
                List<string> jobIds = datas.Select(c => c.Job?.Id).Distinct().ToList();
                jobList = jobService.GetJobList(jobIds);
            }

            #endregion

            #region 执行计划

            List<Trigger> triggerList = null;
            if (query.AllowLoad<ErrorLog>(c => c.Trigger))
            {
                List<string> triggerIds = datas.Select(c => c.Trigger?.Id).Distinct().ToList();
                triggerList = triggerService.GetTriggerList(triggerIds);
            }

            #endregion

            foreach (var log in datas)
            {
                if (!serverList.IsNullOrEmpty())
                {
                    log.SetServer(serverList.FirstOrDefault(c => c.Id == log.Server?.Id));
                }
                if (!jobList.IsNullOrEmpty())
                {
                    log.SetJob(jobList.FirstOrDefault(c => c.Id == log.Job?.Id));
                }
                if (!triggerList.IsNullOrEmpty())
                {
                    log.SetTrigger(triggerList.FirstOrDefault(c => c.Id == log.Trigger?.Id));
                }
            }
            return datas.ToList();
        }

        #endregion
    }
}
