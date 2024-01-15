using System.Collections.Generic;
using Cultivation_Way.Core;
using NeoModLoader.api.attributes;
using UnityEngine;
namespace Cultivation_Way.Utils.General.AboutItem;

public static class ItemIconConstructor
{
    private static readonly Dictionary<string, Dictionary<int, Sprite>> items_icon = new();

    [Hotfixable]
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
            if (pixels[i].a == 0) continue;
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

        Texture2D texture = new(pOriginal.texture.width, pOriginal.texture.height, TextureFormat.RGBA32,
            false);
        texture.filterMode = pOriginal.texture.filterMode;
        texture.wrapMode = pOriginal.texture.wrapMode;
        texture.anisoLevel = pOriginal.texture.anisoLevel;
        texture.mipMapBias = pOriginal.texture.mipMapBias;
        texture.SetPixels32(pixels);
        texture.Apply();
        Sprite new_sprite = Sprite.Create(texture, pOriginal.rect, pOriginal.pivot, pOriginal.pixelsPerUnit,
            0, SpriteMeshType.Tight, pOriginal.border, false);
        new_sprite.name = pOriginal.name + "_" + Toolbox.colorToHex(color_0);
        dict[color_hash] = new_sprite;
        return new_sprite;
    }
    public static Color GetItemQualityColor(ItemData pItemData)
    {
        if (pItemData is not CW_ItemData cw_item_data) return Color.white;
        return (cw_item_data.Level / Constants.Core.item_level_per_stage) switch
        {
            0 => Color.white,
            1 => Color.green,
            2 => Color.blue,
            3 => Color.magenta,
            4 => Color.yellow,
            _ => Color.red
        };
    }
}