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
        /// 法伤减免，以最终伤害计算
        /// </summary>
        public float spell_relief;
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
            this.anti_injury = 0;
            this.base_stats = new BaseStats();
            this.element = new CW_Element();
            this.health_regen = 0;
            this.mod_age = 0;
            this.mod_cultivation = 0;
            this.mod_health_regen = 0;
            this.mod_shied = 0;
            this.mod_soul = 0;
            this.mod_soul_regen = 0;
            this.mod_spell_range = 0;
            this.mod_wakan = 0;
            this.mod_wakan_regen = 0;
            this.shied = 0;
            this.soul = 0;
            this.soul_regen = 0;
            this.spell_range = 0;
            this.spell_relief = 0;
            this.vampire = 0;
            this.wakan = 0;
            this.wakan_regen = 0;
        }
        public void addStats(CW_BaseStats CW_basestats, bool except_element = true)
        {
            if (!except_element)
            {
                for(int i = 0; i < Constants.CW_Constants.base_element_types; i++)
                {
                    element.base_elements[i] += CW_basestats.element.base_elements[i];
                }
            }
            #region Extend Stats
            this.age_bonus += CW_basestats.age_bonus;
            this.anti_injury += CW_basestats.anti_injury;
            this.health_regen += CW_basestats.health_regen;
            this.mod_age += CW_basestats.mod_age;
            this.mod_cultivation += CW_basestats.mod_cultivation;
            this.mod_health_regen += CW_basestats.mod_health_regen;
            this.mod_shied += CW_basestats.mod_shied;
            this.mod_soul += CW_basestats.mod_soul;
            this.mod_soul_regen += CW_basestats.mod_soul_regen;
            this.mod_spell_range += CW_basestats.mod_spell_range;
            this.mod_wakan += CW_basestats.mod_wakan;
            this.mod_wakan_regen += CW_basestats.mod_wakan_regen;
            this.shied += CW_basestats.shied;
            this.soul += CW_basestats.soul;
            this.soul_regen += CW_basestats.soul_regen;
            this.spell_range += CW_basestats.spell_range;
            this.spell_relief += CW_basestats.spell_relief;
            this.vampire += CW_basestats.vampire;
            this.wakan += CW_basestats.wakan;
            this.wakan_regen += CW_basestats.wakan_regen;
#endregion
            #region BaseStats
            this.base_stats.personality_rationality += CW_basestats.base_stats.personality_rationality;
            this.base_stats.personality_aggression += CW_basestats.base_stats.personality_aggression;
            this.base_stats.personality_diplomatic += CW_basestats.base_stats.personality_diplomatic;
            this.base_stats.personality_administration += CW_basestats.base_stats.personality_administration;
            this.base_stats.opinion += CW_basestats.base_stats.opinion;
            this.base_stats.loyalty_traits += CW_basestats.base_stats.loyalty_traits;
            this.base_stats.loyalty_mood += CW_basestats.base_stats.loyalty_mood;
            this.base_stats.scale += CW_basestats.base_stats.scale;
            this.base_stats.damage += CW_basestats.base_stats.damage;
            this.base_stats.attackSpeed += CW_basestats.base_stats.attackSpeed;
            this.base_stats.speed += CW_basestats.base_stats.speed;
            this.base_stats.health += CW_basestats.base_stats.health;
            this.base_stats.armor += CW_basestats.base_stats.armor;
            this.base_stats.diplomacy += CW_basestats.base_stats.diplomacy;
            this.base_stats.warfare += CW_basestats.base_stats.warfare;
            this.base_stats.stewardship += CW_basestats.base_stats.stewardship;
            this.base_stats.intelligence += CW_basestats.base_stats.intelligence;
            this.base_stats.army += CW_basestats.base_stats.army;
            this.base_stats.cities += CW_basestats.base_stats.cities;
            this.base_stats.zone_range += CW_basestats.base_stats.zone_range;
            this.base_stats.bonus_towers += CW_basestats.base_stats.bonus_towers;
            this.base_stats.dodge += CW_basestats.base_stats.dodge;
            this.base_stats.accuracy += CW_basestats.base_stats.accuracy;
            this.base_stats.targets += CW_basestats.base_stats.targets;
            this.base_stats.projectiles += CW_basestats.base_stats.projectiles;
            this.base_stats.crit += CW_basestats.base_stats.crit;
            this.base_stats.damageCritMod += CW_basestats.base_stats.damageCritMod;
            this.base_stats.range += CW_basestats.base_stats.range;
            this.base_stats.size += CW_basestats.base_stats.size;
            this.base_stats.areaOfEffect += CW_basestats.base_stats.areaOfEffect;
            this.base_stats.knockback += CW_basestats.base_stats.knockback;
            this.base_stats.knockbackReduction += CW_basestats.base_stats.knockbackReduction;
            this.base_stats.mod_health += CW_basestats.base_stats.mod_health;
            this.base_stats.mod_damage += CW_basestats.base_stats.mod_damage;
            this.base_stats.mod_armor += CW_basestats.base_stats.mod_armor;
            this.base_stats.mod_crit += CW_basestats.base_stats.mod_crit;
            this.base_stats.mod_diplomacy += CW_basestats.base_stats.mod_diplomacy;
            this.base_stats.mod_speed += CW_basestats.base_stats.mod_speed;
            this.base_stats.mod_supply_timer += CW_basestats.base_stats.mod_supply_timer;
            #endregion
        }
    }
}
