using System.Collections.Generic;
using Cultivation_Way.Library;

namespace Cultivation_Way.Core;

public class CW_ItemData : BaseSystemData
{
    public int Level { get; private set; }
    public CW_ItemAsset FAsset { get; private set; }
    public ItemData VanillaData { get; private set; }
    public HashSet<string> Spells { get; private set; }

    public CW_ItemData(CW_ItemAsset pAsset)
    {
        FAsset = pAsset;
        Level = pAsset.base_level;
        Spells = new HashSet<string>(pAsset.base_spells);
        VanillaData = new ItemData();
    }
}