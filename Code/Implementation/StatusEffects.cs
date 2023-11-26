using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.General.AboutStatusEffect;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Implementation;

internal static class StatusEffects
{
    public static void init()
    {
        add_element_statuses();
        add_common_statuses();
        add_fire_statuses();
        add_shield_statuses();
        add_bound_statuses();

        add_brutalize_status();

        add_ancestor_called_status();
    }

    private static void add_ancestor_called_status()
    {
        CW_StatusEffect status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_ancestor_called", new BaseStats(),
            60f,
            "", "", 1f,
            "effects/vine_bound/40",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.None
        );
        status_effect.action_on_update = (effect, object1, object2) =>
        {
            if (object2.objectType != MapObjectType.Actor) return;
            if (object1 == null || !object1.isAlive())
            {
                if (Toolbox.randomChance(0.8f))
                {
                    effect.effect_val = -1;
                }
                else
                {
                    effect.effect_val = 1;
                }

                effect.finished = true;
            }
        };
        status_effect.action_on_end = (effect, object1, object2) =>
        {
            if (effect.effect_val >= 0)
            {
                // 防止killHimself时更新血脉中的数据
                object2.a.data.clear_blood_nodes();
                object2.a.killHimself();
            }
        };
    }

    // 兽化
    private static void add_brutalize_status()
    {
        BaseStats bonus_stats = new();
        bonus_stats[CW_S.mod_health_regen] = 0.8f;
        bonus_stats[CW_S.mod_shield_regen] = 0.5f;
        bonus_stats[S.mod_health] = 0.3f;
        bonus_stats[S.mod_damage] = 0.5f;
        bonus_stats[S.mod_armor] = 0.3f;
        bonus_stats[S.mod_speed] = 0.2f;
        bonus_stats[S.scale] = 0.1f;
        CW_StatusEffect status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_brutalize", bonus_stats,
            10f,
            "", "", 1f,
            "effects/vine_bound/40",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Advanced
        );
        status_effect.action_on_get = (effect, object1, object2) =>
        {
            if (object2.objectType != MapObjectType.Actor) return;

            CW_Actor actor = (CW_Actor)object2;

            if (actor.asset.race != "yao") return;
            ActorAsset beast_stats = AssetManager.actor_library.get(actor.asset.id.Replace("_yao", ""));
            AnimationContainerUnit anim_beast =
                ActorAnimationLoader.loadAnimationUnit("actors/" + beast_stats.texture_path, beast_stats);

            actor.animationContainer = anim_beast;

            actor.checkSpriteToRender();
        };
        status_effect.action_on_update = (effect, object1, object2) =>
        {
            if (object2.objectType != MapObjectType.Actor) return;

            CW_Actor actor = (CW_Actor)object2;
            if (actor.attackedBy != null || actor.attackTarget != null)
            {
                effect.left_time = effect.status_asset.duration;
            }
        };
        status_effect.action_on_end = (effect, object1, object2) =>
        {
            if (object2.objectType != MapObjectType.Actor) return;

            CW_Actor actor = (CW_Actor)object2;

            if (actor.asset.race != "yao") return;
            actor.dirty_sprite_main = true;
            actor.checkSpriteToRender();
        };
    }

    private static void add_bound_statuses()
    {
        CW_StatusEffect status_effect;
        BaseStats bound_stats;
        // 藤缚
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.TIME_LIMIT,
            loop_time_limit = 3,
            loop_nr_limit = -1,
            anim_froze_frame_idx = 4,
            frame_interval = 0.05f
        };
        anim_setting.frame_action = (int pIdx, ref Vector2 pVec, ref Vector2 pDstVec,
            Animation.SpriteAnimation pAnim) =>
        {
            if (pAnim.src_object == null || !pAnim.src_object.isAlive())
            {
                pAnim.force_stop(false);
                return;
            }
            if (!pAnim.src_object.isActor())
            {
                return;
            }
            CW_Actor actor = (CW_Actor)pAnim.src_object;
            pAnim.set_scale(actor.stats[S.scale]);
            if (actor.statuses != null && actor.statuses.TryGetValue("status_vine_bound", out var status))
            {
                pAnim.play_time = Mathf.Max(0, 3 - status.left_time);
            }
            else
            {
                pAnim.force_stop(true);
            }
        };
        anim_setting.set_trace(AnimationTraceType.ATTACH);
        bound_stats = new BaseStats();
        bound_stats[S.speed] = -999999;
        bound_stats[S.attack_speed] = -999999;
        EffectManager.instance.load_as_controller("status_vine_bound_anim", "effects/vine_bound/",
            controller_setting: anim_setting, base_scale: 1f);
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_vine_bound", bound_stats,
            10f,
            "", "", 1f,
            "effects/vine_bound/40",
            new[] { StatusEffectTag.FETTER },
            StatusTier.Advanced
        );
        status_effect.anim_id = "status_vine_bound_anim";
        status_effect.action_on_update = (effect, object1, object2) =>
        {
            object2.a.has_status_frozen = true;
        };
        // 石化
        bound_stats = new BaseStats();
        bound_stats[S.speed] = -999999;
        bound_stats[S.attack_speed] = -999999;
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_landificate", bound_stats,
            10f,
            "", "", 1f,
            "effects/vine_bound/40",
            new[] { StatusEffectTag.FETTER },
            StatusTier.Advanced
        );
        status_effect.action_on_update = (effect, object1, object2) =>
        {
            object2.a.has_status_frozen = true;
            ((CW_Actor)object2).StartColorEffect("gray", 1);
        };
        // 冰封
        bound_stats = new BaseStats();
        bound_stats[S.speed] = -999999;
        bound_stats[S.attack_speed] = -999999;
        anim_setting = anim_setting.__deepcopy();
        anim_setting.frame_interval = 1f;
        
        anim_setting.frame_action = (int pIdx, ref Vector2 pVec, ref Vector2 pDstVec,
            Animation.SpriteAnimation pAnim) =>
        {
            if (pAnim.src_object == null || !pAnim.src_object.isAlive())
            {
                pAnim.force_stop(false);
                return;
            }
            if (!pAnim.src_object.isActor())
            {
                return;
            }
            CW_Actor actor = (CW_Actor)pAnim.src_object;
            pAnim.set_scale(actor.stats[S.scale]);
            if (actor.statuses != null && actor.statuses.TryGetValue("status_ice_bound", out var status))
            {
                pAnim.play_time = Mathf.Max(0, 3 - status.left_time);
            }
            else
            {
                pAnim.force_stop(true);
            }
        };
        anim_setting.set_trace(AnimationTraceType.ATTACH);
        for (int i = 0; i < 5; i++)
        {
            EffectManager.instance.load_as_controller("status_ice_bound_anim_" + i, "effects/ice_bound_" + i,
                controller_setting: anim_setting, base_scale: 1f);
        }

        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_ice_bound", bound_stats,
            10f,
            "", "", 1f,
            "effects/ice_bound_0/00",
            new[] { StatusEffectTag.FETTER },
            StatusTier.Advanced
        );
        status_effect.action_on_get = (effect, object1, object2) =>
        {
            if (object2 == null || !object2.isAlive() || object2.objectType != MapObjectType.Actor) return;
            EffectManager.instance.spawn_anim("status_ice_bound_anim_" + Toolbox.randomInt(0, 5),
                object2.currentPosition, object2.currentPosition, object2, object2, object2.stats[S.scale]);
        };
        status_effect.action_on_update = (effect, object1, object2) =>
        {
            object2.a.has_status_frozen = true;
        };
    }

    private static void add_shield_statuses()
    {
        BaseStats bonus_stats;
        CW_StatusEffect status_effect;
        AnimationSetting get_hit_anim_setting;
        // 金刚护体
        bonus_stats = new BaseStats();
        bonus_stats[S.armor] = 30f;
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_gold_shield", bonus_stats,
            10f,
            "status_gold_shield_anim", "effects/gold_shield", 1f,
            "effects/gold_shield/金刚护体2-0",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Advanced
        );
        status_effect.action_on_get = (effect, object1, object2) =>
        {
            effect.bonus_stats = new BaseStats();
            effect.bonus_stats.mergeStats(effect.status_asset.bonus_stats);
            effect.bonus_stats[S.armor] += effect.effect_val;
        };
        get_hit_anim_setting = new AnimationSetting
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
            loop_nr_limit = 1
        };
        get_hit_anim_setting.set_trace(AnimationTraceType.NONE);
        EffectManager.instance.load_as_controller("gold_shield_on_hit_anim", "effects/gold_shield_on_hit",
            controller_setting: get_hit_anim_setting, base_scale: 1f);
        status_effect.action_on_get_hit = (effect, object1, object2) =>
        {
            EffectManager.instance.spawn_anim(
                "gold_shield_on_hit_anim",
                object2.currentPosition, object2.currentPosition,
                object2, object2,
                object2.objectType == MapObjectType.Actor ? object2.stats[S.scale] : 1f
            );
        };
        // 水甲
        bonus_stats = new BaseStats();
        bonus_stats[S.armor] = 30f;
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_water_shield", bonus_stats,
            10f,
            "status_water_shield_anim", "effects/water_shield", 1f,
            "effects/water_shield/00",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Advanced
        );
        status_effect.action_on_get = (effect, object1, object2) =>
        {
            effect.bonus_stats = new BaseStats();
            effect.bonus_stats.mergeStats(effect.status_asset.bonus_stats);
            effect.bonus_stats[S.armor] += effect.effect_val;
        };
        get_hit_anim_setting = new AnimationSetting
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
            loop_nr_limit = 1
        };
        get_hit_anim_setting.set_trace(AnimationTraceType.NONE);
        EffectManager.instance.load_as_controller("water_shield_on_hit_anim", "effects/water_shield_on_hit",
            controller_setting: get_hit_anim_setting, base_scale: 1f);
        status_effect.action_on_get_hit = (effect, object1, object2) =>
        {
            EffectManager.instance.spawn_anim(
                "water_shield_on_hit_anim",
                object2.currentPosition, object2.currentPosition,
                object2, object2,
                object2.objectType == MapObjectType.Actor ? object2.stats[S.scale] : 1f
            );
        };
    }

    private static void add_fire_statuses()
    {
        CW_StatusEffect status_effect;
        // 三昧真火
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_samadhi_fire", new BaseStats(),
            10f,
            "status_samadhi_fire_anim", "effects/samadhi_fire", 1f,
            "effects/samadhi_fire/00",
            new[] { StatusEffectTag.NEGATIVE },
            StatusTier.Advanced
        );
        status_effect.action_interval = 1f;
        status_effect.action_on_update = (effect, attacker, target) =>
        {
            if (target.objectType != MapObjectType.Actor) return;
            target.getHit(effect.effect_val, pType: (AttackType)CW_AttackType.RealSpell, pAttacker: attacker);
        };
        // 红莲业火
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_loltus_fire", new BaseStats(),
            10f,
            "status_loltus_fire_anim", "effects/loltus_fire", 1f,
            "effects/loltus_fire/00",
            new[] { StatusEffectTag.NEGATIVE },
            StatusTier.Advanced
        );
        status_effect.action_interval = 1f;
        status_effect.action_on_update = (effect, attacker, target) =>
        {
            if (target.objectType != MapObjectType.Actor) return;
            if (target.a.data.kills == 0)
            {
                effect.left_time = -1;
                return;
            }

            target.getHit(effect.effect_val * Mathf.Log10(target.a.data.kills),
                pType: (AttackType)CW_AttackType.RealSpell, pAttacker: attacker);
        };
        // 九幽冥焰
        status_effect = FormatStatusEffect.create_simple_status_effect(
            "status_fen_fire", new BaseStats(),
            10f,
            "status_fen_fire_anim", "effects/fen_fire", 1f,
            "effects/fen_fire/00",
            new[] { StatusEffectTag.NEGATIVE },
            StatusTier.Advanced
        );
        status_effect.action_interval = 1f;
        status_effect.action_on_update = (effect, attacker, target) =>
        {
            if (target.objectType != MapObjectType.Actor) return;
            target.getHit(effect.effect_val * Mathf.Log10(target.a.data.kills), pType: (AttackType)CW_AttackType.Soul,
                pAttacker: attacker);
        };
    }

    private static void add_common_statuses()
    {
        BaseStats bonus_stats;
        // 破甲
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_armor] = -0.3f;
        FormatStatusEffect.create_simple_status_effect(
            "status_armor_expose", bonus_stats,
            10f,
            "", "", 0.5f,
            "effects/basalt_armor/玄武之甲",
            new[] { StatusEffectTag.POSITIVE }
        );
    }

    private static void add_element_statuses()
    {
        BaseStats bonus_stats;
        // 玄武之甲
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_armor] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_basalt_armor", bonus_stats,
            10f,
            "basalt_armor_anim", "effects/basalt_armor", 0.5f,
            "effects/basalt_armor/玄武之甲",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Basic
        );
        // 青龙之鳞
        bonus_stats = new BaseStats();
        bonus_stats[CW_S.health_regen] = 10f;
        bonus_stats[CW_S.mod_health_regen] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_gloong_scale", bonus_stats,
            10f,
            "gloong_scale_anim", "effects/gloong_scale", 0.5f,
            "effects/gloong_scale/青龙之鳞",
            new[] { StatusEffectTag.POSITIVE }
        );
        // 朱雀之羽
        bonus_stats = new BaseStats();
        FormatStatusEffect.create_simple_status_effect(
            "status_rosefinch_feather", bonus_stats,
            10f,
            "rosefinch_feather_anim", "effects/rosefinch_feather", 0.5f,
            "effects/rosefinch_feather/朱雀之羽",
            new[] { StatusEffectTag.POSITIVE },
            StatusTier.Advanced
        ).action_on_attack = (effect, target, attacker) =>
        {
            if (target.objectType != MapObjectType.Actor) return;
            // TODO: 强化效果
            ((CW_Actor)target).addStatusEffect("burning");
        };
        // 麒麟之角
        bonus_stats = new BaseStats();
        bonus_stats[CW_S.mod_spell_armor] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_unicorn_horn", bonus_stats,
            10f,
            "unicorn_horn_anim", "effects/unicorn_horn", 0.5f,
            "effects/unicorn_horn/麒麟之角",
            new[] { StatusEffectTag.POSITIVE }
        );
        // 白虎之牙
        bonus_stats = new BaseStats();
        bonus_stats[S.mod_damage] = 0.8f;
        FormatStatusEffect.create_simple_status_effect(
            "status_wtiger_tooth", bonus_stats,
            10f,
            "wtiger_tooth_anim", "effects/wtiger_tooth", 0.5f,
            "effects/wtiger_tooth/白虎之牙",
            new[] { StatusEffectTag.POSITIVE }
        );
    }
}