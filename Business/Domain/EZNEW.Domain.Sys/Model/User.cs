using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Framework.ValueType;
using EZNEW.Framework;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using EZNEW.Framework.ExpressionUtil;
using EZNEW.Application.Identity.User;
using EZNEW.Application.Identity;
using EZNEW.Framework.Code;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EZNEW.Domain.Sys.Model
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class User : AggregationRoot<User>
    {
        #region	字段

        /// <summary>
        /// 用户编号
        /// </summary>
        protected long sysNo;

        /// <summary>
        /// 用户名
        /// </summary>
        protected string userName;

        /// <summary>
        /// 真实名称
        /// </summary>
        protected string realName;

        /// <summary>
        /// 密码
        /// </summary>
        protected string pwd;

        /// <summary>
        /// 类型
        /// </summary>
        protected UserType userType;

        /// <summary>
        /// 超级用户
        /// </summary>
        protected bool superUser = false;

        /// <summary>
        /// 状态
        /// </summary>
        protected UserStatus status;

        /// <summary>
        /// 联系方式
        /// </summary>
        protected Contact contact;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime createDate;

        /// <summary>
        /// 最近登录时间
        /// </summary>
        protected DateTime lastLoginDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化用户
        /// </summary>
        internal User()
        {
            Initialization();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 用户编号
        /// </summary>
        public long SysNo
        {
            get
            {
                return sysNo;
            }
            protected set
            {
                sysNo = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName
        {
            get
            {
                return realName;
            }
            set
            {
                realName = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get
            {
                return pwd;
            }
            protected set
            {
                pwd = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public UserType UserType
        {
            get
            {
                return userType;
            }
            protected set
            {
                userType = value;
            }
        }

        /// <summary>
        /// 超级用户
        /// </summary>
        public bool SuperUser
        {
            get
            {
                return superUser;
            }
            protected set
            {
                superUser = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatus Status
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
        /// 联系方式
        /// </summary>
        public Contact Contact
        {
            get { return contact; }
            set { contact = value; }
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

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LastLoginDate
        {
            get
            {
                return lastLoginDate;
            }
            protected set
            {
                lastLoginDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 修改密码

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword">信息的密码</param>
        public void ModifyPassword(string newPassword)
        {
            SetPassword(newPassword);
        }

        #endregion

        #region 执行用户密码加密

        /// <summary>
        /// 执行用户密码加密
        /// </summary>
        /// <param name="pwdValue">密码值</param>
        /// <returns></returns>
        public static string PasswordEncryption(string pwdValue)
        {
            return pwdValue.MD5();
        }

        #endregion

        #region 当前账号是否允许登陆

        /// <summary>
        /// 当前账号是否允许登陆
        /// </summary>
        /// <returns></returns>
        public bool AllowLogin()
        {
            return status == UserStatus.正常;
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitIdentityValue()
        {
            base.InitIdentityValue();
            sysNo = GenerateUserId();
        }

        #endregion

        #region 根据给定的对象更新当前信息

        /// <summary>
        /// 根据给定的对象更新当前信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="excludePropertys">排除更新的属性</param>
        public virtual void ModifyFromOtherUser(User user, IEnumerable<string> excludePropertys = null)
        {
            if (user == null)
            {
                return;
            }
            CopyDataFromSimilarObject(user, excludePropertys);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 设置用户密码

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="newPwd">新的密码</param>
        void SetPassword(string newPwd)
        {
            pwd = PasswordEncryption(newPwd);
        }

        #endregion

        #region 创建初始化信息

        /// <summary>
        /// 创建初始化信息
        /// </summary>
        protected void Initialization()
        {
            createDate = DateTime.Now;
            lastLoginDate = DateTime.Now;
            status = UserStatus.正常;
            repository = this.Instance<IUserRepository>();
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool IdentityValueIsNone()
        {
            return sysNo <= 0;
        }

        #endregion

        #region 从指定对象复制值

        /// <summary>
        /// 从指定对象复制值
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">数据对象</param>
        /// <param name="excludePropertys">排除不复制的属性</param>
        protected override void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null)
        {
            base.CopyDataFromSimilarObject<DT>(similarObject, excludePropertys);
            if (similarObject == null)
            {
                return;
            }
            excludePropertys = excludePropertys ?? new List<string>(0);
            #region 复制值

            if (!excludePropertys.Contains("SysNo"))
            {
                SysNo = similarObject.SysNo;
            }
            if (!excludePropertys.Contains("UserName"))
            {
                UserName = similarObject.UserName;
            }
            if (!excludePropertys.Contains("RealName"))
            {
                RealName = similarObject.RealName;
            }
            if (!excludePropertys.Contains("Pwd"))
            {
                Pwd = similarObject.Pwd;
            }
            if (!excludePropertys.Contains("UserType"))
            {
                UserType = similarObject.UserType;
            }
            if (!excludePropertys.Contains("SuperUser"))
            {
                SuperUser = similarObject.SuperUser;
            }
            if (!excludePropertys.Contains("Status"))
            {
                Status = similarObject.Status;
            }
            if (!excludePropertys.Contains("Contact"))
            {
                Contact = similarObject.Contact;
            }
            if (!excludePropertys.Contains("CreateDate"))
            {
                CreateDate = similarObject.CreateDate;
            }
            if (!excludePropertys.Contains("LastLoginDate"))
            {
                LastLoginDate = similarObject.LastLoginDate;
            }

            #endregion
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成用户编号

        /// <summary>
        /// 生成用户编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateUserId()
        {
            return SerialNumber.GetSerialNumber(IdentityApplicationHelper.GetIdGroupCode(IdentityGroup.用户));
        }

        #endregion

        #region 创建用户对象

        /// <summary>
        /// 生成用户对象
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        public static User CreateUser(long userId, UserType userType = UserType.管理账户)
        {
            User user = null;
            switch (userType)
            {
                case UserType.管理账户:
                    user = AdminUser.CreateNewAdminUser(userId);
                    break;
            }
            return user;
        }

        protected override string GetIdentityValue()
        {
            return SysNo.ToString();
        }

        #endregion

        #endregion

        #endregion
    }
}