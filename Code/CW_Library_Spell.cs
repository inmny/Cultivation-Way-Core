﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Others;
namespace Cultivation_Way.Library
{
    public enum CW_Spell_Animation_Type
    {
        ON_USER,
        ON_TARGET,
        USER_TO_TARGET,
        TARGET_TO_USER,
        UPWARD,
        DOWNWARD,
        CUSTOM
    }
    public enum CW_Spell_Triger_Type
    {
        ATTACK,
        DEFEND,
        MOVE,
        OTHERS
    }
    public enum CW_Spell_Target_Type
    {
        TILE,
        CHUNK,
        ACTOR,
        BUILDING
    }
    public enum CW_Spell_Target_Camp
    {
        ENEMY,
        ALIAS,
        BOTH,
        OTHERS
    }
    public enum CW_Spell_Tag
    {
        INBORN_POWER=53,        //先天神通
        ACQUIRED_POWER,         //后天神通
        BUSHIDO,                //武道
        IMMORTAL,               //仙道
        ATTACK,                 //攻伐
        DEFEND,                 //防御
        SUMMON,                 //召唤
        MOVE,                   //身法
        POSITIVE_STATUS,        //正面效果
        NEGATIVE_STATUS         //负面效果
    }
    public class CW_Asset_Spell : Asset
    {
        public string anim_id;
        public int rarity;
        public float might;
        public float cost;
        public int learn_level;
        public int cast_level;

        internal uint allowed_cultisys;
        internal ulong tags;
        internal bool[] base_elements_contained;
        internal List<string> addition_element_tags;
        public bool cultisys_black_or_white_list;
        public List<string> cultisys_list;
        public List<string> banned_races;
        public string element_type_limit;
        public CW_Element element;
        public CW_Spell_Target_Type target_type;
        public CW_Spell_Target_Camp target_camp;
        public CW_Spell_Triger_Type triger_type;
        public CW_Spell_Animation_Type anim_type;
        public CW_Delegates.CW_Spell_Action spell_action;
        public CW_Delegates.CW_Spell_Action anim_action;
        public CW_Delegates.CW_Spell_Action damage_action;
        public CW_Asset_Spell(string id, string anim_id, CW_Delegates.CW_Spell_Action spell_action, CW_Element element, int rarity = 1, float might = 1, float cost = 1, int learn_level = 1, int cast_level = 1, bool cultisys_black_or_white_list = true, List<string> cultisys_list = null, List<string> banned_races = null, CW_Spell_Target_Type target_type = CW_Spell_Target_Type.ACTOR, CW_Spell_Target_Camp target_camp = CW_Spell_Target_Camp.ENEMY, CW_Spell_Triger_Type triger_type = CW_Spell_Triger_Type.ATTACK, CW_Spell_Animation_Type anim_type = CW_Spell_Animation_Type.ON_TARGET, CW_Delegates.CW_Spell_Action damage_action = null, CW_Delegates.CW_Spell_Action anim_action = null, string element_type_limit = null)
        {
            if (String.IsNullOrEmpty(anim_id)) anim_id = id;
            if (spell_action == null) throw new Exception("spell_action cannot be null");
            this.id = id;
            this.anim_id = anim_id;
            this.element = element;
            this.rarity = rarity;
            this.might = might;
            this.cost = cost;
            this.learn_level = learn_level;
            this.cast_level = cast_level;
            this.cultisys_black_or_white_list = cultisys_black_or_white_list;
            this.cultisys_list = cultisys_list == null ? new List<string>() : cultisys_list;
            this.banned_races = banned_races;
            this.target_type = target_type;
            this.target_camp = target_camp;
            this.triger_type = triger_type;
            this.anim_type = anim_type;
            this.spell_action = spell_action;
            // TODO: 添加damage_action自动适配参数
            this.damage_action = damage_action == null ? Actions.CW_SpellAction_Damage.default_attack_enemy : damage_action;
            // TODO: 添加anim_action自动适配参数
            this.anim_action = anim_action == null ? Actions.CW_SpellAction_Anim.default_on_enemy : anim_action;
            this.element_type_limit = element_type_limit;
            this.base_elements_contained = new bool[element.base_elements.Length];
            this.tags = 0;
            this.addition_element_tags = new List<string>();
        }
        internal void register()
        {
            int i;
            for (i = 0; i < element.base_elements.Length; i++)
            {
                base_elements_contained[i] = element.base_elements[i] > 0;
                if (base_elements_contained[i]) this.tags |= 1ul << i;
            }
            for (i = 0; i < addition_element_tags.Count; i++)
            {
                if (CW_Library_Manager.instance.elements.get(addition_element_tags[i]) == null) throw new Exception(String.Format("No such element '{0}' in register spell '{1}'", addition_element_tags[i], id));
                this.tags |= 1ul << (CW_Library_Manager.instance.elements.get(addition_element_tags[i]).tag + base_elements_contained.Length);
            }
            // TODO: 添加修炼体系
            throw new NotImplementedException();
        }
        public void add_tag(CW_Spell_Tag tag)
        {
            this.tags |= 1ul << (int)tag;
        }
        public void add_element_tag(string element_id)
        {
            this.addition_element_tags.Add(element_id);
        }

    }
    public enum Spell_Search_Type
    {
        EXACT,
        CONTAIN_ANY_TAGS,
        CONTAIN_ALL_TAGS
    }
    public class CW_Library_Spell : AssetLibrary<CW_Asset_Spell>
    {
        internal void register()
        {
            for(int i = 0; i < this.list.Count; i++)
            {
                this.list[i].register();
            }
        }
        internal List<CW_Asset_Spell> search(ulong tags, Spell_Search_Type search_type)
        {
            List<CW_Asset_Spell> list = new List<CW_Asset_Spell>();
            switch (search_type)
            {
                case Spell_Search_Type.EXACT:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if (asset.tags == tags) list.Add(asset);
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ANY_TAGS:
                    {
                        tags = ~tags;
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if ((asset.tags ^ tags) == 0ul) list.Add(asset);
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ALL_TAGS:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if (asset.tags >= tags) list.Add(asset);
                        }
                        break;
                    }
            }
            
            return list;
        }
    }
}