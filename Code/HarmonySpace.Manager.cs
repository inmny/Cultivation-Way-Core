using HarmonyLib;

namespace Cultivation_Way.HarmonySpace
{
    internal class Manager
    {
        public static void init()
        {
            _ = new Harmony(Constants.Others.harmony_id);
            Harmony.CreateAndPatchAll(typeof(H_Actor), Constants.Others.harmony_id);
            Harmony.CreateAndPatchAll(typeof(H_LocalizationManager), Constants.Others.harmony_id);
            Harmony.CreateAndPatchAll(typeof(H_ScrollWindows), Constants.Others.harmony_id);
            Harmony.CreateAndPatchAll(typeof(H_WindowCreatureInfo), Constants.Others.harmony_id);
        }
    }
}
