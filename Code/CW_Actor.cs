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
        public static Func<Actor, bool> get_event_full_heal = CW_ReflectionHelper.create_getter_bool<Actor>("event_full_heal");
        public static Func<Actor, List<ActorTrait>> get_s_special_effect_traits = CW_ReflectionHelper.create_getter<Actor, List<ActorTrait>>("s_special_effect_traits");

        #endregion
        #region Setter
        public static Action<Actor, bool> set_statsDirty = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "statsDirty");
        public static Action<Actor, bool> set_event_full_heal = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "event_full_heal");
        public static Action<Actor, bool> set_item_sprite_dirty = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "item_sprite_dirty");
        public static Action<Actor, bool> set_trait_weightless = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "_trait_weightless");
        public static Action<Actor, bool> set_trait_peaceful = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "_trait_peaceful");
        public static Action<Actor, bool> set_trait_fire_resistant = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "_trait_fire_resistant");
        public static Action<Actor, bool> set_status_frozen = (Action<Actor, bool>)CW_ReflectionHelper.createNewSetter<Actor, bool>(typeof(Actor), "_status_frozen");
        public static Action<Actor, float> set_attackTimer = (Action<Actor, float>)CW_ReflectionHelper.createNewSetter<Actor, float>(typeof(Actor), "attackTimer");
        public static Action<Actor, float> set_s_attackSpeed_seconds = (Action<Actor, float>)CW_ReflectionHelper.createNewSetter<Actor, float>(typeof(Actor), "s_attackSpeed_seconds");
        public static Action<Actor, WeaponType> set_s_attackType = (Action<Actor, WeaponType>)CW_ReflectionHelper.createNewSetter<Actor, WeaponType>(typeof(Actor), "s_attackType");
        public static Action<Actor, string> set_s_slashType = (Action<Actor, string>)CW_ReflectionHelper.createNewSetter<Actor, string>(typeof(Actor), "s_slashType");
        public static Action<Actor, string> set_s_weapon_texture = (Action<Actor, string>)CW_ReflectionHelper.createNewSetter<Actor, string>(typeof(Actor), "s_weapon_texture");
        public static Action<Actor, PersonalityAsset> set_s_personality = (Action<Actor, PersonalityAsset>)CW_ReflectionHelper.createNewSetter<Actor, PersonalityAsset>(typeof(Actor), "s_personality");
        public static Action<Actor, ProfessionAsset> set_professionAsset = (Action<Actor, ProfessionAsset>)CW_ReflectionHelper.createNewSetter<Actor, ProfessionAsset>(typeof(Actor), "professionAsset");
        public static Action<Actor, ActorStatus> set_data = (Action<Actor, ActorStatus>)CW_ReflectionHelper.createNewSetter<Actor, ActorStatus>(typeof(Actor), "data");
        public static Action<Actor, List<ActorTrait>> set_s_special_effect_traits = (Action<Actor, List<ActorTrait>>)CW_ReflectionHelper.createNewSetter<Actor, List<ActorTrait>>(typeof(Actor), "s_special_effect_traits");

        #endregion
        #region Func
        public static Action<Actor> func_updateTargetScale = (Action<Actor>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "updateTargetScale");
        public static Action<Actor> func_findHeadSprite = (Action<Actor>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "findHeadSprite");
        public static Action<Actor, bool> func_checkMadness = (Action<Actor, bool>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "checkMadness");
        public static Func<Actor, string, bool> func_haveStatus = (Func<Actor, string, bool>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "haveStatus");
        public static Action<Actor, int> func_newCreature = (Action<Actor, int>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "newCreature");
        public static Action<Actor, Kingdom> func_setKingdom = (Action<Actor, Kingdom>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "setKingdom");
        public static Action<Actor, WorldTile, float> func_spawnOn = (Action<Actor, WorldTile, float>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "spawnOn");
        public static Action<Actor> func_create = (Action<Actor>)CW_ReflectionHelper.GetFastMethod(typeof(Actor), "create");

        #endregion
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
            this.cw_status.can_culti = this.cw_stats.culti_velo > 0;
            this.cw_status.culti_velo = this.cw_stats.culti_velo;
            this.cw_status.shied = 0;
            this.cw_status.wakan = 0;
            this.cw_status.wakan_level = 1;

            this.cw_data.status = this.cw_status;

            this.cw_data.cultisys_level = new int[CW_Library_Manager.instance.cultisys.list.Count];
            this.cw_data.element = new CW_Element(prefer_elements: this.cw_stats.prefer_element, prefer_scale: this.cw_stats.prefer_element_scale);
            this.cw_data.spells = new List<string>();
        }
    }
}
