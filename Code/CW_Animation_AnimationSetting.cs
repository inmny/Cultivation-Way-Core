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
    public enum AnimationPlayDirection
    {
        FORWARD,
        BACKWARD
    }
    public class CW_AnimationSetting
    {
        internal bool is_default;
        /// <summary>
        /// 帧间隔
        /// </summary>
        public float frame_interval;
        /// <summary>
        /// 播放顺序：正/倒放
        /// </summary>
        public AnimationPlayDirection play_direction;
        /// <summary>
        /// 终止帧
        /// </summary>
        public int stop_frame_idx;
        /// <summary>
        /// 循环限制
        /// </summary>
        public AnimationLoopLimitType loop_limit_type;
        /// <summary>
        /// 循环播放时间限制
        /// </summary>
        public float loop_time_limit;
        /// <summary>
        /// 循环次数限制
        /// </summary>
        public int loop_nr_limit;
        /// <summary>
        /// 循环路径长度限制
        /// </summary>
        public float loop_trace_limit;
        /// <summary>
        /// 循环播放类型：重新开始/首尾相接
        /// </summary>
        public AnimationLoopType loop_type;
        /// <summary>
        /// 轨迹类型
        /// </summary>
        public AnimationTraceType trace_type;
        /// <summary>
        /// 按轨迹移动率
        /// </summary>
        public float trace_grad;
        /// <summary>
        /// 轨迹函数
        /// </summary>
        public CW_Delegates.CW_Animation_Trace_Update trace_updater;
        // TODO: 落地行为，各帧行为
        public CW_Delegates.CW_Animation_Frame_Action frame_action;
        public CW_Delegates.CW_Animation_End_Action end_action;
        internal CW_AnimationSetting __deepcopy()
        {
            CW_AnimationSetting copy = new CW_AnimationSetting();

            copy.is_default = is_default;
            copy.loop_limit_type = loop_limit_type;
            copy.loop_time_limit = loop_time_limit;
            copy.loop_type = loop_type;
            copy.frame_interval = frame_interval;
            copy.play_direction = play_direction;
            copy.loop_trace_limit = loop_trace_limit;
            copy.stop_frame_idx = stop_frame_idx;
            copy.trace_type = trace_type;
            copy.trace_updater = trace_updater;
            return copy;
        }
    }
}
