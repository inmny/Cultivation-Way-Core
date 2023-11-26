using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using UnityEngine;

namespace Cultivation_Way.General.AboutSpell;

public static class MiscUtils
{
    public static float WakanCostToDamage(float pWakanCost, BaseSimObject pSimObject)
    {
        pWakanCost *= pSimObject.stats[CW_S.wakan];
        if (pSimObject.a != null)
        {
            CW_Actor actor = (CW_Actor)pSimObject.a;
            var cultisys = actor.data.GetCultisys(CultisysType.WAKAN);
            if (cultisys != null)
            {
                actor.data.get(cultisys.id, out int level, 0);
                return pWakanCost * Mathf.Pow(cultisys.power_base, cultisys.power_level[level] - 1);
            }
            else
            {
                return pWakanCost;
            }
        }
        else
        {
            return pWakanCost;
        }
    }
}