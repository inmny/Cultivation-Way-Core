using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
namespace Cultivation_Way
{
    public class CW_StatusEffectData
    {
        public string id;
        public float left_time;
        public bool finished;
        public float effect_val;
        public float next_update_action_time;
        public BaseSimObject user;
        /// <summary>
        /// 属性加成，修改此项时请保证深拷贝
        /// </summary>
        public CW_BaseStats bonus_stats;
        public CW_SpriteAnimation anim;
        public CW_Asset_StatusEffect status_asset;
        public CW_StatusEffectData(BaseSimObject _object, string status_effect_id, BaseSimObject user)
        {
            this.id = status_effect_id;
            this.status_asset = CW_Library_Manager.instance.status_effects.get(status_effect_id);
            if (status_asset == null) { finished = true; return; }
            if (!string.IsNullOrEmpty(status_asset.anim_id))
            {
                this.anim = CW_EffectManager.instance.spawn_anim(status_asset.anim_id, _object.currentPosition, _object.currentPosition, _object, _object, status_asset.anim_scale_co * (_object.objectType == MapObjectType.Actor ? ((CW_Actor)_object).cw_cur_stats.base_stats.scale : 1f));
                if (this.anim == null || !this.anim.isOn) { finished = true; return; }
            }
            this.next_update_action_time = status_asset.update_action_interval;
            this.bonus_stats = status_asset.bonus_stats;
            this.left_time = status_asset.effect_time;
            this.effect_val = status_asset.effect_val;
            this.user = user;
        }
        internal bool is_available()
        {
            return !finished;
        }
        public void update(float elapsed, BaseSimObject _object)
        {
            left_time -= elapsed;
            finished = left_time <= 0;
            next_update_action_time -= elapsed;
            if (status_asset.action_on_update != null && next_update_action_time <=0 )
            {
                next_update_action_time = status_asset.update_action_interval;
                status_asset.action_on_update(this, _object);
            }
            if (finished)
            {
                if (status_asset.action_on_end != null) status_asset.action_on_end(this, _object);
                if (anim != null) anim.force_stop(true);
            }
        }
        public void force_finish()
        {
            finished = true;
            if (anim != null) anim.force_stop(true);
        }
        public bool check_finish()
        {
            return left_time <= 0;
        }
    }
}
