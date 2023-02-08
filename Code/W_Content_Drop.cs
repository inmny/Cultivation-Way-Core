using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Extensions;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Drop
    {
        internal static void add_drops()
        {
            add_wakan_increase();
            //add_wakan_decrease();
        }

        private static void add_wakan_decrease()
        {
            AssetManager.drops.add(new DropAsset()
            {
                id = "wakan_decrease",
                path_texture = "drops/drop_delta_rain",
                random_frame = true,
                default_scale = 0.1f,
                action_landed = wakan_decrease
            });
        }

        private static void add_wakan_increase()
        {
            AssetManager.drops.add(new DropAsset()
            {
                id = "wakan_increase",
                path_texture = "drops/drop_lava",
                random_frame = true,
                animated = true,
                animation_speed = 0.03f,
                default_scale = 0.2f,
                action_landed = wakan_increase
            });
        }
        private static void wakan_increase(WorldTile tile, string drop_id)
        {
            if (tile == null) return;
            CW_MapChunk chunk = tile.get_cw_chunk();
            if (chunk.wakan_level >= 3) return;
            chunk.wakan_level += 0.1f;
            if (chunk.wakan_level >= 3)
            {
                chunk.wakan_level = 3;
            }
            chunk.wakan = UnityEngine.Mathf.Max(1, chunk.wakan);
            chunk.update(true);
            Harmony.W_Harmony_MapMode.force_update();
        }
        private static void wakan_decrease(WorldTile tile, string drop_id)
        {
            if (tile == null) return;
            CW_MapChunk chunk = tile.get_cw_chunk();
            if (chunk.wakan <= 1) return;

            if (chunk.wakan_level > 1.2f) chunk.wakan_level -= 0.2f;
            else chunk.wakan *= 0.5f;
            
            chunk.update(true);
        }
    }
}
