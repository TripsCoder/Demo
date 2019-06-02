﻿using EZNEW.Develop.DataAccess;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DataAccess.CTask
{
    /// <summary>
    /// 自定义表达式计划数据访问
    /// </summary>
    public class TriggerExpressionDataAccess : RdbDataAccess<TriggerExpressionEntity>, ITriggerExpressionDbAccess
    {
    }
}
