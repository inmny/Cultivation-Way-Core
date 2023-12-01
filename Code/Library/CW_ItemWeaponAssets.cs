using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;


public class CW_MeleeWeaponAsset : CW_WeaponItemAsset
{
    public CW_MeleeWeaponType type;
    
    public CW_MeleeWeaponAsset(string id, CW_MeleeWeaponType type) : base(id)
    {
        this.type = type;
        this.WeaponType = CW_WeaponType.Melee;
    }
}
public class CW_RangeWeaponAsset : CW_WeaponItemAsset
{
    public CW_RangeWeaponType type;

    public CW_RangeWeaponAsset(string id, CW_RangeWeaponType type) : base(id)
    {
        this.type = type;
        WeaponType = CW_WeaponType.Range;
    }
}
public class CW_SpecialWeaponAsset : CW_WeaponItemAsset
{
    public CW_SpecialWeaponType type;

    public CW_SpecialWeaponAsset(string id, CW_SpecialWeaponType type) : base(id)
    {
        this.type = type;
        WeaponType = CW_WeaponType.Special;
    }
}

public abstract class CW_WeaponItemAsset : CW_ItemAsset
{
    public CW_WeaponType WeaponType { get; protected set; }
    public CW_WeaponItemAsset(string id) : base(id)
    {
        ItemType = CW_ItemType.Weapon;
    }
}