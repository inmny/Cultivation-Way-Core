using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way.Library;

internal static class CW_MapIconLibrary
{
    public static void init()
    {
        AssetManager.map_icons.add(new MapIconAsset
        {
            id = Constants.Core.mod_prefix + "common_icon",
            id_prefab = "p_mapSprite",
            base_scale = 0.15f,
            add_camera_zoom_mod = false,
            draw_call = MapIconHelper._draw_common_icon,
            render_in_game = true
        });
        if (!MapIconManager._initiated) return;

        foreach (var item in AssetManager.map_icons.list)
        {
            if (item.group_system != null) continue;
            var system = new GameObject().AddComponent<MapIconGroupSystem>();
            system.create(item);
            item.group_system = system;
        }
    }
}