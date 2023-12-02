using Cultivation_Way.Core;
using Cultivation_Way.Implementation;

namespace Cultivation_Way.Test;

internal static class WakanInitAndSpread
{
    public static void ClearWakan()
    {
        CW_EnergyMap map = CW_Core.mod_state.energy_map_manager.maps[Content_Constants.energy_wakan_id];

        for (int i = 0; i < map.map.GetLength(0); i++)
        {
            for (int j = 0; j < map.map.GetLength(1); j++)
            {
                map.map[i, j].value = 0;
                map.map[i, j].Update(map.energy);
            }
        }
    }
}