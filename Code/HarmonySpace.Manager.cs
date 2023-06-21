using HarmonyLib;

namespace Cultivation_Way.HarmonySpace
{
    internal class Manager
    {
        public static void init()
        {
            Harmony harmony = new Harmony(Constants.Others.harmony_id);
            Harmony.CreateAndPatchAll(typeof(H_Actor), Constants.Others.harmony_id);
        }
    }
}
