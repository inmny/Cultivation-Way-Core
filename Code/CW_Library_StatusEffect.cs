using Cultivation_Way.Others;
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
        internal float anim_time; // No used now
        internal float effect_time;
        public float effect_val;
        public Others.CW_Delegates.CW_Status_Action action_on_get;
        public Others.CW_Delegates.CW_Status_Action action_on_update;
        public Others.CW_Delegates.CW_Status_GetHit_Action action_on_hit;

        public CW_Asset_StatusEffect(string id, CW_BaseStats bonus_stats, string anim_id = null, float anim_time = 60f, float effect_time = 60f, float effect_val = 1f, CW_Delegates.CW_Status_Action action_on_get = null, CW_Delegates.CW_Status_GetHit_Action action_on_hit = null, CW_Delegates.CW_Status_Action action_on_update = null)
        {
            this.id = id;
            this.bonus_stats = bonus_stats;
            this.anim_id = anim_id;
            this.anim_time = anim_time;
            this.effect_time = effect_time;
            this.effect_val = effect_val;
            this.action_on_get = action_on_get;
            this.action_on_hit = action_on_hit;
            this.action_on_update = action_on_update;
        }

        public void set_anim_time(float second)
        {
            anim_time = second;
        }
        public void set_effect_time(float second)
        {
            this.effect_time = second;
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
