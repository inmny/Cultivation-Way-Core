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
        public float left_time;
        public bool finished;
        public CW_SpriteAnimation anim;
        public CW_Asset_StatusEffect status_asset;
        public CW_StatusEffectData(BaseSimObject _object, string status_effect_id)
        {
            throw new NotImplementedException();
        }
        internal bool is_available()
        {
            return this.anim != null;
        }
        public void update(float elapsed)
        {
            left_time -= elapsed;
            finished = left_time <= 0;
        }
        public bool check_finish()
        {
            return left_time <= 0;
        }
    }
}
