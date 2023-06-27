using Cultivation_Way.Constants;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Content;

internal static class Energy
{
    private static readonly Color level_0 = Color.white;
    private static readonly Color level_1 = new(0.38f, 0.71f, 1);
    private static readonly Color level_2 = Color.magenta;
    private static readonly Color level_3 = Color.yellow;

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
            color_calc = (energy, value, density, power_level) =>
            {
                Color color_to_ret = power_level switch
                {
                    1 => Toolbox.blendColor(level_0, level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(level_1, level_2, 2 - power_level),
                    <= 3 => Toolbox.blendColor(level_2, level_3, 3 - power_level),
                    _ => level_3
                };

                return color_to_ret;
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
            color_calc = (energy, value, density, power_level) =>
            {
                Color color_to_ret = power_level switch
                {
                    1 => Toolbox.blendColor(level_0, level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(level_1, level_2, 2 - power_level),
                    <= 3 => Toolbox.blendColor(level_2, level_3, 3 - power_level),
                    _ => level_3
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
            color_calc = (energy, value, density, power_level) =>
            {
                Color color_to_ret = power_level switch
                {
                    1 => Toolbox.blendColor(level_0, level_1, 100 / (100 + value)),
                    <= 2 => Toolbox.blendColor(level_1, level_2, 2 - power_level),
                    <= 3 => Toolbox.blendColor(level_2, level_3, 3 - power_level),
                    _ => level_3
                };

                return color_to_ret;
            }
        };
        Library.Manager.energies.add(energy);
    }
}