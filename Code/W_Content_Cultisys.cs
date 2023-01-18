using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal class W_Content_Cultisys
    {
        public static void add_cultisys()
        {
            int i;
            CW_Asset_CultiSys immortal = CW_Library_Manager.instance.cultisys.add(
                new CW_Asset_CultiSys()
                {
                    id = "immortal",
                    sprite_name = "icon_immortal",
                    judge = immortal_judge,
                    level_judge = immortal_level_judge
                }
            );
            //WorldBoxConsole.Console.print(immortal == null);
            for (i = 10; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                immortal.power_level[i] = 1 + (i - 9) / 10f;
            }
            for (i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                immortal.bonus_stats[i].age_bonus = 10 * i;
                immortal.bonus_stats[i].base_stats.health = 5 * i;
                immortal.bonus_stats[i].health_regen = i / 2;
                immortal.bonus_stats[i].base_stats.armor = i;
                immortal.bonus_stats[i].base_stats.damage = 20 * (i+1);
                immortal.bonus_stats[i].wakan = 20 * (i+1);
                immortal.bonus_stats[i].mod_wakan = 5 * (i+1);
                immortal.bonus_stats[i].wakan_regen = (i + 1);
                immortal.bonus_stats[i].mod_wakan_regen = (i + 1);
            }


            CW_Asset_CultiSys bushido = CW_Library_Manager.instance.cultisys.add(
                new CW_Asset_CultiSys()
                {
                    id = "bushido",
                    sprite_name = "icon_bushido",
                    judge = bushido_judge,
                    level_judge = bushido_level_judge
                }
            );
            for (i = 10; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.power_level[i] = 1 + (i - 9) / 10f;
            }
            //WorldBoxConsole.Console.print(bushido == null);
            for (i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.bonus_stats[i].age_bonus = 10 * i;
                bushido.bonus_stats[i].base_stats.health = 50 * (i+1);
                bushido.bonus_stats[i].health_regen = 5 * (i+1);
                bushido.bonus_stats[i].base_stats.armor = 5*i;
                bushido.bonus_stats[i].base_stats.damage = 4 * (i + 1);
            }

        }
        private static bool immortal_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            return cw_actor.cw_status.can_culti;
        }
        private static bool bushido_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            return cw_actor.cw_cur_stats.base_stats.health * cw_actor.cw_cur_stats.base_stats.damage * cw_actor.cw_cur_stats.base_stats.armor / ((cw_actor.cw_stats.cw_stats.base_stats.health+1) * (cw_actor.cw_stats.cw_stats.base_stats.damage+1) * (cw_actor.cw_stats.cw_stats.base_stats.armor+1)) > 1.2f;
        }
        private static bool bushido_level_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            if (cw_actor.cw_data.cultisys_level[cultisys.tag] <= Others.CW_Constants.max_cultisys_level - 1 - 1 && cw_actor.fast_data.health >= cw_actor.cw_cur_stats.base_stats.health)
            {
                if (cw_actor.cw_data.cultisys_level[cultisys.tag] == 9) cw_actor.fast_data.age = 0;

                if (cw_actor.cw_status.health_level < cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1])
                {
                    cw_actor.cw_status.health_level = cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1];
                    cw_actor.fast_data.health = (int)Utils.CW_Utils_Others.get_raw_wakan(cw_actor.fast_data.health, cw_actor.cw_status.health_level);
                    cw_actor.fast_data.health = (int)Utils.CW_Utils_Others.compress_raw_wakan(cw_actor.fast_data.health, cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1]);
                }
                return true;
            }
            return false;
        }
        private static bool immortal_level_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            //if(cw_actor_data.cultisys_level[cultisys.tag] < Others.CW_Constants.max_cultisys_level) WorldBoxConsole.Console.print(cw_actor_data.status.wakan + "/" + cultisys.power_level[cw_actor_data.cultisys_level[cultisys.tag]]);
            if( cw_actor.cw_data.cultisys_level[cultisys.tag]+1 < Others.CW_Constants.max_cultisys_level && cw_actor.cw_status.wakan >= cw_actor.cw_cur_stats.wakan)
            {
                if(cw_actor.cw_status.wakan_level < cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1])
                {
                    cw_actor.cw_status.wakan_level = cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1];
                    cw_actor.cw_status.wakan = (int)Utils.CW_Utils_Others.get_raw_wakan(cw_actor.cw_status.wakan, cw_actor.cw_status.wakan_level);
                    cw_actor.cw_status.wakan = (int)Utils.CW_Utils_Others.compress_raw_wakan(cw_actor.cw_status.wakan, cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1]);
                }
                return true;
            }
            return false;
        }
    }
}
