using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using HarmonyLib;
using UnityEngine;

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
        codes.Insert(498, new CodeInstruction(OpCodes.Ldarg_0));
        codes.Insert(499,
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(H_Actor), nameof(cw_updateStats))));
        return codes;
    }

    private static void cw_updateStats(Actor actor)
    {
        // 载入修炼体系的加成
        int[] cultisys_levels = actor.data.get_cultisys_level();
        bool has_cultisys = false;

        for (int i = 0; i < cultisys_levels.Length; i++)
        {
            if (cultisys_levels[i] < 0) continue;
            has_cultisys = true;
            CultisysAsset cultisys = Library.Manager.cultisys.list[i];
            actor.stats.mergeStats(cultisys.get_bonus_stats((CW_Actor)actor, cultisys_levels[i]));
        }

        // 载入功法的加成
        Cultibook cultibook = actor.data.get_cultibook();
        if (cultibook != null && has_cultisys) actor.stats.mergeStats(cultibook.bonus_stats);
        // 载入灵根的加成
        actor.stats.mergeStats(actor.data.get_element().comp_bonus_stats());
        // 载入血脉的加成
        BloodNodeAsset main_blood_node = actor.data.get_main_blood();
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
            actor.stats.max(main_blood_node.ancestor_stats, purity * purity);
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

        // 清空束缚状态
        actor.data.removeFlag(DataS.is_bound);
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
        Dictionary<string, float> blood_nodes = actor.data.get_blood_nodes();
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

            actor.data.set_blood_nodes(blood_nodes);
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
        // 暂且不支持直接的血脉修炼体系
        uint allow_cultisys_types = 0b111;
        // 强制添加的修炼体系
        foreach (CultisysAsset cultisys in actor.cw_asset.force_cultisys)
        {
            if ((allow_cultisys_types & (uint)cultisys.type) == 0) continue;
            actor.data.set(cultisys.id, 0);
            allow_cultisys_types &= ~(uint)cultisys.type;
        }

        foreach (CultisysAsset cultisys in actor.cw_asset.allowed_cultisys)
        {
            if ((allow_cultisys_types & (uint)cultisys.type) == 0 || !cultisys.allow(actor, cultisys))
                continue;
            actor.data.set(cultisys.id, 0);
            allow_cultisys_types &= ~(uint)cultisys.type;
        }

        foreach (string spell_id in actor.cw_asset.born_spells)
        {
            actor.learn_spell(Library.Manager.spells.get(spell_id));
        }

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
    public static void loadObject_patch(ActorManager __instance, ActorData pData, Actor pPrefab)
    {
        // TODO:
    }

    #endregion

    #region 实现状态效果相关函数的替换跳转

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BaseSimObject), nameof(BaseSimObject.hasStatus))]
    public static bool hasStatus_prefix(BaseSimObject __instance, string pID, ref bool __result)
    {
        if (__instance.a != null)
        {
            __result = ((CW_Actor)__instance.a).has_status(pID);
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
            __result = ((CW_Actor)__instance.a).has_any_status_effect();
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

        if (spell.can_trigger(SpellTriggerTag.ATTACK) && actor.cast_spell(spell, pTarget, pTarget.currentTile))
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