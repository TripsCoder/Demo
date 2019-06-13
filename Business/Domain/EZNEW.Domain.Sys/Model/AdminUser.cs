using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Framework.Extension;
using EZNEW.Domain.Sys.Service;
using EZNEW.Develop.CQuery;
using EZNEW.Query.Sys;
using EZNEW.Framework.ValueType;
using EZNEW.Framework;
using System.Linq.Expressions;
using EZNEW.Framework.ExpressionUtil;
using EZNEW.Application.Identity.User;

namespace EZNEW.Domain.Sys.Model
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class AdminUser : User
    {
        #region 字段

        /// <summary>
        /// 用户角色
        /// </summary>
        private LazyMember<List<Role>> roleList = null;

        #endregion

        #region 构造方法


        /// <summary>
        /// 初始化管理账户
        /// </summary>
        internal AdminUser() : base()
        {
            userType = UserType.管理账户;
            roleList = new LazyMember<List<Role>>(GetRoles);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<Role> Roles
        {
            get
            {
                return roleList.Value;
            }
            protected set
            {
                roleList.SetValue(value ?? new List<Role>(0), false);
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 添加角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="newRoles">角色信息</param>
        public void AddRoles(params Role[] newRoles)
        {
            if (newRoles.IsNullOrEmpty())
            {
                return;
            }
            if (roleList.CurrentValue == null)
            {
                roleList.SetValue(newRoles.ToList(), false);
            }
            roleList.CurrentValue.AddRange(newRoles);
            UserRoleDomainService.UserAddRoles(this, newRoles);
        }

        #endregion

        #region 移除角色

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roles">角色信息</param>
        public void RemoveRoles(params Role[] roles)
        {
            if (roles.IsNullOrEmpty())
            {
                return;
            }
            if (!roleList.CurrentValue.IsNullOrEmpty())
            {
                roleList.CurrentValue.RemoveAll(r => roles.Contains(r));
            }
            UserRoleDomainService.UserRemoveRoles(this, roles);
        }

        #endregion

        #region 设置角色

        /// <summary>
        /// 设置用户角色值
        /// </summary>
        /// <param name="roles">角色信息</param>
        /// <param name="init">是否初始化，设置为初始化后将不会再自动加载信息</param>
        public void SetRoles(IEnumerable<Role> roles, bool init = true)
        {
            roles = roles ?? new List<Role>(0);
            roleList.SetValue(roles.ToList(), init);
        }

        #endregion

        #region 根据给定的对象更新当前信息

        /// <summary>
        /// 根据给定的对象更新当前信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="excludePropertys">排除更新的属性</param>
        public override void ModifyFromOtherUser(User user, IEnumerable<string> excludePropertys = null)
        {
            base.ModifyFromOtherUser(user, excludePropertys);
            if (user == null || !(user is AdminUser))
            {
                return;
            }
            var adminUser = user as AdminUser;
            excludePropertys = excludePropertys ?? new List<string>(0);

            #region 修改值

            if (!excludePropertys.Contains("Roles"))
            {
                SetRoles(adminUser.Roles, true);
            }

            #endregion
        }

        #endregion

        #endregion

        #region 内部方法

        #region 获取角色

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        protected List<Role> GetRoles()
        {
            if (!AllowLazyLoad("Roles"))
            {
                return roleList.CurrentValue;
            }
            var nowRoles = RoleDomainService.GetUserBindRole(sysNo);
            if (!roleList.CurrentValue.IsNullOrEmpty())
            {
                nowRoles?.AddRange(roleList.CurrentValue);
            }
            nowRoles = nowRoles?.Distinct().ToList() ?? new List<Role>(0);
            return nowRoles;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 创建管理用户

        /// <summary>
        /// 创建一个管理账号对象
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="realName">真实名称</param>
        /// <param name="superUser">是否为超级用户</param>
        /// <returns>管理账号对象</returns>
        public static AdminUser CreateNewAdminUser(long userId, string userName = "", string pwd = "", string realName = "", bool superUser = false)
        {
            var user = new AdminUser()
            {
                SysNo = userId,
                UserName = userName,
                RealName = realName,
                SuperUser = superUser
            };
            user.ModifyPassword(pwd);
            return user;
        }

        #endregion

        #endregion

        #endregion
    }
}
