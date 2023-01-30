using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Building
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Building), "checkTilesForUpgrade")]
        public static void building_checkTilesForUpgrade(Building __instance, bool __result)
        {
            if (__result) return;
            if (__instance.city != null) __instance.city.abandonBuilding(__instance);
            CW_Building.func_startDestroyBuilding(__instance, true);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox),"addBuilding")]
        public static bool mapbox_addBuilding(string pID, WorldTile pTile, BuildingData pData, bool pCheckForBuild, bool pSfx, BuildPlacingType pType, ref Building __result)
        {
            __result = __mapbox_addBuilding(pID, pTile, pData, pCheckForBuild, pSfx, pType);
            return false;
        }
        private static CW_Building __mapbox_addBuilding(string pID, WorldTile pTile, BuildingData pData, bool pCheckForBuild, bool pSfx, BuildPlacingType pType)
        {
			CW_Asset_Building cw_stats = CW_Library_Manager.instance.buildings.get(pID);
			if (pCheckForBuild && !CW_Building.func_canBuildFrom(MapBox.instance, pTile, cw_stats.origin_stats, null, pType))
			{
				return null;
			}
			CW_Building building = UnityEngine.Object.Instantiate(W_Content_Helper.get_building_prefab()).GetComponent<CW_Building>();
			building.gameObject.SetActive(true);
			building.cw_cur_stats = new CW_BaseStats();
			building.cw_stats = cw_stats;

			CW_Building.func_create(building);
			CW_Building.func_setBuilding(building, pTile, cw_stats.origin_stats, pData);
			if (pData != null)
			{
				building.cw_data = W_Content_Helper.get_load_cw_data(pData);
				building.fast_data = pData;
				building.loadBuilding(pData);
			}
            else
            {
				building.cw_data = new CW_BuildingData();
				building.fast_data = CW_Building.get_data(building);
            }
			building.transform.parent = W_Content_Helper.transformBuildings;
			if (cw_stats.origin_stats.buildingType == BuildingType.Tree)
			{
				building.transform.parent = building.transform.parent.Find("Trees");
			}
			building.resetShadow();
			MapBox.instance.buildings.Add(building);
			if (pSfx && cw_stats.origin_stats.sfx != "none")
			{
				Sfx.play(cw_stats.origin_stats.sfx, true, -1f, -1f);
			}
			if (Config.timeScale > 10f)
			{
				ReflectionUtility.Reflection.CallStaticMethod(typeof(BuildingTweenExtension), "finishScaleTween", building);
			}
			//WorldBoxConsole.Console.print("Create Building by CW");
			return building;
		}
    }
}
