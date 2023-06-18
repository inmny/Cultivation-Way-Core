using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Constants
{
    public static class Others
    {
        public const string harmony_id = "harmony.cw.inmny";
        /// <summary>
        /// 对于意料之外的情况是否严格处理
        /// </summary>
        public const bool strict_mode = false;
        /// <summary>
        /// 功法锁定线(历史最大人数)
        /// </summary>
        public const int cultibook_lock_line = 10000;
    }
    public enum CultisysType
    {
        BODY,
        SOUL,
        OUTWARD
    }
}
