using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using ReflectionUtility;
using Cultivation_Way.Animation;
using UnityEngine;
using Cultivation_Way.Extensions;

namespace Cultivation_Way.Content
{
    internal static class W_Content_GodPower
    {
        internal static void add_god_powers()
        {
            add_spawn_EasternHuman();
            add_spawn_Yao();

            add_wakan_increase();
            add_wakan_decrease();
        }

        private static void add_wakan_decrease()
        {
            CW_AnimationSetting setting = new CW_AnimationSetting();
            setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            setting.loop_time_limit = 2f;
            setting.frame_action = __wakan_decrease_loop_frame_action;
            setting.end_action = __wakan_decrease_end_action;
            setting.layer_name = "EffectsBack";
            setting.play_direction = AnimationPlayDirection.FORWARD;
            setting.set_trace(AnimationTraceType.NONE);
            CW_EffectManager.instance.load_as_controller("wakan_decrease_anim", "effects/wakan_hole/", anim_limit: 10, controller_setting: setting, base_scale: 1.2f);

            setting.frame_action = __wakan_decrease_end_loop_frame_action; setting.end_action = null;
            setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;

            CW_EffectManager.instance.load_as_controller("wakan_decrease_end_anim", "effects/wakan_hole/", anim_limit: 10, controller_setting: setting, base_scale: 1.2f);
            GodPower power = AssetManager.powers.clone("CW_DecreaseWakan", "sponge");
            power.name = "Decrease Wakan";
            power.click_action = __wakan_decrease;
            power.click_brush_action = (PowerActionWithID)Delegate.Combine(power.click_brush_action, new PowerActionWithID(__spawn_wakan_hole));
            power.click_brush_action = (PowerActionWithID)Delegate.Combine(power.click_brush_action, new PowerActionWithID(__keep_wakan_hole));
           
        }

        private static void add_wakan_increase()
        {
            GodPower power = AssetManager.powers.clone("CW_IncreaseWakan", "_drops");
            power.name = "Increase Wakan";
            power.dropID = "wakan_increase";
            power.click_power_action = new PowerAction((WorldTile pTile, GodPower pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnDrops", pTile, pPower);
            });
            power.click_power_brush_action = new PowerAction((WorldTile pTile, GodPower pPower) =>
            {
                return (bool)AssetManager.powers.CallMethod("loopWithCurrentBrushPower", pTile, pPower);
            });
        }

        private static void add_spawn_Yao()
        {
            GodPower power = AssetManager.powers.clone("spawnYao", "_spawnActor");
            power.name = "Yaos";
            power.spawnSound = "spawnOrc";
            power.actorStatsId = "unit_Yao,";// TODO:其他妖族
            power.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
        }

        private static void add_spawn_EasternHuman()
        {
            GodPower power = AssetManager.powers.clone("spawnEastern_Human", "_spawnActor");
            power.name = "Eastern Humans";
            power.spawnSound = "spawnHuman";
            power.actorStatsId = "unit_eastern_human";
            power.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
        }
        private static CW_SpriteAnimation anim;
        private static bool __spawn_wakan_hole(WorldTile center, string id)
        {
            if (anim == null || !anim.isOn)
            {
                anim = CW_EffectManager.instance.spawn_anim("wakan_decrease_anim", center, center, null, null, ((int)Reflection.GetField(typeof(BrushData), Config.currentBrushData, "size")) / 20f);
            }
            return true;
        }
        private static bool __keep_wakan_hole(WorldTile center, string id)
        {
            if (anim == null || !anim.isOn || center ==null) return false;
            anim.set_position(center.posV);
            anim.play_time -= Time.deltaTime;
            float total_level = 0;
            foreach(BrushPixelData pixel in Config.currentBrushData.pos)
            {
                WorldTile tile = MapBox.instance.GetTile(center.x+pixel.x, center.y+pixel.y);
                if (tile == null) continue;
                total_level += tile.get_cw_chunk().wakan_level;
            }
            anim.set_alpha(Mathf.Max(0.8f,Mathf.Min(Mathf.Sqrt((total_level / Config.currentBrushData.pos.Count - 1)*5), anim.renderer.color.a)));
            //anim.force_update(Time.deltaTime);
            MapBox.instance.loopWithBrush(center, Config.currentBrushData, AssetManager.powers.get(id).click_action, id);
            return true;
        }
        private static bool __wakan_decrease(WorldTile center, string id)
        {
            if (center == null) return false;
            CW_MapChunk chunk = center.get_cw_chunk();
            if (chunk.wakan <= 1) return true;

            if (chunk.wakan_level > 1.2f) chunk.wakan_level *= 0.999f;
            else chunk.wakan *= 0.5f;

            chunk.update(true);
            return true;
        }
        private static void __wakan_decrease_loop_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            //WorldBoxConsole.Console.print("Current frame_idx:" + cur_frame_idx);
            if (cur_frame_idx == 17) { anim.cur_frame_idx = 6;}
        }
        private static void __wakan_decrease_end_loop_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if(cur_frame_idx == 17) { anim.cur_frame_idx = 6; anim.setting.play_direction = AnimationPlayDirection.BACKWARD; }
        }
        private static void __wakan_decrease_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation origin_anim)
        {
            anim = CW_EffectManager.instance.spawn_anim("wakan_decrease_end_anim", origin_anim.gameObject.transform.localPosition, origin_anim.gameObject.transform.localPosition, null, null, 1f);
            if (cur_frame_idx > 6) anim.setting.play_direction = AnimationPlayDirection.FORWARD;
            anim.set_position(origin_anim.gameObject.transform.localPosition);
            anim.cur_frame_idx = cur_frame_idx;
            anim.set_alpha(origin_anim.renderer.color.a);
            anim.gameObject.transform.localScale = origin_anim.gameObject.transform.localScale;
        }
    }
}
