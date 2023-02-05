using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public int shield;
        /// <summary>
        /// 护盾系数
        /// </summary>
        public float mod_shield;
        /// <summary>
        /// 护盾恢复/月
        /// </summary>
        public int shield_regen;
        /// <summary>
        /// 护盾恢复系数
        /// </summary>
        public float mod_shield_regen;
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
        public float mod_age;
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
            //this.element = new CW_Element(random_generate: true, comp_type: false);
            this.health_regen = 0;
            this.mod_age = 0;
            this.mod_anti_armor = 0;
            this.mod_anti_crit = 0;
            this.mod_anti_crit_damage = 0;
            this.mod_anti_spell_armor = 0;
            this.mod_cultivation = 0;
            this.mod_health_regen = 0;
            this.mod_shield = 0;
            this.mod_shield_regen = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shield = 0;
            this.shield_regen = 0;
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
            //this.element = new CW_Element(random_generate: true, comp_type: false);
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
            this.mod_shield = 0;
            this.mod_shield_regen = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shield = 0;
            this.shield_regen = 0;
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
            if(copy.element!=null) this.element = copy.element.deepcopy();
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
            this.mod_shield = copy.mod_shield;
            this.mod_shield_regen = copy.mod_shield_regen;
            this.mod_soul = copy.mod_soul;
            this.mod_soul_regen = copy.mod_soul_regen;
            this.mod_spell_armor = copy.mod_spell_armor;
            this.mod_spell_range = copy.mod_spell_range;
            this.mod_wakan = copy.mod_wakan;
            this.mod_wakan_regen = copy.mod_wakan_regen;
            this.shield = copy.shield;
            this.shield_regen = copy.shield_regen;
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
        public CW_Element get_element()
        {
            if (this.element == null) this.element = new CW_Element();
            return this.element;
        }
        public void addStats(CW_BaseStats CW_basestats, bool except_element = true)
        {
            if (CW_basestats == null) return;
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
            this.mod_shield += CW_basestats.mod_shield;
            this.mod_shield_regen += CW_basestats.mod_shield_regen;
            this.mod_soul += CW_basestats.mod_soul;
            this.mod_soul_regen += CW_basestats.mod_soul_regen;
            this.mod_spell_armor += CW_basestats.mod_spell_armor;
            this.mod_spell_range += CW_basestats.mod_spell_range;
            this.mod_wakan += CW_basestats.mod_wakan;
            this.mod_wakan_regen += CW_basestats.mod_wakan_regen;
            this.shield += CW_basestats.shield;
            this.shield_regen += CW_basestats.shield_regen;
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
            if (pStats == null) return;
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
        public void change_to_better(CW_BaseStats another)
        {
            if (another == null) return;
            #region Extend Stats
            if(another.age_bonus > age_bonus) this.age_bonus = another.age_bonus;
            if (another.anti_armor > anti_armor) this.anti_armor = another.anti_armor;
            if (another.anti_crit > anti_crit) this.anti_crit = another.anti_crit;
            if (another.anti_crit_damage > anti_crit_damage) this.anti_crit_damage = another.anti_crit_damage;
            if (another.anti_injury > anti_injury) this.anti_injury = another.anti_injury;
            if (another.anti_spell_armor > anti_spell_armor) this.anti_spell_armor = another.anti_spell_armor;
            this.anti_time_stop = another.anti_time_stop;
            if (another.health_regen > health_regen) this.health_regen = another.health_regen;
            if (another.mod_age > mod_age) this.mod_age = another.mod_age;
            if (another.mod_anti_armor > mod_anti_armor) this.mod_anti_armor = another.mod_anti_armor;
            if (another.mod_anti_crit > mod_anti_crit) this.mod_anti_crit = another.mod_anti_crit;
            if (another.mod_anti_crit_damage > mod_anti_crit_damage) this.mod_anti_crit_damage = another.mod_anti_crit_damage;
            if (another.mod_anti_spell_armor > mod_anti_spell_armor) this.mod_anti_spell_armor = another.mod_anti_spell_armor;
            if (another.mod_cultivation > mod_cultivation) this.mod_cultivation = another.mod_cultivation;
            if (another.mod_health_regen > mod_health_regen) this.mod_health_regen = another.mod_health_regen;
            if (another.mod_shield > mod_shield) this.mod_shield = another.mod_shield;
            if (another.mod_shield_regen > mod_shield_regen) this.mod_shield_regen = another.mod_shield_regen;
            if (another.mod_soul > mod_soul) this.mod_soul = another.mod_soul;
            if (another.mod_soul_regen > mod_soul_regen) this.mod_soul_regen = another.mod_soul_regen;
            if (another.mod_spell_armor > mod_spell_armor) this.mod_spell_armor = another.mod_spell_armor;
            if (another.mod_spell_range > mod_spell_range) this.mod_spell_range = another.mod_spell_range;
            if (another.mod_wakan > mod_wakan) this.mod_wakan = another.mod_wakan;
            if (another.mod_wakan_regen > mod_wakan_regen) this.mod_wakan_regen = another.mod_wakan_regen;
            if (another.shield > shield) this.shield = another.shield;
            if (another.shield_regen > shield_regen) this.shield_regen = another.shield_regen;
            if (another.soul > soul) this.soul = another.soul;
            if (another.soul_regen > soul_regen) this.soul_regen = another.soul_regen;
            if (another.spell_range > spell_range) this.spell_range = another.spell_range;
            if (another.spell_armor > spell_armor) this.spell_armor = another.spell_armor;
            if (another.s_anti_crit > s_anti_crit) this.s_anti_crit = another.s_anti_crit;
            if (another.vampire > vampire) this.vampire = another.vampire;
            if (another.wakan > wakan) this.wakan = another.wakan;
            if (another.wakan_regen > wakan_regen) this.wakan_regen = another.wakan_regen;
            #endregion
            #region BaseStats
            if(another.base_stats.personality_rationality > base_stats.personality_rationality) this.base_stats.personality_rationality = another.base_stats.personality_rationality;
            if (another.base_stats.personality_aggression > base_stats.personality_aggression) this.base_stats.personality_aggression = another.base_stats.personality_aggression;
            if (another.base_stats.personality_diplomatic > base_stats.personality_diplomatic) this.base_stats.personality_diplomatic = another.base_stats.personality_diplomatic;
            if (another.base_stats.personality_administration > base_stats.personality_administration) this.base_stats.personality_administration = another.base_stats.personality_administration;
            if (another.base_stats.opinion > base_stats.opinion) this.base_stats.opinion = another.base_stats.opinion;
            if (another.base_stats.loyalty_traits > base_stats.loyalty_traits) this.base_stats.loyalty_traits = another.base_stats.loyalty_traits;
            if (another.base_stats.loyalty_mood > base_stats.loyalty_mood) this.base_stats.loyalty_mood = another.base_stats.loyalty_mood;
            if (another.base_stats.scale > base_stats.scale) this.base_stats.scale = another.base_stats.scale;
            if (another.base_stats.damage > base_stats.damage) this.base_stats.damage = another.base_stats.damage;
            if (another.base_stats.attackSpeed > base_stats.attackSpeed) this.base_stats.attackSpeed = another.base_stats.attackSpeed;
            if (another.base_stats.speed > base_stats.speed) this.base_stats.speed = another.base_stats.speed;
            if (another.base_stats.health > base_stats.health) this.base_stats.health = another.base_stats.health;
            if (another.base_stats.armor > base_stats.armor) this.base_stats.armor = another.base_stats.armor;
            if (another.base_stats.diplomacy > base_stats.diplomacy) this.base_stats.diplomacy = another.base_stats.diplomacy;
            if (another.base_stats.warfare > base_stats.warfare) this.base_stats.warfare = another.base_stats.warfare;
            if (another.base_stats.stewardship > base_stats.stewardship) this.base_stats.stewardship = another.base_stats.stewardship;
            if (another.base_stats.intelligence > base_stats.intelligence) this.base_stats.intelligence = another.base_stats.intelligence;
            if (another.base_stats.army > base_stats.army) this.base_stats.army = another.base_stats.army;
            if (another.base_stats.cities > base_stats.cities) this.base_stats.cities = another.base_stats.cities;
            if (another.base_stats.zone_range > base_stats.zone_range) this.base_stats.zone_range = another.base_stats.zone_range;
            if (another.base_stats.bonus_towers > base_stats.bonus_towers) this.base_stats.bonus_towers = another.base_stats.bonus_towers;
            if (another.base_stats.dodge > base_stats.dodge) this.base_stats.dodge = another.base_stats.dodge;
            if (another.base_stats.accuracy > base_stats.accuracy) this.base_stats.accuracy = another.base_stats.accuracy;
            if (another.base_stats.targets > base_stats.targets) this.base_stats.targets = another.base_stats.targets;
            if (another.base_stats.projectiles > base_stats.projectiles) this.base_stats.projectiles = another.base_stats.projectiles;
            if (another.base_stats.crit > base_stats.crit) this.base_stats.crit = another.base_stats.crit;
            if (another.base_stats.damageCritMod > base_stats.damageCritMod) this.base_stats.damageCritMod = another.base_stats.damageCritMod;
            if (another.base_stats.range > base_stats.range) this.base_stats.range = another.base_stats.range;
            if (another.base_stats.size > base_stats.size) this.base_stats.size = another.base_stats.size;
            if (another.base_stats.areaOfEffect > base_stats.areaOfEffect) this.base_stats.areaOfEffect = another.base_stats.areaOfEffect;
            if (another.base_stats.knockback > base_stats.knockback) this.base_stats.knockback = another.base_stats.knockback;
            if (another.base_stats.knockbackReduction > base_stats.knockbackReduction) this.base_stats.knockbackReduction = another.base_stats.knockbackReduction;
            if (another.base_stats.mod_health > base_stats.mod_health) this.base_stats.mod_health = another.base_stats.mod_health;
            if (another.base_stats.mod_damage > base_stats.mod_damage) this.base_stats.mod_damage = another.base_stats.mod_damage;
            if (another.base_stats.mod_armor > base_stats.mod_armor) this.base_stats.mod_armor = another.base_stats.mod_armor;
            if (another.base_stats.mod_crit > base_stats.mod_crit) this.base_stats.mod_crit = another.base_stats.mod_crit;
            if (another.base_stats.mod_diplomacy > base_stats.mod_diplomacy) this.base_stats.mod_diplomacy = another.base_stats.mod_diplomacy;
            if (another.base_stats.mod_speed > base_stats.mod_speed) this.base_stats.mod_speed = another.base_stats.mod_speed;
            if (another.base_stats.mod_supply_timer > base_stats.mod_supply_timer) this.base_stats.mod_supply_timer = another.base_stats.mod_supply_timer;
            #endregion
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
            this.mod_shield = 0;
            this.mod_shield_regen = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_spell_armor = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shield = 0;
            this.shield_regen = 0;
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
            this.base_stats.health += (int)(this.base_stats.health * Mathf.Max(this.base_stats.mod_health, -90f) / 100);
            this.base_stats.speed *= 1f + this.base_stats.mod_speed / 100f;
            this.age_bonus += (int)(this.age_bonus * this.mod_age / 100f);
            this.anti_armor += (int)(this.anti_armor * this.mod_anti_armor / 100f);
            this.anti_crit *= 1f + this.mod_anti_crit / 100f;
            this.anti_crit_damage *= 1f + this.mod_anti_crit_damage / 100f;
            this.anti_spell_armor += (int)(this.anti_spell_armor * this.mod_anti_spell_armor / 100f);
            this.health_regen += (int)(this.health_regen * this.mod_health_regen / 100f);
            this.shield += (int)(this.shield * this.mod_shield / 100f);
            this.shield_regen += (int)(this.shield_regen * this.mod_shield_regen / 100f);
            this.soul += (int)(this.soul * this.mod_soul / 100f);
            this.soul_regen += (int)(this.soul_regen * this.mod_soul_regen / 100f);
            this.spell_armor += (int)(this.spell_armor * this.mod_spell_armor / 100f);
            this.spell_range *= 1f + this.mod_spell_range / 100f;
            this.wakan += (int)(this.wakan * Mathf.Max(this.mod_wakan,-70f) / 100f);
            this.wakan_regen += (int)(this.wakan_regen * this.mod_wakan_regen / 100f);
        }
        public void apply_others()
        {
            this.base_stats.s_crit_chance = this.base_stats.crit / 100f;
            this.base_stats.zone_range = this.base_stats.stewardship / 10;
            this.base_stats.cities = this.base_stats.stewardship / 6;
            this.base_stats.army = this.base_stats.warfare + 5;
            this.base_stats.bonus_towers = this.base_stats.warfare / 10;

            this.base_stats.bonus_towers = this.base_stats.bonus_towers > 2 ? 2 : this.base_stats.bonus_towers;
            this.base_stats.army = this.base_stats.army < 5 ? 5 : this.base_stats.army;
            
            //throw new NotImplementedException();
        }
        public void no_zero_for_actor()
        {
            this.anti_armor = this.anti_armor < 0 ? 0 : this.anti_armor;
            this.anti_crit = this.anti_crit < 0?0: this.anti_crit;
            this.anti_crit_damage = this.anti_crit_damage < 0?0: this.anti_crit_damage;
            this.anti_injury = this.anti_injury<0?0: this.anti_injury;
            this.anti_spell_armor = this.anti_spell_armor<0? 0 : this.anti_spell_armor;
            this.health_regen = this.health_regen<0?0: this.health_regen;
            this.shield = this.shield<0?0: this.shield;
            this.shield_regen = this.shield_regen<0?0: this.shield_regen;
            this.soul = this.soul<0?0: this.soul;
            this.soul_regen = this.soul_regen<0?0: this.soul_regen;
            this.spell_range = this.spell_range < 0?0: this.spell_range;
            this.spell_armor = (this.spell_armor < 0)?0: this.spell_armor;
            this.vampire = this.vampire < 0?0: this.vampire;
            this.wakan = this.wakan < 0?0: this.wakan;
            this.wakan_regen = this.wakan_regen<0?0: this.wakan_regen;

            this.base_stats.damage = this.base_stats.damage < 0 ? 0 : this.base_stats.damage;
            this.base_stats.attackSpeed = this.base_stats.attackSpeed<0 ? 0 : this.base_stats.attackSpeed;
            this.base_stats.speed = this.base_stats.speed < 0 ? 0 : this.base_stats.speed;
            this.base_stats.health = this.base_stats.health<0 ? 0 : this.base_stats.health;
            this.base_stats.armor = this.base_stats.armor < 0 ? 0 : this.base_stats.armor;
            this.base_stats.crit = this.base_stats.crit <0? 0 : this.base_stats.crit;
            this.base_stats.damageCritMod = this.base_stats.damageCritMod< 0 ? 0 : this.base_stats.damageCritMod;
            this.base_stats.knockback = this.base_stats.knockback < 0 ? 0 : this.base_stats.knockback;
            this.base_stats.diplomacy = this.base_stats.diplomacy<0 ? 0 : this.base_stats.diplomacy;
            this.base_stats.dodge = this.base_stats.dodge < 0 ? 0 : this.base_stats.dodge;
            this.base_stats.accuracy = this.base_stats.accuracy < 10 ? 10 : this.base_stats.accuracy;
        }
        public void normalize(bool normalize_element = true, int element_normalize_ceil = 100)
        {
            this.base_stats.normalize();
            if(normalize_element && this.element!=null) this.element.normalize(element_normalize_ceil);
            // Others Normalization.
            if (this.base_stats.knockbackReduction > 90) this.base_stats.knockbackReduction = 90;
        }
        public CW_BaseStats deepcopy()
        {
            return new CW_BaseStats(this);
        }
    }
}
