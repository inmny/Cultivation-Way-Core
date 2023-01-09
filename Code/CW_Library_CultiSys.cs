﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_CultiSys : Asset
    {
        internal uint _tag;
        public string sprite_name;
        public float[] grade_val;
        public CW_BaseStats[] bonus_stats;
        // 修炼体系的黑白名单设定
        // 先按照种族黑白名单进行总体的设定，再根据单位的黑白名单进行细节的设定
        // 若要两者同时使用，请理清关系。
        // 具体为:
        // 设所有生物集合为U，种族list集合为R，其包含的生物组成集合RU，生物list集合为UL
        // 定义集合操作符：-为集合差集，+为集合并集
        // 种族黑/白名单  生物黑/白名单 最终允许的生物
        //     黑             黑        U-RU-UL
        //     黑             白        U-RU+UL
        //     白             黑        RU-UL
        //     白             白        RU+UL
        // 如果尚不明白，可选择阅读代码，或另某一列表为空，自行分析。
        public bool units_black_or_white;
        public bool races_black_or_white;
        public List<string> units_list;
        public List<string> races_list;

        internal void register()
        {
            bool tmp;
            foreach(CW_ActorStats cw_actor_stats in CW_Library_Manager.instance.units.list)
            {
                tmp = races_black_or_white ^ races_list.Contains(cw_actor_stats.origin_stats.race);
                if (units_list.Contains(cw_actor_stats.id)) tmp = !units_black_or_white;
                
                if (tmp)
                {
                    cw_actor_stats.allow_cultisys |= this._tag;
                }
            }
        }
    }
    public class CW_Library_CultiSys : AssetLibrary<CW_Asset_CultiSys>
    {
        public override void init()
        {
            base.init();
        }
        internal void register()
        {
            for(int i = 0; i < list.Count; i++)
            {
                list[i]._tag = (uint)(1 << i);
                list[i].register();
            }
        }
    }
}