using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Others;
namespace Cultivation_Way.Animation
{
    public enum AnimationTraceType
    {
        NONE,
        TRACK,
        LINE,
        PARABOLIC,
        CUSTOM
    }
    public enum AnimationLoopLimitType
    {
        NUMBER_LIMIT,
        TIME_LIMIT,
        DST_LIMIT,
        TRACE_LIMIT,
        NO_LIMIT
    }
    public enum AnimationLoopType
    {
        REPEAT,
        ETOE
    }
    public class CW_AnimationSetting
    {
        internal bool is_default;
        /// <summary>
        /// 循环限制
        /// </summary>
        public AnimationLoopLimitType loop_limit_type;
        /// <summary>
        /// 循环播放时间/次数
        /// </summary>
        public float loop_time;
        /// <summary>
        /// 循环播放类型：从头开始/首尾相接
        /// </summary>
        public AnimationLoopType loop_type;
        /// <summary>
        /// 轨迹类型
        /// </summary>
        public AnimationTraceType trace_type;
        /// <summary>
        /// 轨迹函数
        /// </summary>
        public CW_Delegates.CW_Animation_Trace_Update trace_updater;
        // TODO: 路径行为，落地行为
        internal CW_AnimationSetting __deepcopy()
        {
            CW_AnimationSetting copy = new CW_AnimationSetting();

            copy.is_default = is_default;
            copy.loop_limit_type = loop_limit_type;
            copy.loop_time = loop_time;
            copy.loop_type = loop_type;
            return copy;
        }
    }
}
