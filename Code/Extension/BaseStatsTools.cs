using System;
using System.Linq;

namespace Cultivation_Way.Extension;

public static class BaseStatsTools
{
    /// <summary>
    ///     按系数合并两个BaseStats
    /// </summary>
    public static void merge_stats(this BaseStats this_stats, BaseStats another_stats, float co)
    {
        for (int i = 0; i < another_stats.stats_list.Count(); i++)
        {
            // 由于Assembly的公开，此处直接[]访问会导致歧义
            BaseStatsContainer base_stats_container = another_stats.stats_list._items[i];
            string id = base_stats_container.id;
            this_stats[id] += base_stats_container.value * co;
        }
    }

    /// <summary>
    ///     按系数两个BaseStats取最大
    /// </summary>
    public static void max(this BaseStats this_stats, BaseStats another_stats, float co = 1)
    {
        for (int i = 0; i < another_stats.stats_list.Count(); i++)
        {
            // 由于Assembly的公开，此处直接[]访问会导致歧义
            BaseStatsContainer base_stats_container = another_stats.stats_list._items[i] ??
                                                      throw new ArgumentNullException(
                                                          "another_stats.stats_list._items[i]");
            string id = base_stats_container.id;
            float value = base_stats_container.value * co;
            if (value > this_stats[id]) this_stats[id] = value;
        }
    }
}