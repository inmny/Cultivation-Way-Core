using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using Cultivation_Way.Library;
using Cultivation_Way.Extensions;
using UnityEngine;

namespace Cultivation_Way
{
    public class CW_Actor : Actor
    {
        public Library.CW_ActorStats cw_stats = null;
        public CW_BaseStats cw_cur_stats = null;
        public CW_ActorData cw_data = null;
        public CW_ActorStatus cw_status = null;
        internal List<CW_Actor> compose_actors = null;
        internal List<CW_Building> compose_buildings = null;
        internal Dictionary<string, CW_StatusEffectData> status_effects = null;
        public List<string> cur_spells = null;
        public bool can_act = true;
        internal float m_attackSpeed_seconds = 0;
        internal float default_spell_timer = 1f;
        internal float s_spell_seconds = 1f;
        internal float __battle_timer = 0f;
        private static List<string> __status_effects_to_remove = new List<string>();
        private static List<CW_StatusEffectData> __status_effects_to_update = new List<CW_StatusEffectData>();
        private static List<City> __tmp_city_list = new List<City>();
        /// <summary>
        /// 仅提供高效访问，待权限开放后删除
        /// </summary>
        public ActorStatus fast_data = null;
        internal HashSet<BaseMapObject> fast_targets_to_ignore = null;
        internal WorldTimer fast_shake_timer = null;
        #region Getter
        public static Func<Actor, BaseSimObject> get_attackedBy = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("attackedBy");
        public static Func<Actor, BaseSimObject> get_attackTarget = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("attackTarget");
        public static Func<Actor, float> get_attackTimer = CW_ReflectionHelper.create_getter<Actor, float>("attackTimer");
        public static Func<Actor, Dictionary<string, StatusEffectData>> get_activeStatus_dict = CW_ReflectionHelper.create_getter<Actor, Dictionary<string, StatusEffectData>>("activeStatus_dict");
        internal static Func<Actor, PersonalityAsset> get_s_personality = CW_ReflectionHelper.create_getter<Actor, PersonalityAsset>("s_personality");
        internal static Func<Actor, HashSet<BaseMapObject>> get_targets_to_ignore = CW_ReflectionHelper.create_getter<Actor, HashSet<BaseMapObject>>("targetsToIgnore");
        internal static Func<Actor, ActorStatus> get_data = CW_ReflectionHelper.create_getter<Actor, ActorStatus>("data");
        internal static Func<Actor, BaseStats> get_curstats = CW_ReflectionHelper.create_getter<Actor, BaseStats>("curStats");
        internal static Func<Actor, bool> get_event_full_heal = CW_ReflectionHelper.create_getter<Actor, bool>("event_full_heal");
        internal static Func<Actor, List<ActorTrait>> get_s_special_effect_traits = CW_ReflectionHelper.create_getter<Actor, List<ActorTrait>>("s_special_effect_traits");
        internal static Func<Actor, WorldTimer> get_shake_timer = CW_ReflectionHelper.create_getter<Actor, WorldTimer>("shakeTimer");
        internal static Func<Actor, BaseSimObject> get_beh_actor_target = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("beh_actor_target");
        public static Func<Actor, bool> get_is_moving = CW_ReflectionHelper.create_getter<Actor, bool>("is_moving");
        #endregion
        #region Setter
        internal static Action<Actor, string> set_current_texture = CW_ReflectionHelper.create_setter<Actor, string>("current_texture");
        internal static Action<Actor, BaseSimObject> set_beh_actor_target = CW_ReflectionHelper.create_setter<Actor, BaseSimObject>("beh_actor_target");
        internal static Action<Actor, float> set_timer_action = CW_ReflectionHelper.create_setter<Actor, float>("timer_action");
        internal static Action<Actor, bool> set_statsDirty = CW_ReflectionHelper.create_setter<Actor, bool>("statsDirty");
        internal static Action<Actor, bool> set_event_full_heal = CW_ReflectionHelper.create_setter<Actor, bool>("event_full_heal");
        internal static Action<Actor, bool> set_item_sprite_dirty = CW_ReflectionHelper.create_setter<Actor, bool>("item_sprite_dirty");
        internal static Action<Actor, bool> set_trait_weightless = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_weightless");
        internal static Action<Actor, bool> set_trait_peaceful = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_peaceful");
        internal static Action<Actor, bool> set_trait_fire_resistant = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_fire_resistant");
        internal static Action<Actor, bool> set_status_frozen = CW_ReflectionHelper.create_setter<Actor, bool>("_status_frozen");
        public static Action<Actor, float> set_attackTimer = CW_ReflectionHelper.create_setter<Actor, float>("attackTimer");
        internal static Action<Actor, float> set_s_attackSpeed_seconds = CW_ReflectionHelper.create_setter<Actor, float>("s_attackSpeed_seconds");
        internal static Action<Actor, WeaponType> set_s_attackType = CW_ReflectionHelper.create_setter<Actor, WeaponType>("s_attackType");
        internal static Action<Actor, string> set_s_slashType = CW_ReflectionHelper.create_setter<Actor, string>("s_slashType");
        internal static Action<Actor, string> set_s_weapon_texture = CW_ReflectionHelper.create_setter<Actor, string>("s_weapon_texture");
        internal static Action<Actor, PersonalityAsset> set_s_personality = CW_ReflectionHelper.create_setter<Actor, PersonalityAsset>("s_personality");
        public static Action<Actor, ProfessionAsset> set_professionAsset = CW_ReflectionHelper.create_setter<Actor, ProfessionAsset>("professionAsset");
        internal static Action<Actor, ActorStatus> set_data = CW_ReflectionHelper.create_setter<Actor, ActorStatus>("data");
        internal static Action<Actor, List<ActorTrait>> set_s_special_effect_traits = CW_ReflectionHelper.create_setter<Actor, List<ActorTrait>>("s_special_effect_traits");
        public static Action<Actor, BaseSimObject> set_attackedBy = CW_ReflectionHelper.create_setter<Actor, BaseSimObject>("attackedBy");
        internal static Action<Actor, BaseSimObject> set_attackTarget = CW_ReflectionHelper.create_setter<Actor, BaseSimObject>("attackTarget");
        internal static Action<Actor, float> set_colorEffect = CW_ReflectionHelper.create_setter<Actor, float>("colorEffect");
        internal static Action<Actor, Material> set_colorMaterial = CW_ReflectionHelper.create_setter<Actor, Material>("colorMaterial");
        internal static Action<Actor, AnimationDataUnit> set_actorAnimationData = CW_ReflectionHelper.create_setter<Actor, AnimationDataUnit>("actorAnimationData");
        internal static Action<Actor, bool> set_is_moving = CW_ReflectionHelper.create_setter<Actor, bool>("is_moving");
        #endregion
        #region Func
        internal static Action<Actor> func_loadTexture = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("loadTexture");
        public static Action<Actor, Kingdom> func_setKingdom = (Action<Actor, Kingdom>)CW_ReflectionHelper.get_method<Actor>("setKingdom");
        public static Action<Actor, Sprite> func_setBodySprite = (Action<Actor, Sprite>)CW_ReflectionHelper.get_method<Actor>("setBodySprite");
        public static Action<Actor, WorldTile, float> func_spawnOn = (Action<Actor, WorldTile, float>)CW_ReflectionHelper.get_method<Actor>("spawnOn");
        public static Func<Actor, string, bool> func_haveStatus = (Func<Actor, string, bool>)CW_ReflectionHelper.get_method<Actor>("haveStatus");
        public static Func<Actor, BaseSimObject, bool> func_tryToAttack = (Func<Actor, BaseSimObject, bool>)CW_ReflectionHelper.get_method<Actor>("tryToAttack");
        public static Func<Actor, BaseSimObject, bool> func_canAttackTarget = (Func<Actor, BaseSimObject, bool>)CW_ReflectionHelper.get_method<Actor>("canAttackTarget");
        internal static Action<Actor> func_updateTargetScale = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("updateTargetScale");
        internal static Action<Actor> func_findHeadSprite = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("findHeadSprite");
        internal static Action<Actor, bool> func_checkMadness = (Action<Actor, bool>)CW_ReflectionHelper.get_method<Actor>("checkMadness");
        internal static Action<Actor, int> func_newCreature = (Action<Actor, int>)CW_ReflectionHelper.get_method<Actor>("newCreature");
        internal static Action<Actor> func_create = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("create");
        internal static Action<Actor, float, bool, AttackType, BaseSimObject, bool> func_getHit = (Action<Actor, float, bool, AttackType, BaseSimObject, bool>)CW_ReflectionHelper.get_method<Actor>("getHit");
        internal static Action<Actor, float> func_updateColorEffect = (Action<Actor, float>)CW_ReflectionHelper.get_method<Actor>("updateColorEffect");
        internal static Action<Actor, Vector3, WorldTile, bool, bool, float> func_punchTargetAnimation = (Action<Actor, Vector3, WorldTile, bool, bool, float>)CW_ReflectionHelper.get_method<Actor>("punchTargetAnimation");
        
        internal static Action<Actor, float, bool> func_updateAnimation = (Action<Actor, float, bool>)CW_ReflectionHelper.get_method<Actor>("updateAnimation");
        #endregion
        public void add_force(float x, float y, float z)
        {
            x *= Mathf.Max(0, 1 - this.cw_cur_stats.base_stats.knockbackReduction / 100);
            y *= Mathf.Max(0, 1 - this.cw_cur_stats.base_stats.knockbackReduction / 100);
            z *= Mathf.Max(0, 1 - this.cw_cur_stats.base_stats.knockbackReduction / 100);
            this.addForce(x, y, z);
        }
        /// <summary>
        /// 化形
        /// </summary>
        internal void transform_to_yao()
        {
            CW_ActorData cw_data = this.cw_data.deepcopy();
            ActorStatus status = this.fast_data.deepcopy();
            status.actorID = MapBox.instance.mapStats.getNextId("unit");
            status.statsID = status.statsID + "_yao";
            status.skin = -1;
            status.skin_set = -1;
            CW_Actor new_one = Utils.CW_Utils_ActorTools.spawn_actor(status.statsID, this.currentTile, status, cw_data, 0);
            status.transportID = String.Empty;
            status.gender = Toolbox.randomBool()?ActorGender.Male:ActorGender.Female;
            if(MoveCamera.inSpectatorMode() && MoveCamera.focusUnit == this)
            {
                MoveCamera.focusUnit = new_one;
            }
            if (!string.IsNullOrEmpty(cw_data.cultibook_id))
            {
                CW_Library_Manager.instance.cultibooks.get(cw_data.cultibook_id).cur_culti_nr++;
            }
            if (!string.IsNullOrEmpty(cw_data.special_body_id))
            {
                CW_Library_Manager.instance.special_bodies.get(cw_data.special_body_id).cur_own_nr++;
            }
            new_one.takeItems(this, new_one.stats.take_items_ignore_range_weapons);
            this.killHimself(true, AttackType.GrowUp, false, false);
            Cultivation_Way.Content.Harmony.W_Harmony_Actor.__actor_updateStats(new_one);
            if (!string.IsNullOrEmpty(status.firstName))
            {
                status.firstName = CW_NameGenerator.gen_name(new_one.stats.nameTemplate, new_one);
            }
            if (new_one.city == null)
            {
                __tmp_city_list.Clear();
                foreach(City city in MapBox.instance.citiesList)
                {
                    CW_CityData cw_city_data = (CW_CityData)CW_City.get_data(city);
                    if (cw_city_data.race != "yao") continue;
                    __tmp_city_list.Add(city);
                    if(cw_city_data.most_unit_id == new_one.stats.id && new_one.currentTile.isSameIsland(city.getTile()))
                    {
                        city.addNewUnit(new_one, true);
                        return;
                    }
                }
                if (__tmp_city_list.Count == 0) return;
                if(Toolbox.randomChance(0.8f))
                {
                    City city_to_join = __tmp_city_list.GetRandom();
                    if (new_one.currentTile.isSameIsland(city_to_join.getTile()))
                    {
                        __tmp_city_list.GetRandom().addNewUnit(new_one);
                    }
                }
            }
        }
        /// <summary>
        /// 是否处在战斗状态
        /// </summary>
        /// <returns></returns>
        public bool is_in_battle()
        {
            return this.__battle_timer > 0;
        }
        // TODO: 等建筑拓展后适配建筑
        public bool is_in_default_attack_range(BaseSimObject target)
        {
            return Toolbox.DistVec3(this.currentPosition, target.currentPosition) < this.cw_cur_stats.base_stats.range + (target.objectType == MapObjectType.Actor?((CW_Actor)target).cw_cur_stats.base_stats.size : 5);
        }
        /// <summary>
        /// 强制添加修炼体系
        /// </summary>
        /// <param name="cultisys_id">修炼体系id</param>
        public void add_cultisys(string cultisys_id)
        {
            this.cw_status.can_culti = true;
            this.cw_data.cultisys |= CW_Library_Manager.instance.cultisys.get(cultisys_id)._tag;
        }
        public void try_to_set_attack_target_by_attacked_by()
        {
            BaseSimObject cur_target = CW_Actor.get_attackTarget(this);
            BaseSimObject attacked_by = CW_Actor.get_attackedBy(this);
            if(cur_target != null && attacked_by != null && !this.fast_targets_to_ignore.Contains(attacked_by) && CW_Actor.func_canAttackTarget(this, attacked_by))
            {
                CW_Actor.set_attackTarget(this, attacked_by);
            }
        }
        public void start_color_effect(string type, float time)
        {
            if (!this.stats.effectDamage) return;
            Material material = Content.W_Content_Helper.get_color_material(type);
            if (material == null) return;
            set_colorEffect(this, time);
            set_colorMaterial(this, material);
            func_updateColorEffect(this, 0);
        }
        /// <summary>
        /// 是否拥有修炼体系
        /// </summary>
        /// <param name="cultisys_id">需要判断的修炼体系id</param>
        /// <returns></returns>
        public bool has_cultisys(string cultisys_id)
        {
            return (this.cw_data.cultisys & CW_Library_Manager.instance.cultisys.get(cultisys_id)._tag) > 0;
        }
        /// <summary>
        /// 强制移除人物状态，会触发状态结束函数
        /// </summary>
        /// <param name="status_effect_id">状态id</param>
        public void remove_status_effect_forcely(string status_effect_id)
        {
            if (status_effects == null || !status_effects.ContainsKey(status_effect_id)) return;
            CW_StatusEffectData status_to_remove = status_effects[status_effect_id];
            status_effects.Remove(status_effect_id);
            if (status_to_remove.status_asset.action_on_end != null) status_to_remove.status_asset.action_on_end(status_to_remove, this);
            status_to_remove.force_finish();
            this.setStatsDirty();
        }
        /// <summary>
        /// 添加人物状态
        /// </summary>
        /// <param name="status_effect_id">在状态库中查询的模板id</param>
        /// <param name="as_id">添加入人物状态列表的id，默认为模板id</param>
        /// <param name="user">导致添加状态的对象</param>
        /// <returns></returns>
        public CW_StatusEffectData add_status_effect(string status_effect_id, string as_id = null, BaseSimObject user = null)
        {
            if (status_effects == null) status_effects = new Dictionary<string, CW_StatusEffectData>();
            as_id = string.IsNullOrEmpty(as_id) ? status_effect_id : as_id;
            if (status_effects.ContainsKey(as_id)) return status_effects[as_id];

            if (this.haveTrait("asylum") && CW_Library_Manager.instance.status_effects.get(status_effect_id).has_tag(CW_StatusEffect_Tag.BOUND)) return null;

            foreach(CW_StatusEffectData status_effect in status_effects.Values)
            {
                if (status_effect.status_asset.opposite_status != null && status_effect.status_asset.opposite_status.Contains(as_id)) return null;
            }
            CW_StatusEffectData ret = new CW_StatusEffectData(this, status_effect_id, user);
            if (ret.finished) return null;
            ret.id = as_id;
            status_effects.Add(as_id, ret);
            if (ret.status_asset.action_on_get != null) ret.status_asset.action_on_get(ret, this);
            this.setStatsDirty();
            return ret;
        }
        /// <summary>
        /// 清除默认法术释放计时器
        /// </summary>
        public void clear_default_spell_timer()
        {
            this.default_spell_timer = 0;
        }
        internal void update_status_effects(float elapsed)
        {
            if (this.status_effects == null || this.status_effects.Count==0) return;
            __status_effects_to_remove.Clear();
            __status_effects_to_update.Clear();
            string last_effect_id = "null";
            try
            {
                __status_effects_to_update.AddRange(this.status_effects.Values);
                foreach (CW_StatusEffectData status_effect in __status_effects_to_update)
                {
                    last_effect_id = status_effect.id;
                    status_effect.update(elapsed, this);
                    
                    if (status_effect.finished)
                    {
                        __status_effects_to_remove.Add(status_effect.id);
                        
                    }
                }
            }catch(InvalidOperationException e)
            {
                MonoBehaviour.print(string.Format("Last effect '{0}' cause dict modified", last_effect_id));
            }
            if (__status_effects_to_remove.Count > 0)
            {
                foreach (string status_effect_to_remove in __status_effects_to_remove)
                {
                    status_effects.Remove(status_effect_to_remove);
                }
                if (status_effects.Count == 0) status_effects = null;
                setStatsDirty();
            }
            if (!this.can_act)
            {
                this.stopMovement();
            }
        }
        
        internal bool has_cultisys(uint cultisys_tag)
        {
            return (this.cw_data.cultisys & cultisys_tag) > 0;
        }
        internal void add_child(ActorStatus orgin_status)
        {
            //if (this.cw_data.children_info == null) this.cw_data.children_info = new List<CW_Family_Member_Info>();
            //this.cw_data.children_info.Add(new CW_Family_Member_Info(orgin_status));
            this.fast_data.children++;
        }
        public void get_hit(float damage, bool flash = true, Others.CW_Enums.CW_AttackType type = Others.CW_Enums.CW_AttackType.Other, BaseSimObject attacker = null, bool skip_if_shake = true)
        {
            func_getHit(this, damage, flash, (AttackType)type, attacker, skip_if_shake);
        }
        internal static int get_hit_spell_times = 0;
        
        internal bool __get_hit(float damage, Others.CW_Enums.CW_AttackType attack_type, BaseSimObject attacker, bool pSkipIfShake)
        {
            if ((pSkipIfShake && this.fast_shake_timer.isActive) || this.fast_data.health <= 0 || this.haveTrait("asylum") || this==attacker) return false;

            get_hit_spell_times++;

            this.__battle_timer = Others.CW_Constants.battle_timer;

            float damage_reduce = 0;
            // 区分法抗和物抗作用
            if(attack_type == Others.CW_Enums.CW_AttackType.Spell || attack_type == Others.CW_Enums.CW_AttackType.Status_Spell)
            {
                damage_reduce = this.cw_cur_stats.spell_armor / (100f + this.cw_cur_stats.spell_armor);
            }
            else if (attack_type != Others.CW_Enums.CW_AttackType.God && attack_type!= Others.CW_Enums.CW_AttackType.Status_God)
            {
                damage_reduce = this.cw_cur_stats.base_stats.armor / (100f + this.cw_cur_stats.base_stats.armor);
            }
            if (damage_reduce < 0) damage_reduce = 0;
            damage *= 1 - damage_reduce;

            if ((int)damage <= 0) return false; 
            // 反伤
            if(damage > 0 && attack_type != Others.CW_Enums.CW_AttackType.Spell && attack_type != Others.CW_Enums.CW_AttackType.God && attack_type != Others.CW_Enums.CW_AttackType.Status_God)
            {
                if(damage * this.cw_cur_stats.anti_injury > 1f && attacker!=null && attacker!=this)
                {
                    Utils.CW_SpellHelper.cause_damage_to_target(this, attacker, damage * this.cw_cur_stats.anti_injury);
                }
            }

            // 释放法术
            if (this.cur_spells.Count > 0&&attack_type != Others.CW_Enums.CW_AttackType.Status_God && attack_type != Others.CW_Enums.CW_AttackType.Status_Spell )
            {
                CW_Asset_Spell spell = CW_Library_Manager.instance.spells.get(this.cur_spells.GetRandom());
                if(spell.triger_type == CW_Spell_Triger_Type.DEFEND)
                {// TODO: 可能需要增加对自身位置的参数选择
                    CW_Spell.cast(spell, this, attacker, attacker==null?null:attacker.currentTile);
                }
                else if(spell.triger_type == CW_Spell_Triger_Type.ATTACK &&attacker!=null && get_hit_spell_times%5<4     &&Toolbox.randomChance(0.2f))
                {
                    //CW_Spell.cast(spell, this, attacker, attacker.currentTile);
                }
            }
            if(this.status_effects!=null && this.status_effects.Count > 0 && attack_type != Others.CW_Enums.CW_AttackType.Status_God && attack_type !=Others.CW_Enums.CW_AttackType.Status_Spell)
            {
                foreach(CW_StatusEffectData status_effect in this.status_effects.Values)
                {
                    if (status_effect.status_asset.action_on_hit != null)
                    {
                        status_effect.status_asset.action_on_hit(status_effect, this, attacker);
                    }
                }
            }

            if (this.cw_status.shield > 0 && attack_type!=Others.CW_Enums.CW_AttackType.God && attack_type != Others.CW_Enums.CW_AttackType.Status_God)
            {
                if(this.cw_status.wakan_level > 1)
                {
                    float damage_on_shield = Utils.CW_Utils_Others.compress_raw_wakan(damage, this.cw_status.wakan_level);
                    if ((int)damage_on_shield == 0) return false;
                    if (this.cw_status.shield > damage_on_shield)
                    {
                        this.cw_status.shield -= damage_on_shield; return false;
                    }
                    else
                    {
                        this.cw_status.shield = 0; damage -= Utils.CW_Utils_Others.get_raw_wakan(this.cw_status.shield, this.cw_status.wakan_level);
                    }
                }
                else
                {
                    if (this.cw_status.shield > damage)
                    {
                        this.cw_status.shield -= damage; return false;
                    }
                    else
                    {
                        this.cw_status.shield = 0; damage -= this.cw_status.shield;
                    }
                }
            }
            if (this.cw_status.health_level > 1 && attack_type!=Others.CW_Enums.CW_AttackType.God && attack_type != Others.CW_Enums.CW_AttackType.Status_God) damage = Utils.CW_Utils_Others.compress_raw_wakan(damage, this.cw_status.health_level);
            if ((int)damage == 0) return false;
            if(attacker != null && attacker.objectType == MapObjectType.Actor)
            {
                ((CW_Actor)attacker).regen_health(damage * ((CW_Actor)attacker).cw_cur_stats.vampire, this.cw_status.health_level);
            }

            this.fast_data.health -= (int)damage;
            if (this.fast_data.health < 0)
            {
                this.fast_data.health = 0;
            }
            return true;
        }

        internal void prepare_cw_data_for_save()
        {
            this.cw_data.cultisys_to_save = (int)this.cw_data.cultisys;
            //throw new NotImplementedException();
        }

        static int max_wakan_get_once = 128;
        static int[] wakan_get_count = new int[max_wakan_get_once];
        static bool count_init = false;
        /// <summary>
        /// 获取人物的永久加成属性
        /// </summary>
        /// <returns></returns>
        public CW_BaseStats get_fixed_base_stats()
        {
            if (this.cw_data.fixed_base_stats == null) this.cw_data.fixed_base_stats = new CW_BaseStats();
            return this.cw_data.fixed_base_stats;
        }
        internal void updateStatus_month()
        {
            if (!count_init)
            {
                count_init = true;
                for (int i = 0; i < max_wakan_get_once; i++) wakan_get_count[i] = 0;
            }
            if(this.cw_status.shield < this.cw_cur_stats.shield)
            {
                this.cw_status.shield += Mathf.Min(this.cw_cur_stats.shield_regen, this.cw_cur_stats.shield - this.cw_status.shield);
            }
            if (this.cw_status.soul < this.cw_cur_stats.soul)
            {
                this.cw_status.soul += Mathf.Min(this.cw_cur_stats.soul_regen, this.cw_cur_stats.soul - this.cw_status.soul);
            }
            if (this.cw_status.can_culti && this.cw_status.wakan < this.cw_cur_stats.wakan)
            {
                float wakan_get = 0; CW_MapChunk chunk = this.currentTile.get_cw_chunk();
                float chunk_co = chunk.wakan_level * chunk.wakan_level * this.cw_status.wakan_level * this.cw_status.wakan_level;
                // 计算人物应得的level 1灵气量
                // 修炼获取
                wakan_get += this.cw_data.status.culti_velo * chunk_co * Others.CW_Constants.global_immortal_culti_velo;

                if (this.cw_status.wakan * 100  < this.cw_cur_stats.wakan * Others.CW_Constants.wakan_regen_valid_percent)
                {// 灵气恢复属性的加成
                    wakan_get += Mathf.Min(Utils.CW_Utils_Others.get_raw_wakan(Mathf.Max(0,this.cw_cur_stats.wakan * Others.CW_Constants.wakan_regen_valid_percent/100 - this.cw_status.wakan), this.cw_status.wakan_level), this.cw_cur_stats.wakan_regen * chunk_co);
                }
                if (wakan_get <= 0) goto OUT_WAKAN;
                
                // 计算区块能够提供的level 1灵气量
                float wakan_chunk_provide = Utils.CW_Utils_Others.get_raw_wakan(chunk.wakan, chunk.wakan_level);
                // 取较小者
                if (wakan_get > wakan_chunk_provide) wakan_get = wakan_chunk_provide;
                // 计算实际应用于人物灵气等级的灵气量
                float wakan_actor_get = Utils.CW_Utils_Others.compress_raw_wakan(wakan_get, this.cw_status.wakan_level);
                // 防止溢出
                if (wakan_actor_get > this.cw_cur_stats.wakan - this.cw_status.wakan) wakan_actor_get = this.cw_cur_stats.wakan - this.cw_status.wakan;
                // 人物实际获取灵气
                this.cw_status.wakan += wakan_actor_get;
                // MonoBehaviour.print("Get wakan:" + wakan_actor_get);
                // 取人物实际获取的灵气量转为level 1的灵气量
                wakan_actor_get = Utils.CW_Utils_Others.get_raw_wakan(wakan_actor_get, this.cw_status.wakan_level);
                // 取人物获取量与应得量较小者
                wakan_get = (int)Mathf.Min(wakan_actor_get, wakan_get);
                // 从区块移除对应量原始灵气
                chunk.wakan -= Utils.CW_Utils_Others.compress_raw_wakan(wakan_get, chunk.wakan_level);
            }
            OUT_WAKAN:
            float health_to_regen;
            if((this.cw_data.cultisys & Others.CW_Constants.cultisys_bushido_tag) != 0)
            {
                health_to_regen = 
                    this.cw_cur_stats.base_stats.health * Others.CW_Constants.health_regen_valid_percent / 100 > this.fast_data.health ? 
                    (this.cw_status.health_level>1?Mathf.Max(Utils.CW_Utils_Others.compress_raw_wakan(this.cw_cur_stats.health_regen, this.cw_status.health_level),1):this.cw_cur_stats.health_regen)
                    :0;
            }
            else
            {
                health_to_regen = Mathf.Min(this.cw_cur_stats.health_regen, this.cw_cur_stats.base_stats.health - this.fast_data.health);
            }
            this.fast_data.health += (int)health_to_regen;
            
        }
        /// <summary>
        /// 检查各个修炼体系能否晋级
        /// </summary>
        public void check_level_up()
        {
            uint cultisys = this.cw_data.cultisys;
            int cultisys_tag = 0;
            int max_cultisys_tag = -1;
            int max_level=-1;
            uint level_up_tag = 0;
            while (cultisys > 0)
            {
                if (((cultisys & 0x1) == 1) && (CW_Library_Manager.instance.cultisys.list[cultisys_tag].level_judge(this, CW_Library_Manager.instance.cultisys.list[cultisys_tag])))
                {
                    this.cw_data.cultisys_level[cultisys_tag]++;
                    this.setStatsDirty();
                    level_up_tag |= (uint)(1 << cultisys_tag);
                }
                if(this.cw_data.cultisys_level[cultisys_tag] > max_level)
                {
                    max_level = this.cw_data.cultisys_level[cultisys_tag];
                    max_cultisys_tag = cultisys_tag;
                }
                cultisys_tag++;
                cultisys >>= 1;
            }
            level_up_bonus(level_up_tag, max_cultisys_tag, max_level);
        }
        /// <summary>
        /// 检查给定一系列修炼体系能否晋级
        /// </summary>
        /// <param name="cultisys"></param>
        public void check_level_up(uint cultisys)
        {
            int cultisys_tag = 0;
            int max_cultisys_tag = -1;
            int max_level = -1;
            uint level_up_tag = 0;
            while (cultisys > 0)
            {
                if (((cultisys & 0x1) == 1) && (CW_Library_Manager.instance.cultisys.list[cultisys_tag].level_judge(this, CW_Library_Manager.instance.cultisys.list[cultisys_tag])))
                {
                    this.cw_data.cultisys_level[cultisys_tag]++;
                    this.setStatsDirty();
                    level_up_tag |= (uint)(1 << cultisys_tag);
                }
                if (this.cw_data.cultisys_level[cultisys_tag] > max_level)
                {
                    max_level = this.cw_data.cultisys_level[cultisys_tag];
                    max_cultisys_tag = cultisys_tag;
                }
                cultisys_tag++;
                cultisys >>= 1;
            }
            level_up_bonus(level_up_tag, max_cultisys_tag, max_level);
        }
        internal void level_up_bonus(uint level_up_tag, int max_cultisys_tag, int max_level)
        {
            if (level_up_tag != 0)
            {
                this.level_up_learn_spell(level_up_tag);
            }
            if ((level_up_tag & (1 << max_cultisys_tag)) != 0 && max_level % Others.CW_Constants.cultibook_levelup_require == Others.CW_Constants.cultibook_levelup_require - 1)
            {
                this.level_up_cm_cultibook(max_level);
            }
            if (level_up_tag != 0 && max_level == Others.CW_Constants.special_body_create_level && (string.IsNullOrEmpty(this.cw_data.special_body_id) || this.fast_data.level > CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id).level))
            {
                this.level_up_create_body();
            }
        }
        private void level_up_create_body()
        {
            CW_Asset_SpecialBody new_body = new CW_Asset_SpecialBody(this);
            new_body.store();
            this.change_special_body(new_body);
        }
        private void level_up_cm_cultibook(int max_level)
        {
            if (string.IsNullOrEmpty(this.cw_data.cultibook_id))
            {
                this.create_cultibook();
            }
            else if (max_level + 1 > (CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id).order + 1) * Others.CW_Constants.cultibook_levelup_require)
            {
                this.modify_cultibook();
            }
        }
        private void level_up_learn_spell(uint level_up_tag)
        {
            List<CW_Asset_Spell> spells = CW_Library_Manager.instance.spells.search_for_random_learn(CW_Library_Spell.make_tags(CW_Spell_Tag.ATTACK, CW_Spell_Tag.POSITIVE_STATUS, CW_Spell_Tag.DEFEND, CW_Spell_Tag.SUMMON, CW_Spell_Tag.MOVE), Spell_Search_Type.CONTAIN_ANY_TAGS, this);
            CW_Library_Spell.filter_list(spells, CW_Library_Spell.make_tags(level_up_tag), Spell_Search_Type.CONTAIN_ANY_TAGS);
            this.filter_allowed_spells(spells);

            if (spells.Count > 0)
            {
                CW_Asset_Spell spell = spells.GetRandom();
                this.learn_spell(spell);
            }
        }
        public void change_special_body(CW_Asset_SpecialBody target_special_body)
        {
            if (target_special_body == null) return;
            CW_Asset_SpecialBody sb_asset = CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id);
            if (sb_asset != null)
            {
                sb_asset.cur_own_nr--;
                if (sb_asset.cur_own_nr == 0) sb_asset.try_deprecate();
                else if(string.IsNullOrEmpty(sb_asset.author_name) && sb_asset.author_id == this.fast_data.actorID)
                {
                    sb_asset.get_author_name(this);
                }
            }
            this.cw_data.special_body_id = target_special_body.id;
            if (target_special_body != null)
            {
                target_special_body.cur_own_nr++;
                target_special_body.histroy_own_nr++;
            }
        }
        public void change_special_body(string target_special_body)
        {
            change_special_body(CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id));
        }
        internal void learn_spell(CW_Asset_Spell spell)
        {
            if (this.cw_data.spells.Contains(spell.id)) return;
            if (spell.allow_actor(this)) this.cw_data.spells.Add(spell.id);
        }
        internal void learn_spells(string[] spell_ids)
        {
            if (spell_ids == null) throw new Exception("Null spells array");
            List<CW_Asset_Spell> spells = new List<CW_Asset_Spell>(4);
            foreach(string spell_id in spell_ids)
            {
                if (!string.IsNullOrEmpty(spell_id)) spells.Add(CW_Library_Manager.instance.spells.get(spell_id));
            }
            this.learn_spells(spells);
        }
        public void learn_spells(List<CW_Asset_Spell> spells)
        {
            this.filter_allowed_spells(spells);
            foreach(CW_Asset_Spell spell in spells) this.learn_spell(spell);
        }
        internal void filter_allowed_spells(List<CW_Asset_Spell> spells)
        {
            int first = spells.Count;
            CW_Library_Spell.filter_list(spells, CW_Library_Spell.make_tags(this.cw_data.cultisys), Spell_Search_Type.CONTAIN_ANY_TAGS);
            int second = spells.Count;
            CW_Library_Spell.filter_list_by_element(spells, CW_Library_Spell.make_tags(this.cw_data.element));
            int last = spells.Count;
            //if (last == 0) print(string.Format("No found spells in tags {0} and {1}, before filtering by cultisys:{2}, before filtering by elements:{3}", Convert.ToString((long)CW_Library_Spell.make_tags(this.cw_data.cultisys),16), Convert.ToString((long)CW_Library_Spell.make_tags(this.cw_data.element), 16), first,second));
        }
        public void learn_cultibook(string cultibook_id) 
        {
            learn_cultibook(CW_Library_Manager.instance.cultibooks.get(cultibook_id));
        }
        internal void learn_cultibook(CW_Asset_CultiBook cultibook) 
        {
            if (cultibook == null) throw new Exception("Try to learn 'null' cultibook");
            if (!Others.CW_Constants.cultibook_force_learn && this.cw_data.cultisys == 0) return;

            cultibook.cur_culti_nr++;
            cultibook.histroy_culti_nr++;
            CW_Asset_CultiBook prev_cultibook = CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id);
            if (prev_cultibook != null)
            {
                prev_cultibook.cur_culti_nr--;
                if (prev_cultibook.cur_culti_nr <= 0) prev_cultibook.try_deprecate();
            }

            this.cw_data.cultibook_id = cultibook.id;
            this.learn_spells(cultibook.spells);
        }
        private void __force_learn_cultibook(string cultibook_id)
        {
            CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(cultibook_id);
            if (cultibook == null) throw new Exception("Try to learn 'null' cultibook");

            cultibook.cur_culti_nr++;
            cultibook.histroy_culti_nr++;
            CW_Asset_CultiBook prev_cultibook = CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id);
            if (prev_cultibook != null)
            {
                prev_cultibook.cur_culti_nr--;
                if (prev_cultibook.cur_culti_nr <= 0) prev_cultibook.try_deprecate();
            }

            this.cw_data.cultibook_id = cultibook.id;
            this.learn_spells(cultibook.spells);
        }
        private int gen_cultibook_level()
        {
            int level; float continue_chance = (1f + this.fast_data.intelligence) / (4f + this.fast_data.intelligence);
            for (level = 0; level < Others.CW_Constants.cultibook_max_level-1; level++)
            {
                if (!Toolbox.randomChance(continue_chance)) break;
            }
            return level;
        }
        private void __modify_cultibook_last_step(CW_Asset_CultiBook culti_book)
        {
            culti_book.level = gen_cultibook_level();
            culti_book.store();
            culti_book.gen_bonus_stats(this);
            int i; int len_1 = this.cw_data.spells.Count; int j;
            int len_2 = 0;
            for (i = 0; i < Others.CW_Constants.cultibook_spell_limit; i++)
            {
                if (culti_book.spells[i] == null) { len_2 = i; break; }
            }
            for (i = 0; i < len_1; i++)
            {
                for (j = 0; j < len_2; j++)
                {
                    if (this.cw_data.spells[i] == culti_book.spells[j]) break;
                }
                if (j == len_2)
                {
                    culti_book.spells[j] = this.cw_data.spells[i];
                    break;
                }
            }
        }
        private void modify_cultibook()
        {
            CW_Asset_CultiBook prev_cultibook = CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id);
            CW_Asset_CultiBook culti_book = prev_cultibook.deepcopy();
            if (culti_book == null) throw new Exception("No found cultibook '" + this.cw_data.cultibook_id + "'");
            culti_book.order++;
            culti_book.re_author(this);
            __modify_cultibook_last_step(culti_book);
            if (prev_cultibook.author_id == this.fast_data.actorID && prev_cultibook.cur_culti_nr > 1 && string.IsNullOrEmpty(prev_cultibook.author_name)) prev_cultibook.get_author_name(this); 
            this.learn_cultibook(culti_book);
        }

        private void create_cultibook()
        {
            CW_Asset_CultiBook culti_book = new CW_Asset_CultiBook(this);
            __modify_cultibook_last_step(culti_book);
            this.learn_cultibook(culti_book);
        }
        public void regen_health(float health, float health_level = 1)
        {
            float health_to_regen = Utils.CW_Utils_Others.transform_wakan(health, health_level, this.cw_status.health_level);
            this.fast_data.health += (int)health_to_regen;
            if (this.fast_data.health > this.cw_cur_stats.base_stats.health) this.fast_data.health = this.cw_cur_stats.base_stats.health;
            check_level_up();
        }
        public void regen_wakan(float wakan, float wakan_level = 1)
        {
            float wakan_to_regen = Utils.CW_Utils_Others.transform_wakan(wakan, wakan_level, this.cw_status.wakan_level);
            this.cw_status.wakan += wakan_to_regen;
            if (this.cw_status.wakan > this.cw_cur_stats.wakan) this.cw_status.wakan = this.cw_cur_stats.wakan;
            check_level_up();
        }
        public void regen_shield(float shield, float wakan_level = 1)
        {
            float shield_to_regen = Utils.CW_Utils_Others.transform_wakan(shield, wakan_level, this.cw_status.wakan_level);
            this.cw_status.shield += shield_to_regen;
            if (this.cw_status.shield > this.cw_cur_stats.shield) this.cw_status.shield = this.cw_cur_stats.shield;
            check_level_up();
        }
        public CW_Asset_Item get_weapon_asset()
        {
            if(this.stats.use_items && !this.equipment.weapon.isEmpty())
            {
                return CW_Library_Manager.instance.items.get(this.equipment.weapon.data.id);
            }
            return CW_Library_Manager.instance.items.get(this.stats.defaultAttack);
        }
        internal void CW_newCreature()
        {
            this.fast_data = null;
            this.cur_spells = new List<string>();
            this.cw_cur_stats = new CW_BaseStats(CW_Actor.get_curstats(this));
            this.cw_data = new CW_ActorData();
            this.cw_status = new CW_ActorStatus();
            

            this.cw_data.status = this.cw_status;

            this.cw_data.cultisys_level = new int[CW_Library_Manager.instance.cultisys.list.Count];
            this.cw_data.element = new CW_Element(prefer_elements: this.cw_stats.prefer_element, prefer_scale: this.cw_stats.prefer_element_scale);
            this.cw_data.spells = new List<string>();

            this.cw_status.can_culti = false;
            this.cw_status.culti_velo = this.cw_stats.culti_velo;
            this.cw_status.shield = 0;
            this.cw_status.wakan = 0;
            this.cw_status.wakan_level = 1;
            this.cw_status.health_level = 1;
            this.cw_status.max_age = this.cw_stats.origin_stats.maxAge;

            //CW_Library_Manager.instance.cultisys.set_cultisys(cw_data, this.stats.id);
        }
        internal static CW_ActorData procrete(CW_Actor main_parent, CW_Actor second_parent)
        {
            CW_ActorData cw_actor_data = new CW_ActorData();
            CW_ActorStatus cw_actor_status = new CW_ActorStatus();

            cw_actor_data.cultisys_level = new int[CW_Library_Manager.instance.cultisys.list.Count];

            cw_actor_data.element = CW_Element.get_middle(main_parent.cw_data.element, second_parent.cw_data.element);
            if (Main.instance.world_config.random_element_when_procrete && Toolbox.randomChance(Main.instance.world_config.chance_random_element_when_procrete)) cw_actor_data.element.re_random();

            // TODO: New family
            cw_actor_data.family_id = main_parent.cw_data.family_id;
            cw_actor_data.family_name = main_parent.cw_data.family_name;
            // TODO: Select Pope or wait for being selected
            cw_actor_data.pope_id = null;

            cw_actor_data.special_body_id = CW_Library_Manager.instance.special_bodies.select_better(main_parent.cw_data.special_body_id, second_parent.cw_data.special_body_id);
            CW_Asset_SpecialBody body = CW_Library_Manager.instance.special_bodies.get(cw_actor_data.special_body_id);
            if (body != null)
            {
                body.cur_own_nr++;
                body.histroy_own_nr++;
            }

            cw_actor_data.spells = new List<string>();

            cw_actor_data.status = cw_actor_status;
            cw_actor_status.can_culti = false;
            cw_actor_status.culti_velo = 1f;
            cw_actor_status.max_age = main_parent.stats.maxAge;
            cw_actor_status.shield = 0;
            cw_actor_status.wakan = 0;
            cw_actor_status.wakan_level = 1;
            cw_actor_status.health_level = 1;

            //CW_Library_Manager.instance.cultisys.set_cultisys(cw_actor_data, main_parent.stats.id);
            cw_actor_data.pre_learn_cultibook(CW_Library_Manager.instance.cultibooks.get(CW_Library_Manager.instance.cultibooks.select_better(main_parent.cw_data.cultibook_id, second_parent.cw_data.cultibook_id)));
            return cw_actor_data;
        }
    }
}
