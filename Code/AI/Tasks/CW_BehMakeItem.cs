using System.Collections.Generic;
using ai.behaviours;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Utils;

namespace Cultivation_Way.AI.Tasks;

public class CW_BehPrepareMakeItem : BehCity
{
    public override BehResult execute(Actor pCreator)
    {
        pCreator.timer_action = 3f;
        pCreator.stayInBuilding(pCreator.beh_building_target);
        pCreator.beh_tile_target = null;
        pCreator.beh_building_target.startShake(1.5f);
        TryToMakeNormalItem(pCreator);
        return BehResult.Continue;
    }

    private void TryToMakeNormalItem(Actor pCreator)
    {
        var storage = pCreator.city.data.storage;
        for (int i = 0; i < 5; i++)
        {
            CW_ItemAsset asset = Manager.items.FindAssetToCraft(pCreator);
            if (asset != null)
            {
                if (ItemMakerHelper.HasEnoughResourcesToMakeItem(storage, asset))
                {
                    ItemMakerHelper.CostResourcesAndCreateProgress(pCreator, storage, asset);
                    return;
                }
            }
        }
    }
}
public class CW_BehMakeItem : BehCity
{
    private readonly float minTime;
    private readonly float maxTime;

    public CW_BehMakeItem(float pMinTime, float pMaxTime)
    {
        minTime = pMinTime;
        maxTime = pMaxTime;
    }
    public override BehResult execute(Actor pCreator)
    {
        if (Toolbox.randomBool())
        {
            CW_ItemData crafting_item_data = ItemMakerHelper.GetCraftingItemData(pCreator);
            if (crafting_item_data == null) return BehResult.Continue;
            var storage = pCreator.city.data.storage;
            if (ItemMakerHelper.HasEnoughResourcesToContinue(pCreator, storage, crafting_item_data, out var cost))
            {
                ItemMakerHelper.CostResourcesAndAddProgress(pCreator, storage, crafting_item_data, cost);
                pCreator.timer_action = Toolbox.randomFloat(minTime, maxTime);
                pCreator.beh_building_target.startShake(pCreator.timer_action / minTime * 0.5f);
                return BehResult.RepeatStep;
            }
        }
        return BehResult.Continue;
    }
}

public class CW_BehFinishMakeItem : BehaviourActionActor
{
    public override BehResult execute(Actor pCreator)
    {
        CW_ItemData crafting_item_data = ItemMakerHelper.GetCraftingItemData(pCreator);
        if (crafting_item_data == null) return BehResult.Continue;

        bool item_extracted;
        if (pCreator.city == null || !pCreator.city.isAlive())
        {
            item_extracted = City.giveItem(pCreator, new List<ItemData> { crafting_item_data }, null);
        }
        else
        {
            pCreator.city.data.storage.addItem(crafting_item_data);
            item_extracted = true;
        }
        
        if (item_extracted)
        {
            pCreator.data.WriteObj<object>(DataS.crafting_item_data, null);
        }
        return BehResult.Continue;
    }
}