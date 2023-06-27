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
            "single_gold_sword_anim", "effects/single_gold_sword",
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, 30f)
            },
            element_container: new[] { 0, 0, 0, 100, 0 }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.base_scale = 0.15f;
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion

        #region 木剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "wood_sword",
            "single_wood_sword_anim", "effects/single_wood_sword",
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
            "single_water_sword_anim", "effects/single_water_sword",
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