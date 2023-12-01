using System;
using System.Collections.Generic;

namespace Cultivation_Way.Library;

public class CW_ItemAsset : Asset
{
    public int base_level = 0;
    public BaseStats base_stats = new();
    public string main_material = "base";
    public readonly HashSet<string> base_spells = new();
    public readonly HashSet<string> allowed_spell_classes = new();
    public readonly Dictionary<string, int> necessary_resource_cost = new();
    public readonly List<Dictionary<string, int>>[] resource_cost_lists_per_level = new List<Dictionary<string, int>>[Constants.Core.item_level_count];
    public ItemAsset vanilla_asset;
    public CW_ItemAsset()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            resource_cost_lists_per_level[i] = new List<Dictionary<string, int>>();
            var default_cost = new Dictionary<string, int>();

            switch (i / Constants.Core.item_level_per_stage)
            {
                case 0:
                    default_cost.Add(SR.common_metals, i % Constants.Core.item_level_per_stage);
                    break;
                case 1:
                    default_cost.Add(SR.common_metals, i % Constants.Core.item_level_per_stage * 2 + Constants.Core.item_level_per_stage);
                    break;
                case 2:
                    default_cost.Add(SR.silver, i % Constants.Core.item_level_per_stage);
                    break;
                case 3:
                    default_cost.Add(SR.mythril, i % Constants.Core.item_level_per_stage);
                    break;
                case 4:
                    default_cost.Add(SR.adamantine, i % Constants.Core.item_level_per_stage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            resource_cost_lists_per_level[i].Add(default_cost);
        }
    }
    public void ClearCurrentPerLevelCosts()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            resource_cost_lists_per_level[i].Clear();
        }
    }
}
public class CW_ItemLibrary : CW_Library<CW_ItemAsset>
{
    public CW_ItemAsset FindAssetToCraft(Actor pActor)
    {
        return list.GetRandom();
    }
}