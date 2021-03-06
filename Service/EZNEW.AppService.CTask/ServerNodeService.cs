﻿using EZNEW.AppServiceContract.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework;
using EZNEW.BusinessContract.CTask;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.Framework.Response;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Framework.Paging;

namespace EZNEW.AppService.CTask
{
    /// <summary>
    /// 服务节点服务
    /// </summary>
    public class ServerNodeAppService : IServerNodeAppService
    {
        IServerNodeBusiness serverNodeBusiness = null;

        public ServerNodeAppService(IServerNodeBusiness serverNodeBusiness)
        {
            this.serverNodeBusiness = serverNodeBusiness;
        }

        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<ServerNodeDto> SaveServerNode(SaveServerNodeCmdDto saveInfo)
        {
            return serverNodeBusiness.SaveServerNode(saveInfo);
        }

        #endregion

        #region 获取服务节点

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ServerNodeDto GetServerNode(ServerNodeFilterDto filter)
        {
            return serverNodeBusiness.GetServerNode(filter);
        }

        #endregion

        #region 获取服务节点列表

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<ServerNodeDto> GetServerNodeList(ServerNodeFilterDto filter)
        {
            return serverNodeBusiness.GetServerNodeList(filter);
        }

        #endregion

        #region 获取服务节点分页

        /// <summary>
        /// 获取服务节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<ServerNodeDto> GetServerNodePaging(ServerNodeFilterDto filter)
        {
            return serverNodeBusiness.GetServerNodePaging(filter);
        }

        #endregion

        #region 删除服务节点

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteServerNode(DeleteServerNodeCmdDto deleteInfo)
        {
            return serverNodeBusiness.DeleteServerNode(deleteInfo);
        }

        #endregion

        #region 修改服务节点运行状态

        /// <summary>
        /// 修改服务节点运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyRunState(ModifyServerNodeRunStatusCmdDto stateInfo)
        {
            return serverNodeBusiness.ModifyRunState(stateInfo);
        }

        #endregion
    }
}
