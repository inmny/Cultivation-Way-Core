using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_WorldLaw
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WorldLaws), "init")]
        public static void worldLaws_init(WorldLaws __instance)
        {
            foreach(PlayerOptionData world_law in __instance.list)
            {
                if (__instance.dict.ContainsKey(world_law.name) || !W_Content_WorldLaws.added_world_laws.Contains(world_law.name)) continue;
                __instance.dict[world_law.name] = new PlayerOptionData(world_law.name)
                {
                    boolVal = W_Content_WorldLaws.default_world_law_bool_val[W_Content_WorldLaws.added_world_laws.IndexOf(world_law.name)]
                };
            }
            foreach(string added_world_law in W_Content_WorldLaws.added_world_laws)
            {
                if (__instance.dict.ContainsKey(added_world_law)) continue;
                
                __instance.dict[added_world_law] = new PlayerOptionData(added_world_law)
                {
                    boolVal = W_Content_WorldLaws.default_world_law_bool_val[W_Content_WorldLaws.added_world_laws.IndexOf(added_world_law)]
                };
            }
        }
    }
}
