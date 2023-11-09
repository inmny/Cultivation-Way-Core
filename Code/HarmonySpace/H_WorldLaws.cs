using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal static class H_WorldLaws
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldLaws), nameof(WorldLaws.init))]
    public static void post_init(WorldLaws __instance)
    {
        foreach (PlayerOptionData world_law in __instance.list)
        {
            if (__instance.dict.ContainsKey(world_law.name) ||
                !General.AboutUI.WorldLaws.switch_laws.ContainsKey(world_law.name)) continue;
            __instance.dict[world_law.name] = world_law;
        }

        foreach (string law_key in General.AboutUI.WorldLaws.switch_laws.Keys)
        {
            if (__instance.dict.ContainsKey(law_key)) continue;

            __instance.dict[law_key] = new PlayerOptionData(law_key)
            {
                boolVal = General.AboutUI.WorldLaws.switch_laws[law_key]
            };
            __instance.list.Add(__instance.dict[law_key]);
        }
    }
}