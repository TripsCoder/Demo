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
    /// 任务工作文件存储
    /// </summary>
    public interface IJobFileRepository : IAggregationRepository<JobFile>
    {
    }
}
