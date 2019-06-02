using System;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.Framework;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Framework.Extension;
using EZNEW.CTask;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EZNEW.Domain.CTask.Model
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    public class SimpleTrigger : Trigger
    {
        #region	字段

        /// <summary>
        /// 重复次数
        /// </summary>
        protected int repeatCount;

        /// <summary>
        /// 重复间隔
        /// </summary>
        protected long repeatInterval;

        /// <summary>
        /// 一直重复执行
        /// </summary>
        protected bool repeatForever;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化简单类型执行计划对象
        /// </summary>
        /// <param name="triggerId">编号</param>
        internal SimpleTrigger(string triggerId = ""):base(triggerId)
        {
            type = TaskTriggerType.简单;
        }

        #endregion

        #region	属性

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            get
            {
                return repeatCount;
            }
            set
            {
                repeatCount = value;
            }
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        public long RepeatInterval
        {
            get
            {
                return repeatInterval;
            }
            set
            {
                repeatInterval = value;
            }
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        public bool RepeatForever
        {
            get
            {
                return repeatForever;
            }
            set
            {
                repeatForever = value;
            }
        }

        #endregion

        #region 方法

        #region 根据给定的计划对象更新当前信息

        /// <summary>
        /// 根据给定的计划对象更新当前信息
        /// </summary>
        /// <param name="trigger">其它计划信息</param>
        public override void ModifyFromOtherTrigger(Trigger trigger,IEnumerable<string> excludePropertys= null)
        {
            base.ModifyFromOtherTrigger(trigger, excludePropertys);
            if (trigger == null||!(trigger is SimpleTrigger))
            {
                return;
            }
            var simpleTrigger = trigger as SimpleTrigger;
            repeatCount = simpleTrigger.RepeatCount;
            repeatForever = simpleTrigger.repeatForever;
            repeatInterval = simpleTrigger.RepeatInterval;
        } 

        #endregion

        #endregion
    }
}