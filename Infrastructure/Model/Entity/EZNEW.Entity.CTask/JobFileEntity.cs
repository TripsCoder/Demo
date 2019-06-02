using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务文件
    /// </summary>
    [Serializable]
    [Entity("CTask_JobFile", "CTask", "任务文件")]
    public class JobFileEntity : BaseEntity<JobFileEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public long Id
        {
            get { return valueDict.GetValue<long>("Id"); }
            set { valueDict.SetValue("Id", value); }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        [EntityField(Description = "工作任务")]
        public string Job
        {
            get { return valueDict.GetValue<string>("Job"); }
            set { valueDict.SetValue("Job", value); }
        }

        /// <summary>
        /// 文件标题
        /// </summary>
        [EntityField(Description = "文件标题")]
        public string Title
        {
            get { return valueDict.GetValue<string>("Title"); }
            set { valueDict.SetValue("Title", value); }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        [EntityField(Description = "文件路径")]
        public string Path
        {
            get { return valueDict.GetValue<string>("Path"); }
            set { valueDict.SetValue("Path", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [EntityField(Description = "添加时间")]
        public DateTime CreateDate
        {
            get { return valueDict.GetValue<DateTime>("CreateDate"); }
            set { valueDict.SetValue("CreateDate", value); }
        }

        #endregion
    }
}