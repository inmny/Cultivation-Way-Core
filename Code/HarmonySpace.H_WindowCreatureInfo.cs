using Cultivation_Way.UI;
using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

/// <summary>
///     用于为WindowCreatureInfo扩展显示
/// </summary>
internal static class H_WindowCreatureInfo
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WindowCreatureInfo), nameof(WindowCreatureInfo.OnEnable))]
    public static void after_WindowCreatureInfo_OnEnable(WindowCreatureInfo __instance)
    {
        WindowCreatureInfoHelper.OnEnable_postfix(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WindowCreatureInfo), nameof(WindowCreatureInfo.Update))]
    public static void after_WindowCreatureInfo_Update(WindowCreatureInfo __instance)
    {
        WindowCreatureInfoHelper.Update_postfix(__instance);
    }
}