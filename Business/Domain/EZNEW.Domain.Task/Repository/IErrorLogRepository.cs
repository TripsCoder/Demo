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
    /// 任务异常日志存储
    /// </summary>
    public interface IErrorLogRepository : IAggregationRepository<ErrorLog>
    {
    }
}
