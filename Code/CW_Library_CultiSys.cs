using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_CultiSys : Asset
    {
        // TODO: 添加经验获取函数
        public int tag { get; internal set; }
        internal uint _tag;
        public string sprite_name;
        public float[] power_level;
        public CW_BaseStats[] bonus_stats;
        public CW_Spell_Tag[] addition_spell_require;
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
        public Others.CW_Delegates.CW_Cultisys_Stats stats;
        public Others.CW_Delegates.CW_Cultisys_Judge judge;
        public Others.CW_Delegates.CW_Cultisys_Level_Judge level_judge;
        public CW_Asset_CultiSys()
        {
            stats = default_get_stats;
            units_black_or_white = true;
            units_list = new List<string>();
            races_black_or_white = true;
            races_list = new List<string>();
            power_level = new float[Others.CW_Constants.max_cultisys_level];
            bonus_stats = new CW_BaseStats[Others.CW_Constants.max_cultisys_level];
            for(int i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                power_level[i] = 1;
                bonus_stats[i] = new CW_BaseStats();
            }
        }
        internal static CW_BaseStats default_get_stats(CW_Actor cw_actor, CW_Asset_CultiSys cultisys, int level)
        {
            return cultisys.bonus_stats[level];
        }
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
    public class CW_Library_CultiSys : CW_Asset_Library<CW_Asset_CultiSys>
    {
        public override void init()
        {
            base.init();
        }
        internal override void register()
        {
            for(int i = 0; i < list.Count; i++)
            {
                list[i].tag = i;
                list[i]._tag = (uint)(1 << i);
                list[i].register();
            }
        }
        // TODO: 安全保障
        internal CW_BaseStats get_bonus_stats(int cultisys_tag, int level, CW_Actor cw_actor)
        {
            if (level >= Others.CW_Constants.max_cultisys_level) WorldBoxConsole.Console.print(level); 
            return list[cultisys_tag].stats(cw_actor, list[cultisys_tag],level>=Others.CW_Constants.max_cultisys_level?Others.CW_Constants.max_cultisys_level-1:level);
        }
        public void set_cultisys(CW_Actor actor)
        {
            uint actor_allow_cultisys = actor.cw_stats.allow_cultisys;
            int cultisys_tag = 0;
            while (actor_allow_cultisys > 0)
            {
                if ((actor_allow_cultisys & 0x1) == 1 && list[cultisys_tag].judge(actor, list[cultisys_tag])) 
                { 
                    actor.cw_data.cultisys |= (uint)(1 << cultisys_tag);
                }
                cultisys_tag++;
                actor_allow_cultisys >>= 1;
            }
            if (actor.cw_data.cultisys > 0) actor.cw_status.can_culti = true;
        }
        internal string parse_cultisys(CW_ActorData cw_actor_data)
        {
            uint cultisys = cw_actor_data.cultisys;
            int cultisys_tag = 0;
            StringBuilder string_builder = new StringBuilder();
            CW_Asset_CultiSys cultisys_asset;
            while (cultisys > 0)
            {
                if ((cultisys & 0x1) == 1)
                {
                    cultisys_asset = list[cultisys_tag];
                    string_builder.Append(LocalizedTextManager.getText("CW_cultisys_" + cultisys_asset.id));
                    string_builder.Append("\t");
                    string_builder.Append(LocalizedTextManager.getText("cultisys_" + cultisys_asset.id + "_" + cw_actor_data.cultisys_level[cultisys_tag]));
                    string_builder.Append("[" + cw_actor_data.cultisys_level[cultisys_tag] + "]\n");
                }
                cultisys >>= 1;
                cultisys_tag++;
            }
            return string_builder.ToString();
        }
    }
}
