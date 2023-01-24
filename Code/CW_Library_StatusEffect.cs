using Cultivation_Way.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public enum CW_StatusEffect_Tag
    {
        POSTIVE,
        NEGATIVE,
        BOUND,
        DAMAGE,
        REINFORECE,
        WEAKEN
    }
    public class CW_Asset_StatusEffect : Asset
    {
        public CW_BaseStats bonus_stats;
        public string anim_id;
        internal float anim_scale_co; // No used now
        internal float effect_time;
        public float effect_val;
        public float update_action_interval;
        public List<string> opposite_status;
        public List<CW_StatusEffect_Tag> tags;
        public Others.CW_Delegates.CW_Status_Action action_on_get;
        public Others.CW_Delegates.CW_Status_Action action_on_update;
        public Others.CW_Delegates.CW_Status_Attack_Action action_on_attack;
        public Others.CW_Delegates.CW_Status_GetHit_Action action_on_hit;
        public Others.CW_Delegates.CW_Status_Action action_on_end;

        public CW_Asset_StatusEffect(string id, CW_BaseStats bonus_stats, string anim_id = null, float anim_scale_co = 1f, float effect_time = 60f, float effect_val = 1f, CW_Delegates.CW_Status_Action action_on_get = null, CW_Delegates.CW_Status_GetHit_Action action_on_hit = null,  CW_Delegates.CW_Status_Action action_on_update = null, CW_Delegates.CW_Status_Attack_Action action_on_attack = null, List<string> opposite_status = null, List<CW_StatusEffect_Tag> tags = null, CW_Delegates.CW_Status_Action action_on_end = null, float update_action_interval = 1f)
        {
            this.id = id;
            this.bonus_stats = bonus_stats;
            this.anim_id = anim_id;
            this.anim_scale_co = anim_scale_co;
            this.effect_time = effect_time;
            this.effect_val = effect_val;
            this.action_on_get = action_on_get;
            this.action_on_hit = action_on_hit;
            this.action_on_update = action_on_update;
            this.action_on_attack = action_on_attack;
            this.opposite_status = opposite_status;
            this.tags = tags;
            this.action_on_end = action_on_end;
            this.update_action_interval = update_action_interval;
        }
        public bool has_tag(CW_StatusEffect_Tag tag)
        {
            return tags!=null && tags.Contains(tag);
        }
        public void set_anim_time(float second)
        {
            anim_scale_co = second;
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
