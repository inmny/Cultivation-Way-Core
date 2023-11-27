using System.Collections.Generic;

namespace Cultivation_Way.Library;

internal static class CW_CultureTechLibrary
{
    public static void init()
    {
        CultureTechAsset tech = new()
        {
            id = "smelt_mill",
            path_icon = "tech/icon_tech_weaponsmith",
            knowledge_cost = 2f,
            type = TechType.Rare,
            requirements = new List<string>
            {
                "weaponsmith", "armorsmith"
            }
        };
        AssetManager.culture_tech.add(tech);
    }
}