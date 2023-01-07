using Cultivation_Way.Animation;
using UnityEngine;
namespace Cultivation_Way.Others
{
    public class CW_Constants
    {
        public const int save_version = 1;
        public const int base_element_types = 5;
        public const float default_anim_trace_grad = 0.1f;
        public const string default_anim_layer_name = "EffectsTop";
        public const bool is_debugging = true;
    }
    public class CW_Delegates
    {
        public delegate void CW_Animation_Trace_Update(float src_x, float src_y, float dst_x, float dst_y, float play_time, ref float anim_x, ref float anim_y, float grad);
        public delegate void CW_Animation_Frame_Action(int cur_frame_idx, float src_x, float src_y, float dst_x, float dst_y, float play_time, float anim_x, float anim_y, BaseSimObject pUser);
        public delegate void CW_Animation_End_Action(int cur_frame_idx, float src_x, float src_y, float dst_x, float dst_y, float play_time, float anim_x, float anim_y, BaseSimObject pUser, BaseSimObject pTarget);
    }
}