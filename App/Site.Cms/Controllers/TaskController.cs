using EZNEW.Framework;
using EZNEW.Framework.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.ServiceContract.CTask;
using EZNEW.Framework.Paging;
using EZNEW.Web.Mvc;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.CTask;
using EZNEW.CTask.Job;
using EZNEW.CTask.Client;
using EZNEW.ViewModel.Common;
using EZNEW.Framework.Response;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using EZNEW.ViewModel.CTask;
using EZNEW.ViewModel.CTask.Filter;
using EZNEW.ViewModel.CTask.Response;
using EZNEW.ViewModel.CTask.Request;
using EZNEW.Web.Utility;
using System.Threading.Tasks;
using EZNEW.Framework.Net;
using EZNEW.Framework.Net.Upload;

namespace Site.Cms.Controllers
{
    public class TaskController : WebBaseController
    {
        IJobGroupService jobGroupService = null;
        IServerNodeService serverNodeService = null;
        IJobService jobService = null;
        IJobServerHostService jobServerHostService = null;
        ITriggerService triggerService = null;
        ITriggerServerService triggerServerService = null;
        IExecuteLogService executeLogService = null;
        IErrorLogService errorLogService = null;
        IJobFileService jobFileService = null;
        public TaskController()
        {
            jobGroupService = this.Instance<IJobGroupService>();
            serverNodeService = this.Instance<IServerNodeService>();
            jobService = this.Instance<IJobService>();
            jobServerHostService = this.Instance<IJobServerHostService>();
            triggerService = this.Instance<ITriggerService>();
            triggerServerService = this.Instance<ITriggerServerService>();
            executeLogService = this.Instance<IExecuteLogService>();
            errorLogService = this.Instance<IErrorLogService>();
            jobFileService = this.Instance<IJobFileService>();
        }

        #region 工作分组管理

        #region 工作分组列表

        public ActionResult JobGroupList()
        {
            return View();
        }

        #endregion

        #region 编辑/添加工作分组

        public ActionResult EditJobGroup(EditJobGroupViewModel jobGroup)
        {
            if (IsPost)
            {
                var jobGroupCmd = jobGroup.MapTo<JobGroupCmdDto>();
                var saveResult = jobGroupService.SaveJobGroup(jobGroupCmd);
                Result result = saveResult.Success ? Result.SuccessResult(saveResult.Message) : Result.FailedResult(saveResult.Message);
                var ajaxResult = AjaxResult.CopyFromResult(result);
                ajaxResult.SuccessClose = true;
                return Json(ajaxResult);
            }
            else if (!jobGroup.Code.IsNullOrEmpty())
            {
                JobGroupFilterDto filter = new JobGroupFilterDto()
                {
                    Codes = new List<string>()
                    {
                        jobGroup.Code
                    },
                    LoadParent = true
                };
                jobGroup = jobGroupService.GetJobGroup(filter).MapTo<EditJobGroupViewModel>();
            }
            return View(jobGroup);
        }

        #endregion

        #region 删除工作分组

        [HttpPost]
        public ActionResult DeleteJobGroup(List<string> codes)
        {
            Result result = jobGroupService.DeleteJobGroup(new DeleteJobGroupCmdDto()
            {
                JobGroupIds = codes
            });
            var ajaxResult = AjaxResult.CopyFromResult(result);
            return Json(ajaxResult);
        }

        #endregion

        #region 修改工作分组排序

        [HttpPost]
        public ActionResult ChangeJobGroupSort(string code, int sortIndex)
        {
            Result result = null;
            result = jobGroupService.ModifySortIndex(new ModifyJobGroupSortCmdDto()
            {
                Code = code,
                NewSort = sortIndex
            });
            var ajaxResult = AjaxResult.CopyFromResult(result);
            return Json(ajaxResult);
        }

        #endregion

        #region 获取一级数据

        [HttpPost]
        public IActionResult GetJobGroupValue(List<string> excludeIds)
        {
            var jobGroupDatas = InitJobGroupTreeData("", excludeIds);
            return Json(new
            {
                AllNodes = jobGroupDatas.Item1,
                AllJobGroup = jobGroupDatas.Item3
            });
        }

        #endregion

        #region 获取下级数据

        /// <summary>
        /// 获取下级数据
        /// </summary>
        /// <param name="parentId">上级编号</param>
        /// <returns></returns>
        public ActionResult LoadChildJobGroups(string parentId, List<string> excludeIds = null)
        {
            List<JobGroupViewModel> childJobGroups = null;
            if (!parentId.IsNullOrEmpty())
            {
                JobGroupFilterDto filter = new JobGroupFilterDto()
                {
                    Parent = parentId
                };
                if (!excludeIds.IsNullOrEmpty())
                {
                    filter.ExcludeCodes = excludeIds;
                }
                childJobGroups = jobGroupService.GetJobGroupList(filter).Select(c => c.MapTo<JobGroupViewModel>()).OrderBy(r => r.Sort).ToList();
            }
            childJobGroups = childJobGroups ?? new List<JobGroupViewModel>(0);
            List<TreeNode> treeNodeList = childJobGroups.Select(c => JobGroupToTreeNode(c)).ToList();
            string nodesString = JsonSerialize.ObjectToJson<List<TreeNode>>(treeNodeList);
            string jobGroupsData = JsonSerialize.ObjectToJson(childJobGroups.ToDictionary(c => c.Code.ToString()));
            return Json(new
            {
                ChildNodes = nodesString,
                JobGroupData = jobGroupsData
            });
        }

        #endregion

        #region 数据序列化

        string JobGroupListToJsonString(IEnumerable<JobGroupViewModel> jobGroupList)
        {
            List<TreeNode> nodeList = JobGroupListToTreeNodes(jobGroupList.ToList());
            return JsonSerialize.ObjectToJson<List<TreeNode>>(nodeList);
        }

        List<TreeNode> JobGroupListToTreeNodes(List<JobGroupViewModel> jobGroupList)
        {
            if (jobGroupList.IsNullOrEmpty())
            {
                return new List<TreeNode>(0);
            }
            List<TreeNode> nodeList = new List<TreeNode>(jobGroupList.Count);
            var levelOneJobGroups = jobGroupList.Where(c => c.Level == 1).OrderBy(c => c.Sort);
            foreach (var jobGroup in levelOneJobGroups)
            {
                TreeNode node = JobGroupToTreeNode(jobGroup);
                AppendChildNodes(node, jobGroup.Code, jobGroupList);
                nodeList.Add(node);
            }
            return nodeList;
        }

        void AppendChildNodes(TreeNode parentNode, string parentCode, IEnumerable<JobGroupViewModel> allJobGroups)
        {
            var childJobGroups = allJobGroups.Where(c => c.Parent != null && c.Parent.Code == parentCode && c.Code != c.Parent.Code && !c.Parent.Code.IsNullOrEmpty()).OrderBy(c => c.Sort);
            if (childJobGroups.IsNullOrEmpty())
            {
                return;
            }
            foreach (var jobGroup in childJobGroups)
            {
                TreeNode node = JobGroupToTreeNode(jobGroup);
                parentNode.Children.Add(node);
                AppendChildNodes(node, jobGroup.Code, allJobGroups);
            }
        }

        TreeNode JobGroupToTreeNode(JobGroupViewModel jobGroup)
        {
            return new TreeNode()
            {
                Value = jobGroup.Code.ToString(),
                Text = jobGroup.Name,
                Children = new List<TreeNode>(),
                IsParent = true,
                LoadData = false
            };
        }

        Tuple<string, string, string> InitJobGroupTreeData(string parentId, List<string> excludeIds = null)
        {
            JobGroupFilterDto filter = new JobGroupFilterDto();
            if (!parentId.IsNullOrEmpty())
            {
                filter.Parent = parentId;
            }
            else
            {
                filter.Level = 1;
            }
            if (!excludeIds.IsNullOrEmpty())
            {
                filter.ExcludeCodes = excludeIds;
            }
            List<JobGroupViewModel> allJobGroups = jobGroupService.GetJobGroupList(filter).Select(c => c.MapTo<JobGroupViewModel>()).ToList();
            string allNodesString = JobGroupListToJsonString(allJobGroups);
            JobGroupViewModel[] copyJobGroups = new JobGroupViewModel[allJobGroups.Count];
            allJobGroups.CopyTo(copyJobGroups);
            List<JobGroupViewModel> selectJobGroups = copyJobGroups.ToList();
            selectJobGroups.Insert(0, new JobGroupViewModel()
            {
                Name = "一级工作分组",
                Code = "",
                Level = 1
            });
            string selectNodesString = JobGroupListToJsonString(selectJobGroups);
            return new Tuple<string, string, string>(allNodesString, selectNodesString, JsonSerialize.ObjectToJson(allJobGroups.ToDictionary(c => c.Code.ToString())));
        }

        #endregion

        #region 工作分组多选

        public ActionResult JobGroupMultipleSelect()
        {
            return View();
        }

        #endregion

        #region 工作分组单选

        public ActionResult JobGroupSingleSelect()
        {
            return View();
        }

        #endregion

        #endregion

        #region 服务节点管理

        #region 服务节点列表

        public ActionResult ServerNodeList()
        {
            return View();
        }

        public ActionResult SearchServerNode(ServerNodeFilterViewModel filter)
        {
            IPaging<ServerNodeViewModel> serverNodePager = serverNodeService.GetServerNodePaging(filter.MapTo<ServerNodeFilterDto>()).ConvertTo<ServerNodeViewModel>();
            object objResult = new
            {
                TotalCount = serverNodePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverNodePager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 编辑/添加服务节点

        public ActionResult EditServerNode(EditServerNodeViewModel serverNode)
        {
            if (IsPost)
            {
                SaveServerNodeCmdDto saveInfo = new SaveServerNodeCmdDto()
                {
                    ServerNode = serverNode.MapTo<ServerNodeCmdDto>()
                };
                var result = serverNodeService.SaveServerNode(saveInfo);
                return Json(result);
            }
            else if (!(serverNode.Id.IsNullOrEmpty()))
            {
                ServerNodeFilterDto filterDto = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        serverNode.Id
                    }
                };
                serverNode = serverNodeService.GetServerNode(filterDto).MapTo<EditServerNodeViewModel>();
            }
            return View(serverNode);
        }

        #endregion

        #region 删除服务节点

        public ActionResult DeleteServerNode(string ids)
        {
            IEnumerable<string> idArray = ids.LSplit(",");
            Result result = serverNodeService.DeleteServerNode(new DeleteServerNodeCmdDto()
            {
                ServerNodeIds = idArray
            });
            return Json(result);
        }

        #endregion

        #region 服务详情

        public ActionResult ServerNodeDetail(string id)
        {
            ServerNodeViewModel server = null;
            if (!id.IsNullOrEmpty())
            {
                ServerNodeFilterDto filterDto = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        id
                    }
                };
                server = serverNodeService.GetServerNode(filterDto).MapTo<ServerNodeViewModel>();
            }
            if (server == null)
            {
                return Content("信息获取失败");
            }
            return View(server);
        }

        #endregion

        #region 服务节点多选

        public ActionResult ServerNodeMultipleSelect()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ServerNodeMultipleSelectSearch(ServerNodeFilterViewModel filter)
        {
            ServerNodeFilterDto filterDto = filter.MapTo<ServerNodeFilterDto>();
            IPaging<ServerNodeViewModel> serverNodePager = serverNodeService.GetServerNodePaging(filterDto).ConvertTo<ServerNodeViewModel>();
            object objResult = new
            {
                TotalCount = serverNodePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverNodePager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 修改服务运行状态

        [HttpPost]
        public ActionResult ModifyServiceRunState(ServerNodeViewModel service)
        {
            ModifyServerNodeRunStatusCmdDto cmd = new ModifyServerNodeRunStatusCmdDto()
            {
                Servers = new List<ServerNodeCmdDto>()
                {
                    service.MapTo<ServerNodeCmdDto>()
                }
            };
            return Json(serverNodeService.ModifyRunState(cmd));
        }

        #endregion

        #endregion

        #region 工作任务管理

        #region 工作任务列表

        public ActionResult JobList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchJob(JobFilterViewModel filter)
        {
            var filterDto = filter.MapTo<JobFilterDto>();
            IPaging<JobViewModel> jobPager = jobService.GetJobPaging(filterDto).ConvertTo<JobViewModel>();

            #region 分组

            IEnumerable<string> groupCodes = jobPager.Select(c => c.Group?.Code).Distinct().ToList();
            JobGroupFilterDto groupFilter = new JobGroupFilterDto()
            {
                Codes = groupCodes.ToList()
            };
            IEnumerable<JobGroupViewModel> jobGroupList = jobGroupService.GetJobGroupList(groupFilter).Select(c => c.MapTo<JobGroupViewModel>());
            foreach (var job in jobPager)
            {
                job.Group = jobGroupList.FirstOrDefault(c => c.Code == job.Group?.Code);
            }

            #endregion

            object objResult = new
            {
                TotalCount = jobPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(jobPager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 任务详情

        public ActionResult JobDetail(string id)
        {
            var jobFilter = new JobFilterDto()
            {
                Ids = new List<string>()
                {
                    id
                },
                LoadGroup = true
            };
            var nowJob = jobService.GetJob(jobFilter).MapTo<JobViewModel>();
            if (nowJob == null)
            {
                return Content("获取数据失败");
            }
            return View(nowJob);
        }

        #endregion

        #region 编辑/添加工作任务

        public ActionResult EditJob(EditJobViewModel job,string preGroupId)
        {
            if (IsPost)
            {
                var result = jobService.SaveJob(new SaveJobCmdDto()
                {
                    Job = job.MapTo<JobCmdDto>()
                });
                return Json(result);
            }
            else if (!(job.Id.IsNullOrEmpty()))
            {
                var jobFilter = new JobFilterDto()
                {
                    Ids = new List<string>()
                {
                    job.Id
                },
                    LoadGroup = true
                };
                job = jobService.GetJob(jobFilter).MapTo<EditJobViewModel>();
            }
            else if (!preGroupId.IsNullOrEmpty())
            {
                job.Group = jobGroupService.GetJobGroup(new JobGroupFilterDto()
                {
                    Codes = new List<string>()
                    {
                        preGroupId
                    }
                }).MapTo<EditJobGroupViewModel>();
            }
            return View(job);
        }

        #endregion

        #region 删除工作任务

        public ActionResult DeleteJob(IEnumerable<string> jobIds)
        {
            Result result = jobService.DeleteJob(new DeleteJobCmdDto()
            {
                JobIds = jobIds
            });
            return Json(result);
        }

        #endregion

        #region 工作任务多选

        public ActionResult JobMultiSelect()
        {
            var result = InitJobGroupTreeData("");
            ViewBag.AllNodes = result.Item1;
            return View();
        }

        [HttpPost]
        public ActionResult JobMultiSelectSearch(JobFilterViewModel filter)
        {
            var filterDto = filter.MapTo<JobFilterDto>();
            List<JobViewModel> jobList = jobService.GetJobList(filterDto).Select(c => c.MapTo<JobViewModel>()).ToList();
            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(jobList)
            };
            return Json(result);
        }

        #endregion

        #region 加载分组任务


        [HttpPost]
        public ActionResult GetGroupJobs(string groupId, string key)
        {
            JobFilterDto filter = new JobFilterDto()
            {
                Group = groupId,
                Name = key
            };
            List<JobViewModel> jobList = jobService.GetJobList(filter).Select(c => c.MapTo<JobViewModel>()).ToList();
            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(jobList)
            };
            return Json(result);
        }

        #endregion

        #region 修改任务运行状态

        [HttpPost]
        public ActionResult ModifyJobRunState(JobViewModel job)
        {
            ModifyJobRunStatusCmdDto stateInfo = new ModifyJobRunStatusCmdDto()
            {
                Jobs = new List<JobCmdDto>()
                {
                    job.MapTo<JobCmdDto>()
                }
            };
            return Json(jobService.ModifyJobRunState(stateInfo));
        }

        #endregion

        #region 上传工作文件

        public async Task<ActionResult> UploadJobFile()
        {
            var file = Request.Form.Files["job_file"];
            object parameters = null;
            var result = await WebUploadHelper.UploadByConfigAsync(new UploadFile()
            {
                FileName = file.FileName,
                FileType = "ctask",
                Rename = true,
                Folder = DateTime.Now.ToString("yyyyMMdd")
            }, file.OpenReadStream().ToBytes(), parameters).ConfigureAwait(false);
            return Json(result);

        }

        #endregion

        #endregion

        #region 任务&服务承载

        #region 获取任务承载服务

        public ActionResult GetJobServerHostByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            foreach (var serverHost in serverHostPager)
            {
                serverHost.Server = serverList.FirstOrDefault(c => c.Id == serverHost.Server?.Id);
            }
            var serverHostViewPager = serverHostPager.ConvertTo<JobServerHostViewModel>();
            object objResult = new
            {
                TotalCount = serverHostViewPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverHostViewPager.ToList())
            };
            return Json(objResult);
        }

        public ActionResult GetJobServerHostByServer(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //任务信息
            List<string> jobIds = serverHostPager.Select(c => c.Job?.Id).Distinct().ToList();
            JobFilterDto jobFilter = new JobFilterDto()
            {
                Ids = jobIds
            };
            var jobList = jobService.GetJobList(jobFilter);
            foreach (var serverHost in serverHostPager)
            {
                serverHost.Job = jobList.FirstOrDefault(c => c.Id == serverHost.Job?.Id);
            }
            var serverHostViewPager = serverHostPager.ConvertTo<JobServerHostViewModel>();
            object objResult = new
            {
                TotalCount = serverHostViewPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverHostPager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 保存任务承载服务

        [HttpPost]
        public ActionResult SaveJobServerHost(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            if (serverHosts.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定任何要保存的信息"));
            }
            SaveJobServerHostCmdDto saveInfo = new SaveJobServerHostCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => { c.RunStatus = JobServerStatus.启用; return c.MapTo<JobServerHostCmdDto>(); }).ToList()
            };
            var result = jobServerHostService.SaveJobServerHost(saveInfo);
            return Json(result);
        }

        #endregion

        #region 修改任务承载运行状态

        [HttpPost]
        public ActionResult ModifyJobServerHostRunState(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            ModifyJobServerHostRunStatusCmdDto modifyInfo = new ModifyJobServerHostRunStatusCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => c.MapTo<JobServerHostCmdDto>()).ToList()
            };
            var result = jobServerHostService.ModifyRunState(modifyInfo);
            return Json(result);
        }

        #endregion

        #region 删除任务承载服务

        [HttpPost]
        public ActionResult DeleteJobServerHost(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            DeleteJobServerHostCmdDto deleteInfo = new DeleteJobServerHostCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => c.MapTo<JobServerHostCmdDto>()).ToList()
            };
            var result = jobServerHostService.DeleteJobServerHost(deleteInfo);
            return Json(result);
        }

        #endregion

        #endregion

        #region 执行计划

        public ActionResult EditTrigger(TriggerViewModel trigger)
        {
            if (IsPost)
            {
                #region 初始化计划基本信息

                if (trigger.Type == TaskTriggerType.简单)
                {
                    Result<SimpleTriggerViewModel> simpleDataResult = InitSimpleTriggerValue(trigger);
                    if (!simpleDataResult.Success)
                    {
                        return Json(simpleDataResult);
                    }
                    trigger = simpleDataResult.Object;
                }
                else
                {
                    Result<ExpressionTriggerViewModel> expressionDataResult = InitExpressionTriggerValue(trigger);
                    if (!expressionDataResult.Success)
                    {
                        return Json(expressionDataResult);
                    }
                    trigger = expressionDataResult.Object;
                }

                #endregion

                #region 计划附加条件

                var conditionResult = InitTriggerCondition(trigger);
                if (!conditionResult.Success)
                {
                    return Json(conditionResult);
                }
                trigger.Condition = conditionResult.Object;

                #endregion

                //#region 应用对象

                //var serverResult = InitTriggerServer(trigger);
                //if (!serverResult.Success)
                //{
                //    return Json(serverResult);
                //}

                //#endregion

                SaveTriggerCmdDto saveInfo = new SaveTriggerCmdDto()
                {
                    Trigger = trigger.MapTo<TriggerCmdDto>(),
                    //TriggerServers = serverResult.Data?.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
                };
                return Json(triggerService.SaveTrigger(saveInfo));
            }
            if (!trigger.Id.IsNullOrEmpty())
            {
                TriggerFilterDto filter = new TriggerFilterDto()
                {
                    Ids = new List<string>()
                    {
                        trigger.Id
                    },
                    LoadCondition = true
                };
                trigger = triggerService.GetTrigger(filter).MapTo<TriggerViewModel>();
                if (trigger == null)
                {
                    return Content("获取信息失败");
                }
            }
            if (trigger.Job == null || trigger.Job.Id.IsNullOrEmpty())
            {
                return Content("请指定要添加计划的任务");
            }
            JobFilterDto jobFilter = new JobFilterDto()
            {
                Ids = new List<string>()
                    {
                        trigger.Job.Id
                    }
            };
            var job = jobService.GetJob(jobFilter);
            if (job == null)
            {
                return Content("请指定要添加计划的任务");
            }
            trigger.Job = job.MapTo<JobViewModel>();
            return View(trigger);
        }

        public ActionResult TriggerDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return Content("信息获取失败");
            }
            TriggerFilterDto filter = new TriggerFilterDto()
            {
                Ids = new List<string>()
                {
                    id
                },
                LoadJob = true,
                LoadCondition = true
            };
            TriggerViewModel trigger = triggerService.GetTrigger(filter).MapTo<TriggerViewModel>();
            if (trigger == null)
            {
                return Content("信息获取失败");
            }
            return View(trigger);
        }

        /// <summary>
        /// 获取工作任务的执行计划
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobTrigger(TriggerFilterViewModel filter)
        {
            IPaging<TriggerViewModel> triggerPager = triggerService.GetTriggerPaging(filter.MapTo<TriggerFilterDto>()).ConvertTo<TriggerViewModel>();
            object objResult = new
            {
                TotalCount = triggerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerPager.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult RemoveTrigger(IEnumerable<string> triggerIds)
        {
            DeleteTriggerCmdDto deleteInfo = new DeleteTriggerCmdDto()
            {
                TriggerIds = triggerIds
            };
            return Json(triggerService.DeleteTrigger(deleteInfo));
        }

        [HttpPost]
        public ActionResult ModifyTriggerState(IEnumerable<TriggerViewModel> triggers)
        {
            ModifyTriggerStatusCmdDto stateInfo = new ModifyTriggerStatusCmdDto()
            {
                Triggers = triggers.Select(c => c.MapTo<TriggerCmdDto>()).ToList()
            };
            return Json(triggerService.ModifyTriggerState(stateInfo));
        }

        #region 初始执行计划信息

        Result<SimpleTriggerViewModel> InitSimpleTriggerValue(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<SimpleTriggerViewModel>.FailedResult("执行计划信息为空");
            }
            bool repeatForever = false;
            string repeatForeverVallue = Request.Form["RepeatForever"];
            bool.TryParse(repeatForeverVallue.LSplit(",")[0], out repeatForever);
            int repeatCount = 0;
            string repeatCountValue = Request.Form["RepeatCount"];
            int.TryParse(repeatCountValue, out repeatCount);
            int repeatInterval = 0;
            string repeatIntervalValue = Request.Form["RepeatInterval"];
            int.TryParse(repeatIntervalValue, out repeatInterval);
            if (!repeatForever)
            {
                if (repeatCount <= 0)
                {
                    return Result<SimpleTriggerViewModel>.FailedResult("请指定任务要重复执行的次数");
                }
            }
            else
            {
                repeatCount = 0;
            }
            if (repeatInterval <= 0)
            {
                return Result<SimpleTriggerViewModel>.FailedResult("请指定任务执行的间隔毫秒数");
            }
            var result = Result<SimpleTriggerViewModel>.SuccessResult("数据初始化成功");
            result.Data = trigger.MapTo<SimpleTriggerViewModel>();
            result.Object.RepeatForever = repeatForever;
            result.Object.RepeatCount = repeatCount;
            result.Object.RepeatInterval = repeatInterval;
            return result;
        }

        Result<ExpressionTriggerViewModel> InitExpressionTriggerValue(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<ExpressionTriggerViewModel>.FailedResult("执行计划数据为空");
            }
            var options = Enum.GetValues(typeof(TaskTriggerExpressionItem));
            List<ExpressionItemViewModel> expressionItems = new List<ExpressionItemViewModel>();
            foreach (TaskTriggerExpressionItem item in options)
            {
                string valueTypeKey = string.Format("Item_ValueType_{0}", (int)item);
                int valueTypeVal = 0;
                int.TryParse(Request.Form[valueTypeKey], out valueTypeVal);
                if (!Enum.IsDefined(typeof(TaskTriggerExpressionItemConfigType), valueTypeVal))
                {
                    continue;
                }
                TaskTriggerExpressionItemConfigType valueType = (TaskTriggerExpressionItemConfigType)valueTypeVal;
                ExpressionItemViewModel expressionItem = new ExpressionItemViewModel()
                {
                    ValueType = valueType,
                    Option = item,
                };
                switch (valueType)
                {
                    case TaskTriggerExpressionItemConfigType.集合值:
                        List<string> arrayValues = new List<string>();
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            string itemWeekArrayVal = Request.Form["Item_Week_ArrayVal"];
                            arrayValues = itemWeekArrayVal.Trim().LSplit(",").ToList();
                        }
                        else
                        {
                            string itemArrayValueItems = Request.Form["Item_ArrayValue_" + (int)item];
                            arrayValues = itemArrayValueItems.Trim().LSplit(",").ToList();
                        }
                        if (arrayValues.IsNullOrEmpty())
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("选项【{0}】集合值为空", item.ToString()));
                        }
                        expressionItem.BeginValue = 0;
                        expressionItem.EndValue = 0;
                        expressionItem.ArrayValue = arrayValues;
                        break;
                    case TaskTriggerExpressionItemConfigType.范围值:
                        int beginValue = 0;
                        int endValue = 0;
                        string beginVal = Request.Form["Item_BeginValue_" + (int)item];
                        string endVal = Request.Form["Item_EndValue_" + (int)item];
                        if (!int.TryParse(beginVal.Trim(), out beginValue) || !int.TryParse(endVal.Trim(), out endValue))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的范围值", item.ToString()));
                        }
                        if (beginValue > endValue)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("选项【{0}】范围起始值不能大于结束值", item.ToString()));
                        }
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            Type weekType = typeof(TaskWeek);
                            if (!Enum.IsDefined(weekType, beginValue) || !Enum.IsDefined(weekType, endValue))
                            {
                                return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的范围值", item.ToString()));
                            }
                        }
                        expressionItem.BeginValue = beginValue;
                        expressionItem.EndValue = endValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.开始_间隔:
                        int intervalBeginValue = 0;
                        int intervalValue = 0;
                        string intervalBeginVal = Request.Form["Item_IntervalBeginValue_" + (int)item];
                        string intervalVal = Request.Form["Item_IntervalSplitValue_" + (int)item];
                        if (!int.TryParse(intervalBeginVal.Trim(), out intervalBeginValue) || !int.TryParse(intervalVal.Trim(), out intervalValue))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的起始/间隔值", item.ToString()));
                        }
                        if (intervalValue <= 0)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的执行间隔值", item.ToString()));
                        }
                        expressionItem.BeginValue = intervalBeginValue;
                        expressionItem.EndValue = intervalValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月倒数第N天:
                        int monthEndDay = 0;
                        string monthEndDayVal = Request.Form["Item_MonthEndDay_" + (int)item];
                        if (!int.TryParse(monthEndDayVal.Trim(), out monthEndDay) || monthEndDay <= 0 || monthEndDay > 31)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的倒数天数", item.ToString()));
                        }
                        expressionItem.BeginValue = monthEndDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月个最后一个星期N:
                        int monthLastWeekDay = 0;
                        string monthLastWeekDayVal = Request.Form["Item_MonthLastWeekDay_" + (int)item];
                        if (!int.TryParse(monthLastWeekDayVal.Trim(), out monthLastWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthLastWeekDay))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthLastWeekDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.本月第M个星期N:
                        int monthWhichWeekDayNumber = 0;
                        int monthWhichWeekDay = 0;
                        string monthWhichWeekDayNumberVal = Request.Form["Item_MonthWhichWeekDay_Num_" + (int)item];
                        string monthWhichWeekDayVal = Request.Form["Item_MonthWhichWeekDay_" + (int)item];
                        if (!int.TryParse(monthWhichWeekDayNumberVal.Trim(), out monthWhichWeekDayNumber) || !int.TryParse(monthWhichWeekDayVal.Trim(), out monthWhichWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthWhichWeekDay) || monthWhichWeekDayNumber <= 0)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthWhichWeekDayNumber;
                        expressionItem.EndValue = monthWhichWeekDay;
                        break;
                }
                expressionItems.Add(expressionItem);
            }
            ExpressionTriggerViewModel expressionTrigger = trigger.MapTo<ExpressionTriggerViewModel>();
            expressionTrigger.ExpressionItems = expressionItems;
            var result = Result<ExpressionTriggerViewModel>.SuccessResult("初始化成功");
            result.Data = expressionTrigger;
            return result;
        }

        #endregion

        #region 初始化执行计划条件

        Result<TriggerConditionViewModel> InitTriggerCondition(TriggerViewModel trigger)
        {
            if (trigger == null || trigger.Condition == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            Result<TriggerConditionViewModel> conditionResult = null;
            switch (trigger.Condition.Type)
            {
                case TaskTriggerConditionType.不限制:
                default:
                    conditionResult = Result<TriggerConditionViewModel>.SuccessResult("没有条件限制");
                    break;
                case TaskTriggerConditionType.固定日期:
                    conditionResult = InitFullDateCondition(trigger);
                    break;
                case TaskTriggerConditionType.星期配置:
                    conditionResult = InitWeeklyCondition(trigger);
                    break;
                case TaskTriggerConditionType.每天时间段:
                    conditionResult = InitDaylyCondition(trigger);
                    break;
                case TaskTriggerConditionType.每年日期:
                    conditionResult = InitAnnualCondition(trigger);
                    break;
                case TaskTriggerConditionType.每月日期:
                    conditionResult = InitMontylyCondition(trigger);
                    break;
                case TaskTriggerConditionType.自定义:
                    conditionResult = InitExpressionCondition(trigger);
                    break;
            }
            return conditionResult;
        }

        /// <summary>
        /// 初始化年度计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitAnnualCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] yearMonthKeys = Request.Form.Keys.Where(c => c.StartsWith("Condition_YearDay_Month")).ToArray();
            List<AnnualConditionDayViewModel> dayList = new List<AnnualConditionDayViewModel>();
            foreach (var monthKey in yearMonthKeys)
            {
                string[] monthKeyArray = monthKey.LSplit("-");
                int keyIndex = 0;
                if (monthKeyArray.Length != 2 || !int.TryParse(monthKeyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int month = 0;
                int day = 0;
                bool enable = false;
                if (!int.TryParse(Request.Form[monthKey], out month) || month <= 0 || month > 12)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的月份");
                }
                if (!int.TryParse(Request.Form["Condition_YearDay_Day-" + keyIndex], out day) || day <= 0 || day > 31)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request.Form["Condition_YearDay_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                dayList.Add(new AnnualConditionDayViewModel()
                {
                    Month = month,
                    Day = day,
                    Include = enable
                });
            }
            if (dayList.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerAnnualConditionViewModel annualCondition = new TriggerAnnualConditionViewModel()
            {
                Type = TaskTriggerConditionType.每年日期,
                Days = dayList
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = annualCondition;
            return result;
        }

        /// <summary>
        /// 初始化月份计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitMontylyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] monthDayKeys = Request.Form.Keys.Where(c => c.StartsWith("Condition_MonthDay_Day")).ToArray();
            List<MonthConditionDayViewModel> days = new List<MonthConditionDayViewModel>();
            foreach (string dayKey in monthDayKeys)
            {
                if (dayKey.IsNullOrEmpty())
                {
                    continue;
                }
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int day = 0;
                bool enable = false;
                if (!int.TryParse(Request.Form[dayKey], out day) || day <= 0 || day > 31)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request.Form["Condition_MonthDay_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new MonthConditionDayViewModel()
                {
                    Day = day,
                    Include = enable
                });
            }
            if (days.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerMonthlyConditionViewModel monthlyCondition = new TriggerMonthlyConditionViewModel()
            {
                Days = days,
                Type = TaskTriggerConditionType.每月日期
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = monthlyCondition;
            return result;
        }

        /// <summary>
        /// 初始化固定日期计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitFullDateCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] dayKeys = Request.Form.Keys.Where(c => c.StartsWith("Condition_FullDate_Date")).ToArray();
            List<FullDateConditionDateViewModel> days = new List<FullDateConditionDateViewModel>();
            foreach (var dayKey in dayKeys)
            {
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                DateTime date = DateTime.Now;
                bool enable = false;
                if (!DateTime.TryParse(Request.Form[dayKey], out date))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request.Form["Condition_FullDate_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new FullDateConditionDateViewModel()
                {
                    Date = date,
                    Include = enable
                });
            }
            TriggerFullDateConditionViewModel fullDateCondition = new TriggerFullDateConditionViewModel()
            {
                Dates = days,
                Type = TaskTriggerConditionType.固定日期
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = fullDateCondition;
            return result;
        }

        /// <summary>
        /// 初始化每天时间段计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitDaylyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string beginTime = Request.Form["Condition_TimeRange_BeginTime"];
            string endTime = Request.Form["Condition_TimeRange_EndTime"];
            if (beginTime.IsNullOrEmpty() || endTime.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请设置完整的时间区间");
            }
            bool inversion = false;
            bool.TryParse(Request.Form["Condition_TimeRange_Inversion"], out inversion);
            TriggerDailyConditionViewModel daylyCondition = new TriggerDailyConditionViewModel()
            {
                BeginTime = beginTime,
                EndTime = endTime,
                Inversion = inversion,
                Type = TaskTriggerConditionType.每天时间段
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = daylyCondition;
            return result;
        }

        /// <summary>
        /// 初始化星期计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitWeeklyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] weekDayKeys = Request.Form.Keys.Where(c => c.StartsWith("Condition_Week_Day")).ToArray();
            List<WeeklyConditionDayViewModel> days = new List<WeeklyConditionDayViewModel>();
            foreach (var dayKey in weekDayKeys)
            {
                if (dayKey.IsNullOrEmpty())
                {
                    continue;
                }
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int weekDay = 0;
                bool enable = false;
                if (!int.TryParse(Request.Form[dayKey], out weekDay) || !Enum.IsDefined(typeof(TaskWeek), weekDay))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请选择正确的日期");
                }
                if (!bool.TryParse(Request.Form["Condition_Week_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new WeeklyConditionDayViewModel()
                {
                    Day = weekDay,
                    Include = enable
                });
            }
            if (days.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerWeeklyConditionViewModel weeklyCondition = new TriggerWeeklyConditionViewModel()
            {
                Days = days,
                Type = TaskTriggerConditionType.星期配置
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = weeklyCondition;
            return result;
        }

        /// <summary>
        /// 初始化自定义计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitExpressionCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            var options = Enum.GetValues(typeof(TaskTriggerExpressionItem));
            List<ExpressionItemViewModel> expressionItems = new List<ExpressionItemViewModel>();
            foreach (TaskTriggerExpressionItem item in options)
            {
                string valueTypeKey = string.Format("Condition_ValueType_{0}", (int)item);
                int valueTypeVal = 0;
                int.TryParse(Request.Form[valueTypeKey], out valueTypeVal);
                if (!Enum.IsDefined(typeof(TaskTriggerExpressionItemConfigType), valueTypeVal))
                {
                    continue;
                }
                TaskTriggerExpressionItemConfigType valueType = (TaskTriggerExpressionItemConfigType)valueTypeVal;
                ExpressionItemViewModel expressionItem = new ExpressionItemViewModel()
                {
                    ValueType = valueType,
                    Option = item,
                };
                switch (valueType)
                {
                    case TaskTriggerExpressionItemConfigType.集合值:
                        List<string> arrayValues = new List<string>();
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            string conditionWeekArrayVal = Request.Form["Condition_Week_ArrayVal"];
                            arrayValues = conditionWeekArrayVal.Trim().LSplit(",").ToList();
                        }
                        else
                        {
                            string conditionArrayValue = Request.Form["Condition_ArrayValue_" + (int)item];
                            arrayValues = conditionArrayValue.Trim().LSplit(",").ToList();
                        }
                        if (arrayValues.IsNullOrEmpty())
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("选项【{0}】集合值为空", item.ToString()));
                        }
                        expressionItem.BeginValue = 0;
                        expressionItem.EndValue = 0;
                        expressionItem.ArrayValue = arrayValues;
                        break;
                    case TaskTriggerExpressionItemConfigType.范围值:
                        int beginValue = 0;
                        int endValue = 0;
                        string beginVal = Request.Form["Condition_BeginValue_" + (int)item];
                        string endVal = Request.Form["Condition_EndValue_" + (int)item];
                        if (!int.TryParse(beginVal.Trim(), out beginValue) || !int.TryParse(endVal.Trim(), out endValue))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的范围值", item.ToString()));
                        }
                        if (beginValue > endValue)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("选项【{0}】范围起始值不能大于结束值", item.ToString()));
                        }
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            Type weekType = typeof(TaskWeek);
                            if (!Enum.IsDefined(weekType, beginValue) || !Enum.IsDefined(weekType, endValue))
                            {
                                return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的范围值", item.ToString()));
                            }
                        }
                        expressionItem.BeginValue = beginValue;
                        expressionItem.EndValue = endValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.开始_间隔:
                        int intervalBeginValue = 0;
                        int intervalValue = 0;
                        string intervalBeginVal = Request.Form["Condition_IntervalBeginValue_" + (int)item];
                        string intervalVal = Request.Form["Condition_IntervalSplitValue_" + (int)item];
                        if (!int.TryParse(intervalBeginVal.Trim(), out intervalBeginValue) || !int.TryParse(intervalVal.Trim(), out intervalValue))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的起始/间隔值", item.ToString()));
                        }
                        if (intervalValue <= 0)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的执行间隔值", item.ToString()));
                        }
                        expressionItem.BeginValue = intervalBeginValue;
                        expressionItem.EndValue = intervalValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月倒数第N天:
                        int monthEndDay = 0;
                        string monthEndDayVal = Request.Form["Condition_MonthEndDay_" + (int)item];
                        if (!int.TryParse(monthEndDayVal.Trim(), out monthEndDay) || monthEndDay <= 0 || monthEndDay > 31)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的倒数天数", item.ToString()));
                        }
                        expressionItem.BeginValue = monthEndDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月个最后一个星期N:
                        int monthLastWeekDay = 0;
                        string monthLastWeekDayVal = Request.Form["Condition_MonthLastWeekDay_" + (int)item];
                        if (!int.TryParse(monthLastWeekDayVal.Trim(), out monthLastWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthLastWeekDay))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthLastWeekDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.本月第M个星期N:
                        int monthWhichWeekDayNumber = 0;
                        int monthWhichWeekDay = 0;
                        string monthWhichWeekDayNumberVal = Request.Form["Condition_MonthWhichWeekDay_Num_" + (int)item];
                        string monthWhichWeekDayVal = Request.Form["Condition_MonthWhichWeekDay_" + (int)item];
                        if (!int.TryParse(monthWhichWeekDayNumberVal.Trim(), out monthWhichWeekDayNumber) || !int.TryParse(monthWhichWeekDayVal.Trim(), out monthWhichWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthWhichWeekDay) || monthWhichWeekDayNumber <= 0)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthWhichWeekDayNumber;
                        expressionItem.EndValue = monthWhichWeekDay;
                        break;
                }
                expressionItems.Add(expressionItem);
            }
            TriggerExpressionConditionViewModel expressionCondition = new TriggerExpressionConditionViewModel()
            {
                Type = TaskTriggerConditionType.自定义,
                ExpressionItems = expressionItems
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = expressionCondition;
            return result;
        }

        #endregion

        #region 应用对象信息

        /// <summary>
        /// 初始化应用对象信息
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <returns></returns>
        Result<List<TriggerServerViewModel>> InitTriggerServer(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<List<TriggerServerViewModel>>.FailedResult("执行计划信息为空");
            }
            if (trigger.ApplyTo != TaskTriggerApplyTo.服务)
            {
                return Result<List<TriggerServerViewModel>>.SuccessResult("初始化成功");
            }
            string[] serverNodeKeys = Request.Form.Keys.Where(c => c.StartsWith("Trigger_Server_") && !c.EndsWith("State")).ToArray();
            List<TriggerServerViewModel> serverList = new List<TriggerServerViewModel>();
            foreach (string serverKey in serverNodeKeys)
            {
                string serverId = Request.Form[serverKey];
                if (serverId.IsNullOrEmpty())
                {
                    continue;
                }
                int runState = 0;
                if (!int.TryParse(Request.Form[serverKey + "_State"], out runState) || !Enum.IsDefined(typeof(TaskTriggerServerRunStatus), runState))
                {
                    return Result<List<TriggerServerViewModel>>.FailedResult("请执行正确的运行状态");
                }
                serverList.Add(new TriggerServerViewModel()
                {
                    Server = new ServerNodeViewModel()
                    {
                        Id = serverId
                    },
                    RunStatus = (TaskTriggerServerRunStatus)runState
                });
            }
            if (serverList.IsNullOrEmpty())
            {
                return Result<List<TriggerServerViewModel>>.FailedResult("请至少设置一条服务信息");
            }
            var result = Result<List<TriggerServerViewModel>>.SuccessResult("初始化成功");
            result.Data = serverList;
            return result;
        }

        #endregion

        #region 任务&服务执行计划

        public ActionResult ServerScheduleTriggerList(string jobId, string serverCode)
        {
            ViewBag.JobId = jobId;
            ViewBag.ServerCode = serverCode;
            return View();
        }

        [HttpPost]
        public ActionResult SearchServerScheduleTrigger(string jobId, string serverCode)
        {
            ServerScheduleTriggerFilterDto filter = new ServerScheduleTriggerFilterDto()
            {
                Job = jobId,
                ServerFilter = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        serverCode
                    }
                }
            };
            List<TriggerViewModel> triggerList = triggerService.GetTriggerList(filter).Select(c => c.MapTo<TriggerViewModel>()).ToList();

            #region 调度信息

            List<TriggerServerViewModel> triggerServerList = null;
            if (!triggerList.IsNullOrEmpty())
            {
                triggerServerList = triggerServerService.GetTriggerServerList(new TriggerServerFilterDto()
                {
                    Triggers = triggerList.Select(c => c.Id).ToList(),
                    Servers = new List<string>()
                {
                    serverCode
                },

                }).Select(c => c.MapTo<TriggerServerViewModel>()).ToList();
                triggerServerList.ForEach(c =>
                {
                    c.Trigger = triggerList.FirstOrDefault(t => t.Id == c.Trigger?.Id);
                });
            }

            #endregion

            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(result);
        }

        public ActionResult TriggerHostServerList(string triggerId)
        {
            if (triggerId.IsNullOrEmpty())
            {
                return Content("获取信息失败");
            }
            var trigger = triggerService.GetTrigger(new TriggerFilterDto()
            {
                Ids = new List<string>()
                {
                    triggerId
                }
            }).MapTo<TriggerViewModel>();
            if (trigger == null)
            {
                return Content("获取信息失败");
            }
            return View(trigger);
        }

        [HttpPost]
        public ActionResult SearchTriggerHostServer(TriggerServerFilterViewModel filter)
        {
            IEnumerable<TriggerServerViewModel> triggerServerList = triggerServerService.GetTriggerServerList(filter.MapTo<TriggerServerFilterDto>()).Select(c => c.MapTo<TriggerServerViewModel>()).ToList();
            IEnumerable<string> serverIds = triggerServerList.Select(c => c.Server?.Id).Distinct();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds.ToList()
            };
            List<ServerNodeViewModel> serverList = serverNodeService.GetServerNodeList(serverFilter).Select(c => c.MapTo<ServerNodeViewModel>()).ToList();
            if (!serverList.IsNullOrEmpty())
            {
                foreach (var triggerServer in triggerServerList)
                {
                    triggerServer.Server = serverList.FirstOrDefault(c => c.Id == triggerServer.Server?.Id);
                }
            }
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult SearchTriggerHostServerByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            var triggerServerList = serverList.Select(c => new TriggerServerViewModel()
            {
                RunStatus = TaskTriggerServerRunStatus.运行,
                Server = c.MapTo<ServerNodeViewModel>()
            });
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(objResult);
        }

        #endregion

        #region 执行计划多选

        public ActionResult TriggerMultipleSelect(TriggerFilterViewModel filter)
        {
            ViewBag.TriggerFilter = filter;
            return View();
        }

        public ActionResult TriggerMultipleSelectSearch(TriggerFilterViewModel filter)
        {
            filter.Ids = filter.Ids?.Where(c => !c.IsNullOrEmpty()).ToList();
            IPaging<TriggerViewModel> triggerPager = triggerService.GetTriggerPaging(filter.MapTo<TriggerFilterDto>()).ConvertTo<TriggerViewModel>();
            object objResult = new
            {
                TotalCount = triggerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerPager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #endregion

        #region 计划服务

        [HttpPost]
        public ActionResult GetTriggerServers(TriggerServerFilterViewModel filter)
        {
            IPaging<TriggerServerViewModel> triggerServerPager = triggerServerService.GetTriggerServerPaging(filter.MapTo<TriggerServerFilterDto>()).ConvertTo<TriggerServerViewModel>();
            IEnumerable<string> serverIds = triggerServerPager.Select(c => c.Server?.Id).Distinct();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds.ToList()
            };
            List<ServerNodeViewModel> serverList = serverNodeService.GetServerNodeList(serverFilter).Select(c => c.MapTo<ServerNodeViewModel>()).ToList();
            if (!serverList.IsNullOrEmpty())
            {
                foreach (var triggerServer in triggerServerPager)
                {
                    triggerServer.Server = serverList.FirstOrDefault(c => c.Id == triggerServer.Server?.Id);
                }
            }
            object objResult = new
            {
                TotalCount = triggerServerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerServerPager.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult GetTriggerServersByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            var triggerServerList = serverList.Select(c => new TriggerServerViewModel()
            {
                RunStatus = TaskTriggerServerRunStatus.运行,
                Server = c.MapTo<ServerNodeViewModel>()
            });
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult DeleteTriggerServers(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            DeleteTriggerServerCmdDto deleteInfo = new DeleteTriggerServerCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            };
            return Json(triggerServerService.DeleteTriggerServer(deleteInfo));
        }

        [HttpPost]
        public ActionResult ModifyTriggerServerStatus(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定要操作的信息"));
            }
            ModifyTriggerServerRunStatusCmdDto stateInfo = new ModifyTriggerServerRunStatusCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            };
            return Json(triggerServerService.ModifyRunState(stateInfo));
        }

        [HttpPost]
        public ActionResult SaveTriggerServer(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定任何要保存的信息"));
            }
            return Json(triggerServerService.SaveTriggerServer(new SaveTriggerServerCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            }));
        }

        #endregion

        #region 执行日志

        [HttpPost]
        public ActionResult SearchJobExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            filter.LoadTrigger = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult SearchTriggerExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult SearchServerExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadTrigger = true;
            filter.LoadJob = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 错误日志

        [HttpPost]
        public ActionResult SearchJobErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult);
        }

        [HttpPost]
        public ActionResult SearchServerErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadJob = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult);
        }

        public ActionResult ErrorLogDetail(long id)
        {
            if (id <= 0)
            {
                return Content("获取信息失败");
            }
            ErrorLogFilterDto filter = new ErrorLogFilterDto()
            {
                Ids = new List<long>()
                {
                    id
                },
                LoadJob = true,
                LoadServer = true
            };
            ErrorLogViewModel log = errorLogService.GetErrorLog(filter).MapTo<ErrorLogViewModel>();
            if (log == null)
            {
                return Content("获取信息失败");
            }
            return View(log);
        }


        public ActionResult ErrorLogList()
        {
            return View();
        }

        public ActionResult SearchErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadJob = true;
            filter.LoadServer = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult);
        }

        #endregion

        #region 工作文件

        public ActionResult SearchJobFile(JobFileFilterViewModel filter)
        {
            IPaging<JobFileViewModel> jobFilePager = jobFileService.GetJobFilePaging(filter.MapTo<JobFileFilterDto>()).ConvertTo<JobFileViewModel>();
            object objResult = new
            {
                TotalCount = jobFilePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(jobFilePager.ToList())
            };
            return Json(objResult);
        }

        #endregion
    }
}