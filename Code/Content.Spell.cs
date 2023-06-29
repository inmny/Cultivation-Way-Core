using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutSpell;
using Cultivation_Way.Library;

namespace Cultivation_Way.Content;

internal static class Spell
{
    public static void init()
    {
        add_track_projectile_spells();
        add_give_self_status_spells();
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
                new(DataS.wakan, 30f)
            }
        );
        // 青龙之鳞
        FormatSpells.create_give_self_status_spell(
            "gloong_scale", "status_gloong_scale",
            rarity: 3, element_container: new[] { 0, 0, 100, 0, 0 },
            trigger_tags: new[] { SpellTriggerTag.NAMED_DEFEND },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, 30f)
            }
        );
        // 朱雀之羽
        FormatSpells.create_give_self_status_spell(
            "rosefinch_feather", "status_rosefinch_feather",
            rarity: 3, element_container: new[] { 0, 100, 0, 0, 0 },
            trigger_tags: new[] { SpellTriggerTag.ATTACK },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, 30f)
            }
        );
        // 麒麟之角
        FormatSpells.create_give_self_status_spell(
            "unicorn_horn", "status_unicorn_horn",
            rarity: 3, element_container: new[] { 0, 0, 0, 0, 100 },
            trigger_tags: new[] { SpellTriggerTag.NAMED_DEFEND },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, 30f)
            }
        );
        // 白虎之牙
        FormatSpells.create_give_self_status_spell(
            "wtiger_tooth", "status_wtiger_tooth",
            rarity: 3, element_container: new[] { 0, 0, 0, 100, 0 },
            trigger_tags: new[] { SpellTriggerTag.ATTACK },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, 30f)
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
                new(DataS.wakan, 30f)
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
                new(DataS.wakan, 30f)
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
                new(DataS.wakan, 30f)
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