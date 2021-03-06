﻿using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Query.CTask;
using EZNEW.Domain.CTask.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Model;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.CTask.Service;
using EZNEW.BusinessContract.CTask;
using EZNEW.Framework.Response;
using EZNEW.Framework.IoC;

namespace EZNEW.Business.CTask
{
    /// <summary>
    /// 任务异常日志业务
    /// </summary>
    public class ErrorLogBusiness : IErrorLogBusiness
    {
        IErrorLogService errorLogService = ContainerManager.Resolve<IErrorLogService>();

        public ErrorLogBusiness()
        {
        }

        #region 保存任务异常日志

        /// <summary>
        /// 保存任务异常日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveErrorLog(SaveErrorLogCmdDto saveInfo)
        {
            if (saveInfo == null || saveInfo.ErrorLogs.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要保存的信息");
            }
            using (var businessWork = WorkFactory.Create())
            {
                errorLogService.SaveErrorLog(saveInfo.ErrorLogs.Select(c => { c.Id = ErrorLog.GenerateErrorLogId(); return c.MapTo<ErrorLog>(); }));
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("保存成功") : Result.FailedResult("保存失败");
            }
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
            var errorLog = errorLogService.GetErrorLog(CreateQueryObject(filter));
            return errorLog.MapTo<ErrorLogDto>();
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
            var errorLogList = errorLogService.GetErrorLogList(CreateQueryObject(filter));
            return errorLogList.Select(c => c.MapTo<ErrorLogDto>()).ToList();
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
            var errorLogPaging = errorLogService.GetErrorLogPaging(CreateQueryObject(filter));
            return errorLogPaging.ConvertTo<ErrorLogDto>();
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(ErrorLogFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create(filter);

            #region 筛选条件

            if (!filter.Ids.IsNullOrEmpty())
            {
                query.In<ErrorLogQuery>(c => c.Id, filter.Ids);
            }
            if (!filter.Server.IsNullOrEmpty())
            {
                query.Equal<ErrorLogQuery>(c => c.Server, filter.Server);
            }
            if (!filter.Job.IsNullOrEmpty())
            {
                query.Equal<ErrorLogQuery>(c => c.Job, filter.Job);
            }
            if (!filter.Trigger.IsNullOrEmpty())
            {
                query.Equal<ErrorLogQuery>(c => c.Trigger, filter.Trigger);
            }
            if (!filter.Message.IsNullOrEmpty())
            {
                query.Equal<ErrorLogQuery>(c => c.Message, filter.Message);
            }
            if (!filter.Description.IsNullOrEmpty())
            {
                query.Equal<ErrorLogQuery>(c => c.Description, filter.Description);
            }
            if (filter.Type.HasValue)
            {
                query.Equal<ErrorLogQuery>(c => c.Type, filter.Type.Value);
            }
            if (filter.BeginDate.HasValue)
            {
                query.GreaterThanOrEqual<ErrorLogQuery>(c => c.Date, filter.BeginDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                query.LessThanOrEqual<ErrorLogQuery>(c => c.Date, filter.EndDate.Value);
            }

            #endregion

            #region 数据加载

            if (filter.LoadServer)
            {
                query.SetLoadPropertys<ErrorLog>(true, c => c.Server);
            }
            if (filter.LoadJob)
            {
                query.SetLoadPropertys<ErrorLog>(true, c => c.Job);
            }
            if (filter.LoadTrigger)
            {
                query.SetLoadPropertys<ErrorLog>(true, c => c.Trigger);
            }

            #endregion

            #region 排序

            query.Desc<ErrorLogQuery>(c => c.Date);

            #endregion

            return query;
        }

        #endregion
    }
}
