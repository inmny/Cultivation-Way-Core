using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content.HarmonySpace
{
    internal static class H_Actor
    {
        /// <summary>
        /// 按年更新仙路修炼进度
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), nameof(Actor.updateAge))]
        public static void updateAge_postfix(Actor __instance)
        {
            CW_Actor actor = (CW_Actor)__instance;

            actor.data.get(Content_Constants.immortal_id, out int level, -1);

            if (level < 0) return;
            
            actor.data.get(Constants.DataS.wakan, out float wakan, 0);
            float max_wakan = actor.stats[Constants.CW_S.wakan];

            if (wakan >= max_wakan) 
            {
                actor.check_level_up(Content_Constants.immortal_id);
                return; 
            }
            
            float middle_wakan_line = max_wakan * Content_Constants.immortal_max_wakan_regen;
            if(wakan < middle_wakan_line)
            {
                if(middle_wakan_line - wakan > actor.stats[Constants.CW_S.wakan_regen])
                {
                    wakan += actor.stats[Constants.CW_S.wakan_regen];
                }
                else
                {
                    wakan = middle_wakan_line;
                }
            }

            float culti_wakan = actor.cw_asset.culti_velo * (1+actor.stats[Constants.CW_S.mod_cultivelo]) * Content_Constants.immortal_base_cultivelo * actor.data.get_element().get_type().rarity;
            if (wakan + culti_wakan > max_wakan)
            {
                wakan = max_wakan;
                actor.check_level_up(Content_Constants.immortal_id);
            }
            else
            {
                wakan += culti_wakan;
            }
            actor.data.set(Constants.DataS.wakan, wakan);
        }
    }
}
