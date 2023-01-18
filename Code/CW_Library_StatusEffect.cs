using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_StatusEffect : Asset
    {
        public CW_BaseStats bonus_stats;
        public string anim_id;
        internal float anim_time;
        public Others.CW_Delegates.CW_Status_Action action_on_get;
        public Others.CW_Delegates.CW_Status_GetHit_Action get_hit_action;
        public void set_time_by_month(int month)
        {
            anim_time = month * Others.CW_Constants.seconds_per_month;
        }
        public void set_time_by_second(float second)
        {
            anim_time = second;
        }
        internal void register()
        {
            throw new NotImplementedException();
        }
    }
    public class CW_Library_StatusEffect : CW_Asset_Library<CW_Asset_StatusEffect>
    {
        internal override void register()
        {
            throw new NotImplementedException();
        }
    }
}
