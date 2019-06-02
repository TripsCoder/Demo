using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Query.CTask;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 每日时间段计划存储
    /// </summary>
    public class TriggerDailyConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerDailyConditionEntity, ITriggerDailyConditionDataAccess>, ITriggerDailyConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data">条件数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data)
        {
            var dailyCondition = data as TriggerDailyCondition;
            if (dailyCondition == null)
            {
                return null;
            }
            //移除当前的条件信息
            var removeQuery = QueryFactory.Create<TriggerDailyConditionQuery>(c => c.TriggerId == data.TriggerId);
            Remove(removeQuery);
            //保存数据
            return await SaveEntityAsync(dailyCondition.MapTo<TriggerDailyConditionEntity>()).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        public void RemoveTriggerConditionByTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            var query = QueryFactory.Create<TriggerDailyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(query);
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 根据执行执行计划获取附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        /// <returns></returns>
        public List<TriggerCondition> GetTriggerConditionByTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<TriggerCondition>();
            }
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            var query = QueryFactory.Create<TriggerDailyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
