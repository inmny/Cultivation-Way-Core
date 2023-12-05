using System;
using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Animation;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using NeoModLoader.General;
using UnityEngine;

namespace Cultivation_Way.Library;

internal static class CW_GodPowers
{
    private static readonly List<string> _energy_map_ids = new();

    private static Animation.SpriteAnimation anim;

    public static void init()
    {
        add_more_map_modes_switch();
        add_energy_increase();
        add_energy_decrease();
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
        global_switch.id = Constants.Core.energy_maps_toggle_name;
        global_switch.name = "Energy Maps Layer";
        global_switch.unselectWhenWindow = true;
        global_switch.map_modes_switch = true;
        global_switch.toggle_name = Constants.Core.energy_maps_toggle_name;
        if (!PlayerConfig.dict.ContainsKey(Constants.Core.energy_maps_toggle_name))
        {
            PlayerConfig.instance.data.add(new PlayerOptionData(Constants.Core.energy_maps_toggle_name)
            {
                stringVal = "",
                boolVal = false
            });
        }

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

            WorldTip.instance.showToolbarText(LM.Get(data.stringVal));
            PlayerConfig.saveData();
        };
        add(global_switch);
    }

    private static void add_energy_decrease()
    {
        AnimationSetting setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.TIME_LIMIT,
            loop_time_limit = 2f,
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx == 17) anim.cur_frame_idx = 6;
            },
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation origin_anim) =>
            {
                anim = EffectManager.instance.spawn_anim("wakan_decrease_end_anim",
                    origin_anim.gameObject.transform.localPosition, origin_anim.gameObject.transform.localPosition);
                if (idx > 6) anim.setting.play_direction = AnimationPlayDirection.FORWARD;
                anim.set_position(origin_anim.gameObject.transform.localPosition);
                anim.cur_frame_idx = idx;
                anim.gameObject.transform.localScale = origin_anim.gameObject.transform.localScale;
            },
            layer_name = "EffectsBack",
            play_direction = AnimationPlayDirection.FORWARD,
            visible_in_low_res = true
        };
        setting.set_trace(AnimationTraceType.NONE);
        EffectManager.instance.load_as_controller("wakan_decrease_anim", "effects/wakan_hole/", 10,
            controller_setting: setting, base_scale: 1.2f);

        setting.frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation animation) =>
        {
            if (idx == 17)
            {
                anim.cur_frame_idx = 6;
                anim.setting.play_direction = AnimationPlayDirection.BACKWARD;
            }
        };
        setting.end_action = null;
        setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;

        EffectManager.instance.load_as_controller("wakan_decrease_end_anim", "effects/wakan_hole/", 10,
            controller_setting: setting, base_scale: 1.2f);
        GodPower power = AssetManager.powers.clone(Constants.Core.power_energy_decrease, "sponge");
        power.name = "Decrease Wakan";
        power.click_action = (tile, id) =>
        {
            if (!PlayerConfig.optionBoolEnabled(Constants.Core.energy_maps_toggle_name)
                || string.IsNullOrEmpty(CW_Core.mod_state.energy_map_manager.current_map_id))
                return false;
            if (tile == null) return false;
            EnergyAsset energy_asset = Manager.energies.get(CW_Core.mod_state.energy_map_manager.current_map_id);
            CW_EnergyMapTile energy_tile = tile.GetEnergyTile(energy_asset.id);

            lock (energy_tile)
            {
                if (energy_tile.value is <= 1 or float.NaN)
                {
                    energy_tile.value = 0;
                    return true;
                }

                energy_tile.value *=
                    1 - CW_Core.Instance.GetConfig()["worldlaw_energy_grid"]["energy_change_scale"].FloatVal * 0.01f;
                energy_tile.Update(energy_asset);
            }

            return true;
        };
        power.click_brush_action = (PowerActionWithID)Delegate.Combine(power.click_brush_action, new PowerActionWithID(
            (tile, id) =>
            {
                if (anim == null || !anim.isOn)
                {
                    anim = EffectManager.instance.spawn_anim("wakan_decrease_anim", tile, tile, null, null,
                        Config.currentBrushData.size / 20f);
                }

                return true;
            }));
        power.click_brush_action = (PowerActionWithID)Delegate.Combine(power.click_brush_action, new PowerActionWithID(
            (center, id) =>
            {
                if (anim == null || !anim.isOn || center == null) return false;
                anim.set_position(center.posV);
                anim.play_time -= Time.deltaTime;
                anim.play_time = Mathf.Max(anim.play_time, 0.1f);
                MapBox.instance.loopWithBrush(center, Config.currentBrushData, AssetManager.powers.get(id).click_action,
                    id);
                return true;
            }));
    }

    private static void add_energy_increase()
    {
        GodPower power = AssetManager.powers.clone(Constants.Core.power_energy_increase, "_drops");
        power.name = "Increase Wakan";
        power.dropID = "wakan_increase";
        power.click_power_action = (pTile, pPower)
            => AssetManager.powers.spawnDrops(pTile, pPower);
        power.click_power_brush_action =
            (pTile, pPower) => AssetManager.powers.loopWithCurrentBrushPower(pTile, pPower);
    }

    private static void add(GodPower power)
    {
        AssetManager.powers.add(power);
    }
}