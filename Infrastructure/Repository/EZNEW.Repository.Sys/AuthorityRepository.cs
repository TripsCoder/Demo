using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.Sys.Model;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Entity.Sys;
using EZNEW.Framework.Extension;
using EZNEW.DataAccessContract.Sys;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Develop.CQuery;
using EZNEW.Query.Sys;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.Sys
{
    /// <summary>
    /// 权限存储
    /// </summary>
    public class AuthorityRepository : DefaultAggregationRepository<Authority, AuthorityEntity, IAuthorityDataAccess>, IAuthorityRepository
    {
        #region 根据分组删除权限

        /// <summary>
        /// 根据分组删除权限
        /// </summary>
        /// <param name="groups">分组信息</param>
        public void RemoveAuthorityByGroup(IEnumerable<AuthorityGroup> groups)
        {
            if (groups.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> groupIds = groups.Select(c => c.SysNo).Distinct().ToList();
            IQuery query = QueryFactory.Create<AuthorityQuery>(c => groupIds.Contains(c.AuthGroup));
            Remove(query);
        }

        /// <summary>
        /// 根据分组删除权限
        /// </summary>
        /// <param name="query">query</param>
        public void RemoveAuthorityByGroup(IQuery query)
        {
            if (query == null)
            {
                return;
            }
            var removeGroupQuery = query.Clone();
            removeGroupQuery.QueryFields.Clear();
            removeGroupQuery.AddQueryFields<AuthorityGroupQuery>(c => c.SysNo);
            //remove authorigty
            var removeAuthorityQuery = QueryFactory.Create<AuthorityQuery>();
            removeAuthorityQuery.And<AuthorityQuery>(c => c.AuthGroup, CriteriaOperator.In, removeGroupQuery);
            Remove(removeAuthorityQuery);
        }
        #endregion
    }
}
