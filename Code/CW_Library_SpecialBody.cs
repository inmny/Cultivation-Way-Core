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
        public int anim_id;
        public int level;
        public string[] spells;
        public int cur_own_nr { get; internal set; }
        public int histroy_own_nr { get; internal set; }
        public CW_BaseStats stxh_bonus_stats;
        public CW_BaseStats bonus_stats;
        public CW_Asset_SpecialBody(string id, string name, string description, string author_name, int level, string[] spells, CW_BaseStats bonus_stats)
        {
            id = id;
            name = name;
            description = description;
            author_name = author_name;
            level = level;
            spells = spells;
            bonus_stats = bonus_stats;
        }
        public CW_Asset_SpecialBody(CW_Actor author)
        {
            id = author.fast_data.actorID + "_" + author.fast_data.level;
            name = author.getName() + "的体质";
            author_name = author.getName();
            description = "可随机生成的描述";
            level = author.fast_data.level;
            spells = null;
            bonus_stats = new CW_BaseStats();
            stxh_bonus_stats = new CW_BaseStats();
            anim_id = Toolbox.randomInt(0, 3);
            gen_bonus_stats(author, bonus_stats);
            gen_bonus_stats(author, stxh_bonus_stats);
        }
        private void gen_bonus_stats(CW_Actor author, CW_BaseStats bonus_stats)
        {
            bonus_stats.clear();
            bonus_stats.mod_age = __get_co() * author.cw_cur_stats.mod_age;
            bonus_stats.mod_anti_armor = __get_co() * author.cw_cur_stats.mod_anti_armor;
            bonus_stats.mod_anti_crit = __get_co() * author.cw_cur_stats.mod_anti_crit;
            bonus_stats.mod_anti_crit_damage = __get_co() * author.cw_cur_stats.mod_anti_crit_damage;
            bonus_stats.mod_anti_spell_armor = __get_co() * author.cw_cur_stats.mod_anti_spell_armor;
            bonus_stats.mod_cultivation = __get_co() * author.cw_cur_stats.mod_cultivation;
            bonus_stats.mod_health_regen = __get_co() * author.cw_cur_stats.mod_health_regen;
            bonus_stats.mod_shield = __get_co() * author.cw_cur_stats.mod_shield;
            bonus_stats.mod_shield_regen = __get_co() * author.cw_cur_stats.mod_shield_regen;
            bonus_stats.mod_soul = __get_co() * author.cw_cur_stats.mod_soul;
            bonus_stats.mod_soul_regen = __get_co() * author.cw_cur_stats.mod_soul_regen;
            bonus_stats.mod_spell_armor = __get_co() * author.cw_cur_stats.mod_spell_armor;
            bonus_stats.mod_spell_range = __get_co() * author.cw_cur_stats.mod_spell_range;
            bonus_stats.mod_wakan = __get_co() * author.cw_cur_stats.mod_wakan;
            bonus_stats.mod_wakan_regen = __get_co() * author.cw_cur_stats.mod_wakan_regen;
            bonus_stats.vampire = __get_co() * author.cw_cur_stats.vampire;
            //throw new NotImplementedException();
        }
        private float __get_co()
        {
            return Toolbox.randomFloat(0, level / 12f);
        }
        public void store()
        {
            CW_Library_Manager.instance.special_bodies.add(this);
        }
        public void try_deprecate(bool force = false)
        {
            if (cur_own_nr > 0) return;
            CW_Library_Manager.instance.special_bodies.delete(id, force);
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
            if (dict.TryGetValue(id, out body))
            {
                dict.Remove(id);
                list.Remove(body);
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
