using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.General.AboutSpell;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way.Content;

internal static class Spells
{
    public static void init()
    {
        // TODO: 调整法术消耗
        add_track_projectile_spells();
        add_give_self_status_spells();
        add_blade_spells();
    }

    /// <summary>
    ///     添加金刃相似法术
    /// </summary>
    private static void add_blade_spell(
        string spell_id, int rarity,
        int[] element,
        string anim_id, string anim_path,
        KeyValuePair<string, float>[] spell_cost_list,
        float range, AnimFrameAction anim_frame_action)
    {
        CW_SpellAsset spell_asset = new()
        {
            id = spell_id, rarity = rarity, element = new CW_Element(element),
            anim_id = anim_id, anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY, target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell_asset.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell_asset.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell_asset);

        AnimationSetting anim_setting = new()
        {
            point_to_dst = true,
            always_point_to_dst = false,
            loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT,
            loop_trace_limit = range,
            anim_froze_frame_idx = 3,
            frame_interval = 0.05f,
            trace_grad = 15f,
            frame_action = anim_frame_action
        };
        anim_setting.set_trace(AnimationTraceType.LINE);
        CW_Core.mod_state.anim_manager.load_as_controller(
            anim_id, anim_path, controller_setting: anim_setting, base_scale: 0.08f
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_blade_spells()
    {
        add_blade_spell(
            "gold_blade", 3, new[] { 0, 0, 100, 0, 0 },
            "gold_blade_anim", "effects/gold_blade", new KeyValuePair<string, float>[]
            {
                new(CW_S.wakan, Content_Constants.default_spell_cost)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        add_blade_spell(
            "water_blade", 3, new[] { 100, 0, 0, 0, 0 },
            "water_blade_anim", "effects/water_blade", new KeyValuePair<string, float>[]
            {
                new(CW_S.wakan, Content_Constants.default_spell_cost)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);

                    float dist = Toolbox.DistVec2Float(vec, dst_vec);
                    float force_x = (dst_vec.x - vec.x) / dist * 0.4f;
                    float force_y = (dst_vec.y - vec.y) / dist * 0.4f;
                    float force_z = 0.1f;
                    foreach (Actor target_unit in tiles.SelectMany(target_tile => target_tile._units))
                    {
                        target_unit.addForce(force_x, force_y, force_z);
                    }
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        // TODO: 补充火刃燃烧效果
        add_blade_spell(
            "fire_blade", 3, new[] { 0, 100, 0, 0, 0 },
            "fire_blade_anim", "effects/fire_blade", new KeyValuePair<string, float>[]
            {
                new(CW_S.wakan, Content_Constants.default_spell_cost)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        add_blade_spell(
            "wind_blade", 3, new[] { 40, 40, 20, 0, 0 },
            "wind_blade_anim", "effects/wind_blade", new KeyValuePair<string, float>[]
            {
                new(CW_S.wakan, Content_Constants.default_spell_cost)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);

                    foreach (Actor target_unit in tiles.SelectMany(target_tile => target_tile._units))
                    {
                        target_unit.addForce(0, 0, 0.8f);
                    }
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_give_self_status_spells()
    {
        CW_SpellAsset spell_asset;
        EffectController effect_controller;
        AnimationSetting anim_setting;

        // 玄武之甲
        FormatSpells.create_give_self_status_spell(
            "basalt_armor", "status_basalt_armor",
            rarity: 3, element_container: new[] { 100, 0, 0, 0, 0 },
            trigger_tags: new[] { SpellTriggerTag.NAMED_DEFEND },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }
        );
        // 青龙之鳞
        FormatSpells.create_give_self_status_spell(
            "gloong_scale", "status_gloong_scale",
            rarity: 3, element_container: new[] { 0, 0, 100, 0, 0 },
            trigger_tags: new[] { SpellTriggerTag.NAMED_DEFEND },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }
        );
        // 朱雀之羽
        FormatSpells.create_give_self_status_spell(
            "rosefinch_feather", "status_rosefinch_feather",
            rarity: 3, element_container: new[] { 0, 100, 0, 0, 0 },
            trigger_tags: new[] { SpellTriggerTag.ATTACK },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }
        );
        // 麒麟之角
        FormatSpells.create_give_self_status_spell(
            "unicorn_horn", "status_unicorn_horn",
            rarity: 3, element_container: new[] { 0, 0, 0, 0, 100 },
            trigger_tags: new[] { SpellTriggerTag.NAMED_DEFEND },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }
        );
        // 白虎之牙
        FormatSpells.create_give_self_status_spell(
            "wtiger_tooth", "status_wtiger_tooth",
            rarity: 3, element_container: new[] { 0, 0, 0, 100, 0 },
            trigger_tags: new[] { SpellTriggerTag.ATTACK },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_track_projectile_spells()
    {
        CW_SpellAsset spell_asset;
        EffectController effect_controller;
        AnimationSetting anim_setting;

        #region 金剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "gold_sword",
            "single_gold_sword_anim", "effects/single_gold_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            element_container: new[] { 0, 0, 0, 100, 0 }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion

        #region 木剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "wood_sword",
            "single_wood_sword_anim", "effects/single_wood_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            element_container: new[] { 0, 0, 100, 0, 0 }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.base_scale = 0.15f;
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion

        #region 水剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "water_sword",
            "single_water_sword_anim", "effects/single_water_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            element_container: new[] { 100, 0, 0, 0, 0 }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.base_scale = 0.15f;
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion
    }
}