using System;
using System.Linq;

namespace Cultivation_Way.Extension;

public static class BaseStatsTools
{
    /// <summary>
    ///     按系数合并两个BaseStats
    /// </summary>
    public static void MergeStats(this BaseStats pBaseStats, BaseStats pAnotherStats, float pCo)
    {
        for (int i = 0; i < pAnotherStats.stats_list.Count(); i++)
        {
            // 由于Assembly的公开，此处直接[]访问会导致歧义
            BaseStatsContainer base_stats_container = pAnotherStats.stats_list._items[i];
            string id = base_stats_container.id;
            pBaseStats[id] += base_stats_container.value * pCo;
        }
    }

    /// <summary>
    ///     按系数两个BaseStats取最大
    /// </summary>
    public static void Max(this BaseStats pBaseStats, BaseStats pAnotherStats, float pCo = 1)
    {
        for (int i = 0; i < pAnotherStats.stats_list.Count(); i++)
        {
            // 由于Assembly的公开，此处直接[]访问会导致歧义
            BaseStatsContainer base_stats_container = pAnotherStats.stats_list._items[i] ??
                                                      throw new ArgumentNullException(
                                                          "pAnotherStats.stats_list._items[i]");
            string id = base_stats_container.id;
            float value = base_stats_container.value * pCo;
            if (value > pBaseStats[id]) pBaseStats[id] = value;
        }
    }
}