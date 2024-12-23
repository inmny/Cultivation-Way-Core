using System.Collections.Generic;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Utils.General.AboutItem;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using Newtonsoft.Json;
using UnityEngine;

namespace Cultivation_Way.Core;

public class CW_ItemData : ItemData
{
    [JsonIgnore] private CW_ItemAsset _asset;
    [JsonIgnore] private bool         _sprite_dirty  = true;
    public               BaseStats    addition_stats = new();

    public CW_Element element = new(new[]
    {
        20, 20, 20, 20, 20
    });

    public CW_ItemData()
    {
    }

    public CW_ItemData(CW_ItemAsset pAsset, Actor pCreator, string pMainMaterial)
    {
        id = pAsset.VanillaAsset.id;
        Level = pAsset.BaseLevel;
        Spells = new HashSet<string>();
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(pAsset.BaseSpells);
        }

        material = pMainMaterial;
        year = World.world.mapStats.year;
        if (pCreator == null)
        {
            by = LM.Get("world");
            byColor = Toolbox.colorToHex(Color.gray);
            from = LM.Get("world");
            fromColor = Toolbox.colorToHex(Color.gray);
        }
        else
        {
            by = pCreator.getName();
            if (pCreator.kingdom != null)
            {
                byColor = pCreator.kingdom.kingdomColor.color_text;
                from = pCreator.kingdom.name;
                fromColor = pCreator.kingdom.kingdomColor.color_text;
            }
        }

        element.Set(pAsset.BaseElement);
    }

    [JsonIgnore] public CW_ItemAsset Asset => _asset ??= Manager.items.get(id);

    [JsonIgnore] public Sprite CachedSprite { get; private set; }

    [JsonProperty("level")] public int Level { get; private set; }

    [JsonProperty("spells")] public HashSet<string> Spells { get; private set; }

    [Hotfixable]
    public Sprite GetSprite()
    {
        if (!_sprite_dirty) return CachedSprite;
        string default_path = "ui/icons/items/icon_" + id + (material == "base" ? "" : "_" + material);
        Sprite original_sprite = SpriteTextureLoader.getSprite(default_path);
        CachedSprite = ItemIconConstructor.GetItemIcon(original_sprite, element);
        _sprite_dirty = false;
        return CachedSprite;
    }

    [Hotfixable]
    public void UpgradeWithCosts(Actor pCreator, Dictionary<string, int> pCost)
    {
        Level++;
        CW_ItemAsset asset = Manager.items.get(id);
        if (Level >= Constants.Core.item_level_per_stage)
        {
            Spells.UnionWith(asset.BaseSpells);
        }

        if (Toolbox.randomChance(0.1f)) addition_stats[S.damage_range] += 0.1f;

        HashSet<string> new_spells = new();
        var add_spell_from_creator =
            Toolbox.randomChance(1 - 10 /
                (10 +
                 Mathf.Log10(
                     Mathf.Max(
                         Mathf.Max(1, pCreator.stats[S.intelligence]),
                         pCreator.data.GetSpellImprintExp()
                     )
                 )
                )
            );
        foreach (string material_id in pCost.Keys)
        {
            CW_ItemMaterialAsset material_asset = Manager.item_materials.get(material_id);
            if (material_asset == null) continue;
            float ratio = Toolbox.randomFloat(0, pCost[material_id]);
            int equip_type = (int)asset.VanillaAsset.equipmentType;
            addition_stats.MergeStats(material_asset.base_stats_on_slot[equip_type],
                ratio);

            element.MergeWith(material_asset.Element, ratio / (Level * 10));

            _sprite_dirty = true;

            if (Level < Constants.Core.item_level_per_stage) continue;
            new_spells.UnionWith(material_asset.possible_spells_on_slot[equip_type]);
        }

        if (Level < Constants.Core.item_level_per_stage) return;
        if (add_spell_from_creator)
            foreach (var spell in pCreator.CW().data_spells)
            {
                CW_SpellAsset spell_asset = Manager.spells.get(spell);
                if (spell_asset == null) continue;
                if (Toolbox.randomChance(Level / (float)spell_asset.rarity *
                                         CW_Element.get_similarity(spell_asset.element, element)))
                    new_spells.Add(spell);
            }

        new_spells.ExceptWith(Spells);
        new_spells.RemoveWhere(spell_id =>
            !Manager.spells.get(spell_id)?.spell_classes.Overlaps(asset.AllowedSpellClasses) ?? true);
        if (new_spells.Count == 0) return;

        var spell_to_add = new_spells.GetRandom();
        Spells.Add(spell_to_add);
        if (add_spell_from_creator && pCreator.CW().data_spells.Contains(spell_to_add))
            pCreator.data.IncreaseSpellImprintExp(Manager.spells.get(spell_to_add).rarity);
        CW_Core.LogInfo($"{pCreator.getName()} 升级法宝 添加法术: {spell_to_add}");
    }
}