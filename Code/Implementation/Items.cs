using System.Collections.Generic;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal static class Items
{
    public static void init()
    {
        CW_ItemAsset item = new();
        item.id = "赤血戟"; // 虽然用中文作为id不太合适, 但着实不想翻译了
        item.vanilla_asset =
            NeoModLoader.General.Game.ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "adamantine" }, equipment_value: 100, name_class: "item_class_halberd");
        item.base_level = 1;
        item.main_material = SR.adamantine;
        item.necessary_resource_cost[SR.common_metals] = 32;
        item.necessary_resource_cost[SR.silver] = 8;
        item.necessary_resource_cost[SR.adamantine] = 4;
        item.base_spells.Add("fire_blade");
        
        Library.Manager.items.add(item);
    }
}