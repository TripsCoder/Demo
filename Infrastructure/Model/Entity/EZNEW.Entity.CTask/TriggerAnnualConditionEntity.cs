using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划年度条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerAnnualCondition", "CTask", "计划年度条件")]
    public class TriggerAnnualConditionEntity : BaseEntity<TriggerAnnualConditionEntity>
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
        /// 月份
        /// </summary>
        [EntityField(Description = "月份", PrimaryKey = true)]
        public int Month
        {
            get { return valueDict.GetValue<int>("Month"); }
            set { valueDict.SetValue("Month", value); }
        }

        /// <summary>
        /// 日期
        /// </summary>
        [EntityField(Description = "日期", PrimaryKey = true)]
        public int Day
        {
            get { return valueDict.GetValue<int>("Day"); }
            set { valueDict.SetValue("Day", value); }
        }

        /// <summary>
        /// 包含
        /// </summary>
        [EntityField(Description = "包含")]
        public bool Include
        {
            get { return valueDict.GetValue<bool>("Include"); }
            set { valueDict.SetValue("Include", value); }
        }

        #endregion
    }
}