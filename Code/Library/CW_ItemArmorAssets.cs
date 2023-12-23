using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;

public class CW_HelmetArmorAsset : CW_ItemArmorAsset
{
    public CW_HelmetArmorType type;

    public CW_HelmetArmorAsset(string id, CW_HelmetArmorType type) : base(id)
    {
        this.type = type;
        ArmorType = CW_ArmorType.Helmet;
    }
}

public class CW_BreastplateArmorAsset : CW_ItemArmorAsset
{
    public CW_BreastplateArmorType type;

    public CW_BreastplateArmorAsset(string id, CW_BreastplateArmorType type) : base(id)
    {
        this.type = type;
        ArmorType = CW_ArmorType.Breastplate;
    }
}

public class CW_BootsArmorAsset : CW_ItemArmorAsset
{
    public CW_BootsArmorType type;

    public CW_BootsArmorAsset(string id, CW_BootsArmorType type) : base(id)
    {
        this.type = type;
        ArmorType = CW_ArmorType.Boots;
    }
}

public abstract class CW_ItemArmorAsset : CW_ItemAsset
{
    public CW_ItemArmorAsset(string id) : base(id)
    {
        ItemType = CW_ItemType.Armor;
    }

    public CW_ArmorType ArmorType { get; protected set; }
}