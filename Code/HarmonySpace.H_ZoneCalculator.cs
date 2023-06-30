using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using HarmonyLib;
using UnityEngine;

namespace Cultivation_Way.HarmonySpace;

internal static class H_ZoneCalculator
{
    /// <summary>
    ///     暂且使用<see cref="MapMode.Special" />来表示
    /// </summary>
    /// <returns></returns>
    private static bool showEnergyMaps()
    {
        return PlayerConfig.optionBoolEnabled(Constants.Core.energy_maps_toggle_name)
               || World.world.isPowerForceMapMode(MapMode.Special);
    }

    private static ZoneDisplayMode _last_mode;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ZoneCalculator), nameof(ZoneCalculator.redrawZones))]
    public static bool before_ZoneCalculator_redrawZones(ZoneCalculator __instance)
    {
        bool all_redraw = false;

        #region 逻辑扩展

        if (__instance._mode == ZoneDisplayMode.None)
        {
            if (showEnergyMaps())
            {
                __instance._mode = (ZoneDisplayMode)CW_ZoneDisplayMode.Energy;

                if (_last_mode != __instance._mode) all_redraw = true;

                __instance._redraw_timer = 0.0f;
                _last_mode = __instance._mode;
            }
            else
            {
                _last_mode = __instance._mode;
                return true;
            }
        }
        else
        {
            _last_mode = __instance._mode;
            return true;
        }

        __instance.sprRnd.enabled = true;

        #endregion

        if (__instance._redraw_timer > 0)
        {
            __instance._redraw_timer -= Time.deltaTime;
            return false;
        }

        __instance._redraw_timer = 0.3f;
        __instance._dirty = false;
        __instance._debug_redrawn_last = 0;
        if (__instance._currentDrawnZones.Any())
            __instance._toCleanUp.UnionWith(__instance._currentDrawnZones);

        #region 选择性渲染

        if (string.IsNullOrEmpty(CW_Core.mod_state.map_chunk_manager.current_map_id))
        {
            if (__instance._toCleanUp.Any()) __instance.clearDrawnZones();
            if (!__instance._dirty) return false;
            __instance.updatePixels();
            return false;
        }

        CW_EnergyMapChunk[,] map =
            CW_Core.mod_state.map_chunk_manager.maps[
                CW_Core.mod_state.map_chunk_manager.current_map_id
            ].map;
        if (all_redraw)
        {
            for (int i = 0; i < CW_Core.mod_state.map_chunk_manager.height; i++)
            {
                for (int j = 0; j < CW_Core.mod_state.map_chunk_manager.width; j++)
                {
                    TileZone zone = World.world.tilesMap[i, j].zone;
                    __instance._currentDrawnZones.Add(zone);
                    __instance._toCleanUp.Remove(zone);

                    map[i, j].need_redraw = false;

                    __instance.pixels[World.world.tilesMap[i, j].data.tile_id] = map[i, j].color;
                    __instance._dirty = true;
                    //`color_zone(__instance, zone, ref map[i, j].color);
                }
            }
        }
        else
        {
            for (int i = 0; i < CW_Core.mod_state.map_chunk_manager.height; i++)
            {
                for (int j = 0; j < CW_Core.mod_state.map_chunk_manager.width; j++)
                {
                    TileZone zone = World.world.tilesMap[i, j].zone;
                    __instance._toCleanUp.Remove(zone);

                    if (!map[i, j].need_redraw) continue;

                    __instance._currentDrawnZones.Add(zone);

                    map[i, j].need_redraw = false;

                    __instance.pixels[World.world.tilesMap[i, j].data.tile_id] = map[i, j].color;
                    __instance._dirty = true;
                    //color_zone(__instance, zone, ref map[i, j].color);
                }
            }
        }

        #endregion


        if (__instance._toCleanUp.Any()) __instance.clearDrawnZones();
        if (!__instance._dirty) return false;
        __instance.updatePixels();

        return false;
    }

    private static void color_zone(ZoneCalculator zone_calculator, TileZone zone, ref Color32 color)
    {
        foreach (WorldTile tile in zone.tiles)
        {
            zone_calculator.pixels[tile.data.tile_id] = color;
        }

        zone_calculator._dirty = true;

        ++zone_calculator._debug_redrawn_last;
    }
}