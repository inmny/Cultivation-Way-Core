using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Building
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Building), "checkTilesForUpgrade")]
        public static void building_checkTilesForUpgrade(Building __instance, bool __result)
        {
            if (__result) return;
            if (__instance.city != null) __instance.city.abandonBuilding(__instance);
            CW_Building.func_startDestroyBuilding(__instance, true);
        }
    }
}
