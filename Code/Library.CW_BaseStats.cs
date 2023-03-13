using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
namespace Cultivation_Way.Library
{
    internal static class CW_BaseStatsLibrary
    {
        public static void init()
        {
            add(new BaseStatAsset
            {
                hidden = true,
                icon = String.Empty,
                id = CW_S.mod_age,
                ignore = false,
                main_stat_to_mod = S.max_age,
                mod = true,
                normalize = true,
                normalize_min = -90f,
                normalize_max = 999,
                used_only_for_civs = true
            });
            add(new BaseStatAsset
            {
                hidden = false,
                icon = String.Empty,
                id = CW_S.spell_armor,
                ignore = false,
                normalize = false,
                used_only_for_civs = true
            });
            add(new BaseStatAsset
            {
                hidden = false,
                icon = String.Empty,
                id = CW_S.shield,
                ignore = false,
                normalize = false,
                used_only_for_civs = true
            });
        }
        private static BaseStatAsset add(BaseStatAsset new_stats)
        {
            BaseStatsLibrary library = AssetManager.base_stats_library;
            return library.add(new_stats);
        }
    }
}
