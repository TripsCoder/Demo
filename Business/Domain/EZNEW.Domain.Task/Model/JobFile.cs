using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Framework.Code;
using EZNEW.Application.CTask;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 任务工作文件
    /// </summary>
    public class JobFile : AggregationRoot<JobFile>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long id;

        /// <summary>
        /// 工作
        /// </summary>
        protected string job;

        /// <summary>
        /// 文件名称
        /// </summary>
        protected string fileName;

        /// <summary>
        /// 文件路径
        /// </summary>
        protected string filePath;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime createDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务工作文件对象
        /// </summary>
        /// <param name="id">编号</param>
        internal JobFile(long id = 0)
        {
            this.id = id;
            repository = this.Instance<IJobFileRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
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
        /// 工作
        /// </summary>
        public string Job
        {
            get
            {
                return job;
            }
            protected set
            {
                job = value;
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            protected set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            protected set
            {
                filePath = value;
            }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return createDate;
            }
            protected set
            {
                createDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitIdentityValue()
        {
            base.InitIdentityValue();
            id = GenerateJobFileId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 判断标识对象值是否为空

        /// <summary>
        /// 判断标识对象值是否为空
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return id <= 0;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个任务工作文件编号

        /// <summary>
        /// 生成一个任务工作文件编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateJobFileId()
        {
            return SerialNumber.GetSerialNumber(CTaskApplicationHelper.GetIdGroupCode(TaskIdGroup.工作任务));
        }

        #endregion

        #region 创建任务工作文件

        /// <summary>
        /// 创建一个任务工作文件对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static JobFile CreateJobFile(long id = 0)
        {
            id = id <= 0 ? GenerateJobFileId() : id;
            return new JobFile(id);
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