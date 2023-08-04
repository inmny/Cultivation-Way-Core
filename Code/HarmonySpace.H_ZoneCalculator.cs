using System.Collections.Generic;
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
        if (__instance._currentDrawnZones.Count < World.world.zoneCalculator.zones.Count)
        {
            __instance._currentDrawnZones.UnionWith(__instance.zones);
            __instance._toCleanUp.UnionWith(__instance._currentDrawnZones);
        }

        #region 选择性渲染

        if (string.IsNullOrEmpty(CW_Core.mod_state.map_chunk_manager.current_map_id))
        {
            if (__instance._toCleanUp.Any()) __instance.clearDrawnZones();
            if (!__instance._dirty) return false;
            __instance.updatePixels();
            return false;
        }

        CW_EnergyMapTile[,] map =
            CW_Core.mod_state.map_chunk_manager.maps[
                CW_Core.mod_state.map_chunk_manager.current_map_id
            ].map;
        if (all_redraw)
        {
            for (int x = 0; x < CW_Core.mod_state.map_chunk_manager.width; x++)
            {
                for (int y = 0; y < CW_Core.mod_state.map_chunk_manager.height; y++)
                {
                    __instance.pixels[World.world.tilesMap[x, y].data.tile_id] = map[x, y].color;
                    __instance._dirty = true;
                }
            }

            CW_Core.mod_state.map_chunk_manager.maps[
                CW_Core.mod_state.map_chunk_manager.current_map_id
            ].tiles_to_redraw.Clear();
        }
        else
        {
            HashSet<CW_EnergyMapTile> energy_tiles_to_redraw = CW_Core.mod_state.map_chunk_manager.maps[
                CW_Core.mod_state.map_chunk_manager.current_map_id
            ].tiles_to_redraw;
            foreach (CW_EnergyMapTile energy_tile in energy_tiles_to_redraw)
            {
                __instance.pixels[World.world.tilesMap[energy_tile.x, energy_tile.y].data.tile_id] = energy_tile.color;
                __instance._dirty = true;
            }

            energy_tiles_to_redraw.Clear();
        }

        #endregion


        //if (__instance._toCleanUp.Any()) __instance.clearDrawnZones();
        if (!__instance._dirty) return false;
        __instance.updatePixels();

        return false;
    }
}