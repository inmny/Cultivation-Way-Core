using System;
using System.Collections.Generic;
using Cultivation_Way.Factory;
using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way.Library;

internal static class CW_MapIconLibrary
{
    public static void init()
    {
        AssetManager.map_icons.add(new MapIconAsset()
        {
            id = Constants.Core.mod_prefix + "common_icon",
            id_prefab = "p_mapSprite",
            base_scale = 0.15f,
            add_camera_zoom_mod = false,
            draw_call = new MapIconUpdater(MapIconHelper._draw_common_icon),
            render_in_game = true
        });
    }
}