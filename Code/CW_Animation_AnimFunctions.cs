using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Animation
{
    public static class CW_Anim_Functions
    {
        public static void trace_line(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            float dist = Toolbox.DistVec2Float(src_vec, dst_vec);

            delta_x = (dst_vec.x - src_vec.x) * anim.setting.trace_grad / dist;
            delta_y = (dst_vec.y - src_vec.y) * anim.setting.trace_grad / dist;
        }
        public static void trace_track(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            float dist = Toolbox.DistVec2Float(src_vec, dst_vec);

            if (dist < Others.CW_Constants.anim_dst_error)
            {
                delta_x = 0; delta_y = 0;
                return;
            }

            delta_x = (dst_vec.x - src_vec.x) * anim.setting.trace_grad / dist;
            delta_y = (dst_vec.y - src_vec.y) * anim.setting.trace_grad / dist;
        }
        public static void trace_parabolic(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            throw new NotImplementedException();
        }
        public static void bigger_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            anim.gameObject.transform.localScale = anim.gameObject.transform.localScale * 1.5f;
        }
    }
}
