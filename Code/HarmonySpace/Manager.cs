using System;
using System.Reflection;
using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal class Manager
{
    public static void init()
    {
        _ = new Harmony(Constants.Others.harmony_id);
        Type[] all_types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in all_types)
        {
            if (type.Namespace != $"{nameof(Cultivation_Way)}.{nameof(HarmonySpace)}") continue;

            if (type.Name.StartsWith("H_"))
            {
                Harmony.CreateAndPatchAll(
                    type, Constants.Others.harmony_id
                );
            }
        }
    }
}