﻿using System;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Utils.General.AboutSpell;

/// <summary>
///     提供一般法术的anim_action
/// </summary>
public static class AnimActions
{
    /// <summary>
    ///     下落高度来自于anim_setting.free_val
    /// </summary>
    public static void fall_to_ground(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;

        float fall_height = EffectManager.instance.get_controller(spell_asset.anim_id).default_setting.free_val;
        Animation.SpriteAnimation anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
            (target == null ? target_tile.posV : target.currentPosition) + new Vector2(0, fall_height),
            target == null ? target_tile.posV : target.currentPosition,
            user, target);

        if (anim is not { isOn: true }) return;
        anim.data.set(DataS.spell_cost, cost);
    }

    /// <summary>
    ///     使用者(src_obj), 目标(dst_obj), 按anim_type=ON_USER/ON_TARGET, 设定src_vec,dst_vec = target_vec / user_vec
    /// </summary>
    public static void simple_on_something(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;
        Animation.SpriteAnimation anim = spell_asset.anim_type switch
        {
            SpellAnimType.ON_USER => CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
                user.currentPosition, user.currentPosition, user, target),
            SpellAnimType.ON_TARGET => CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
                target == null ? target_tile.posV : target.currentPosition,
                target == null ? target_tile.posV : target.currentPosition, user, target),
            _ => throw new Exception(
                $"simple_on_obj: anim_type {spell_asset.anim_type} not supported for {spell_asset.id}")
        };

        if (anim is not { isOn: true }) return;
        anim.data.set(DataS.spell_cost, cost);
    }

    /// <summary>
    ///     使用者(src_obj), 目标(dst_obj), 按anim_type=ON_USER/ON_TARGET, 设定src_vec,dst_vec = target_vec / user_vec
    ///     <para>自动调整为目标大小</para>
    /// </summary>
    public static void on_something_auto_scale(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;
        Animation.SpriteAnimation anim = spell_asset.anim_type switch
        {
            SpellAnimType.ON_USER => CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
                user.currentPosition, user.currentPosition, user, target, user.stats[S.scale]),
            SpellAnimType.ON_TARGET => CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
                target == null ? target_tile.posV : target.currentPosition,
                target == null ? target_tile.posV : target.currentPosition, user, target,
                target == null ? 1f : target.stats[S.scale]),
            _ => throw new Exception(
                $"simple_on_obj: anim_type {spell_asset.anim_type} not supported for {spell_asset.id}")
        };

        if (anim is not { isOn: true }) return;
        anim.data.set(DataS.spell_cost, cost);
    }

    /// <summary>
    ///     使用者(src), 目标(dst)传入spawn_anim
    /// </summary>
    public static void simple_user_to_target(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;

        Animation.SpriteAnimation anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
            user.currentPosition, target == null ? target_tile.posV : target.currentPosition, user, target);

        if (anim is not { isOn: true }) return;
        anim.data.set(DataS.spell_cost, cost);
    }

    /// <summary>
    ///     使用者(dst), 目标(src)传入spawn_anim
    /// </summary>
    public static void simple_target_to_user(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;

        Animation.SpriteAnimation anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
            target == null ? target_tile.posV : target.currentPosition, user.currentPosition, target, user);

        if (anim is not { isOn: true }) return;
        anim.data.set(DataS.spell_cost, cost);
    }
}