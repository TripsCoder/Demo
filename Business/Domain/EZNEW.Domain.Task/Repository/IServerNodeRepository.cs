
using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Repository
{
    /// <summary>
    /// 服务节点存储
    /// </summary>
    public interface IServerNodeRepository : IAggregationRepository<ServerNode>
    {
    }
}
