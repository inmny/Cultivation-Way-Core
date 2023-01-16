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
    /// <summary>
    /// 动画设置
    /// </summary>
    public class CW_AnimationSetting
    {
        internal bool possible_associated = true;
        /// <summary>
        /// 帧间隔
        /// </summary>
        public float frame_interval = 0.1f;
        /// <summary>
        /// 播放顺序：正/倒放
        /// </summary>
        public AnimationPlayDirection play_direction = AnimationPlayDirection.FORWARD;
        /// <summary>
        /// 终止帧
        /// </summary>
        public int stop_frame_idx = -1;
        /// <summary>
        /// 动画冻结帧
        /// </summary>
        public int anim_froze_frame_idx = -1;
        /// <summary>
        /// 循环限制
        /// </summary>
        public AnimationLoopLimitType loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;
        /// <summary>
        /// 循环播放时间限制
        /// </summary>
        public float loop_time_limit = -1;
        /// <summary>
        /// 播放次数限制
        /// </summary>
        public int loop_nr_limit = 1;
        /// <summary>
        /// 循环路径长度限制
        /// </summary>
        public float loop_trace_limit = -1;
        /// <summary>
        /// 循环播放类型：重新开始/首尾相接
        /// </summary>
        public AnimationLoopType loop_type = AnimationLoopType.REPEAT;
        /// <summary>
        /// 轨迹类型
        /// </summary>
        internal AnimationTraceType trace_type = AnimationTraceType.NONE;
        /// <summary>
        /// 按轨迹移动率
        /// </summary>
        public float trace_grad = Others.CW_Constants.default_anim_trace_grad;
        /// <summary>
        /// 轨迹函数
        /// </summary>
        internal CW_Delegates.CW_Animation_Trace_Update trace_updater = null;
        /// <summary>
        /// 各帧函数
        /// </summary>
        public CW_Delegates.CW_Animation_Frame_Action frame_action = null;
        /// <summary>
        /// 终止函数
        /// </summary>
        public CW_Delegates.CW_Animation_End_Action end_action = null;
        /// <summary>
        /// 图层
        /// </summary>
        public string layer_name = Others.CW_Constants.default_anim_layer_name;
        /// <summary>
        /// 始终旋转
        /// </summary>
        public bool always_roll = false;
        /// <summary>
        /// 每帧旋转角度
        /// </summary>
        public float roll_angle_per_frame = 10;
        /// <summary>
        /// 指向终点
        /// </summary>
        public bool point_to_dst = false;
        internal CW_AnimationSetting __deepcopy()
        {
            CW_AnimationSetting copy = new CW_AnimationSetting();
            copy.possible_associated = false;
            copy.loop_limit_type = loop_limit_type;
            copy.loop_time_limit = loop_time_limit;
            copy.loop_nr_limit = loop_nr_limit;
            copy.loop_type = loop_type;
            copy.frame_interval = frame_interval;
            copy.play_direction = play_direction;
            copy.loop_trace_limit = loop_trace_limit;
            copy.stop_frame_idx = stop_frame_idx;
            copy.trace_type = trace_type;
            copy.trace_updater = trace_updater;
            copy.frame_action = frame_action;
            copy.end_action = end_action;
            copy.trace_grad = trace_grad;
            copy.layer_name = layer_name;
            copy.always_roll = always_roll;
            copy.roll_angle_per_frame = roll_angle_per_frame;
            copy.point_to_dst = point_to_dst;
            copy.anim_froze_frame_idx = anim_froze_frame_idx;
            return copy;
        }
        public void set_trace(AnimationTraceType type)
        {
            this.trace_type = type;
            switch (type)
            {
                case AnimationTraceType.NONE:
                    {
                        this.trace_updater = null;
                        break;
                    }
                case AnimationTraceType.TRACK:
                    {
                        this.trace_updater = CW_TraceFunctions.trace_track;
                        break;
                    }
                case AnimationTraceType.LINE:
                    {
                        this.trace_updater = CW_TraceFunctions.trace_line;
                        break;
                    }
                case AnimationTraceType.PARABOLIC:
                    {
                        this.trace_updater = CW_TraceFunctions.trace_parabolic;
                        break;
                    }
                default:
                    this.trace_updater = null; break;
            }
        }
        public void set_trace(Others.CW_Delegates.CW_Animation_Trace_Update trace_updater)
        {
            this.trace_updater = trace_updater;
            this.trace_type = AnimationTraceType.CUSTOM;
        }
    }
}
