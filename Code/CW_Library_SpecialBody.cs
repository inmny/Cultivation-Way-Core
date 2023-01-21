using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_SpecialBody : Asset
    {
        public string name;
        public string description;
        public string author_name;
        public int level;
        public string[] spells;
        public int cur_own_nr { get; internal set; }
        public int histroy_own_nr { get; internal set; }
        public CW_BaseStats bonus_stats;
        public CW_Asset_SpecialBody(string id, string name, string description, string author_name, int level, string[] spells, CW_BaseStats bonus_stats)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.author_name = author_name;
            this.level = level;
            this.spells = spells;
            this.bonus_stats = bonus_stats;
        }
        public CW_Asset_SpecialBody(CW_Actor author)
        {
            this.id = author.fast_data.actorID + "_" + author.fast_data.level;
            this.name = author.getName() + "的体质";
            this.author_name = author.getName();
            this.description = "可随机生成的描述";
            this.level = author.fast_data.level;
            this.spells = null;
            this.bonus_stats = new CW_BaseStats();
            gen_bonus_stats(author);
        }
        private void gen_bonus_stats(CW_Actor author)
        {
            this.bonus_stats.clear();
            this.bonus_stats.mod_age = __get_co() * author.cw_cur_stats.mod_age;
            this.bonus_stats.mod_anti_armor = __get_co() * author.cw_cur_stats.mod_anti_armor;
            this.bonus_stats.mod_anti_crit = __get_co() * author.cw_cur_stats.mod_anti_crit;
            this.bonus_stats.mod_anti_crit_damage = __get_co() * author.cw_cur_stats.mod_anti_crit_damage;
            this.bonus_stats.mod_anti_spell_armor = __get_co() * author.cw_cur_stats.mod_anti_spell_armor;
            this.bonus_stats.mod_cultivation = __get_co() * author.cw_cur_stats.mod_cultivation;
            this.bonus_stats.mod_health_regen = __get_co() * author.cw_cur_stats.mod_health_regen;
            this.bonus_stats.mod_shied = __get_co() * author.cw_cur_stats.mod_shied;
            this.bonus_stats.mod_shied_regen = __get_co() * author.cw_cur_stats.mod_shied_regen;
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
            return Toolbox.randomFloat(0, this.level / 12f);
        }
        public void store()
        {
            CW_Library_Manager.instance.special_bodies.add(this);
        }
        public void try_deprecate(bool force = false)
        {
            if (this.cur_own_nr > 0) return;
            CW_Library_Manager.instance.special_bodies.delete(this.id, force);
        }
        public string get_info_without_name()
        {
            StringBuilder string_builder = new StringBuilder();
            string_builder.AppendLine("创造者:\t\t\t" + author_name);
            string_builder.AppendLine("等级:\t\t\t\t" + level);
            string_builder.AppendLine("描述:\t\t\t\t" + description);
            return string_builder.ToString();
        }
    }
    public class CW_Library_SpecialBody : CW_Dynamic_Library<CW_Asset_SpecialBody>
    {
        internal void delete(string id, bool force)
        {
            CW_Asset_SpecialBody body;
            if (this.dict.TryGetValue(id, out body))
            {
                this.dict.Remove(id);
                this.list.Remove(body);
            }
            return;
        }

        internal override void register()
        {
            throw new NotImplementedException();
        }
        internal override void reset()
        {
            throw new NotImplementedException();
        }
        internal string select_better(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b;
            if (string.IsNullOrEmpty(b)) return a;
            CW_Asset_SpecialBody A = get(a);
            CW_Asset_SpecialBody B = get(b);
            if (A == null) return B == null ? null : b;
            if (B == null) return a;
            return A.level >= B.level ? a : b;
        }

    }
}
