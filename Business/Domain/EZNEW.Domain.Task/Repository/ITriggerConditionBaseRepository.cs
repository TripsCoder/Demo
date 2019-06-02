using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Repository
{
    public interface ITriggerConditionBaseRepository : IAggregationRepository<TriggerCondition>
    {
        /// <summary>
        /// 根据执行计划删除计划附加条件
        /// </summary>
        /// <param name="triggers"></param>
        void RemoveTriggerConditionByTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 根据执行执行计划获取附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        /// <returns></returns>
        List<TriggerCondition> GetTriggerConditionByTrigger(IEnumerable<Trigger> triggers);
    }
}
