using System.Collections.Generic;
using System.Linq;
using ai.behaviours;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Utils;
using NeoModLoader.api.attributes;

namespace Cultivation_Way.AI.Tasks.Actors;

public class CW_BehPrepareMakeItem : BehCity
{
    public override void create()
    {
        base.create();
        this.special_inside_object = true;
        this.check_building_target_non_usable = true;
        this.null_check_building_target = true;
    }
    [Hotfixable]
    public override BehResult execute(Actor pCreator)
    {
        if (pCreator.beh_building_target == null) return BehResult.Continue;
        pCreator.timer_action = 3f;
        pCreator.stayInBuilding(pCreator.beh_building_target);
        pCreator.beh_tile_target = null;
        pCreator.beh_building_target.startShake(1.5f);
        TryToMakeNormalItem(pCreator);
        return BehResult.Continue;
    }
    [Hotfixable]
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

    public override void create()
    {
        base.create();
        this.special_inside_object = true;
        this.check_building_target_non_usable = true;
        this.null_check_building_target = true;
    }

    private static MapIconAsset mark = AssetManager.map_icons.get("ate_item");
    [Hotfixable]
    public override BehResult execute(Actor pCreator)
    {
        CW_ItemData crafting_item_data = ItemMakerHelper.GetCraftingItemData(pCreator);
        if (crafting_item_data == null)
        {
            return BehResult.Continue;
        }
        if (Toolbox.randomChance(0.9f))
        {
            var storage = pCreator.city.data.storage;
            if (ItemMakerHelper.HasEnoughResourcesToContinue(pCreator, storage, crafting_item_data, out var cost))
            {
                ItemMakerHelper.CostResourcesAndAddProgress(pCreator, storage, crafting_item_data, cost);
            
                MapIconHelper.AddCommonIcon(AssetManager.resources.get(cost.First().Key).getSprite(), pCreator.currentPosition);
                
                pCreator.timer_action = Toolbox.randomFloat(minTime, maxTime);
                pCreator.stayInBuilding(pCreator.beh_building_target);
                pCreator.beh_tile_target = null;
            
                pCreator.beh_building_target.startShake(pCreator.timer_action / minTime * 0.5f);
                CW_Core.LogInfo($"Item made by {pCreator.name} added progress to {crafting_item_data.Level}");
                return BehResult.RepeatStep;
            }
        }
        return BehResult.Continue;
    }
}

public class CW_BehFinishMakeItem : BehCity
{
    [Hotfixable]
    public override void create()
    {
        base.create();
        this.null_check_building_target = true;
        this.check_building_target_non_usable = true;
        this.special_inside_object = true;
    }
    [Hotfixable]
    public override BehResult execute(Actor pCreator)
    {
        pCreator.exitHouse();
        pCreator.beh_building_target.startShake(0.01f);
        
        CW_ItemData crafting_item_data = ItemMakerHelper.GetCraftingItemData(pCreator);
        if (crafting_item_data == null) return BehResult.Continue;

        if (string.IsNullOrEmpty(crafting_item_data.name))
        {
            crafting_item_data.name = General.AboutNameGenerate.NameGenerateUtils.GenerateItemName(crafting_item_data,
                Manager.items.get(crafting_item_data.id), (CW_Actor)pCreator);
        }
        
        CW_Core.LogInfo($"{pCreator.name}'s crafting item level at {crafting_item_data.Level}");
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
            pCreator.data.WriteObj<object>(DataS.crafting_item_data, null, true);
            CW_Core.LogInfo($"Item made by {pCreator.name}({pCreator.getName()}) extracted to {pCreator.city?.getCityName() ?? "null"}");
        }
        
        return BehResult.Continue;
    }
}