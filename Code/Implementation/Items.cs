using System.Collections.Generic;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal static class Items
{
    public static void init()
    {
        CW_ItemAsset item = new();
        item.id = "test";
        item.vanilla_asset =
            NeoModLoader.General.Game.ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "iron" });
        item.base_level = 1;
        item.necessary_resource_cost[SR.common_metals] = 1;
        Library.Manager.items.add(item);
    }
}