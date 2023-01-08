﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_ActorStats : Asset
    {
        /// <summary>
        /// 原版的生物信息
        /// </summary>
        public ActorStats origin_stats;
        /// <summary>
        /// 此类生物修炼速度
        /// </summary>
        public float culti_velo;
        /// <summary>
        /// 偏好元素
        /// </summary>
        public int[] prefer_element;
        /// <summary>
        /// 元素偏好系数
        /// </summary>
        public float prefer_element_scale;
        /// <summary>
        /// 自带法术
        /// </summary>
        public List<string> born_spells;
        /// <summary>
        /// 反时间停止（非游戏暂停
        /// </summary>
        public bool anti_time_stop;
        /// <summary>
        /// 预设名
        /// </summary>
        public string fixed_name;
    }
    public class CW_Library_ActorStats : AssetLibrary<CW_ActorStats>
    {
        public override void init()
        {
            foreach(ActorStats actor_stats in AssetManager.unitStats.list)
            {
                this.__add(actor_stats, 1f, new int[] { 20, 20, 20, 20, 20 }, 0f, new List<string>(), false, null);
            }
        }
        internal void register()
        {
            throw new NotImplementedException();
        }
        public CW_ActorStats add(ActorStats stats, float culti_velo = 1f, int[] prefer_element = null, float prefer_element_scale = 0f, List<string> born_spells = null, bool anti_time_stop = false, string fixed_name = null)
        {
            return __add(stats, culti_velo, prefer_element == null ? new int[] { 20, 20, 20, 20, 20 } : prefer_element, prefer_element_scale, born_spells == null ? new List<string>() : born_spells, anti_time_stop, fixed_name);
        }
        private CW_ActorStats __add(ActorStats stats, float culti_velo, int[] prefer_element, float prefer_element_scale, List<string> born_spells, bool anti_time_stop, string fixed_name)
        {
            CW_ActorStats cw_actor_stats = new CW_ActorStats();
            cw_actor_stats.origin_stats = stats;
            cw_actor_stats.culti_velo = culti_velo;
            cw_actor_stats.prefer_element = prefer_element;
            cw_actor_stats.prefer_element_scale = prefer_element_scale;
            cw_actor_stats.born_spells = born_spells;
            cw_actor_stats.anti_time_stop = anti_time_stop;
            cw_actor_stats.fixed_name = fixed_name;
            this.list.Add(cw_actor_stats);
            this.dict.Add(stats.id, cw_actor_stats);
            return cw_actor_stats;
        }
    }
}
