using EZNEW.Application.CTask;
using EZNEW.Application.Identity;
using EZNEW.Framework.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site.Cms.Config
{
    /// <summary>
    /// 唯一标识符配置
    /// </summary>
    public static class IdentityKeyConfig
    {
        public static void Init()
        {
            List<string> groupCodes = new List<string>();

            #region Identity

            Array values = Enum.GetValues(IdentityGroup.授权操作.GetType());
            foreach (IdentityGroup group in values)
            {
                groupCodes.Add(IdentityApplicationHelper.GetIdGroupCode(group));
            }

            #endregion

            #region Task

            Array taskValues = Enum.GetValues(TaskIdGroup.任务分组.GetType());
            foreach (TaskIdGroup group in taskValues)
            {
                groupCodes.Add(CTaskApplicationHelper.GetIdGroupCode(group));
            }

            #endregion

            SerialNumber.RegisterGenerator(groupCodes, 1, 1);
        }
    }
}