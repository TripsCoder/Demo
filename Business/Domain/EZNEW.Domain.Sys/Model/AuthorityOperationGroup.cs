using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Develop.CQuery;
using EZNEW.Query.Sys;
using EZNEW.Framework.Extension;
using System.Collections.Generic;
using EZNEW.Framework.ValueType;
using EZNEW.Framework;
using EZNEW.Application.Identity.Auth;
using EZNEW.Application.Identity;
using EZNEW.Framework.Code;
using System.Threading.Tasks;

namespace EZNEW.Domain.Sys.Model
{
    /// <summary>
    /// 授权操作组
    /// </summary>
    public class AuthorityOperationGroup : AggregationRoot<AuthorityOperationGroup>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long sysNo;

        /// <summary>
        /// 名称
        /// </summary>
        protected string name;

        /// <summary>
        /// 排序
        /// </summary>
        protected int sort;

        /// <summary>
        /// 上级
        /// </summary>
        protected LazyMember<AuthorityOperationGroup> parent;

        /// <summary>
        /// 根组
        /// </summary>
        protected LazyMember<AuthorityOperationGroup> root;

        /// <summary>
        /// 等级
        /// </summary>
        protected int level;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityOperationGroupStatus status;

        /// <summary>
        /// 说明
        /// </summary>
        protected string remark;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个操作分组对象
        /// </summary>
        internal AuthorityOperationGroup()
        {
            parent = new LazyMember<AuthorityOperationGroup>(LoadParentGroup);
            root = new LazyMember<AuthorityOperationGroup>(LoadRootGroup);
            repository = this.Instance<IAuthorityOperationGroupRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long SysNo
        {
            get
            {
                return sysNo;
            }
            set
            {
                sysNo = value;
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
        /// 上级
        /// </summary>
        public AuthorityOperationGroup Parent
        {
            get
            {
                return parent.Value;
            }
            protected set
            {
                parent.SetValue(value, false);
            }
        }

        /// <summary>
        /// 根组
        /// </summary>
        public AuthorityOperationGroup Root
        {
            get
            {
                return root.Value;
            }
            protected set
            {
                root.SetValue(value, false);
            }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityOperationGroupStatus Status
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

        #endregion

        #region 方法

        #region 功能方法

        #region 保存分组

        /// <summary>
        /// 保存分组
        /// </summary>
        public override async Task SaveAsync()
        {
            //根节点
            ValidateRootGroup();
            await repository.SaveAsync(this).ConfigureAwait(false);
        }

        #endregion

        #region 设置上级分组

        /// <summary>
        /// 设置上级分组
        /// </summary>
        /// <param name="parentGroup">上级分组</param>
        public void SetParentGroup(AuthorityOperationGroup parentGroup)
        {
            int parentLevel = 0;
            long parentSysNo = 0;
            if (parentGroup != null)
            {
                if (sysNo == parentGroup.SysNo && !IdentityValueIsNone())
                {
                    throw new Exception("不能将分组数据设置为自己的上级分组");
                }
                if (parentGroup.Root != null && parentGroup.Root.SysNo == sysNo)
                {
                    throw new Exception("不能将当前分组的下级设置为上级分组");
                }
                parentLevel = parentGroup.Level;
                parentSysNo = parentGroup.SysNo;
                root.SetValue(parentGroup.Root, false);
            }
            else
            {
                root.SetValue(null, false);
            }
            //排序
            IQuery sortQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.Parent == parentSysNo);
            sortQuery.AddQueryFields<AuthorityOperationGroupQuery>(c => c.Sort);
            int maxSortIndex = repository.Max<int>(sortQuery);
            sort = maxSortIndex + 1;
            parent.SetValue(parentGroup, true);
            //等级
            int newLevel = parentLevel + 1;
            bool modifyChild = newLevel != level;
            level = newLevel;
            if (modifyChild)
            {
                //修改所有子集信息
                ModifyChildAuthorityGroupParentGroup();
            }
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitIdentityValue()
        {
            base.InitIdentityValue();
            sysNo = GenerateAuthorityOperationGroupId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 修改下级分组

        /// <summary>
        /// 修改下级分组
        /// </summary>
        void ModifyChildAuthorityGroupParentGroup()
        {
            if (IsNew)
            {
                return;
            }
            IQuery query = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.Parent == SysNo);
            List<AuthorityOperationGroup> childGroupList = repository.GetList(query);
            foreach (var group in childGroupList)
            {
                group.SetParentGroup(this);
                group.Save();
            }
        }

        #endregion

        #region 加载上级分组

        /// <summary>
        /// 加载上级分组
        /// </summary>
        AuthorityOperationGroup LoadParentGroup()
        {
            if (!AllowLazyLoad(r => r.Parent))
            {
                return parent.CurrentValue;
            }
            if (level <= 1 || parent.CurrentValue == null)
            {
                return parent.CurrentValue;
            }
            IQuery parentQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.SysNo == parent.CurrentValue.SysNo);
            return repository.Get(parentQuery);
        }

        #endregion

        #region 加载根级分组

        /// <summary>
        /// 加载根级分组
        /// </summary>
        AuthorityOperationGroup LoadRootGroup()
        {
            if (!AllowLazyLoad(r => r.Root))
            {
                return root.CurrentValue;
            }
            if (root.CurrentValue == null || root.CurrentValue.SysNo <= 0)
            {
                return null;
            }
            IQuery rootQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.SysNo == root.CurrentValue.SysNo);
            return repository.Get(rootQuery);
        }

        #endregion

        #region 验证根组

        /// <summary>
        /// 验证根组
        /// </summary>
        void ValidateRootGroup()
        {
            if (root.CurrentValue == null || root.CurrentValue.SysNo <= 0)
            {
                if (IdentityValueIsNone())
                {
                    InitIdentityValue();
                }
                var newRootGroup = CreateAuthorityOperationGroup(sysNo);
                root.SetValue(newRootGroup, false);
            }
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

        #region 获取对象标识信息

        /// <summary>
        /// 获取对象标识信息
        /// </summary>
        /// <returns></returns>
        protected override string GetIdentityValue()
        {
            return sysNo.ToString();
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成分组对象编号

        /// <summary>
        /// 生成分组对象编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateAuthorityOperationGroupId()
        {
            return SerialNumber.GetSerialNumber(IdentityApplicationHelper.GetIdGroupCode(IdentityGroup.授权操作分组));
        }

        #endregion

        #region 创建新的操作分组对象

        /// <summary>
        /// 创建操作分组对象
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static AuthorityOperationGroup CreateAuthorityOperationGroup(long sysNo, string name = "")
        {
            var operationGroup = new AuthorityOperationGroup()
            {
                SysNo = sysNo,
                Name = name
            };
            return operationGroup;
        }

        #endregion

        #endregion

        #endregion
    }
}