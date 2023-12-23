using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;

public class CW_RingAccessoryAsset : CW_ItemAccessoryAsset
{
    public CW_RingAccessoryType type;

    public CW_RingAccessoryAsset(string id, CW_RingAccessoryType type) : base(id)
    {
        this.type = type;
        AccessoryType = CW_AccessoryType.Ring;
    }
}

public class CW_AmuletAccessoryAsset : CW_ItemAccessoryAsset
{
    public CW_AmuletAccessoryType type;

    public CW_AmuletAccessoryAsset(string id, CW_AmuletAccessoryType type) : base(id)
    {
        this.type = type;
        AccessoryType = CW_AccessoryType.Amulet;
    }
}

public abstract class CW_ItemAccessoryAsset : CW_ItemAsset
{
    public CW_ItemAccessoryAsset(string id) : base(id)
    {
        ItemType = CW_ItemType.Accessory;
    }

    public CW_AccessoryType AccessoryType { get; protected set; }
}