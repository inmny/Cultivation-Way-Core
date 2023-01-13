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
            WorldBoxConsole.Console.print(immortal == null);
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
                immortal.grade_val[i] = 20 * (i+1);
            }
       

        }
        private static bool immortal_judge(CW_ActorData cw_actor_data, CW_Asset_CultiSys cultisys)
        {
            return cw_actor_data.status.can_culti;
        }
        private static bool bushido_judge(CW_ActorData cw_actor_data, CW_Asset_CultiSys cultisys)
        {
            return true;
        }
        private static bool immortal_level_judge(CW_ActorData cw_actor_data, CW_Asset_CultiSys cultisys)
        {
            //if(cw_actor_data.cultisys_level[cultisys.tag] < Others.CW_Constants.max_cultisys_level) WorldBoxConsole.Console.print(cw_actor_data.status.wakan + "/" + cultisys.grade_val[cw_actor_data.cultisys_level[cultisys.tag]]);
            if( cw_actor_data.cultisys_level[cultisys.tag] <= Others.CW_Constants.max_cultisys_level-1-1 && cw_actor_data.status.wakan >= cultisys.grade_val[cw_actor_data.cultisys_level[cultisys.tag]])
            {
                cw_actor_data.status.wakan = (int)cultisys.grade_val[cw_actor_data.cultisys_level[cultisys.tag]] / 10;
                //WorldBoxConsole.Console.print("Level up to "+(1+cw_actor_data.cultisys_level[cultisys.tag]));
                return true;
            }
            return false;
        }
    }
}
