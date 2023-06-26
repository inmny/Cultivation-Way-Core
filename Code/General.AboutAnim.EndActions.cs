using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.General.AboutAnim
{
    /// <summary>
    /// 一般动画的setting.end_action
    /// </summary>
    public static class EndActions
    {
        public static void src_obj_damage_to_dst_obj(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim)
        {
            a_damage_to_b(cur_frame_idx, anim.src_object, anim.dst_object, anim);
        }
        public static void src_obj_damage_to_dst_tile(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim)
        {
            a_damage_to_b_tile(cur_frame_idx, ref src_vec, ref dst_vec, anim);
        }
        public static void dst_obj_damage_to_src_obj(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim)
        {
            a_damage_to_b(cur_frame_idx, anim.dst_object, anim.src_object, anim);
        }
        public static void dst_obj_damage_to_src_tile(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim)
        {
            a_damage_to_b_tile(cur_frame_idx, ref dst_vec, ref src_vec, anim);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void a_damage_to_b(int cur_frame_idx, BaseSimObject a_obj, BaseSimObject b_obj, Animation.SpriteAnimation anim){

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void a_damage_to_b_tile(int cur_frame_idx, ref Vector2 a_vec, ref Vector2 b_vec, Animation.SpriteAnimation anim)
        {

        }
    }
}
