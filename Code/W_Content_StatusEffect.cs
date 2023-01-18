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
        }

        private static void add_gold_shied_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_gold_shied",
                anim_id: "gold_shied_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f,
                action_on_get: gold_shied_on_get,
                action_on_hit: gold_shied_on_hit
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        private static void gold_shied_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            status_effect.bonus_stats = status_effect.bonus_stats.deepcopy();
        }
        private static void gold_shied_on_hit(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject attacker)
        {
            CW_EffectManager.instance.spawn_anim("gold_shied_on_hit_anim", _obejct.currentPosition, _obejct.currentPosition, _obejct, _obejct, _obejct.objectType == MapObjectType.Actor ? ((CW_Actor)_obejct).cw_cur_stats.base_stats.scale : 1f);   
        }
    }
}
