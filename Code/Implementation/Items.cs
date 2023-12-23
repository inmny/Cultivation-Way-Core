using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using NeoModLoader.General.Game;

namespace Cultivation_Way.Implementation;

internal static class Items
{
    [Hotfixable]
    public static void init()
    {
        add_materials();
        CW_ItemAsset item = new CW_MeleeWeaponAsset("赤血戟", CW_MeleeWeaponType.戟);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "adamantine" }, equipment_value: 100, name_class: "item_class_halberd");
        item.BaseLevel = 1;
        item.MainMaterials[SR.adamantine] = 4;
        item.NecessaryResourceCost[SR.common_metals] = 32;
        item.NecessaryResourceCost[SR.silver] = 8;
        item.BaseSpells.Add("fire_blade");
        item.BaseElement = new CW_Element(new[] { 0, 80, 0, 15, 5 });

        Library.Manager.items.AddCreatableItem(item);

        add_five_mountain_seals();
    }

    private static void add_five_mountain_seals()
    {
        CW_ItemAsset item;
        item = new CW_SpecialWeaponAsset("衡山印", CW_SpecialWeaponType.印);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "base" }, equipment_value: 1000, name_class: "item_class_seal");
        item.BaseLevel = 35;
        item.MainMaterials[SR.stone] = 256;
        item.NecessaryResourceCost[SR.adamantine] = 32;
        item.NecessaryResourceCost[SR.mythril] = 8;
        item.BaseSpells.Add("fall_heng1_mountain");
        item.BaseElement = new CW_Element(new[] { 0, 0, 0, 15, 85 });

        Library.Manager.items.add(item);


        item = new CW_SpecialWeaponAsset("华山印", CW_SpecialWeaponType.印);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "base" }, equipment_value: 1000, name_class: "item_class_seal");
        item.BaseLevel = 35;
        item.MainMaterials[SR.stone] = 256;
        item.NecessaryResourceCost[SR.adamantine] = 32;
        item.NecessaryResourceCost[SR.mythril] = 8;
        item.BaseSpells.Add("fall_hua_mountain");
        item.BaseElement = new CW_Element(new[] { 0, 0, 0, 15, 85 });

        Library.Manager.items.add(item);


        item = new CW_SpecialWeaponAsset("恒山印", CW_SpecialWeaponType.印);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "base" }, equipment_value: 1000, name_class: "item_class_seal");
        item.BaseLevel = 35;
        item.MainMaterials[SR.stone] = 256;
        item.NecessaryResourceCost[SR.adamantine] = 32;
        item.NecessaryResourceCost[SR.mythril] = 8;
        item.BaseSpells.Add("fall_heng2_mountain");
        item.BaseElement = new CW_Element(new[] { 0, 0, 0, 15, 85 });

        Library.Manager.items.add(item);


        item = new CW_SpecialWeaponAsset("泰山印", CW_SpecialWeaponType.印);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "base" }, equipment_value: 1000, name_class: "item_class_seal");
        item.BaseLevel = 35;
        item.MainMaterials[SR.stone] = 256;
        item.NecessaryResourceCost[SR.adamantine] = 32;
        item.NecessaryResourceCost[SR.mythril] = 8;
        item.BaseSpells.Add("fall_tai_mountain");
        item.BaseElement = new CW_Element(new[] { 0, 0, 0, 15, 85 });

        Library.Manager.items.add(item);


        item = new CW_SpecialWeaponAsset("嵩山印", CW_SpecialWeaponType.印);
        item.VanillaAsset =
            ItemAssetCreator.CreateMeleeWeapon(item.id, item.base_stats,
                new List<string> { "base" }, equipment_value: 1000, name_class: "item_class_seal");
        item.BaseLevel = 35;
        item.MainMaterials[SR.stone] = 256;
        item.NecessaryResourceCost[SR.adamantine] = 32;
        item.NecessaryResourceCost[SR.mythril] = 8;
        item.BaseSpells.Add("fall_song_mountain");
        item.BaseElement = new CW_Element(new[] { 0, 0, 0, 15, 85 });

        Library.Manager.items.add(item);
    }

    private static void add_materials()
    {
        CW_ItemMaterialAsset asset;
        asset = Library.Manager.item_materials.add(new CW_ItemMaterialAsset
            (Library.Manager.resources.get(SR.common_metals)));
        asset.base_stats_on_slot[(int)EquipmentType.Weapon][S.damage] = 5;
        asset.base_stats_on_slot[(int)EquipmentType.Weapon][S.mod_damage] = 0.1f;

        asset = Library.Manager.item_materials.add(new CW_ItemMaterialAsset
            (Library.Manager.resources.get(SR.silver)));

        asset = Library.Manager.item_materials.add(new CW_ItemMaterialAsset
            (Library.Manager.resources.get(SR.mythril)));

        asset = Library.Manager.item_materials.add(new CW_ItemMaterialAsset
            (Library.Manager.resources.get(SR.adamantine)));
        asset.base_stats_on_slot[(int)EquipmentType.Weapon][S.damage] = 50;
        asset.base_stats_on_slot[(int)EquipmentType.Weapon][S.mod_damage] = 1f;
    }
}