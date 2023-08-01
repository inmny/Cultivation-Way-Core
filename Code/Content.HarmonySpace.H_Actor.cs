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
        if (__instance.isProfession(UnitProfession.Baby) &&
            __instance.asset.years_to_grow_to_adult <= __instance.data.getAge())
        {
            __instance.setProfession(UnitProfession.Unit);
            __instance.dirty_sprite_main = true;
            __instance.dirty_sprite_head = true;
            __instance.dirty_sprite_item = true;
            __instance.checkSpriteToRender();
        }

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

        CW_EnergyMapTile energy_tile = actor.currentTile.get_energy_tile(Content_Constants.energy_wakan_id);
        if (energy_tile == null) return;

        float culti_wakan = actor.cw_asset.culti_velo * energy_tile.density * (1 + actor.stats[CW_S.mod_cultivelo]) *
                            Content_Constants.immortal_base_cultivelo * actor.data.get_element().get_type().rarity;

        culti_wakan = culti_wakan > energy_tile.value ? energy_tile.value : culti_wakan;

        if (wakan + culti_wakan >= max_wakan)
        {
            wakan = max_wakan;
            culti_wakan = max_wakan - wakan;
            actor.check_level_up(Content_Constants.immortal_id);
        }
        else
        {
            wakan += culti_wakan;
        }

        energy_tile.value -= culti_wakan;

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