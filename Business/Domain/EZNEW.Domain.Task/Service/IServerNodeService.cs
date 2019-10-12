using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 服务节点服务
    /// </summary>
    public interface IServerNodeService
    {
        #region 删除服务节点

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="servers">服务信息</param>
        void DeleteServerNode(IEnumerable<ServerNode> servers);

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="serverCodes">服务编码</param>
        void DeleteServerNode(IEnumerable<string> serverCodes);

        #endregion

        #region 获取服务节点

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>服务节点</returns>
        ServerNode GetServerNode(IQuery query);

        /// <summary>
        /// 根据编号获取服务节点
        /// </summary>
        /// <param name="id">服务编号</param>
        /// <returns>服务节点</returns>
        ServerNode GetServerNode(string id);

        #endregion

        #region 获取服务节点列表

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>服务节点列表</returns>
        List<ServerNode> GetServerNodeList(IQuery query);

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="ids">节点列表</param>
        /// <returns>节点列表</returns>
        List<ServerNode> GetServerNodeList(IEnumerable<string> ids);

        #endregion

        #region 获取服务节点分页

        /// <summary>
        /// 获取服务节点分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<ServerNode> GetServerNodePaging(IQuery query);

        #endregion

        #region 修改服务节点运行状态

        /// <summary>
        /// 修改服务节点运行状态
        /// </summary>
        /// <param name="servers">要操作的服务信息</param>
        void ModifyServerNodeRunState(IEnumerable<ServerNode> servers);

        #endregion

        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="server">服务节点对象</param>
        /// <returns>执行结果</returns>
        void SaveServerNode(ServerNode server);

        #endregion
    }
}
