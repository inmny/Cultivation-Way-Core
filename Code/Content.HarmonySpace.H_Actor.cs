using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using HarmonyLib;

namespace Cultivation_Way.Content.HarmonySpace;

internal static class H_Actor
{
    /// <summary>
    ///     按年更新仙路修炼进度
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Actor), nameof(Actor.updateAge))]
    public static void updateAge_postfix(Actor __instance)
    {
        CW_Actor actor = (CW_Actor)__instance;

        actor.data.get(Content_Constants.immortal_id, out int level, -1);

        if (level < 0) return;

        actor.data.get(DataS.wakan, out float wakan);
        float max_wakan = actor.stats[CW_S.wakan];

        if (wakan >= max_wakan)
        {
            actor.check_level_up(Content_Constants.immortal_id);
            return;
        }

        float culti_wakan = actor.cw_asset.culti_velo * (1 + actor.stats[CW_S.mod_cultivelo]) *
                            Content_Constants.immortal_base_cultivelo * actor.data.get_element().get_type().rarity;
        if (wakan + culti_wakan >= max_wakan)
        {
            wakan = max_wakan;
            actor.check_level_up(Content_Constants.immortal_id);
        }
        else
        {
            wakan += culti_wakan;
        }

        actor.data.set(DataS.wakan, wakan);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActorBase), nameof(ActorBase.getUnitTexturePath))]
    public static bool actor_getUnitTexture(ActorBase __instance, ref string __result)
    {
        if (__instance.asset.race != "yao") return true;
        CW_Actor actor = (CW_Actor)__instance;
        switch (actor.data.profession)
        {
            case UnitProfession.Baby:
                __result = __instance.asset.texture_path + "/unit_child";
                break;
            case UnitProfession.Null:
            case UnitProfession.Unit:
                __result = __instance.asset.texture_path +
                           (actor.data.gender == ActorGender.Male ? "/unit_male_1" : "/unit_female_1");
                break;
            case UnitProfession.Warrior:
                __result = __instance.asset.texture_path + "/unit_warrior_1";
                break;
            case UnitProfession.Leader:
                __result = __instance.asset.texture_path + "/unit_leader";
                break;
            case UnitProfession.King:
                __result = "races/eastern_human/unit_king";
                break;
        }

        return false;
    }
}