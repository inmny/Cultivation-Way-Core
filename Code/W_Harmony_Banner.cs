using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Banner
    {
        private static Font font_STLiti = Font.CreateDynamicFontFromOSFont("STLiti", 18);
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BannerLoader), "load")]
        public static bool bannerloader_load(BannerLoader __instance, Kingdom pKingdom)
        {
            if (pKingdom.banner_icon_id >= 0) 
            {
                __instance.transform.Find("Icon").gameObject.SetActive(true);
				__instance.transform.Find("Text").GetComponent<Text>().text = String.Empty;
				//__instance.transform.Find("Text").gameObject.SetActive(false);
				return true; 
            }
            Reflection.SetField(__instance, "kingdom", pKingdom);
            
            Race kingdom_race = AssetManager.raceLibrary.get(pKingdom.raceID);
            KingdomColor kingdom_color = (KingdomColor)Reflection.GetField(typeof(Kingdom), pKingdom, "kingdomColor");
            BannerContainer bannerContainer = BannerGenerator.dict[kingdom_race.banner_id];

            __instance.partBackround.sprite = bannerContainer.backrounds[pKingdom.banner_background_id];
            __instance.partBackround.color = kingdom_color.colorBorderOut;

            Text text = __instance.transform.Find("Text").GetComponent<Text>();
            text.text = pKingdom.name[0].ToString();
            text.fontStyle = UnityEngine.FontStyle.Bold;
            text.fontSize = 18;
            text.font = font_STLiti;
            text.transform.localPosition = new Vector3(6, -8.15f);
            text.color = kingdom_color.colorBorderBannerIcon;

            __instance.transform.Find("Icon").gameObject.SetActive(false);
            __instance.transform.Find("Text").gameObject.SetActive(true);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(KingdomCustomizeWindow), "apply")]
        public static bool kingdomcustomize_apply(KingdomCustomizeWindow __instance)
        {
            //if (Config.selectedKingdom.banner_icon_id >= 0) return true;
			Kingdom selectedKingdom = Config.selectedKingdom;
			KingdomColorContainer container = KingdomColors.getContainer(selectedKingdom.raceID);
			if (selectedKingdom.colorID < 0)
			{
				selectedKingdom.colorID = container.list.Count - 1;
			}
			else if (selectedKingdom.colorID > container.list.Count - 1)
			{
				selectedKingdom.colorID = 0;
			}
			KingdomColor color = KingdomColors.getColor(selectedKingdom.raceID, selectedKingdom.colorID);
			if ((bool)selectedKingdom.CallMethod("updateColor", color))
			{
				W_Content_Helper.zone_calculator.CallMethod("setDrawnZonesDirty");
				W_Content_Helper.zone_calculator.CallMethod("clearCurrentDrawnZones");
			}
			__instance.counter_color.text = (selectedKingdom.colorID + 1).ToString() + "/" + container.list.Count.ToString();

			Race kingdom_race = AssetManager.raceLibrary.get(selectedKingdom.raceID);
			KingdomColor kingdom_color = (KingdomColor)Reflection.GetField(typeof(Kingdom), selectedKingdom, "kingdomColor");


			BannerContainer bannerContainer = BannerGenerator.dict[kingdom_race.banner_id];
			if (selectedKingdom.banner_background_id < 0)
			{
				selectedKingdom.banner_background_id = bannerContainer.backrounds.Count - 1;
			}
			else if (selectedKingdom.banner_background_id > bannerContainer.backrounds.Count - 1)
			{
				selectedKingdom.banner_background_id = 0;
			}
			__instance.counter_banner_design.text = (selectedKingdom.banner_background_id + 1).ToString() + "/" + bannerContainer.backrounds.Count.ToString();
			if (selectedKingdom.banner_icon_id < -1)
			{
				selectedKingdom.banner_icon_id = bannerContainer.icons.Count - 1;
			}
			else if (selectedKingdom.banner_icon_id > bannerContainer.icons.Count - 1)
			{
				selectedKingdom.banner_icon_id = -1;
			}

			__instance.counter_banner_emblem.text = (selectedKingdom.banner_icon_id + 1).ToString() + "/" + bannerContainer.icons.Count.ToString();

			__instance.kingdomBanner.load(selectedKingdom);
			__instance.image_banner_design.sprite = bannerContainer.backrounds[selectedKingdom.banner_background_id];
			__instance.image_banner_emblem.sprite = selectedKingdom.banner_icon_id == -1 ? Resources.Load<Sprite>("ui/Icons/iconText") : bannerContainer.icons[selectedKingdom.banner_icon_id];
			__instance.image_banner_design.color = kingdom_color.colorBorderOut;
			__instance.image_banner_emblem.color = kingdom_color.colorBorderBannerIcon;
			__instance.kingdom_color_1.color = kingdom_color.colorBorderOut;
			__instance.kingdom_color_2.color = kingdom_color.colorBorderInside;
			return false;
        }
    }
}
