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
    /// 工作任务存储
    /// </summary>
    public interface IJobRepository : IAggregationRepository<Job>
    {
        #region 移除工作日分组时移除任务信息

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="jobGroups">任务分组</param>
        void RemoveByJobGroup(IEnumerable<JobGroup> jobGroups);

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="query">删除条件</param>
        void RemoveByJobGroup(IQuery query);

        #endregion
    }
}
