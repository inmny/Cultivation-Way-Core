using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extension
{
    public static class BaseStatsTools
    {
        /// <summary>
        /// 按系数合并两个BaseStats
        /// </summary>
        public static void mergeStats(this BaseStats this_stats, BaseStats another_stats, float co)
        {
            for (int i = 0; i < another_stats.stats_list.Count; i++)
            {
                BaseStatsContainer baseStatsContainer = another_stats.stats_list[i];
                string id = baseStatsContainer.id;
                this_stats[id] += baseStatsContainer.value * co;
            }
        }
        /// <summary>
        /// 按系数两个BaseStats取最大
        /// </summary>
        public static void max(this BaseStats this_stats, BaseStats another_stats, float co = 1)
        {
            for (int i = 0; i < another_stats.stats_list.Count; i++)
            {
                BaseStatsContainer baseStatsContainer = another_stats.stats_list[i];
                string id = baseStatsContainer.id;
                float value = baseStatsContainer.value * co;
                if(value > this_stats[id]) this_stats[id] = value;
            }
        }
    }
}
