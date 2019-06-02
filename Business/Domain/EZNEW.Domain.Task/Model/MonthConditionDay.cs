using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 月计划日期
    /// </summary>
    public class MonthConditionDay
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected int day;

        /// <summary>
        /// 包含当前日期
        /// </summary>
        protected bool include;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get
            {
                return day;
            }
            protected set
            {
                day = value;
            }
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get
            {
                return include;
            }
            protected set
            {
                include = value;
            }
        }

        #endregion
    }
}
