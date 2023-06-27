using System;
using System.Collections.Generic;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.General.AboutAnim;
using Cultivation_Way.Library;
using Cultivation_Way.Others;

namespace Cultivation_Way.General.AboutSpell;

/// <summary>
///     提供格式化的简单法术的一键创建
/// </summary>
public static class FormatSpells
{
    /// <summary>
    ///     创建并添加一个简单的追踪投射物法术, 并同时创建添加其动画控制器
    /// </summary>
    /// <param name="id">法术的id</param>
    /// <param name="rarity">法术稀有度</param>
    /// <param name="anim_id">动画id</param>
    /// <param name="anim_path">动画文件夹路径</param>
    /// <param name="anim_scale">动画大小</param>
    /// <param name="anim_type">动画类型, 释放者到目标/目标到释放者, USER_TO_TARGET/TARGET_TO_USER</param>
    /// <param name="element_container">法术元素</param>
    /// <param name="trigger_tags">触发条件, 默认ATTACK</param>
    /// <param name="target_type">目标类型, ACTOR或BUILDING</param>
    /// <param name="cultisys_require">修习需要的修炼体系类型, 默认WAKAN</param>
    /// <param name="spell_cost_list">法术消耗列表</param>
    /// <returns>新添加spell asset</returns>
    /// <exception cref="Exception">已存在对应id的法术; <paramref name="anim_type"/>不满足限定; <paramref name="target_type"/>不满足限定</exception>
    public static CW_SpellAsset create_track_projectile_attack_spell(
        string id,
        string anim_id, string anim_path, float anim_scale = 1f, SpellAnimType anim_type = SpellAnimType.USER_TO_TARGET,
        int rarity = 1,
        int[] element_container = null,
        SpellTriggerTag[] trigger_tags = null,
        SpellTargetType target_type = SpellTargetType.ACTOR,
        CultisysType[] cultisys_require = null, KeyValuePair<string, float>[] spell_cost_list = null
    )
    {
        if (Manager.spells.contains(id))
        {
            throw new Exception($"create_trace_projectile_spell: repeated spell id {id}");
        }

        if (anim_type != SpellAnimType.USER_TO_TARGET && anim_type != SpellAnimType.TARGET_TO_USER)
        {
            throw new Exception("create_trace_projectile_spell: anim_type must be USER_TO_TARGET or TARGET_TO_USER");
        }

        if (target_type != SpellTargetType.ACTOR && target_type != SpellTargetType.BUILDING)
        {
            throw new Exception("create_trace_projectile_spell: target_type must be ACTOR");
        }

        element_container ??= new[] { 20, 20, 20, 20, 20 };
        trigger_tags ??= new[] { SpellTriggerTag.ATTACK };
        cultisys_require ??= new[] { CultisysType.WAKAN };
        spell_cost_list ??= new KeyValuePair<string, float>[]
        {
            new(S.health, 1f)
        };

        CW_SpellAsset spell_asset = new()
        {
            id = id, rarity = rarity, element = new CW_Element(element_container),
            anim_id = anim_id, anim_type = anim_type,
            anim_action = anim_type == SpellAnimType.USER_TO_TARGET
                ? AnimActions.simple_user_to_target
                : AnimActions.simple_target_to_user,
            target_camp = SpellTargetCamp.ENEMY, target_type = target_type,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell_asset.add_trigger_tags(trigger_tags);
        spell_asset.add_cultisys_requires(cultisys_require);


        if (CW_Core.mod_state.anim_manager.get_controller(anim_id) != null)
        {
            Logger.Warn(
                $"create_trace_projectile_spell: There is already animation {anim_id}, it will not create a new one for spell {id}");
        }
        else
        {
            AnimationSetting anim_setting = new()
            {
                point_to_dst = true,
                always_point_to_dst = true,
                loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
                end_action = anim_type == SpellAnimType.USER_TO_TARGET
                    ? EndActions.src_obj_damage_to_dst_obj
                    : EndActions.dst_obj_damage_to_src_obj
            };
            anim_setting.set_trace(AnimationTraceType.TRACK);

            CW_Core.mod_state.anim_manager.load_as_controller(
                anim_id, anim_path, 100, anim_scale, anim_setting
            );
        }

        Manager.spells.add(spell_asset);
        return spell_asset;
    }

    /// <summary>
    ///     创造并添加一个简单的给予使用者状态的法术
    /// </summary>
    /// <param name="id">法术id</param>
    /// <param name="status_id">给予的状态效果的id, 要求在调用前已经添加</param>
    /// <param name="anim_id">可选的动画id, 将在使用者身上播放一次</param>
    /// <param name="anim_path">可选动画的文件夹路径</param>
    /// <param name="anim_scale">可选动画的大小</param>
    /// <param name="rarity">稀有度</param>
    /// <param name="element_container">元素数组</param>
    /// <param name="trigger_tags">触发条件, 默认ATTACK</param>
    /// <param name="cultisys_require">修炼体系类型要求, 默认WAKAN</param>
    /// <param name="spell_cost_list">法术消耗列表</param>
    /// <returns>新添加的spell asset</returns>
    /// <exception cref="Exception">status_id的状态效果未添加; 已存在对应id的法术</exception>
    public static CW_SpellAsset create_give_self_status_spell(
        string id, string status_id,
        string anim_id = "", string anim_path = "", float anim_scale = 1f,
        int rarity = 1,
        int[] element_container = null,
        SpellTriggerTag[] trigger_tags = null,
        CultisysType[] cultisys_require = null, KeyValuePair<string, float>[] spell_cost_list = null
    )
    {
        if (!Manager.statuses.contains(status_id))
            throw new Exception($"create_give_status_spell: not found status {status_id} when the function called");
        if (Manager.spells.contains(id)) throw new Exception($"create_give_status_spell: repeated spell {id}");

        element_container ??= new[] { 20, 20, 20, 20, 20 };
        trigger_tags ??= new[] { SpellTriggerTag.ATTACK };
        cultisys_require ??= new[] { CultisysType.WAKAN };
        spell_cost_list ??= new KeyValuePair<string, float>[]
        {
            new(S.health, 1f)
        };

        CW_SpellAsset spell_asset = new()
        {
            id = id, rarity = rarity, element = new(element_container),
            target_camp = SpellTargetCamp.ALIAS, target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list),
            spell_learn_check = LearnChecks.default_learn_check,
            spell_action = SpellActions.generate_give_status_spell_action(status_id),
            anim_type = SpellAnimType.ON_USER
        };
        spell_asset.add_trigger_tags(trigger_tags);
        spell_asset.add_cultisys_requires(cultisys_require);

        if (anim_id != null)
        {
            if (CW_Core.mod_state.anim_manager.get_controller(anim_id) != null)
            {
                Logger.Warn(
                    $"create_give_self_status_spell: There is already animation {anim_id}, it will not create a new one for spell {id}");
            }
            else
            {
                AnimationSetting setting = new()
                {
                    loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
                    loop_nr_limit = 1
                };
                EffectController controller =
                    CW_Core.mod_state.anim_manager.load_as_controller(anim_id, anim_path, base_scale: anim_scale,
                        controller_setting: setting);
                if (controller != null)
                {
                    spell_asset.anim_id = anim_id;
                    spell_asset.anim_action = AnimActions.simple_on_something;
                }
            }
        }

        Manager.spells.add(spell_asset);
        return spell_asset;
    }

    /// <summary>
    ///     创建并添加一个简单的作用于目标的(默认攻击)法术
    /// </summary>
    /// <param name="id">法术id</param>
    /// <param name="anim_id">动画id</param>
    /// <param name="anim_path">动画文件夹路径</param>
    /// <param name="anim_scale">动画大小</param>
    /// <param name="rarity">法术稀有度</param>
    /// <param name="target_type">目标类型</param>
    /// <param name="element_container">法术元素</param>
    /// <param name="trigger_tags">触发条件</param>
    /// <param name="frame_action">自定义动画帧行为</param>
    /// <param name="cultisys_require">修炼体系类型要求</param>
    /// <param name="spell_cost_list">法术消耗</param>
    /// <returns></returns>
    /// <exception cref="Exception">已存在对应id的法术</exception>
    public static CW_SpellAsset create_on_target_attack_spell(
        string id, string anim_id, string anim_path, float anim_scale = 1f,
        int rarity = 1, SpellTargetType target_type = SpellTargetType.ACTOR,
        int[] element_container = null,
        SpellTriggerTag[] trigger_tags = null,
        AnimFrameAction frame_action = null,
        CultisysType[] cultisys_require = null, KeyValuePair<string, float>[] spell_cost_list = null
    )
    {
        if (Manager.spells.contains(id)) throw new Exception($"create_on_target_spell: repeated spell {id}");

        element_container ??= new[] { 20, 20, 20, 20, 20 };
        trigger_tags ??= new[] { SpellTriggerTag.ATTACK };
        cultisys_require ??= new[] { CultisysType.WAKAN };
        spell_cost_list ??= new KeyValuePair<string, float>[]
        {
            new(S.health, 1f)
        };

        CW_SpellAsset spell_asset = new()
        {
            id = id,
            anim_id = anim_id,
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            rarity = rarity,
            element = new CW_Element(element_container),
            target_camp = SpellTargetCamp.ENEMY,
            target_type = target_type,
            spell_learn_check = LearnChecks.default_learn_check,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list)
        };
        if (frame_action != null) spell_asset.spell_action = SpellActions.default_damage_on;
        spell_asset.add_trigger_tags(trigger_tags);
        spell_asset.add_cultisys_requires(cultisys_require);

        if (CW_Core.mod_state.anim_manager.get_controller(anim_id) != null)
        {
            Logger.Warn(
                $"create_on_target_attack_spell: There is already animation {anim_id}, it will not create a new one for spell {id}");
        }
        else
        {
            AnimationSetting anim_setting = new()
            {
                frame_action = frame_action,
                loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
                loop_nr_limit = 1
            };
            anim_setting.set_trace(AnimationTraceType.NONE);
            CW_Core.mod_state.anim_manager.load_as_controller(
                anim_id, anim_path, 100, anim_scale, anim_setting
            );
        }

        Manager.spells.add(spell_asset);
        return spell_asset;
    }
}