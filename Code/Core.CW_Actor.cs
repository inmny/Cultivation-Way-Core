using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Core
{
    public class CW_Actor : Actor
    {
        public ActorData fast_data;
        public BaseStats fast_stats;
        internal Dictionary<string, CW_StatusEffectData> statuses;

        private static List<string> __status_effects_to_remove = new List<string>();
        private static List<CW_StatusEffectData> __status_effects_to_update = new List<CW_StatusEffectData>();

        internal void cw_newCreature()
        {

        }
    }
}
