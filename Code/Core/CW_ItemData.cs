using System;
using System.Collections.Generic;
using Cultivation_Way.Library;
using Newtonsoft.Json;

namespace Cultivation_Way.Core;

public class CW_ItemData : ItemData
{
    [JsonProperty("level")]
    public int Level { get; private set; }
    [JsonProperty("spells")]
    public HashSet<string> Spells { get; private set; }

    public BaseStats addition_stats = new();
    public CW_ItemData()
    {
    }
    public CW_ItemData(CW_ItemAsset pAsset)
    {
        id = pAsset.vanilla_asset.id;
        Level = pAsset.base_level;
        Spells = new HashSet<string>(pAsset.base_spells);
    }

    public void UpgradeWithCosts(Actor pCreator, Dictionary<string, int> pCost)
    {
        Level++;
        CW_ItemAsset asset = Manager.items.get(id);
        foreach(string material_id in pCost.Keys)
        {
            CW_ItemMaterialAsset material_asset = Manager.item_materials.get(material_id);
            if (material_asset == null) continue;

            addition_stats.mergeStats(material_asset.base_stats);
            
            Spells.Add(material_asset.possible_spells_on_slot[(int)asset.vanilla_asset.equipmentType].GetRandom());
        }
    }
}