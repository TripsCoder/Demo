using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Application.CTask;
using EZNEW.CTask;
using EZNEW.Framework.ValueType;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Framework.Code;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class Job : AggregationRoot<Job>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string id;

        /// <summary>
        /// 分组
        /// </summary>
        protected LazyMember<JobGroup> group;

        /// <summary>
        /// 名称
        /// </summary>
        protected string name;

        /// <summary>
        /// 任务类型
        /// </summary>
        protected JobApplicationType type;

        /// <summary>
        /// 执行类型
        /// </summary>
        protected JobRunType runType;

        /// <summary>
        /// 状态
        /// </summary>
        protected JobStatus status;

        /// <summary>
        /// 说明
        /// </summary>
        protected string description;

        /// <summary>
        /// 更新时间
        /// </summary>
        protected DateTime updateDate;

        /// <summary>
        /// 任务路径
        /// </summary>
        protected string jobPath;

        /// <summary>
        /// 任务文件名称
        /// </summary>
        protected string jobFileName;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化工作任务对象
        /// </summary>
        /// <param name="id">编号</param>
        internal Job(string id = "")
        {
            this.id = id;
            repository = this.Instance<IJobRepository>();
            group = new LazyMember<JobGroup>(LoadGroup);
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        public JobGroup Group
        {
            get
            {
                return group.Value;
            }
            protected set
            {
                group.SetValue(value, false);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobApplicationType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        public JobRunType RunType
        {
            get
            {
                return runType;
            }
            set
            {
                runType = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public JobStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                return updateDate;
            }
            protected set
            {
                updateDate = value;
            }
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        public string JobPath
        {
            get
            {
                return jobPath;
            }
            set
            {
                jobPath = value;
            }
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        public string JobFileName
        {
            get
            {
                return jobFileName;
            }
            set
            {
                jobFileName = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 设置任务分组

        /// <summary>
        /// 设置任务所属分组
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <param name="init">是否用新的分组信息初始化任务的分组，若初始化将不会再自动去加载分组信息</param>
        public void SetGroup(JobGroup group, bool init = true)
        {
            if (group == null)
            {
                init = false;
            }
            this.group.SetValue(group, init);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitIdentityValue()
        {
            base.InitIdentityValue();
            id = GenerateJobId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载任务分组

        /// <summary>
        /// 加载任务分组
        /// </summary>
        /// <returns></returns>
        JobGroup LoadGroup()
        {
            if (!AllowLazyLoad(r => r.Group))
            {
                return group.CurrentValue;
            }
            if (group.CurrentValue == null || group.CurrentValue.Code.IsNullOrEmpty())
            {
                return group.CurrentValue;
            }
            return this.Instance<IJobGroupRepository>().Get(QueryFactory.Create<JobGroupQuery>(r => r.Code == group.CurrentValue.Code));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return id.IsNullOrEmpty();
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个工作任务编号

        /// <summary>
        /// 生成一个工作任务编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateJobId()
        {
            return SerialNumber.GetSerialNumber(CTaskApplicationHelper.GetIdGroupCode(TaskIdGroup.工作任务)).ToString();
        }

        #endregion

        #region 创建工作任务

        /// <summary>
        /// 创建一个工作任务对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static Job CreateJob(string id = "")
        {
            id = id.IsNullOrEmpty() ? GenerateJobId() : id;
            return new Job(id);
        }

        protected override string GetIdentityValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #endregion
    }
}