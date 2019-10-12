using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 工作承载服务
    /// </summary>
    public interface IJobServerHostService
    {
        #region 保存工作承载信息

        /// <summary>
        /// 保存工作承载信息
        /// </summary>
        /// <param name="jobServerHosts">工作承载信息</param>
        void SaveJobServerHost(IEnumerable<JobServerHost> jobServerHosts);

        #endregion

        #region 删除工作承载信息

        /// <summary>
        /// 删除工作承载信息
        /// </summary>
        /// <param name="jobServerHosts"></param>
        void DeleteJobServerHost(IEnumerable<JobServerHost> jobServerHosts);

        #endregion

        #region 修改工作承载运行状态

        /// <summary>
        /// 修改工作承载运行状态
        /// </summary>
        /// <param name="newStateJobServerHosts">新的状态信息</param>
        void ModifyRunState(IEnumerable<JobServerHost> newStateJobServerHosts);

        #endregion

        #region 获取工作承载节点

        /// <summary>
        /// 获取工作承载节点
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        JobServerHost GetJobServerHost(IQuery query);

        #endregion

        #region 获取工作承载节点列表

        /// <summary>
        /// 获取工作承载节点列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        List<JobServerHost> GetJobServerHostList(IQuery query);

        #endregion

        #region 获取工作承载节点分页

        /// <summary>
        /// 获取工作承载节点分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        IPaging<JobServerHost> GetJobServerHostPaging(IQuery query);

        #endregion
    }
}
