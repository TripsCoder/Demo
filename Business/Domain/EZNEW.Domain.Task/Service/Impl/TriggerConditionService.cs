using EZNEW.CTask;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.IoC;

namespace EZNEW.Domain.CTask.Service.Impl
{
    /// <summary>
    /// 计划条件服务
    /// </summary>
    public class TriggerConditionService : ITriggerConditionService
    {
        static ITriggerConditionRepository conditionRepository = ContainerManager.Resolve<ITriggerConditionRepository>();

        #region 获取指定执行计划的条件

        /// <summary>
        /// 获取指定计划的附加条件
        /// </summary>
        /// <param name="triggerId">计划编号</param>
        /// <param name="conditionType">条件类型</param>
        /// <returns></returns>
        public TriggerCondition GetTriggerConditionByTrigger(string triggerId, TaskTriggerConditionType conditionType)
        {
            if (triggerId.IsNullOrEmpty())
            {
                return null;
            }
            var trigger = Trigger.CreateTrigger(triggerId);
            trigger.SetCondition(TriggerCondition.CreateTriggerCondition(conditionType, triggerId));
            return GetTriggerConditionList(trigger)?.FirstOrDefault();
        }

        /// <summary>
        /// 获取执行计划的附加条件
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        /// <returns></returns>
        public List<TriggerCondition> GetTriggerConditionList(params Trigger[] triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            return conditionRepository.GetTriggerConditionByTrigger(triggers);
        }

        #endregion
    }
}
