using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.Sys
{
    /// <summary>
    /// 权限
    /// </summary>
    [Serializable]
    [Entity("Sys_Authority", "Sys", "权限")]
    public class AuthorityEntity : BaseEntity<AuthorityEntity>
    {
        #region	字段

        /// <summary>
        /// 权限编码
        /// </summary>
        [EntityField(Description = "权限编码", PrimaryKey = true)]
        public string Code
        {
            get { return valueDict.GetValue<string>("Code"); }
            set { valueDict.SetValue("Code", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityField(Description = "名称")]
        public string Name
        {
            get { return valueDict.GetValue<string>("Name"); }
            set { valueDict.SetValue("Name", value); }
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        [EntityField(Description = "权限类型")]
        public int AuthType
        {
            get { return valueDict.GetValue<int>("AuthType"); }
            set { valueDict.SetValue("AuthType", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return valueDict.GetValue<int>("Status"); }
            set { valueDict.SetValue("Status", value); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        [EntityField(Description = "排序")]
        public int Sort
        {
            get { return valueDict.GetValue<int>("Sort"); }
            set { valueDict.SetValue("Sort", value); }
        }

        /// <summary>
        /// 权限分组
        /// </summary>
        [EntityField(Description = "权限分组")]
        public long AuthGroup
        {
            get { return valueDict.GetValue<long>("AuthGroup"); }
            set { valueDict.SetValue("AuthGroup", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Remark
        {
            get { return valueDict.GetValue<string>("Remark"); }
            set { valueDict.SetValue("Remark", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [EntityField(Description = "添加时间")]
        public DateTime CreateDate
        {
            get { return valueDict.GetValue<DateTime>("CreateDate"); }
            set { valueDict.SetValue("CreateDate", value); }
        }

        #endregion
    }
}