using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 任务执行日志服务
    /// </summary>
    public interface IExecuteLogService
    {
        #region 保存执行日志

        /// <summary>
        /// 保存执行日志
        /// </summary>
        /// <param name="logs">日志信息</param>
        void SaveExecuteLog(IEnumerable<ExecuteLog> logs);

        #endregion

        #region 获取任务执行日志

        /// <summary>
        /// 获取任务执行日志
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        ExecuteLog GetExecuteLog(IQuery query);

        #endregion

        #region 获取任务执行日志列表

        /// <summary>
        /// 获取任务执行日志列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        List<ExecuteLog> GetExecuteLogList(IQuery query);

        #endregion

        #region 获取任务执行日志分页

        /// <summary>
        /// 获取任务执行日志分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<ExecuteLog> GetExecuteLogPaging(IQuery query);

        #endregion
    }
}
