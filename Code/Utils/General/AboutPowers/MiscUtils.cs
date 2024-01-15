using System.Collections.Generic;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using UnityEngine;
namespace Cultivation_Way.Utils.General.AboutPowers;

public static class MiscUtils
{

    private static readonly Queue<WakanIncreaseDropData> wakan_increase_drop_data_queue = new();
    private static DropAsset CodeWakanIncreaseDrop = AssetManager.drops.add(new DropAsset
    {
        id = "code_wakan_increase",
        path_texture = "drops/drop_lava",
        random_frame = true,
        animated = true,
        animation_speed = 0.03f,
        default_scale = 0.2f,
        action_landed = (tile, id) =>
        {
            if (tile == null) return;
            WakanIncreaseDropData data = wakan_increase_drop_data_queue.Dequeue();
            EnergyAsset energy_asset = Manager.energies.get(data.energy_id);
            if (energy_asset == null) return;
            CW_EnergyMapTile energy_tile = tile.GetEnergyTile(data.energy_id);

            float scale = data.scale;
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
            energy_tile.Update(energy_asset, true);
        }
    });
    public static void WakanIncreaseDrop(WorldTile pTile, string pEnergyID, float pScale = 1)
    {
        wakan_increase_drop_data_queue.Enqueue(new WakanIncreaseDropData());
    }

    private struct WakanIncreaseDropData
    {
        public float scale;
        public string energy_id;
        public int x;
        public int y;
    }
}