using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Animation
{
    internal static class CW_TraceFunctions
    {
        public static void trace_line(float src_x, float src_y, float dst_x, float dst_y, float play_time, float total_time, int loop_nr, int loop_nr_limit, ref float anim_x, ref float anim_y, float grad)
        {
            anim_x += (dst_x - src_x) * grad;
            anim_y += (dst_y - src_y) * grad;
        }
        public static void trace_track(float src_x, float src_y, float dst_x, float dst_y, float play_time, float total_time, int loop_nr, int loop_nr_limit, ref float anim_x, ref float anim_y, float grad)
        {
            anim_x += (dst_x - src_x) * grad;
            anim_y += (dst_y - src_y) * grad;
        }
        public static void trace_parabolic(float src_x, float src_y, float dst_x, float dst_y, float play_time, float total_time, int loop_nr, int loop_nr_limit, ref float anim_x, ref float anim_y, float grad)
        {
            throw new NotImplementedException();
        }
    }
}
