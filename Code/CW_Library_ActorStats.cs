using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public bool anti_time_stop = false;
        /// <summary>
        /// 预设名
        /// </summary>
        public string fixed_name = null;
        /// <summary>
        /// 允许的修炼体系
        /// </summary>
        internal uint allow_cultisys = 0;
        /// <summary>
        /// 强制的修炼体系，或关系
        /// </summary>
        internal uint force_cultisys = 0;
        /// <summary>
        /// 拓展属性加成
        /// </summary>
        public CW_BaseStats cw_base_stats;
    }
    public class CW_Library_ActorStats : CW_Asset_Library<CW_ActorStats>
    {
        internal List<string> added_actors = new List<string>();
        public override void init()
        {
            base.init();
            foreach(ActorStats actor_stats in AssetManager.unitStats.list)
            {
                this.__add(actor_stats, actor_stats.unit? 0.8f:Mathf.Max(0.03f*(int)actor_stats.actorSize,0.3f), new int[] { 20, 20, 20, 20, 20 }, 0f, new List<string>(), false, null, new CW_BaseStats());
            }
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        public override CW_ActorStats clone(string pNew, string pFrom)
        {
            CW_ActorStats new_stats = JsonUtility.FromJson<CW_ActorStats>(JsonUtility.ToJson(this.dict[pFrom]));
            new_stats.id = pNew;
            new_stats.cw_base_stats = dict[pFrom].cw_base_stats.deepcopy();
            new_stats.origin_stats = JsonUtility.FromJson<ActorStats>(JsonUtility.ToJson(AssetManager.unitStats.dict[pFrom]));
            new_stats.origin_stats.id = pNew;
            new_stats.origin_stats.baseStats = new_stats.cw_base_stats.base_stats;
            new_stats.born_spells.AddRange(dict[pFrom].born_spells);
            this.add(new_stats);
            return new_stats;
        }
        public override CW_ActorStats add(CW_ActorStats pAsset)
        {
            added_actors.Add(pAsset.id);
            AssetManager.unitStats.add(pAsset.origin_stats);
            return base.add(pAsset);
        }
        public CW_ActorStats add(ActorStats stats, float culti_velo = 1f, int[] prefer_element = null, float prefer_element_scale = 0f, List<string> born_spells = null, bool anti_time_stop = false, string fixed_name = null, CW_BaseStats addition_stats = null)
        {
            if (!AssetManager.unitStats.list.Contains(stats)) AssetManager.unitStats.add(stats);
            return __add(stats, culti_velo, prefer_element == null ? new int[] { 20, 20, 20, 20, 20 } : prefer_element, prefer_element_scale, born_spells == null ? new List<string>() : born_spells, anti_time_stop, fixed_name, addition_stats==null?new CW_BaseStats():addition_stats);
        }
        private CW_ActorStats __add(ActorStats stats, float culti_velo, int[] prefer_element, float prefer_element_scale, List<string> born_spells, bool anti_time_stop, string fixed_name, CW_BaseStats addition_stats)
        {
            CW_ActorStats cw_actor_stats = new CW_ActorStats();
            cw_actor_stats.id = stats.id;
            cw_actor_stats.origin_stats = stats;
            cw_actor_stats.culti_velo = culti_velo;
            cw_actor_stats.prefer_element = prefer_element;
            cw_actor_stats.prefer_element_scale = prefer_element_scale;
            cw_actor_stats.born_spells = born_spells;
            cw_actor_stats.anti_time_stop = anti_time_stop;
            cw_actor_stats.fixed_name = fixed_name;
            addition_stats.addStats(stats.baseStats);
            cw_actor_stats.cw_base_stats = addition_stats;
            cw_actor_stats.allow_cultisys = 0;
            this.list.Add(cw_actor_stats);
            this.dict.Add(stats.id, cw_actor_stats);
            return cw_actor_stats;
        }
    }
}
