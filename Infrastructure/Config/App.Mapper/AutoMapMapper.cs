using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.Configuration;
using EZNEW.Framework.ValueType;
using EZNEW.Framework.Extension;
using EZNEW.Framework.ObjectMap;      
using EZNEW.Application.Identity.User;
using EZNEW.ViewModel.Sys.Filter;
using EZNEW.ViewModel.Sys.Request;
using EZNEW.ViewModel.Sys.Response;
using EZNEW.Domain.Sys.Model;
using EZNEW.Domain.Sys.Service.Param;
using EZNEW.DTO.Sys.Cmd;
using EZNEW.DTO.Sys.Query;
using EZNEW.DTO.Sys.Query.Filter;
using EZNEW.Entity.Sys;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.ViewModel.CTask;
using EZNEW.ViewModel.CTask.Filter;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.CTask.Job;
using EZNEW.CTask.Job.LocalApp;
using EZNEW.CTask;
using EZNEW.CTask.Trigger;
using EZNEW.Framework.Serialize;
using EZNEW.CTask.Log;
using EZNEW.ViewModel.CTask.Response;

namespace App.Mapper
{
    public class AutoMapMapper : IObjectMap
    {
        /// <summary>
        /// 转换对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="sourceObj">源对象类型</param>
        /// <returns>目标对象类型</returns>
        public T MapTo<T>(object sourceObj)
        {
            return AutoMapper.Mapper.Map<T>(sourceObj);
        }

        /// <summary>
        /// /// <summary>
        /// 注册对象映射
        /// </summary>
        /// </summary>
        public void Register()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly || p.GetMethod.IsPrivate || p.GetMethod.IsFamilyAndAssembly || p.GetMethod.IsFamily || p.GetMethod.IsFamilyOrAssembly;

            #region Sys

            #region User

            cfg.CreateMap<UserEntity, User>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            cfg.CreateMap<User, UserEntity>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            cfg.CreateMap<User, AdminUser>();
            //admin user
            cfg.CreateMap<AdminUser, AdminUserDto>();
            cfg.CreateMap<AdminUserDto, AdminUserViewModel>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            cfg.CreateMap<AdminUserDto, EditAdminUserViewModel>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            cfg.CreateMap<EditAdminUserViewModel, AdminUserCmdDto>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            cfg.CreateMap<AdminUserCmdDto, AdminUser>();
            cfg.CreateMap<UserViewModel, AdminUserViewModel>();
            cfg.CreateMap<User, UserDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserDto user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUser)c).MapTo<AdminUserDto>();
                        break;
                }
                return user;
            });
            cfg.CreateMap<UserCmdDto, User>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                User user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserCmdDto)c).MapTo<AdminUser>();
                        break;
                }
                return user;
            });
            cfg.CreateMap<UserDto, UserViewModel>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserViewModel user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserDto)c).MapTo<AdminUserViewModel>();
                        break;
                }
                return user;
            });
            cfg.CreateMap<UserDto, EditAdminUserViewModel>().ConvertUsing(c=> 
            {
                if (c == null)
                {
                    return null;
                }
                EditAdminUserViewModel user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserDto)c).MapTo<EditAdminUserViewModel>();
                        break;
                }
                return user;
            });
            cfg.CreateMap<UserViewModel, UserCmdDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserCmdDto user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserViewModel)c).MapTo<AdminUserCmdDto>();
                        break;
                }
                return user;
            });
            cfg.CreateMap<UserFilterViewModel, UserFilterDto>();
            cfg.CreateMap<AdminUserFilterViewModel, AdminUserFilterDto>();
            cfg.CreateMap<ModifyPasswordViewModel, ModifyPasswordCmdDto>();
            cfg.CreateMap<ModifyPasswordCmdDto, ModifyUserPassword>();

            #endregion

            #region Role

            cfg.CreateMap<RoleEntity, Role>().ForMember(r => r.Parent, r => r.ResolveUsing(re => { return Role.CreateRole(re.Parent); }));
            cfg.CreateMap<Role, RoleEntity>().ForMember(re => re.Parent, r => r.MapFrom(rs => rs.Parent.SysNo));
            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<RoleCmdDto, Role>();
            cfg.CreateMap<RoleViewModel, RoleCmdDto>();
            cfg.CreateMap<RoleDto, RoleViewModel>();
            cfg.CreateMap<RoleFilterViewModel, RoleFilterDto>();
            cfg.CreateMap<ModifyRoleAuthorizeCmdDto, ModifyRoleAuthorize>()
                .ForMember(c => c.Binds, ce => ce.MapFrom(cs => cs.Binds.Select(cm => new Tuple<Role, Authority>(cm.Item1.MapTo<Role>(), cm.Item2.MapTo<Authority>()))))
                .ForMember(c => c.UnBinds, ce => ce.MapFrom(cs => cs.UnBinds.Select(cm => new Tuple<Role, Authority>(cm.Item1.MapTo<Role>(), cm.Item2.MapTo<Authority>()))));

            #endregion

            #region AuthorityGroup

            cfg.CreateMap<AuthorityGroup, AuthorityGroupEntity>().ForMember(r => r.Parent, re => re.MapFrom(rs => rs.Parent.SysNo));
            cfg.CreateMap<AuthorityGroupEntity, AuthorityGroup>().ForMember(re => re.Parent, r => r.ResolveUsing(re => { return AuthorityGroup.CreateAuthorityGroup(re.Parent); }));
            cfg.CreateMap<AuthorityGroup, AuthorityGroupDto>();
            cfg.CreateMap<AuthorityGroupCmdDto, AuthorityGroup>();
            cfg.CreateMap<AuthorityGroupDto, AuthorityGroupViewModel>();
            cfg.CreateMap<AuthorityGroupDto, EditAuthorityGroupViewModel>();
            cfg.CreateMap<EditAuthorityGroupViewModel, AuthorityGroupCmdDto>();
            cfg.CreateMap<EditAuthorityGroupViewModel, SaveAuthorityGroupCmdDto>().ForMember(a => a.AuthorityGroup, a => a.MapFrom(c => c));

            #endregion

            #region Authority

            cfg.CreateMap<Authority, AuthorityEntity>().ForMember(c => c.AuthGroup, re => re.MapFrom(rs => rs.AuthGroup.SysNo));
            cfg.CreateMap<AuthorityEntity, Authority>().ForMember(c => c.AuthGroup, re => re.ResolveUsing(rs => { return AuthorityGroup.CreateAuthorityGroup(rs.AuthGroup); }));
            cfg.CreateMap<Authority, AuthorityDto>();
            cfg.CreateMap<AuthorityCmdDto, Authority>();
            cfg.CreateMap<AuthorityDto, AuthorityViewModel>();
            cfg.CreateMap<AuthorityDto, EditAuthorityViewModel>();
            cfg.CreateMap<AuthorityViewModel, AuthorityCmdDto>();
            cfg.CreateMap<EditAuthorityViewModel, AuthorityCmdDto>();
            cfg.CreateMap<AuthorityFilterViewModel, AuthorityFilterDto>();
            cfg.CreateMap<AuthorityDto, AuthorizationAuthorityViewModel>();

            #endregion

            #region AuthorityOperationGroup

            cfg.CreateMap<AuthorityOperationGroup, AuthorityOperationGroupEntity>().ForMember(r => r.Parent, re => re.MapFrom(rs => rs.Parent.SysNo)).ForMember(r => r.Root, re => re.MapFrom(rs => rs.Root.SysNo));
            cfg.CreateMap<AuthorityOperationGroupEntity, AuthorityOperationGroup>().ForMember(re => re.Parent, r => r.ResolveUsing(re => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(re.Parent); })).ForMember(re => re.Root, r => r.ResolveUsing(re => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(re.Root); }));
            cfg.CreateMap<AuthorityOperationGroup, AuthorityOperationGroupDto>();
            cfg.CreateMap<AuthorityOperationGroupCmdDto, AuthorityOperationGroup>();
            cfg.CreateMap<AuthorityOperationGroupDto, AuthorityOperationGroupViewModel>();
            cfg.CreateMap<AuthorityOperationGroupDto, EditAuthorityOperationGroupViewModel>();
            cfg.CreateMap<EditAuthorityOperationGroupViewModel, AuthorityOperationGroupCmdDto>();

            #endregion

            #region AuthorityOperation

            cfg.CreateMap<AuthorityOperation, AuthorityOperationEntity>().ForMember(c => c.Group, re => re.MapFrom(rs => rs.Group.SysNo));
            cfg.CreateMap<AuthorityOperationEntity, AuthorityOperation>().ForMember(c => c.Group, re => re.ResolveUsing(rs => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(rs.Group); }));
            cfg.CreateMap<AuthorityOperation, AuthorityOperationDto>();
            cfg.CreateMap<AuthorityOperationCmdDto, AuthorityOperation>();
            cfg.CreateMap<AuthorityOperationDto, AuthorityOperationViewModel>();
            cfg.CreateMap<AuthorityOperationDto, EditAuthorityOperationViewModel>();
            cfg.CreateMap<EditAuthorityOperationViewModel, AuthorityOperationCmdDto>();
            cfg.CreateMap<AuthorityOperationFilterViewModel, AuthorityOperationFilterDto>();

            #endregion

            #region AuthorityBinOperation

            cfg.CreateMap<ModifyAuthorityBindAuthorityOperationCmdDto, ModifyAuthorityAndAuthorityOperationBind>()
                .ForMember(c => c.Binds, ce => ce.MapFrom(cs => cs.Binds.Select(cm => new Tuple<Authority, AuthorityOperation>(cm.Item1.MapTo<Authority>(), cm.Item2.MapTo<AuthorityOperation>()))))
                .ForMember(c => c.UnBinds, ce => ce.MapFrom(cs => cs.UnBinds.Select(cm => new Tuple<Authority, AuthorityOperation>(cm.Item1.MapTo<Authority>(), cm.Item2.MapTo<AuthorityOperation>()))));
            cfg.CreateMap<AuthorityBindOperationFilterViewModel, AuthorityBindOperationFilterDto>();
            cfg.CreateMap<AuthorityOperationBindAuthorityFilterViewModel, AuthorityOperationBindAuthorityFilterDto>();

            #endregion

            #region UserAuthorize

            cfg.CreateMap<UserAuthorizeCmdDto, UserAuthorize>();
            cfg.CreateMap<UserAuthorize, UserAuthorizeEntity>().ForMember(c => c.User, ce => ce.MapFrom(cs => cs.User.SysNo)).ForMember(c => c.Authority, ce => ce.MapFrom(cs => cs.Authority.Code));
            cfg.CreateMap<UserAuthorizeEntity, UserAuthorize>().ForMember(c => c.User, ce => ce.ResolveUsing(cs => { return User.CreateUser(cs.User); })).ForMember(c => c.Authority, ce => ce.ResolveUsing(cs => Authority.CreateAuthority(cs.Authority)));

            #endregion

            #region Authentication

            cfg.CreateMap<AuthenticationCmdDto, Authentication>();

            #endregion

            #endregion

            #region Task

            #region JobGroup

            cfg.CreateMap<JobGroup, JobGroupEntity>().ForMember(r => r.Parent, re => re.MapFrom(rs => rs.Parent.Code)).ForMember(r => r.Root, re => re.MapFrom(rs => rs.Root.Code));
            cfg.CreateMap<JobGroupEntity, JobGroup>().ForMember(re => re.Parent, r => r.ResolveUsing(re => { return JobGroup.CreateJobGroup(re.Parent); })).ForMember(re => re.Root, r => r.ResolveUsing(re => { return JobGroup.CreateJobGroup(re.Root); }));
            cfg.CreateMap<JobGroup, JobGroupDto>();
            cfg.CreateMap<JobGroupCmdDto, JobGroup>();
            cfg.CreateMap<JobGroupDto, JobGroupViewModel>();
            cfg.CreateMap<JobGroupViewModel, JobGroupCmdDto>();

            #endregion

            #region ServerNode

            cfg.CreateMap<ServerNode, ServerNodeEntity>();
            cfg.CreateMap<ServerNodeEntity, ServerNode>();
            cfg.CreateMap<ServerNode, ServerNodeDto>();
            cfg.CreateMap<ServerNodeCmdDto, ServerNode>();
            cfg.CreateMap<ServerNodeDto, ServerNodeViewModel>();
            cfg.CreateMap<ServerNodeViewModel, ServerNodeCmdDto>();
            cfg.CreateMap<ServerNodeFilterViewModel, ServerNodeFilterDto>();
            //cfg.CreateMap<ServerNode, TaskService>().ForMember(c => c.Remark, c => c.MapFrom(cs => cs.Description)).ForMember(c => c.Thread, c => c.ResolveUsing<ThreadConfig>(cs =>
            //{
            //    return new ThreadConfig()
            //    {
            //        ThreadCount = cs.ThreadCount,
            //        ThreadPriority = cs.ThreadPriority
            //    };
            //}));

            #endregion

            #region Job

            cfg.CreateMap<Job, JobEntity>().ForMember(r => r.Group, re => re.MapFrom(rs => rs.Group.Code));
            cfg.CreateMap<JobEntity, Job>().ForMember(r => r.Group, r => r.ResolveUsing(re => { return JobGroup.CreateJobGroup(re.Group); }));
            cfg.CreateMap<Job, JobDto>();
            cfg.CreateMap<JobCmdDto, Job>();
            cfg.CreateMap<JobDto, JobViewModel>();
            cfg.CreateMap<JobViewModel, JobCmdDto>();
            cfg.CreateMap<JobFilterViewModel, JobFilterDto>();
            //cfg.CreateMap<Job, TaskJob>().ConvertUsing(c =>
            //{
            //    if (c == null)
            //    {
            //        return null;
            //    }
            //    TaskJob job = new TaskJob()
            //    {
            //        Id = c.Id,
            //        Name = c.Name,
            //        AppType = c.Type,
            //        RunType = c.RunType,
            //        State = c.State,
            //        Group = new TaskJobGroup()
            //        {
            //            Id = c.Group?.Code,
            //            Name = c.Group?.Name
            //        }
            //    };
            //    switch (c.Type)
            //    {
            //        case JobApplicationType.本地应用:
            //            job.ApplicationJob = new ApplicationJob()
            //            {
            //                ApplicationPath = c.JobPath
            //            };
            //            break;
            //        case JobApplicationType.自定义任务:
            //            job.CustomerJob = new CustomerJob()
            //            {
            //                JobFilePath = c.JobFileName,
            //                JobTypeFullName = c.JobPath
            //            };
            //            break;
            //        case JobApplicationType.远程任务:
            //            job.RemoteJob = new RemoteJob()
            //            {
            //                RemoteUrl = c.JobPath
            //            };
            //            break;
            //    }
            //    return job;
            //});
            //cfg.CreateMap<JobDto, TaskJob>().ConvertUsing(c =>
            //{
            //    if (c == null)
            //    {
            //        return null;
            //    }
            //    TaskJob job = new TaskJob()
            //    {
            //        Id = c.Id,
            //        Name = c.Name,
            //        AppType = c.Type,
            //        RunType = c.RunType,
            //        State = c.State,
            //        Group = new TaskJobGroup()
            //        {
            //            Id = c.Group?.Code,
            //            Name = c.Group?.Name
            //        }
            //    };
            //    switch (c.Type)
            //    {
            //        case JobApplicationType.本地应用:
            //            job.ApplicationJob = new ApplicationJob()
            //            {
            //                ApplicationPath = c.JobPath
            //            };
            //            break;
            //        case JobApplicationType.自定义任务:
            //            job.CustomerJob = new CustomerJob()
            //            {
            //                JobFilePath = c.JobFileName,
            //                JobTypeFullName = c.JobPath
            //            };
            //            break;
            //        case JobApplicationType.远程任务:
            //            job.RemoteJob = new RemoteJob()
            //            {
            //                RemoteUrl = c.JobPath
            //            };
            //            break;
            //    }
            //    return job;
            //});

            #endregion

            #region JobServerHost

            cfg.CreateMap<JobServerHost, JobServerHostEntity>().ForMember(r => r.Server, re => re.MapFrom(rs => rs.Server.Id)).ForMember(r => r.Job, re => re.MapFrom(rs => rs.Job.Id));
            cfg.CreateMap<JobServerHostEntity, JobServerHost>().ForMember(r => r.Server, re => re.ResolveUsing(rs => { return ServerNode.CreateServerNode(rs.Server); })).ForMember(r => r.Job, re => re.ResolveUsing(rs => { return Job.CreateJob(rs.Job); }));
            cfg.CreateMap<JobServerHost, JobServerHostDto>();
            cfg.CreateMap<JobServerHostCmdDto, JobServerHost>();
            cfg.CreateMap<JobServerHostDto, JobServerHostViewModel>();
            cfg.CreateMap<JobServerHostViewModel, JobServerHostCmdDto>();
            cfg.CreateMap<JobServerHostFilterViewModel, JobServerHostFilterDto>();

            #endregion

            #region Trigger

            cfg.CreateMap<Trigger, TriggerEntity>().ForMember(c => c.Job, c => c.MapFrom(cs => cs.Job.Id)).ForMember(c => c.ConditionType, c => c.MapFrom(ce => ce.Condition == null ? 0 : (int)ce.Condition.Type));
            cfg.CreateMap<Trigger, SimpleTrigger>();
            cfg.CreateMap<Trigger, ExpressionTrigger>();
            cfg.CreateMap<TriggerEntity, Trigger>().ForMember(c => c.Job, c => c.ResolveUsing(ce => Job.CreateJob(ce.Job))).ForMember(c => c.Condition, c => c.ResolveUsing(ce => TriggerCondition.CreateTriggerCondition((TaskTriggerConditionType)ce.ConditionType, ce.Id)));
            cfg.CreateMap<TriggerFilterViewModel, TriggerFilterDto>();
            cfg.CreateMap<Trigger, TaskTrigger>().ConstructUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TaskTrigger taskTrigger = new TaskTrigger()
                {
                    Id = c.Id,
                    Job = c.Job?.MapTo<TaskJob>(),
                    Name = c.Name,
                    State = c.Status,
                    Type = c.Type,
                };
                switch (c.Type)
                {
                    case TaskTriggerType.简单:
                        var simpleTaskTrigger = ((SimpleTrigger)c).MapTo<TaskSimpleTrigger>();
                        taskTrigger.TriggerJsonData = JsonSerialize.ObjectToJson(simpleTaskTrigger);
                        break;
                    case TaskTriggerType.自定义:
                        var expressionTaskTrigger = ((ExpressionTrigger)c).MapTo<TaskExpressionTrigger>();
                        taskTrigger.TriggerJsonData = JsonSerialize.ObjectToJson(expressionTaskTrigger);
                        break;
                }
                return taskTrigger;
            });
            cfg.CreateMap<TriggerDto, TaskTrigger>().ConstructUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TaskTrigger taskTrigger = new TaskTrigger()
                {
                    Id = c.Id,
                    Job = c.Job?.MapTo<TaskJob>(),
                    Name = c.Name,
                    State = c.Status,
                    Type = c.Type,
                };
                switch (c.Type)
                {
                    case TaskTriggerType.简单:
                        var simpleTaskTrigger = ((SimpleTriggerDto)c).MapTo<TaskSimpleTrigger>();
                        taskTrigger.TriggerJsonData = JsonSerialize.ObjectToJson(simpleTaskTrigger);
                        break;
                    case TaskTriggerType.自定义:
                        var expressionTaskTrigger = ((ExpressionTriggerDto)c).MapTo<TaskExpressionTrigger>();
                        taskTrigger.TriggerJsonData = JsonSerialize.ObjectToJson(expressionTaskTrigger);
                        break;
                }
                return taskTrigger;
            });
            //simple trigger 
            cfg.CreateMap<TriggerSimpleEntity, SimpleTrigger>().ForMember(c => c.Id, ce => ce.MapFrom(cs => cs.TriggerId));
            cfg.CreateMap<Trigger, TriggerSimpleEntity>();
            cfg.CreateMap<SimpleTrigger, TriggerSimpleEntity>().ForMember(c => c.TriggerId, ce => ce.MapFrom(cs => cs.Id));
            cfg.CreateMap<TriggerSimpleEntity, Trigger>().ConvertUsing(c =>
            {
                return c.MapTo<SimpleTrigger>();
            });
            cfg.CreateMap<SimpleTrigger, SimpleTriggerDto>();
            cfg.CreateMap<SimpleTriggerCmdDto, SimpleTrigger>();
            cfg.CreateMap<SimpleTriggerDto, SimpleTriggerViewModel>();
            cfg.CreateMap<SimpleTriggerViewModel, SimpleTriggerCmdDto>();
            cfg.CreateMap<TriggerViewModel, SimpleTriggerViewModel>();
            cfg.CreateMap<SimpleTrigger, TaskSimpleTrigger>().ForMember(c => c.RepeatInterval, c => c.MapFrom(cs => TimeSpan.FromMilliseconds(cs.RepeatInterval)));
            cfg.CreateMap<SimpleTriggerDto, TaskSimpleTrigger>().ForMember(c => c.RepeatInterval, c => c.MapFrom(cs => TimeSpan.FromMilliseconds(cs.RepeatInterval)));

            //expression trigger
            cfg.CreateMap<ExpressionTrigger, ExpressionTriggerDto>();
            cfg.CreateMap<ExpressionTriggerCmdDto, ExpressionTrigger>();
            cfg.CreateMap<ExpressionTriggerDto, ExpressionTriggerViewModel>();
            cfg.CreateMap<ExpressionTriggerViewModel, ExpressionTriggerCmdDto>();
            cfg.CreateMap<TriggerViewModel, ExpressionTriggerViewModel>();
            cfg.CreateMap<ExpressionTrigger, TaskExpressionTrigger>();
            cfg.CreateMap<ExpressionTriggerDto, TaskExpressionTrigger>();

            //expression item
            cfg.CreateMap<ExpressionItem, TriggerExpressionEntity>().ForMember(c => c.ArrayValue, c => c.MapFrom(cs => string.Join(",", cs.ArrayValue)));
            cfg.CreateMap<TriggerExpressionEntity, ExpressionItem>().ForMember(c => c.ArrayValue, c => c.MapFrom(cs => cs.ArrayValue.LSplit(",")));
            cfg.CreateMap<ExpressionItem, ExpressionItemDto>();
            cfg.CreateMap<ExpressionItemCmdDto, ExpressionItem>();
            cfg.CreateMap<ExpressionItemDto, ExpressionItemViewModel>();
            cfg.CreateMap<ExpressionItemViewModel, ExpressionItemCmdDto>();
            cfg.CreateMap<ExpressionItem, TaskExpressionItem>();
            cfg.CreateMap<ExpressionItemDto, TaskExpressionItem>();
            cfg.CreateMap<Trigger, TriggerDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TriggerDto triggerDto = null;
                switch (c.Type)
                {
                    case TaskTriggerType.简单:
                        triggerDto = ((SimpleTrigger)c).MapTo<SimpleTriggerDto>();
                        break;
                    case TaskTriggerType.自定义:
                        triggerDto = ((ExpressionTrigger)c).MapTo<ExpressionTriggerDto>();
                        break;
                }
                return triggerDto;
            });
            cfg.CreateMap<TriggerCmdDto, Trigger>().ConvertUsing(t =>
            {
                if (t == null)
                {
                    return null;
                }
                Trigger trigger = null;
                switch (t.Type)
                {
                    case TaskTriggerType.简单:
                        trigger = ((SimpleTriggerCmdDto)t).MapTo<SimpleTrigger>();
                        break;
                    case TaskTriggerType.自定义:
                        trigger = ((ExpressionTriggerCmdDto)t).MapTo<ExpressionTrigger>();
                        break;
                }
                return trigger;
            });
            cfg.CreateMap<TriggerDto, TriggerViewModel>().ConvertUsing(t =>
            {
                if (t == null)
                {
                    return null;
                }
                TriggerViewModel trigger = null;
                switch (t.Type)
                {
                    case TaskTriggerType.简单:
                        trigger = ((SimpleTriggerDto)t).MapTo<SimpleTriggerViewModel>();
                        break;
                    case TaskTriggerType.自定义:
                        trigger = ((ExpressionTriggerDto)t).MapTo<ExpressionTriggerViewModel>();
                        break;
                }
                return trigger;
            });
            cfg.CreateMap<TriggerViewModel, TriggerCmdDto>().ConvertUsing(t =>
            {
                if (t == null)
                {
                    return null;
                }
                TriggerCmdDto trigger = null;
                switch (t.Type)
                {
                    case TaskTriggerType.简单:
                        trigger = ((SimpleTriggerViewModel)t).MapTo<SimpleTriggerCmdDto>();
                        break;
                    case TaskTriggerType.自定义:
                        trigger = ((ExpressionTriggerViewModel)t).MapTo<ExpressionTriggerCmdDto>();
                        break;
                    default:
                        var newTrigger = new SimpleTriggerViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Type = TaskTriggerType.简单,
                            StartTime = t.StartTime,
                            Description = t.Description,
                            ApplyTo = t.ApplyTo,
                            Condition = t.Condition,
                            EndTime = t.EndTime,
                            FireTotalCount = t.FireTotalCount,
                            Job = t.Job,
                            MisFireType = t.MisFireType,
                            NextFireTime = t.NextFireTime,
                            PrevFireTime = t.PrevFireTime,
                            Priority = t.Priority,
                            Status = t.Status
                        };
                        trigger = newTrigger.MapTo<SimpleTriggerCmdDto>();
                        break;
                }
                return trigger;
            });

            #endregion

            #region TriggerCondition

            //年计划
            cfg.CreateMap<TriggerAnnualConditionEntity, AnnualConditionDay>();
            cfg.CreateMap<AnnualConditionDay, TriggerAnnualConditionEntity>();

            cfg.CreateMap<TriggerAnnualCondition, TriggerAnnualConditionDto>();
            cfg.CreateMap<TriggerAnnualConditionDto, TriggerAnnualConditionViewModel>();
            cfg.CreateMap<TriggerAnnualConditionViewModel, TriggerAnnualConditionCmdDto>();
            cfg.CreateMap<TriggerAnnualConditionCmdDto, TriggerAnnualCondition>();

            cfg.CreateMap<AnnualConditionDay, AnnualConditionDayDto>();
            cfg.CreateMap<AnnualConditionDayDto, AnnualConditionDayViewModel>();
            cfg.CreateMap<AnnualConditionDayViewModel, AnnualConditionDayCmdDto>();
            cfg.CreateMap<AnnualConditionDayCmdDto, AnnualConditionDay>();

            //转化为任务对象
            cfg.CreateMap<TriggerAnnualConditionDto, TaskTriggerAnnualCondition>();
            cfg.CreateMap<TriggerAnnualCondition, TaskTriggerAnnualCondition>();
            cfg.CreateMap<AnnualConditionDayDto, TaskAnnualConditionDay>();
            cfg.CreateMap<AnnualConditionDay, TaskAnnualConditionDay>();

            //完整日期计划
            cfg.CreateMap<TriggerFullDateConditionEntity, FullDateConditionDate>();
            cfg.CreateMap<FullDateConditionDate, TriggerFullDateConditionEntity>();

            cfg.CreateMap<TriggerFullDateCondition, TriggerFullDateConditionDto>();
            cfg.CreateMap<TriggerFullDateConditionDto, TriggerFullDateConditionViewModel>();
            cfg.CreateMap<TriggerFullDateConditionViewModel, TriggerFullDateConditionCmdDto>();
            cfg.CreateMap<TriggerFullDateConditionCmdDto, TriggerFullDateCondition>();

            cfg.CreateMap<FullDateConditionDate, FullDateConditionDateDto>();
            cfg.CreateMap<FullDateConditionDateDto, FullDateConditionDateViewModel>();
            cfg.CreateMap<FullDateConditionDateViewModel, FullDateConditionDateCmdDto>();
            cfg.CreateMap<FullDateConditionDateCmdDto, FullDateConditionDate>();

            //转化为任务对象
            cfg.CreateMap<TriggerFullDateConditionDto, TaskTriggerFullDateCondition>();
            cfg.CreateMap<TriggerFullDateCondition, TaskTriggerFullDateCondition>();
            cfg.CreateMap<FullDateConditionDateDto, TaskFullDateConditionDate>();
            cfg.CreateMap<FullDateConditionDate, TaskFullDateConditionDate>();

            //月份日期
            cfg.CreateMap<TriggerMonthlyConditionEntity, MonthConditionDay>();
            cfg.CreateMap<MonthConditionDay, TriggerMonthlyConditionEntity>();

            cfg.CreateMap<TriggerMonthlyCondition, TriggerMonthlyConditionDto>();
            cfg.CreateMap<TriggerMonthlyConditionDto, TriggerMonthlyConditionViewModel>();
            cfg.CreateMap<TriggerMonthlyConditionViewModel, TriggerMonthlyConditionCmdDto>();
            cfg.CreateMap<TriggerMonthlyConditionCmdDto, TriggerMonthlyCondition>();

            cfg.CreateMap<MonthConditionDay, MonthConditionDayDto>();
            cfg.CreateMap<MonthConditionDayDto, MonthConditionDayViewModel>();
            cfg.CreateMap<MonthConditionDayViewModel, MonthConditionDayCmdDto>();
            cfg.CreateMap<MonthConditionDayCmdDto, MonthConditionDay>();

            //转化为任务对象
            cfg.CreateMap<TriggerMonthlyConditionDto, TaskTriggerMonthlyCondition>();
            cfg.CreateMap<TriggerMonthlyCondition, TaskTriggerMonthlyCondition>();
            cfg.CreateMap<MonthConditionDayDto, TaskMonthConditionDay>();
            cfg.CreateMap<MonthConditionDay, TaskMonthConditionDay>();

            //星期日期
            cfg.CreateMap<TriggerWeeklyConditionEntity, WeeklyConditionDay>();
            cfg.CreateMap<WeeklyConditionDay, TriggerWeeklyConditionEntity>();

            cfg.CreateMap<TriggerWeeklyCondition, TriggerWeeklyConditionDto>();
            cfg.CreateMap<TriggerWeeklyConditionDto, TriggerWeeklyConditionViewModel>();
            cfg.CreateMap<TriggerWeeklyConditionViewModel, TriggerWeeklyConditionCmdDto>();
            cfg.CreateMap<TriggerWeeklyConditionCmdDto, TriggerWeeklyCondition>();

            cfg.CreateMap<WeeklyConditionDay, WeeklyConditionDayDto>();
            cfg.CreateMap<WeeklyConditionDayDto, WeeklyConditionDayViewModel>();
            cfg.CreateMap<WeeklyConditionDayViewModel, WeeklyConditionDayCmdDto>();
            cfg.CreateMap<WeeklyConditionDayCmdDto, WeeklyConditionDay>();

            //转化为任务对象
            cfg.CreateMap<TriggerWeeklyConditionDto, TaskTriggerWeeklyCondition>();
            cfg.CreateMap<TriggerWeeklyCondition, TaskTriggerWeeklyCondition>();
            cfg.CreateMap<WeeklyConditionDayDto, TaskWeeklyConditionDay>();
            cfg.CreateMap<WeeklyConditionDay, TaskWeeklyConditionDay>();

            //表达式
            cfg.CreateMap<TriggerExpressionConditionEntity, ExpressionItem>().ForMember(c => c.ArrayValue, c => c.MapFrom(cs => cs.ArrayValue.LSplit(","))).ForMember(c => c.Option, c => c.MapFrom(cs => cs.ConditionOption));
            cfg.CreateMap<ExpressionItem, TriggerExpressionConditionEntity>().ForMember(c => c.ArrayValue, c => c.MapFrom(cs => string.Join(",", cs.ArrayValue))).ForMember(c => c.ConditionOption, c => c.MapFrom(cs => cs.Option));

            cfg.CreateMap<TriggerExpressionCondition, TriggerExpressionConditionDto>();
            cfg.CreateMap<TriggerExpressionConditionDto, TriggerExpressionConditionViewModel>();
            cfg.CreateMap<TriggerExpressionConditionViewModel, TriggerExpressionConditionCmdDto>();
            cfg.CreateMap<TriggerExpressionConditionCmdDto, TriggerExpressionCondition>();

            //转化为任务对象
            cfg.CreateMap<TriggerExpressionConditionDto, TaskTriggerExpressionCondition>();
            cfg.CreateMap<TriggerExpressionCondition, TaskTriggerExpressionCondition>();

            //时间段
            cfg.CreateMap<TriggerDailyConditionEntity, TriggerCondition>();
            cfg.CreateMap<TriggerCondition, TriggerDailyConditionEntity>();
            cfg.CreateMap<TriggerDailyConditionEntity, TriggerDailyCondition>();
            cfg.CreateMap<TriggerDailyCondition, TriggerDailyConditionEntity>();

            cfg.CreateMap<TriggerDailyCondition, TriggerDailyConditionDto>();
            cfg.CreateMap<TriggerDailyConditionDto, TriggerDailyConditionViewModel>();
            cfg.CreateMap<TriggerDailyConditionViewModel, TriggerDailyConditionCmdDto>();
            cfg.CreateMap<TriggerDailyConditionCmdDto, TriggerDailyCondition>();

            //转化为任务对象
            cfg.CreateMap<TriggerDailyConditionDto, TaskTriggerDailyCondition>();
            cfg.CreateMap<TriggerDailyCondition, TaskTriggerDailyCondition>();
            cfg.CreateMap<TriggerDailyConditionDto, TaskTriggerDailyCondition>();
            cfg.CreateMap<TriggerDailyCondition, TaskTriggerDailyCondition>();

            cfg.CreateMap<TriggerCondition, TriggerConditionDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TriggerConditionDto triggerConditionDto = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        triggerConditionDto = ((TriggerFullDateCondition)c).MapTo<TriggerFullDateConditionDto>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        triggerConditionDto = ((TriggerWeeklyCondition)c).MapTo<TriggerWeeklyConditionDto>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        triggerConditionDto = ((TriggerDailyCondition)c).MapTo<TriggerDailyConditionDto>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        triggerConditionDto = ((TriggerAnnualCondition)c).MapTo<TriggerAnnualConditionDto>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        triggerConditionDto = ((TriggerMonthlyCondition)c).MapTo<TriggerMonthlyConditionDto>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        triggerConditionDto = ((TriggerExpressionCondition)c).MapTo<TriggerExpressionConditionDto>();
                        break;
                }
                return triggerConditionDto;
            });

            cfg.CreateMap<TriggerConditionDto, TriggerConditionViewModel>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TriggerConditionViewModel triggerConditionViewModel = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        triggerConditionViewModel = ((TriggerFullDateConditionDto)c).MapTo<TriggerFullDateConditionViewModel>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        triggerConditionViewModel = ((TriggerWeeklyConditionDto)c).MapTo<TriggerWeeklyConditionViewModel>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        triggerConditionViewModel = ((TriggerDailyConditionDto)c).MapTo<TriggerDailyConditionViewModel>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        triggerConditionViewModel = ((TriggerAnnualConditionDto)c).MapTo<TriggerAnnualConditionViewModel>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        triggerConditionViewModel = ((TriggerMonthlyConditionDto)c).MapTo<TriggerMonthlyConditionViewModel>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        triggerConditionViewModel = ((TriggerExpressionConditionDto)c).MapTo<TriggerExpressionConditionViewModel>();
                        break;
                }
                return triggerConditionViewModel;
            });

            cfg.CreateMap<TriggerConditionViewModel, TriggerConditionCmdDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TriggerConditionCmdDto triggerConditionCmdDto = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        triggerConditionCmdDto = ((TriggerFullDateConditionViewModel)c).MapTo<TriggerFullDateConditionCmdDto>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        triggerConditionCmdDto = ((TriggerWeeklyConditionViewModel)c).MapTo<TriggerWeeklyConditionCmdDto>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        triggerConditionCmdDto = ((TriggerDailyConditionViewModel)c).MapTo<TriggerDailyConditionCmdDto>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        triggerConditionCmdDto = ((TriggerAnnualConditionViewModel)c).MapTo<TriggerAnnualConditionCmdDto>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        triggerConditionCmdDto = ((TriggerMonthlyConditionViewModel)c).MapTo<TriggerMonthlyConditionCmdDto>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        triggerConditionCmdDto = ((TriggerExpressionConditionViewModel)c).MapTo<TriggerExpressionConditionCmdDto>();
                        break;
                }
                return triggerConditionCmdDto;
            });

            cfg.CreateMap<TriggerConditionCmdDto, TriggerCondition>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TriggerCondition triggerCondition = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        triggerCondition = ((TriggerFullDateConditionCmdDto)c).MapTo<TriggerFullDateCondition>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        triggerCondition = ((TriggerWeeklyConditionCmdDto)c).MapTo<TriggerWeeklyCondition>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        triggerCondition = ((TriggerDailyConditionCmdDto)c).MapTo<TriggerDailyCondition>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        triggerCondition = ((TriggerAnnualConditionCmdDto)c).MapTo<TriggerAnnualCondition>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        triggerCondition = ((TriggerMonthlyConditionCmdDto)c).MapTo<TriggerMonthlyCondition>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        triggerCondition = ((TriggerExpressionConditionCmdDto)c).MapTo<TriggerExpressionCondition>();
                        break;
                }
                return triggerCondition;
            });
            cfg.CreateMap<TriggerConditionDto, TaskTriggerCondition>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TaskTriggerCondition taskCondition = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        taskCondition = ((TriggerFullDateConditionDto)c).MapTo<TaskTriggerFullDateCondition>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        taskCondition = ((TriggerWeeklyConditionDto)c).MapTo<TaskTriggerWeeklyCondition>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        taskCondition = ((TriggerDailyConditionDto)c).MapTo<TaskTriggerDailyCondition>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        taskCondition = ((TriggerAnnualConditionDto)c).MapTo<TaskTriggerAnnualCondition>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        taskCondition = ((TriggerMonthlyConditionDto)c).MapTo<TaskTriggerMonthlyCondition>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        taskCondition = ((TriggerExpressionConditionDto)c).MapTo<TaskTriggerExpressionCondition>();
                        break;
                }
                return taskCondition;
            });
            cfg.CreateMap<TriggerCondition, TaskTriggerCondition>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                TaskTriggerCondition taskCondition = null;
                switch (c.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        taskCondition = ((TriggerFullDateCondition)c).MapTo<TaskTriggerFullDateCondition>();
                        break;
                    case TaskTriggerConditionType.星期配置:
                        taskCondition = ((TriggerWeeklyCondition)c).MapTo<TaskTriggerWeeklyCondition>();
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        taskCondition = ((TriggerDailyCondition)c).MapTo<TaskTriggerDailyCondition>();
                        break;
                    case TaskTriggerConditionType.每年日期:
                        taskCondition = ((TriggerAnnualCondition)c).MapTo<TaskTriggerAnnualCondition>();
                        break;
                    case TaskTriggerConditionType.每月日期:
                        taskCondition = ((TriggerMonthlyCondition)c).MapTo<TaskTriggerMonthlyCondition>();
                        break;
                    case TaskTriggerConditionType.自定义:
                        taskCondition = ((TriggerExpressionCondition)c).MapTo<TaskTriggerExpressionCondition>();
                        break;
                }
                return taskCondition;
            });

            #endregion

            #region TriggerServer

            cfg.CreateMap<TriggerServer, TriggerServerEntity>().ForMember(c => c.Trigger, c => c.MapFrom(cs => cs.Trigger.Id)).ForMember(c => c.Server, c => c.MapFrom(cs => cs.Server.Id));
            cfg.CreateMap<TriggerServerEntity, TriggerServer>().ForMember(c => c.Trigger, c => c.ResolveUsing(cs => Trigger.CreateTrigger(cs.Trigger))).ForMember(c => c.Server, c => c.ResolveUsing(cs => ServerNode.CreateServerNode(cs.Server)));
            cfg.CreateMap<TriggerServer, TriggerServerDto>();
            cfg.CreateMap<TriggerServerCmdDto, TriggerServer>();
            cfg.CreateMap<TriggerServerDto, TriggerServerViewModel>();
            cfg.CreateMap<TriggerServerViewModel, TriggerServerCmdDto>();
            cfg.CreateMap<TriggerServerFilterViewModel, TriggerServerFilterDto>();

            #endregion

            #region ExecuteLog

            cfg.CreateMap<ExecuteLog, ExecuteLogEntity>().ForMember(c => c.Server, ce => ce.MapFrom(cs => cs.Server.Id)).ForMember(c => c.Trigger, ce => ce.MapFrom(cs => cs.Trigger.Id)).ForMember(c => c.Job, ce => ce.MapFrom(cs => cs.Job.Id));
            cfg.CreateMap<ExecuteLogEntity, ExecuteLog>().ForMember(c => c.Server, ce => ce.ResolveUsing(cs => { return ServerNode.CreateServerNode(cs.Server); })).ForMember(c => c.Job, ce => ce.ResolveUsing(cs => { return Job.CreateJob(cs.Job); })).ForMember(c => c.Trigger, ce => ce.ResolveUsing(cs => { return Trigger.CreateTrigger(cs.Trigger); }));
            cfg.CreateMap<ExecuteLog, ExecuteLogDto>();
            cfg.CreateMap<ExecuteLogCmdDto, ExecuteLog>();
            cfg.CreateMap<ExecuteLogDto, ExecuteLogViewModel>();
            cfg.CreateMap<ExecuteLogViewModel, ExecuteLogCmdDto>();
            cfg.CreateMap<ExecuteLogFilterViewModel, ExecuteLogFilterDto>();

            //任务对象转换
            cfg.CreateMap<TaskJobExecuteLog, ExecuteLogCmdDto>()
                .ForMember(c => c.Job, ce => ce.ResolveUsing(cs => { return new JobCmdDto() { Id = cs.Job?.Id }; }))
                .ForMember(c => c.Trigger, ce => ce.ResolveUsing(cs => { return new TriggerCmdDto() { Id = cs.Trigger?.Id }; }));

            #endregion

            #region ErrorLog

            cfg.CreateMap<ErrorLog, ErrorLogEntity>().ForMember(c => c.Server, ce => ce.MapFrom(cs => cs.Server.Id)).ForMember(c => c.Trigger, ce => ce.MapFrom(cs => cs.Trigger.Id)).ForMember(c => c.Job, ce => ce.MapFrom(cs => cs.Job.Id)); ;
            cfg.CreateMap<ErrorLogEntity, ErrorLog>().ForMember(c => c.Server, ce => ce.ResolveUsing(cs => { return ServerNode.CreateServerNode(cs.Server); })).ForMember(c => c.Job, ce => ce.ResolveUsing(cs => { return Job.CreateJob(cs.Job); })).ForMember(c => c.Trigger, ce => ce.ResolveUsing(cs => { return Trigger.CreateTrigger(cs.Trigger); }));
            cfg.CreateMap<ErrorLog, ErrorLogDto>();
            cfg.CreateMap<ErrorLogCmdDto, ErrorLog>();
            cfg.CreateMap<ErrorLogDto, ErrorLogViewModel>();
            cfg.CreateMap<ErrorLogViewModel, ErrorLogCmdDto>();
            cfg.CreateMap<ErrorLogFilterViewModel, ErrorLogFilterDto>();

            #endregion

            #region JobFile

            cfg.CreateMap<JobFile, JobFileEntity>();
            cfg.CreateMap<JobFileEntity, JobFile>();
            cfg.CreateMap<JobFile, JobFileDto>();
            cfg.CreateMap<JobFileCmdDto, JobFile>();
            cfg.CreateMap<JobFileDto, JobFileViewModel>();
            cfg.CreateMap<JobFileViewModel, JobFileCmdDto>();

            #endregion

            #endregion

            AutoMapper.Mapper.Initialize(cfg);
        }
    }
}
