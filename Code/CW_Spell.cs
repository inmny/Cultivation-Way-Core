using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public static class CW_Spell
    {
        public static void cast(string spell_id, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            Library.CW_Asset_Spell asset = Library.CW_Library_Manager.instance.spells.get(spell_id);
            // TODO: 细化
            asset.spell_action(asset, pUser, pTarget, pTargetTile);
            if(asset.anim_type!=Library.CW_Spell_Animation_Type.CUSTOM) asset.anim_action(asset, pUser, pTarget, pTargetTile);
        }
    }
}
