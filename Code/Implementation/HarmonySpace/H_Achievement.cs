using HarmonyLib;
using UnityEngine;
using WorldBoxConsole;

namespace Cultivation_Way.Implementation.HarmonySpace;

internal static class H_Achievement
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Console), nameof(Console.HandleLog))]
    public static void unlock_lost(LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
        Achievements.cw_achievementLost.check();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WindowCreatureInfo), nameof(WindowCreatureInfo.OnEnable))]
    public static void unlock_complete()
    {
        Achievements.cw_achievementComplete.check();
    }
}