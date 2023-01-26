using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Cultivation_Way.Library
{
    public class CW_Asset_CultiBook : Asset
    {
        public bool is_fixed;
        /// <summary>
        /// 功法名
        /// </summary>
        public string name;
        /// <summary>
        /// 这个暂时用不到，以后可能会用到
        /// </summary>
        public string content;
        /// <summary>
        /// 作者名，作者id即功法的id
        /// </summary>
        public string author_name;
        /// <summary>
        /// 当前修炼人数，用于回收统计
        /// </summary>
        public int cur_culti_nr { get; internal set; }
        /// <summary>
        /// 历史修炼人数
        /// </summary>
        public int histroy_culti_nr { get; internal set; }
        /// <summary>
        /// 等阶
        /// </summary>
        public int order;
        /// <summary>
        /// 品级
        /// </summary>
        public int level;
        /// <summary>
        /// 修炼速度加成
        /// </summary>
        public float culti_promt;
        /// <summary>
        /// 自带法术
        /// </summary>
        public string[] spells;
        /// <summary>
        /// 属性加成
        /// </summary>
        public CW_BaseStats bonus_stats;

        public CW_Asset_CultiBook(CW_Actor author)
        {
            this.order = 0;
            this.id = author.fast_data.actorID+"_"+order;
            this.is_fixed = false;
            this.cur_culti_nr = 0;
            this.histroy_culti_nr = 0;
            this.author_name = author.getName();
            this.name = author.getName()+"创造的功法";
            this.spells = new string[Others.CW_Constants.cultibook_spell_limit];
            this.bonus_stats = new CW_BaseStats();
        }

        public CW_Asset_CultiBook()
        {
        }

        internal void re_author(CW_Actor author)
        {
            this.id =author.fast_data.actorID + "_" + order;
            this.is_fixed = false;
            this.cur_culti_nr = 0;
            this.histroy_culti_nr = 0;
            this.author_name = author.getName();
            this.name = author.getName() + "修改的功法";
        }
        public CW_Asset_CultiBook deepcopy()
        {
            CW_Asset_CultiBook ret = new CW_Asset_CultiBook();
            ret.culti_promt = culti_promt;
            ret.cur_culti_nr = cur_culti_nr;
            ret.author_name = author_name;
            ret.bonus_stats = bonus_stats.deepcopy();
            ret.content = content;
            ret.histroy_culti_nr = histroy_culti_nr;
            ret.id = id;
            ret.is_fixed = is_fixed;
            ret.spells = new string[Others.CW_Constants.cultibook_spell_limit];
            for(int i = 0; i < Others.CW_Constants.cultibook_spell_limit; i++)
            {
                ret.spells[i] = spells[i];
            }
            ret.level = level;
            ret.name = name;
            ret.order = order;
            return ret;
        }
        public void store()
        {
            CW_Library_Manager.instance.cultibooks.add(this);
            //WorldBoxConsole.Console.print("store '" + id + "'");
        }
        private static List<string> funcs = new List<string>();
        public void try_deprecate(bool force = false)
        {
            if (histroy_culti_nr >= Others.CW_Constants.fix_cultibook_line) is_fixed = true;
            if (this.cur_culti_nr > 0) return;
            //WorldBoxConsole.Console.print("deprecate '" + id + "'");
            CW_Library_Manager.instance.cultibooks.delete(this.id, force);
            //WorldBoxConsole.Console.print(string.Format("Deprecate cultibook '{0}'", id));
        }
        internal string get_info_without_name()
        {
            StringBuilder string_builder = new StringBuilder();
            string_builder.AppendLine("\t创始人\t\t\t" + author_name+"\t");
            string_builder.AppendLine("\t品阶\t\t\t" + LocalizedTextManager.getText("cultibook_order_" + this.order) + Others.CW_Constants.num_to_cz[9-level]+"品");
            for(int i = 0; i < this.spells.Length; i++)
            {
                if (this.spells[i] == null) break;
                string_builder.AppendLine("\t法术["+i+"]\t\t\t"+LocalizedTextManager.getText("spell_"+this.spells[i])+"\t");
            }
            return string_builder.ToString();
        }
        internal void gen_bonus_stats(CW_Actor author)
        {
            this.bonus_stats.clear();
            this.bonus_stats.mod_age = __get_co() * author.cw_cur_stats.mod_age;
            this.bonus_stats.mod_anti_armor = __get_co() * author.cw_cur_stats.mod_anti_armor;
            this.bonus_stats.mod_anti_crit = __get_co() * author.cw_cur_stats.mod_anti_crit;
            this.bonus_stats.mod_anti_crit_damage = __get_co() * author.cw_cur_stats.mod_anti_crit_damage;
            this.bonus_stats.mod_anti_spell_armor = __get_co() * author.cw_cur_stats.mod_anti_spell_armor;
            this.bonus_stats.mod_cultivation = __get_co() * author.cw_cur_stats.mod_cultivation;
            this.bonus_stats.mod_health_regen = __get_co() * author.cw_cur_stats.mod_health_regen;
            this.bonus_stats.mod_shield = __get_co() * author.cw_cur_stats.mod_shield;
            this.bonus_stats.mod_shield_regen = __get_co() * author.cw_cur_stats.mod_shield_regen;
            this.bonus_stats.mod_soul = __get_co() * author.cw_cur_stats.mod_soul;
            this.bonus_stats.mod_soul_regen = __get_co() * author.cw_cur_stats.mod_soul_regen;
            this.bonus_stats.mod_spell_armor = __get_co() * author.cw_cur_stats.mod_spell_armor;
            this.bonus_stats.mod_spell_range = __get_co() * author.cw_cur_stats.mod_spell_range;
            this.bonus_stats.mod_wakan = __get_co() * author.cw_cur_stats.mod_wakan;
            this.bonus_stats.mod_wakan_regen = __get_co() * author.cw_cur_stats.mod_wakan_regen;
            this.bonus_stats.vampire = __get_co() * author.cw_cur_stats.vampire;
            //throw new NotImplementedException();
        }
        private float __get_co()
        {
            return Toolbox.randomFloat(0, (this.order * 10 + this.level) / 100f);
        }
    }
    public class CW_Library_CultiBook : CW_Dynamic_Library<CW_Asset_CultiBook>
    {
        public override void init()
        {
            base.init();

        }
        internal override void reset()
        {
            throw new NotImplementedException();
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        internal void delete(string cultibook_id, bool force_delete = false)
        {
            CW_Asset_CultiBook book;
            if(this.dict.TryGetValue(cultibook_id, out book))
            {
                if (book.is_fixed && !force_delete) return;
                this.dict.Remove(cultibook_id);
                this.list.Remove(book);
            }
            return;
        }
        internal string select_better(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b;
            if (string.IsNullOrEmpty(b)) return a;
            CW_Asset_CultiBook A = get(a);
            CW_Asset_CultiBook B = get(b);
            if (A == null) return B==null?null:b;
            if (B == null) return a;
            return (A.order << 4 + A.level) >= (B.order << 4 + B.level) ? a : b;
        }
    }
}
