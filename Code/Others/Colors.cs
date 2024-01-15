using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way.Others;

public static class Colors
{
    public static readonly Color default_color = new(1, 0.6073f, 0.1103f, 1);
    private static readonly Dictionary<string, ContainerItemColor> container_colors_dict = new();
    private static readonly Dictionary<string, Color> colors_dict = new();
    internal static Color GetColor(string color_id)
    {
        if (colors_dict.TryGetValue(color_id, out Color ret)) return ret;
        try
        {
            ret = (Color)typeof(Colors).GetField(color_id)?.GetValue(null);
            if (ret != null)
            {
                colors_dict[color_id] = ret;
                return ret;
            }
        }
        catch (Exception) { }
        try
        {
            ret = (Color)typeof(Color).GetField(color_id)?.GetValue(null);
            if (ret != null)
            {
                colors_dict[color_id] = ret;
                return ret;
            }
        }
        catch (Exception) { }
        try
        {
            ret = (Color)typeof(Toolbox).GetField(color_id)?.GetValue(null);
            if (ret != null)
            {
                colors_dict[color_id] = ret;
                return ret;
            }
        }
        catch (Exception) { }

        return default_color;
    }
    internal static ContainerItemColor GetContainerItemColor(string pColorHex)
    {
        if (container_colors_dict.TryGetValue(pColorHex, out ContainerItemColor ret)) return ret;
        ret = new ContainerItemColor(pColorHex, "materials/ItemLegendary");
        container_colors_dict[pColorHex] = ret;
        return ret;
    }
}