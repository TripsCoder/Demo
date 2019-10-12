using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.IoC;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Framework;
using EZNEW.Framework.Paging;

namespace EZNEW.Domain.CTask.Service.Impl
{
    /// <summary>
    /// 任务分组服务
    /// </summary>
    public class JobGroupService : IJobGroupService
    {
        static IJobGroupRepository jobGroupRepository = ContainerManager.Resolve<IJobGroupRepository>();

        #region 删除任务分组

        /// <summary>
        /// 删除任务分组
        /// </summary>
        /// <param name="jobGroups">任务分组信息</param>
        public void DeleteJobGroup(IEnumerable<JobGroup> jobGroups)
        {
            if (jobGroups.IsNullOrEmpty())
            {
                return;
            }
            DeleteJobGroup(jobGroups.Select(c => c.Code));
        }

        /// <summary>
        /// 删除任务分组
        /// </summary>
        /// <param name="groupIds">任务分组编号</param>
        public void DeleteJobGroup(IEnumerable<string> groupIds)
        {
            if (groupIds.IsNullOrEmpty())
            {
                return;
            }
            var removeQuery = QueryFactory.Create<JobGroupQuery>(q => groupIds.Contains(q.Code));
            removeQuery.SetRecurve<JobGroupQuery>(c => c.Code, c => c.Parent);
            jobGroupRepository.Remove(removeQuery);
        }

        #endregion

        #region 保存工作分组

        /// <summary>
        /// 保存工作分组
        /// </summary>
        /// <param name="jobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        public void SaveJobGroup(JobGroup jobGroup)
        {
            if (jobGroup == null)
            {
                return;
            }
            if (jobGroup.Code.IsNullOrEmpty())
            {
                AddJobGroup(jobGroup);
            }
            else
            {
                UpdateJobGroup(jobGroup);
            }
        }

        /// <summary>
        /// 添加工作分组
        /// </summary>
        /// <param name="jobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        static void AddJobGroup(JobGroup jobGroup)
        {
            #region 上级

            string parentGroupId = jobGroup.Parent == null ? "" : jobGroup.Parent.Code;
            JobGroup parentGroup = null;
            if (!parentGroupId.IsNullOrEmpty())
            {
                IQuery parentQuery = QueryFactory.Create<JobGroupQuery>(c => c.Code == parentGroupId);
                parentGroup = jobGroupRepository.Get(parentQuery);
                if (parentGroup == null)
                {
                    throw new Exception("请选择正确的上级分组");
                }
            }
            jobGroup.SetParentGroup(parentGroup);

            #endregion

            jobGroup.Save();//保存
        }

        /// <summary>
        /// 更新工作分组
        /// </summary>
        /// <param name="newJobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        static void UpdateJobGroup(JobGroup newJobGroup)
        {
            JobGroup jobGroup = jobGroupRepository.Get(QueryFactory.Create<JobGroupQuery>(r => r.Code == newJobGroup.Code));
            if (jobGroup == null)
            {
                throw new Exception("没有指定要操作的分组信息");
            }
            //上级
            string newParentGroupId = newJobGroup.Parent?.Code ?? string.Empty;
            string oldParentGroupId = jobGroup.Parent?.Code ?? string.Empty;
            //上级改变后 
            if (newParentGroupId != oldParentGroupId)
            {
                JobGroup parentGroup = null;
                if (!newParentGroupId.IsNullOrEmpty())
                {
                    IQuery parentQuery = QueryFactory.Create<JobGroupQuery>(c => c.Code == newParentGroupId);
                    parentGroup = jobGroupRepository.Get(parentQuery);
                    if (parentGroup == null)
                    {
                        throw new Exception("请选择正确的上级分组");
                    }
                }
                jobGroup.SetParentGroup(parentGroup);
            }
            //修改信息
            jobGroup.Name = newJobGroup.Name;
            jobGroup.Remark = newJobGroup.Remark;
            jobGroup.Save();//保存
        }

        #endregion

        #region 获取工作分组

        /// <summary>
        /// 获取工作分组
        /// </summary>
        /// <param name="query">筛选信息</param>
        /// <returns></returns>
        public JobGroup GetJobGroup(IQuery query)
        {
            var jobGroup = jobGroupRepository.Get(query);
            return jobGroup;
        }

        #endregion

        #region 获取工作分组列表

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public List<JobGroup> GetJobGroupList(IQuery query)
        {
            var jobGroupList = jobGroupRepository.GetList(query);
            jobGroupList = LoadOtherObjectData(jobGroupList, query);
            return jobGroupList;
        }

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="groupCodes">分组编码信息</param>
        /// <returns></returns>
        public List<JobGroup> GetJobGroupList(IEnumerable<string> groupCodes)
        {
            if (groupCodes.IsNullOrEmpty())
            {
                return new List<JobGroup>(0);
            }
            IQuery query = QueryFactory.Create<JobGroupQuery>(c => groupCodes.Contains(c.Code));
            return GetJobGroupList(query);
        }

        #endregion

        #region 获取工作分组分页

        /// <summary>
        /// 获取工作分组分页
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public IPaging<JobGroup> GetJobGroupPaging(IQuery query)
        {
            var jobGroupPaging = jobGroupRepository.GetPaging(query);
            var jobGroupList = LoadOtherObjectData(jobGroupPaging, query);
            return new Paging<JobGroup>(jobGroupPaging.Page, jobGroupPaging.PageSize, jobGroupPaging.TotalCount, jobGroupList);
        }

        #endregion

        #region 加载其它数据

        /// <summary>
        /// 加载其它数据
        /// </summary>
        /// <param name="jobGroups">分组信息</param>
        /// <param name="query">筛选对象</param>
        /// <returns></returns>
        List<JobGroup> LoadOtherObjectData(IEnumerable<JobGroup> jobGroups, IQuery query)
        {
            if (jobGroups.IsNullOrEmpty())
            {
                return new List<JobGroup>(0);
            }
            if (query == null)
            {
                return jobGroups.ToList();
            }
            #region 上级/根分组

            List<string> groupCodeIds = new List<string>();
            List<JobGroup> jobGroupList = null;
            if (query.AllowLoad<JobGroup>(c => c.Parent))
            {
                groupCodeIds.AddRange(jobGroups.Select(c => c.Parent?.Code).Distinct().ToList());
            }

            if (query.AllowLoad<JobGroup>(c => c.Root))
            {
                groupCodeIds.AddRange(jobGroups.Select(c => c.Root?.Code).Distinct().ToList());
            }
            if (!groupCodeIds.IsNullOrEmpty())
            {
                jobGroupList = jobGroupRepository.GetList(QueryFactory.Create<JobGroupQuery>(c => groupCodeIds.Contains(c.Code)));
            }

            #endregion

            foreach (var jobGroup in jobGroups)
            {
                if (!jobGroupList.IsNullOrEmpty())
                {
                    if (query.AllowLoad<JobGroup>(c => c.Parent))
                    {
                        jobGroup.SetParentGroup(jobGroupList.FirstOrDefault(c => c.Code == jobGroup.Parent?.Code));
                    }
                    if (query.AllowLoad<JobGroup>(c => c.Root))
                    {
                        jobGroup.SetRootGroup(jobGroupList.FirstOrDefault(c => c.Code == jobGroup.Root?.Code));
                    }
                }
            }
            return jobGroups.ToList();
        }

        #endregion
    }
}
