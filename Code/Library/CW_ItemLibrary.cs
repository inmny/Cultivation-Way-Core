using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;

namespace Cultivation_Way.Library;

public class CW_ItemAsset : Asset
{
    public int BaseLevel = 0;
    public CW_Element BaseElement = new(new int[] { 20, 20, 20, 20, 20 });
    public CW_ItemType ItemType { get; protected set; }
    public BaseStats base_stats = new();
    public string MainMaterial = "base";
    public readonly HashSet<string> BaseSpells = new();
    public readonly HashSet<string> AllowedSpellClasses = new();
    public readonly Dictionary<string, int> NecessaryResourceCost = new();
    public readonly List<Dictionary<string, int>>[] ResourceCostListsPerLevel = new List<Dictionary<string, int>>[Constants.Core.item_level_count];
    public ItemAsset VanillaAsset;
    public CW_ItemAsset()
    {
        SetDefaultResourceCostPerLevel();
    }

    private void SetDefaultResourceCostPerLevel()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            ResourceCostListsPerLevel[i] = new List<Dictionary<string, int>>();
            var default_cost = new Dictionary<string, int>();

            switch (i / Constants.Core.item_level_per_stage)
            {
                case 0:
                    default_cost.Add(SR.common_metals, i % Constants.Core.item_level_per_stage);
                    break;
                case 1:
                    default_cost.Add(SR.common_metals,
                        i % Constants.Core.item_level_per_stage * 2 + Constants.Core.item_level_per_stage);
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


            ResourceCostListsPerLevel[i].Add(default_cost);
        }
    }

    public CW_ItemAsset(string id)
    {
        this.id = id;
        SetDefaultResourceCostPerLevel();
    }
    }
    public void ClearCurrentPerLevelCosts()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            ResourceCostListsPerLevel[i].Clear();
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