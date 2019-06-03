using EZNEW.Develop.Domain.Repository;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.Sys.Repository
{
    /// <summary>
    /// 权限&操作绑定存储
    /// </summary>
    public interface IAuthorityBindOperationRepository : IRelationRepository<Authority, AuthorityOperation>
    {
    }
}
