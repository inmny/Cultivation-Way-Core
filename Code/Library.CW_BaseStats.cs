﻿using Cultivation_Way.Constants;
using System;
using System.Reflection;
namespace Cultivation_Way.Library
{
    internal static class CW_BaseStatsLibrary
    {
        public static void init()
        {
            foreach (FieldInfo field in typeof(CW_S).GetFields())
            {
                if (field.FieldType == typeof(string))
                {
                    string stat_id = (string)field.GetValue(null);
                    BaseStatAsset stat_asset = add(new BaseStatAsset
                    {
                        hidden = false,
                        icon = String.Empty,
                        id = stat_id,
                        translation_key = "stat_"+Constants.Core.mod_prefix + stat_id,
                        ignore = false,
                        normalize = false,
                        used_only_for_civs = true
                    });
                    if (stat_id.StartsWith("mod_"))
                    {
                        stat_asset.main_stat_to_mod = stat_id.Substring(4);
                        stat_asset.mod = true;
                        stat_asset.show_as_percents = true;
                    }
                }
            }

            get(CW_S.mod_cultivelo).mod = false;
        }
        private static BaseStatAsset t;
        private static BaseStatAsset get(string id)
        {
            t = AssetManager.base_stats_library.get(id);
            return t;
        }
        private static BaseStatAsset add(BaseStatAsset new_stats)
        {
            BaseStatsLibrary library = AssetManager.base_stats_library;
            t = library.add(new_stats);
            return t;
        }
    }
}