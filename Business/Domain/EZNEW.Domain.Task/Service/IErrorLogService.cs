using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 错误日志服务信息
    /// </summary>
    public interface IErrorLogService
    {
        #region 保存错误日志

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="logs">日志信息</param>
        void SaveErrorLog(IEnumerable<ErrorLog> logs);

        #endregion

        #region 获取任务异常日志

        /// <summary>
        /// 获取任务异常日志
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        ErrorLog GetErrorLog(IQuery query);

        #endregion

        #region 获取任务异常日志列表

        /// <summary>
        /// 获取任务异常日志列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        List<ErrorLog> GetErrorLogList(IQuery query);

        #endregion

        #region 获取任务异常日志分页

        /// <summary>
        /// 获取任务异常日志分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<ErrorLog> GetErrorLogPaging(IQuery query);

        #endregion
    }
}
