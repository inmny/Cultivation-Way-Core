using System;
using System.Collections.Generic;
using NeoModLoader.api.attributes;

namespace Cultivation_Way.Library;

public class CW_ItemMaterialAsset : ElementalAsset
{
    public BaseStats base_stats = new();
    public List<string>[] possible_spells_on_slot = new List<string>[Enum.GetValues(typeof(EquipmentType)).Length];

    public CW_ItemMaterialAsset()
    {
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
        add(new CW_ItemMaterialAsset
        {
            id = SR.common_metals
        });
        t.base_stats[S.mod_damage] = 0.1f;
        t.base_stats[S.damage] = 5;

        add(new CW_ItemMaterialAsset
        {
            id = SR.silver
        });

        add(new CW_ItemMaterialAsset
        {
            id = SR.mythril
        });

        add(new CW_ItemMaterialAsset
        {
            id = SR.adamantine
        });
    }
}