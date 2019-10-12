using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Model;
using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Domain.CTask.Service
{
    /// <summary>
    /// 执行计划服务
    /// </summary>
    public interface ITriggerService
    {
        #region 保存执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="trigger">执行计划信息</param>
        /// <returns>执行结果</returns>
        void SaveTrigger(Trigger trigger);

        #endregion

        #region 删除执行计划

        /// <summary>
        /// 删除执行计划
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        void DeleteTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 删除执行计划
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        void DeleteTrigger(IEnumerable<string> triggerIds);

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="triggers">计划信息</param>
        void ModifyTriggerState(IEnumerable<Trigger> triggers);

        #endregion

        #region 获取任务执行计划

        /// <summary>
        /// 获取任务执行计划
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划对象</returns>
        Trigger GetTrigger(IQuery query);

        /// <summary>
        /// 根据编号获取任务执行计划
        /// </summary>
        /// <param name="id">计划编号</param>
        /// <returns>任务执行计划</returns>
        Trigger GetTrigger(string id);

        #endregion

        #region 获取任务执行计划列表

        /// <summary>
        /// 获取任务执行计划列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划列表</returns>
        List<Trigger> GetTriggerList(IQuery query);

        /// <summary>
        /// 根据计划编号获取计划列表
        /// </summary>
        /// <param name="ids">执行计划编号</param>
        /// <returns>执行计划列表</returns>
        List<Trigger> GetTriggerList(IEnumerable<string> ids);

        #endregion

        #region 获取任务执行计划分页

        /// <summary>
        /// 获取任务执行计划分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划分页</returns>
        IPaging<Trigger> GetTriggerPaging(IQuery query);

        #endregion
    }
}
