using Cultivation_Way.Core;
using HarmonyLib;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.HarmonySpace;

internal static class H_Item
{
    [Hotfixable]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemTools), nameof(ItemTools.mergeStatsWithItem))]
    private static void MergeStatsWithItemPostfix(BaseStats pStats, ItemData pItemData)
    {
        if (pItemData is CW_ItemData item_data)
        {
            pStats.mergeStats(item_data.addition_stats);

            switch (item_data.Level / Constants.Core.item_level_per_stage)
            {
                case <= 1:
                    ItemTools.s_quality = ItemQuality.Normal;
                    break;
                case 2:
                    ItemTools.s_quality = ItemQuality.Rare;
                    break;
                case 3:
                    ItemTools.s_quality = ItemQuality.Epic;
                    break;
                case >= 4:
                    ItemTools.s_quality = ItemQuality.Legendary;
                    break;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemAsset), nameof(ItemAsset.getSprite))]
    private static bool GetSpritePrefix(ref Sprite __result, ItemData pData)
    {
        if (pData is not CW_ItemData item_data) return true;

        __result = item_data.GetSprite();

        return false;
    }
}