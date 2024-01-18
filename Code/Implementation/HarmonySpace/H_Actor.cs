using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using HarmonyLib;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.Implementation.HarmonySpace;

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

        actor.data.get(Content_Constants.blood_id, out var level, -1);
        if (level >= 0)
        {
            UpdateBlood(actor, level);
            actor.CheckLevelUp(Content_Constants.blood_id);
        }

        actor.data.get(Content_Constants.immortal_id, out level, -1);

        if (level >= 0)
        {
            actor.data.get(DataS.wakan, out float wakan);
            float max_wakan = actor.stats[CW_S.wakan];

            if (wakan >= max_wakan)
            {
                actor.CheckLevelUp(Content_Constants.immortal_id);
                goto BUSHIDO_CHECK;
            }

            CW_EnergyMapTile energy_tile = actor.currentTile.GetEnergyTile(Content_Constants.energy_wakan_id);
            if (energy_tile == null)
            {
                goto BUSHIDO_CHECK;
            }

            float culti_wakan = actor.cw_asset.culti_velo * Mathf.Pow(10, energy_tile.density) *
                                (1 + actor.stats[CW_S.mod_cultivelo]) *
                                Content_Constants.immortal_base_cultivelo *
                                actor.data.GetElement().GetElementType().rarity *
                                Cultisyses.immortal_power_co[level];

            culti_wakan = culti_wakan > energy_tile.value ? energy_tile.value : culti_wakan;

            max_wakan *= Cultisyses.immortal_power_co[level];
            wakan *= Cultisyses.immortal_power_co[level];
            if (wakan + culti_wakan >= max_wakan)
            {
                wakan = max_wakan;
                culti_wakan = max_wakan - wakan;
            }
            else
            {
                wakan += culti_wakan;
            }

            energy_tile.value -= culti_wakan;

            wakan /= Cultisyses.immortal_power_co[level];
            actor.data.set(DataS.wakan, wakan);
            actor.CheckLevelUp(Content_Constants.immortal_id);
        }

        BUSHIDO_CHECK:
        actor.data.get(Content_Constants.bushido_id, out level, -1);
        if (level >= 0)
        {
            float health = actor.data.health;
            float max_health = actor.stats[S.health];

            if (health >= max_health * 0.95f)
            {
                actor.CheckLevelUp(Content_Constants.bushido_id);
                goto SOUL_CHECK;
            }

            actor.data.get(Content_Constants.data_bushido_cultivelo, out float culti_health, 1);
            culti_health *= actor.cw_asset.culti_velo * (1 + actor.stats[CW_S.mod_cultivelo]) *
                            Content_Constants.bushido_base_cultivelo;

            if (health + culti_health >= max_health)
            {
                health = max_health;
            }
            else
            {
                health += culti_health;
            }

            actor.data.health = (int)health;

            actor.CheckLevelUp(Content_Constants.bushido_id);
        }

        SOUL_CHECK:
        actor.CheckLevelUp(Content_Constants.soul_id);
    }

    [Hotfixable]
    private static void UpdateBlood(CW_Actor pActor, int pLevel)
    {
        var blood = pActor.data.GetBloodNodes();
        if (blood == null || blood.Count == 0) return;
        var main_blood_id = pActor.data.GetMainBloodID();
        blood[main_blood_id] += Content_Constants.blood_cultivelo_co_t *
                                Mathf.Log((pLevel + 2) / (float)(pLevel + 1), Content_Constants.blood_cultivelo_co_k) /
                                (blood.Count * blood.Count);

        pActor.data.SetBloodNodes(blood);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CW_Actor), nameof(CW_Actor.leave_data))]
    public static void leaveWakan(CW_Actor __instance)
    {
        CultisysAsset cultisys = __instance.data.GetCultisys(CultisysType.WAKAN);
        if (cultisys == null) return;
        __instance.data.get(cultisys.id, out int level, -1);
        if (level < 0) return;

        if (level == cultisys.power_level.Length - 1)
        {
        }

        __instance.data.get(DataS.wakan, out float wakan);
        if (wakan <= 0) return;
        CW_EnergyMapTile energy_tile = __instance.currentTile.GetEnergyTile(Content_Constants.energy_wakan_id);
        if (energy_tile == null) return;
        energy_tile.UpdateValue(energy_tile.value +
                                wakan * Mathf.Pow(cultisys.power_base, cultisys.power_level[level]));
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