using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using UnityEngine;

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
                    || string.IsNullOrEmpty(CW_Core.mod_state.energy_map_manager.current_map_id))
                    return;
                EnergyAsset energy_asset = Manager.energies.get(CW_Core.mod_state.energy_map_manager.current_map_id);
                CW_EnergyMapTile energy_tile = tile.GetEnergyTile(energy_asset.id);
                float scale = CW_Core.Instance.GetConfig()["worldlaw_energy_grid"]["energy_change_scale"].FloatVal;

                float new_value = Mathf.Min((energy_tile.value + scale) * 1.1f * scale, 1e15f);

                if (new_value is float.NaN)
                {
                    new_value = 1e15f;
                }
                else if (new_value < 0)
                {
                    new_value = 0;
                }

                energy_tile.UpdateValue(new_value);
                energy_tile.Update(energy_asset);
            }
        });
    }
}