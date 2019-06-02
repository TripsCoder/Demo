using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Serializable]
    [Entity("Sys_RoleAuthorize", "Sys", "角色权限")]
    public class RoleAuthorizeEntity : BaseEntity<RoleAuthorizeEntity>
    {
        #region	字段

        /// <summary>
        /// 角色
        /// </summary>
        [EntityField(Description = "角色", PrimaryKey = true)]
        public long Role
        {
            get { return valueDict.GetValue<long>("Role"); }
            set { valueDict.SetValue("Role", value); }
        }

        /// <summary>
        /// 权限
        /// </summary>
        [EntityField(Description = "权限", PrimaryKey = true)]
        public string Authority
        {
            get { return valueDict.GetValue<string>("Authority"); }
            set { valueDict.SetValue("Authority", value); }
        }

        #endregion
    }
}