using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Actions;
using Cultivation_Way.Animation;
namespace Cultivation_Way.Content
{
    internal class W_Content_Spell
    {
        public static void add_spells()
        {
            add_example_spell();
            add_gold_blade_spell();
        }

        private static void add_gold_blade_spell()
        {
            CW_AnimationSetting gold_blade_setting = new CW_AnimationSetting();
            gold_blade_setting.loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT;
            gold_blade_setting.loop_trace_limit = 100f;
            gold_blade_setting.loop_nr_limit = -1;
            gold_blade_setting.point_to_dst = true;
            gold_blade_setting.anim_froze_frame_idx = 3;
            gold_blade_setting.set_trace(AnimationTraceType.TRACK);
            CW_EffectManager.instance.load_as_controller("gold_blade", "effects/gold_blade/", controller_setting: gold_blade_setting, base_scale: 0.08f);
        }
        private static void add_example_spell()
        {
            CW_AnimationSetting example_spell_anim_setting = new CW_AnimationSetting();
            example_spell_anim_setting.end_action = example_spell_end_action;
            CW_EffectManager.instance.load_as_controller("example_spell_anim", "effects/example/", controller_setting: example_spell_anim_setting, base_scale: 0.015f);
            CW_Asset_Spell example_spell = new CW_Asset_Spell(
                    "example", "example_spell_anim", CW_SpellAction_Damage.default_attack_enemy, new CW_Element(), 1, 1, 0.10f, 1, 1, true, null, null, CW_Spell_Target_Type.ACTOR, CW_Spell_Target_Camp.ENEMY, CW_Spell_Triger_Type.ATTACK, CW_Spell_Animation_Type.ON_TARGET, null, null, null
                    );
            example_spell.add_tag(CW_Spell_Tag.ATTACK);
            CW_Library_Manager.instance.spells.add(example_spell);
        }
        private static void example_spell_end_action(int cur_frame_idx, float src_x, float src_y, float dst_x, float dst_y, float play_time, float anim_x, float anim_y, BaseSimObject pUser, BaseSimObject pTarget)
        {
            //WorldBoxConsole.Console.print("example spell end successfully");
            ((CW_Actor)pUser).fast_data.health += 1;
        }
    }
}
