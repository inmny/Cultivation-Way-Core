using System.Collections.Generic;
using ai;
using ai.behaviours;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal static class H_City
{
    #region 城市生育与血脉继承

    /// <summary>
    ///     城市生育人口后需要从父母继承的灵根和血脉
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CityBehProduceUnit), nameof(CityBehProduceUnit.produceNewCitizen))]
    public static bool produceNewCitizen_prefix(CityBehProduceUnit __instance, Building pBuilding, City pCity,
        ref bool __result)
    {
        __result = false;
        Actor parent_1 = __instance._possibleParents.Pop();
        if (parent_1 == null)
        {
            return false;
        }

        if (!Toolbox.randomChance(parent_1.stats[S.fertility]))
        {
            return false;
        }

        Actor parent_2 = null;
        if (__instance._possibleParents.Count > 0)
        {
            parent_2 = __instance._possibleParents.Pop();
        }

        ResourceAsset foodItem = pCity.getFoodItem();
        pCity.eatFoodItem(foodItem.id);
        pCity.status.housingFree--;
        pCity.data.born++;
        if (pCity.kingdom != null)
        {
            pCity.kingdom.data.born++;
        }

        ActorData actorData = new()
        {
            created_time = BehaviourActionBase<City>.world.getCreationTime(),
            cityID = pCity.data.id,
            id = BehaviourActionBase<City>.world.mapStats.getNextId("unit"),
            skin = ActorTool.getBabyColor(parent_1, parent_2),
            skin_set = parent_1.data.skin_set
        };

        ActorAsset asset = inherit_from_parents(actorData, parent_1, parent_2);

        ActorBase.generateCivUnit(asset, actorData, AssetManager.raceLibrary.get(asset.race));
        actorData.generateTraits(asset, AssetManager.raceLibrary.get(asset.race));
        actorData.inheritTraits(parent_1.data.traits);

        actorData.hunger = asset.maxHunger / 2;
        parent_1.data.makeChild(BehaviourActionBase<City>.world.getCurWorldTime());
        if (parent_2 != null)
        {
            actorData.inheritTraits(parent_2.data.traits);
            parent_2.data.makeChild(BehaviourActionBase<City>.world.getCurWorldTime());
        }

        Culture babyCulture = CityBehProduceUnit.getBabyCulture(parent_1, parent_2);
        if (babyCulture != null)
        {
            actorData.culture = babyCulture.data.id;
            actorData.level = babyCulture.getBornLevel();
        }

        Clan clan = CityBehProduceUnit.checkGreatClan(parent_1, parent_2);
        if (clan != null)
        {
            Actor actor3 = pCity.spawnPopPoint(actorData, parent_1.currentTile);
            clan.addUnit(actor3);
        }
        else
        {
            pCity.addPopPoint(actorData);
        }

        __result = true;
        return false;
    }

    private static ActorAsset inherit_from_parents(ActorData child_data, Actor parent_1_actor, Actor parent_2_actor)
    {
        CW_Actor parent_1 = (CW_Actor)parent_1_actor;
        CW_Actor parent_2 = (CW_Actor)(parent_2_actor ?? parent_1_actor);

        ActorAsset asset = null;

        // 设置血脉
        Dictionary<string, float> parent_1_blood = parent_1.data.GetBloodNodes();
        Dictionary<string, float> parent_2_blood = parent_2.data.GetBloodNodes();

        if (parent_1_blood == null && parent_2_blood != null)
        {
            child_data.set_blood_nodes_only_save(parent_2_blood);
        }
        else if (parent_1_blood != null && parent_2_blood == null)
        {
            child_data.set_blood_nodes_only_save(parent_1_blood);
        }
        else if (parent_1_blood != null && parent_2_blood != null)
        {
            foreach (KeyValuePair<string, float> blood_node in parent_2_blood)
            {
                if (parent_1_blood.ContainsKey(blood_node.Key))
                {
                    parent_1_blood[blood_node.Key] += blood_node.Value;
                }
                else
                {
                    parent_1_blood.Add(blood_node.Key, blood_node.Value);
                }
            }

            child_data.set_blood_nodes_only_save(parent_1_blood);
        }

        BloodNodeAsset main_blood = child_data.GetMainBlood();
        if (main_blood != null) asset = AssetManager.actor_library.get(main_blood.ancestor_data.asset_id);

        asset ??= parent_1.asset;
        child_data.asset_id = asset.id;

        // 如果asset与parent_1的asset不同，那么main_blood必定不为空，且需要重新设置skin相关
        if (asset != parent_1.asset)
        {
            child_data.skin = main_blood.ancestor_data.skin;
            child_data.skin_set = main_blood.ancestor_data.skin_set;
        }

        CW_ActorAsset cw_asset = Library.Manager.actors.get(asset.id);
        // 设置灵根
        child_data.SetElement(
            Toolbox.randomChance(Constants.Others.random_element_when_inherit_chance)
                ? CW_Element.get_element_for_set_data(cw_asset.prefer_element, cw_asset.prefer_element_scale)
                : CW_Element.GetMean(parent_1.data.GetElement(), parent_2.data.GetElement()));

        // 准备传承功法
        Cultibook parent_1_cultibook = parent_1.data.GetCultibook();
        Cultibook parent_2_cultibook = parent_2.data.GetCultibook();
        if (parent_1_cultibook != null && parent_2_cultibook != null)
        {
            child_data.set(DataS.cultibook_id,
                parent_1_cultibook.level >= parent_2_cultibook.level ? parent_1_cultibook.id : parent_2_cultibook.id);
        }
        else if (parent_1_cultibook != null)
        {
            child_data.set(DataS.cultibook_id, parent_1_cultibook.id);
        }
        else if (parent_2_cultibook != null)
        {
            child_data.set(DataS.cultibook_id, parent_2_cultibook.id);
        }

        return asset;
    }

    #endregion
}