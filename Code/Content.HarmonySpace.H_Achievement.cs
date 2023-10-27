using HarmonyLib;
using UnityEngine;
using WorldBoxConsole;

namespace Cultivation_Way.Content.HarmonySpace;

internal static class H_Achievement
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldBoxConsole.Console), nameof(WorldBoxConsole.Console.HandleLog))]
    public static void unlock_lost(LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
        Achievements.lost.check();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WindowCreatureInfo), nameof(WindowCreatureInfo.OnEnable))]
    public static void unlock_complete()
    {
        Achievements.complete.check();
    }
}