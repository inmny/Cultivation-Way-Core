using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Actions
{
    public class CW_SpellAction_Cost
    {
        public static float default_cost(CW_Asset_Spell spell_asset, BaseSimObject user)
        {
            if (user.objectType == MapObjectType.Actor)
            {
                float cost = 0;
                CW_Actor cw_actor = (CW_Actor)user;
                if (cw_actor.cw_data.status.can_culti)
                {
                    cost = cw_actor.cw_status.wakan * spell_asset.cost;
                    if(cost > 1)
                    {
                        cw_actor.cw_status.wakan -= (int)cost;
                    }
                }
                else if ((cw_actor.cw_data.cultisys & 0x10) == 1)// TODO: change to bushido
                {
                    cost = (cw_actor.fast_data.health - 5) * spell_asset.cost;
                    if (cost > 1)
                    {
                        cw_actor.fast_data.health -= (int)cost;
                    }
                }
                return cost;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
