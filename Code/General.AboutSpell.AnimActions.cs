using System;
using Cultivation_Way.Constants;
using Cultivation_Way.Library;

namespace Cultivation_Way.General.AboutSpell;

/// <summary>
///     提供一般法术的anim_action
/// </summary>
public static class AnimActions
{
    /// <summary>
    ///     使用者(src_obj), 目标(dst_obj), 按anim_type=ON_USER/ON_TARGET, 设定src_vec,dst_vec = target_vec / user_vec
    /// </summary>
    /// <param name="spell_asset"></param>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <param name="target_tile"></param>
    /// <param name="cost"></param>
    public static void simple_on_obj(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;
        Animation.SpriteAnimation anim;
        switch (spell_asset.anim_type)
        {
            case SpellAnimType.ON_USER:
                anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id, src_vec: user.currentPosition,
                    dst_vec: user.currentPosition, src_obj: user, dst_obj: target);
                break;
            case SpellAnimType.ON_TARGET:
                anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
                    src_vec: target == null ? target_tile.posV : target.currentPosition,
                    dst_vec: target == null ? target_tile.posV : target.currentPosition, src_obj: user,
                    dst_obj: target);
                break;
            default:
                throw new Exception(
                    $"simple_on_obj: anim_type {spell_asset.anim_type} not supported for {spell_asset.id}");
        }
        if(anim is not { isOn: true }) return;
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