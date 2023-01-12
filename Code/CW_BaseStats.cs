using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    /// <summary>
    /// 拓展属性
    /// </summary>
    [Serializable]
    public class CW_BaseStats
    {
        /// <summary>
        /// 护盾
        /// </summary>
        public int shied;
        /// <summary>
        /// 护盾系数
        /// </summary>
        public float mod_shied;
        /// <summary>
        /// 元神
        /// </summary>
        public int soul;
        /// <summary>
        /// 元神系数
        /// </summary>
        public float mod_soul;
        /// <summary>
        /// 元神恢复/月
        /// </summary>
        public int soul_regen;
        /// <summary>
        /// 元神恢复系数
        /// </summary>
        public float mod_soul_regen;
        /// <summary>
        /// 寿命加成，可为负数
        /// </summary>
        public int age_bonus;
        /// <summary>
        /// 寿命系数
        /// </summary>
        public int mod_age;
        /// <summary>
        /// 施法距离
        /// </summary>
        public float spell_range;
        /// <summary>
        /// 施法距离系数
        /// </summary>
        public float mod_spell_range;
        /// <summary>
        /// 吸血比率，以最终伤害计算
        /// </summary>
        public float vampire;
        /// <summary>
        /// 反伤比率，以最终伤害计算
        /// </summary>
        public float anti_injury;
        /// <summary>
        /// 法抗
        /// </summary>
        public int spell_armor;
        /// <summary>
        /// 法抗系数
        /// </summary>
        public float mod_spell_armor;
        /// <summary>
        /// 生命回复每月
        /// </summary>
        public int health_regen;
        /// <summary>
        /// 生命回复系数
        /// </summary>
        public float mod_health_regen;
        /// <summary>
        /// 灵力上限
        /// </summary>
        public int wakan;
        /// <summary>
        /// 灵力上限系数
        /// </summary>
        public float mod_wakan;
        /// <summary>
        /// 灵力恢复每月
        /// </summary>
        public int wakan_regen;
        /// <summary>
        /// 灵力恢复系数
        /// </summary>
        public float mod_wakan_regen;
        /// <summary>
        /// 修炼速度系数
        /// </summary>
        public float mod_cultivation;
        // TODO: 补充其他函数
        /// <summary>
        /// 抗暴率
        /// </summary>
        public float anti_crit;
        /// <summary>
        /// 抗暴率系数
        /// </summary>
        public float mod_anti_crit;
        /// <summary>
        /// 抗暴伤率
        /// </summary>
        public float anti_crit_damage;
        /// <summary>
        /// 抗暴伤系数
        /// </summary>
        public float mod_anti_crit_damage;
        /// <summary>
        /// 总抗暴率
        /// </summary>
        public float s_anti_crit;
        /// <summary>
        /// 法穿
        /// </summary>
        public int anti_spell_armor;
        /// <summary>
        /// 物穿
        /// </summary>
        public int anti_armor;
        /// <summary>
        /// 物穿系数
        /// </summary>
        public float mod_anti_armor;
        /// <summary>
        /// 法穿系数
        /// </summary>
        public float mod_anti_spell_armor;
        /// <summary>
        /// 抗时停
        /// </summary>
        public bool anti_time_stop;
        /// <summary>
        /// 附加元素
        /// </summary>
        public CW_Element element;
        /// <summary>
        /// 原版属性
        /// </summary>
        public BaseStats base_stats;
        public CW_BaseStats()
        {
            this.age_bonus = 0;
            this.anti_armor = 0;
            this.anti_crit = 0;
            this.anti_crit_damage = 0;
            this.anti_injury = 0;
            this.anti_spell_armor = 0;
            this.anti_time_stop = false;
            this.base_stats = new BaseStats();
            this.element = new CW_Element(random_generate: true, comp_type: false);
            this.health_regen = 0;
            this.mod_age = 0;
            this.mod_anti_armor = 0;
            this.mod_anti_crit = 0;
            this.mod_anti_crit_damage = 0;
            this.mod_anti_spell_armor = 0;
            this.mod_cultivation = 0;
            this.mod_health_regen = 0;
            this.mod_shied = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shied = 0;
            this.soul = 0;
            this.soul_regen = 0;
            this.spell_range = 0;
            this.spell_armor = 0;
            this.s_anti_crit = 0;
            this.vampire = 0;
            this.wakan = 0;
            this.wakan_regen = 0;
        }
        internal CW_BaseStats(BaseStats baseStats)
        {
            this.element = new CW_Element(random_generate: true, comp_type: false);
            #region Extend Stats
            this.age_bonus = 0;
            this.anti_armor = 0;
            this.anti_crit = 0;
            this.anti_crit_damage = 0;
            this.anti_injury = 0;
            this.anti_spell_armor = 0;
            this.anti_time_stop = false;
            this.health_regen = 0;
            this.mod_age = 0;
            this.mod_anti_armor = 0;
            this.mod_anti_crit = 0;
            this.mod_anti_crit_damage = 0;
            this.mod_anti_spell_armor = 0;
            this.mod_cultivation = 0;
            this.mod_health_regen = 0;
            this.mod_shied = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shied = 0;
            this.soul = 0;
            this.soul_regen = 0;
            this.spell_range = 0;
            this.spell_armor = 0;
            this.s_anti_crit = 0;
            this.vampire = 0;
            this.wakan = 0;
            this.wakan_regen = 0;
            #endregion
            this.base_stats = baseStats;
        }
        internal CW_BaseStats(CW_BaseStats copy)
        {
            this.base_stats = new BaseStats();
            this.element = copy.element.deepcopy();
            #region Extend Stats
            this.age_bonus = copy.age_bonus;
            this.anti_armor = copy.anti_armor;
            this.anti_crit = copy.anti_crit;
            this.anti_crit_damage = copy.anti_crit_damage;
            this.anti_injury = copy.anti_injury;
            this.anti_spell_armor = copy.anti_spell_armor;
            this.anti_time_stop = copy.anti_time_stop;
            this.health_regen = copy.health_regen;
            this.mod_age = copy.mod_age;
            this.mod_anti_armor = copy.mod_anti_armor;
            this.mod_anti_crit = copy.mod_anti_crit;
            this.mod_anti_crit_damage = copy.mod_anti_crit_damage;
            this.mod_anti_spell_armor = copy.mod_anti_spell_armor;
            this.mod_cultivation = copy.mod_cultivation;
            this.mod_health_regen = copy.mod_health_regen;
            this.mod_shied = copy.mod_shied;
            this.mod_soul = copy.mod_soul;
            this.mod_soul_regen = copy.mod_soul_regen;
            this.mod_spell_armor = copy.mod_spell_armor;
            this.mod_spell_range = copy.mod_spell_range;
            this.mod_wakan = copy.mod_wakan;
            this.mod_wakan_regen = copy.mod_wakan_regen;
            this.shied = copy.shied;
            this.soul = copy.soul;
            this.soul_regen = copy.soul_regen;
            this.spell_range = copy.spell_range;
            this.spell_armor = copy.spell_armor;
            this.s_anti_crit = copy.s_anti_crit;
            this.vampire = copy.vampire;
            this.wakan = copy.wakan;
            this.wakan_regen = copy.wakan_regen;
            #endregion
            #region BaseStats
            this.base_stats.personality_rationality = copy.base_stats.personality_rationality;
            this.base_stats.personality_aggression = copy.base_stats.personality_aggression;
            this.base_stats.personality_diplomatic = copy.base_stats.personality_diplomatic;
            this.base_stats.personality_administration = copy.base_stats.personality_administration;
            this.base_stats.opinion = copy.base_stats.opinion;
            this.base_stats.loyalty_traits = copy.base_stats.loyalty_traits;
            this.base_stats.loyalty_mood = copy.base_stats.loyalty_mood;
            this.base_stats.scale = copy.base_stats.scale;
            this.base_stats.damage = copy.base_stats.damage;
            this.base_stats.attackSpeed = copy.base_stats.attackSpeed;
            this.base_stats.speed = copy.base_stats.speed;
            this.base_stats.health = copy.base_stats.health;
            this.base_stats.armor = copy.base_stats.armor;
            this.base_stats.diplomacy = copy.base_stats.diplomacy;
            this.base_stats.warfare = copy.base_stats.warfare;
            this.base_stats.stewardship = copy.base_stats.stewardship;
            this.base_stats.intelligence = copy.base_stats.intelligence;
            this.base_stats.army = copy.base_stats.army;
            this.base_stats.cities = copy.base_stats.cities;
            this.base_stats.zone_range = copy.base_stats.zone_range;
            this.base_stats.bonus_towers = copy.base_stats.bonus_towers;
            this.base_stats.dodge = copy.base_stats.dodge;
            this.base_stats.accuracy = copy.base_stats.accuracy;
            this.base_stats.targets = copy.base_stats.targets;
            this.base_stats.projectiles = copy.base_stats.projectiles;
            this.base_stats.crit = copy.base_stats.crit;
            this.base_stats.damageCritMod = copy.base_stats.damageCritMod;
            this.base_stats.range = copy.base_stats.range;
            this.base_stats.size = copy.base_stats.size;
            this.base_stats.areaOfEffect = copy.base_stats.areaOfEffect;
            this.base_stats.knockback = copy.base_stats.knockback;
            this.base_stats.knockbackReduction = copy.base_stats.knockbackReduction;
            this.base_stats.mod_health = copy.base_stats.mod_health;
            this.base_stats.mod_damage = copy.base_stats.mod_damage;
            this.base_stats.mod_armor = copy.base_stats.mod_armor;
            this.base_stats.mod_crit = copy.base_stats.mod_crit;
            this.base_stats.mod_diplomacy = copy.base_stats.mod_diplomacy;
            this.base_stats.mod_speed = copy.base_stats.mod_speed;
            this.base_stats.mod_supply_timer = copy.base_stats.mod_supply_timer;
            #endregion
        }
        public void addStats(CW_BaseStats CW_basestats, bool except_element = true)
        {
            if (!except_element)
            {
                for(int i = 0; i < Others.CW_Constants.base_element_types; i++)
                {
                    element.base_elements[i] += CW_basestats.element.base_elements[i];
                }
            }
            #region Extend Stats
            this.age_bonus += CW_basestats.age_bonus;
            this.anti_armor += CW_basestats.anti_armor;
            this.anti_crit += CW_basestats.anti_crit;
            this.anti_crit_damage += CW_basestats.anti_crit_damage;
            this.anti_injury += CW_basestats.anti_injury;
            this.anti_spell_armor += CW_basestats.anti_spell_armor;
            this.anti_time_stop |= CW_basestats.anti_time_stop;
            this.health_regen += CW_basestats.health_regen;
            this.mod_age += CW_basestats.mod_age;
            this.mod_anti_armor += CW_basestats.mod_anti_armor;
            this.mod_anti_crit += CW_basestats.mod_anti_crit;
            this.mod_anti_crit_damage += CW_basestats.mod_anti_crit_damage;
            this.mod_anti_spell_armor += CW_basestats.mod_anti_spell_armor;
            this.mod_cultivation += CW_basestats.mod_cultivation;
            this.mod_health_regen += CW_basestats.mod_health_regen;
            this.mod_shied += CW_basestats.mod_shied;
            this.mod_soul += CW_basestats.mod_soul;
            this.mod_soul_regen += CW_basestats.mod_soul_regen;
            this.mod_spell_armor += CW_basestats.mod_spell_armor;
            this.mod_spell_range += CW_basestats.mod_spell_range;
            this.mod_wakan += CW_basestats.mod_wakan;
            this.mod_wakan_regen += CW_basestats.mod_wakan_regen;
            this.shied += CW_basestats.shied;
            this.soul += CW_basestats.soul;
            this.soul_regen += CW_basestats.soul_regen;
            this.spell_range += CW_basestats.spell_range;
            this.spell_armor += CW_basestats.spell_armor;
            this.s_anti_crit += CW_basestats.s_anti_crit;
            this.vampire += CW_basestats.vampire;
            this.wakan += CW_basestats.wakan;
            this.wakan_regen += CW_basestats.wakan_regen;
            #endregion
            addStats(CW_basestats.base_stats);
        }
        public void addStats(BaseStats pStats)
        {
            this.base_stats.personality_rationality += pStats.personality_rationality;
            this.base_stats.personality_aggression += pStats.personality_aggression;
            this.base_stats.personality_diplomatic += pStats.personality_diplomatic;
            this.base_stats.personality_administration += pStats.personality_administration;
            this.base_stats.opinion += pStats.opinion;
            this.base_stats.loyalty_traits += pStats.loyalty_traits;
            this.base_stats.loyalty_mood += pStats.loyalty_mood;
            this.base_stats.scale += pStats.scale;
            this.base_stats.damage += pStats.damage;
            this.base_stats.attackSpeed += pStats.attackSpeed;
            this.base_stats.speed += pStats.speed;
            this.base_stats.health += pStats.health;
            this.base_stats.armor += pStats.armor;
            this.base_stats.diplomacy += pStats.diplomacy;
            this.base_stats.warfare += pStats.warfare;
            this.base_stats.stewardship += pStats.stewardship;
            this.base_stats.intelligence += pStats.intelligence;
            this.base_stats.army += pStats.army;
            this.base_stats.cities += pStats.cities;
            this.base_stats.zone_range += pStats.zone_range;
            this.base_stats.bonus_towers += pStats.bonus_towers;
            this.base_stats.dodge += pStats.dodge;
            this.base_stats.accuracy += pStats.accuracy;
            this.base_stats.targets += pStats.targets;
            this.base_stats.projectiles += pStats.projectiles;
            this.base_stats.crit += pStats.crit;
            this.base_stats.damageCritMod += pStats.damageCritMod;
            this.base_stats.range += pStats.range;
            this.base_stats.size += pStats.size;
            this.base_stats.areaOfEffect += pStats.areaOfEffect;
            this.base_stats.knockback += pStats.knockback;
            this.base_stats.knockbackReduction += pStats.knockbackReduction;
            this.base_stats.mod_health += pStats.mod_health;
            this.base_stats.mod_damage += pStats.mod_damage;
            this.base_stats.mod_armor += pStats.mod_armor;
            this.base_stats.mod_crit += pStats.mod_crit;
            this.base_stats.mod_diplomacy += pStats.mod_diplomacy;
            this.base_stats.mod_speed += pStats.mod_speed;
            this.base_stats.mod_supply_timer += pStats.mod_supply_timer;
        }
        public void clear(bool except_element = true)
        {
            if (!except_element)
            {
                this.element = new CW_Element(random_generate: true, comp_type: false);
            }
            #region Extend Stats
            this.age_bonus = 0;
            this.anti_armor = 0;
            this.anti_crit = 0;
            this.anti_crit_damage = 0;
            this.anti_injury = 0;
            this.anti_spell_armor = 0;
            this.anti_time_stop = false;
            this.health_regen = 0;
            this.mod_age = 0;
            this.mod_anti_armor = 0;
            this.mod_anti_crit = 0;
            this.mod_anti_crit_damage = 0;
            this.mod_anti_spell_armor = 0;
            this.mod_cultivation = 0;
            this.mod_health_regen = 0;
            this.mod_shied = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shied = 0;
            this.soul = 0;
            this.soul_regen = 0;
            this.spell_range = 0;
            this.spell_armor = 0;
            this.s_anti_crit = 0;
            this.vampire = 0;
            this.wakan = 0;
            this.wakan_regen = 0;
            #endregion
            #region BaseStats
            this.base_stats.s_crit_chance = 0f;
            this.base_stats.personality_rationality = 0f;
            this.base_stats.personality_aggression = 0f;
            this.base_stats.personality_diplomatic = 0f;
            this.base_stats.personality_administration = 0f;
            this.base_stats.opinion = 0;
            this.base_stats.loyalty_mood = 0;
            this.base_stats.loyalty_traits = 0;
            this.base_stats.mod_supply_timer = 0f;
            this.base_stats.scale = 0f;
            this.base_stats.damage = 0;
            this.base_stats.attackSpeed = 0f;
            this.base_stats.projectiles = 0;
            this.base_stats.speed = 0f;
            this.base_stats.health = 0;
            this.base_stats.armor = 0;
            this.base_stats.diplomacy = 0;
            this.base_stats.warfare = 0;
            this.base_stats.stewardship = 0;
            this.base_stats.intelligence = 0;
            this.base_stats.army = 0;
            this.base_stats.cities = 0;
            this.base_stats.zone_range = 0;
            this.base_stats.bonus_towers = 0;
            this.base_stats.dodge = 0f;
            this.base_stats.accuracy = 0f;
            this.base_stats.targets = 0;
            this.base_stats.crit = 0f;
            this.base_stats.damageCritMod = 0f;
            this.base_stats.range = 0f;
            this.base_stats.size = 0f;
            this.base_stats.areaOfEffect = 0f;
            this.base_stats.knockback = 0f;
            this.base_stats.knockbackReduction = 0f;
            this.base_stats.mod_health = 0f;
            this.base_stats.mod_damage = 0f;
            this.base_stats.mod_armor = 0f;
            this.base_stats.mod_crit = 0f;
            this.base_stats.mod_diplomacy = 0f;
            this.base_stats.mod_speed = 0f;
            this.base_stats.mod_attackSpeed = 0f;
            #endregion
        }
        public void apply_mod()
        {
            this.base_stats.armor += (int)(this.base_stats.armor * this.base_stats.mod_armor / 100f);
            this.base_stats.attackSpeed *= 1f + this.base_stats.mod_attackSpeed / 100f;
            this.base_stats.crit *= 1f + this.base_stats.mod_crit / 100f;
            this.base_stats.damage += (int)(this.base_stats.damage * this.base_stats.mod_damage / 100f);
            this.base_stats.diplomacy += (int)(this.base_stats.diplomacy * this.base_stats.mod_diplomacy / 100f);
            this.base_stats.health += (int)(this.base_stats.health * this.base_stats.mod_health / 100);
            this.base_stats.speed *= 1f + this.base_stats.mod_speed / 100f;
            this.age_bonus += (int)(this.age_bonus * this.mod_age / 100f);
            this.anti_armor += (int)(this.anti_armor * this.mod_anti_armor / 100f);
            this.anti_crit *= 1f + this.mod_anti_crit / 100f;
            this.anti_crit_damage *= 1f + this.mod_anti_crit_damage / 100f;
            this.anti_spell_armor += (int)(this.anti_spell_armor * this.mod_anti_spell_armor / 100f);
            this.health_regen += (int)(this.health_regen * this.mod_health_regen / 100f);
            this.shied += (int)(this.shied * this.mod_shied / 100f);
            this.soul += (int)(this.soul * this.mod_soul / 100f);
            this.soul_regen += (int)(this.soul_regen * this.mod_soul_regen / 100f);
            this.spell_armor += (int)(this.spell_armor * this.mod_spell_armor / 100f);
            this.spell_range *= 1f + this.mod_spell_range / 100f;
            this.wakan += (int)(this.wakan * this.mod_wakan / 100f);
            this.wakan_regen += (int)(this.wakan_regen * this.mod_wakan_regen / 100f);
        }
        public void apply_others()
        {
            //throw new NotImplementedException();
        }
        public void no_zero()
        {
            //throw new NotImplementedException();
        }
        public void normalize(bool normalize_element = true, int element_normalize_ceil = 100)
        {
            this.base_stats.normalize();
            if(normalize_element) this.element.normalize(element_normalize_ceil);
            // Others Normalization.
        }
        public CW_BaseStats deepcopy()
        {
            return new CW_BaseStats(this);
        }
    }
}
