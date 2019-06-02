using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.Sys
{
    /// <summary>
    /// 权限&授权操作关联
    /// </summary>
    [Serializable]
    [Entity("Sys_AuthorityBindOperation", "Sys", "权限&授权操作关联")]
    public class AuthorityBindOperationEntity : BaseEntity<AuthorityBindOperationEntity>
    {
        #region	字段

        /// <summary>
        /// 授权操作
        /// </summary>
        [EntityField(Description = "授权操作", PrimaryKey = true)]
        public long AuthorithOperation
        {
            get { return valueDict.GetValue<long>("AuthorithOperation"); }
            set { valueDict.SetValue("AuthorithOperation", value); }
        }

        /// <summary>
        /// 权限
        /// </summary>
        [EntityField(Description = "权限", PrimaryKey = true)]
        public string AuthorityCode
        {
            get { return valueDict.GetValue<string>("AuthorityCode"); }
            set { valueDict.SetValue("AuthorityCode", value); }
        }

        #endregion
    }
}