using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划表达式附加条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerExpressionCondition", "CTask", "计划表达式附加条件")]
    public class TriggerExpressionConditionEntity : BaseEntity<TriggerExpressionConditionEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string TriggerId
        {
            get { return valueDict.GetValue<string>("TriggerId"); }
            set { valueDict.SetValue("TriggerId", value); }
        }

        /// <summary>
        /// 条件选项
        /// </summary>
        [EntityField(Description = "条件选项", PrimaryKey = true)]
        public int ConditionOption
        {
            get { return valueDict.GetValue<int>("ConditionOption"); }
            set { valueDict.SetValue("ConditionOption", value); }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        [EntityField(Description = "值类型")]
        public int ValueType
        {
            get { return valueDict.GetValue<int>("ValueType"); }
            set { valueDict.SetValue("ValueType", value); }
        }

        /// <summary>
        /// 起始值
        /// </summary>
        [EntityField(Description = "起始值")]
        public int BeginValue
        {
            get { return valueDict.GetValue<int>("BeginValue"); }
            set { valueDict.SetValue("BeginValue", value); }
        }

        /// <summary>
        /// 结束值
        /// </summary>
        [EntityField(Description = "结束值")]
        public int EndValue
        {
            get { return valueDict.GetValue<int>("EndValue"); }
            set { valueDict.SetValue("EndValue", value); }
        }

        /// <summary>
        /// 集合值
        /// </summary>
        [EntityField(Description = "集合值")]
        public string ArrayValue
        {
            get { return valueDict.GetValue<string>("ArrayValue"); }
            set { valueDict.SetValue("ArrayValue", value); }
        }

        #endregion
    }
}