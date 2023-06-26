using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal static class Energy
    {
        private static Color level_0 = Color.white;
        private static Color level_1 = new(0.38f, 0.71f, 1);
        private static Color level_2 = Color.magenta;
        private static Color level_3 = Color.yellow;
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
                type = Constants.CultisysType.BODY,
                base_value = 1,
                power_base_value = 1000,
                is_dissociative = false,
                color_calc = (energy, value, density, power_level) =>
                {
                    Color color_to_ret = level_3;
                    if (power_level == 1)
                    {
                        color_to_ret = Toolbox.blendColor(level_0, level_1, 100 / (100 + value));
                    }
                    else if (power_level <= 2)
                    {
                        color_to_ret = Toolbox.blendColor(level_1, level_2, 2 - power_level);
                    }
                    else if (power_level <= 3)
                    {
                        color_to_ret = Toolbox.blendColor(level_2, level_3, 3 - power_level);
                    }
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
                type = Constants.CultisysType.SOUL,
                base_value = 1,
                power_base_value = 1000,
                is_dissociative = false,
                color_calc = (energy, value, density, power_level) =>
                {
                    Color color_to_ret = level_3;
                    if (power_level == 1)
                    {
                        color_to_ret = Toolbox.blendColor(level_0, level_1, 100 / (100 + value));
                    }
                    else if (power_level <= 2)
                    {
                        color_to_ret = Toolbox.blendColor(level_1, level_2, 2 - power_level);
                    }
                    else if (power_level <= 3)
                    {
                        color_to_ret = Toolbox.blendColor(level_2, level_3, 3 - power_level);
                    }
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
                type = Constants.CultisysType.WAKAN,
                base_value = 1,
                power_base_value = 1000,
                is_dissociative = true,
                color_calc = (energy, value, density, power_level) =>
                {
                    Color color_to_ret = level_3;
                    if (power_level == 1)
                    {
                        color_to_ret = Toolbox.blendColor(level_0, level_1, 100 / (100 + value));
                    }
                    else if (power_level <= 2)
                    {
                        color_to_ret = Toolbox.blendColor(level_1, level_2, 2 - power_level);
                    }
                    else if (power_level <= 3)
                    {
                        color_to_ret = Toolbox.blendColor(level_2, level_3, 3 - power_level);
                    }
                    return color_to_ret;
                }
            };
            Library.Manager.energies.add(energy);
        }
    }
}
