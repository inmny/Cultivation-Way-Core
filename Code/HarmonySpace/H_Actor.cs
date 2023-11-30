using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using HarmonyLib;
using NeoModLoader.api.attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cultivation_Way.HarmonySpace;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class H_Actor
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Actor), nameof(Actor.killHimself))]
    public static void killHimself_patch(Actor __instance)
    {
        ((CW_Actor)__instance).leave_data();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BatchActors), nameof(BatchActors.createJobs))]
    public static void createJobs_patch(BatchActors __instance)
    {
        JobManagerTools.add_actor_update_month_job(__instance);
    }

    #region 人物属性更新 目的在于应用模组中新的人物属性加成

    /**
     * 原版中采用dirty标记来判断是否需要更新人物属性
     * 当需要更新时, 会调用updateStats函数
     * 这里拦截updateStats函数,
     * 通过HarmonyTranspiler在原版代码
     * <code>
     *  test = S.attack_speed;
     *  baseStats[text] += (float)(this.data.level-1);
     *  //**此处插入代码
     *  if (base.hasAnyStatusEffect())
     *  </code>
     * 中指定位置插入调用cw_updateStats(this)函数
     *
     * 恰好在统计完生物类型属性、心情、数据中的四维属性、等级、默认武器、状态效果、特质后
     * 忽略了装备目的是为了统计作为血脉主导者在血脉中记录的属性加成
     */
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ActorBase), "updateStats")]
    public static IEnumerable<CodeInstruction> updateStats_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = instructions.ToList();
        int index = codes.FindIndex(instr => instr.opcode == OpCodes.Stfld && ((FieldInfo)instr.operand).Name == "has_status_frozen");
        if (index == -1)
        {
            CW_Core.LogWarning("updateStats_Transpiler: index not found");
            return codes;
        }
        codes.Insert(index+1, new CodeInstruction(OpCodes.Ldarg_0));
        codes.Insert(index+2,
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(H_Actor), nameof(cw_updateStats))));
        return codes;
    }
    [Hotfixable]
    private static void cw_updateStats(Actor actor)
    {
        // 载入修炼体系的加成
        int[] cultisys_levels = actor.data.GetAllCultisysLevels();
        bool has_cultisys = false;

        for (int i = 0; i < cultisys_levels.Length; i++)
        {
            if (cultisys_levels[i] < 0) continue;
            has_cultisys = true;
            CultisysAsset cultisys = Library.Manager.cultisys.list[i];
            actor.stats.mergeStats(cultisys.get_bonus_stats((CW_Actor)actor, cultisys_levels[i]));
        }

        // 载入功法的加成
        Cultibook cultibook = actor.data.GetCultibook();
        if (cultibook != null && has_cultisys) actor.stats.mergeStats(cultibook.bonus_stats);
        // 载入灵根的加成
        actor.stats.mergeStats(actor.data.GetElement().ComputeBonusStats());
        // 载入血脉的加成
        BloodNodeAsset main_blood_node = actor.data.GetMainBlood();
        if (main_blood_node == null) return;
        if (main_blood_node.id == actor.data.id)
        {
            // 记录属性到血脉
            main_blood_node.ancestor_stats.clear();
            main_blood_node.ancestor_stats.mergeStats(actor.stats);
        }
        else
        {
            actor.data.get(DataS.main_blood_purity, out float purity);
            actor.stats.Max(main_blood_node.ancestor_stats, purity * purity);
        }

        CW_Actor cw_actor = (CW_Actor)actor;
        // 载入状态效果的加成
        if (cw_actor.statuses != null)
        {
            foreach (CW_StatusEffectData effect_data in cw_actor.statuses.Values.Where(effect_data =>
                         !effect_data.finished))
            {
                actor.stats.mergeStats(effect_data.bonus_stats);
            }
        }

        // 载入法术
        cw_actor.cur_spells.Clear();
        cw_actor.cur_spells.AddRange(cw_actor.__data_spells);

        if (cw_actor.asset.use_items)
        {
            List<ActorEquipmentSlot> slots = ActorEquipment.getList(cw_actor.equipment);
            foreach (ActorEquipmentSlot slot in slots)
            {
                if (slot.data is not CW_ItemData cw_item_data) continue;
                cw_actor.cur_spells.AddRange(cw_item_data.Spells);
            }
        }
        
        // 载入阴/阳性生物的加成
        if (cw_actor.hasTrait(CW_ActorTraits.negative_creature.id) && !World.world_era.overlay_darkness)
        {
            actor.stats.mergeStats(CW_ActorTraits.negative_creature.base_stats);
        }
        else if (cw_actor.hasTrait(CW_ActorTraits.positive_creature.id) && World.world_era.overlay_darkness)
        {
            actor.stats.mergeStats(CW_ActorTraits.positive_creature.base_stats);
        }

        actor.data.get(DataS.soul, out float soul);
        if (soul > actor.stats[CW_S.soul])
        {
            actor.data.set(DataS.soul, actor.stats[CW_S.soul]);
        }
    }

    #endregion

    #region 人物转换 目的在于将游戏使用的所有Actor转换为CW_Actor

    /**
     * 游戏中Actor均由ActorManager创建，
     * Actor的GameObject存在数个创建的函数
     * 1.
     * <see cref="ActorManager.createNewUnit(string, WorldTile, float)" />
     * 一般创建
     * 2.
     * <see cref="ActorManager.spawnPopPoint(ActorData, WorldTile, City)" />
     * 城市人口出生
     * 3.
     * <see cref="ActorManager.loadObject(ActorData, Actor)" />
     * 读档
     * 此处将两个函数均拦截, 操作与原版操作基本一致，但将Actor替换成经复制得到的CW_Actor
     * 以外，调用了cw_newCreature函数，用于初始化修炼相关的数据
     */
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActorManager), nameof(ActorManager.createNewUnit))]
    public static bool createNewUnit_patch(ActorManager __instance, string pStatsID, WorldTile pTile, float pZHeight,
        ref Actor __result)
    {
        ActorAsset asset = AssetManager.actor_library.get(pStatsID);
        if (asset == null)
        {
            __result = null;
            return false;
        }

        //Logger.Log($"createNewUnit: {pStatsID} with prefab {asset.prefab}");
        CW_Actor prefab = FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<CW_Actor>();
        CW_Actor actor = (CW_Actor)__instance.newObject(prefab);
        actor.setData((ActorData)actor.base_data);

        actor.cw_asset = Library.Manager.actors.get(pStatsID);

        actor.gameObject.SetActive(true);
        __instance.finalizeActor(pStatsID, actor, pTile, pZHeight);
        actor.newCreature();
        actor.cw_newCreature();
        actor.cw_finalize();

        __result = actor;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActorManager), nameof(ActorManager.spawnPopPoint))]
    public static bool spawnPopPoint_patch(ActorManager __instance, ActorData pData, WorldTile pTile, City pCity,
        ref Actor __result)
    {
        ActorAsset asset = AssetManager.actor_library.get(pData.asset_id);
        if (asset == null)
        {
            __result = null;
            return false;
        }

        CW_Actor prefab = FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<CW_Actor>();
        CW_Actor actor = ActorManager_base_loadObject(__instance, pData, prefab);
        actor.setData(pData);

        // 修正血脉，移除不存在的血脉, 由City produce new unit产生.
        Dictionary<string, float> blood_nodes = actor.data.GetBloodNodes();
        actor.data.set(DataS.blood_nodes, "");
        if (blood_nodes != null)
        {
            List<string> keys = blood_nodes.Keys.ToList();
            foreach (string blood_node_id in keys)
            {
                BloodNodeAsset blood_node = Library.Manager.bloods.get(blood_node_id);
                if (blood_node == null)
                {
                    blood_nodes.Remove(blood_node_id);
                }
            }

            actor.data.SetBloodNodes(blood_nodes);
        }

        // 传承功法
        actor.data.get(DataS.cultibook_id, out string cultibook_id, "");
        if (!string.IsNullOrEmpty(cultibook_id))
        {
            if (Library.Manager.cultibooks.contains(cultibook_id))
            {
                Library.Manager.cultibooks.get(cultibook_id).increase();
            }
            else
            {
                actor.data.set(DataS.cultibook_id, "");
            }
        }


        actor.cw_asset = Library.Manager.actors.get(pData.asset_id);
        actor.cw_finalize();

        actor.gameObject.SetActive(true);
        __instance.finalizeActor(asset.id, actor, pTile);
        pCity.addNewUnit(actor);
        __result = actor;

        return false;
    }

    private static CW_Actor ActorManager_base_loadObject(ActorManager instance, ActorData data, CW_Actor prefab)
    {
        CW_Actor tobject = Object.Instantiate(prefab);
        BaseSimObject baseSimObject = tobject;
        int latest_hash = instance._latest_hash;
        instance._latest_hash = latest_hash + 1;
        baseSimObject.setHash(latest_hash);
        tobject.loadData(data);
        instance.addObject(tobject);
        return tobject;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActorManager), nameof(ActorManager.loadObject))]
    public static bool loadObject_patch(ActorManager __instance, ActorData pData, Actor pPrefab, ref Actor __result)
    {
        __result = null;
        if (__instance.dict.ContainsKey(pData.id))
        {
            Logger.Log("Trying to load unit with same ID, that already is loaded. " + pData.id);
            return false;
        }

        WorldTile tile = World.world.GetTile(pData.x, pData.y);
        if (tile == null)
        {
            return false;
        }

        ActorAsset actorAsset = AssetManager.actor_library.get(pData.asset_id);
        if (actorAsset == null)
        {
            return false;
        }

        int num = pData.health;
        CW_Actor actor_prefab = FastVisit.get_actor_prefab("actors/" + actorAsset.prefab).GetComponent<CW_Actor>();
        Actor actor2 = ActorManager_base_loadObject(__instance, pData, actor_prefab);
        CW_Actor cw_actor = (CW_Actor)actor2;
        cw_actor.cw_asset = Library.Manager.actors.get(actorAsset.id);

        actor2.setData(pData);
        if (actor2.data.GetElement().BaseElements[0] < 0)
        {
            cw_actor.cw_newCreature();
        }

        actor2.gameObject.SetActive(true);

        __instance.finalizeActor(actorAsset.id, actor2, tile);
        if (actor2.asset.use_items)
        {
            actor2.equipment.load(pData.items);
        }

        if (actor2.asset.unit)
        {
            actor2.reloadInventory();
        }

        actor2.loadFromSave();
        actor2.updateStats();
        num = Mathf.Clamp(num, 1, actor2.getMaxHealth());
        actor2.data.health = num;
        return actor2;
    }

    #endregion

    #region 实现状态效果相关函数的替换跳转

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BaseSimObject), nameof(BaseSimObject.hasStatus))]
    public static bool hasStatus_prefix(BaseSimObject __instance, string pID, ref bool __result)
    {
        if (__instance.a != null)
        {
            __result = ((CW_Actor)__instance.a).HasStatus(pID);
        }
        else if (__instance.b != null)
        {
            return true;
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BaseSimObject), nameof(BaseSimObject.hasAnyStatusEffect))]
    public static bool hasAnyStatusEffect_prefix(Actor __instance, ref bool __result)
    {
        if (__instance.a != null)
        {
            __result = ((CW_Actor)__instance.a).HasAnyStatusEffect();
        }
        else if (__instance.b != null)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region 攻击机制补充

    /// <summary>
    ///     法术释放
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Actor), nameof(Actor.tryToAttack))]
    public static bool tryToAttack_prefix(Actor __instance, BaseSimObject pTarget, ref bool pDoChecks,
        ref bool __result)
    {
        if (((CW_Actor)__instance).__data_spells.Count == 0) return true;

        __result = false;
        bool can_continue_check = true;
        if (pDoChecks)
        {
            pDoChecks = false;
            if (__instance.isInLiquid() && !__instance.asset.oceanCreature) return false;
            if (!__instance.isAttackReady())
            {
                can_continue_check = false;
            }

            if ((__instance.s_attackType == WeaponType.Melee && pTarget.zPosition.y > 0) ||
                !__instance.isInAttackRange(pTarget))
            {
                can_continue_check = false;
            }
        }

        CW_Actor actor = (CW_Actor)__instance;
        CW_SpellAsset spell = Library.Manager.spells.get(actor.__data_spells.GetRandom());

        if (spell.can_trigger(SpellTriggerTag.ATTACK) && actor.CastSpell(spell, pTarget, pTarget.currentTile))
        {
            actor.timer_action = actor.s_attackSpeed_seconds;
            actor.attackTimer = actor.s_attackSpeed_seconds;
            // 播放法术声音
            // 简单的释放动作
            __result = true;
            can_continue_check = false;
        }

        return can_continue_check;
    }

    /// <summary>
    ///     攻击后触发状态效果
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Actor), nameof(Actor.tryToAttack))]
    public static void tryToAttack_postfix(Actor __instance, BaseSimObject pTarget, bool __result)
    {
        if (!__result) return;
        CW_Actor actor = (CW_Actor)__instance;
        if (actor.statuses == null || actor.statuses.Count == 0) return;

        List<CW_StatusEffectData> list = Factories.status_list_factory.get_next();
        list.AddRange(actor.statuses.Values);
        foreach (CW_StatusEffectData status in list.Where(status => !status.finished))
        {
            status.status_asset.action_on_attack?.Invoke(status, pTarget, actor);
        }

        list.Clear();
    }

    #endregion
}