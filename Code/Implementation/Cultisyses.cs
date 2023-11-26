using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal static class Cultisyses
{
    public static CultisysAsset immortal;
    public static CultisysAsset bushido;
    public static CultisysAsset soul;

    public static void init()
    {
        add_immortal();
        add_bushido();
        add_soul_cultisys();
    }

    private static void add_immortal()
    {
        CultisysAsset cultisys = new("cw_cultisys_immortal", Content_Constants.energy_wakan_id, CultisysType.WAKAN,
            Content_Constants.immortal_max_level)
        {
            sprite_path = "ui/Icons/iconCultiBook_immortal",
            power_base = 1000,
            curr_progress = (actor, culti, level) =>
            {
                actor.data.get(DataS.wakan, out float wakan);
                return wakan;
            },
            max_progress = (actor, culti, level) => actor.stats[CW_S.wakan],
            allow = (actor, culti) => actor.data.get_element().get_type().id != "cw_common",
            can_levelup = (actor, culti) =>
                immortal.curr_progress(actor, immortal, 0) >= immortal.max_progress(actor, immortal, 0),
            monthly_update_action = (actor, culti, level) =>
            {
                float regen_wakan_line = actor.stats[CW_S.wakan] * Content_Constants.immortal_max_wakan_regen;
                actor.data.get(DataS.wakan, out float wakan);

                if (wakan >= regen_wakan_line) return 0;


                if (regen_wakan_line - wakan > actor.stats[CW_S.wakan_regen])
                {
                    wakan += actor.stats[CW_S.wakan_regen];
                }
                else
                {
                    wakan = regen_wakan_line;
                }

                actor.data.set(DataS.wakan, wakan);
                return 0;
            }
        };
        for (int i = 0; i < Content_Constants.immortal_max_level; i++)
        {
            cultisys.power_level[i] = 1 + i * 0.1f;
            cultisys.bonus_stats[i][CW_S.wakan] = 1 + i * 99;
            cultisys.bonus_stats[i][CW_S.wakan_regen] = i * 0.1f;
            cultisys.bonus_stats[i][S.armor] = i * 10;
            cultisys.bonus_stats[i][S.mod_armor] = i;
            cultisys.bonus_stats[i][S.health] = i * 99;
            cultisys.bonus_stats[i][S.mod_health] = i;
            cultisys.bonus_stats[i][S.damage] = i * 9;
            cultisys.bonus_stats[i][S.max_age] = i * i * 10;
        }

        Library.Manager.cultisys.add(cultisys);
        immortal = cultisys;

        Library.Manager.actors.get("unit_human").allowed_cultisys.Add(cultisys);
    }

    private static void add_bushido()
    {
        CultisysAsset cultisys = new("cw_cultisys_bushido", Content_Constants.energy_bushido_id, CultisysType.BODY,
            Content_Constants.bushido_max_level)
        {
            sprite_path = "ui/Icons/iconCultiBook_bushido",
            curr_progress = (actor, culti, level) => actor.data.health,
            max_progress = (actor, asset, level) => actor.stats[S.health],
            can_levelup = (actor, asset) =>
            {
                if (asset.curr_progress(actor, asset, 0) < asset.max_progress(actor, asset, 0) * 0.95f) return false;
                actor.data.get(asset.id, out int level, 1);
                return Toolbox.randomChance(1f / (level + 1));
            },
            monthly_update_action = (actor, asset, level) =>
            {
                // 强制控制生命恢复上限
                float regen_health_line = actor.stats[S.health] * Content_Constants.bushido_max_health_regen;
                if (!(actor.data.health >= regen_health_line)) return 0;

                if (actor.data.health > regen_health_line + actor.stats[CW_S.health_regen])
                {
                    actor.data.health -= (int)actor.stats[CW_S.health_regen];
                }
                else
                {
                    actor.data.health = (int)regen_health_line;
                }

                return 0;
            },
            allow = (actor, asset) =>
            {
                if (false)
                {
                    return false;
                }

                actor.data.set(Content_Constants.data_bushido_cultivelo, Toolbox.randomFloat(1, 100));
                return true;
            },
            external_levelup_bonus = (actor, asset, level) =>
            {
                actor.data.get(Content_Constants.data_bushido_cultivelo, out float cultivelo, 1);
                cultivelo *= Toolbox.randomFloat(level-1, level);
                actor.data.set(Content_Constants.data_bushido_cultivelo, cultivelo);
                return 0;
            }
        };
        for (int i = 0; i < Content_Constants.bushido_max_level; i++)
        {
            cultisys.power_level[i] = 1 + i * 0.1f;
            cultisys.bonus_stats[i][S.mod_health] = i * 0.2f;
            cultisys.bonus_stats[i][CW_S.health_regen] = i;
            cultisys.bonus_stats[i][S.armor] = i * 10;
            cultisys.bonus_stats[i][S.health] = i * 99;
            cultisys.bonus_stats[i][S.mod_armor] = i * 3;
            cultisys.bonus_stats[i][S.mod_health] = i;
            cultisys.bonus_stats[i][S.damage] = i * 9;
            cultisys.bonus_stats[i][S.max_age] = (i + 1) * (i + 1);
        }

        Library.Manager.cultisys.add(cultisys);
        bushido = cultisys;
    }

    private static void add_soul_cultisys()
    {
        CultisysAsset cultisys = new("cw_cultisys_soul", Content_Constants.energy_soul_id, CultisysType.SOUL,
            Content_Constants.soul_max_level)
        {
            sprite_path = "ui/Icons/iconCultiBook_immortal",
            curr_progress = (actor, asset, level) =>
            {
                actor.data.get(DataS.soul, out float soul_val);
                return soul_val;
            },
            max_progress = (actor, asset, level) => actor.stats[CW_S.soul],
            can_levelup = (actor, asset) =>
            {
                if (asset.curr_progress(actor, asset, 0) < asset.max_progress(actor, asset, 0) * 0.95f) return false;
                actor.data.get(asset.id, out int level, 1);
                return Toolbox.randomChance(1f / (level + 1));
            },
            allow = (actor, asset) => { return true; }
        };
        for (int i = 0; i < Content_Constants.soul_max_level; i++)
        {
            cultisys.power_level[i] = 1 + i * 0.1f;
            cultisys.bonus_stats[i][CW_S.soul] = 1 + i * i * 99;
            cultisys.bonus_stats[i][CW_S.soul_regen] = 0.1f + 0.1f * (i + 1) * (i + 1);
            cultisys.bonus_stats[i][S.max_age] = (i + 1) * (i + 1) / 2;
            cultisys.bonus_stats[i][S.health] = i * 30;
        }

        Library.Manager.cultisys.add(cultisys);
        soul = cultisys;
    }
}