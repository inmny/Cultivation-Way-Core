using System.Collections.Generic;

namespace Cultivation_Way.Utils.General.AboutBlood;

public static class MiscUtils
{
    public static KeyValuePair<string, float> MaxBlood(Dictionary<string, float> pBlood)
    {
        KeyValuePair<string, float> max = new("", 0);
        foreach (var blood in pBlood)
            if (blood.Value > max.Value)
                max = blood;

        return max;
    }
}