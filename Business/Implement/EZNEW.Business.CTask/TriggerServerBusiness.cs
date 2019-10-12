using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Query.CTask;
using EZNEW.Domain.CTask.Repository;
using EZNEW.BusinessContract.CTask;
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
using EZNEW.Framework.Response;
using EZNEW.Framework.IoC;

namespace EZNEW.Business.CTask
{
    /// <summary>
    /// 服务节点执行计划业务
    /// </summary>
    public class TriggerServerBusiness : ITriggerServerBusiness
    {
        ITriggerServerService triggerServerService = ContainerManager.Resolve<ITriggerServerService>();

        public TriggerServerBusiness()
        {
        }

        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public TriggerServerDto GetTriggerServer(TriggerServerFilterDto filter)
        {
            var triggerServer = triggerServerService.GetTriggerServer(CreateQueryObject(filter));
            return triggerServer.MapTo<TriggerServerDto>();
        }

        #endregion

        #region 获取服务节点执行计划列表

        /// <summary>
        /// 获取服务节点执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<TriggerServerDto> GetTriggerServerList(TriggerServerFilterDto filter)
        {
            var triggerServerList = triggerServerService.GetTriggerServerList(CreateQueryObject(filter));
            return triggerServerList.Select(c => c.MapTo<TriggerServerDto>()).ToList();
        }

        #endregion

        #region 获取服务节点执行计划分页

        /// <summary>
        /// 获取服务节点执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<TriggerServerDto> GetTriggerServerPaging(TriggerServerFilterDto filter)
        {
            var triggerServerPaging = triggerServerService.GetTriggerServerPaging(CreateQueryObject(filter));
            return triggerServerPaging.ConvertTo<TriggerServerDto>();
        }

        #endregion

        #region 删除服务节点执行计划

        /// <summary>
        /// 删除服务节点执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteTriggerServer(DeleteTriggerServerCmdDto deleteInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.TriggerServers.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的服务节点执行计划");
                }

                #endregion

                triggerServerService.DeleteTriggerServer(deleteInfo.TriggerServers.Select(c => c.MapTo<TriggerServer>()));
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改计划服务运行状态

        /// <summary>
        /// 修改计划服务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyRunState(ModifyTriggerServerRunStatusCmdDto stateInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (stateInfo == null || stateInfo.TriggerServers.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要修改的信息");
                }
                triggerServerService.ModifyRunState(stateInfo.TriggerServers.Select(c => c.MapTo<TriggerServer>()));
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 保存服务节点执行计划

        /// <summary>
        /// 保存服务节点执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns></returns>
        public Result SaveTriggerServer(SaveTriggerServerCmdDto saveInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (saveInfo == null || saveInfo.TriggerServers.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定任何要保存的信息");
                }
                triggerServerService.SaveTriggerServer(saveInfo.TriggerServers.Select(c => c.MapTo<TriggerServer>()));
                var commitResult = businessWork.Commit();
                return commitResult.EmptyResultOrSuccess ? Result.SuccessResult("保存成功") : Result.FailedResult("保存失败");
            }
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(TriggerServerFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<TriggerServerQuery>(filter);
            if (!filter.Triggers.IsNullOrEmpty())
            {
                query.In<TriggerServerQuery>(c => c.Trigger, filter.Triggers);
            }
            if (!filter.Servers.IsNullOrEmpty())
            {
                query.In<TriggerServerQuery>(c => c.Server, filter.Servers);
            }
            if (filter.RunStatus.HasValue)
            {
                query.Equal<TriggerServerQuery>(c => c.RunStatus, filter.RunStatus.Value);
            }
            if (filter.LastFireDate.HasValue)
            {
                query.Equal<TriggerServerQuery>(c => c.LastFireDate, filter.LastFireDate.Value);
            }
            if (filter.NextFireDate.HasValue)
            {
                query.Equal<TriggerServerQuery>(c => c.NextFireDate, filter.NextFireDate.Value);
            }
            return query;
        }

        #endregion
    }
}
