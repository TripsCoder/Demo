using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.CTask;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 表达式选项
    /// </summary>
    public class ExpressionItem
    {
        #region	字段

        /// <summary>
        /// 表达式配置项
        /// </summary>
        protected TaskTriggerExpressionItem option;

        /// <summary>
        /// 值类型
        /// </summary>
        protected TaskTriggerExpressionItemConfigType valueType;

        /// <summary>
        /// 开始值
        /// </summary>
        protected int beginValue;

        /// <summary>
        /// 结束值
        /// </summary>
        protected int endValue;

        /// <summary>
        /// 集合值
        /// </summary>
        protected List<string> arrayValue;

        #endregion

        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public TaskTriggerExpressionItem Option
        {
            get
            {
                return option;
            }
            protected set
            {
                option = value;
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public TaskTriggerExpressionItemConfigType ValueType
        {
            get
            {
                return valueType;
            }
            protected set
            {
                valueType = value;
            }
        }

        /// <summary>
        /// 开始值
        /// </summary>
        public int BeginValue
        {
            get
            {
                return beginValue;
            }
            protected set
            {
                beginValue = value;
            }
        }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue
        {
            get
            {
                return endValue;
            }
            protected set
            {
                endValue = value;
            }
        }

        /// <summary>
        /// 集合值
        /// </summary>
        public List<string> ArrayValue
        {
            get
            {
                return arrayValue;
            }
            protected set
            {
                arrayValue = value;
            }
        }

        #endregion
    }
}
