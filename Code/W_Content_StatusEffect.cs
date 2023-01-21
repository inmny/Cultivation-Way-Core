using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
namespace Cultivation_Way.Content
{
    internal static class W_Content_StatusEffect
    {
        internal static void add_status_effects()
        {
            add_gold_shied_status_effect();
            add_burning_status_effect();
            add_curse_status_effect();
            add_corrode_status_effect();
            add_armor_expose_status_effect();
            add_wtiger_tooth_status_effect();
            //load_status_effects_anims();
        }
        
        private static void load_status_effects_anims()
        {
            throw new NotImplementedException();
        }
        private static void add_status_effect_for_custom()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats();
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_custom",
                anim_id: null,
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        private static void add_wtiger_tooth_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.anti_injury = 0.5f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_wtiger_tooth",
                anim_id: "wtiger_tooth_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void add_armor_expose_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor -= 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_armor_expose",
                anim_id: null,//"armor_expose_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void add_corrode_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); //base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_corrode",
                anim_id: null,//"corrode_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void add_curse_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.mod_health -= 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_curse",
                anim_id: null,//"curse_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void add_burning_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); //base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_burning",
                anim_id: null,//"burning_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void add_gold_shied_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_gold_shied",
                anim_id: "gold_shied_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f,
                action_on_get: copy_bonus_stats_on_get,
                action_on_hit: gold_shied_on_hit
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        private static void copy_bonus_stats_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            status_effect.bonus_stats = status_effect.bonus_stats.deepcopy();
        }
        private static void gold_shied_on_hit(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject attacker)
        {
            CW_EffectManager.instance.spawn_anim("gold_shied_on_hit_anim", _obejct.currentPosition, _obejct.currentPosition, _obejct, _obejct, _obejct.objectType == MapObjectType.Actor ? ((CW_Actor)_obejct).cw_cur_stats.base_stats.scale : 1f);   
        }
    }
}
