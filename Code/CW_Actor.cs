﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using Cultivation_Way.Library;
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

        #endregion
        public void add_child(ActorStatus orgin_status)
        {
            if (this.cw_data.children_info == null) this.cw_data.children_info = new List<CW_Family_Member_Info>();
            this.cw_data.children_info.Add(new CW_Family_Member_Info(orgin_status));
            this.fast_data.children++;
        }
        public static CW_ActorData procrete(CW_Actor main_parent, CW_Actor second_parent)
        {
            CW_ActorData cw_actor_data = new CW_ActorData();
            CW_ActorStatus cw_actor_status = new CW_ActorStatus();
            cw_actor_data.cultibook_id = CW_Library_Manager.instance.cultibooks.select_better(main_parent.cw_data.cultibook_id, second_parent.cw_data.cultibook_id);

            if (!string.IsNullOrEmpty(cw_actor_data.cultibook_id)) 
            {
                try
                {
                    CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(cw_actor_data.cultibook_id);
                    cultibook.cur_culti_nr++; cultibook.histroy_culti_nr++;
                }
                catch (Exception)
                {
                    throw new Exception(String.Format("Cultibook error for actor:{0} and actor:{1}",main_parent.fast_data.actorID,second_parent.fast_data.actorID));
                }
            }

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

            CW_Library_Manager.instance.cultisys.set_cultisys(cw_actor_data, main_parent.stats.id);
            return cw_actor_data;
        }
        public void updateStatus_month()
        {
            this.cw_status.shied += Mathf.Min(this.cw_cur_stats.shied_regen, this.cw_cur_stats.shied - this.cw_status.shied);
            this.cw_status.wakan += Mathf.Min(this.cw_cur_stats.wakan_regen, this.cw_cur_stats.wakan - this.cw_status.wakan);
            this.fast_data.health += Mathf.Min(this.cw_cur_stats.health_regen, this.cw_cur_stats.base_stats.health - this.fast_data.health);
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
                    if (this.cw_data.cultisys_level[cultisys_tag] == Others.CW_Constants.max_cultisys_level - 1)
                    {
                        //this.fast_data.favorite = true;
                    }
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
                else
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
            cultibook.cur_culti_nr++;
            cultibook.histroy_culti_nr++;
            CW_Asset_CultiBook prev_cultibook = CW_Library_Manager.instance.cultibooks.get(this.cw_data.cultibook_id);
            if (prev_cultibook != null && --prev_cultibook.cur_culti_nr == 0) prev_cultibook.try_deprecate();

            this.cw_data.cultibook_id = cultibook.id;
            this.learn_spells(cultibook.spells);
        }
        private int gen_cultibook_level()
        {
            int level; float stop_chance = (1f + this.fast_data.intelligence) / (4f + this.fast_data.intelligence);
            for (level = 0; level < Others.CW_Constants.cultibook_max_level; level++)
            {
                if (Toolbox.randomChance(stop_chance)) break;
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
            culti_book.re_author(this);
            culti_book.order++;
            __modify_cultibook_last_step(culti_book);
            this.learn_cultibook(culti_book);
        }

        private void create_cultibook()
        {
            CW_Asset_CultiBook culti_book = new CW_Asset_CultiBook(this);
            culti_book.order = 0;
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
            this.cw_status.max_age = this.cw_stats.origin_stats.maxAge;

            CW_Library_Manager.instance.cultisys.set_cultisys(cw_data, this.stats.id);
        }
    }
}
