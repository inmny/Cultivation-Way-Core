using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
namespace Cultivation_Way.Actions
{
    public class CW_SpellAction_Anim
    {
        public static void default_on_enemy(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if(spell_asset.anim_type == CW_Spell_Animation_Type.ON_TARGET)
            {
                CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pTarget == null ? pTargetTile.pos : pTarget.currentPosition, pTarget == null ? pTargetTile.pos : pTarget.currentPosition, pUser, pTarget, 1f);
            }
            
        }
    }
}
