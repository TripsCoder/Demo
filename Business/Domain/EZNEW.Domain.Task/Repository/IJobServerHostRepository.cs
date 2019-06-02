
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
    /// 工作承载节点存储
    /// </summary>
    public interface IJobServerHostRepository : IAggregationRepository<JobServerHost>
    {
        #region 删除工作时删除工作承载信息

        /// <summary>
        ///  删除工作时删除工作承载信息
        /// </summary>
        /// <param name="jobs">工作信息</param>
        void RemoveJobServerHostByJob(IEnumerable<Job> jobs);

        #endregion

        #region 删除服务节点时删除工作承载信息

        /// <summary>
        /// 删除服务节点时删除工作承载信息
        /// </summary>
        /// <param name="servers">服务信息</param>
        void RemoveJobServerHostByServer(IEnumerable<ServerNode> servers);

        #endregion
    }
}
