using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.HarmonySpace
{
    internal static class Manager
    {
        public static void init()
        {
            _ = new Harmony(Constants.Others.harmony_id+".Content");
            Harmony.CreateAndPatchAll(typeof(H_Actor));
        }
    }
}
