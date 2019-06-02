using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 星期条件
    /// </summary>
    public class TriggerWeeklyCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<WeeklyConditionDay> days;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<WeeklyConditionDay> Days
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

        public TriggerWeeklyCondition(string id = "") : base(id)
        {
            type = TaskTriggerConditionType.星期配置;
        }

        #endregion
    }
}
