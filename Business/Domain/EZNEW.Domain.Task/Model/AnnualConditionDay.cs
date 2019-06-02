using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 年计划天
    /// </summary>
    [Serializable]
    public class AnnualConditionDay
    {
        #region	字段

        /// <summary>
        /// 月份
        /// </summary>
        protected int month;

        /// <summary>
        /// 日期
        /// </summary>
        protected int day;

        /// <summary>
        /// 包含
        /// </summary>
        protected bool include;

        #endregion

        #region	属性

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get
            {
                return month;
            }
            protected set
            {
                month = value;
            }
        }

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
        /// 包含
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
