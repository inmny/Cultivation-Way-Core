using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai;
using ai.behaviours;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal static class W_Content_ActorTask
    {
        internal static void add_actor_tasks()
        {
            BehaviourTaskActor check_settler_appropriate = new BehaviourTaskActor()
            {
                id = "check_settler_appropriate"
            };
            AssetManager.tasks_actor.add(check_settler_appropriate);
            check_settler_appropriate.addBeh(new BehCheckSettler());
            check_settler_appropriate.addBeh(new BehEndJob());

            BehaviourTaskActor attack_back = new BehaviourTaskActor()
            {
                id = "attack_back"
            };
            AssetManager.tasks_actor.add(attack_back);
            attack_back.addBeh(new BehCheckAttackedBy());
            attack_back.addBeh(new BehGoToActorTarget());
            attack_back.addBeh(new BehAttackActorTarget());
        }
    }
    
    public class BehCheckAttackedBy : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            BaseSimObject attacked_by = CW_Actor.get_attackedBy(pObject);
            BaseSimObject attack_target = CW_Actor.get_attackTarget(pObject);
            if ((attacked_by == null || !attacked_by.base_data.alive) && (attack_target == null || !attack_target.base_data.alive)) return BehResult.Stop;

            if (attack_target == null || !attack_target.base_data.alive)
            {
                CW_Actor.set_attackTarget(pObject, attacked_by);
                CW_Actor.set_beh_actor_target(pObject, attacked_by);
            }
            else
            {
                CW_Actor.set_beh_actor_target(pObject, attack_target);
            }
            return BehResult.Continue;
        }
    }
    public class BehCheckSettler : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            if (pObject.stats.race != "yao") return BehResult.Stop;
            if (pObject.city == null) return BehResult.Stop;
            CW_CityData cw_data = (CW_CityData)CW_City.get_data(pObject.city);
            if (pObject.stats.id != cw_data.most_unit_id || cw_data.most_unit_id==cw_data.least_unit_id) return BehResult.Stop;

            //Debug.LogFormat("{0} in {1}, least {2}, most {3}.", pObject.stats.id, cw_data.cityName, cw_data.least_unit_id, cw_data.most_unit_id);
            return BehResult.Continue;
        }
    }
}
