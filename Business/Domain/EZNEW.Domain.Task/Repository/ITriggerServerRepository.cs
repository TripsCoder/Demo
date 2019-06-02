
using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Domain.CTask.Repository
{
    /// <summary>
    /// 服务节点执行计划存储
    /// </summary>
    public interface ITriggerServerRepository : IAggregationRepository<TriggerServer>
    {
        #region 根据执行计划移除计划服务

        /// <summary>
        /// 根据执行计划移除计划服务
        /// </summary>
        /// <param name="triggers">执行计划数据</param>
        void RemoveTriggerServerByTrigger(IEnumerable<Trigger> triggers);

        #endregion

        #region 根据服务节点移除计划服务

        /// <summary>
        /// 根据服务节点移除计划服务
        /// </summary>
        /// <param name="servers">服务数据</param>
        void RemoveTriggerServerByServer(IEnumerable<ServerNode> servers);

        #endregion

        #region 移除任务&服务承载时移除服务执行计划

        /// <summary>
        /// 移除任务&服务承载时移除服务执行计划
        /// </summary>
        /// <param name="jobServerHosts">任务服务承载信息</param>
        void RemoveTriggerServerByJobHost(IEnumerable<JobServerHost> jobServerHosts);

        #endregion
    }
}
