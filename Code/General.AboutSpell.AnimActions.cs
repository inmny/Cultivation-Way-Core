using Cultivation_Way.Constants;
using Cultivation_Way.Library;

namespace Cultivation_Way.General.AboutSpell;

/// <summary>
///     提供一般法术的anim_action
/// </summary>
public static class AnimActions
{
    /// <summary>
    ///     使用者(src), 目标(dst)传入spawn_anim
    /// </summary>
    public static void simple_user_to_target(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
        if (string.IsNullOrEmpty(spell_asset.anim_id)) return;

        Animation.SpriteAnimation anim = CW_Core.mod_state.anim_manager.spawn_anim(spell_asset.anim_id,
            user.currentPosition, target == null ? target_tile.posV : target.currentPosition, user, target);

        if (anim == null || !anim.isOn) return;
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

        if (anim == null || !anim.isOn) return;
        anim.data.set(DataS.spell_cost, cost);
    }
}