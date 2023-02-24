using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Others;
using UnityEngine;

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
    /// <summary>
    /// 法术触发类型，目前只支持ATTACK, DEFEND
    /// </summary>
    public enum CW_Spell_Triger_Type
    {
        ATTACK,
        ATT_DEF,
        ATT_MOV,
        ATT_DEF_MOV,
        ALL,
        DEFEND,
        MOVE,
        DEF_MOV,
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
        /// <summary>
        /// 动画id，在纯粹的状态法术上为状态id，当然，采用自定义spell_action可无视此项
        /// </summary>
        public string anim_id;
        public int rarity;
        public float free_val;
        public int min_cost_val;
        public float cost;
        /// <summary>
        /// 暂不使用，学习等级
        /// </summary>
        public int learn_level;
        /// <summary>
        /// 暂不使用，释放等级
        /// </summary>
        public int cast_level;
        public bool can_get_by_random;
        public bool can_store_in_book;
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
        public CW_Delegates.CW_Spell_Cost_Action check_and_cost_action;
        public CW_Delegates.CW_Spell_Action spell_action;
        public CW_Delegates.CW_Spell_Action anim_action;
        public CW_Delegates.CW_Spell_Action damage_action;
        internal float random_learn_chance;
        public CW_Asset_Spell(
            string id, string anim_id, 
            CW_Element element, string element_type_limit = null, 
            int rarity = 3, float free_val = 1, float cost = 0.01f, int min_cost = 5, int learn_level = 1, int cast_level = 1, bool can_get_by_random = true, bool can_store_in_book = true,
            bool cultisys_black_or_white_list = true, List<string> cultisys_list = null, List<string> banned_races = null, 
            CW_Spell_Target_Type target_type = CW_Spell_Target_Type.ACTOR, 
            CW_Spell_Target_Camp target_camp = CW_Spell_Target_Camp.ENEMY, 
            CW_Spell_Triger_Type triger_type = CW_Spell_Triger_Type.ATTACK, 
            CW_Spell_Animation_Type anim_type = CW_Spell_Animation_Type.ON_TARGET, 
            CW_Delegates.CW_Spell_Action damage_action = null, 
            CW_Delegates.CW_Spell_Action anim_action = null, 
            CW_Delegates.CW_Spell_Action spell_action = null,  
            CW_Delegates.CW_Spell_Cost_Action check_and_cost_action = null)
        {
            if (String.IsNullOrEmpty(anim_id)) anim_id = id;
            //if (spell_action == null) throw new Exception("spell_action cannot be null");
            this.id = id;
            this.anim_id = anim_id;
            this.element = element;
            this.rarity = rarity;
            this.random_learn_chance = 5f / (5 + rarity);
            this.free_val = free_val;
            this.cost = cost;
            this.min_cost_val = min_cost;
            this.learn_level = learn_level;
            this.cast_level = cast_level;
            this.can_get_by_random = can_get_by_random;
            this.can_store_in_book = can_store_in_book;
            this.cultisys_black_or_white_list = cultisys_black_or_white_list;
            this.cultisys_list = cultisys_list == null ? new List<string>() : cultisys_list;
            this.banned_races = banned_races == null ? new List<string>() : banned_races;
            this.target_type = target_type;
            this.target_camp = target_camp;
            this.triger_type = triger_type;
            this.anim_type = anim_type;
            this.spell_action = spell_action;
            // TODO: 添加damage_action自动适配参数
            this.damage_action = damage_action;
            // TODO: 添加anim_action自动适配参数
            this.anim_action = anim_action;
            // TODO: check_and_cost_action
            this.check_and_cost_action = check_and_cost_action;
            this.element_type_limit = element_type_limit;
            this.base_elements_contained = new bool[element.base_elements.Length];
            this.tags = 0;
            this.addition_element_tags = new List<string>();
        }
        internal void remove_element_tags()
        {
            this.tags &= ~((1ul << (element.base_elements.Length + addition_element_tags.Count)) - 1);
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
            //WorldBoxConsole.Console.print("Spell tag:" + Convert.ToString((long)tags, 16));
            if (cultisys_black_or_white_list)
            {// 黑名单
                this.allowed_cultisys = 0xffffffff;
                foreach(string cultisys in cultisys_list)
                {
                    this.allowed_cultisys &= ~CW_Library_Manager.instance.cultisys.get(cultisys)._tag;
                }
            }
            else
            {// 白名单
                this.allowed_cultisys = 0;
                foreach (string cultisys in cultisys_list)
                {
                    this.allowed_cultisys &= CW_Library_Manager.instance.cultisys.get(cultisys)._tag;
                }
            }
            if (element_type_limit == "none")
            {
                remove_element_tags();
            }
        }
        internal bool judge_can_get(CW_Actor actor)
        {
            return Utils.CW_Utils_Others.max_of(actor.cw_data.cultisys_level)>=this.learn_level && Toolbox.randomChance(this.random_learn_chance * CW_Element.get_similarity(actor.cw_data.element, this.element) * Mathf.Sqrt(actor.fast_data.intelligence+1));
        }
        public void add_tag(CW_Spell_Tag tag)
        {
            this.tags |= 1ul << (int)tag;
        }
        public bool have_tag(CW_Spell_Tag tag)
        {
            return (this.tags & (1ul << (int)tag)) > 0;
        }
        public string get_type()
        {
            if (this.have_tag(CW_Spell_Tag.INBORN_POWER))
            {
                return "0_inborn_power_spell";
            }
            else if (this.have_tag(CW_Spell_Tag.ACQUIRED_POWER))
            {
                return "1_acquired_power_spell";
            }
            else if (this.have_tag(CW_Spell_Tag.IMMORTAL))
            {
                return "2_immortal_spell";
            }
            else if (this.have_tag(CW_Spell_Tag.BUSHIDO))
            {
                return "3_bushido_spell";
            }
            else
            {
                return "4_other_spell";
            }
        }
        public void add_element_tag(string element_id)
        {
            this.addition_element_tags.Add(element_id);
        }
        public bool allow_actor(CW_Actor cw_actor, bool ignore_level_limit = false)
        {
            return ((cw_actor.cw_data.cultisys & this.allowed_cultisys) > 0) && (!banned_races.Contains(cw_actor.stats.race));
        }
        internal BaseSimObject select_target(BaseSimObject pUser, BaseSimObject pEnemy)
        {
            return this.anim_type == CW_Spell_Animation_Type.ON_USER ? pUser : pEnemy;
        }
        public float check_and_cost(BaseSimObject pUser)
        {
            return check_and_cost_action==null?0:check_and_cost_action(this, pUser);
        }
    }
    public enum Spell_Search_Type
    {
        EXACT,
        CONTAIN_ANY_TAGS,
        CONTAIN_ALL_TAGS
    }
    public class CW_Library_Spell : CW_Asset_Library<CW_Asset_Spell>
    {
        internal static ulong make_tags(uint cultisys)
        {
            ulong tags = 0;
            int cultisys_tag = 0;
            CW_Asset_CultiSys cultisys_asset;
            while(cultisys > 0)
            {
                if((cultisys & 0x1) == 1)
                {
                    cultisys_asset = CW_Library_Manager.instance.cultisys.list[cultisys_tag];
                    foreach(CW_Spell_Tag tag in cultisys_asset.addition_spell_require)
                    {
                        tags |= 1ul << (int)tag;
                    }
                }
                cultisys >>= 1;
                cultisys_tag++;
            }
            return tags;
        }
        internal static ulong make_tags(params CW_Spell_Tag[] tags)
        {
            ulong tag = 0;
            foreach(CW_Spell_Tag tag_tag in tags)
            {
                tag |= 1ul << (int)tag_tag;
            }
            return tag;
        }
        internal static ulong make_tags(string element_id, ulong prefix_tags, params CW_Spell_Tag[] addition_tags)
        {
            return prefix_tags|make_tags(element_id, addition_tags);
        }
        internal static ulong make_tags(string element_id, params CW_Spell_Tag[] addition_tags)
        {
            //throw new NotImplementedException();
            ulong tag = make_tags(addition_tags);
            tag |= CW_Library_Manager.instance.elements.get(element_id)._tag;
            return tag;
        }
        internal static ulong make_tags(CW_Element element, params CW_Spell_Tag[] addition_tags)
        {
            ulong tag = make_tags(addition_tags);
            for(int i = 0; i < element.base_elements.Length; i++)
            {
                if (element.base_elements[i] > 0) tag |= 1ul << i;
            }
            tag |= element.get_type()._tag;
            return tag;
        }
        internal static void filter_list_by_element(List<CW_Asset_Spell> spell, ulong tags)
        {
            int i;
            for(i=0; i < spell.Count; i++)
            {
                if (((spell[i].tags & 0x1f) | tags) == tags) continue;
                spell.Swap(i, spell.Count - 1);
                spell.RemoveAt(spell.Count - 1);
                i--;
            }
        }
        internal static void filter_list(List<CW_Asset_Spell> spell, ulong tags, Spell_Search_Type search_type)
        {
            int i;
            switch (search_type)
            {
                case Spell_Search_Type.EXACT:
                    {
                        for (i = 0; i < spell.Count; i++)
                        {
                            if (spell[i].tags != tags)
                            {
                                spell.Swap(i, spell.Count - 1);
                                spell.RemoveAt(spell.Count - 1);
                                i--;
                            }
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ANY_TAGS:
                    {
                        for (i = 0; i < spell.Count; i++)
                        {
                            if ((spell[i].tags & tags)==0)
                            {
                                spell.Swap(i, spell.Count - 1);
                                spell.RemoveAt(spell.Count - 1);
                                i--;
                            }
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ALL_TAGS:
                    {
                        for (i = 0; i < spell.Count; i++)
                        {
                            if ((spell[i].tags | tags) != spell[i].tags)
                            {
                                spell.Swap(i, spell.Count - 1);
                                spell.RemoveAt(spell.Count - 1);
                                i--;
                            }
                        }
                        break;
                    }
            }
        }
        public override CW_Asset_Spell clone(string pNew, string pFrom)
        {
            CW_Asset_Spell new_asset = base.clone(pNew, pFrom);
            CW_Asset_Spell from = get(pFrom);
            new_asset.element = from.element.deepcopy();
            new_asset.cultisys_list = new List<string>();
            foreach(string cultisys in from.cultisys_list)
            {
                new_asset.cultisys_list.Add(cultisys);
            }
            new_asset.banned_races = new List<string>();
            foreach(string race in from.banned_races)
            {
                new_asset.banned_races.Add(race);
            }
            new_asset.base_elements_contained = new bool[from.base_elements_contained.Length];
            for(int i = 0; i < from.base_elements_contained.Length; i++)
            {
                new_asset.base_elements_contained[i] = from.base_elements_contained[i];
            }
            new_asset.addition_element_tags = new List<string>();
            foreach(string element_tag in from.addition_element_tags)
            {
                new_asset.addition_element_tags.Add(element_tag);
            }
            new_asset.damage_action = from.damage_action;
            new_asset.anim_action = from.anim_action;
            new_asset.spell_action = from.spell_action;
            new_asset.check_and_cost_action = from.check_and_cost_action;
            new_asset.tags = from.tags;
            new_asset.allowed_cultisys = from.allowed_cultisys;
            new_asset.random_learn_chance = from.random_learn_chance;
            return new_asset;
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
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if ((asset.tags & tags) != 0ul) list.Add(asset);
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ALL_TAGS:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if ((asset.tags | tags) == asset.tags) list.Add(asset);
                        }
                        break;
                    }
            }

            return list;
        }
        internal List<CW_Asset_Spell> search_for_random_learn(ulong tags, Spell_Search_Type search_type, CW_Actor actor)
        {
            List<CW_Asset_Spell> list = new List<CW_Asset_Spell>();
            switch (search_type)
            {
                case Spell_Search_Type.EXACT:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if (asset.tags == tags && asset.can_get_by_random && asset.judge_can_get(actor)) list.Add(asset);
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ANY_TAGS:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if ((asset.tags & tags) != 0ul && asset.can_get_by_random && asset.judge_can_get(actor)) list.Add(asset);
                        }
                        break;
                    }
                case Spell_Search_Type.CONTAIN_ALL_TAGS:
                    {
                        foreach (CW_Asset_Spell asset in this.list)
                        {
                            if ((asset.tags | tags)==asset.tags && asset.can_get_by_random && asset.judge_can_get(actor)) list.Add(asset);
                        }
                        break;
                    }
            }
            
            return list;
        }

        internal override void register()
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                this.list[i].register();
            }
        }
    }
}
