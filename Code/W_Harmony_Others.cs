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
            ModState.instance.cur_language = pLanguage;
            if (pLanguage != "cz") pLanguage = "en";

            Dictionary<string, object> textDic = Json.Deserialize(
                Utils.CW_ResourceHelper.load_json_once("cw_locales/" + pLanguage)
                ) as Dictionary<string, object>;

            Dictionary<string, string> localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            foreach (string key in textDic.Keys)
            {
                localizedText[key] = textDic[key] as string;
            }
            foreach(CW_Addon addon in ModState.instance.addons)
            {
                addon.load_localized_text(pLanguage);
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
            BaseSimObject target = CW_Actor.get_beh_actor_target(pActor);
            if (target == null || target.objectType != MapObjectType.Actor || !target.base_data.alive) return false;
            CW_Actor actor = (CW_Actor)pActor;
            if (actor.is_in_default_attack_range(target))
            {
                if(actor.get_weapon_asset().origin_asset.attackType == WeaponType.Melee) CW_Actor.set_s_attackType(actor, WeaponType.Melee);
                CW_Actor.func_tryToAttack(actor, target);
            }
            if (target.base_data.alive) __result = ai.behaviours.BehResult.RestartTask;
            else
            {
                __result = ai.behaviours.BehResult.Continue;
            }
            return false;
        }
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(ActorAnimationLoader), "loadAnimationBoat")]
        public static bool loadAnimationBoat_Prefix(ref string pTexturePath)
        {
            pTexturePath = pTexturePath.Replace("eastern_human", "human");
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NameGenerator), "getName")]
        public static bool generateName_Prefix(string pAssetID, ref string __result)
        {
            __result = CW_NameGenerator.__gen_name(pAssetID);
            //MonoBehaviour.print(string.Format("Try to gen name for '{0}', the result is '{1}'", pAssetID, __result));
            return string.IsNullOrEmpty(__result);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NameGenerator),"generateNameFromTemplate")]
        public static bool generateNameFromTemplate_Prefix(NameGeneratorAsset pAsset, ActorBase pActor, ref string __result)
        {
            __result = CW_NameGenerator.__gen_name(pAsset.id, pActor==null?null:(CW_Actor)pActor);
            //MonoBehaviour.print(string.Format("Try to gen name for '{0}', the result is '{1}'", pAsset.id, __result));
            return string.IsNullOrEmpty(__result);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PowerLibrary), "disableAllOtherMapModes")]
        public static void __disableAllOtherMapModes(string pMainPower)
        {
            GodPower power = AssetManager.powers.get(pMainPower);
            if (PlayerConfig.dict[power.toggle_name].boolVal)
            {
                ModState.instance.map_mode = power.toggle_name;
            }
            else
            {
                ModState.instance.map_mode = String.Empty;
            }
        }
    }
}
