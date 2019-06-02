using EZNEW.CTask;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Repository
{
    /// <summary>
    /// 计划附加条件存储接口
    /// </summary>
    public interface ITriggerConditionRepository : ITriggerConditionBaseRepository
    {
        /// <summary>
        /// 保存执行计划中的附加条件
        /// </summary>
        /// <param name="triggers"></param>
        void SaveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers);
    }
}
