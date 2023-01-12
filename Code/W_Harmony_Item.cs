using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Item
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ItemGenerator), "generateItem")]
        public static bool itemgenerator_generateItem(ItemAsset pItemAsset, string pMaterial, int pYear, string pWhere, string pWho, int pTries, ActorBase pActor, ref ItemData __result)
        {
            __result = Utils.CW_ItemTools.generate_item(pItemAsset, pMaterial, pYear, pWhere, pWho, pTries, pActor);
            return false;
        }
        
    }
}
