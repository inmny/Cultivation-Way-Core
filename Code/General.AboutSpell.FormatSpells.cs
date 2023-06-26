using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.General.AboutSpell
{
    /// <summary>
    /// 提供格式化的简单法术的一键创建
    /// </summary>
    public static class FormatSpells
    {
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 创建并添加一个简单的追踪投射物法术, 并同时创建添加其动画控制器
        /// </summary>
        ///
        /// <param name="id">法术的id</param>
        /// <param name="rarity">法术稀有度</param>
        /// <param name="anim_id">动画id</param>
        /// <param name="anim_path">动画文件夹路径</param>
        /// <param name="anim_type">动画类型, 释放者到目标/目标到释放者</param>
        /// <param name="element_container">法术元素</param>
        /// <param name="trigger_tags">触发条件</param>
        /// <param name="target_camp">目标阵营</param>
        /// <param name="target_type">目标类型</param>
        /// <param name="cultisys_require">修习需要的修炼体系类型</param>
        /// <param name="spell_cost_list">法术消耗列表</param>
        /// 
        /// <returns>新添加spell asset</returns>
        /// <exception cref="Exception"></exception>
        public static CW_SpellAsset create_track_projectile_spell(
                string id,
                string anim_id, string anim_path, SpellAnimType anim_type = SpellAnimType.USER_TO_TARGET,
                int rarity = 1,
                int[] element_container = null,
                SpellTriggerTag[] trigger_tags = null, 
                SpellTargetCamp target_camp = SpellTargetCamp.ENEMY, SpellTargetType target_type = SpellTargetType.ACTOR,
                CultisysType[] cultisys_require = null, KeyValuePair<string, float>[] spell_cost_list = null
            )
        {
            if(Manager.spells.contains(id)) {
                throw new Exception($"create_trace_projectile_spell: repeated spell id {id}");
            }
            if(anim_type != SpellAnimType.USER_TO_TARGET && anim_type != SpellAnimType.TARGET_TO_USER) {
                throw new Exception("create_trace_projectile_spell: anim_type must be USER_TO_TARGET or TARGET_TO_USER");
            }
            if(target_type != SpellTargetType.ACTOR) {
                throw new Exception("create_trace_projectile_spell: target_type must be ACTOR");
            }
            
            element_container ??= new int[] { 20, 20, 20, 20, 20 };
            trigger_tags ??= new SpellTriggerTag[] { SpellTriggerTag.ATTACK };
            cultisys_require ??= new CultisysType[] { CultisysType.WAKAN };
            spell_cost_list ??= new KeyValuePair<string, float>[] { 
                new(S.health, 1f)
            };

            CW_SpellAsset spell_asset = new()
            {
                id = id, rarity = rarity, element = new CW_Element(element_container),
                anim_id = anim_id, anim_type = anim_type,
                anim_action = anim_type== SpellAnimType.USER_TO_TARGET ? AnimActions.simple_user_to_target : AnimActions.simple_target_to_user,
                target_camp = target_camp, target_type = target_type
            };
            foreach(SpellTriggerTag tag in trigger_tags)
            {
                spell_asset.add_trigger_tag(tag);
            }
            foreach(CultisysType cultisys in cultisys_require)
            {
                spell_asset.add_cultisys_require(cultisys);
            }
            spell_asset.spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list);


            if (CW_Core.mod_state.anim_manager.get_controller(anim_id) != null)
            {
                Logger.Warn($"create_trace_projectile_spell: There is already animation {anim_id}, it will not create a new one for spell {id}");
            }
            else
            {
                AnimationSetting anim_setting = new()
                {
                    point_to_dst = true,
                    always_point_to_dst = true,
                    loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
                    end_action = anim_type == SpellAnimType.USER_TO_TARGET ? AboutAnim.EndActions.src_obj_damage_to_dst_obj : AboutAnim.EndActions.dst_obj_damage_to_src_obj
                };
                anim_setting.set_trace(AnimationTraceType.TRACK);

                CW_Core.mod_state.anim_manager.load_as_controller(
                    anim_id, anim_path, 100, 1, anim_setting
                );
            }
            
            Library.Manager.spells.add(spell_asset);
            return spell_asset;
        }
    }
}
