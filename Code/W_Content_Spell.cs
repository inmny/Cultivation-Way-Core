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
        }
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
                rarity: 1, might: 1, cost: 0.1f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ALIAS,
                triger_type: CW_Spell_Triger_Type.DEFEND,
                anim_type: CW_Spell_Animation_Type.ON_USER,
                damage_action: CW_SpellAction_Damage.no_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                cost_action: CW_SpellAction_Cost.default_cost
                );
            spell.add_tag(CW_Spell_Tag.DEFEND);
            CW_Library_Manager.instance.spells.add(spell);
        }
        private static void add_gold_blade_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            anim_setting.loop_trace_limit = 33;
            anim_setting.loop_nr_limit = -1;
            anim_setting.point_to_dst = true;
            anim_setting.anim_froze_frame_idx = 3;
            anim_setting.frame_interval = 0.05f;
            anim_setting.trace_grad = 5;
            anim_setting.set_trace(AnimationTraceType.LINE);

            anim_setting.end_action = example_spell_end_action;
            anim_setting.frame_action = gold_blade_frame_action;
            CW_EffectManager.instance.load_as_controller("gold_blade_anim", "effects/gold_blade/", controller_setting: anim_setting, base_scale: 0.08f);
            CW_Asset_Spell spell = new CW_Asset_Spell(
                id: "gold_blade", anim_id: "gold_blade_anim",
                new CW_Element(new int[] { 0, 0, 0, 100, 0 }),
                rarity: 1, might: 1, cost: 0.05f, learn_level: 1, cast_level: 1,
                target_type: CW_Spell_Target_Type.ACTOR,
                target_camp: CW_Spell_Target_Camp.ENEMY,
                triger_type: CW_Spell_Triger_Type.ATTACK,
                anim_type: CW_Spell_Animation_Type.USER_TO_TARGET,
                damage_action: CW_SpellAction_Damage.defualt_damage,
                anim_action: CW_SpellAction_Anim.default_anim,
                cost_action: CW_SpellAction_Cost.default_cost
                );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            CW_Library_Manager.instance.spells.add(spell);
        }
        private static void add_example_spell()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.end_action = example_spell_end_action;
            CW_EffectManager.instance.load_as_controller("example_spell_anim", "effects/example/", controller_setting: anim_setting, base_scale: 0.015f);

            CW_Asset_Spell spell = new CW_Asset_Spell(
                    "example", "example_spell_anim", new CW_Element(), null, 1, 1, 0.05f, 1, 1, true, null, null, CW_Spell_Target_Type.ACTOR, CW_Spell_Target_Camp.ENEMY, CW_Spell_Triger_Type.ATTACK, CW_Spell_Animation_Type.ON_TARGET, CW_SpellAction_Damage.defualt_damage, CW_SpellAction_Anim.default_anim, null, CW_SpellAction_Cost.default_cost
                    );
            spell.add_tag(CW_Spell_Tag.ATTACK);
            CW_Library_Manager.instance.spells.add(spell);
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
                    CW_Actor.func_spawnOn((CW_Actor)anim.src_object, target, 0f);
                    anim.set_position(target.posV); 
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
            ((CW_Actor)anim.src_object).fast_data.health += (int)anim.cost_for_spell;
        }
    }
}
