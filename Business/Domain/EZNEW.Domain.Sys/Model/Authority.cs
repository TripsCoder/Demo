using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework.Extension;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Develop.CQuery;
using EZNEW.Query.Sys;
using EZNEW.Framework.ValueType;
using EZNEW.Application.Identity.Auth;
using System.Threading.Tasks;

namespace EZNEW.Domain.Sys.Model
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Authority : AggregationRoot<Authority>
    {
        #region	字段

        /// <summary>
        /// 权限编码
        /// </summary>
        protected string code;

        /// <summary>
        /// 名称
        /// </summary>
        protected string name;

        /// <summary>
        /// 权限类型
        /// </summary>
        protected AuthorityType authType;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityStatus status;

        /// <summary>
        /// 排序
        /// </summary>
        protected int sort;

        /// <summary>
        /// 权限分组
        /// </summary>
        protected LazyMember<AuthorityGroup> authGroup;

        /// <summary>
        /// 说明
        /// </summary>
        protected string remark;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime createDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个权限对象
        /// </summary>
        public Authority()
        {
            status = AuthorityStatus.启用;
            authGroup = new LazyMember<AuthorityGroup>(LoadAuthorityGroup);
            authType = AuthorityType.管理;
            createDate = DateTime.Now;
            sort = 0;
            repository = this.Instance<IAuthorityRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
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
        /// 权限类型
        /// </summary>
        public AuthorityType AuthType
        {
            get
            {
                return authType;
            }
            set
            {
                authType = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityStatus Status
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get
            {
                return sort;
            }
            set
            {
                sort = value;
            }
        }

        /// <summary>
        /// 权限分组
        /// </summary>
        public AuthorityGroup AuthGroup
        {
            get
            {
                return authGroup.Value;
            }
            protected set
            {
                authGroup.SetValue(value, false);
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
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
            set
            {
                createDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 设置分组

        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">权限分组</param>
        /// <param name="init">是否初始化</param>
        public void SetGroup(AuthorityGroup group, bool init = true)
        {
            authGroup.SetValue(group, init);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载权限分组

        /// <summary>
        /// 加载权限分组
        /// </summary>
        /// <returns></returns>
        AuthorityGroup LoadAuthorityGroup()
        {
            if (!AllowLazyLoad(r => r.AuthGroup))
            {
                return authGroup.CurrentValue;
            }
            if (authGroup.CurrentValue == null || authGroup.CurrentValue.SysNo <= 0)
            {
                return authGroup.CurrentValue;
            }
            return this.Instance<IAuthorityGroupRepository>().Get(QueryFactory.Create<AuthorityGroupQuery>(r => r.SysNo == authGroup.CurrentValue.SysNo));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return code.IsNullOrEmpty();
        }

        #endregion

        #region 获取对象标识信息

        /// <summary>
        /// 获取对象标识信息
        /// </summary>
        /// <returns></returns>
        protected override string GetIdentityValue()
        {
            return Code;
        } 

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建一个权限对象
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static Authority CreateAuthority(string code = "", string name = "")
        {
            var authority = new Authority()
            {
                Code = code,
                Name = name
            };
            return authority;
        }

        #endregion

        #endregion
    }
}