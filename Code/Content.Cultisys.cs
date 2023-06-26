using Cultivation_Way.Constants;
using Cultivation_Way.Library;

namespace Cultivation_Way.Content;

internal static class Cultisys
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
            curr_progress = (actor, culti, level) =>
            {
                actor.data.get(DataS.wakan, out float wakan);
                return wakan;
            },
            max_progress = (actor, culti, level) => actor.stats[CW_S.wakan],
            allow = (actor, culti) => true,
            can_levelup = (actor, culti) =>
            {
                return immortal.curr_progress(actor, immortal, 0) >= immortal.max_progress(actor, immortal, 0);
            },
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
            cultisys.bonus_stats[i][S.health] = i * 99;
            cultisys.bonus_stats[i][S.damage] = i * 9;
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
            sprite_path = "ui/Icons/iconCultiBook_bushido"
        };
        for (int i = 0; i < Content_Constants.bushido_max_level; i++)
        {
            cultisys.power_level[i] = 1 + i * 0.1f;
        }

        Library.Manager.cultisys.add(cultisys);
        bushido = cultisys;
    }

    private static void add_soul_cultisys()
    {
        CultisysAsset cultisys = new("cw_cultisys_soul", Content_Constants.energy_soul_id, CultisysType.SOUL,
            Content_Constants.soul_max_level)
        {
            sprite_path = "ui/Icons/iconCultiBook_immortal"
        };
        for (int i = 0; i < Content_Constants.soul_max_level; i++)
        {
            cultisys.power_level[i] = 1 + i * 0.1f;
        }

        Library.Manager.cultisys.add(cultisys);
        soul = cultisys;
    }
}