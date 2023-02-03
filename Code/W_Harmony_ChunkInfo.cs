using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using HarmonyLib;
using Cultivation_Way.Extensions;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_ChunkInfo
    {
        private static string color = "#6185FF";
        private static GroupSpriteObject last_object;
        // Patch于checkUnitGroups之后只为了在最后的update之前执行
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DebugTextGroupSystem), "checkUnitGroups")]
        public static void checkChunk(DebugTextGroupSystem __instance)
        {
            if (last_object != null) last_object.setScale(new UnityEngine.Vector3(1,1,0));
            if (!Utils.CW_Utils_Others.is_map_mode_active("map_wakan_zones")) return;

            WorldTile tile = MapBox.instance.getMouseTilePos();
            if (tile == null) return;
            CW_MapChunk chunk = tile.get_cw_chunk();

            GroupSpriteObject _object = __instance.CallMethod("getNext") as GroupSpriteObject;
            DebugWorldText world_text = _object.gameObject.GetComponent<DebugWorldText>();
            world_text.CallMethod("prepare", "区块灵气等", color, 0.2f);
            world_text.CallMethod("add", "级", ((int)(100 * chunk.wakan_level)) / 100f);
            world_text.CallMethod("add", "灵气量", chunk.wakan);
            world_text.CallMethod("fin");
            UnityEngine.Vector3 pos = MapBox.instance.getMousePos();
            float scale = (float)Reflection.GetField(typeof(QualityChanger), Animation.CW_EffectManager.quality_changer, "currentZoom") / 3;
            pos.y += 2 * scale;
            pos.y = Math.Min(pos.y, 280);
            _object.setPosOnly(ref pos);

            
            _object.setScale(new UnityEngine.Vector3(scale, scale, 0));
            last_object = _object;
        }
    }
}
