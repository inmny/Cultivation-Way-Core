using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public static class CW_Spell
    {
        public static bool cast(Library.CW_Asset_Spell asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            float cost = asset.check_and_cost(pUser);
            if (cost < 5) return false;
            // TODO: 细化
            if (asset.anim_type != Library.CW_Spell_Animation_Type.CUSTOM)
            {
                asset.damage_action(asset, pUser, pTarget, pTargetTile, cost);
                asset.anim_action(asset, pUser, pTarget, pTargetTile, cost);
                //WorldBoxConsole.Console.print("Spell should cast");
                ((CW_Actor)pUser).fast_data.favorite = true;
            }
            else
            {
                asset.spell_action(asset, pUser, pTarget, pTargetTile, cost);
            }
            return true;
        }
        public static bool cast(string spell_id, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            return cast(Library.CW_Library_Manager.instance.spells.get(spell_id), pUser, pTarget, pTargetTile);
        }
    }
}
