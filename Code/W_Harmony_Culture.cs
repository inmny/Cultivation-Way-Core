using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ReflectionUtility;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Culture
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Culture), "create")]
        public static bool culture_create(Race pRace, City pCity, Culture __instance)
        {
            __instance.race = pRace.id;
            __instance.list_tech_ids = new List<string>();
            __instance.id = MapBox.instance.mapStats.getNextId("culture");
            __instance.name = CW_NameGenerator.gen_name(pRace.name_template_culture);
            if (pCity != null)
            {
                __instance.village_origin = CW_City.get_data(pCity).cityName;
            }
            else
            {
                __instance.village_origin = "??";
            }
            __instance.year = MapBox.instance.mapStats.year;
            __instance.CallMethod("prepare");
            return false;
        }
    }
}
