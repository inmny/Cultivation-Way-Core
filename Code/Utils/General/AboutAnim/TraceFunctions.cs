using System;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.General.AboutAnim;

/// <summary>
///     一般动画的trace_updater
/// </summary>
public static class TraceFunctions
{
    [Hotfixable]
    public static void free_fall(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
        ref float delta_x, ref float delta_y)
    {
        delta_y = -4.9f * anim.play_time * anim.play_time * anim.setting.trace_grad;
    }

    /// <summary>
    ///     按src_vec->dst_vec方向直线移动
    /// </summary>
    public static void trace_line(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
        ref float delta_x, ref float delta_y)
    {
        float dist = Toolbox.DistVec2Float(src_vec, dst_vec);

        delta_x = (dst_vec.x - src_vec.x) * anim.setting.trace_grad / dist;
        delta_y = (dst_vec.y - src_vec.y) * anim.setting.trace_grad / dist;
    }

    /// <summary>
    ///     按src_vec->dst_vec方向移动, 当trace_grad合理时收敛至dst_vec
    /// </summary>
    public static void trace_track(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
        ref float delta_x, ref float delta_y)
    {
        float dist = Toolbox.DistVec2Float(src_vec, dst_vec);

        if (dist < Constants.Others.anim_dst_error)
        {
            delta_x = dst_vec.x - src_vec.x;
            delta_y = dst_vec.y - src_vec.y;
            return;
        }

        delta_x = (dst_vec.x - src_vec.x) * anim.setting.trace_grad / dist;
        delta_y = (dst_vec.y - src_vec.y) * anim.setting.trace_grad / dist;
    }

    /// <summary>
    ///     以src_vec和dst_vec为两根的抛物线, 未实现
    /// </summary>
    internal static void trace_parabolic(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
        ref float delta_x, ref float delta_y)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     逐帧快速变大
    /// </summary>
    public static void bigger_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
        Animation.SpriteAnimation anim)
    {
        anim.gameObject.transform.localScale = anim.gameObject.transform.localScale * 1.5f;
    }
}