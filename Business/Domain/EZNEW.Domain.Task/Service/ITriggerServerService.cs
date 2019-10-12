using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 计划服务操作服务
    /// </summary>
    public interface ITriggerServerService
    {
        #region 删除执行计划服务节点

        /// <summary>
        /// 删除执行计划服务节点
        /// </summary>
        /// <param name="triggerServers">节点服务信息</param>
        void DeleteTriggerServer(IEnumerable<TriggerServer> triggerServers);

        #endregion

        #region 修改运行状态

        /// <summary>
        /// 修改运行状态
        /// </summary>
        /// <param name="triggerServers">节点服务信息</param>
        void ModifyRunState(IEnumerable<TriggerServer> triggerServers);

        #endregion

        #region 保存计划服务承载信息

        /// <summary>
        /// 保存计划服务承载信息
        /// </summary>
        /// <param name="newTriggerServers">计划服务信息</param>
        void SaveTriggerServer(IEnumerable<TriggerServer> newTriggerServers);

        #endregion

        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        TriggerServer GetTriggerServer(IQuery query);

        #endregion

        #region 获取服务节点执行计划列表

        /// <summary>
        /// 获取服务节点执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<TriggerServer> GetTriggerServerList(IQuery query);

        #endregion

        #region 获取服务节点执行计划分页

        /// <summary>
        /// 获取服务节点执行计划分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<TriggerServer> GetTriggerServerPaging(IQuery query);

        #endregion
    }
}
