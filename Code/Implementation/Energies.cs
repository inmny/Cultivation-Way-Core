using Cultivation_Way.Constants;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Implementation;

internal static class Energies
{
    private static readonly Color wakan_level_0 = Color.white;
    private static readonly Color wakan_level_1 = new(0.38f, 0.71f, 1);
    private static readonly Color wakan_level_2 = Color.magenta;
    private static readonly Color wakan_level_3 = Color.yellow;

    private static readonly Color inforce_level_0 = new(1, 0.31f, 0.26f);
    private static readonly Color inforce_level_1 = Color.red;
    private static readonly Color inforce_level_2 = Color.white;
    private static readonly Color inforce_level_3 = Color.yellow;

    private static readonly Color soul_level_0 = Color.white;
    private static readonly Color soul_level_1 = Color.green;
    private static readonly Color soul_level_2 = Color.blue;
    private static readonly Color soul_level_3 = Color.black;

    public static void init()
    {
        add_wakan();
        add_soul();
        add_inforce();
    }

    private static void add_inforce()
    {
        EnergyAsset energy = new()
        {
            id = Content_Constants.energy_bushido_id,
            type = CultisysType.BODY,
            base_value = 1,
            power_base_value = 1000,
            is_dissociative = false,
            color_calc = (energy, value, density) =>
            {
                Color color_to_ret = density switch
                {
                    1 => Toolbox.blendColor(inforce_level_0, inforce_level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(inforce_level_1, inforce_level_2, 2 - density),
                    <= 3 => Toolbox.blendColor(inforce_level_2, inforce_level_3, 3 - density),
                    _ => inforce_level_3
                };

                return color_to_ret;
            },
            spread_grad_calc = (value, density, target_value, target_density, tile, target_tile) =>
            {
                return target_density - density;
            }
        };
        Library.Manager.energies.add(energy);
    }

    private static void add_soul()
    {
        EnergyAsset energy = new()
        {
            id = Content_Constants.energy_soul_id,
            type = CultisysType.SOUL,
            base_value = 1,
            power_base_value = 1000,
            is_dissociative = false,
            color_calc = (energy, value, density) =>
            {
                Color color_to_ret = density switch
                {
                    1 => Toolbox.blendColor(soul_level_0, soul_level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(soul_level_1, soul_level_2, 2 - density),
                    <= 3 => Toolbox.blendColor(soul_level_2, soul_level_3, 3 - density),
                    _ => soul_level_3
                };

                return color_to_ret;
            }
        };
        Library.Manager.energies.add(energy);
    }

    private static void add_wakan()
    {
        EnergyAsset energy = new()
        {
            id = Content_Constants.energy_wakan_id,
            type = CultisysType.WAKAN,
            base_value = 1,
            power_base_value = 1000,
            is_dissociative = true,
            color_calc = (energy, value, density) =>
            {
                Color color_to_ret = density switch
                {
                    1 => Toolbox.blendColor(wakan_level_0, wakan_level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(wakan_level_1, wakan_level_2, 2 - density),
                    <= 3 => Toolbox.blendColor(wakan_level_2, wakan_level_3, 3 - density),
                    _ => wakan_level_3
                };

                return color_to_ret;
            },
            spread_grad_calc = (value, density, target_value, target_density, tile, target_tile) =>
            {
                return (target_value - value) * 0.2f;
            },
            initializer = (tiles, x, y, width, height) =>
            {
                float x_center = width / 2f;
                float y_center = height / 2f;
                float dx = x - x_center;
                float dy = y - y_center;
                tiles.value = Mathf.Max(0, x_center * y_center - dx * dx - dy * dy);
                //CW_Core.LogInfo($"({x},{y}): {tiles.value}");
            }
        };
        Library.Manager.energies.add(energy);
    }
}