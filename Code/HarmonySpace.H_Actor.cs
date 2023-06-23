﻿using Cultivation_Way.Extension;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine.Tilemaps;
using Cultivation_Way.Library;
using Cultivation_Way.Core;

namespace Cultivation_Way.HarmonySpace
{
    internal static class H_Actor
    {
        #region 人物属性更新 目的在于应用模组中新的人物属性加成
        /**原版中采用dirty标记来判断是否需要更新人物属性
         * 当需要更新时, 会调用updateStats函数
         * 这里拦截updateStats函数, 
         * 通过HarmonyTranspiler在原版代码
         * 
         * <code>
         * test = S.attack_speed;
         * baseStats[text] += (float)(this.data.level-1);
         * //**此处插入代码
         * if (base.hasAnyStatusEffect())
         * </code>
         * 
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
            codes.Insert(499, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(H_Actor), nameof(cw_updateStats))));
            return codes;
        }
        private static void cw_updateStats(Actor actor)
        {
            // 载入修炼体系的加成
            int[] cultisys_levels = actor.data.get_cultisys_level();
            for(int i = 0; i < cultisys_levels.Length; i++)
            {
                if (cultisys_levels[i] < 0) continue;
                CultisysAsset cultisys = Library.Manager.cultisys.list[i];
                actor.stats.mergeStats(cultisys.get_bonus_stats((CW_Actor)actor, cultisys_levels[i]));
            }
            // 载入功法的加成
            Cultibook cultibook = actor.data.get_cultibook();
            if(cultibook!=null) actor.stats.mergeStats(cultibook.bonus_stats);
            // 载入灵根的加成
            actor.stats.mergeStats(actor.data.get_element().comp_bonus_stats());
            // 载入血脉的加成
            BloodNodeAsset main_blood_node = actor.data.get_main_blood();
            if (main_blood_node != null)
            {
                if(main_blood_node.id == actor.data.id)
                {
                    // 记录属性到血脉
                    main_blood_node.ancestor_stats.clear();
                    main_blood_node.ancestor_stats.mergeStats(actor.stats);
                }
                else
                {
                    // TODO: 添加血脉纯度对属性加成的影响
                    actor.stats.max(main_blood_node.ancestor_stats, 0.6f);
                }
            }
        }
        #endregion
        #region 人物转换 目的在于将游戏使用的所有Actor转换为CW_Actor

        /**游戏中Actor均由ActorManager创建，
         * Actor的GameObject存在两个创建的函数
         * 1. <see cref="ActorManager.createNewUnit(string, WorldTile, float)"/>
         * 2. <see cref="ActorManager.spawnPopPoint(ActorData, WorldTile, City)"/>
         * 此处将两个函数均拦截, 操作与原版操作一致，但将Actor替换成经复制得到的CW_Actor
         * 以外，调用了cw_newCreature函数，用于初始化修炼相关的数据
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorManager), "createNewUnit")]
        public static bool createNewUnit_patch(ActorManager __instance, string pStatsID, WorldTile pTile, float pZHeight, ref Actor __result)
        {
            ActorAsset asset = AssetManager.actor_library.get(pStatsID);
            if (asset == null)
            {
                __result = null;
                return false;
            }
            //Logger.Log($"createNewUnit: {pStatsID} with prefab {asset.prefab}");
            Core.CW_Actor prefab = Others.FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<Core.CW_Actor>();
            Core.CW_Actor actor = (Core.CW_Actor)__instance.newObject(prefab);
            actor.setData((ActorData)actor.base_data);

            actor.cw_asset = Library.Manager.actors.get(pStatsID);

            __instance.finalizeActor(pStatsID, actor, pTile, pZHeight);
            actor.newCreature();
            actor.cw_newCreature();

            actor.m_gameObject.SetActive(true);
            __result = actor;
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorManager), "spawnPopPoint")]
        public static bool spawnPopPoint_patch(ActorManager __instance, ActorData pData, WorldTile pTile, City pCity, ref Actor __result)
        {
            ActorAsset asset = AssetManager.actor_library.get(pData.asset_id);
            Core.CW_Actor prefab = Others.FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<Core.CW_Actor>();
            Core.CW_Actor actor = (Core.CW_Actor)__instance.loadObject(pData, prefab);
            actor.setData(pData);

            actor.cw_asset = Library.Manager.actors.get(pData.asset_id);

            __instance.finalizeActor(asset.id, actor, pTile, 0f);
            pCity.addNewUnit(actor, true);
            __result = actor;

            return false;
        }
        #endregion
    }
}