using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 工作分组
    /// </summary>
    [Serializable]
    [Entity("CTask_JobGroup", "CTask", "工作分组")]
    public class JobGroupEntity : BaseEntity<JobGroupEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Code
        {
            get { return valueDict.GetValue<string>("Code"); }
            set { valueDict.SetValue("Code", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityField(Description = "名称")]
        public string Name
        {
            get { return valueDict.GetValue<string>("Name"); }
            set { valueDict.SetValue("Name", value); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        [EntityField(Description = "排序")]
        public int Sort
        {
            get { return valueDict.GetValue<int>("Sort"); }
            set { valueDict.SetValue("Sort", value); }
        }

        /// <summary>
        /// 上级
        /// </summary>
        [EntityField(Description = "上级")]
        public string Parent
        {
            get { return valueDict.GetValue<string>("Parent"); }
            set { valueDict.SetValue("Parent", value); }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        [EntityField(Description = "根节点")]
        public string Root
        {
            get { return valueDict.GetValue<string>("Root"); }
            set { valueDict.SetValue("Root", value); }
        }

        /// <summary>
        /// 等级
        /// </summary>
        [EntityField(Description = "等级")]
        public int Level
        {
            get { return valueDict.GetValue<int>("Level"); }
            set { valueDict.SetValue("Level", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Remark
        {
            get { return valueDict.GetValue<string>("Remark"); }
            set { valueDict.SetValue("Remark", value); }
        }

        #endregion
    }
}