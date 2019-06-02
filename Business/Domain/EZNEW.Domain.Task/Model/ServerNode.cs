using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.CTask;
using EZNEW.Application.CTask;
using EZNEW.Framework.Code;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 服务节点
    /// </summary>
    public class ServerNode : AggregationRoot<ServerNode>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string id;

        /// <summary>
        /// 实例名称
        /// </summary>
        protected string instanceName;

        /// <summary>
        /// 节点名称
        /// </summary>
        protected string name;

        /// <summary>
        /// 状态
        /// </summary>
        protected ServerStatus status;

        /// <summary>
        /// 地址
        /// </summary>
        protected string host;

        /// <summary>
        /// 线程数量
        /// </summary>
        protected int threadCount;

        /// <summary>
        /// 线程优先级
        /// </summary>
        protected string threadPriority;

        /// <summary>
        /// 说明
        /// </summary>
        protected string description;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化服务节点对象
        /// </summary>
        /// <param name="id">编号</param>
        internal ServerNode(string id = "")
        {
            this.id = id;
            repository = this.Instance<IServerNodeRepository>();
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
        /// 实例名称
        /// </summary>
        public string InstanceName
        {
            get
            {
                return instanceName;
            }
            set
            {
                instanceName = value;
            }
        }

        /// <summary>
        /// 节点名称
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
        /// 状态
        /// </summary>
        public ServerStatus Status
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
        /// 地址
        /// </summary>
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
            }
        }

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount
        {
            get
            {
                return threadCount;
            }
            set
            {
                threadCount = value;
            }
        }

        /// <summary>
        /// 线程优先级
        /// </summary>
        public string ThreadPriority
        {
            get
            {
                return threadPriority;
            }
            set
            {
                threadPriority = value;
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
            id = GenerateServerNodeId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 保存数据验证

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected override bool SaveValidation()
        {
            instanceName = instanceName ?? string.Empty;
            description = description ?? string.Empty;
            return base.SaveValidation();
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

        #region 生成一个服务节点编号

        /// <summary>
        /// 生成一个服务节点编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateServerNodeId()
        {
            return SerialNumber.GetSerialNumber(CTaskApplicationHelper.GetIdGroupCode(TaskIdGroup.服务节点)).ToString();
        }

        #endregion

        #region 创建服务节点

        /// <summary>
        /// 创建一个服务节点对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static ServerNode CreateServerNode(string id = "")
        {
            id = id.IsNullOrEmpty() ? GenerateServerNodeId() : id;
            return new ServerNode(id);
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