using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content
{
    internal static class W_Content_ActorJob
    {
        internal static void add_actor_jobs()
        {

        }
        internal static void modify_actor_jobs()
        {
            ActorJob settler = AssetManager.job_actor.get("settler");
            settler.tasks = settler.tasks.Prepend(new TaskContainer<BehaviourActorCondition>
            {
                id = "check_settler_appropriate"
            }).ToList();

            AssetManager.job_actor.get("animal").addTask("attack_back");
            AssetManager.job_actor.get("animal_herd").addTask("attack_back");
            AssetManager.job_actor.get("animal_water_eater").addTask("attack_back");
            AssetManager.job_actor.get("crab").addTask("attack_back");
            AssetManager.job_actor.get("unit_on_fire").addTask("attack_back");
        }
    }
}
