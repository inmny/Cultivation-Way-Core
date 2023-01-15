using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Animation
{
    internal static class CW_TraceFunctions
    {
        public static void trace_line(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            delta_x = anim.setting.trace_grad;
            delta_y = anim.setting.trace_grad;
        }
        public static void trace_track(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            float dist = Toolbox.DistVec2Float(src_vec, dst_vec);

            delta_x = (dst_vec.x - src_vec.x) * anim.setting.trace_grad / dist;
            delta_y = (dst_vec.y - src_vec.y) * anim.setting.trace_grad / dist;
        }
        public static void trace_parabolic(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y)
        {
            throw new NotImplementedException();
        }
    }
}
