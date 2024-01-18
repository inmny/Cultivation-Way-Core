using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.Implementation;

internal static class Cultisyses
{
    public static CultisysAsset immortal;
    public static CultisysAsset bushido;
    public static CultisysAsset soul;
    public static CultisysAsset body;
    public static float[] immortal_power_co = new float[Content_Constants.immortal_max_level];

    public static void init()
    {
        add_immortal();
        add_bushido();
        add_soul_cultisys();
        add_blood();
    }

    private static void add_blood()
    {
        [Hotfixable]
        void init_blood(CultisysAsset cultisys)
        {
            for (var i = 0; i < Content_Constants.blood_max_level; i++) cultisys.power_level[i] = 1 + i * 0.1f;
        }

        var max_progress = new float[Content_Constants.blood_max_level];
        for (var i = 0; i < Content_Constants.blood_max_level; i++) max_progress[i] = (i + 1) * 0.05f;
        CultisysAsset cultisys = new(Content_Constants.blood_id, Content_Constants.energy_blood_id, CultisysType.BLOOD,
            Content_Constants.blood_max_level, init_blood)
        {
            sprite_path = "ui/Icons/iconWus",
            curr_progress = (actor, asset, level) => actor.data.GetMainBloodPurity(),
            max_progress = (actor, asset, level) => max_progress[level],
            power_base = 1000,
            can_levelup = (actor, asset, level) =>
                asset.curr_progress(actor, asset, level) >= asset.max_progress(actor, asset, level),
            allow = (actor, asset, level) => true
        };

        Library.Manager.cultisys.add(cultisys);
        body = cultisys;
    }

    private static void add_immortal()
    {
        [Hotfixable]
        void init_immortal(CultisysAsset cultisys)
        {
            for (int i = 0; i < Content_Constants.immortal_max_level; i++)
            {
                cultisys.power_level[i] = 1 + i * 0.1f;

                cultisys.bonus_stats[i][S.health] = Mathf.Pow(1.4f, i + 1) * 100 - 100f / (i + 1);
                cultisys.bonus_stats[i][S.mod_health] = i;

                cultisys.bonus_stats[i][CW_S.wakan] = Mathf.Pow(1.4f, i) * 100 / (i * i + 1);
                cultisys.bonus_stats[i][CW_S.mod_wakan] = i * i;

                cultisys.bonus_stats[i][CW_S.spell_armor] = Mathf.Pow(1.2787536f, i) / (i * i + 1);
                cultisys.bonus_stats[i][CW_S.mod_spell_armor] = i * i;

                cultisys.bonus_stats[i][CW_S.wakan_regen] = Mathf.Pow(2f, i);
                cultisys.bonus_stats[i][S.armor] = i;
                cultisys.bonus_stats[i][S.mod_armor] = Mathf.Pow(1.2f, i) * 0.2f;
                cultisys.bonus_stats[i][S.damage] = Mathf.Pow(1.2f, i) * 9;
                cultisys.bonus_stats[i][S.max_age] = i * i * 10;
                immortal_power_co[i] = Mathf.Pow(cultisys.power_base, cultisys.power_level[i]);
            }
        }

        CultisysAsset cultisys = new("cw_cultisys_immortal", Content_Constants.energy_wakan_id, CultisysType.WAKAN,
            Content_Constants.immortal_max_level, init_immortal)
        {
            sprite_path = "ui/Icons/iconCultiBook_immortal",
            power_base = 1000,
            curr_progress = (actor, culti, level) =>
            {
                actor.data.get(DataS.wakan, out float wakan);
                return wakan;
            },
            max_progress = (actor, culti, level) => actor.stats[CW_S.wakan],
            allow = (actor, culti, level) => actor.data.GetElement().GetElementType().id != "cw_common",
            can_levelup = (actor, culti, level) =>
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
            },
            external_levelup_bonus = (actor, culti, level) =>
            {
                if (level % 5 == 0)
                {
                    SpecialSpells.CastDoom(actor, (DoomType)(level / 5 - 1));
                }

                actor.data.get(DataS.wakan, out float wakan);
                wakan *= immortal_power_co[level - 1] / immortal_power_co[level];
                actor.data.set(DataS.wakan, wakan);
                return 0;
            }
        };

        Library.Manager.cultisys.add(cultisys);
        immortal = cultisys;
    }

    private static void add_bushido()
    {
        [Hotfixable]
        void init_bushido(CultisysAsset cultisys)
        {
            for (int i = 0; i < Content_Constants.bushido_max_level; i++)
            {
                cultisys.power_level[i] = 1 + i * 0.1f;

                cultisys.bonus_stats[i][S.health] = Mathf.Pow(1.4f, i + 1) * 100 - 100f / (i + 1);
                cultisys.bonus_stats[i][S.mod_health] = i;
                cultisys.bonus_stats[i][CW_S.health_regen] = i;

                cultisys.bonus_stats[i][S.damage] = Mathf.Pow(1.4f, i) * 100 / (i * i + 1);
                cultisys.bonus_stats[i][S.mod_damage] = i * i;

                cultisys.bonus_stats[i][CW_S.spell_armor] = Mathf.Pow(1.2787536f, i) / (i * i + 1);
                cultisys.bonus_stats[i][CW_S.mod_spell_armor] = i;

                cultisys.bonus_stats[i][S.armor] = Mathf.Pow(1.2787536f, i) / (i * i + 1);
                cultisys.bonus_stats[i][S.mod_armor] = i * i;

                cultisys.bonus_stats[i][S.max_age] = (i + 1) * (i + 1);
            }
        }

        CultisysAsset cultisys = new("cw_cultisys_bushido", Content_Constants.energy_bushido_id, CultisysType.BODY,
            Content_Constants.bushido_max_level, init_bushido)
        {
            sprite_path = "ui/Icons/iconCultiBook_bushido",
            curr_progress = (actor, culti, level) => actor.data.health,
            max_progress = (actor, asset, level) => actor.stats[S.health],
            power_base = 1000,
            can_levelup = [Hotfixable](actor, asset, level) =>
            {
                if (asset.curr_progress(actor, asset, 0) < asset.max_progress(actor, asset, 0) * 0.95f) return false;
                return Toolbox.randomChance(1f / (level * level));
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
            allow = (actor, asset, level) =>
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
                cultivelo *= Toolbox.randomFloat(level - 1, level);
                actor.data.set(Content_Constants.data_bushido_cultivelo, cultivelo);
                return 0;
            }
        };


        Library.Manager.cultisys.add(cultisys);
        bushido = cultisys;
    }

    private static void add_soul_cultisys()
    {
        [Hotfixable]
        void init_soul(CultisysAsset cultisys)
        {
            for (int i = 0; i < Content_Constants.soul_max_level; i++)
            {
                cultisys.power_level[i] = 1 + i * 0.1f;
                cultisys.bonus_stats[i][CW_S.soul] = 1 + i * i * 99;
                cultisys.bonus_stats[i][CW_S.soul_regen] = 0.1f + 0.1f * (i + 1) * (i + 1);
                cultisys.bonus_stats[i][S.max_age] = (i + 1) * (i + 1) / 2;
                cultisys.bonus_stats[i][S.health] = i * 30;
            }
        }

        CultisysAsset cultisys = new("cw_cultisys_soul", Content_Constants.energy_soul_id, CultisysType.SOUL,
            Content_Constants.soul_max_level, init_soul)
        {
            sprite_path = "ui/Icons/iconCultiBook_immortal",
            curr_progress = (actor, asset, level) =>
            {
                actor.data.get(DataS.soul, out float soul_val);
                return soul_val;
            },
            max_progress = (actor, asset, level) => actor.stats[CW_S.soul],
            power_base = 1000,
            can_levelup = (actor, asset, level) =>
            {
                if (asset.curr_progress(actor, asset, 0) < asset.max_progress(actor, asset, 0) * 0.95f) return false;
                return Toolbox.randomChance(1f / level);
            },
            allow = (actor, asset, level) => { return true; }
        };

        Library.Manager.cultisys.add(cultisys);
        soul = cultisys;
    }
}