using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using Cultivation_Way.Library;
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
        }
    }
}
