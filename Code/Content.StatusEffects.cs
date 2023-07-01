using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.General.AboutStatusEffect;

namespace Cultivation_Way.Content;

internal static class StatusEffects
{
    public static void init()
    {
        add_element_statuses();
        add_common_statuses();
    }

    private static void add_common_statuses()
    {
        BaseStats bonus_stats;
        // 破甲
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_armor] = -0.3f;
        FormatStatusEffect.create_simple_status_effect(
            "status_armor_expose", bonus_stats,
            10f,
            "", "", 0.5f,
            "effects/basalt_armor/玄武之甲",
            new[] { StatusEffectTag.POSITIVE }
        );
    }

    private static void add_element_statuses()
    {
        BaseStats bonus_stats;
        // 玄武之甲
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_armor] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_basalt_armor", bonus_stats,
            10f,
            "basalt_armor_anim", "effects/basalt_armor", 0.5f,
            "effects/basalt_armor/玄武之甲",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Basic
        );
        // 青龙之鳞
        bonus_stats = new BaseStats();
        bonus_stats[CW_S.health_regen] = 10f;
        bonus_stats[CW_S.mod_health_regen] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_gloong_scale", bonus_stats,
            10f,
            "gloong_scale_anim", "effects/gloong_scale", 0.5f,
            "effects/gloong_scale/青龙之鳞",
            new[] { StatusEffectTag.POSITIVE }
        );
        // 朱雀之羽
        bonus_stats = new BaseStats();
        FormatStatusEffect.create_simple_status_effect(
            "status_rosefinch_feather", bonus_stats,
            10f,
            "rosefinch_feather_anim", "effects/rosefinch_feather", 0.5f,
            "effects/rosefinch_feather/朱雀之羽",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Advanced
        ).action_on_attack = (effect, target, attacker) =>
        {
            if (target.objectType != MapObjectType.Actor) return;
            // TODO: 强化效果
            ((CW_Actor)target).addStatusEffect("burning");
        };
        // 麒麟之角
        bonus_stats = new BaseStats();
        bonus_stats[CW_S.mod_spell_armor] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_unicorn_horn", bonus_stats,
            10f,
            "unicorn_horn_anim", "effects/unicorn_horn", 0.5f,
            "effects/unicorn_horn/麒麟之角",
            new[] { StatusEffectTag.POSITIVE }
        );
        // 白虎之牙
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_damage] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_wtiger_tooth", bonus_stats,
            10f,
            "wtiger_tooth_anim", "effects/wtiger_tooth", 0.5f,
            "effects/wtiger_tooth/白虎之牙",
            new[] { StatusEffectTag.POSITIVE }
        );
    }
}