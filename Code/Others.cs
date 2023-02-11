using Cultivation_Way.Animation;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Others
{
    public class CW_Constants
    {
        public const string mod_name = "启源核心";
        public const string mod_id = "inmny.cw_core";
        public const float anim_dst_error = 0.5f;
        public const float global_immortal_culti_velo = 0.2f;
        public const float battle_timer = 3f;
        public const int save_version = 1;
        public const bool force_load_units = true;
        public const bool force_load_cities = true;
        public const int base_element_types = 5;
        public const float max_anim_time = 600f;
        public const float max_anim_trace_length = 1000f;
        public const float default_anim_trace_grad = 2f;
        public const string default_anim_layer_name = "EffectsTop";
        public const bool is_debugging = true;
        public const int wakan_level_co = 1000;
        public const int wakan_regen_valid_percent = 60;
        public const int health_regen_valid_percent = 40;
        public const int fix_cultibook_line = 10000;
        public const int cultibook_spell_limit = 4;
        public const int cultibook_levelup_require = 5;
        public const int cultibook_max_level = 9;
        public const int cultibook_max_order = 4;
        public const bool cultibook_force_learn = false;
        public const float exceed_max_age_chance = 0.85f;
        public const int max_unique_legendary_names_count = 128;
        public const int max_cultisys_level = 20;
        public const float default_spell_damage_co = 32;
        public const float chunk_wakan_compress_co = 0.9f;
        public const float chunk_wakan_spread_grad = 0.3f;
        public const float chunk_wakan_level_spread_grad = 0.1f;
        public const uint cultisys_immortol_tag = 0x1;
        public const uint cultisys_bushido_tag = 0x2;
        public const float bushido_force_culti_chance = 3f;
        public const float bushido_force_culti_co = 0.01f;
        public const float seconds_per_month = 3f;
        public const int yao_transform_level = 7;
        public const int special_body_create_level = 19;
        public const float new_family_name_chance = 0.1f;
        public const float max_knockback_reduction = 93f;

        internal static string[] num_to_cz = new string[10] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
    }
    public class CW_Delegates
    {
        // TODO: reduce the parameters cost
        public delegate void CW_Animation_Trace_Update(ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim, ref float delta_x, ref float delta_y);
        public delegate void CW_Animation_Frame_Action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim);
        public delegate void CW_Animation_End_Action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim);
        public delegate void CW_Status_Action(CW_StatusEffectData status_effect, BaseSimObject _object);
        public delegate void CW_Status_Attack_Action(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject target);
        public delegate void CW_Status_GetHit_Action(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject attacker);
        public delegate void CW_Spell_Action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost);
        public delegate float CW_Spell_Cost_Action(CW_Asset_Spell spell_asset, BaseSimObject pUser);
        public delegate void CW_WorldEvent_Action(CW_Asset_WorldEvent event_asset);
        public delegate CW_BaseStats CW_Cultisys_Stats(CW_Actor cw_actor, CW_Asset_CultiSys cultisys, int level);
        public delegate bool CW_Cultisys_Judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys);
        public delegate bool CW_Cultisys_Level_Judge(CW_Actor cw_actor, CW_Asset_CultiSys cultisys);
        public delegate string CW_Name_Template_Decode(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null);
        public delegate bool CW_Name_Template_Select(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null);
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
            Status_Spell,
            Status_God,
            God
        }
        
    }
}
