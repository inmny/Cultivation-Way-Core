using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Actions
{
    public class CW_SpellAction_Spell
    {
        public static void default_add_status(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if (spell_asset.target_camp == CW_Spell_Target_Camp.ALIAS) pTarget = pUser;
            
            if(pTarget.objectType == MapObjectType.Actor)
            {
                ((CW_Actor)pTarget).add_status_effect("status_" + spell_asset.anim_id);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
