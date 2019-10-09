using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework;
using EZNEW.BusinessContract.CTask;
using EZNEW.Framework.Response;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Framework.Paging;
using EZNEW.AppServiceContract.CTask;

namespace EZNEW.AppService.CTask
{
    /// <summary>
    /// 任务异常日志服务
    /// </summary>
    public class ErrorLogAppService : IErrorLogAppService
    {
        IErrorLogBusiness errorLogBusiness = null;

        public ErrorLogAppService(IErrorLogBusiness errorLogBusiness)
        {
            this.errorLogBusiness = errorLogBusiness;
        }

        #region 保存任务异常日志

        /// <summary>
        /// 保存任务异常日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveErrorLog(SaveErrorLogCmdDto saveInfo)
        {
            return errorLogBusiness.SaveErrorLog(saveInfo);
        }

        #endregion

        #region 获取任务异常日志

        /// <summary>
        /// 获取任务异常日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ErrorLogDto GetErrorLog(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLog(filter);
        }

        #endregion

        #region 获取任务异常日志列表

        /// <summary>
        /// 获取任务异常日志列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<ErrorLogDto> GetErrorLogList(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLogList(filter);
        }

        #endregion

        #region 获取任务异常日志分页

        /// <summary>
        /// 获取任务异常日志分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<ErrorLogDto> GetErrorLogPaging(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLogPaging(filter);
        }

        #endregion

    }
}
