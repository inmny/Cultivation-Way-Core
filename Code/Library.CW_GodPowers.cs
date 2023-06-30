using System.Collections.Generic;
using System.Linq;

namespace Cultivation_Way.Library;

internal static class CW_GodPowers
{
    private static readonly List<string> _energy_map_ids = new();

    public static void init()
    {
        add_more_map_modes_switch();
    }

    public static void post_init()
    {
        foreach (EnergyAsset energy_asset in Manager.energies.list.Where(energy_asset => energy_asset.is_dissociative))
        {
            _energy_map_ids.Add(energy_asset.id);
        }
    }

    private static void add_more_map_modes_switch()
    {
        GodPower global_switch = new();
        global_switch.id = Constants.Core.mod_prefix + "energy_maps_switch";
        global_switch.name = "Energy Maps Layer";
        global_switch.unselectWhenWindow = true;
        global_switch.map_modes_switch = true;
        global_switch.toggle_name = Constants.Core.energy_maps_toggle_name;
        global_switch.toggle_action = power_id =>
        {
            PlayerOptionData data = PlayerConfig.dict[global_switch.toggle_name];
            if (!data.boolVal)
            {
                if (_energy_map_ids.Count > 0)
                {
                    data.boolVal = true;
                    data.stringVal = _energy_map_ids[0];
                    AssetManager.powers.disableAllOtherMapModes(power_id);
                }
                else
                {
                    WorldTip.instance.showToolbarText("?????");
                    return;
                }
            }
            else
            {
                int curr_idx = _energy_map_ids.IndexOf(data.stringVal);
                curr_idx = (curr_idx + 1) % _energy_map_ids.Count;
                if (curr_idx == 0)
                {
                    data.boolVal = false;
                }
                else
                {
                    data.stringVal = _energy_map_ids[curr_idx];
                }
            }

            WorldTip.instance.showToolbarText(data.stringVal);
            PlayerConfig.saveData();
        };
        add(global_switch);
    }

    private static void add(GodPower power)
    {
        AssetManager.powers.add(power);
    }
}