using ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Utils;
using Cultivation_Way.Extensions;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Actor
    {
        private static CW_Actor new_created_actor;
        private static CW_BaseStats tmp_base_stats = new CW_BaseStats();
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "getUnitTexture")]
        public static bool actor_getUnitTexture(ActorBase __instance, ref string __result)
        {
            if (__instance.stats.race != "yao") return true; 
            CW_Actor actor = (CW_Actor)__instance;
            switch (actor.fast_data.profession)
            {
                case UnitProfession.Baby:
                    __result = __instance.stats.texture_path + "/unit_child";
                    break;
                case UnitProfession.Null:
                case UnitProfession.Unit:
                    __result = __instance.stats.texture_path + (actor.fast_data.gender == ActorGender.Male ? "/unit_male_1" : "/unit_female_1");
                    break;
                case UnitProfession.Warrior:
                    __result = __instance.stats.texture_path + "/unit_warrior_1";
                    break;
                case UnitProfession.Leader:
                    __result = __instance.stats.texture_path + "/unit_leader";
                    break;
                case UnitProfession.King:
                    __result = "races/eastern_human/unit_king";
                    break;
            }
            return false;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        public static bool actor_getHit(Actor __instance, ref float pDamage, ref bool pFlash, AttackType pType = AttackType.Other, BaseSimObject pAttacker = null, bool pSkipIfShake = true)
        {
            if(((CW_Actor)__instance).__get_hit(pDamage, (Others.CW_Enums.CW_AttackType)pType, pAttacker, pSkipIfShake))
            {
                __instance.base_data.health++;
                pDamage = 0f;
                return true;
            }
            else
            {
                if (pAttacker != __instance)
                {
                    CW_Actor.set_attackedBy(__instance, pAttacker);
                    ((CW_Actor)__instance).try_to_set_attack_target_by_attacked_by();
                    //CW_Actor.set_attackTarget(__instance, pAttacker);
                }
                pFlash = false;
                return false;
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        public static void actor_getHit_post(Actor __instance, bool pFlash)
        {
            if(pFlash) CW_Actor.set_timer_action(__instance, -1);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "updateAge")]
        public static bool actor_updateAge(Actor __instance)
        {
            CW_Actor cw_actor = (CW_Actor)__instance;
            // 单独为武道添加的经验获取方式
            if (cw_actor.fast_data.health < cw_actor.cw_cur_stats.base_stats.health && cw_actor.has_cultisys(Others.CW_Constants.cultisys_bushido_tag) && Toolbox.randomChance(Others.CW_Constants.bushido_force_culti_chance*(cw_actor.cw_cur_stats.base_stats.health - cw_actor.fast_data.health) / (cw_actor.cw_cur_stats.base_stats.health * (1+cw_actor.cw_data.cultisys_level[1]))))
            {
                cw_actor.get_fixed_base_stats().age_bonus-= cw_actor.cw_data.cultisys_level[1]+1;
                // 允许血量超限
                cw_actor.fast_data.health += (int)(Others.CW_Constants.bushido_force_culti_co/cw_actor.cw_status.health_level * cw_actor.cw_cur_stats.base_stats.health);
                cw_actor.check_level_up();
            }
            if(!__new_updateAge(cw_actor.fast_data, cw_actor.cw_data.status))
            {
                __instance.killHimself(false, AttackType.Age, true, true);
                return false;
            }
            if (cw_actor.fast_data.age > __instance.stats.maxAge>>1 && Toolbox.randomChance(0.3f))
            {
                __instance.addTrait("wise", false);
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static bool actor_updateStats(ActorBase __instance)
        {
            if (((CW_Actor)__instance).fast_data == null) ((CW_Actor)__instance).fast_data = CW_Actor.get_data((Actor)__instance);

            __actor_updateStats(__instance);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "createNewUnit")]
        public static bool mapbox_createNewUnit(string pStatsID, WorldTile pTile, string pJob, float pZHeight, ActorData pData, ref Actor __result)
        {
            __mapbox_createNewUnit(pStatsID, pTile, pJob, pZHeight, pData, ref __result);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapStats), "updateAge")]
        public static bool mapstats_updateAge(float pElapsed)
        {
            if(MapBox.instance.mapStats.monthTime + pElapsed > 3f && DebugConfig.isOn(DebugOption.SystemUpdateUnits))
            {
                int i;
                for (i = 0; i < W_Content_Helper.list_systems.Count; i++)
                {
                    W_Content_Helper.list_systems[i].clearList();
                }
                MapBox.instance.units.checkAddRemove();
                List<Actor> simpleList = MapBox.instance.units.getSimpleList();
                int count = MapBox.instance.units.Count;
                for (i = 0; i < count; i++)
                {
                    CW_Actor actor = (CW_Actor)simpleList[i];
                    actor.updateStatus_month();
                    actor.check_level_up();
                }

                MapBox.instance.units.checkAddRemove();
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "destroyActor", new Type[] { typeof(Actor) })]
        public static bool mapbox_destroyActor(Actor pActor)
        {
            if (pActor == null) throw new ArgumentNullException("pActor should not be null! In origin game, no possible to pass null to this function");
            if (pActor.object_destroyed) { return true; }

            CW_Actor cw_actor = (CW_Actor)pActor;
            // 回收功法
            CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(cw_actor.cw_data.cultibook_id);
            if (cultibook != null)
            {
                cultibook.cur_culti_nr--;
                if (cultibook.cur_culti_nr <= 0) cultibook.try_deprecate();
                else if(cw_actor.fast_data.actorID == cultibook.author_id)
                {
                    if (string.IsNullOrEmpty(cultibook.author_name)) cultibook.get_author_name(cw_actor);
                    if (string.IsNullOrEmpty(cultibook.name)) cultibook.get_name(cw_actor);
                }
            }
            // 回收体质
            CW_Asset_SpecialBody body = CW_Library_Manager.instance.special_bodies.get(cw_actor.cw_data.special_body_id);
            if (body != null)
            {
                body.cur_own_nr--;
                if (body.cur_own_nr <= 0) body.try_deprecate();
                else if (cw_actor.fast_data.actorID == body.author_id)
                {
                    if(string.IsNullOrEmpty(body.author_name))body.get_author_name(cw_actor);
                    if (string.IsNullOrEmpty(body.name)) body.get_name(cw_actor);
                }
            }
            // 灵气归还
            if (cw_actor.cw_status.wakan > 0 && ModState.instance.destroy_unit_reason == Destroy_Unit_Reason.KILL)
            {
                CW_MapChunk chunk = cw_actor.currentTile.get_cw_chunk();
                chunk.wakan += Utils.CW_Utils_Others.compress_raw_wakan(CW_Utils_Others.get_raw_wakan(cw_actor.cw_status.wakan, cw_actor.cw_status.wakan_level), chunk.wakan_level);
                //MonoBehaviour.print(string.Format("Reuse {0} raw wakan", (int)CW_Utils_Others.get_raw_wakan(cw_actor.cw_status.wakan, cw_actor.cw_status.wakan_level)));
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Baby), "update")]
        public static bool baby_update(Baby __instance, float pElapsed)
        {
            if (Config.paused || ScrollWindow.isWindowActive()) return false;
            CW_Actor cw_actor = __instance.GetComponent<CW_Actor>();
            if (!cw_actor.fast_data.alive) return false;
            if (__instance.timerGrow > pElapsed) return true;
            __instance.timerGrow -= pElapsed;
            __baby_growup(cw_actor);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "restoreHealth")]
        public static bool actor_restore_health(Actor __instance, int pVal)
        {
            ((CW_Actor)__instance).regen_health(pVal, Mathf.Max(1, ((CW_Actor)__instance).cw_status.health_level - 0.1f));
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "killHimself")]
        public static bool actor_killhimself(Actor __instance, AttackType pType)
        {
            if (pType == AttackType.GrowUp) return true;
            if (__instance.haveTrait("asylum")) return false;
            return true;
        }
        private static void __baby_growup(CW_Actor cw_actor)
        {
            CW_Actor new_cw_actor = (CW_Actor)MapBox.instance.createNewUnit(cw_actor.stats.growIntoID, cw_actor.currentTile, null, 0f, null);
            new_cw_actor.startBabymakingTimeout();
            new_cw_actor.fast_data.hunger = new_cw_actor.stats.maxHunger / 2;
            W_Content_Helper.game_stats_data.creaturesBorn--;
            if (cw_actor.stats.unit)
            {
                if (cw_actor.city != null) cw_actor.city.addNewUnit(new_cw_actor, true, true);
                CW_Actor.func_setKingdom(new_cw_actor, cw_actor.kingdom);
            }
            new_cw_actor.fast_data.diplomacy = cw_actor.fast_data.diplomacy;
            new_cw_actor.fast_data.intelligence = cw_actor.fast_data.intelligence;
            new_cw_actor.fast_data.stewardship = cw_actor.fast_data.stewardship;
            new_cw_actor.fast_data.warfare = cw_actor.fast_data.warfare;
            new_cw_actor.fast_data.culture = cw_actor.fast_data.culture;
            new_cw_actor.fast_data.experience = cw_actor.fast_data.experience;
            new_cw_actor.fast_data.level = cw_actor.fast_data.level;
            new_cw_actor.fast_data.setName(cw_actor.fast_data.firstName);
            if (cw_actor.fast_data.skin != -1) new_cw_actor.fast_data.skin = cw_actor.fast_data.skin;
            if (cw_actor.fast_data.skin_set != -1) new_cw_actor.fast_data.skin_set = cw_actor.fast_data.skin_set;
            new_cw_actor.fast_data.age = cw_actor.fast_data.age;
            new_cw_actor.fast_data.bornTime = cw_actor.fast_data.bornTime;
            new_cw_actor.fast_data.health = cw_actor.fast_data.health;
            new_cw_actor.fast_data.gender = cw_actor.fast_data.gender;
            new_cw_actor.fast_data.kills = cw_actor.fast_data.kills;
            new_cw_actor.fast_data.favorite = cw_actor.fast_data.favorite;
            foreach (string text in cw_actor.fast_data.traits)
            {
                if (!(text == "peaceful"))
                {
                    new_cw_actor.addTrait(text, false);
                }
            }
            if (MoveCamera.inSpectatorMode() && MoveCamera.focusUnit == cw_actor)
            {
                MoveCamera.focusUnit = new_cw_actor;
            }
            // 由于是值拷贝，cw_status不需要修改
            cw_actor.cw_data.deepcopy_to(new_cw_actor.cw_data);
            // 采用此方式防止功法误删
            if (!string.IsNullOrEmpty(new_cw_actor.cw_data.cultibook_id))
            {
                new_cw_actor.cw_data.cultibook_id = null;
                new_cw_actor.cw_data.spells.Clear();
                new_cw_actor.learn_cultibook(cw_actor.cw_data.cultibook_id);
            }
            // 同样防止体质误删
            if (!string.IsNullOrEmpty(new_cw_actor.cw_data.special_body_id))
            {
                new_cw_actor.cw_data.special_body_id = null;
                new_cw_actor.change_special_body(cw_actor.cw_data.special_body_id);
            }
            // 未使用的对象组合
            if (cw_actor.compose_actors != null && cw_actor.compose_actors.Count>0) throw new NotImplementedException();
            if (cw_actor.compose_buildings != null && cw_actor.compose_buildings.Count>0) throw new NotImplementedException();

            cw_actor.killHimself(true, AttackType.GrowUp, false, false);
        }

        private static void __mapbox_createNewUnit(string pStatsID, WorldTile pTile, string pJob, float pZHeight, ActorData pData, ref Actor __result)
        {
            //WorldBoxConsole.Console.print("try to create cw_actor");

            CW_ActorStats actor_stats = CW_Library_Manager.instance.units.get(pStatsID);

            if (actor_stats == null) { __result = null; if (Others.CW_Constants.is_debugging) WorldBoxConsole.Console.print("No find CW_ActorStats for '"+pStatsID+"'"); return; }
            CW_Actor actor;
            if (Others.CW_Constants.is_debugging)
            {
                try
                {
                    actor = UnityEngine.Object.Instantiate(W_Content_Helper.get_actor_prefab("actors/" + actor_stats.origin_stats.prefab)).gameObject.GetComponent<CW_Actor>();
                }
                catch (Exception)
                {
                    WorldBoxConsole.Console.print("Tried to create actor: " + actor_stats.id);
                    WorldBoxConsole.Console.print("Failed to load prefab for actor: " + actor_stats.origin_stats.prefab);
                    __result = null; return;
                }
            }
            else
            {
                actor = UnityEngine.Object.Instantiate(W_Content_Helper.get_actor_prefab("actors/" + actor_stats.origin_stats.prefab)).gameObject.GetComponent<CW_Actor>();
            }
            actor.transform.name = actor_stats.id;

            actor.setWorld();
            actor.cw_stats = actor_stats;
            actor.loadStats(actor_stats.origin_stats);
            if (pData == null)
            {
                actor.new_creature = true;
                actor.CW_newCreature();
                CW_Actor.func_newCreature(actor, (int)(W_Content_Helper.game_stats_data.gameTime + (double)MapBox.instance.units.Count));
                // 在func_newCreature中会调用updateStats，从而使得fast_data会指向本身的data
                // actor.fast_data = (ActorStatus)CW_Actor.get_data(actor);
                actor.transform.position = pTile.posV3;
                CW_Actor.func_spawnOn(actor, pTile, pZHeight);
                CW_Actor.func_create(actor);
                actor.fast_targets_to_ignore = CW_Actor.get_targets_to_ignore(actor);
            }
            else
            {
                actor.new_creature = false;
                if (pData.status == null) WorldBoxConsole.Console.print("pData.status is null");
                actor.cw_cur_stats = new CW_BaseStats(CW_Actor.get_curstats(actor));
                actor.cw_data = W_Content_Helper.get_load_cw_data(pData); // 由于原版的城市生成生物和存档加载生物采用同样的加载方式，此处通过一个bus函数进行区分。
                if(ModState.instance.load_object_reason == Load_Object_Reason.LOAD_SAVES)
                {
                    if (Others.CW_Constants.force_load_units && (actor.cw_data == null || actor.cw_data.status == null))
                    {
                        actor.new_creature = true;
                        actor.CW_newCreature();
                        //CW_Actor.func_newCreature(actor, (int)(W_Content_Helper.game_stats_data.gameTime + (double)MapBox.instance.units.Count));
                        Debug.Log("Force Load Unit:" + pData.status.actorID);
                    }
                }
                actor.fast_data = pData.status;
                actor.cw_status = actor.cw_data.status;
                actor.cur_spells = new List<string>();
                CW_Actor.set_data(actor, actor.fast_data);
                CW_Actor.set_professionAsset(actor, AssetManager.professions.get(pData.status.profession));

                actor.transform.position = pTile.posV3;
                CW_Actor.func_spawnOn(actor, pTile, pZHeight);
                CW_Actor.func_create(actor);
                actor.fast_targets_to_ignore = CW_Actor.get_targets_to_ignore(actor);

                __actor_updateStats(actor);
                
                
                if (ModState.instance.load_object_reason == Load_Object_Reason.SPAWN)
                {
                    CW_Library_Manager.instance.cultisys.set_cultisys(actor);
                    if (!Others.CW_Constants.cultibook_force_learn && actor.cw_data.cultisys == 0)
                    {
                        CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(actor.cw_data.cultibook_id);
                        if (cultibook != null)
                        {
                            cultibook.cur_culti_nr--;
                            if (cultibook.cur_culti_nr <= 0) cultibook.try_deprecate();
                        }
                        actor.cw_data.cultibook_id = null;
                    }
                    else
                    {// 已经进行了预学习，只需再学习法术即可。
                        CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(actor.cw_data.cultibook_id);
                        if (cultibook != null) actor.learn_spells(cultibook.spells);
                    }
                    
                }
            }
            if (actor.new_creature) CW_Library_Manager.instance.cultisys.set_cultisys(actor);

            actor.fast_shake_timer = CW_Actor.get_shake_timer(actor);
            
            if(Others.CW_Constants.is_debugging) LogText.log("createNewUnit", actor.stats.id + ", " + actor.fast_data.actorID, "");
            if (actor.stats.kingdom != "") CW_Actor.func_setKingdom(actor, MapBox.instance.kingdoms.dict_hidden[actor.stats.kingdom]);
            if (actor.stats.hideOnMinimap)
            {
                actor.transform.parent = W_Content_Helper.transformUnits;
            }
            else
            {
                actor.transform.parent = W_Content_Helper.transformCreatures;
            }
            MapBox.instance.units.Add(actor);
            __result = actor;
        }
        internal static void __actor_updateStats(ActorBase actor_base)
        {
            Actor actor = (Actor)actor_base;
            CW_Actor cw_actor = (CW_Actor)actor;
            
            CW_Actor.set_statsDirty(actor, false);
            
            //if (!cw_actor.fast_data.alive) return;
            // 设定默认皮肤集合
            if (cw_actor.stats.useSkinColors && cw_actor.fast_data.skin_set == -1 && cw_actor.stats.color_sets != null) cw_actor.setSkinSet("default");
            // 若皮肤不存在，则随机选择
            if (cw_actor.stats.useSkinColors && cw_actor.fast_data.skin == -1) cw_actor.fast_data.skin = Toolbox.randomInt(0, cw_actor.stats.color_sets[cw_actor.fast_data.skin_set].colors.Count);
            // 安全检查，只抛出不捕获
            if (Others.CW_Constants.is_debugging && (cw_actor.cw_cur_stats.base_stats != CW_Actor.get_curstats(actor))) throw new Exception("Actor curStats reference error in cw_cur_stats");

            int i, len; uint tmp1;
            cw_actor.cur_spells.Clear();
            cw_actor.cw_cur_stats.clear();
            cw_actor.cw_status.culti_velo = 0;
            cw_actor.cur_spells.AddRange(cw_actor.cw_stats.born_spells);
            cw_actor.cur_spells.AddRange(cw_actor.cw_data.spells);
            // 基础属性样板
            cw_actor.cw_cur_stats.addStats(cw_actor.cw_stats.cw_base_stats);
            cw_actor.cw_cur_stats.addStats(cw_actor.cw_data.fixed_base_stats);
            cw_actor.cw_cur_stats.base_stats.diplomacy += cw_actor.fast_data.diplomacy;
            cw_actor.cw_cur_stats.base_stats.stewardship += cw_actor.fast_data.stewardship;
            cw_actor.cw_cur_stats.base_stats.intelligence += cw_actor.fast_data.intelligence;
            cw_actor.cw_cur_stats.base_stats.warfare += cw_actor.fast_data.warfare;
            // 添加原版等级的属性影响
            cw_actor.cw_cur_stats.base_stats.damage += (cw_actor.fast_data.level - 1) / 2;
            cw_actor.cw_cur_stats.base_stats.armor += (cw_actor.fast_data.level - 1) / 3;
            cw_actor.cw_cur_stats.base_stats.crit += (float)(cw_actor.fast_data.level - 1);
            cw_actor.cw_cur_stats.base_stats.attackSpeed += (float)(cw_actor.fast_data.level - 1);
            cw_actor.cw_cur_stats.base_stats.health += (cw_actor.fast_data.level - 1) * 20;
            // 添加元素的属性影响
            cw_actor.cw_cur_stats.addStats(cw_actor.cw_data.element.comp_bonus_stats());
            // 添加体质的属性影响
            if (!String.IsNullOrEmpty(cw_actor.cw_data.special_body_id) && CW_Library_Manager.instance.special_bodies.dict.ContainsKey(cw_actor.cw_data.special_body_id))
            {
                cw_actor.cw_cur_stats.addStats(CW_Library_Manager.instance.special_bodies.get(cw_actor.cw_data.special_body_id).bonus_stats);
                cw_actor.cur_spells.Add("stxh");
            }

            // 添加修炼产生的属性增幅
            if (cw_actor.cw_status.can_culti)
            {
                cw_actor.cw_status.culti_velo = cw_actor.cw_stats.culti_velo;
                // 添加体系的属性影响
                tmp1 = cw_actor.cw_data.cultisys;
                len = CW_Library_Manager.instance.cultisys.list.Count;
                tmp_base_stats.clear();
                for (i = 0; i < len && tmp1 > 0; i++)
                {
                    if ((tmp1 & 0x1) != 0) { tmp_base_stats.change_to_better((CW_Library_Manager.instance.cultisys.get_bonus_stats(i, cw_actor.cw_data.cultisys_level[i], cw_actor))); }
                    tmp1 >>= 1;
                }
                cw_actor.cw_cur_stats.addStats(tmp_base_stats);
                // 添加功法的属性影响
                if (!string.IsNullOrEmpty(cw_actor.cw_data.cultibook_id) && CW_Library_Manager.instance.cultibooks.dict.ContainsKey(cw_actor.cw_data.cultibook_id))
                {
                    CW_Asset_CultiBook culti_book = CW_Library_Manager.instance.cultibooks.get(cw_actor.cw_data.cultibook_id);
                    cw_actor.cw_cur_stats.addStats(culti_book.bonus_stats);
                    cw_actor.cw_status.culti_velo *= (1 + culti_book.culti_promt);
                }
            }
            // 添加心情的属性影响
            if (string.IsNullOrEmpty(cw_actor.fast_data.mood)) cw_actor.fast_data.mood = "normal";
            MoodAsset moodAsset = AssetManager.moods.get(cw_actor.fast_data.mood);
            if(moodAsset!=null) cw_actor.cw_cur_stats.addStats(moodAsset.baseStats);
            // 添加状态的属性影响
            Dictionary<string, StatusEffectData> activeStatus_dict = CW_Actor.get_activeStatus_dict(actor);
            if (activeStatus_dict != null)
            {
                foreach(StatusEffectData status_effect in activeStatus_dict.Values)
                {
                    cw_actor.cw_cur_stats.addStats(status_effect.asset.baseStats);
                }
            }
            Dictionary<string, CW_StatusEffectData> cw_status_dict = cw_actor.status_effects;
            if(cw_status_dict != null)
            {
                foreach(CW_StatusEffectData status_effect in cw_status_dict.Values)
                {
                    if(status_effect.bonus_stats!=null) cw_actor.cw_cur_stats.addStats(status_effect.bonus_stats);
                    if (cw_actor.can_act && status_effect.status_asset.has_tag(CW_StatusEffect_Tag.BOUND)) cw_actor.can_act = false;
                }
            }
            // 添加特质的属性影响
            len = cw_actor.fast_data.traits.Count;
            for (i = 0; i < len; i++)
            {
                CW_Asset_Trait trait = CW_Library_Manager.instance.traits.get(cw_actor.fast_data.traits[i]);
                if (trait != null)
                {
                    cw_actor.cw_cur_stats.addStats(trait.origin_asset.baseStats);
                    cw_actor.cw_cur_stats.addStats(trait.addition_stats);
                }
            }
            // 添加装备的属性影响
            CW_Asset_Item default_attack = CW_Library_Manager.instance.items.get(cw_actor.stats.defaultAttack);
            if (default_attack != null) cw_actor.cw_cur_stats.addStats(default_attack.cw_base_stats);
            if (cw_actor.stats.use_items)
            {
                List<ActorEquipmentSlot> equipment_slots = ActorEquipment.getList(cw_actor.equipment, false);
                len = equipment_slots.Count;
                for (i = 0; i < len; i++)
                {
                    if(equipment_slots[i].data != null)
                    {
                        try
                        {
                            CW_ItemTools.calc_item_values((CW_ItemData)equipment_slots[i].data);
                        }
                        catch(InvalidCastException)
                        {
                            //WorldBoxConsole.Console.print(cw_actor.fast_data.actorID+" have type error item in slot " + equipment_slots[i].type.ToString());
                            //WorldBoxConsole.Console.print(Utils.CW_ItemTools.item_to_string(equipment_slots[i].data));
                            equipment_slots[i].data = new CW_ItemData(equipment_slots[i].data);
                        }
                        cw_actor.cw_cur_stats.addStats(CW_ItemTools.s_cw_stats);
                    }
                }
            }
            // 添加性格的属性影响
            if (cw_actor.stats.unit)
            {
                CW_Actor.set_s_personality(actor, null);
                if((cw_actor.kingdom!=null && cw_actor.kingdom.isCiv() && cw_actor.kingdom.king==cw_actor) || (cw_actor.city!=null && cw_actor.city.leader == cw_actor))
                {
                    string personality_id = "balanced";
                    int max_val = cw_actor.cw_cur_stats.base_stats.diplomacy;
                    if (cw_actor.cw_cur_stats.base_stats.diplomacy > cw_actor.cw_cur_stats.base_stats.stewardship)
                    {
                        personality_id = "diplomat";
                        max_val = cw_actor.cw_cur_stats.base_stats.diplomacy;
                    }
                    else if (cw_actor.cw_cur_stats.base_stats.diplomacy < cw_actor.cw_cur_stats.base_stats.stewardship)
                    {
                        personality_id = "administrator";
                        max_val = cw_actor.cw_cur_stats.base_stats.stewardship;
                    }
                    if (cw_actor.cw_cur_stats.base_stats.warfare > max_val)
                    {
                        personality_id = "militarist";
                    }
                    PersonalityAsset personality_asset = AssetManager.personalities.get(personality_id);
                    CW_Actor.set_s_personality(actor, personality_asset);
                    cw_actor.cw_cur_stats.addStats(personality_asset.baseStats);
                }
            }
            // 首次属性总结
            cw_actor.cw_cur_stats.normalize();
            cw_actor.cw_cur_stats.apply_mod();
            if (CW_Actor.get_event_full_heal(actor))
            {
                CW_Actor.set_event_full_heal(actor, false);
                cw_actor.cw_status.shield = cw_actor.cw_cur_stats.shield;
            }
            // 添加文化的属性影响
            Culture culture = cw_actor.getCulture();
            if(culture != null)
            {
                cw_actor.cw_cur_stats.base_stats.damage = (int)(cw_actor.cw_cur_stats.base_stats.damage + cw_actor.cw_cur_stats.base_stats.damage * culture.stats.bonus_damage.value);
                cw_actor.cw_cur_stats.base_stats.armor = (int)(cw_actor.cw_cur_stats.base_stats.armor + cw_actor.cw_cur_stats.base_stats.armor * culture.stats.bonus_armor.value);
            }
            // 最后属性总结
            cw_actor.cw_cur_stats.no_zero_for_actor();
            cw_actor.cw_cur_stats.apply_others();
            cw_actor.cw_cur_stats.normalize();
            cw_actor.cw_status.wakan = Mathf.Min(cw_actor.cw_status.wakan, cw_actor.cw_cur_stats.wakan);
            cw_actor.fast_data.health = Mathf.Min(cw_actor.fast_data.health, cw_actor.cw_cur_stats.base_stats.health);
            cw_actor.cw_status.max_age = (int)((cw_actor.cw_stats.origin_stats.maxAge+ cw_actor.cw_cur_stats.age_bonus) * (1f + cw_actor.cw_cur_stats.mod_age/100f));
            // 修炼速度
            cw_actor.cw_status.culti_velo *= 1 + cw_actor.cw_cur_stats.mod_cultivation / 100f;
            // 攻速以及法术释放速度
            cw_actor.m_attackSpeed_seconds = (300f - cw_actor.cw_cur_stats.base_stats.attackSpeed) / (100f + cw_actor.cw_cur_stats.base_stats.attackSpeed);
            cw_actor.s_spell_seconds = cw_actor.m_attackSpeed_seconds;
            CW_Actor.set_s_attackSpeed_seconds(actor, cw_actor.m_attackSpeed_seconds);
            // 设置攻击样式以及武器贴图
            CW_Asset_Item weapon_asset = cw_actor.get_weapon_asset();
            if(weapon_asset.origin_asset.attackType == WeaponType.Range && string.IsNullOrEmpty(weapon_asset.origin_asset.projectile))
            {
                Debug.LogWarning(string.Format("weapon '{0}''s attack type error", weapon_asset.id));
            }
            CW_Actor.set_s_attackType(actor, weapon_asset.origin_asset.attackType);
            CW_Actor.set_s_slashType(actor, weapon_asset.origin_asset.slash);
            CW_Actor.set_item_sprite_dirty(actor, true);
            CW_Actor.set_s_weapon_texture(actor, (cw_actor.stats.use_items && !cw_actor.equipment.weapon.isEmpty())?cw_actor.getWeaponId():String.Empty);
            // 设置头部贴图
            CW_Actor.func_findHeadSprite(actor);
            // 特质更新
            bool has_madness_before = cw_actor.haveTrait("madness");
            HashSet<string> s_traits_ids = CW_ActorStatus.get_s_traits_ids(cw_actor.fast_data);
            List<ActorTrait> s_special_effect_traits = CW_Actor.get_s_special_effect_traits(cw_actor);
            s_traits_ids.Clear();
            if (s_special_effect_traits != null) s_special_effect_traits.Clear();
            len = cw_actor.fast_data.traits.Count; ActorTrait tmp_trait;
            for (i = 0; i < len; i++)
            {
                s_traits_ids.Add(cw_actor.fast_data.traits[i]);
                tmp_trait = AssetManager.traits.get(cw_actor.fast_data.traits[i]);
                if(tmp_trait != null && tmp_trait.action_special_effect != null)
                {
                    if (s_special_effect_traits == null) { s_special_effect_traits = new List<ActorTrait>();  CW_Actor.set_s_special_effect_traits(cw_actor, s_special_effect_traits); }
                    s_special_effect_traits.Add(tmp_trait);
                }
            }
            if (s_special_effect_traits != null && s_special_effect_traits.Count == 0) CW_Actor.set_s_special_effect_traits(cw_actor, null);

            bool has_madness_now = cw_actor.haveTrait("madness");
            if (has_madness_before != has_madness_now) CW_Actor.func_checkMadness(actor, has_madness_now);

            CW_Actor.set_trait_weightless(actor, cw_actor.haveTrait("weightless"));
            CW_Actor.set_trait_peaceful(actor, cw_actor.haveTrait("peaceful"));
            CW_Actor.set_trait_fire_resistant(actor, cw_actor.haveTrait("fire_proof"));
            CW_Actor.set_status_frozen(actor, CW_Actor.func_haveStatus(actor, "frozen"));
            // 收尾
            CW_Actor.set_attackTimer(actor, 0f);
            cw_actor.default_spell_timer = 0;
            CW_Actor.func_updateTargetScale(actor);
            cw_actor.currentScale.x = cw_actor.cw_cur_stats.base_stats.scale;
            cw_actor.currentScale.y = cw_actor.cw_cur_stats.base_stats.scale;
            cw_actor.currentScale.z = cw_actor.cw_cur_stats.base_stats.scale;
            return;
        }
        /// <summary>
        /// 当年龄超上限，返回false；否则true
        /// </summary>
        internal static bool __new_updateAge(ActorStatus origin_status, CW_ActorStatus cw_status)
        {
            origin_status.age++;
            CW_ActorStats cw_actor_stats = CW_Library_Manager.instance.units.get(origin_status.statsID);
            CW_ActorStatus.actorstatus_updateAttributes(origin_status, cw_actor_stats.origin_stats, AssetManager.raceLibrary.get(cw_actor_stats.origin_stats.race), false);
            

            if (cw_actor_stats.origin_stats.maxAge == 0 || !MapBox.instance.worldLaws.world_law_old_age.boolVal || origin_status.haveTrait("immortal")) return true;

            return cw_status.max_age > origin_status.age || Toolbox.randomChance(Others.CW_Constants.exceed_max_age_chance);
        }
        
    }
}
