using HarmonyLib;

namespace Cultivation_Way.Implementation.HarmonySpace;

internal static class Manager
{
    public static void init()
    {
        _ = new Harmony(Constants.Others.harmony_id + ".Implementation");
        Harmony.CreateAndPatchAll(typeof(H_Actor));
        Harmony.CreateAndPatchAll(typeof(H_Achievement));
    }
}