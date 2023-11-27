using System.Collections.Generic;

namespace Cultivation_Way.Library;

public class CW_ItemAsset : Asset
{
    public int base_level = 0;
    public BaseStats base_stats = new();
    public HashSet<string> base_spells = new();
    public Dictionary<string, int> necessary_resource_cost = new();
}
public class CW_ItemLibrary : CW_Library<CW_ItemAsset>
{
    
}