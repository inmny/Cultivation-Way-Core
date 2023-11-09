using System;
using System.Collections.Generic;
using Cultivation_Way.Core;
using Cultivation_Way.Library;

namespace Cultivation_Way.Test;

internal static class SpellCast
{
    public static bool cast_spell(string user_id, string target_id, string spell_id)
    {
        CW_Actor user = null;
        CW_Actor target = null;
        List<Actor> simple_list = World.world.units.getSimpleList();
        foreach (Actor actor in simple_list)
        {
            if (actor.data.id == user_id) user = (CW_Actor)actor;
            if (actor.data.id == target_id) target = (CW_Actor)actor;
        }

        if (user == null || target == null) return false;

        CW_SpellAsset spell_asset = Manager.spells.get(spell_id);
        if (spell_asset == null) throw new Exception($"No found spell {spell_id}");

        spell_asset.anim_action?.Invoke(spell_asset, user, target, target.currentTile, 100);
        spell_asset.spell_action?.Invoke(spell_asset, user, target, target.currentTile, 100);

        return true;
    }
}