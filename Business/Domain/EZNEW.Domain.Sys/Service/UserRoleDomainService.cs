using EZNEW.Domain.Sys.Model;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Framework;
using EZNEW.Framework.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Framework.Response;
using EZNEW.Develop.CQuery;
using EZNEW.Query.Sys;

namespace EZNEW.Domain.Sys.Service
{
    /// <summary>
    /// 用户角色绑定服务
    /// </summary>
    public static class UserRoleDomainService
    {
        static IUserRoleRepository userRoleRepository = ContainerManager.Container.Resolve<IUserRoleRepository>();

        #region 用户->角色

        #region 添加用户角色

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="newRoles">新角色</param>
        /// <returns></returns>
        public static Result UserAddRoles(User user, params Role[] newRoles)
        {
            if (user == null || newRoles.IsNullOrEmpty())
            {
                return Result.FailedResult("用户或角色信息为空");
            }
            var newRoleIds = newRoles.Select(c => c.SysNo);
            var query = QueryFactory.Create<UserRoleQuery>(c => c.UserSysNo == user.SysNo && newRoleIds.Contains(c.RoleSysNo));
            var nowRoles = userRoleRepository.GetList(query);
            var nowRoleIds = nowRoles.Select(c => c.Item2.SysNo);
            //保存数据
            var saveRoleIds = newRoleIds.Except(nowRoleIds);
            if (saveRoleIds.IsNullOrEmpty())
            {
                return Result.SuccessResult("添加成功");
            }
            var userRoles = newRoles.Where(c => saveRoleIds.Contains(c.SysNo)).Select(r => new Tuple<User, Role>(user, r));
            userRoleRepository.Save(userRoles);
            return Result.SuccessResult("添加成功");
        }

        #endregion

        #region 删除用户角色

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="removeRoles">要移除的角色</param>
        /// <returns></returns>
        public static Result UserRemoveRoles(User user, params Role[] removeRoles)
        {
            if (user == null || removeRoles.IsNullOrEmpty())
            {
                return Result.FailedResult("用户或角色信息为空");
            }
            var userRoles = removeRoles.Select(r => new Tuple<User, Role>(user, r));
            userRoleRepository.Remove(userRoles);
            return Result.SuccessResult("角色删除成功");
        }

        #endregion

        #endregion

        #region 角色->用户

        public static Result RoleAddUses(Role role, params User[] users)
        {
            if (role == null || users.IsNullOrEmpty())
            {
                return Result.FailedResult("角色或用户信息为空");
            }
            var newUserIds = users.Select(c => c.SysNo);
            var query = QueryFactory.Create<UserRoleQuery>(c => c.RoleSysNo == role.SysNo && newUserIds.Contains(c.UserSysNo));
            var nowUserRoles = userRoleRepository.GetList(query);
            var nowUserIds = nowUserRoles.Select(c => c.Item1.SysNo);
            //add role users
            var saveUserIds = newUserIds.Except(nowUserIds);
            if (saveUserIds.IsNullOrEmpty())
            {
                return Result.SuccessResult("添加成功");
            }
            var userRoles = users.Where(c => saveUserIds.Contains(c.SysNo)).Select(c => new Tuple<User, Role>(c, role));
            userRoleRepository.Save(userRoles);
            return Result.SuccessResult("添加成功");
        }

        #endregion
    }
}
