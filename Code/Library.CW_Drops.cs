using Cultivation_Way.Core;
using Cultivation_Way.Extension;

namespace Cultivation_Way.Library;

internal static class CW_Drops
{
    public static void init()
    {
        add_wakan_increase_drop();
    }

    private static void add_wakan_increase_drop()
    {
        AssetManager.drops.add(new DropAsset
        {
            id = "wakan_increase",
            path_texture = "drops/drop_lava",
            random_frame = true,
            animated = true,
            animation_speed = 0.03f,
            default_scale = 0.2f,
            action_landed = (tile, id) =>
            {
                if (tile == null) return;
                if (!PlayerConfig.optionBoolEnabled(Constants.Core.energy_maps_toggle_name)
                    || string.IsNullOrEmpty(CW_Core.mod_state.map_chunk_manager.current_map_id))
                    return;
                EnergyAsset energy_asset = Manager.energies.get(CW_Core.mod_state.map_chunk_manager.current_map_id);
                CW_EnergyMapTile energy_tile = tile.get_energy_tile(energy_asset.id);
                energy_tile.value += 1f;
                energy_tile.value *= 1.1f;
                energy_tile.update(energy_asset);
            }
        });
    }
}