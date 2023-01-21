﻿using System;
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
            add_example_spell();
            add_gold_blade_spell();
            add_gold_escape_spell();
            add_gold_shied_spell();
            add_single_gold_sword_spell();
            add_default_lightning_spell();
            add_positive_quintuple_lightning_spell();
            add_negative_quintuple_lightning_spell();
            add_wtiger_tooth_spell();
            add_stxh_spell();
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
            CW_Library_Manager.instance.spells.add(spell);

        }
        // TODO: 添加效果并加入法术库，图像需要上移
        // 圣体显化
        private static void add_stxh_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.trace_type = AnimationTraceType.ATTACH;
            CW_EffectManager.instance.load_as_controller("stxh_0", "effects/stxh_HWMT", controller_setting: anim_setting, base_scale: 0.10f);
            CW_EffectManager.instance.load_as_controller("stxh_1", "effects/stxh_LXST", controller_setting: anim_setting, base_scale: 0.10f);
            CW_EffectManager.instance.load_as_controller("stxh_2", "effects/stxh_XTDT", controller_setting: anim_setting, base_scale: 0.10f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "stxh", anim_id: "stxh_{0}",
                new CW_Element(), element_type_limit: null,
                rarity: 1, free_val: 1, cost: 0.3f, learn_level: 1, cast_level: 1,
                cultisys_black_or_white_list: true, cultisys_list: null,
                banned_races: null,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                spell_action: stxh_spell_action
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
            //CW_Library_Manager.instance.spells.add(spell);
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
                rarity: 1, free_val: 1, cost: 0.1f, learn_level: 1, cast_level: 1,
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
                rarity: 1, free_val: 1, cost: 0.1f, learn_level: 1, cast_level: 1,
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
                rarity: 1, free_val: 1, cost: 0.1f, learn_level:1, cast_level:1,
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
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金刚护体
        private static void add_gold_shied_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 120f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.set_trace(AnimationTraceType.ATTACH);

            CW_EffectManager.instance.load_as_controller("gold_shied_anim", "effects/gold_shied/", controller_setting: anim_setting, base_scale: 1f);

            anim_setting = anim_setting.__deepcopy();
            anim_setting.loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT;
            anim_setting.loop_nr_limit = 1;
            anim_setting.frame_interval = 0.05f;
            anim_setting.set_trace(AnimationTraceType.NONE);
            CW_EffectManager.instance.load_as_controller("gold_shied_on_hit_anim", "effects/gold_shied_on_hit/", controller_setting: anim_setting, base_scale: 1f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_shied", anim_id: "gold_shied_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.CUSTOM,
                damage_action: null,
                anim_action: null,
                spell_action: gold_shied_spell_action,
                check_and_cost_action: CW_SpellAction_Cost.default_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            spell.add_tag(CW_Spell_Tag.POSITIVE_STATUS);
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
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 金遁
        private static void add_gold_escape_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.frame_interval = 0.05f;
            anim_setting.frame_action = gold_escape_frame_action;
            anim_setting.trace_type = AnimationTraceType.NONE;
            CW_EffectManager.instance.load_as_controller("gold_escape_anim", "effects/gold_escape/", controller_setting: anim_setting, base_scale: 1f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_escape", anim_id: "gold_escape_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, free_val: 0.15f, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: CW_SpellAction_Damage.no_damage,
                anim_action: on_user_auto_scale,
                check_and_cost_action: CW_SpellAction_Cost.low_health_check_and_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
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
            CW_Library_Manager.instance.spells.add(spell);
        }
        // 示例法术
        private static void add_example_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.end_action = example_spell_end_action;
            CW_EffectManager.instance.load_as_controller("example_spell_anim", "effects/example/", controller_setting: anim_setting, base_scale: 0.015f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                    "example", "example_spell_anim", 
                    new CW_Element(), null, 
                    1, 1, 0.05f, 1, 1, 
                    true, null, null, 
                    CW_Spell_Target_Type.ACTOR, 
                    CW_Spell_Target_Camp.ENEMY, 
                    CW_Spell_Triger_Type.ATTACK, 
                    CW_Spell_Animation_Type.ON_TARGET, 
                    CW_SpellAction_Damage.defualt_damage, 
                    CW_SpellAction_Anim.default_anim, 
                    null, 
                    CW_SpellAction_Cost.default_check_and_cost
                    );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            CW_Library_Manager.instance.spells.add(spell);
        }
        private static void stxh_spell_action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            throw new NotImplementedException();
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
            WorldTile center = MapBox.instance.GetTile((int)src_vec.x, (int)src_vec.y);
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
        private static void gold_shied_spell_action(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
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
        private static void gold_escape_frame_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            if (anim.src_object == null || !anim.src_object.base_data.alive) { anim.force_stop(false); return; }
            if (cur_frame_idx == 9)
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
                            if (target == null || target.Type.block || target.Type.ground) target = null;
                        }
                    }
                }
                if (target != null) 
                { 
                    ((CW_Actor)anim.src_object).cancelAllBeh(null);
                    CW_Actor.set_attackedBy((Actor)anim.src_object, null);
                    CW_Actor.set_attackTarget((Actor)anim.src_object, null);
                    CW_Actor.func_spawnOn((CW_Actor)anim.src_object, target, 0f);
                    ((CW_Actor)anim.src_object).updatePos();
                    anim.set_position(anim.src_object.currentPosition); 
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
        private static void example_spell_end_action(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, CW_SpriteAnimation anim)
        {
            ((CW_Actor)anim.src_object).fast_data.health += 1;
        }
    }
}