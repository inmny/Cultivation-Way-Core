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
using NCMS;

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

            if(pLanguage == "en")
            {
                textDic = Json.Deserialize(
                Utils.CW_ResourceHelper.load_json_once("cw_locales/" + "cz")
                ) as Dictionary<string, object>;

                localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
                foreach (string key in textDic.Keys)
                {
                    if (!localizedText.ContainsKey(key)) localizedText[key] = key.Replace("_", " ").Replace(" title", "").Replace(" description", "");
                }
                foreach (CW_Addon addon in ModState.instance.addons)
                {
                    addon.load_localized_text(pLanguage);
                }
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
        [HarmonyPatch(typeof(MapBox),"clearSimObjects")]
        public static bool clearSimObjects_Prefix()
        {
            ModState.instance.destroy_unit_reason = Destroy_Unit_Reason.CLEAR;
            return true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "clearSimObjects")]
        public static void clearSimObjects_Postfix()
        {
            ModState.instance.destroy_unit_reason = Destroy_Unit_Reason.KILL;
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
        
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "getName")]
        public static bool actor_getName(ActorBase __instance, ref string __result)
        {
            CW_Actor actor = (CW_Actor)__instance;
            if (string.IsNullOrEmpty(actor.fast_data.firstName))
            {
                actor.fast_data.firstName = CW_NameGenerator.gen_name(__instance.stats.nameTemplate, (CW_Actor)__instance);
            }
            __result = actor.fast_data.firstName;
            return false;
        }
        /**
        public static bool allmodswindow_setName(NCMod mod, GameObject modBackgound)
        {
            string text = "modName";
            Vector3 vector = new Vector3(25f, -17.5f, 0f);
            int num = 18;
            Color color = new Color(0.11f, 0.94f, 1f, 1f);
            HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow;

            Type type_of_AllModsWindow = AccessTools.TypeByName("AllModsWindow");

            string text2 = AccessTools.Method(type_of_AllModsWindow, "cutText").Invoke(null, new object[] { mod.name.Replace("postload.", "").Replace("preload.", ""), 20, true }) + " <color=\"#DBDB00\">" + AccessTools.Method(type_of_AllModsWindow, "cutText").Invoke(null, new object[] { mod.version, 10, false }) + "</color>";

            Vector2 vector2 = new Vector2(100f, 100f);
            AccessTools.Method(type_of_AllModsWindow, "setText").Invoke(null, new object[] { text, vector, num, color, horizontalWrapMode, text2, vector2, modBackgound });
            return false;
        }
        */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NameGenerator), "getName")]
        public static bool generateName_Prefix(string pAssetID, ref string __result)
        {
			string tmp_result = CW_NameGenerator.__gen_name(pAssetID);
			if(string.IsNullOrEmpty(tmp_result)) return true;
            __result = tmp_result;
            //MonoBehaviour.print(string.Format("Try to gen name for '{0}', the result is '{1}'", pAssetID, __result));
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NameGenerator),"generateNameFromTemplate")]
        public static bool generateNameFromTemplate_Prefix(NameGeneratorAsset pAsset, ActorBase pActor, ref string __result)
        {
            string tmp_result = CW_NameGenerator.__gen_name(pAsset.id, pActor==null?null:(CW_Actor)pActor);
            if(string.IsNullOrEmpty(tmp_result)) return true;
            __result = tmp_result;
            //MonoBehaviour.print(string.Format("Try to gen name for '{0}', the result is '{1}'", pAssetID, __result));
            return false;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PowerLibrary), "disableAllOtherMapModes")]
        public static void switch_to_this_mode(string pMainPower)
        {
            GodPower power = AssetManager.powers.get(pMainPower);
            
            for (int i = 0; i < AssetManager.powers.list.Count; i++)
            {
                GodPower godPower = AssetManager.powers.list[i];
                if (godPower.map_modes_switch && !(godPower.id == pMainPower))
                {
                    PlayerOptionData playerOptionData = PlayerConfig.dict[godPower.toggle_name];
                    if (playerOptionData.boolVal)
                    {
                        playerOptionData.boolVal = false;
                    }
                }
            }
            if (PlayerConfig.dict[power.toggle_name].boolVal)
                ModState.instance.map_mode = power.toggle_name;
            else
            {
                ModState.instance.map_mode = String.Empty;
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpectateUnit), "updateStats")]
        public static void spectateUnit_updateStats(SpectateUnit __instance)
        {
            W_Content_SpectateUnit.updateStats(__instance);
        }
    }
}
