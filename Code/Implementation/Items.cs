using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using NeoModLoader.General.Game;

namespace Cultivation_Way.Implementation;

internal static class Items
{
    [Hotfixable]
    public static void init()
    {
        CW_ItemAsset item = new CW_MeleeWeaponAsset("赤血戟", CW_MeleeWeaponType.戟);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "adamantine" }, equipment_value: 100, name_class: "item_class_halberd");
        item.BaseLevel = 1;
        item.MainMaterial = SR.adamantine;
        item.NecessaryResourceCost[SR.common_metals] = 32;
        item.NecessaryResourceCost[SR.silver] = 8;
        item.NecessaryResourceCost[SR.adamantine] = 4;
        item.BaseSpells.Add("fire_blade");

        Library.Manager.items.add(item);
    }
}