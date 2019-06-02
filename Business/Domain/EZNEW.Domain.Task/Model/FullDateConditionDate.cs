using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 日期
    /// </summary>
    public class FullDateConditionDate
    {
        #region	字段

        /// <summary>
        /// 时间
        /// </summary>
        protected DateTime date;

        /// <summary>
        /// 包含当前日期
        /// </summary>
        protected bool include;

        #endregion

        #region	属性

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
            protected set
            {
                date = value;
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
