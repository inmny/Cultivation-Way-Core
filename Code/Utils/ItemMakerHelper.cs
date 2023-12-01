using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;

namespace Cultivation_Way.Utils;

internal static class ItemMakerHelper
{
    public static bool HasEnoughResourcesToMakeItem(CityStorage pStorage, CW_ItemAsset pItem)
    {
        foreach (KeyValuePair<string, int> resource in pItem.NecessaryResourceCost)
        {
            if(!pStorage.resources.TryGetValue(resource.Key, out var slot) || slot.amount < resource.Value)
            {
                return false;
            }
        }
        return true;
    }
    [Hotfixable]
    public static void CostResourcesAndAddProgress(Actor pCreator, CityStorage pStorage, CW_ItemData pItemData, Dictionary<string, int> pCost)
    {
        foreach (KeyValuePair<string, int> resource in pCost)
        {
            pStorage.resources[resource.Key].amount -= resource.Value;
        }
        pItemData.UpgradeWithCosts(pCreator, pCost);
        pCreator.data.WriteObj(DataS.crafting_item_data, pItemData, true);
    }
    [Hotfixable]
    public static void CostResourcesAndCreateProgress(Actor pCreator, CityStorage pStorage, CW_ItemAsset pAsset)
    {
        foreach (KeyValuePair<string, int> resource in pAsset.NecessaryResourceCost)
        {
            pStorage.resources[resource.Key].amount -= resource.Value;
        }
        CW_ItemData item_data = new CW_ItemData(pAsset, pCreator);
        pCreator.data.WriteObj(DataS.crafting_item_data, item_data, true);
    }
    [Hotfixable]
    public static CW_ItemData GetCraftingItemData(Actor pActor)
    {
        var ret = pActor.data.ReadObj<CW_ItemData>(DataS.crafting_item_data, true);
        ret?.addition_stats.AfterDeserialize();
        return ret;
    }

    public static bool HasEnoughResourcesToContinue(Actor pCreator, CityStorage pStorage, CW_ItemData pCraftingItemData, out Dictionary<string, int> pCost)
    {
        CW_ItemAsset asset = Manager.items.get(pCraftingItemData.id);
        if(asset.ResourceCostListsPerLevel[pCraftingItemData.Level].Count == 0)
        {
            pCost = null;
            return false;
        }
        for (int i = 0; i < Constants.Core.item_level_up_res_search_times; i++)
        {
            pCost = asset.ResourceCostListsPerLevel[pCraftingItemData.Level].GetRandom();
            foreach (KeyValuePair<string, int> resource in pCost)
            {
                if (pStorage.resources.TryGetValue(resource.Key, out var slot) &&
                    slot.amount >= resource.Value) continue;
                pCost = null;
                break;
            }
            if (pCost != null)
            {
                return true;
            }
        }
        pCost = null;
        return false;
    }
}