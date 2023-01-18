using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.Harmony
{
    internal static class W_Harmony_StatusEffect
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BaseSimObject), "updateStatusEffects")]
        public static bool simobject_updateStatusEffects(BaseSimObject __instance, float pElapsed)
        {
            if(__instance.objectType == MapObjectType.Actor)
            {
                ((CW_Actor)__instance).update_status_effects(pElapsed);
            }
            else
            {

            }
            return true;
        }
    }
}
