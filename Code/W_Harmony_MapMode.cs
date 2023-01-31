using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_MapMode
    {
        internal enum CW_ZoneDisplayMode
        {
            None,
            CityBorders,
            KingdomBorders,
            Cultures,
            Wakan
        }
        private static string last_map_mode = string.Empty;
        internal static Action<ZoneCalculator> func_checkDrawnZonesDirty = (Action<ZoneCalculator>)CW_ReflectionHelper.get_method<ZoneCalculator>("checkDrawnZonesDirty");
        internal static Action<ZoneCalculator, ZoneDisplayMode> func_setMode = (Action<ZoneCalculator, ZoneDisplayMode>)CW_ReflectionHelper.get_method<ZoneCalculator>("setMode");
        internal static Action<ZoneCalculator> func_redrawZones = (Action<ZoneCalculator>)CW_ReflectionHelper.get_method<ZoneCalculator>("redrawZones");
        internal static Action<MapLayer, float> func_base_update = (Action<MapLayer, float>)CW_ReflectionHelper.get_method<MapLayer>("update");
        internal static Action<ZoneCalculator> func_updatePixels = (Action<ZoneCalculator>)CW_ReflectionHelper.get_method<ZoneCalculator>("updatePixels");
        internal static Func<ZoneCalculator, Color32[]> get_pixels = CW_ReflectionHelper.create_getter<ZoneCalculator, Color32[]>("pixels");
        internal static Action<ZoneCalculator> func_setDrawnZonesDirty = (Action<ZoneCalculator>)CW_ReflectionHelper.get_method<ZoneCalculator>("setDrawnZonesDirty");
        private static float __timer = 0;
        private const float __time_to_update = 1f;
        private static Color32[] pixels;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ZoneCalculator), "update")]
        public static bool update_prefix(ZoneCalculator __instance, float pElapsed)
        {
            if(__timer>0)__timer -= pElapsed / (Config.timeScale+1);
            func_setDrawnZonesDirty(__instance);
            if (last_map_mode != ModState.instance.map_mode)
            {
                clear_all_zones(__instance);
                last_map_mode = ModState.instance.map_mode;
                func_updatePixels(__instance);
            }
            func_checkDrawnZonesDirty(__instance);
            W_Content_Helper.zone_cal_sprRnd.enabled = true;
            
            switch (ModState.instance.map_mode)
            {
                case "map_city_zones":
                    func_setMode(__instance, ZoneDisplayMode.CityBorders);
                    break;
                case "map_kingdom_zones":
                    func_setMode(__instance, ZoneDisplayMode.KingdomBorders);
                    break;
                case "map_culture_zones":
                    func_setMode(__instance, ZoneDisplayMode.Cultures);
                    break;
                case "map_wakan_zones":
                    func_setMode(__instance, (ZoneDisplayMode)CW_ZoneDisplayMode.Wakan);
                    break;
                default:
                    func_setMode(__instance, ZoneDisplayMode.None);
                    W_Content_Helper.zone_cal_sprRnd.enabled = false;
                    break;
            }


            switch (ModState.instance.map_mode)
            {
                case "map_city_zones":
                case "map_kingdom_zones":
                case "map_culture_zones":
                    func_redrawZones(__instance);
                    break;
                case "map_wakan_zones":
                    if (Utils.CW_Utils_Others.is_map_mode_active("map_wakan_zones") && World_Data.instance.map_chunk_manager.zone_dirty && __timer<=0)
                    {
                        __timer = __time_to_update;
                        World_Data.instance.map_chunk_manager.zone_dirty = false;
                        force_redraw_wakan_zones(__instance);
                    }
                    break;
                default:
                    func_redrawZones(__instance);
                    break;
            }

            Color white = Color.white;
            white.a  = Animation.CW_EffectManager.quality_changer.isFullLowRes()?0.7f: Mathf.Clamp(((float)ReflectionUtility.Reflection.CallStaticMethod(typeof(ZoneCalculator),"getCameraScaleZoom")) * 0.3f, 0f, 0.7f);

            W_Content_Helper.zone_cal_sprRnd.color = white;

            //func_base_update(__instance, pElapsed);
            return false;
        }

        private static void clear_all_zones(ZoneCalculator instance)
        {
            pixels = get_pixels(instance);
            for(int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Toolbox.clear;
            }
            foreach(TileZone zone in instance.zones)
            {
                zone.last_drawn_hashcode = 0;
                zone.last_drawn_id = 0;
            }
        }

        private static void force_redraw_wakan_zones(ZoneCalculator instance)
        {
            int i, j;
            CW_MapChunk_Manager manager = World_Data.instance.map_chunk_manager;
            CW_MapChunk[,] chunks = manager.chunks;

            pixels = get_pixels(instance);
            for (i = 0; i < manager.width; i++)
            {
                for (j = 0; j < manager.height; j++)
                {
                    color_wakan_zone(instance.getZone(i, j), chunks[i, j].wakan_level, chunks[i, j].wakan);
                }
            }
            func_updatePixels(instance);
        }
        

        
        private static void color_wakan_zone(TileZone zone, float wakan_level, float cur_wakan)
        {
            int draw_id = Mathf.CeilToInt(wakan_level * 10);
            if (draw_id == 10) draw_id = (int)(1000*cur_wakan);
            if(draw_id == zone.last_drawn_id)
            {
                return;
            }
            zone.last_drawn_id = draw_id;
            for(int i = 0; i < zone.tiles.Count; i++)
            {
                pixels[zone.tiles[i].data.tile_id] = Utils.CW_Utils_Others.get_wakan_color(wakan_level, cur_wakan);
            }
        }
    }
}
