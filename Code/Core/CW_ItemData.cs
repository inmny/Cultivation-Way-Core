using System.Collections.Generic;
using System.Text;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using NeoModLoader.services;
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
    public CW_ItemData(CW_ItemAsset pAsset, Actor pCreator)
    {
        id = pAsset.vanilla_asset.id;
        Level = pAsset.base_level;
        Spells = new();
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(pAsset.base_spells);
        }

        material = pAsset.main_material;
        year = World.world.mapStats.year;
        by = pCreator.getName();
        if (pCreator.kingdom != null)
        {
            byColor = pCreator.kingdom.kingdomColor.color_text;
            from = pCreator.kingdom.name;
            fromColor = pCreator.kingdom.kingdomColor.color_text;
        }
        
    }
    [Hotfixable]
    public void UpgradeWithCosts(Actor pCreator, Dictionary<string, int> pCost)
    {
        Level++;
        CW_ItemAsset asset = Manager.items.get(id);
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(asset.base_spells);
        }
        foreach(string material_id in pCost.Keys)
        {
            CW_ItemMaterialAsset material_asset = Manager.item_materials.get(material_id);
            if (material_asset == null) continue;

            addition_stats.mergeStats(material_asset.base_stats);
            StringBuilder sb = new();
            sb.AppendLine($"Upgrade {id} with {material_id}({pCost[material_id]})");
            foreach (var stat in addition_stats.stats_dict)
            {
                sb.AppendLine($"\t{stat.Key}: {stat.Value.value} ");
            }
            LogService.LogWarning(sb.ToString());
            if (Level < Constants.Core.item_level_per_stage) continue;
            if (material_asset.possible_spells_on_slot[(int)asset.vanilla_asset.equipmentType].Count == 0) continue;
            Spells.Add(material_asset.possible_spells_on_slot[(int)asset.vanilla_asset.equipmentType].GetRandom());
        }
    }
}