using EZNEW.CTask;
using EZNEW.Develop.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 触发条件
    /// </summary>
    [VirtualAggregation]
    public class TriggerAnnualCondition : TriggerCondition
    {
        #region 字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<AnnualConditionDay> days;

        #endregion

        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<AnnualConditionDay> Days
        {
            get
            {
                return days;
            }
            set
            {
                days = value;
            }
        }

        #endregion

        #region 构造方法

        public TriggerAnnualCondition(string id = "") : base(id)
        {
            type = TaskTriggerConditionType.每年日期;
        }

        #endregion
    }
}
