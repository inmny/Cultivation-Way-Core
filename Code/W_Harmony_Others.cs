using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using ReflectionUtility;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Others
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalizedTextManager), "setLanguage")]
        public static void setLanguage_Postfix(string pLanguage)
        {
            if (pLanguage != "cz") pLanguage = "en";

            Dictionary<string, object> textDic = Json.Deserialize(
                Utils.CW_ResourceHelper.load_json_once("cw_locales/" + pLanguage)
                ) as Dictionary<string, object>;

            Dictionary<string, string> localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            foreach (string key in textDic.Keys)
            {
                localizedText[key] = textDic[key] as string;

            }
        }
    }
}
