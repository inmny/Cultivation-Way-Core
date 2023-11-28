using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Utils;

internal static class ItemMakerHelper
{
    public static bool HasEnoughResourcesToMakeItem(CityStorage pStorage, CW_ItemAsset pItem)
    {
        foreach (KeyValuePair<string, int> resource in pItem.necessary_resource_cost)
        {
            if(!pStorage.resources.TryGetValue(resource.Key, out var slot) || slot.amount < resource.Value)
            {
                return false;
            }
        }
        return true;
    }

    public static void CostResourcesAndAddProgress(Actor pCreator, CityStorage pStorage, CW_ItemData pItemData, Dictionary<string, int> pCost)
    {
        foreach (KeyValuePair<string, int> resource in pCost)
        {
            pStorage.resources[resource.Key].amount -= resource.Value;
        }
        pItemData.UpgradeWithCosts(pCreator, pCost);
    }

    public static void CostResourcesAndCreateProgress(Actor pCreator, CityStorage pStorage, CW_ItemAsset pAsset)
    {
        foreach (KeyValuePair<string, int> resource in pAsset.necessary_resource_cost)
        {
            pStorage.resources[resource.Key].amount -= resource.Value;
        }
        CW_ItemData item_data = new CW_ItemData(pAsset);
        pCreator.data.WriteObj(DataS.crafting_item_data, item_data);
    }
    public static CW_ItemData GetCraftingItemData(Actor pActor)
    {
        return pActor.data.ReadObj<CW_ItemData>(DataS.crafting_item_data);
    }

    public static bool HasEnoughResourcesToContinue(Actor pCreator, CityStorage pStorage, CW_ItemData pCraftingItemData, out Dictionary<string, int> pCost)
    {
        CW_ItemAsset asset = Manager.items.get(pCraftingItemData.id);
        if(asset.resource_cost_lists_per_level[pCraftingItemData.Level].Count == 0)
        {
            pCost = null;
            return false;
        }
        for (int i = 0; i < Constants.Core.item_level_up_res_search_times; i++)
        {
            pCost = asset.resource_cost_lists_per_level[pCraftingItemData.Level].GetRandom();
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