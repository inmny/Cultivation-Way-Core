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
            BaseStatsContainer baseStatsContainer = another_stats.stats_list._items[i];
            string id = baseStatsContainer.id;
            this_stats[id] += baseStatsContainer.value * co;
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
            BaseStatsContainer baseStatsContainer = another_stats.stats_list._items[i];
            string id = baseStatsContainer.id;
            float value = baseStatsContainer.value * co;
            if (value > this_stats[id]) this_stats[id] = value;
        }
    }
}