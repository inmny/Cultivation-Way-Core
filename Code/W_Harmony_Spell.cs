using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Spell
    {
        // 攻击类
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "tryToAttack")]
        public static void actor_tryToAttack(Actor __instance, BaseSimObject pTarget, ref bool __result)
        {
            CW_Actor cw_actor = (CW_Actor)__instance;
            cw_actor.__battle_timer = Others.CW_Constants.battle_timer;
            if (pTarget == null || pTarget == __instance ) return;
            
            if (!cw_actor.can_act|| cw_actor.cur_spells.Count == 0) return;

            if (cw_actor.default_spell_timer > 0) return;

            CW_Asset_Spell spell = CW_Library_Manager.instance.spells.get(cw_actor.cur_spells.GetRandom());
            

            if (!__can_cast(spell, cw_actor, spell.select_target(cw_actor, pTarget))) return;

            bool ret = CW_Spell.cast(spell, cw_actor, pTarget, pTarget.currentTile);
            
            if (ret)
            {
                cw_actor.default_spell_timer = cw_actor.s_spell_seconds;
                CW_Actor.func_punchTargetAnimation(cw_actor, pTarget.currentPosition, pTarget.currentTile, true, (spell.tags & (1ul << (int)CW_Spell_Tag.IMMORTAL)) > 0, 40f);
                //CW_Actor.set_attackTimer(cw_actor, cw_actor.m_attackSpeed_seconds);
            }
            __result |= ret;
            return;
        }
        private static bool __can_cast(CW_Asset_Spell spell, CW_Actor cw_actor, BaseSimObject target)
        {
            if((int)spell.triger_type > (int)CW_Spell_Triger_Type.ALL) return false;
            if (spell.target_camp == CW_Spell_Target_Camp.ALIAS && cw_actor!=target) return false;
            switch (spell.target_type)
            {
                case CW_Spell_Target_Type.ACTOR: 
                    if(target.objectType != MapObjectType.Actor) 
                        return false; 
                    break;
                case CW_Spell_Target_Type.BUILDING:
                    if (target.objectType != MapObjectType.Building)
                        return false;
                    break;
                default: break;
            }
            if (spell.anim_type == CW_Spell_Animation_Type.ON_USER || spell.anim_type == CW_Spell_Animation_Type.UPWARD || spell.anim_type == CW_Spell_Animation_Type.DOWNWARD) return true;
            // 攻击距离判断
            return cw_actor.cw_cur_stats.spell_range + cw_actor.cw_cur_stats.base_stats.range >= Toolbox.DistTile(cw_actor.currentTile, target.currentTile);
        }
        // 防御类见CW_Actor.__get_hit


        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "applyAttack")]
        public static void status_effect_on_attack(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if(pAttacker!=null && pAttacker.objectType== MapObjectType.Actor &&pAttacker!=pTarget&& ((CW_Actor)pAttacker).status_effects!=null && ((CW_Actor)pAttacker).status_effects.Count > 0)
            {
                foreach (CW_StatusEffectData status_effect in ((CW_Actor)pAttacker).status_effects.Values)
                {   
                    if (!status_effect.finished && status_effect.status_asset.action_on_attack != null) status_effect.status_asset.action_on_attack(status_effect, pAttacker, pTarget);
                }
            }
            return;
        }
    }
}
