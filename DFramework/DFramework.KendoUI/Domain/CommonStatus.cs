using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DFramework.KendoUI.Domain
{
    public enum CommonStatus
    {
        [Description("所有")] All = 0,

        /// <summary>
        ///     正常
        /// </summary>
        [Description("可用")] Normal = 1,

        /// <summary>
        ///     禁用
        /// </summary>
        [Description("不可用")] Disabled = 2,

        /// <summary>
        ///     删除
        /// </summary>
        [Description("已删除")] Deleted = 3
    }
}