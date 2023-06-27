using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutStatusEffect;

namespace Cultivation_Way.Content;

internal static class StatusEffect
{
    public static void init()
    {
        add_simple_statuses();
    }

    private static void add_simple_statuses()
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
    }
}