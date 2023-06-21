using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Animation
{
    public enum AnimationTraceType
    {
        NONE,
        TRACK,
        LINE,
        PARABOLIC,
        ATTACH,
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
    public class AnimationSetting
    {
        /// <summary>
        /// 略缩地图隐藏动画
        /// </summary>
        public bool visible_in_low_res = false;
        /// <summary>
        /// 终点停滞
        /// </summary>
        public float froze_time_after_end = -1f;
        /// <summary>
        /// 自由变量
        /// </summary>
        public float free_val = 0;
        /// <summary>
        /// 可能被引用
        /// </summary>
        internal bool possible_referenced = true;
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
        public float trace_grad = Constants.Others.default_anim_trace_grad;
        /// <summary>
        /// 轨迹函数
        /// </summary>
        internal Others.AnimTraceUpdate trace_updater = null;
        /// <summary>
        /// 各帧函数
        /// </summary>
        public Others.AnimFrameAction frame_action = null;
        /// <summary>
        /// 终止函数
        /// </summary>
        public Others.AnimEndAction end_action = null;
        /// <summary>
        /// 图层，从底至顶顺序依次为Default, Tiles, MapLayer, EffectsBack, Objects,         EffectsTop, MapOverlay, Debug
        /// </summary>
        public string layer_name = Constants.Others.default_anim_layer_name;
        /// <summary>
        /// 始终旋转
        /// </summary>
        public bool always_roll = false;
        /// <summary>
        /// 始终旋转的旋转轴
        /// </summary>
        public Vector3 always_roll_axis = Vector3.right;
        /// <summary>
        /// 每帧旋转角度
        /// </summary>
        public float roll_angle_per_frame = 10;
        /// <summary>
        /// 指向终点
        /// </summary>
        public bool point_to_dst = false;
        /// <summary>
        /// 始终指向终点
        /// </summary>
        public bool always_point_to_dst = false;
        internal AnimationSetting __deepcopy()
        {
            AnimationSetting copy = new();
            copy.visible_in_low_res = visible_in_low_res;
            copy.froze_time_after_end = froze_time_after_end;
            copy.free_val = free_val;
            copy.possible_referenced = false;
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
            copy.always_roll_axis = always_roll_axis;
            copy.always_point_to_dst = always_point_to_dst;
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
                        this.trace_updater = AnimFunctions.trace_track;
                        break;
                    }
                case AnimationTraceType.LINE:
                    {
                        this.trace_updater = AnimFunctions.trace_line;
                        break;
                    }
                case AnimationTraceType.PARABOLIC:
                    {
                        this.trace_updater = AnimFunctions.trace_parabolic;
                        break;
                    }
                default:
                    this.trace_updater = null; break;
            }
        }
        public void set_trace(Others.AnimTraceUpdate trace_updater)
        {
            this.trace_updater = trace_updater;
            this.trace_type = AnimationTraceType.CUSTOM;
        }
    }
}
