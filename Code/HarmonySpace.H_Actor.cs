using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Cultivation_Way.HarmonySpace
{
    internal static class H_Actor
    {
        #region 人物属性更新
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static IEnumerable<CodeInstruction> updateStats_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            codes.Insert(111, new CodeInstruction(OpCodes.Ldarg_0));
            codes.Insert(112, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(H_Actor), nameof(cw_updateStats))));
            return codes;
        }
        private static void cw_updateStats(Actor actor)
        {
            
        }
        #endregion
        #region 人物转换
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorManager), "createNewUnit")]
        public static bool createNewUnit_patch(ActorManager __instance, string pStatsID, WorldTile pTile, float pZHeight, ref Actor __result)
        {
            __result = __create_new_unit(__instance, pStatsID, pTile, pZHeight);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorManager), "spawnPopPoint")]
        public static bool spawnPopPoint_patch(ActorManager __instance, ActorData pData, WorldTile pTile, City pCity, ref Actor __result)
        {
            __result = __spawn_pop_point(__instance, pData, pTile, pCity);
            return false;
        }

        private static Actor __spawn_pop_point(ActorManager instance, ActorData pData, WorldTile pTile, City pCity)
        {
            ActorAsset asset = AssetManager.actor_library.get(pData.asset_id);
            Core.CW_Actor prefab = Others.FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<Core.CW_Actor>();
            Core.CW_Actor actor = (Core.CW_Actor)instance.loadObject(pData, prefab);
            actor.setData(pData);

            actor.cw_asset = Library.Manager.actors.get(pData.asset_id);

            instance.finalizeActor(asset.id, actor, pTile, 0f);
            pCity.addNewUnit(actor, true);
            return actor;
        }

        private static Actor __create_new_unit(ActorManager instance, string asset_id, WorldTile tile, float z_height)
        {
            ActorAsset asset = AssetManager.actor_library.get(asset_id);
            if (asset == null)
            {
                return null;
            }
            Core.CW_Actor prefab = Others.FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<Core.CW_Actor>();
            Core.CW_Actor actor = (Core.CW_Actor)instance.newObject(prefab);
            actor.setData((ActorData)actor.base_data);

            actor.cw_asset = Library.Manager.actors.get(asset_id);

            instance.finalizeActor(asset_id, actor, tile, z_height);
            actor.newCreature();
            actor.cw_newCreature();
            return actor;
        }
        #endregion
    }
}
