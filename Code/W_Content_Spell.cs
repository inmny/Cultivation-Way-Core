using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Actions;
using Cultivation_Way.Animation;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal class W_Content_Spell
    {
        public static void add_spells()
        {
            //add_bushido_base_spell();
            //add_example_spell();

            add_fall_rock();
            add_fall_wood();

            add_gold_blade_spell();
            add_fire_blade_spell();
            add_wind_blade_spell();
            add_water_blade_spell();

            add_gold_escape_spell();
            add_fire_escape_spell();
            add_water_escape_spell();
            add_wood_escape_spell();
            add_ground_escape_spell();

            add_gold_shield_spell();
            add_water_shield_spell();

            add_single_gold_sword_spell();
            add_single_water_sword_spell();
            add_single_wood_sword_spell();

            add_fen_fire_spell();
            add_loltus_fire_spell();
            add_samadhi_fire_spell();
            add_void_fire_spell();

            add_fire_polo_spell();
            add_water_polo_spell();
            add_wind_polo_spell();
            add_lightning_polo_spell();

            add_wood_thorn_spell();
            add_ground_thorn_spell();

            add_ice_bound_spell();
            add_landificate_spell();
            add_vine_bound_spell();

            add_default_lightning_spell();
            add_positive_quintuple_lightning_spell();
            add_negative_quintuple_lightning_spell();

            add_wtiger_tooth_spell();
            add_unicorn_horn_spell();
            add_basalt_armor_spell();
            add_gdragon_scale_spell();
            add_rosefinch_feather_spell();

            add_stxh_spell();

            add_bushido_spells();
            load_other_anims();
        }

        private static CW_Asset_Spell clone(string pNew, string pFrom)
        {
            return CW_Library_Manager.instance.spells.clone(pNew, pFrom);
        }
        private static void add_bushido_spells()
        {
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "五步拳", anim_id: null,
                element: new CW_Element(new int[] { 20,20,20,20,20}),
                rarity: 1, free_val: 0.1f, cost: 0.05f, min_cost: 5,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.BUSHIDO);
            spell.add_tag(CW_Spell_Tag.ATTACK);
            CW_Library_Manager.instance.spells.add(spell);
            clone("一十八路登堂拳", "五步拳").free_val = 0.08f;
            clone("明月剑法", "五步拳").free_val = 0.18f;
            clone("五虎断门刀", "五步拳").free_val = 0.15f;
            clone("抱山枪", "五步拳").free_val = 0.17f;
            clone("破阵枪", "五步拳").free_val = 0.28f;
            clone("齐眉棍法", "五步拳").free_val = 0.19f;
            clone("太祖长棍", "五步拳").free_val = 0.28f;
            spell = clone("一十二路入室拳", "五步拳");
            spell.rarity = 4;
            spell.free_val = 0.78f;
            clone("太祖长拳", "一十二路入室拳");
            clone("弈天剑", "一十二路入室拳").free_val = 0.65f;
            clone("随波天殇剑", "一十二路入室拳").free_val = 0.68f;
            clone("神风刀法", "一十二路入室拳").free_val = 0.68f;
            clone("铁枪诀", "一十二路入室拳").free_val = 0.8f;
            clone("震山棍法", "一十二路入室拳").free_val = 0.7f;
            clone("里合腿", "一十二路入室拳").free_val = 0.68f;
            clone("飞星腿", "一十二路入室拳").free_val = 0.78f;
        }
        private static void load_other_anims()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            CW_EffectManager.instance.load_as_controller("explosion_anim", "effects/explosion/", controller_setting: anim_setting, base_scale: 1f);
        }
        // 落木
        private static void add_fall_wood()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 25f;
            anim_setting.froze_time_after_end = 0.3f;
            anim_setting.always_roll = true;
            anim_setting.always_roll_axis = new Vector3(0,0,1);
            anim_setting.roll_angle_per_frame = 1000;
            anim_setting.layer_name = "Objects";
            anim_setting.end_action = fall_rock_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("fall_wood_anim", "effects/fall_wood/", controller_setting: anim_setting, base_scale: 0.25f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fall_wood", anim_id: "fall_wood_anim",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 1, free_val: 15, cost: 0.06f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.DOWNWARD,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 落石
        private static void add_fall_rock()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 25f;
            anim_setting.froze_time_after_end = 0.3f;
            anim_setting.layer_name = "Objects";
            anim_setting.end_action = fall_rock_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("fall_rock_anim", "effects/fall_rock/", controller_setting: anim_setting, base_scale: 0.25f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fall_rock", anim_id: "fall_rock_anim",
                new CW_Element(new int[] { 0, 0, 0, 0, 100 }),
                rarity: 1, free_val: 15, cost: 0.06f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.DOWNWARD,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 地刺
        private static void add_ground_thorn_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;
            anim_setting.loop_nr_limit = 1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.layer_name = "Objects";
            anim_setting.frame_action = ground_thorn_frame_action;
            anim_setting.set_trace(AnimationTraceType.NONE);

            CW_EffectManager.instance.load_as_controller("ground_thorn_anim", "effects/ground_thorn/", controller_setting: anim_setting, base_scale: 0.3f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "ground_thorn", anim_id: "ground_thorn_anim",
                new CW_Element(new int[] { 0, 0, 0, 0, 100 }),
                rarity: 1, free_val: 1, cost: 0.06f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 木刺
        private static void add_wood_thorn_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 12;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 6;
            anim_setting.frame_interval = 0.05f;
            anim_setting.layer_name = "Objects";
            anim_setting.frame_action = wood_thorn_frame_action;
            anim_setting.set_trace(AnimationTraceType.NONE);

            CW_EffectManager.instance.load_as_controller("wood_thorn_anim", "effects/wood_thorn/", controller_setting: anim_setting, base_scale: 0.3f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wood_thorn", anim_id: "wood_thorn_anim",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.06f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 藤缚
        private static void add_vine_bound_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 12;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 4;
            anim_setting.frame_interval = 0.05f;
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("vine_bound_anim", "effects/vine_bound/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "vine_bound", anim_id: "vine_bound",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 5, free_val: 1, cost: 0.06f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.NEGATIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 石化
        private static void add_landificate_spell()
        {
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "landificate", anim_id: "landificate",
                new CW_Element(new int[] { 0, 0, 0, 0, 100 }),
                rarity: 10, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.NEGATIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 冰封
        private static void add_ice_bound_spell()
        {
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "ice_bound", anim_id: "ice_bound",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 5, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.NEGATIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 风丸
        private static void add_wind_polo_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.end_action = wind_polo_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("wind_polo_anim", "effects/wind_polo/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wind_polo", anim_id: "wind_polo_anim",
                new CW_Element(new int[] { 40, 40, 20, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.03f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 雷丸
        private static void add_lightning_polo_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.end_action = lightning_polo_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("lightning_polo_anim", "effects/lightning_polo/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "lightning_polo", anim_id: "lightning_polo_anim",
                new CW_Element(new int[] { 40, 40, 0, 20, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 水球
        private static void add_water_polo_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.end_action = water_polo_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("water_polo_anim", "effects/water_polo/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "water_polo", anim_id: "water_polo_anim",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.03f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 火球
        private static void add_fire_polo_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.end_action = fire_polo_end_action;
            anim_setting.set_trace(AnimationTraceType.LINE);

            CW_EffectManager.instance.load_as_controller("fire_polo_anim", "effects/fire_polo/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fire_polo", anim_id: "fire_polo_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.03f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 氵衮
        private static void add_bushido_base_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 0.2f;
            anim_setting.frame_action = CW_Anim_Functions.bigger_frame_action;
            CW_EffectManager.instance.load_as_controller("bushido_base_anim", "effects/bushido_base/", controller_setting: anim_setting, base_scale: 0.001f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "bushido_base", anim_id: "bushido_base_anim",
                new CW_Element(new int[5] { 20, 20, 20, 20, 20 }), element_type_limit: null,
                rarity: 1, free_val: 10, cost: 0.1f, learn_level: 1, cast_level: 1,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: bushido_base_damage_action,
                anim_action: CW_SpellAction_Anim.default_anim,
                spell_action: null,
                check_and_cost_action: CW_SpellAction_Cost.enemy_nr_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.BUSHIDO);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 朱雀之羽
        private static void add_rosefinch_feather_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("rosefinch_feather_anim", "effects/rosefinch_feather/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "rosefinch_feather", anim_id: "rosefinch_feather",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 青龙之鳞
        private static void add_gdragon_scale_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("gdragon_scale_anim", "effects/gdragon_scale/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gdragon_scale", anim_id: "gdragon_scale",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 玄武之甲
        private static void add_basalt_armor_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("basalt_armor_anim", "effects/basalt_armor/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "basalt_armor", anim_id: "basalt_armor",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 麒麟之角
        private static void add_unicorn_horn_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("unicorn_horn_anim", "effects/unicorn_horn/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "unicorn_horn", anim_id: "unicorn_horn",
                new CW_Element(new int[] { 0, 0, 0, 0, 100 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 白虎之牙
        private static void add_wtiger_tooth_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("wtiger_tooth_anim", "effects/wtiger_tooth/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wtiger_tooth", anim_id: "wtiger_tooth",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: CW_SpellAction_Spell.default_add_status,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 圣体显化
        private static void add_stxh_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.trace_type = AnimationTraceType.ATTACH;
            CW_EffectManager.instance.load_as_controller("stxh_0", "effects/stxh_HWMT", controller_setting: anim_setting, base_scale: 0.08f);
            CW_EffectManager.instance.load_as_controller("stxh_1", "effects/stxh_LXST", controller_setting: anim_setting, base_scale: 0.08f);
            CW_EffectManager.instance.load_as_controller("stxh_2", "effects/stxh_XTDT", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "stxh", anim_id: "stxh_{0}",
                new CW_Element(), element_type_limit: null,
                rarity: 1, free_val: 1, cost: 0.3f, learn_level: 1, cast_level: 1,
                can_get_by_random: false,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                spell_action: stxh_spell_action,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            spell.add_tag(CW_Spell_Tag.BUSHIDO);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 太阴五雷
        private static void add_negative_quintuple_lightning_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_nr_limit = 5;
            anim_setting.trace_type = AnimationTraceType.ATTACH;
            anim_setting.frame_action = default_lightning_frame_action;
            anim_setting.end_action = negative_lightning_end_action;
            CW_EffectManager.instance.load_as_controller("negative_quintuple_lightning_anim", "effects/default_lightning/", controller_setting: anim_setting, base_scale: 0.125f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "negative_quintuple_lightning", anim_id: "negative_quintuple_lightning_anim",
                new CW_Element(new int[5] { 40, 40, 0, 20, 0 }), element_type_limit: null,
                rarity: 10, free_val: 1, cost: 0.1f, learn_level: 1, cast_level: 1,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                spell_action: null,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.NEGATIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 太阳五雷
        private static void add_positive_quintuple_lightning_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_nr_limit = 5;
            anim_setting.trace_type = AnimationTraceType.ATTACH;
            anim_setting.frame_action = default_lightning_frame_action;
            anim_setting.end_action = positive_lightning_end_action;
            CW_EffectManager.instance.load_as_controller("positive_quintuple_lightning_anim", "effects/default_lightning/", controller_setting: anim_setting, base_scale: 0.125f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "positive_quintuple_lightning", anim_id: "positive_quintuple_lightning_anim",
                new CW_Element(new int[5] { 40, 40, 0, 20, 0 }), element_type_limit: null,
                rarity: 10, free_val: 1, cost: 0.1f, learn_level: 1, cast_level: 1,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                spell_action: null,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.NEGATIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 雷法
        private static void add_default_lightning_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_action = default_lightning_frame_action;
            CW_EffectManager.instance.load_as_controller("default_lightning_anim", "effects/default_lightning/", controller_setting: anim_setting, base_scale: 0.125f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "default_lightning", anim_id:"default_lightning_anim",
                new CW_Element(new int[5] { 40, 40, 0, 20, 0 }), element_type_limit: null,
                rarity: 4, free_val: 1, cost: 0.1f, learn_level:1, cast_level:1,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.TILE,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                spell_action: null,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金刚护体
        private static void add_gold_shield_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("gold_shield_anim", "effects/gold_shield/", controller_setting: anim_setting, base_scale: 1f);

            anim_setting = anim_setting.__deepcopy();
            anim_setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;
            anim_setting.loop_nr_limit = 1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.set_trace(AnimationTraceType.NONE);
            CW_EffectManager.instance.load_as_controller("gold_shield_on_hit_anim", "effects/gold_shield_on_hit/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_shield", anim_id: "gold_shield_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: __shield_spell_action,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 水甲
        private static void add_water_shield_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("water_shield_anim", "effects/water_shield/", controller_setting: anim_setting, base_scale: 1f);

            anim_setting = anim_setting.__deepcopy();
            anim_setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;
            anim_setting.loop_nr_limit = 1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.set_trace(AnimationTraceType.NONE);
            CW_EffectManager.instance.load_as_controller("water_shield_on_hit_anim", "effects/water_shield_on_hit/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "water_shield", anim_id: "water_shield_anim",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: __shield_spell_action,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金剑
        private static void add_single_gold_sword_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 1;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = true;
            anim_setting.always_point_to_dst = true;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 40;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("single_gold_sword_anim", "effects/single_gold_sword/", anim_limit:10000, controller_setting: anim_setting, base_scale: 0.15f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_sword", anim_id: "single_gold_sword_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 木剑, TODO: 添加对冥族、妖族的额外伤害
        private static void add_single_wood_sword_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 1;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = true;
            anim_setting.always_point_to_dst = true;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 40;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("single_wood_sword_anim", "effects/single_wood_sword/", anim_limit: 10000, controller_setting: anim_setting, base_scale: 0.15f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wood_sword", anim_id: "single_wood_sword_anim",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 水剑
        private static void add_single_water_sword_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 1;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = true;
            anim_setting.always_point_to_dst = true;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 40;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("single_water_sword_anim", "effects/single_water_sword/", anim_limit: 10000, controller_setting: anim_setting, base_scale: 0.15f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "water_sword", anim_id: "single_water_sword_anim",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.02f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 虚无之火
        private static void add_void_fire_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = false;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 20;
            anim_setting.end_action = void_fire_end_action;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("void_fire_anim", "effects/void_fire/", anim_limit: 10, controller_setting: anim_setting, base_scale: 0.12f);

            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 3f;
            anim_setting.frame_action = anti_matter_loop_frame_action;
            anim_setting.layer_name = "EffectsBack";
            anim_setting.end_action = null;
            anim_setting.set_trace(AnimationTraceType.NONE);
            CW_EffectManager.instance.load_as_controller("anti_matter_anim", "effects/anti_matter/", anim_limit: 10, controller_setting: anim_setting, base_scale: 0.12f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "void_fire", anim_id: "void_fire_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 95, free_val: 1, cost: 0.8f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 三昧真火
        private static void add_samadhi_fire_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = false;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 20;
            anim_setting.end_action = samadhi_fire_end_action;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("samadhi_fire_anim", "effects/samadhi_fire/", anim_limit: 10, controller_setting: anim_setting, base_scale: 0.12f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "samadhi_fire", anim_id: "samadhi_fire_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 95, free_val: 1, cost: 0.08f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 红莲业火
        private static void add_loltus_fire_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = false;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 20;
            anim_setting.end_action = loltus_fire_end_action;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("loltus_fire_anim", "effects/loltus_fire/", anim_limit: 10, controller_setting: anim_setting, base_scale: 0.12f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "loltus_fire", anim_id: "loltus_fire_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 20, free_val: 1, cost: 0.08f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 九幽冥焰
        private static void add_fen_fire_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.loop_limit_type = AnimationLoopLimitType.DST_LIMIT;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = false;
            anim_setting.anim_froze_frame_idx = -1;
            anim_setting.trace_grad = 20;
            anim_setting.end_action = fen_fire_end_action;
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_EffectManager.instance.load_as_controller("fen_fire_anim", "effects/fen_fire/", anim_limit: 10, controller_setting: anim_setting, base_scale: 0.12f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fen_fire", anim_id: "fen_fire_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 20, free_val: 1, cost: 0.08f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: null,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金遁
        private static void add_gold_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = __escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            anim_setting.free_val = 9;
            CW_EffectManager.instance.load_as_controller("gold_escape_anim", "effects/gold_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_escape", anim_id: "gold_escape_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 5, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: null,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 土遁
        private static void add_ground_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = __escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            anim_setting.free_val = 9;
            CW_EffectManager.instance.load_as_controller("ground_escape_anim", "effects/ground_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "ground_escape", anim_id: "ground_escape_anim",
                new CW_Element(new int[] { 0, 0, 0, 0, 100 }),
                rarity: 5, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: null,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 木遁
        private static void add_wood_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = __escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            anim_setting.free_val = 7;
            CW_EffectManager.instance.load_as_controller("wood_escape_anim", "effects/wood_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wood_escape", anim_id: "wood_escape_anim",
                new CW_Element(new int[] { 0, 0, 100, 0, 0 }),
                rarity: 5, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: null,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 水遁
        private static void add_water_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = __escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            anim_setting.free_val = 9;
            CW_EffectManager.instance.load_as_controller("water_escape_anim", "effects/water_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "water_escape", anim_id: "water_escape_anim",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 5, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: null,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 火遁
        private static void add_fire_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = __escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            anim_setting.free_val = 5;
            CW_EffectManager.instance.load_as_controller("fire_escape_anim", "effects/fire_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fire_escape", anim_id: "fire_escape_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 5, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: null,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金刃
        private static void add_gold_blade_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            anim_setting.loop_trace_limit = 33;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 3;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.set_trace(AnimationTraceType.LINE);

            anim_setting.frame_action = gold_blade_frame_action;
            CW_EffectManager.instance.load_as_controller("gold_blade_anim", "effects/gold_blade/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_blade", anim_id: "gold_blade_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 1, cost: 0.10f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 水刃
        private static void add_water_blade_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            anim_setting.loop_trace_limit = 33;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 3;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.set_trace(AnimationTraceType.LINE);

            anim_setting.frame_action = water_blade_frame_action;
            CW_EffectManager.instance.load_as_controller("water_blade_anim", "effects/water_blade/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "water_blade", anim_id: "water_blade_anim",
                new CW_Element(new int[] { 100, 0, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.10f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 火刃
        private static void add_fire_blade_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            anim_setting.loop_trace_limit = 33;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 3;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.set_trace(AnimationTraceType.LINE);

            anim_setting.frame_action = fire_blade_frame_action;
            CW_EffectManager.instance.load_as_controller("fire_blade_anim", "effects/fire_blade/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "fire_blade", anim_id: "fire_blade_anim",
                new CW_Element(new int[] { 0, 100, 0, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.10f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 风刃
        private static void add_wind_blade_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            anim_setting.loop_trace_limit = 33;
            anim_setting.loop_nr_limit = -1;
            anim_setting.anim_froze_frame_idx = 3;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 15f;
            anim_setting.point_to_dst = true;
            anim_setting.set_trace(AnimationTraceType.LINE);

            anim_setting.frame_action = wind_blade_frame_action;
            CW_EffectManager.instance.load_as_controller("wind_blade_anim", "effects/wind_blade/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "wind_blade", anim_id: "wind_blade_anim",
                new CW_Element(new int[] { 40, 40, 20, 0, 0 }),
                rarity: 1, free_val: 1, cost: 0.10f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 示例法术
        private static void add_example_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.end_action = example_spell_end_action;
            CW_EffectManager.instance.load_as_controller("example_spell_anim", "effects/example/", controller_setting: anim_setting, base_scale: 0.015f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                    id:"example", anim_id: "example_spell_anim", 
                    element:new CW_Element(), element_type_limit: null, 
                    rarity: 1, free_val: 1, 
                    cost: 0.05f, min_cost: 20,
                    learn_level: 1, cast_level: 1, can_get_by_random: true,
                    cultisys_black_or_white_list: true, 
                    cultisys_list: null, banned_races: null, 
                    target_type: CW_Spell_Target_Type.ACTOR, 
                    target_camp: CW_Spell_Target_Camp.ENEMY, 
                    triger_type: CW_Spell_Triger_Type.ATTACK, 
                    anim_type: CW_Spell_Animation_Type.ON_TARGET, 
                    damage_action: CW_SpellAction_Damage.defualt_damage, 
                    anim_action: CW_SpellAction_Anim.default_anim, 
                    spell_action: null, 
                    check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                    );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.IMMORTAL);
            CW_Library_Manager.instance.spells.add(spell);
        }
        
        private static void stxh_spell_action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if (pUser.objectType != MapObjectType.Actor) return;
            CW_Actor cw_actor = (CW_Actor)pUser;
            CW_Asset_SpecialBody body = CW_Library_Manager.instance.special_bodies.get(cw_actor.cw_data.special_body_id);

            CW_StatusEffectData status_effect = cw_actor.add_status_effect("status_custom", "stxh");
            status_effect.bonus_stats = body.stxh_bonus_stats;
            CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim("stxh_" + body.anim_id, pUser.currentPosition, pUser.currentPosition, pUser, pUser, cw_actor.cw_cur_stats.base_stats.scale);
            status_effect.anim = anim;
        }
        // TODO: 补充对建筑的效果
        private static void positive_lightning_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            float radius = 5;
            WorldTile center = MapBox.instance.GetTile((int)src_vec.x, (int)src_vec.y);
            if (center == null) return;
            List<WorldTile> tiles = Utils.CW_SpellHelper.get_circle_tiles(center, radius);
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_tiles(tiles, anim.src_object.kingdom);
            foreach (BaseSimObject enemy in enemies)
            {
                if (enemy.objectType == MapObjectType.Actor)
                {
                    ((CW_Actor)enemy).add_status_effect("status_burning");
                    ((CW_Actor)enemy).add_status_effect("status_armor_expose");
                }
                else
                {
                    continue;
                }
            }
        }
        // TODO: 补充对建筑的效果
        private static void negative_lightning_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            float radius = 5;
            WorldTile center = MapBox.instance.GetTile((int)src_vec.x, (int)src_vec.y);
            if (center == null) return;
            List<WorldTile> tiles = Utils.CW_SpellHelper.get_circle_tiles(center, radius);
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_tiles(tiles, anim.src_object.kingdom);
            foreach (BaseSimObject enemy in enemies)
            {
                if(enemy.objectType == MapObjectType.Actor)
                {
                    ((CW_Actor)enemy).add_status_effect("status_curse");
                    ((CW_Actor)enemy).add_status_effect("status_corrode");
                }
                else
                {
                    continue;
                }
            }
        }
        private static void default_lightning_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx != 5) return;
            float radius = 5;
            if (anim.src_object == null || !anim.src_object.base_data.alive) return;
            WorldTile center = anim.src_object.currentTile;
            if (center == null) return;
            List<WorldTile> tiles = Utils.CW_SpellHelper.get_circle_tiles(center, radius);
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_tiles(tiles, anim.src_object.kingdom);
            foreach(BaseSimObject enemy in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, enemy, Mathf.Sqrt(anim.cost_for_spell)*2);
            }
            foreach(WorldTile tile in tiles)
            {
                tile.setBurned();
            }
        }
        private static void __shield_spell_action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if (pUser == null || !pUser.base_data.alive) return;
            CW_StatusEffectData status = ((CW_Actor)pUser).add_status_effect("status_" + spell_asset.id);
            //CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pUser.currentPosition, pUser.currentPosition, pUser, pUser, pUser.objectType == MapObjectType.Actor ? ((CW_Actor)pUser).cw_cur_stats.base_stats.scale : 1f);
            status.bonus_stats.base_stats.armor += (int)cost;
        }
        private static void on_user_auto_scale(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            if (pUser == null) return;
            CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pUser.currentPosition, pUser.currentPosition, pUser, pUser, ((CW_Actor)pUser).cw_cur_stats.base_stats.scale);
            if (anim == null) return;
            anim.cost_for_spell = cost;
        }
        private static void __escape_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null || !anim.src_object.base_data.alive) { anim.force_stop(false); return; }
            if (cur_frame_idx == anim.setting.free_val)
            {
                WorldTile target = null;
                if (anim.src_object.city != null)
                {// 优先回城
                    target = anim.src_object.city.getTile();
                }
                else
                {
                    int time_limit = 5;
                    while (time_limit-- > 0 && target==null)
                    {
                        TileIsland ground_island = Utils.CW_SpellHelper.get_random_ground_island(true);
                        if(ground_island != null)
                        {
                            MapRegion random = ground_island.regions.GetRandom();
                            target = ((random != null) ? random.tiles.GetRandom<WorldTile>() : null);
                            if (target == null || target.Type.block || !target.Type.ground) target = null;
                        }
                    }
                }
                if (target != null) 
                { 
                    //((CW_Actor)anim.src_object).cancelAllBeh(null);
                    //CW_Actor.set_attackedBy((Actor)anim.src_object, null);
                    CW_Actor.set_attackTarget((Actor)anim.src_object, null);
                    CW_Actor.func_spawnOn((CW_Actor)anim.src_object, target, 0f);
                    ((CW_Actor)anim.src_object).updatePos();
                    anim.set_position(anim.src_object.currentPosition); 
                }
            }
        }
        private static void wood_thorn_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx > 2)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell * 0.1f);
                    }

                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void ground_thorn_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx == 3)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    float force_z = 1.0f;
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                        if (actor.objectType == MapObjectType.Actor) ((CW_Actor)actor).addForce(0, 0, force_z);
                    }

                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void gold_blade_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx > 2)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                    }
                    
                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void water_blade_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx > 2)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    float dist = Toolbox.DistVec2Float(src_vec, dst_vec);
                    float force_x = (dst_vec.x - src_vec.x) / dist * 0.4f;
                    float force_y = (dst_vec.y - src_vec.y) / dist * 0.4f;
                    float force_z = 0.1f;
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                        if (actor.objectType == MapObjectType.Actor) ((CW_Actor)actor).addForce(force_x, force_y, force_z);
                    }

                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void wind_blade_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx > 2)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    float force_z = 0.8f;
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                        if (actor.objectType == MapObjectType.Actor) ((CW_Actor)actor).addForce(0, 0, force_z);
                    }

                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void fire_blade_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx > 2)
            {
                if (anim.src_object == null || !anim.src_object.base_data.alive) return;
                int x = (int)anim.gameObject.transform.position.x;
                int y = (int)anim.gameObject.transform.position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    List<BaseSimObject> targets = Utils.CW_SpellHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    foreach (BaseSimObject actor in targets)
                    {
                        Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                        if (actor.objectType == MapObjectType.Actor) ((CW_Actor)actor).add_status_effect("status_burning");
                    }

                }
                else
                {
                    anim.force_stop(false);
                }
            }
        }
        private static void anti_matter_loop_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (cur_frame_idx == 17) anim.cur_frame_idx = 6;
            WorldTile center = MapBox.instance.GetTile((int)src_vec.x, (int)src_vec.y);
            if (center == null || anim.src_object==null || !anim.src_object.base_data.alive) { anim.force_stop(false); return; }
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 5);
            float dist;
            foreach(BaseSimObject enemy in enemies)
            {
                dist = Toolbox.DistVec2Float(dst_vec, enemy.currentPosition);
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, enemy, anim.cost_for_spell / (1+dist));
                if (enemy.objectType != MapObjectType.Actor) continue;
                if (!enemy.base_data.alive)
                {
                    // 夺取灵气
                    ((CW_Actor)anim.src_object).cw_status.wakan += (int)Utils.CW_Utils_Others.transform_wakan(((CW_Actor)enemy).cw_status.wakan, ((CW_Actor)enemy).cw_status.wakan_level, ((CW_Actor)anim.src_object).cw_status.wakan_level);
                    ((CW_Actor)enemy).cw_status.wakan = 0;
                    ((CW_Actor)anim.src_object).checkLevelUp();
                }
                else
                {
                    // 拖拽
                    ((CW_Actor)enemy).addForce((dst_vec.x-enemy.currentPosition.x)*0.1f, (dst_vec.y - enemy.currentPosition.y)*0.1f, 0.05f);
                }
            }
        }
        private static void example_spell_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            ((CW_Actor)anim.src_object).fast_data.health += 1;
        }
        private static void fall_rock_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null) return;
            WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
            if (center == null) return;
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 5);
            float force = 5f;
            //CW_EffectManager.instance.spawn_anim("bushido_base_anim", dst_vec, dst_vec, anim.src_object, null, 1f);
            foreach (BaseSimObject actor in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
                if (actor.objectType != MapObjectType.Actor) continue;
                ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force, (actor.currentPosition.y - dst_vec.y) / force, 1/force);
            }
        }
        private static void fire_polo_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null) return;
            WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
            if (center == null) return;
            CW_EffectManager.instance.spawn_anim("explosion_anim", center.posV, center.posV, null, null, 0.06f);
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
            float force = 3f;
            foreach (BaseSimObject actor in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);

                if (actor.objectType != MapObjectType.Actor) continue;
                ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force, (actor.currentPosition.y - dst_vec.y) / force, Toolbox.DistVec2Float(actor.currentPosition, dst_vec) / force);
            }
        }
        private static void water_polo_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null) return;
            WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
            if (center == null) return;
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
            float dist = Toolbox.DistVec2Float(src_vec, dst_vec);
            float force_x = (dst_vec.x - src_vec.x) / dist * 0.5f;
            float force_y = (dst_vec.y - src_vec.y) / dist * 0.5f;
            float force_z = 0.15f;
            foreach (BaseSimObject actor in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);

                if (actor.objectType != MapObjectType.Actor) continue;
                ((CW_Actor)actor).addForce(force_x, force_y, force_z);
            }
        }
        private static void wind_polo_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null) return;
            WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
            if (center == null) return;
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
            float force_z = 0.8f;
            foreach (BaseSimObject actor in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);

                if (actor.objectType != MapObjectType.Actor) continue;
                ((CW_Actor)actor).addForce(0, 0, force_z);
            }
        }
        private static void lightning_polo_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null) return;
            WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
            if (center == null) return;
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
            foreach (BaseSimObject actor in enemies)
            {
                Utils.CW_SpellHelper.cause_damage_to_target(anim.src_object, actor, anim.cost_for_spell);
            }
        }
        
        private static void void_fire_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.dst_object == null || !anim.dst_object.base_data.alive || anim.src_object == null || !anim.src_object.base_data.alive) return;
            CW_SpriteAnimation anti_matter = CW_EffectManager.instance.spawn_anim("anti_matter_anim", anim.dst_object.currentPosition, anim.dst_object.currentPosition, anim.src_object, anim.dst_object, 1f);
            if (anti_matter == null) return;
            anti_matter.cost_for_spell = anim.cost_for_spell / 3;
        }
        private static void samadhi_fire_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.dst_object == null || !anim.dst_object.base_data.alive) return;
            CW_StatusEffectData status_effect = Utils.CW_SpellHelper.add_status_to_target(anim.src_object, anim.dst_object, "status_samadhi_fire");
        }
        private static void loltus_fire_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.dst_object == null || !anim.dst_object.base_data.alive || anim.src_object == null || !anim.src_object.base_data.alive) return;
            CW_StatusEffectData status_effect = Utils.CW_SpellHelper.add_status_to_target(anim.src_object, anim.dst_object, "status_loltus_fire");
            status_effect.left_time = (1 + ((CW_Actor)anim.dst_object).fast_data.kills / 10f) * status_effect.status_asset.effect_time;
            status_effect.effect_val = Utils.CW_Utils_Others.get_raw_wakan(status_effect.status_asset.effect_val, ((CW_Actor)anim.src_object).cw_status.wakan_level);
        }
        private static void fen_fire_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.dst_object == null || !anim.dst_object.base_data.alive) return;
            CW_StatusEffectData status_effect = Utils.CW_SpellHelper.add_status_to_target(anim.src_object, anim.dst_object, "status_fen_fire");
        }
        private static void bushido_base_damage_action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            List<BaseSimObject> enemies = Utils.CW_SpellHelper.find_enemies_in_circle(pUser.currentTile, pUser.kingdom, 5);
            float force = spell_asset.free_val / enemies.Count;
            foreach(BaseSimObject actor in enemies)
            {
                if (actor.objectType != MapObjectType.Actor) continue;
                ((CW_Actor)actor).addForce((actor.currentPosition.x - pUser.currentPosition.x) * force, (actor.currentPosition.y - pUser.currentPosition.y) * force, force);
            }
        }
    }
}
