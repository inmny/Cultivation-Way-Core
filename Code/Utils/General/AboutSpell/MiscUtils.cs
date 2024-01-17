using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.Utils.General.AboutSpell;

public static class MiscUtils
{
    [Hotfixable]
    public static float WakanCostToDamage(float pWakanCost, BaseSimObject pSimObject)
    {
        if (pSimObject.a == null)
        {
            return pWakanCost;
        }

        CultisysAsset cultisys = pSimObject.a.data.GetCultisys(CultisysType.WAKAN);
        if (cultisys == null)
        {
            return pWakanCost;
        }

        pSimObject.base_data.get(cultisys.id, out int level);

        return pWakanCost * Mathf.Pow(cultisys.power_base, cultisys.power_level[level]);
    }
}