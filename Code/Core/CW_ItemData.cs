using System.Collections.Generic;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using Newtonsoft.Json;

namespace Cultivation_Way.Core;

public class CW_ItemData : ItemData
{
    public BaseStats addition_stats = new();

    public CW_ItemData()
    {
    }

    public CW_ItemData(CW_ItemAsset pAsset, Actor pCreator)
    {
        id = pAsset.VanillaAsset.id;
        Level = pAsset.BaseLevel;
        Spells = new HashSet<string>();
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(pAsset.BaseSpells);
        }

        material = pAsset.MainMaterial;
        year = World.world.mapStats.year;
        by = pCreator.getName();
        if (pCreator.kingdom != null)
        {
            byColor = pCreator.kingdom.kingdomColor.color_text;
            from = pCreator.kingdom.name;
            fromColor = pCreator.kingdom.kingdomColor.color_text;
        }
    }

    [JsonProperty("level")] public int Level { get; private set; }

    [JsonProperty("spells")] public HashSet<string> Spells { get; private set; }

    [Hotfixable]
    public void UpgradeWithCosts(Actor pCreator, Dictionary<string, int> pCost)
    {
        Level++;
        CW_ItemAsset asset = Manager.items.get(id);
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(asset.BaseSpells);
        }

        foreach (string material_id in pCost.Keys)
        {
            CW_ItemMaterialAsset material_asset = Manager.item_materials.get(material_id);
            if (material_asset == null) continue;

            addition_stats.mergeStats(material_asset.base_stats);
            if (Level < Constants.Core.item_level_per_stage) continue;
            if (material_asset.possible_spells_on_slot[(int)asset.VanillaAsset.equipmentType].Count == 0) continue;

            HashSet<string> new_spells =
                new(material_asset.possible_spells_on_slot[(int)asset.VanillaAsset.equipmentType]);
            new_spells.ExceptWith(Spells);
            new_spells.RemoveWhere(spell_id =>
                !Manager.spells.get(spell_id)?.spell_classes.Overlaps(asset.AllowedSpellClasses) ?? true);

            Spells.Add(new_spells.GetRandom());
        }
    }
}