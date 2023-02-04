﻿using System;
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
            settler.tasks.Prepend(new TaskContainer<BehaviourActorCondition>
            {
                id = "check_settler_appropriate"
            });
        }
    }
}