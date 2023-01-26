using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal class W_Content_Cultisys
    {
        public static void add_cultisys()
        {
            int i;
            CW_Asset_CultiSys immortal = CW_Library_Manager.instance.cultisys.add(
                new CW_Asset_CultiSys()
                {
                    id = "immortal",
                    sprite_name = "icon_immortal",
                    judge = immortal_judge,
                    level_judge = immortal_level_judge,
                    addition_spell_require = new CW_Spell_Tag[] { CW_Spell_Tag.IMMORTAL}
                }
            );
            //WorldBoxConsole.Console.print(immortal == null);
            for (i = 10; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                immortal.power_level[i] = 1 + (i - 9) / 10f;
            }
            #region 修仙属性
            //物抗
            immortal.bonus_stats[0].base_stats.armor = 0;
            immortal.bonus_stats[1].base_stats.armor = 10;
            immortal.bonus_stats[2].base_stats.armor = 30;
            immortal.bonus_stats[3].base_stats.armor = 60;
            immortal.bonus_stats[4].base_stats.armor = 90;
            immortal.bonus_stats[5].base_stats.armor = 150;
            immortal.bonus_stats[6].base_stats.armor = 300;
            immortal.bonus_stats[7].base_stats.armor = 400;
            immortal.bonus_stats[8].base_stats.armor = 500;
            for (i = 9; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                immortal.bonus_stats[i].base_stats.armor = immortal.bonus_stats[i - 1].base_stats.armor + 10;
            }
            for (i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                immortal.bonus_stats[i].spell_range = Mathf.Sqrt(i) * 4;
                // 寿命
                immortal.bonus_stats[i].age_bonus = i * i * 2 + (i % 10) * 15 + (i > 0 ? immortal.bonus_stats[i-1].age_bonus:0);
                // 生命以及生命回复
                immortal.bonus_stats[i].base_stats.health = 3 * i * i * i/2 + i * 142;
                immortal.bonus_stats[i].health_regen = (int)Mathf.Sqrt(Mathf.Sqrt(immortal.bonus_stats[i].base_stats.health));
                // 攻击
                immortal.bonus_stats[i].base_stats.damage = (int)Mathf.Sqrt(immortal.bonus_stats[i].base_stats.health);
                // 灵气以及灵气恢复
                immortal.bonus_stats[i].wakan = (i+1) * (i+1) * (i+1)*2 + (i+1) * 18+24;
                immortal.bonus_stats[i].wakan_regen = (int)(Utils.CW_Utils_Others.get_raw_wakan(immortal.bonus_stats[i].wakan, immortal.power_level[i]) / Mathf.Sqrt(immortal.bonus_stats[i].wakan))/2;
                // 法抗
                immortal.bonus_stats[i].spell_armor = (int)(Mathf.Sqrt(i) * immortal.bonus_stats[i].base_stats.armor * (Mathf.Log(Utils.CW_Utils_Others.get_raw_wakan(immortal.bonus_stats[i].wakan, immortal.power_level[i]) / immortal.bonus_stats[i].wakan, Others.CW_Constants.wakan_level_co)));
                // 护盾恢复
                immortal.bonus_stats[i].shield_regen = (int)(Mathf.Sqrt(i) * Mathf.Sqrt(Mathf.Sqrt(immortal.bonus_stats[i].wakan)) * (Mathf.Log(Utils.CW_Utils_Others.get_raw_wakan(immortal.bonus_stats[i].wakan, immortal.power_level[i]) / immortal.bonus_stats[i].wakan, Others.CW_Constants.wakan_level_co) + 1) * 2);
                // 护盾
                immortal.bonus_stats[i].shield = 12 * immortal.bonus_stats[i].shield_regen;
                // 抗击退
                immortal.bonus_stats[i].base_stats.knockbackReduction = (i + 1) * 4.5f;
            }
            #endregion

            CW_Asset_CultiSys bushido = CW_Library_Manager.instance.cultisys.add(
                new CW_Asset_CultiSys()
                {
                    id = "bushido",
                    sprite_name = "icon_bushido",
                    judge = bushido_judge,
                    level_judge = bushido_level_judge,
                    addition_spell_require = new CW_Spell_Tag[] { CW_Spell_Tag.BUSHIDO, CW_Spell_Tag.ACQUIRED_POWER }
                }
            );
            for (i = 10; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.power_level[i] = 1 + (i - 9) / 10f;
            }
            //WorldBoxConsole.Console.print(bushido == null);
            #region 武道属性
            // 物抗
            for (i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.bonus_stats[i].base_stats.armor = 2 * immortal.bonus_stats[i].base_stats.armor;
            }
            bushido.bonus_stats[i - 1].base_stats.armor = 1500;
            
            for (i = 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.bonus_stats[i].age_bonus = i * i * 6 + i * 15 + (i > 0 ? bushido.bonus_stats[i-1].age_bonus:0);
                // 生命及生命回复
                bushido.bonus_stats[i].base_stats.health = 3*i*i*i+42*i;
                bushido.bonus_stats[i].health_regen = (int)Mathf.Sqrt(bushido.bonus_stats[i].base_stats.health * Utils.CW_Utils_Others.get_raw_wakan(bushido.bonus_stats[i].base_stats.health, bushido.power_level[i]))/50;
                // 攻击
                bushido.bonus_stats[i].base_stats.damage = i * i * i /2+ 2 * i * i;
                // 法抗
                bushido.bonus_stats[i].spell_armor = (int)(bushido.bonus_stats[i].base_stats.armor * Mathf.Sqrt(i));
                // 抗击退
                bushido.bonus_stats[i].base_stats.knockbackReduction = (i + 1) * 5f;
            }
            // 攻速
            for (i = 0; i < 10; i++)
            {
                bushido.bonus_stats[i].base_stats.attackSpeed = 5 * i;
            }
            for (; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.bonus_stats[i].base_stats.attackSpeed = (int)((150 - bushido.bonus_stats[i - 1].base_stats.attackSpeed) * Mathf.Log(Utils.CW_Utils_Others.get_raw_wakan(bushido.bonus_stats[i].base_stats.health, bushido.power_level[i]), Others.CW_Constants.wakan_level_co) + bushido.bonus_stats[i - 1].base_stats.attackSpeed);
            }
            // 护盾以及护盾恢复
            for(i= 0; i < Others.CW_Constants.max_cultisys_level; i++)
            {
                bushido.bonus_stats[i].shield_regen = (int)bushido.bonus_stats[i].base_stats.attackSpeed / 2;
                bushido.bonus_stats[i].shield = bushido.bonus_stats[i].shield_regen * 12;
            }
            #endregion
        }
        private static bool immortal_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            return cw_actor.cw_data.element.comp_type()!="CW_common";
        }
        private static bool bushido_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            return (cw_actor.cw_cur_stats.base_stats.health+1) * (cw_actor.cw_cur_stats.base_stats.damage +1)* (cw_actor.cw_cur_stats.base_stats.armor +1)/ ((cw_actor.cw_stats.cw_base_stats.base_stats.health+1) * (cw_actor.cw_stats.cw_base_stats.base_stats.damage+1) * (cw_actor.cw_stats.cw_base_stats.base_stats.armor+1)) >= 1.2f;
        }
        private static bool bushido_level_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            if (cw_actor.cw_data.cultisys_level[cultisys.tag] <= Others.CW_Constants.max_cultisys_level - 1 - 1 && cw_actor.fast_data.health >= cw_actor.cw_cur_stats.base_stats.health)
            {
                if (cw_actor.cw_data.cultisys_level[cultisys.tag] == 9) cw_actor.fast_data.age = 0;

                if (cw_actor.cw_status.health_level < cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1])
                {
                    cw_actor.fast_data.health = (int)Utils.CW_Utils_Others.get_raw_wakan(cw_actor.fast_data.health, cw_actor.cw_status.health_level);

                    cw_actor.cw_status.health_level = cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1];

                    cw_actor.fast_data.health = (int)Utils.CW_Utils_Others.compress_raw_wakan(cw_actor.fast_data.health, cw_actor.cw_status.health_level);
                }
                return true;
            }
            return false;
        }
        private static bool immortal_level_judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys)
        {
            //if(cw_actor_data.cultisys_level[cultisys.tag] < Others.CW_Constants.max_cultisys_level) WorldBoxConsole.Console.print(cw_actor_data.status.wakan + "/" + cultisys.power_level[cw_actor_data.cultisys_level[cultisys.tag]]);
            if( cw_actor.cw_data.cultisys_level[cultisys.tag]+1 < Others.CW_Constants.max_cultisys_level && cw_actor.cw_status.wakan >= cw_actor.cw_cur_stats.wakan)
            {
                if(cw_actor.cw_status.wakan_level < cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1])
                {
                    cw_actor.cw_status.wakan = (int)Utils.CW_Utils_Others.get_raw_wakan(cw_actor.cw_status.wakan, cw_actor.cw_status.wakan_level);

                    cw_actor.cw_status.wakan_level = cultisys.power_level[cw_actor.cw_data.cultisys_level[cultisys.tag] + 1];

                    cw_actor.cw_status.wakan = (int)Utils.CW_Utils_Others.compress_raw_wakan(cw_actor.cw_status.wakan, cw_actor.cw_status.wakan_level);
                }
                return true;
            }
            return false;
        }
    }
}
