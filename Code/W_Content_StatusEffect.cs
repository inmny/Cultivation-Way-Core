using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
using Cultivation_Way.Others;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal static class W_Content_StatusEffect
    {

        internal static void add_status_effects()
        {
            add_gold_shield_status_effect();
            add_water_shield_status_effect();
            add_ice_bound_status_effect();
            add_vine_bound_status_effect();
            add_landicate_status_effect();
            add_burning_status_effect();
            add_curse_status_effect();
            add_corrode_status_effect();
            add_armor_expose_status_effect();
            add_wtiger_tooth_status_effect();
            add_unicorn_horn_status_effect();
            add_rosefinch_feather_status_effect();
            add_gdragon_scale_status_effect();
            add_basalt_armor_status_effect();

            add_samadhi_fire_status_effect();
            add_fen_fire_status_effect();
            add_loltus_fire_status_effect();

            add_brutalization_status_effect();

            load_status_effects_anims();
        }
        // 兽化 TODO: 添加get action，人物贴图变化
        private static void add_brutalization_status_effect()
        {
            CW_BaseStats bonus_stats = new CW_BaseStats();
            bonus_stats.mod_health_regen += 80;
            bonus_stats.mod_shield_regen += 50;
            bonus_stats.base_stats.mod_health += 30;
            bonus_stats.base_stats.mod_damage += 50;
            bonus_stats.base_stats.mod_speed += 30;
            bonus_stats.base_stats.mod_armor += 20;
            bonus_stats.base_stats.scale += 0.2f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_brutalization",
                anim_id: null,//"burning_anim",
                bonus_stats: bonus_stats,
                effect_val: 0,
                effect_time: 5f,
                action_on_get: change_body_sprite_to_beast,
                action_on_update: brutalization_on_update,
                action_on_end: change_body_sprite_to_yao
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }


        // 红莲业火
        private static void add_loltus_fire_status_effect()
        {
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_loltus_fire",
                anim_id: "status_loltus_fire_anim",//"burning_anim",
                bonus_stats: null,
                effect_val: 5,
                effect_time: 60f,
                action_on_update: loltus_fire_on_update
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 九幽冥火
        private static void add_fen_fire_status_effect()
        {
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_fen_fire",
                anim_id: "status_fen_fire_anim",//"burning_anim",
                bonus_stats: null,
                effect_val: 5,
                effect_time: 60f,
                action_on_update: fen_fire_on_update
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 三昧真火
        private static void add_samadhi_fire_status_effect()
        {
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_samadhi_fire",
                anim_id: "status_samadhi_fire_anim",
                bonus_stats: null,
                effect_val: 5,
                effect_time: 60f,
                action_on_update: samadhi_fire_on_update
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }

        private static void load_status_effects_anims()
        {
            CW_AnimationSetting anim_setting = new CW_AnimationSetting();
            anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
            anim_setting.loop_time_limit = 6f;
            anim_setting.frame_interval = 0.1f;
            anim_setting.set_trace(AnimationTraceType.ATTACH);
            for (int i = 0; i < 5; i++)
            {
                CW_EffectManager.instance.load_as_controller("ice_bound_anim_" + i, "effects/ice_bound_" + i + "/", controller_setting: anim_setting, base_scale: 1f);
            }
            anim_setting.loop_time_limit = 60f;
            CW_EffectManager.instance.load_as_controller("status_samadhi_fire_anim", "effects/samadhi_fire", controller_setting: anim_setting, base_scale: 1f);
            CW_EffectManager.instance.load_as_controller("status_fen_fire_anim", "effects/fen_fire", controller_setting: anim_setting, base_scale: 1f);
            CW_EffectManager.instance.load_as_controller("status_loltus_fire_anim", "effects/loltus_fire", controller_setting: anim_setting, base_scale: 1f);
        }
        // 自定义效果
        private static void add_status_effect_for_custom()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats();
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_custom",
                anim_id: null,
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 玄武之甲
        private static void add_basalt_armor_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.mod_armor = 80f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_basalt_armor",
                anim_id: "basalt_armor_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 青龙之鳞
        private static void add_gdragon_scale_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.mod_health_regen = 80f; base_bonus_stats.health_regen = 10;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_gdragon_scale",
                anim_id: "gdragon_scale_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 朱雀之羽
        private static void add_rosefinch_feather_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats();
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_rosefinch_feather",
                anim_id: "rosefinch_feather_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f,
                action_on_attack: rosefinch_feather_on_attack
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 麒麟之角
        private static void add_unicorn_horn_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.mod_spell_armor = 80f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_unicorn_horn",
                anim_id: "unicorn_horn_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 白虎之牙
        private static void add_wtiger_tooth_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.anti_injury = 0.5f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_wtiger_tooth",
                anim_id: "wtiger_tooth_anim",
                anim_scale_co: 0.5f,
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 破甲
        private static void add_armor_expose_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor -= 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_armor_expose",
                anim_id: null,//"armor_expose_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 腐蚀
        private static void add_corrode_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); //base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_corrode",
                anim_id: null,//"corrode_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 诅咒
        private static void add_curse_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.mod_health -= 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_curse",
                anim_id: null,//"curse_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 灼伤
        private static void add_burning_status_effect()
        {
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_burning",
                anim_id: null,//"burning_anim",
                bonus_stats: null,
                effect_time: 60f
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 石化
        private static void add_landicate_status_effect()
        {
            CW_BaseStats bound_stats = new CW_BaseStats();
            bound_stats.base_stats.knockbackReduction = 100f;
            bound_stats.base_stats.speed = -100000f;
            bound_stats.base_stats.attackSpeed = -100000f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_landificate",
                anim_id: null,
                bonus_stats: bound_stats,
                effect_time: 6f,
                action_on_get: landificate_bound_on_get,
                tags: new List<CW_StatusEffect_Tag> { CW_StatusEffect_Tag.BOUND }
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 藤缚
        private static void add_vine_bound_status_effect()
        {
            CW_BaseStats bound_stats = new CW_BaseStats();
            bound_stats.base_stats.knockbackReduction = 100f;
            bound_stats.base_stats.speed = -100000f;
            bound_stats.base_stats.attackSpeed = -100000f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_vine_bound",
                anim_id: "vine_bound_anim",
                bonus_stats: bound_stats,
                effect_time: 6f,
                tags: new List<CW_StatusEffect_Tag> { CW_StatusEffect_Tag.BOUND }
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 冰封
        private static void add_ice_bound_status_effect()
        {
            CW_BaseStats bound_stats = new CW_BaseStats();
            bound_stats.base_stats.knockbackReduction = 100f;
            bound_stats.base_stats.speed = -100000f;
            bound_stats.base_stats.attackSpeed = -100000f;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_ice_bound",
                anim_id: null,
                bonus_stats: bound_stats,
                effect_time: 6f,
                action_on_get: ice_bound_on_get,
                tags: new List<CW_StatusEffect_Tag> { CW_StatusEffect_Tag.BOUND}
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 金刚护体
        private static void add_gold_shield_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_gold_shield",
                anim_id: "gold_shield_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f,
                action_on_get: copy_bonus_stats_on_get,
                action_on_hit: gold_shield_on_hit
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // 水甲
        private static void add_water_shield_status_effect()
        {
            CW_BaseStats base_bonus_stats = new CW_BaseStats(); base_bonus_stats.base_stats.armor = 30;
            CW_Asset_StatusEffect status_effect = new CW_Asset_StatusEffect(
                id: "status_water_shield",
                anim_id: "water_shield_anim",
                bonus_stats: base_bonus_stats,
                effect_time: 60f,
                action_on_get: water_shield_on_get,
                action_on_hit: water_shield_on_hit
                );
            CW_Library_Manager.instance.status_effects.add(status_effect);
        }
        // TODO: 添加对建筑的支持
        private static void water_shield_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            copy_bonus_stats_on_get(status_effect, _obejct);
            if(_obejct.objectType == MapObjectType.Actor)
            {
                ((CW_Actor)_obejct).remove_status_effect_forcely("status_burning");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private static void copy_bonus_stats_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            status_effect.bonus_stats = status_effect.bonus_stats.deepcopy();
        }
        private static void gold_shield_on_hit(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject attacker)
        {
            CW_EffectManager.instance.spawn_anim("gold_shield_on_hit_anim", _obejct.currentPosition, _obejct.currentPosition, _obejct, _obejct, _obejct.objectType == MapObjectType.Actor ? ((CW_Actor)_obejct).cw_cur_stats.base_stats.scale : 1f);   
        }
        private static void water_shield_on_hit(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject attacker)
        {
            CW_EffectManager.instance.spawn_anim("water_shield_on_hit_anim", _obejct.currentPosition, _obejct.currentPosition, _obejct, _obejct, _obejct.objectType == MapObjectType.Actor ? ((CW_Actor)_obejct).cw_cur_stats.base_stats.scale : 1f);
        }
        // TODO: 对建筑的支持
        private static void rosefinch_feather_on_attack(CW_StatusEffectData status_effect, BaseSimObject _obejct, BaseSimObject target)
        {
            if (target.objectType != MapObjectType.Actor) return;
            ((CW_Actor)target).add_status_effect("status_burning");
        }
        // TODO: 对建筑的支持
        private static void ice_bound_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            if(_obejct == null || _obejct.objectType != MapObjectType.Actor) return;
            CW_EffectManager.instance.spawn_anim("ice_bound_anim_" + Toolbox.randomInt(0, 5), _obejct.currentPosition, _obejct.currentPosition, _obejct, _obejct, ((CW_Actor)_obejct).cw_cur_stats.base_stats.scale);
        }
        // TODO: 对建筑的支持
        private static void landificate_bound_on_get(CW_StatusEffectData status_effect, BaseSimObject _obejct)
        {
            if (_obejct == null || _obejct.objectType != MapObjectType.Actor) return;
            ((CW_Actor)_obejct).start_color_effect("grey", status_effect.status_asset.effect_time);
        }
        private static void samadhi_fire_on_update(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;
            Utils.CW_SpellHelper.cause_damage_to_target(status_effect.user, _object, status_effect.effect_val,  Others.CW_Enums.CW_AttackType.Status_God);
        }
        private static void loltus_fire_on_update(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;
            if (((CW_Actor)_object).fast_data.kills == 0) status_effect.force_finish();
            Utils.CW_SpellHelper.cause_damage_to_target(status_effect.user, _object, ((CW_Actor)_object).fast_data.kills * status_effect.effect_val, Others.CW_Enums.CW_AttackType.Status_Spell);
        }
        private static void fen_fire_on_update(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;
            Utils.CW_SpellHelper.cause_damage_to_target(status_effect.user, _object, status_effect.effect_val, Others.CW_Enums.CW_AttackType.Status_Spell);
            if (!_object.base_data.alive)
            {
                // TODO: 转换成骷髅
                ActionLibrary.turnIntoSkeleton(_object);
            }
        }


        private static void change_body_sprite_to_beast(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;
            
            CW_Actor actor = (CW_Actor)_object;

            if (actor.stats.race != "yao") return;
            CW_ActorStats beast_stats = CW_Library_Manager.instance.units.get(actor.stats.id.Replace("_yao", ""));
            AnimationDataUnit actorAnimationData = ActorAnimationLoader.loadAnimationUnit("actors/" + beast_stats.origin_stats.texture_path, beast_stats.origin_stats);

            CW_Actor.set_actorAnimationData(actor, actorAnimationData);
            //CW_Actor.func_setBodySprite(actor, Resources.Load<Sprite>("actors/" + beast_stats.origin_stats.texture_path + "/walk_0"));
            //bool __is_moving = CW_Actor.get_is_moving(actor);
            //if (!__is_moving) CW_Actor.set_is_moving(actor, true);
            CW_Actor.func_updateAnimation(actor, 0, true);
            //if (!__is_moving) CW_Actor.set_is_moving(actor, false);
        }

        private static void change_body_sprite_to_yao(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;

            CW_Actor actor = (CW_Actor)_object;

            if (actor.stats.race != "yao") return;
            CW_Actor.set_actorAnimationData(actor, ActorAnimationLoader.loadAnimationUnit("actors/" + actor.stats.texture_path, actor.stats));
            //CW_Actor.func_setBodySprite(actor, Resources.Load<Sprite>("actors/" + actor.stats.texture_path + "/walk_0"));
            //bool __is_moving = CW_Actor.get_is_moving(actor);
            //if (!__is_moving) CW_Actor.set_is_moving(actor, true);
            CW_Actor.func_updateAnimation(actor, 0, true);
            //if (!__is_moving) CW_Actor.set_is_moving(actor, false);
        }
        private static void brutalization_on_update(CW_StatusEffectData status_effect, BaseSimObject _object)
        {
            if (_object.objectType != MapObjectType.Actor) return;
            CW_Actor actor = (CW_Actor)_object;
            if (actor.is_in_battle())
            {
                status_effect.left_time = Others.CW_Constants.battle_timer;
            }
        }
    }
}
