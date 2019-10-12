using EZNEW.Domain.Sys.Model;
using EZNEW.Framework.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.Sys.Service
{
    /// <summary>
    /// 用户角色服务
    /// </summary>
    public interface IUserRoleService
    {
        #region 用户和角色的绑定

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        /// <returns></returns>
        Result BindUserAndRole(params Tuple<User, Role>[] userRoleBinds);

        #endregion

        #region 用户角色解绑

        /// <summary>
        /// 用户角色解绑
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        /// <returns></returns>
        Result UnBindUserAndRole(params Tuple<User, Role>[] userRoleBinds);

        #endregion
    }
}
