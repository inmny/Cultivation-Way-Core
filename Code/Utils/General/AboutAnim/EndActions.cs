using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutSpell;
using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way.General.AboutAnim;

/// <summary>
///     一般动画的setting.end_action
/// </summary>
public static class EndActions
{
    public static void src_obj_damage_to_dst_obj(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
        Animation.SpriteAnimation anim)
    {
        a_damage_to_b(anim.src_object, anim.dst_object, anim);
    }

    public static void src_obj_damage_to_dst_tile(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
        Animation.SpriteAnimation anim)
    {
        a_damage_to_b_tile(anim.src_object, ref dst_vec, anim);
    }

    public static void dst_obj_damage_to_src_obj(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
        Animation.SpriteAnimation anim)
    {
        a_damage_to_b(anim.dst_object, anim.src_object, anim);
    }

    public static void dst_obj_damage_to_src_tile(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
        Animation.SpriteAnimation anim)
    {
        a_damage_to_b_tile(anim.dst_object, ref src_vec, anim);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void a_damage_to_b(BaseSimObject a_obj, BaseSimObject b_obj, Animation.SpriteAnimation anim)
    {
        if (b_obj != null && b_obj.isAlive())
        {
            if (!GeneralHelper.is_enemy(b_obj, a_obj)) return;
            anim.data.get(DataS.spell_cost, out float spell_cost, 1);
            spell_cost = MiscUtils.WakanCostToDamage(spell_cost, a_obj);
            b_obj.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: a_obj);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void a_damage_to_b_tile(BaseSimObject a_obj, ref Vector2 b_vec, Animation.SpriteAnimation anim)
    {
        if (a_obj == null || !a_obj.isAlive()) return;
        WorldTile center_tile = World.world.GetTile((int)b_vec.x, (int)b_vec.y);
        if (center_tile == null) return;

        // anim.data中应当保存范围
        anim.data.get(DataS.spell_range, out float radius, 1);
        anim.data.get(DataS.spell_cost, out float spell_cost, 1);
        spell_cost = MiscUtils.WakanCostToDamage(spell_cost, a_obj);

        List<WorldTile> tiles = GeneralHelper.get_tiles_in_circle(center_tile, radius);
        foreach (WorldTile tile in tiles)
        {
            if (tile.building != null && GeneralHelper.is_enemy(tile.building, a_obj))
                tile.building.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: a_obj);
            foreach (Actor actor in tile._units)
            {
                if (!GeneralHelper.is_enemy(actor, a_obj)) continue;
                actor.getHit(spell_cost, pAttackType: (AttackType)CW_AttackType.Spell, pAttacker: a_obj);
            }
        }
    }
}