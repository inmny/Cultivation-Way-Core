using System;
using System.Collections.Generic;
using Cultivation_Way.Core;
using NeoModLoader.api.attributes;

namespace Cultivation_Way.Library;

public class CW_ItemMaterialAsset : ElementalAsset
{
    public BaseStats base_stats = new();
    public BaseStats[] base_stats_on_slot = new BaseStats[Enum.GetValues(typeof(EquipmentType)).Length];
    public List<string>[] possible_spells_on_slot = new List<string>[Enum.GetValues(typeof(EquipmentType)).Length];

    public CW_ItemMaterialAsset()
    {
        init();
    }

    public CW_ItemMaterialAsset(CW_ResourceAsset pResourceAsset)
    {
        id = pResourceAsset.id;
        init();
        Element = pResourceAsset.element.Deepcopy();
    }

    private void init()
    {
        Element ??= new CW_Element(new[] { 20, 20, 20, 20, 20 }, comp_type: false);
        for (int i = 0; i < base_stats_on_slot.Length; i++)
        {
            base_stats_on_slot[i] = new BaseStats();
        }

        for (int i = 0; i < possible_spells_on_slot.Length; i++)
        {
            possible_spells_on_slot[i] = new List<string>();
        }
    }
}

public class CW_ItemMaterialLibrary : CW_Library<CW_ItemMaterialAsset>
{
    [Hotfixable]
    public override void init()
    {
        base.init();
    }

    public override void post_init()
    {
        base.post_init();
        foreach (CW_ItemMaterialAsset item_material_asset in list)
        {
            item_material_asset.Element.ComputeType();
        }
    }
}