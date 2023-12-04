using System.Collections.Generic;
using Cultivation_Way.Core;
using UnityEngine;

namespace Cultivation_Way.Utils.General.AboutItem;

public static class ItemIconConstructor
{
    private static readonly Dictionary<string, Dictionary<int, Sprite>> items_icon = new();

    public static Sprite GetItemIcon(Sprite pOriginal, CW_Element pElement)
    {
        Color32[] pixels = pOriginal.texture.GetPixels32();

        Color32 color_0 = pElement.GetColor();

        int color_hash = color_0.GetHashCode();
        if (!items_icon.TryGetValue(pOriginal.name, out Dictionary<int, Sprite> dict))
        {
            dict = new Dictionary<int, Sprite>();
            items_icon[pOriginal.name] = dict;
        }
        else if (dict.TryGetValue(color_hash, out Sprite sprite))
        {
            return sprite;
        }

        Color32 color_1 = Toolbox.makeDarkerColor(color_0, 0.9f);
        Color32 color_2 = Toolbox.makeDarkerColor(color_0, 0.8f);
        Color32 color_3 = Toolbox.makeDarkerColor(color_0, 0.7f);

        for (int i = 0; i < pixels.Length; i++)
        {
            if (Toolbox.areColorsEqual(pixels[i], Toolbox.color_green_0))
            {
                pixels[i] = color_0;
            }
            else if (Toolbox.areColorsEqual(pixels[i], Toolbox.color_green_1))
            {
                pixels[i] = color_1;
            }
            else if (Toolbox.areColorsEqual(pixels[i], Toolbox.color_green_2))
            {
                pixels[i] = color_2;
            }
            else if (Toolbox.areColorsEqual(pixels[i], Toolbox.color_green_3))
            {
                pixels[i] = color_3;
            }
        }

        Sprite new_sprite = Sprite.Create(pOriginal.texture, pOriginal.rect, pOriginal.pivot, pOriginal.pixelsPerUnit,
            0, SpriteMeshType.Tight, pOriginal.border, false);
        dict[color_hash] = new_sprite;
        return new_sprite;
    }
}