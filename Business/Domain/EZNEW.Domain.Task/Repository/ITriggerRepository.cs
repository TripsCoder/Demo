
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
    /// 任务执行计划存储
    /// </summary>
    public interface ITriggerRepository : IAggregationRepository<Trigger>
    {
        #region 删除工作任务时删除相应的计划

        /// <summary>
        /// 删除工作任务时删除相应的计划
        /// </summary>
        /// <param name="jobs">工作任务</param>
        void RemoveTriggerByJob(IEnumerable<Job> jobs);

        #endregion
    }
}
