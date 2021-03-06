﻿using EZNEW.AppServiceContract.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework;
using EZNEW.BusinessContract.CTask;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.Framework.Response;

namespace EZNEW.AppService.CTask
{
    /// <summary>
    /// 服务节点执行计划服务
    /// </summary>
    public class TriggerServerAppService : ITriggerServerAppService
    {
        ITriggerServerBusiness triggerServerBusiness = null;
        public TriggerServerAppService(ITriggerServerBusiness triggerServerBusiness)
        {
            this.triggerServerBusiness = triggerServerBusiness;
        }

        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public TriggerServerDto GetTriggerServer(TriggerServerFilterDto filter)
        {
            return triggerServerBusiness.GetTriggerServer(filter);
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
            return triggerServerBusiness.GetTriggerServerList(filter);
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
            return triggerServerBusiness.GetTriggerServerPaging(filter);
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
            return triggerServerBusiness.DeleteTriggerServer(deleteInfo);
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
            return triggerServerBusiness.ModifyRunState(stateInfo);
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
            return triggerServerBusiness.SaveTriggerServer(saveInfo);
        }

        #endregion
    }
}
