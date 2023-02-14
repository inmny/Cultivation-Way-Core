using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    public static class CW_Spell
    {
        public static bool cast(Library.CW_Asset_Spell asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            float cost = asset.check_and_cost(pUser);
            if (cost < 0) return false;
            return CW_Spell_Manager.instance.enqueue_spell(asset, pUser, pTarget, pTargetTile, cost);
        }
        public static bool cast(string spell_id, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            return cast(Library.CW_Library_Manager.instance.spells.get(spell_id), pUser, pTarget, pTargetTile);
        }
        internal static void __cast(Library.CW_Asset_Spell asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if (asset.anim_type != Library.CW_Spell_Animation_Type.CUSTOM)
            {
                if (asset.damage_action != null) asset.damage_action(asset, pUser, pTarget, pTargetTile, cost);
                if (asset.anim_action != null) asset.anim_action(asset, pUser, pTarget, pTargetTile, cost);
            }
            else
            {
                asset.spell_action(asset, pUser, pTarget, pTargetTile, cost);
            }
        }
    }
}
