using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 工作任务
    /// </summary>
    [Serializable]
    [Entity("CTask_Job", "CTask", "工作任务")]
    public class JobEntity : BaseEntity<JobEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Id
        {
            get { return valueDict.GetValue<string>("Id"); }
            set { valueDict.SetValue("Id", value); }
        }

        /// <summary>
        /// 分组
        /// </summary>
        [EntityField(Description = "分组")]
        public string Group
        {
            get { return valueDict.GetValue<string>("Group"); }
            set { valueDict.SetValue("Group", value); }
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
        /// 任务类型
        /// </summary>
        [EntityField(Description = "任务类型")]
        public int Type
        {
            get { return valueDict.GetValue<int>("Type"); }
            set { valueDict.SetValue("Type", value); }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        [EntityField(Description = "执行类型")]
        public int RunType
        {
            get { return valueDict.GetValue<int>("RunType"); }
            set { valueDict.SetValue("RunType", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return valueDict.GetValue<int>("Status"); }
            set { valueDict.SetValue("Status", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Description
        {
            get { return valueDict.GetValue<string>("Description"); }
            set { valueDict.SetValue("Description", value); }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityField(Description = "更新时间", RefreshDate = true)]
        public DateTime UpdateDate
        {
            get { return valueDict.GetValue<DateTime>("UpdateDate"); }
            set { valueDict.SetValue("UpdateDate", value); }
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        [EntityField(Description = "任务路径")]
        public string JobPath
        {
            get { return valueDict.GetValue<string>("JobPath"); }
            set { valueDict.SetValue("JobPath", value); }
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        [EntityField(Description = "任务文件名称")]
        public string JobFileName
        {
            get { return valueDict.GetValue<string>("JobFileName"); }
            set { valueDict.SetValue("JobFileName", value); }
        }

        #endregion
    }
}