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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "tryToAttack")]
        public static bool actor_tryToAttack(Actor __instance, BaseSimObject pTarget, ref bool __result)
        {
            if (__instance == pTarget) { __result = false; CW_Actor.set_attackTarget(__instance, null); return true; }
            CW_Actor cw_actor = (CW_Actor)__instance;
            if (!cw_actor.can_act) return false;
            if (cw_actor.cur_spells.Count == 0) return true;
            if (CW_Actor.get_attackTimer(cw_actor) > 0) return true;
            
            CW_Asset_Spell spell = CW_Library_Manager.instance.spells.get(cw_actor.cur_spells.GetRandom());
            

            if (!__can_cast(spell, cw_actor, spell.select_target(cw_actor, pTarget))) return true;

            bool ret = CW_Spell.cast(spell, cw_actor, pTarget, pTarget.currentTile);
            __result = ret;
            if (ret)
            {
                CW_Actor.func_punchTargetAnimation(cw_actor, pTarget.currentPosition, pTarget.currentTile, true, (spell.tags & (1ul << (int)CW_Spell_Tag.IMMORTAL)) > 0, 40f);
                //CW_Actor.set_attackTimer(cw_actor, cw_actor.m_attackSpeed_seconds);
            }
            return true;
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
            if(pAttacker!=null && pAttacker.objectType== MapObjectType.Actor && ((CW_Actor)pAttacker).status_effects!=null && ((CW_Actor)pAttacker).status_effects.Count > 0)
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
