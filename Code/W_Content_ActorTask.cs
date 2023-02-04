using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai;
using ai.behaviours;

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
        }
    }
    public class BehCheckSettler : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            if (pObject.stats.race != "yao") return BehResult.Continue;
            if (pObject.city == null) return BehResult.Continue;
            CW_CityData cw_data = (CW_CityData)CW_City.get_data(pObject.city);
            if (pObject.stats.id == cw_data.least_unit_id) return BehResult.Continue;
            return BehResult.Stop;
        }
    }
}
