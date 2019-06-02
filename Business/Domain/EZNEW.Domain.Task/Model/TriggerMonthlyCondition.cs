using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 月份附加条件
    /// </summary>
    public class TriggerMonthlyCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<MonthConditionDay> days;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<MonthConditionDay> Days
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

        public TriggerMonthlyCondition(string id = ""):base(id)
        {
            type = TaskTriggerConditionType.每月日期;
        }

        #endregion
    }
}
