using EZNEW.DataAccessContract.Sys;
using EZNEW.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Develop.DataAccess;

namespace EZNEW.DataAccess.Sys
{
    /// <summary>
    /// 权限&授权操作关联数据访问
    /// </summary>
    public class AuthorityBindOperationDataAccess : RdbDataAccess<AuthorityBindOperationEntity>, IAuthorityBindOperationDataAccess
    {
    }
}
