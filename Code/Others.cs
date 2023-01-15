using Cultivation_Way.Animation;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Others
{
    public class CW_Constants
    {
        public const string mod_name = "修真之路核心";
        public const string mod_id = "inmny.cw_core";
        public const float anim_dst_error = 0.2f;
        public const int save_version = 1;
        public const int base_element_types = 5;
        public const float default_anim_trace_grad = 2f;
        public const string default_anim_layer_name = "EffectsTop";
        public const bool is_debugging = true;
        public const int wakan_regen_valid_percent = 40;
        public const int fix_cultibook_line = 1000;
        public const int cultibook_spell_limit = 4;
        public const int cultibook_levelup_require = 5;
        public const int cultibook_max_level = 9;
        public const int cultibook_max_order = 4;
        public const bool cultibook_force_learn = false;
        public const float exceed_max_age_chance = 0.85f;
        public const int max_unique_legendary_names_count = 128;
        public const int max_cultisys_level = 20;
        public const float default_spell_damage_co = 1;
    }
    public class CW_Delegates
    {
        // TODO: reduce the parameters cost
        public delegate void CW_Animation_Trace_Update(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y);
        public delegate void CW_Animation_Frame_Action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim);
        public delegate void CW_Animation_End_Action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim);

        public delegate void CW_Spell_Action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost);
        public delegate float CW_Spell_Cost_Action(CW_Asset_Spell spell_asset, BaseSimObject pUser);
        public delegate void CW_WorldEvent_Action();
        public delegate bool CW_Cultisys_Judge(CW_ActorData cw_actor_data, CW_Asset_CultiSys cultisys);
        public delegate bool CW_Cultisys_Level_Judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys);
    }
    public class CW_Enums
    {
        public enum CW_AttackType
        {
            Acid,
            Fire,
            Plague,
            Tumor,
            Other,
            Hunger,
            Eaten,
            Age,
            None,
            GrowUp,
            Poison,
            Block,
            Spell,
            God
        }
        
    }
}