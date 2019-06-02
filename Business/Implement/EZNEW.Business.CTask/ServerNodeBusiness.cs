using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.Domain.CTask.Repository;
using EZNEW.BusinessContract.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Query.CTask;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Domain.CTask.Service;
using EZNEW.Framework.Response;

namespace EZNEW.Business.CTask
{
    /// <summary>
    /// 服务节点业务
    /// </summary>
    public class ServerNodeBusiness : IServerNodeBusiness
    {
        public ServerNodeBusiness()
        {
        }

        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="saveInfo">服务节点对象</param>
        /// <returns>执行结果</returns>
        public Result<ServerNodeDto> SaveServerNode(SaveServerNodeCmdDto saveInfo)
        {
            using (var businessWork = WorkFactory.Create())
            {
                if (saveInfo == null || saveInfo.ServerNode == null)
                {
                    return Result<ServerNodeDto>.FailedResult("服务节点信息为空");
                }
                var serverNode = saveInfo.ServerNode.MapTo<ServerNode>();
                ServerNodeDomainService.SaveServerNode(serverNode);
                var commitResult = businessWork.Commit();
                Result<ServerNodeDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<ServerNodeDto>.SuccessResult("保存成功");
                    result.Data = serverNode.MapTo<ServerNodeDto>();
                }
                else
                {
                    result = Result<ServerNodeDto>.FailedResult("保存失败");
                }
                #region 远程命令

                if (result.Success)
                {
                    TaskCommandBusiness.RefreshServiceAsync(new List<string>() { result.Object?.Id });
                }

                #endregion

                return result;
            }
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
            var serverNode = ServerNodeDomainService.GetServerNode(CreateQueryObject(filter));
            return serverNode.MapTo<ServerNodeDto>();
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
            var serverNodeList = ServerNodeDomainService.GetServerNodeList(CreateQueryObject(filter));
            return serverNodeList.Select(c => c.MapTo<ServerNodeDto>()).ToList();
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
            var serverNodePaging = ServerNodeDomainService.GetServerNodePaging(CreateQueryObject(filter));
            return serverNodePaging.ConvertTo<ServerNodeDto>();
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
            using (var businessWork = WorkFactory.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.ServerNodeIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的服务节点");
                }

                #endregion

                var nowServers =ServerNodeDomainService.GetServerNodeList(QueryFactory.Create<ServerNodeQuery>(c => deleteInfo.ServerNodeIds.Contains(c.Id)));
                //删除逻辑
                ServerNodeDomainService.DeleteServerNode(deleteInfo.ServerNodeIds);
                var commitResult = businessWork.Commit();

                #region 远程命令

                if (commitResult.ExecutedSuccess)
                {
                    TaskCommandBusiness.RemoveServiceAsync(nowServers);
                }

                #endregion

                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
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
            using (var businessWork = WorkFactory.Create())
            {
                if (stateInfo == null || stateInfo.Servers.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要修改的服务信息");
                }
                var servers = stateInfo.Servers.Select(c => c.MapTo<ServerNode>());
                ServerNodeDomainService.ModifyServerNodeRunState(servers);
                var commitResult = businessWork.Commit();
                #region 远程命令

                if (commitResult.ExecutedSuccess)
                {
                    TaskCommandBusiness.RefreshServiceAsync(stateInfo.Servers.Select(c => c.Id));
                }

                #endregion
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IQuery CreateQueryObject(ServerNodeFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<ServerNodeQuery>(filter);
            if (!filter.Ids.IsNullOrEmpty())
            {
                query.In<ServerNodeQuery>(c => c.Id, filter.Ids);
            }
            if (!filter.InstanceName.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.InstanceName, filter.InstanceName);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Like<ServerNodeQuery>(c => c.Name, filter.Name);
            }
            if (filter.Status.HasValue)
            {
                query.Equal<ServerNodeQuery>(c => c.Status, filter.Status.Value);
            }
            if (!filter.Host.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.Host, filter.Host);
            }
            if (filter.ThreadCount.HasValue)
            {
                query.Equal<ServerNodeQuery>(c => c.ThreadCount, filter.ThreadCount.Value);
            }
            if (!filter.ThreadPriority.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.ThreadPriority, filter.ThreadPriority);
            }
            if (!filter.Description.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.Description, filter.Description);
            }
            return query;
        }

        #endregion
    }
}
