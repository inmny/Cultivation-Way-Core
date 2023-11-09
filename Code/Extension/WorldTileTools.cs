using Cultivation_Way.Core;

namespace Cultivation_Way.Extension;

public static class WorldTileTools
{
    public static CW_EnergyMapTile get_energy_tile(this WorldTile tile, string energy_id)
    {
        return CW_Core.mod_state.energy_map_manager.maps[energy_id].map[tile.x, tile.y];
    }
}