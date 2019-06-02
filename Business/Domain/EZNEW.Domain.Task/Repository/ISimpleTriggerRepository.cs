using EZNEW.Develop.Domain.Repository;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Repository
{
    public interface ISimpleTriggerRepository : IAggregationRepository<SimpleTrigger>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void SaveSimpleTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void RemoveSimpleTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        List<Trigger> LoadSimpleTrigger(IEnumerable<Trigger> triggers);
    }
}
