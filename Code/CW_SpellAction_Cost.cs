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
        public static float default_check_and_cost(CW_Asset_Spell spell_asset, BaseSimObject user)
        {
            if (user.objectType == MapObjectType.Actor)
            {
                float cost = -1;
                CW_Actor cw_actor = (CW_Actor)user;
                if (cw_actor.cw_data.status.can_culti)
                {
                    cost += cw_actor.cw_status.wakan * spell_asset.cost;
                    if(cost > spell_asset.min_cost_val)
                    {
                        cw_actor.cw_status.wakan -= (int)cost;
                        cost = Utils.CW_Utils_Others.get_raw_wakan(cost, cw_actor.cw_status.wakan_level);
                    }
                    else
                    {
                        cost = -1;
                    }
                }

                else if ((cw_actor.cw_data.cultisys & Others.CW_Constants.cultisys_bushido_tag) != 0)
                {
                    float health_cost = (cw_actor.fast_data.health - 10) * spell_asset.cost;
                    if (health_cost > spell_asset.min_cost_val)
                    {
                        cw_actor.fast_data.health -= (int)health_cost;
                        cost += Utils.CW_Utils_Others.get_raw_wakan(health_cost, cw_actor.cw_status.health_level);
                    }
                    else
                    {
                        cost = -1;
                    }
                }
                return cost;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static float enemy_nr_check_and_cost(CW_Asset_Spell spell_asset, BaseSimObject user)
        {
            if (Utils.CW_SpellHelper.find_enemies_in_circle(user.currentTile, user.kingdom, 5).Count < spell_asset.free_val) return 0;
            return default_check_and_cost(spell_asset, user);
        }
        public static float low_health_check_and_cost(CW_Asset_Spell spell_asset, BaseSimObject user)
        {
            if (user.objectType == MapObjectType.Actor)
            {
                CW_Actor cw_actor = (CW_Actor)user;
                if (cw_actor.fast_data.health > spell_asset.free_val * cw_actor.cw_cur_stats.base_stats.health) return 0;
                return default_check_and_cost(spell_asset, user);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
