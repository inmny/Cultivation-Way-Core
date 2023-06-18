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
        /// <summary>
        /// 动画对象回收间隔
        /// </summary>
        public const float anim_recycle_interval = 60f;
        /// <summary>
        /// 默认的动画移动速度
        /// </summary>
        public const float default_anim_trace_grad = 2f;
        /// <summary>
        /// 动画最大持续时间
        /// </summary>
        public const float max_anim_time = 600f;
        /// <summary>
        /// 动画最大移动距离
        /// </summary>
        public const float max_anim_trace_length = 1000f;
        /// <summary>
        /// 判定动画移动结束的误差
        /// </summary>
        public const float anim_dst_error = 0.5f;
        /// <summary>
        /// 默认的动画图层, 仅高于人物
        /// </summary>
        public const string default_anim_layer_name = "EffectsTop";
    }
    public enum CultisysType
    {
        BODY,
        SOUL,
        OUTWARD
    }
}
