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
        public List<CW_Actor> compose_actors = null;
        public List<CW_Building> compose_buildings = null;
        public Dictionary<string, CW_StatusEffectData> status_effects = null;
        public List<string> cur_spells = null;
        internal WorldTimer fast_shake_timer = null;
        public bool can_act = true;
        internal float m_attackSpeed_seconds = 0;
        private static List<string> _status_effects_to_remove = new List<string>();
        /// <summary>
        /// 仅提供高效访问，待权限开放后删除
        /// </summary>
        public ActorStatus fast_data = null;
        #region Getter
        public static Func<Actor, ActorStatus> get_data = CW_ReflectionHelper.create_getter<Actor, ActorStatus>("data");
        public static Func<Actor, BaseStats> get_curstats = CW_ReflectionHelper.create_getter<Actor, BaseStats>("curStats");
        public static Func<Actor, BaseSimObject> get_attackedBy = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("attackedBy");
        public static Func<Actor, BaseSimObject> get_attackTarget = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("attackTarget");
        public static Func<Actor, float> get_attackTimer = CW_ReflectionHelper.create_getter<Actor, float>("attackTimer");
        public static Func<Actor, Dictionary<string, StatusEffectData>> get_activeStatus_dict = CW_ReflectionHelper.create_getter<Actor, Dictionary<string, StatusEffectData>>("activeStatus_dict");
        public static Func<Actor, PersonalityAsset> get_s_personality = CW_ReflectionHelper.create_getter<Actor, PersonalityAsset>("s_personality");
        public static Func<Actor, bool> get_event_full_heal = CW_ReflectionHelper.create_getter<Actor, bool>("event_full_heal");
        public static Func<Actor, List<ActorTrait>> get_s_special_effect_traits = CW_ReflectionHelper.create_getter<Actor, List<ActorTrait>>("s_special_effect_traits");
        internal static Func<Actor, WorldTimer> get_shake_timer = CW_ReflectionHelper.create_getter<Actor, WorldTimer>("shakeTimer");
        internal static Func<Actor, BaseSimObject> get_beh_actor_target = CW_ReflectionHelper.create_getter<Actor, BaseSimObject>("beh_actor_target");
        #endregion
        #region Setter
        public static Action<Actor, bool> set_statsDirty = CW_ReflectionHelper.create_setter<Actor, bool>("statsDirty");
        public static Action<Actor, bool> set_event_full_heal = CW_ReflectionHelper.create_setter<Actor, bool>("event_full_heal");
        public static Action<Actor, bool> set_item_sprite_dirty = CW_ReflectionHelper.create_setter<Actor, bool>("item_sprite_dirty");
        public static Action<Actor, bool> set_trait_weightless = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_weightless");
        public static Action<Actor, bool> set_trait_peaceful = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_peaceful");
        public static Action<Actor, bool> set_trait_fire_resistant = CW_ReflectionHelper.create_setter<Actor, bool>("_trait_fire_resistant");
        public static Action<Actor, bool> set_status_frozen = CW_ReflectionHelper.create_setter<Actor, bool>("_status_frozen");
        public static Action<Actor, float> set_attackTimer = CW_ReflectionHelper.create_setter<Actor, float>("attackTimer");
        public static Action<Actor, float> set_s_attackSpeed_seconds = CW_ReflectionHelper.create_setter<Actor, float>("s_attackSpeed_seconds");
        public static Action<Actor, WeaponType> set_s_attackType = CW_ReflectionHelper.create_setter<Actor, WeaponType>("s_attackType");
        public static Action<Actor, string> set_s_slashType = CW_ReflectionHelper.create_setter<Actor, string>("s_slashType");
        public static Action<Actor, string> set_s_weapon_texture = CW_ReflectionHelper.create_setter<Actor, string>("s_weapon_texture");
        public static Action<Actor, PersonalityAsset> set_s_personality = CW_ReflectionHelper.create_setter<Actor, PersonalityAsset>("s_personality");
        public static Action<Actor, ProfessionAsset> set_professionAsset = CW_ReflectionHelper.create_setter<Actor, ProfessionAsset>("professionAsset");
        public static Action<Actor, ActorStatus> set_data = CW_ReflectionHelper.create_setter<Actor, ActorStatus>("data");
        public static Action<Actor, List<ActorTrait>> set_s_special_effect_traits = CW_ReflectionHelper.create_setter<Actor, List<ActorTrait>>("s_special_effect_traits");
        public static Action<Actor, BaseSimObject> set_attackedBy = CW_ReflectionHelper.create_setter<Actor, BaseSimObject>("attackedBy");
        public static Action<Actor, BaseSimObject> set_attackTarget = CW_ReflectionHelper.create_setter<Actor, BaseSimObject>("attackTarget");
        public static Action<Actor, float> set_colorEffect = CW_ReflectionHelper.create_setter<Actor, float>("colorEffect");
        public static Action<Actor, Material> set_colorMaterial = CW_ReflectionHelper.create_setter<Actor, Material>("colorMaterial");
        #endregion
        #region Func
        public static Action<Actor> func_updateTargetScale = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("updateTargetScale");
        public static Action<Actor> func_findHeadSprite = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("findHeadSprite");
        public static Action<Actor, bool> func_checkMadness = (Action<Actor, bool>)CW_ReflectionHelper.get_method<Actor>("checkMadness");
        public static Func<Actor, string, bool> func_haveStatus = (Func<Actor, string, bool>)CW_ReflectionHelper.get_method<Actor>("haveStatus");
        public static Action<Actor, int> func_newCreature = (Action<Actor, int>)CW_ReflectionHelper.get_method<Actor>("newCreature");
        public static Action<Actor, Kingdom> func_setKingdom = (Action<Actor, Kingdom>)CW_ReflectionHelper.get_method<Actor>("setKingdom");
        public static Action<Actor, WorldTile, float> func_spawnOn = (Action<Actor, WorldTile, float>)CW_ReflectionHelper.get_method<Actor>("spawnOn");
        public static Action<Actor> func_create = (Action<Actor>)CW_ReflectionHelper.get_method<Actor>("create");
        internal static Action<Actor, float, bool, AttackType, BaseSimObject, bool> func_getHit = (Action<Actor, float, bool, AttackType, BaseSimObject, bool>)CW_ReflectionHelper.get_method<Actor>("getHit");
        public static Action<Actor, float> func_updateColorEffect = (Action<Actor, float>)CW_ReflectionHelper.get_method<Actor>("updateColorEffect");
        #endregion
        public void start_color_effect(string type, float time)
        {
            if (!this.stats.effectDamage) return;
            Material material = Content.W_Content_Helper.get_color_material(type);
            if (material == null) return;
            set_colorEffect(this, time);
            set_colorMaterial(this, material);
            func_updateColorEffect(this, 0);
        }
        public bool has_cultisys(string cultisys_id)
        {
            return (this.cw_data.cultisys & CW_Library_Manager.instance.cultisys.get(cultisys_id)._tag) > 0;
        }
        public void remove_status_effect_forcely(string status_effect_id)
        {
            if (status_effects == null || !status_effects.ContainsKey(status_effect_id)) return;
            CW_StatusEffectData status_to_remove = status_effects[status_effect_id];
            status_effects.Remove(status_effect_id);
            if (status_to_remove.status_asset.action_on_end != null) status_to_remove.status_asset.action_on_end(status_to_remove, this);
            status_to_remove.force_finish();
            this.setStatsDirty();
        }
        public CW_StatusEffectData add_status_effect(string status_effect_id, string as_id = null, BaseSimObject user = null)
        {
            if (status_effects == null) status_effects = new Dictionary<string, CW_StatusEffectData>();
            as_id = string.IsNullOrEmpty(as_id) ? status_effect_id : as_id;
            if (status_effects.ContainsKey(as_id)) return status_effects[as_id];
            foreach(CW_StatusEffectData status_effect in status_effects.Values)
            {
                if (status_effect.status_asset.opposite_status != null && status_effect.status_asset.opposite_status.Contains(as_id)) return null;
            }
            CW_StatusEffectData ret = new CW_StatusEffectData(this, status_effect_id, user);
            ret.id = as_id;
            status_effects.Add(as_id, ret);
            if (ret.status_asset.action_on_get != null) ret.status_asset.action_on_get(ret, this);
            this.setStatsDirty();
            return ret;
        }
        internal void update_status_effects(float elapsed)
        {
            if (this.status_effects == null || this.status_effects.Count==0) return;
            _status_effects_to_remove.Clear();
            string last_effect_id = "null";
            try
            {
                foreach (CW_StatusEffectData status_effect in this.status_effects.Values)
                {
                    last_effect_id = status_effect.id;
                    status_effect.update(elapsed, this);
                    
                    if (status_effect.finished)
                    {
                        _status_effects_to_remove.Add(status_effect.status_asset.id);
                        
                    }
                }
            }catch(InvalidOperationException e)
            {
                MonoBehaviour.print(string.Format("Last effect '{0}' cause dict modified", last_effect_id));
            }
            if (_status_effects_to_remove.Count > 0)
            {
                foreach (string status_effect_to_remove in _status_effects_to_remove)
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
        public void add_child(ActorStatus orgin_status)
        {
            if (this.cw_data.children_info == null) this.cw_data.children_info = new List<CW_Family_Member_Info>();
            this.cw_data.children_info.Add(new CW_Family_Member_Info(orgin_status));
            this.fast_data.children++;
        }
        public void get_hit(float damage, bool flash = true, Others.CW_Enums.CW_AttackType type = Others.CW_Enums.CW_AttackType.Other, BaseSimObject attacker = null, bool skip_if_shake = true)
        {
            func_getHit(this, damage, flash, (AttackType)type, attacker, skip_if_shake);
        }
        internal bool __get_hit(float damage, Others.CW_Enums.CW_AttackType attack_type, BaseSimObject attacker, bool pSkipIfShake)
        {
            if ((pSkipIfShake && this.fast_shake_timer.isActive) || this.fast_data.health <= 0) return false;

            float damage_reduce = 0;
            // 区分法抗和物抗作用
            if(attack_type == Others.CW_Enums.CW_AttackType.Spell || attack_type == Others.CW_Enums.CW_AttackType.Status_Spell)
            {
                damage_reduce = this.cw_cur_stats.base_stats.armor / (100 + this.cw_cur_stats.spell_armor);
            }
            else if (attack_type != Others.CW_Enums.CW_AttackType.God && attack_type!= Others.CW_Enums.CW_AttackType.Status_God)
            {
                damage_reduce = this.cw_cur_stats.base_stats.armor / (100 + this.cw_cur_stats.base_stats.armor);
            }
            if (damage_reduce < 0) damage_reduce = 0;
            damage *= 1 - damage_reduce;

            if (damage < 0) return false; 
            // 反伤
            if(damage > 0 && attack_type != Others.CW_Enums.CW_AttackType.Spell && attack_type != Others.CW_Enums.CW_AttackType.God && attack_type != Others.CW_Enums.CW_AttackType.Status_God)
            {
                if(damage * this.cw_cur_stats.anti_injury > 1f && attacker!=null && attacker!=this)
                {
                    Utils.CW_SpellHelper.cause_damage_to_target(this, attacker, damage * this.cw_cur_stats.anti_injury);
                }
            }

            // 释放防御类法术
            if (this.cur_spells.Count > 0 && attack_type != Others.CW_Enums.CW_AttackType.Status_God && attack_type != Others.CW_Enums.CW_AttackType.Status_Spell)
            {
                CW_Asset_Spell spell = CW_Library_Manager.instance.spells.get(this.cur_spells.GetRandom());
                if(spell.triger_type == CW_Spell_Triger_Type.DEFEND)
                {// TODO: 可能需要增加对自身位置的参数选择
                    CW_Spell.cast(spell, this, attacker, attacker==null?null:attacker.currentTile);
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
                    if (this.cw_status.shield > damage_on_shield)
                    {
                        this.cw_status.shield -= (int)damage_on_shield; damage = 0;
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
                        this.cw_status.shield -= (int)damage; damage = 0;
                    }
                    else
                    {
                        this.cw_status.shield = 0; damage -= this.cw_status.shield;
                    }
                }
            }
            if (this.cw_status.health_level > 1 && attack_type!=Others.CW_Enums.CW_AttackType.God && attack_type != Others.CW_Enums.CW_AttackType.Status_God) damage = Utils.CW_Utils_Others.compress_raw_wakan(damage, this.cw_status.health_level);

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
        static int max_wakan_get_once = 128;
        static int[] wakan_get_count = new int[max_wakan_get_once];
        static bool count_init = false;
        public void updateStatus_month()
        {
            if (!count_init)
            {
                count_init = true;
                for (int i = 0; i < max_wakan_get_once; i++) wakan_get_count[i] = 0;
            }
            if(this.cw_status.shield < this.cw_cur_stats.shield)
            {
                this.cw_status.shield += Mathf.Min((int)Utils.CW_Utils_Others.compress_raw_wakan(this.cw_cur_stats.shield_regen, this.cw_status.wakan_level), this.cw_cur_stats.shield - this.cw_status.shield);
            }
            
            if (this.cw_status.can_culti && this.cw_status.wakan < this.cw_cur_stats.wakan)
            {
                int wakan_get = 0; CW_MapChunk chunk = this.currentTile.get_cw_chunk();
                float chunk_co = chunk.wakan_level;
                // 计算人物应得的level 1灵气量
                // 修炼获取
                wakan_get += (int)((1 + this.cw_cur_stats.mod_cultivation / 100) * this.cw_data.status.culti_velo * chunk_co);
                wakan_get_count[wakan_get]++;

                if (this.cw_status.wakan * 100  < this.cw_cur_stats.wakan * Others.CW_Constants.wakan_regen_valid_percent)
                {// 灵气恢复属性的加成
                    wakan_get += (int)(this.cw_cur_stats.wakan_regen * chunk_co);
                }
                if (wakan_get <= 0) goto OUT_WAKAN;
                
                // 计算区块能够提供的level 1灵气量
                float wakan_chunk_provide = Utils.CW_Utils_Others.get_raw_wakan(chunk.wakan, chunk.wakan_level);
                // 取较小者
                if (wakan_get > wakan_chunk_provide) wakan_get = (int)wakan_chunk_provide;
                // 计算实际应用于人物灵气等级的灵气量
                float wakan_actor_get = Utils.CW_Utils_Others.compress_raw_wakan(wakan_get, this.cw_status.wakan_level);
                // 防止溢出
                if (wakan_actor_get > this.cw_cur_stats.wakan - this.cw_status.wakan) wakan_actor_get = this.cw_cur_stats.wakan - this.cw_status.wakan;
                // 人物实际获取灵气
                this.cw_status.wakan += (int)wakan_actor_get;
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
                    (this.cw_status.health_level>1?Utils.CW_Utils_Others.compress_raw_wakan(this.cw_cur_stats.health_regen, this.cw_status.health_level):this.cw_cur_stats.health_regen)
                    :0;
            }
            else
            {
                health_to_regen = this.cw_cur_stats.base_stats.health - this.fast_data.health;
            }
            this.fast_data.health += (int)health_to_regen;
            
        }
        public void checkLevelUp()
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
            if (level_up_tag != 0)
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
            if((level_up_tag & (1<<max_cultisys_tag))!=0 && max_level % Others.CW_Constants.cultibook_levelup_require == Others.CW_Constants.cultibook_levelup_require - 1)
            {
                if (string.IsNullOrEmpty(this.cw_data.cultibook_id))
                {
                    this.create_cultibook();
                }
                else if(max_level+1>(CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id).order +1)*Others.CW_Constants.cultibook_levelup_require)
                {
                    this.modify_cultibook();
                }
            }
            if(level_up_tag!=0 && max_level == Others.CW_Constants.special_body_create_level && (string.IsNullOrEmpty(this.cw_data.special_body_id) || this.fast_data.level > CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id).level))
            {
                CW_Asset_SpecialBody new_body = new CW_Asset_SpecialBody(this);
                new_body.store();
                this.change_special_body(new_body);
            }
        }
        public void change_special_body(CW_Asset_SpecialBody target_special_body)
        {
            CW_Asset_SpecialBody sb_asset = CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id);
            if (sb_asset != null)
            {
                sb_asset.cur_own_nr--;
                if (sb_asset.cur_own_nr == 0) sb_asset.try_deprecate();
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
            CW_Asset_SpecialBody sb_asset = CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id);
            if(sb_asset != null)
            {
                sb_asset.cur_own_nr--;
                if (sb_asset.cur_own_nr == 0) sb_asset.try_deprecate();
            }
            this.cw_data.special_body_id = target_special_body;
            sb_asset = CW_Library_Manager.instance.special_bodies.get(this.cw_data.special_body_id);
            if (sb_asset != null)
            {
                sb_asset.cur_own_nr++;
                sb_asset.histroy_own_nr++;
            }
        }
        internal void learn_spell(CW_Asset_Spell spell)
        {
            if (this.cw_data.spells.Contains(spell.id)) return;
            if (spell.allow_actor(this)) this.cw_data.spells.Add(spell.id);
        }
        public void learn_spells(string[] spell_ids)
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
        public void learn_cultibook(CW_Asset_CultiBook cultibook) 
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
            for (level = 0; level < Others.CW_Constants.cultibook_max_level; level++)
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
            CW_Asset_CultiBook culti_book = CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id).deepcopy();
            if (culti_book == null) throw new Exception("No found cultibook '" + this.cw_data.cultibook_id + "'");
            culti_book.order++;
            culti_book.re_author(this);
            __modify_cultibook_last_step(culti_book);
            this.learn_cultibook(culti_book);
        }

        private void create_cultibook()
        {
            CW_Asset_CultiBook culti_book = new CW_Asset_CultiBook(this);
            __modify_cultibook_last_step(culti_book);
            this.learn_cultibook(culti_book);
        }
        public void regen_health(float health, float health_level)
        {
            float health_to_regen = Utils.CW_Utils_Others.transform_wakan(health, health_level, this.cw_status.health_level);
            this.fast_data.health += (int)health_to_regen;
            if (this.fast_data.health > this.cw_cur_stats.base_stats.health) this.fast_data.health = this.cw_cur_stats.base_stats.health;
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
