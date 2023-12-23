using NeoModLoader.api.attributes;
using NeoModLoader.services;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cultivation_Way.Core;

public class CW_EnergyMapLayer : MapLayer
{
    private object lock_pixels = new object();
    private Color32[] tmp_pixels;
    private bool to_update = false;
    private Color spr_color = Color.white;
    public override void create()
    {
        base.create();
    }
    private bool last_enabled = false;
    public override void update(float pElapsed)
    {
        if (pixels == null)
            createTextureNew();
        if (!PlayerConfig.optionBoolEnabled(Constants.Core.energy_maps_toggle_name))
        {
            sprRnd.enabled = false;
            return;
        }
        
        if (MapBox.isRenderMiniMap())
        {
            spr_color.a = World.world.zoneCalculator.minimap_opacity;
        }
        else
        {
            spr_color.a = Mathf.Clamp(ZoneCalculator.getCameraScaleZoom() * 0.3f, 0f, 0.7f);
        }
        spr_color.a *= World.world.zoneCalculator._night_mod;

        sprRnd.enabled = true;
        sprRnd.color = spr_color;

        if (!to_update) return;
        Monitor.Enter(lock_pixels);
        updatePixels();
        Monitor.Exit(lock_pixels);

        base.update(pElapsed);
    }
    private object force_redraw_obj = new object();
    private bool force_redraw = false;

    public void ForceRedraw()
    {
        Monitor.Enter(force_redraw_obj);
        force_redraw = true;
        Monitor.Exit(force_redraw_obj);
    }
    private void ExitForceRedraw()
    {
        Monitor.Enter(force_redraw_obj);
        force_redraw = false;
        Monitor.Exit(force_redraw_obj);
    }
    [Hotfixable]
    internal void PrepareRedraw()
    {
        if (pixels == null) return;
        if (tmp_pixels == null || tmp_pixels.Length != pixels.Length) tmp_pixels = new Color32[pixels.Length];
        if (!sprRnd.enabled && !force_redraw)
        {
            last_enabled = false;
            return;
        }

        CW_EnergyMap map =
            CW_Core.mod_state.energy_map_manager.maps[
                CW_Core.mod_state.energy_map_manager.current_map_id
            ];
        int count = 0;
        Array.Copy(pixels, tmp_pixels, pixels.Length);
        if (last_enabled) // Check to update all
        {
            while (map.tiles_to_redraw.Count > 0 && map.tiles_to_redraw.TryTake(out var tile))
            {
                tmp_pixels[World.world.tilesMap[tile.x, tile.y].data.tile_id] = tile.color;
                count++;
            }
            if (count == 0)
            {
                return;
            }
        }
        else
        {
            for (int x = 0; x < CW_Core.mod_state.energy_map_manager.width; x++)
            {
                for (int y = 0; y < CW_Core.mod_state.energy_map_manager.height; y++)
                {
                    tmp_pixels[World.world.tilesMap[x, y].data.tile_id] = map.map[x, y].color;
                }
            }
        }

        last_enabled = true;
        Monitor.Enter(lock_pixels);
        to_update = true;
        var tmp = pixels;
        pixels = tmp_pixels;
        tmp_pixels = tmp;
        Monitor.Exit(lock_pixels);
        ExitForceRedraw();
    }
}