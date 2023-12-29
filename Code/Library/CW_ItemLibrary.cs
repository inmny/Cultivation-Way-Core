using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
namespace Cultivation_Way.Library;

public class CW_ItemAsset : Asset
{
    public readonly HashSet<string> AllowedSpellClasses = new();
    public readonly HashSet<string> BaseSpells = new();

    /// <summary>
    ///     主要材料, 用于确定物品的图标样式
    /// </summary>
    public readonly Dictionary<string, int> MainMaterials = new();

    /// <summary>
    ///     必需的材料消耗
    /// </summary>
    public readonly Dictionary<string, int> NecessaryResourceCost = new();

    public readonly List<Dictionary<string, int>>[] ResourceCostListsPerLevel =
        new List<Dictionary<string, int>>[Constants.Core.item_level_count];

    public BaseStats base_stats = new();
    public CW_Element BaseElement = new(new[]
    {
        20, 20, 20, 20, 20
    });
    public int BaseLevel = 0;
    public ItemAsset VanillaAsset;

    public CW_ItemAsset()
    {
        SetDefaultResourceCostPerLevel();
    }

    public CW_ItemAsset(string id)
    {
        this.id = id;
        SetDefaultResourceCostPerLevel();
    }

    public CW_ItemType ItemType { get; protected set; }

    private void SetDefaultResourceCostPerLevel()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            ResourceCostListsPerLevel[i] = new List<Dictionary<string, int>>();
            var default_cost = new Dictionary<string, int>();

            switch (i / Constants.Core.item_level_per_stage)
            {
                case 0:
                    default_cost.Add(SR.common_metals, i % Constants.Core.item_level_per_stage);
                    break;
                case 1:
                    default_cost.Add(SR.common_metals,
                        i % Constants.Core.item_level_per_stage * 2 + Constants.Core.item_level_per_stage);
                    break;
                case 2:
                    default_cost.Add(SR.silver, i % Constants.Core.item_level_per_stage);
                    break;
                case 3:
                    default_cost.Add(SR.mythril, i % Constants.Core.item_level_per_stage);
                    break;
                case 4:
                    default_cost.Add(SR.adamantine, i % Constants.Core.item_level_per_stage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            ResourceCostListsPerLevel[i].Add(default_cost);
        }
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public string GetTypeName()
    {
        return ItemType switch
        {
            CW_ItemType.Weapon => (this as CW_WeaponItemAsset).WeaponType switch
            {
                CW_WeaponType.Melee => (this as CW_MeleeWeaponAsset).type.ToString(),
                CW_WeaponType.Range => (this as CW_RangeWeaponAsset).type.ToString(),
                CW_WeaponType.Special => (this as CW_SpecialWeaponAsset).type.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            },
            CW_ItemType.Armor => (this as CW_ItemArmorAsset).ArmorType switch
            {
                CW_ArmorType.Helmet => (this as CW_HelmetArmorAsset).type.ToString(),
                CW_ArmorType.Breastplate => (this as CW_BreastplateArmorAsset).type.ToString(),
                CW_ArmorType.Boots => (this as CW_BootsArmorAsset).type.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            },
            CW_ItemType.Accessory => (this as CW_ItemAccessoryAsset).AccessoryType switch
            {
                CW_AccessoryType.Ring => (this as CW_RingAccessoryAsset).type.ToString(),
                CW_AccessoryType.Amulet => (this as CW_AmuletAccessoryAsset).type.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void ClearCurrentPerLevelCosts()
    {
        for (int i = 0; i < Constants.Core.item_level_count; i++)
        {
            ResourceCostListsPerLevel[i].Clear();
        }
    }
}

public class CW_ItemLibrary : CW_Library<CW_ItemAsset>
{
    private readonly HashSet<CW_ItemAsset> creatable_items = new();
    public bool IsCreatable(CW_ItemAsset item)
    {
        return creatable_items.Contains(item);
    }
    public CW_ItemAsset FindAssetToCraft(Actor pActor)
    {
        return creatable_items.GetRandom();
    }

    public void AddCreatableItem(CW_ItemAsset item)
    {
        creatable_items.Add(item);
        if (!dict.ContainsKey(item.id))
        {
            add(item);
        }
    }
}