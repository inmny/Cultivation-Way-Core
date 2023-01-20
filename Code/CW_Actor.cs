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
        internal WorldTimer fast_shake_timer = null;
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
        public static Func<Actor, Dictionary<string, StatusEffectData>> get_activeStatus_dict = CW_ReflectionHelper.create_getter<Actor, Dictionary<string, StatusEffectData>>("activeStatus_dict");
        public static Func<Actor, PersonalityAsset> get_s_personality = CW_ReflectionHelper.create_getter<Actor, PersonalityAsset>("s_personality");
        public static Func<Actor, bool> get_event_full_heal = CW_ReflectionHelper.create_getter<Actor, bool>("event_full_heal");
        public static Func<Actor, List<ActorTrait>> get_s_special_effect_traits = CW_ReflectionHelper.create_getter<Actor, List<ActorTrait>>("s_special_effect_traits");
        internal static Func<Actor, WorldTimer> get_shake_timer = CW_ReflectionHelper.create_getter<Actor, WorldTimer>("shakeTimer");
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

        #endregion
        public bool has_cultisys(string cultisys_id)
        {
            return (this.cw_data.cultisys & CW_Library_Manager.instance.cultisys.get(cultisys_id)._tag) > 0;
        }
        public CW_StatusEffectData add_status_effect(string status_effect_id)
        {
            if (status_effects == null) status_effects = new Dictionary<string, CW_StatusEffectData>();
            if (status_effects.ContainsKey(status_effect_id)) return status_effects[status_effect_id];
            CW_StatusEffectData ret = new CW_StatusEffectData(this, status_effect_id);
            status_effects.Add(status_effect_id, ret);
            if (ret.status_asset.action_on_get != null) ret.status_asset.action_on_get(ret, this);
            this.setStatsDirty();
            return ret;
        }
        internal void update_status_effects(float elapsed)
        {
            if (this.status_effects == null || this.status_effects.Count==0) return;
            _status_effects_to_remove.Clear();
            foreach(CW_StatusEffectData status_effect in this.status_effects.Values)
            {
                status_effect.update(elapsed);
                if(status_effect.status_asset.action_on_update!=null) status_effect.status_asset.action_on_update(status_effect, this);
                if (status_effect.finished) _status_effects_to_remove.Add(status_effect.status_asset.id);
            }
            if (_status_effects_to_remove.Count > 0)
            {
                foreach (string status_effect_to_remove in _status_effects_to_remove)
                {
                    status_effects.Remove(status_effect_to_remove);
                }
                setStatsDirty();
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
            if(attack_type == Others.CW_Enums.CW_AttackType.Spell)
            {
                damage_reduce = this.cw_cur_stats.base_stats.armor / (100 + this.cw_cur_stats.spell_armor);
            }
            else if (attack_type != Others.CW_Enums.CW_AttackType.God)
            {
                damage_reduce = this.cw_cur_stats.base_stats.armor / (100 + this.cw_cur_stats.base_stats.armor);
            }
            if (damage_reduce < 0) damage_reduce = 0;
            damage *= 1 - damage_reduce;

            if(damage < 0) damage = 0;
            // 反伤
            if(damage > 0 && attack_type != Others.CW_Enums.CW_AttackType.Spell && attack_type != Others.CW_Enums.CW_AttackType.God)
            {
                if(damage * this.cw_cur_stats.anti_injury > 1f && attacker!=null && attacker!=this)
                {
                    Utils.CW_SpellHelper.cause_damage_to_target(this, attacker, damage * this.cw_cur_stats.anti_injury);
                }
            }
            
            // 释放防御类法术
            if (this.cw_data.spells.Count > 0)
            {
                CW_Asset_Spell spell = CW_Library_Manager.instance.spells.get(this.cw_data.spells.GetRandom());
                if(spell.triger_type == CW_Spell_Triger_Type.DEFEND)
                {// TODO: 可能需要增加对自身位置的参数选择
                    CW_Spell.cast(spell, this, attacker, attacker==null?null:attacker.currentTile);
                }
            }
            if(this.status_effects!=null && this.status_effects.Count > 0)
            {
                foreach(CW_StatusEffectData status_effect in this.status_effects.Values)
                {
                    if (status_effect.status_asset.action_on_hit != null)
                    {
                        status_effect.status_asset.action_on_hit(status_effect, this, attacker);
                    }
                }
            }

            if (this.cw_status.shied > 0)
            {
                if(this.cw_status.wakan_level > 1)
                {
                    float damage_on_shied = Utils.CW_Utils_Others.compress_raw_wakan(damage, this.cw_status.wakan_level);
                    if (this.cw_status.shied > damage_on_shied)
                    {
                        this.cw_status.shied -= (int)damage_on_shied; damage = 0;
                    }
                    else
                    {
                        this.cw_status.shied = 0; damage -= Utils.CW_Utils_Others.get_raw_wakan(this.cw_status.shied, this.cw_status.wakan_level);
                    }
                }
                else
                {
                    if (this.cw_status.shied > damage)
                    {
                        this.cw_status.shied -= (int)damage; damage = 0;
                    }
                    else
                    {
                        this.cw_status.shied = 0; damage -= this.cw_status.shied;
                    }
                }
            }
            if (this.cw_status.health_level > 1) damage = Utils.CW_Utils_Others.compress_raw_wakan(damage, this.cw_status.health_level);
            this.fast_data.health -= (int)damage;
            if (this.fast_data.health < 0)
            {
                this.fast_data.health = 0;
            }
            this.fast_data.health++; // 反原版getHit强制扣血
            return true;
        }
        public static CW_ActorData procrete(CW_Actor main_parent, CW_Actor second_parent)
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
            cw_actor_data.spells = new List<string>();

            cw_actor_data.status = cw_actor_status;
            cw_actor_status.can_culti = cw_actor_data.element.comp_type() != "CW_common";
            cw_actor_status.culti_velo = 1f;
            cw_actor_status.max_age = main_parent.stats.maxAge;
            cw_actor_status.shied = 0;
            cw_actor_status.wakan = 0;
            cw_actor_status.wakan_level = 1;
            cw_actor_status.health_level = 1;

            //CW_Library_Manager.instance.cultisys.set_cultisys(cw_actor_data, main_parent.stats.id);
            cw_actor_data.pre_learn_cultibook(CW_Library_Manager.instance.cultibooks.get(CW_Library_Manager.instance.cultibooks.select_better(main_parent.cw_data.cultibook_id, second_parent.cw_data.cultibook_id)));
            return cw_actor_data;
        }
        public void updateStatus_month()
        {
            if(this.cw_status.shied < this.cw_cur_stats.shied)
            {
                this.cw_status.shied += Mathf.Min((int)Utils.CW_Utils_Others.compress_raw_wakan(this.cw_cur_stats.shied_regen, this.cw_status.wakan_level), this.cw_cur_stats.shied - this.cw_status.shied);
            }
            
            if (this.cw_status.can_culti && this.cw_status.wakan < this.cw_cur_stats.wakan)
            {
                int wakan_get = 0; CW_MapChunk chunk = this.currentTile.get_cw_chunk();
                float chunk_co = chunk.wakan_level;
                // 计算人物应得的level 1灵气量
                if (this.cw_status.wakan * Others.CW_Constants.wakan_regen_valid_percent < this.cw_cur_stats.wakan * 100)
                {// 灵气恢复属性的加成
                    wakan_get += (int)(this.cw_cur_stats.wakan_regen * chunk_co);
                }// 修炼获取
                wakan_get += (int)((1 + this.cw_cur_stats.mod_cultivation) * (1+this.cw_data.status.culti_velo) * chunk_co);
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
            if((this.cw_data.cultisys & Others.CW_Constants.cultisys_bushido_tag)==0 || this.fast_data.health * Others.CW_Constants.health_regen_valid_percent < this.cw_cur_stats.base_stats.health * 100)
            {
                if (this.cw_status.health_level>1)
                {
                    float pure_regen_health = Utils.CW_Utils_Others.compress_raw_wakan(this.cw_cur_stats.health_regen, this.cw_status.health_level);
                    if (pure_regen_health > this.cw_cur_stats.base_stats.health - this.fast_data.health) pure_regen_health = this.cw_cur_stats.base_stats.health - this.fast_data.health;
                    this.fast_data.health += (int)pure_regen_health;
                }
                else
                {
                    this.fast_data.health += Mathf.Min(this.cw_cur_stats.health_regen, this.cw_cur_stats.base_stats.health - this.fast_data.health);
                }
            }
            
            
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
                List<CW_Asset_Spell> spells = CW_Library_Manager.instance.spells.search(CW_Library_Spell.make_tags(this.cw_data.element.comp_type(), CW_Spell_Tag.ATTACK, CW_Spell_Tag.SUMMON, CW_Spell_Tag.DEFEND), Spell_Search_Type.CONTAIN_ANY_TAGS);
                if (spells.Count > 0) this.learn_spell(spells.GetRandom().id);
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
        }
        public void learn_spell(string spell_id)
        {
            // 此处不抛异常，数组中元素为null是正常情况
            if (string.IsNullOrEmpty(spell_id)) throw new Exception("Try to learn 'null' spell");
            if (this.cw_data.spells.Contains(spell_id)) return;

            CW_Asset_Spell spell_asset = CW_Library_Manager.instance.spells.get(spell_id);
            if (spell_asset == null) throw new Exception("No Found Spell '" + spell_id + "'");
            if (spell_asset.allow_actor(this)) this.cw_data.spells.Add(spell_id);
            //print(string.Format("{0} successfully learn '{1}'", this.fast_data.actorID, spell_id));
        }
        public void learn_spells(string[] spell_ids)
        {
            if (spell_ids == null) throw new Exception("Null spells array");
            foreach(string spell_id in spell_ids)
            {
                if(!string.IsNullOrEmpty(spell_id)) this.learn_spell(spell_id);
            }
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
            this.cw_cur_stats = new CW_BaseStats(CW_Actor.get_curstats(this));
            this.cw_data = new CW_ActorData();
            this.cw_status = new CW_ActorStatus();
            

            this.cw_data.status = this.cw_status;

            this.cw_data.cultisys_level = new int[CW_Library_Manager.instance.cultisys.list.Count];
            this.cw_data.element = new CW_Element(prefer_elements: this.cw_stats.prefer_element, prefer_scale: this.cw_stats.prefer_element_scale);
            this.cw_data.spells = new List<string>();

            this.cw_status.can_culti = this.cw_data.element.comp_type()!="CW_common";
            this.cw_status.culti_velo = this.cw_stats.culti_velo;
            this.cw_status.shied = 0;
            this.cw_status.wakan = 0;
            this.cw_status.wakan_level = 1;
            this.cw_status.health_level = 1;
            this.cw_status.max_age = this.cw_stats.origin_stats.maxAge;

            //CW_Library_Manager.instance.cultisys.set_cultisys(cw_data, this.stats.id);
        }
    }
}
