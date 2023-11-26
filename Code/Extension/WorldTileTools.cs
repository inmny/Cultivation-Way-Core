using Cultivation_Way.Core;

namespace Cultivation_Way.Extension;

public static class WorldTileTools
{
    public static CW_EnergyMapTile GetEnergyTile(this WorldTile pTile, string pEnergyID)
    {
        return CW_Core.mod_state.energy_map_manager.maps[pEnergyID].map[pTile.x, pTile.y];
    }
}