using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content
{
    internal static class W_Content_WindowTop_Helper
    {
        private static List<CW_Actor> _actors = new List<CW_Actor>();
        internal static void clear_tmp_lists()
        {
            _actors.Clear();
        }
        private static void collect_actors()
        {
            if (_actors.Count == 0)
            {
                List<Actor> units = MapBox.instance.units.getSimpleList();
                foreach(Actor actor in units)
                {
                    if (actor.object_destroyed || !actor.base_data.alive) continue;
                    _actors.Add((CW_Actor)actor);
                }
            }
        }
        internal static List<CW_Actor> sort_creatures_by_level(int amount)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = Utils.CW_Utils_Others.max_of(left.cw_data.cultisys_level) * left.cw_status.health_level * left.cw_status.wakan_level;
                float l_2 = Utils.CW_Utils_Others.max_of(right.cw_data.cultisys_level) * left.cw_status.health_level * right.cw_status.wakan_level;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for(int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }
    }
}
