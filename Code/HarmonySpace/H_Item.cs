using Cultivation_Way.Core;
using Cultivation_Way.Utils;
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
            if (item_data.addition_stats.stats_dict.Count > 0)
            {
            }
            else if (item_data.Level > 1)
            {
                CW_Core.LogWarning($"Item {item_data.id} has no stats but level is {item_data.Level}");
                //item_data.addition_stats[S.damage] = 5 * item_data.Level;
                //item_data.addition_stats[S.mod_damage] = 0.1f * item_data.Level;
                CW_Core.LogInfo(GeneralHelper.to_json(item_data, true));
            }

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