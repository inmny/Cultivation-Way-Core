using Cultivation_Way.Animation;
using Cultivation_Way.Library;
using UnityEngine;
namespace Cultivation_Way.Others
{
    public class CW_Constants
    {
        public const string mod_id = "inmny.cw_core";
        public const int save_version = 1;
        public const int base_element_types = 5;
        public const float default_anim_trace_grad = 0.1f;
        public const string default_anim_layer_name = "EffectsTop";
        public const bool is_debugging = true;
        public const int wakan_regen_valid_percent = 40;
        public const int fix_cultibook_line = 1000;
        public const int cultibook_spell_limit = 4;
        public const float exceed_max_age_chance = 0.85f;
        public const int max_unique_legendary_names_count = 128;
        public const int max_cultisys_level = 20;
    }
    public class CW_Delegates
    {
        // TODO: reduce the parameters cost
        public delegate void CW_Animation_Trace_Update(float src_x, float src_y, float dst_x, float dst_y, float play_time, float total_time, int loop_nr, int loop_nr_limit, ref float anim_x, ref float anim_y, float grad);
        public delegate void CW_Animation_Frame_Action(int cur_frame_idx, float src_x, float src_y, float dst_x, float dst_y, float play_time, float anim_x, float anim_y, BaseSimObject pUser);
        public delegate void CW_Animation_End_Action(int cur_frame_idx, float src_x, float src_y, float dst_x, float dst_y, float play_time, float anim_x, float anim_y, BaseSimObject pUser, BaseSimObject pTarget);
        public delegate void CW_Spell_Action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile);
        public delegate void CW_WorldEvent_Action();
        public delegate bool CW_Cultisys_Judge(CW_ActorData cw_actor_data, CW_Asset_CultiSys cultisys);
    }
}