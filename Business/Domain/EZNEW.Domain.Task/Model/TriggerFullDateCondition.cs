using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 时间条件
    /// </summary>
    public class TriggerFullDateCondition:TriggerCondition
    {
        #region 字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<FullDateConditionDate> dates;

        #endregion

        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<FullDateConditionDate> Dates
        {
            get
            {
                return dates;
            }
            set
            {
                dates = value;
            }
        }

        #endregion

        #region 构造方法

        public TriggerFullDateCondition(string id = "") : base(id)
        {
            type = TaskTriggerConditionType.固定日期;
        }

        #endregion
    }
}
