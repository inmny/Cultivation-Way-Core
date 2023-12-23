using System;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.General.AboutStatusEffect;

public static class FormatStatusEffect
{
    /// <summary>
    ///     创建并添加一个简单的状态效果
    /// </summary>
    /// <param name="id">状态效果id</param>
    /// <param name="bonus_stats">状态的属性加成</param>
    /// <param name="duration">状态的持续时间</param>
    /// <param name="anim_id">动画id</param>
    /// <param name="anim_path">动画文件夹路径</param>
    /// <param name="anim_scale">动画大小</param>
    /// <param name="path_icon">图标</param>
    /// <param name="tags">状态类别标签</param>
    /// <param name="tier">等阶(None: 隐藏; Basic: 基础; Advanced: 高级)</param>
    /// <returns>创建的状态效果</returns>
    /// <exception cref="Exception">状态效果的属性加成为空; 对应id的状态效果已经存在</exception>
    public static CW_StatusEffect create_simple_status_effect(
        string id, BaseStats bonus_stats,
        float duration = 30f,
        string anim_id = "", string anim_path = "", float anim_scale = 1f,
        string path_icon = "", StatusEffectTag[] tags = null, StatusTier tier = StatusTier.Basic
    )
    {
        if (bonus_stats == null) throw new Exception("create_simple_status_effect: bonus_stats is null");
        if (Manager.statuses.contains(id) || AssetManager.status.dict.ContainsKey(id))
            throw new Exception($"create_simple_status_effect: status {id} already exists");


        CW_StatusEffect status_asset = new()
        {
            id = id,
            bonus_stats = bonus_stats,
            duration = duration,
            anim_id = anim_id,
            path_icon = path_icon,
            tier = tier
        };
        if (tags != null) status_asset.add_tags(tags);

        switch (string.IsNullOrEmpty(anim_id))
        {
            case false when CW_Core.mod_state.anim_manager.get_controller(anim_id) != null:
                Logger.Warn(
                    $"create_simple_status_effect: There is already animation {anim_id}, it will not create a new one for status {id}");
                break;
            case false:
            {
                AnimationSetting anim_setting = new()
                {
                    loop_limit_type = AnimationLoopLimitType.TIME_LIMIT,
                    layer_name = "EffectsBack",
                    loop_time_limit = duration
                };
                anim_setting.frame_action = [Hotfixable](int pIdx, ref Vector2 pVec, ref Vector2 pDstVec,
                    Animation.SpriteAnimation pAnim) =>
                {
                    if (pAnim.src_object == null || !pAnim.src_object.isAlive())
                    {
                        pAnim.force_stop();
                        return;
                    }

                    if (!pAnim.src_object.isActor())
                    {
                        return;
                    }

                    CW_Actor actor = (CW_Actor)pAnim.src_object;

                    float cur_scale = pAnim.get_scale();
                    if (Math.Abs(cur_scale - actor.stats[S.scale]) > 1e-3)
                    {
                        pAnim.set_scale(actor.stats[S.scale]);
                    }

                    if (actor.statuses != null && actor.statuses.TryGetValue(id, out var status))
                    {
                        pAnim.play_time = Math.Max(0, anim_setting.loop_time_limit - status.left_time);
                    }
                    else
                    {
                        pAnim.force_stop(true);
                    }
                };
                anim_setting.set_trace(AnimationTraceType.ATTACH);

                CW_Core.mod_state.anim_manager.load_as_controller(
                    anim_id,
                    anim_path,
                    100,
                    anim_scale,
                    anim_setting
                );
                break;
            }
        }


        Manager.statuses.add(status_asset);
        return status_asset;
    }
}