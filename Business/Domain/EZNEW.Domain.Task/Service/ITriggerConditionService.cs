using EZNEW.CTask;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 计划条件服务
    /// </summary>
    public interface ITriggerConditionService
    {
        #region 获取指定执行计划的条件

        /// <summary>
        /// 获取指定计划的附加条件
        /// </summary>
        /// <param name="triggerId">计划编号</param>
        /// <param name="conditionType">条件类型</param>
        /// <returns></returns>
        TriggerCondition GetTriggerConditionByTrigger(string triggerId, TaskTriggerConditionType conditionType);

        /// <summary>
        /// 获取执行计划的附加条件
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        /// <returns></returns>
        List<TriggerCondition> GetTriggerConditionList(params Trigger[] triggers);

        #endregion
    }
}
