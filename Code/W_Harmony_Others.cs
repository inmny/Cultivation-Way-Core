using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using ReflectionUtility;
using System.Reflection.Emit;
using System.Reflection;

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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "setMapSize")]
        public static bool setMapSize_Prefix(int pWidth, int pHeight)
        {
            World_Data.instance.map_chunk_manager.reset(pWidth * Config.CITY_ZONE_SIZE, pHeight * Config.CITY_ZONE_SIZE);
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(sfx.MusicMan), "count")]
        public static bool count_Prefix(Building pBuilding)
        {
            if(pBuilding.kingdom==null || !pBuilding.kingdom.isCiv() || sfx.MusicMan.races.ContainsKey(pBuilding.kingdom.raceID)) return true;

            sfx.MusicRaceContainer container = new sfx.MusicRaceContainer();
            container.kingdom_exists = true;
            sfx.MusicMan.races.Add(pBuilding.kingdom.raceID, container);
            
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ai.behaviours.BehAttackActorTarget), "execute")]
        public static bool beh_attack_actor_target_Prefix(Actor pActor, ref ai.behaviours.BehResult __result)
        {
            if (CW_Actor.get_beh_actor_target(pActor) == null)
            {
                __result = ai.behaviours.BehResult.Stop;
                return false;
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorAnimationLoader), "loadAnimationBoat")]
        public static bool loadAnimationBoat_Prefix(ref string pTexturePath)
        {
            pTexturePath = pTexturePath.Replace("eastern_human", "human");
            return true;
        }
    }
}
