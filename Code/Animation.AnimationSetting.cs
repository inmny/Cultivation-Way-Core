using Cultivation_Way.General.AboutAnim;
using Cultivation_Way.Others;
using UnityEngine;

namespace Cultivation_Way.Animation;

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
///     动画设置
/// </summary>
public class AnimationSetting
{
    /// <summary>
    ///     始终指向终点
    /// </summary>
    public bool always_point_to_dst;

    /// <summary>
    ///     始终旋转
    /// </summary>
    public bool always_roll;

    /// <summary>
    ///     始终旋转的旋转轴
    /// </summary>
    public Vector3 always_roll_axis = Vector3.right;

    /// <summary>
    ///     动画冻结帧
    /// </summary>
    public int anim_froze_frame_idx = -1;

    /// <summary>
    ///     终止函数
    /// </summary>
    public AnimEndAction end_action;

    /// <summary>
    ///     各帧函数
    /// </summary>
    public AnimFrameAction frame_action;

    /// <summary>
    ///     帧间隔
    /// </summary>
    public float frame_interval = 0.1f;

    /// <summary>
    ///     自由变量
    /// </summary>
    public float free_val;

    /// <summary>
    ///     终点停滞
    /// </summary>
    public float froze_time_after_end = -1f;

    /// <summary>
    ///     图层，从底至顶顺序依次为Default, Tiles, MapLayer, EffectsBack, Objects,         EffectsTop, MapOverlay, Debug
    /// </summary>
    public string layer_name = Constants.Others.default_anim_layer_name;

    /// <summary>
    ///     循环限制
    /// </summary>
    public AnimationLoopLimitType loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;

    /// <summary>
    ///     播放次数限制
    /// </summary>
    public int loop_nr_limit = 1;

    /// <summary>
    ///     循环播放时间限制
    /// </summary>
    public float loop_time_limit = -1;

    /// <summary>
    ///     循环路径长度限制
    /// </summary>
    public float loop_trace_limit = -1;

    /// <summary>
    ///     循环播放类型：重新开始/首尾相接
    /// </summary>
    public AnimationLoopType loop_type = AnimationLoopType.REPEAT;

    /// <summary>
    ///     播放顺序：正/倒放
    /// </summary>
    public AnimationPlayDirection play_direction = AnimationPlayDirection.FORWARD;

    /// <summary>
    ///     指向终点
    /// </summary>
    public bool point_to_dst;

    /// <summary>
    ///     可能被引用
    /// </summary>
    internal bool possible_referenced = true;

    /// <summary>
    ///     每帧旋转角度
    /// </summary>
    public float roll_angle_per_frame = 10;

    /// <summary>
    ///     终止帧
    /// </summary>
    public int stop_frame_idx = -1;

    /// <summary>
    ///     按轨迹移动率
    /// </summary>
    public float trace_grad = Constants.Others.default_anim_trace_grad;

    /// <summary>
    ///     轨迹类型
    /// </summary>
    internal AnimationTraceType trace_type = AnimationTraceType.NONE;

    /// <summary>
    ///     轨迹函数
    /// </summary>
    internal AnimTraceUpdate trace_updater;

    /// <summary>
    ///     略缩地图隐藏动画
    /// </summary>
    public bool visible_in_low_res;

    internal AnimationSetting __deepcopy()
    {
        AnimationSetting copy = new()
        {
            visible_in_low_res = visible_in_low_res,
            froze_time_after_end = froze_time_after_end,
            free_val = free_val,
            possible_referenced = false,
            loop_limit_type = loop_limit_type,
            loop_time_limit = loop_time_limit,
            loop_nr_limit = loop_nr_limit,
            loop_type = loop_type,
            frame_interval = frame_interval,
            play_direction = play_direction,
            loop_trace_limit = loop_trace_limit,
            stop_frame_idx = stop_frame_idx,
            trace_type = trace_type,
            trace_updater = trace_updater,
            frame_action = frame_action,
            end_action = end_action,
            trace_grad = trace_grad,
            layer_name = layer_name,
            always_roll = always_roll,
            always_roll_axis = always_roll_axis,
            always_point_to_dst = always_point_to_dst,
            roll_angle_per_frame = roll_angle_per_frame,
            point_to_dst = point_to_dst,
            anim_froze_frame_idx = anim_froze_frame_idx
        };
        return copy;
    }

    public void set_trace(AnimationTraceType type)
    {
        trace_type = type;
        switch (type)
        {
            case AnimationTraceType.NONE:
            {
                trace_updater = null;
                break;
            }
            case AnimationTraceType.TRACK:
            {
                trace_updater = TraceFunctions.trace_track;
                break;
            }
            case AnimationTraceType.LINE:
            {
                trace_updater = TraceFunctions.trace_line;
                break;
            }
            case AnimationTraceType.PARABOLIC:
            {
                trace_updater = TraceFunctions.trace_parabolic;
                break;
            }
            default:
                trace_updater = null;
                break;
        }
    }

    public void set_trace(AnimTraceUpdate trace_updater)
    {
        this.trace_updater = trace_updater;
        trace_type = AnimationTraceType.CUSTOM;
    }
}