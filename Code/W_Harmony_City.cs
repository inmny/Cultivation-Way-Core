﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Cultivation_Way.Utils;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_City
    {
        private static int parent_idx = 0;
        internal static CW_ActorData tmp_data;
        // build cw city
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "create")]
        public static bool city_create(City __instance)
        {
            __city_create(__instance);
            return false;
        }
        // kill random pop points
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "killRandomPopPoints")]
        public static bool city_killRandomPopPoints(City __instance, int pAmount, ref int __result)
        {
            __city_killRandomPopPoints(__instance, pAmount, ref __result);
            return false;
        }
        // update pop points
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "updatePopPoints")]
        public static bool city_updatePopPoints(City __instance)
        {
            __city_updatePopPoints(__instance);
            return false;
        }
        // add pop point harmony is not needed for the below function
        // produce new citizen
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ai.behaviours.CityBehProduceUnit), "produceNewCitizen")]
        public static bool city_beh_produceNewCitizen(Building pBuilding, City pCity, ref bool __result)
        {
            __result = __city_beh_produceNewCitizen(pBuilding, pCity);
            return false;
        }
        // get actor data pop point
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "getActorDataPopPoint")]
        public static bool city_getActorDataPopPoint(City pCity, ref ActorData __result)
        {
            CW_CityData cw_city_data = (CW_CityData)CW_City.get_data(pCity);
            if (cw_city_data.popPoints.Count == 0)
            {
                __result = null;
                return false;
            }
            if (Others.CW_Constants.is_debugging && cw_city_data.popPoints.Count != cw_city_data.cw_pop_points.Count) throw new Exception("Pop points Not match"); 
            int random_idx = Toolbox.randomInt(0, cw_city_data.popPoints.Count);
            __result = cw_city_data.popPoints[random_idx];
            tmp_data = cw_city_data.cw_pop_points[random_idx];
            cw_city_data.popPoints.RemoveAtSwapBack(__result);
            cw_city_data.cw_pop_points.RemoveAtSwapBack(tmp_data);
            return false;
        }
        private static void __city_create(City __instance)
        {
            CW_City.set_created(__instance, true);
            __instance.setWorld();
            CW_City.set_data(__instance, new CW_CityData(__instance));
            __instance.city_hash_id = City.LAST_CITY_HASH_ID++;
            CW_City.func_createAI(__instance);
            __instance.setStatusDirty();
        }
        private static void __city_killRandomPopPoints(City __instance, int pAmount, ref int __result)
        {
            CW_CityData cw_city_data = (CW_CityData)CW_City.get_data(__instance);

            if (cw_city_data.popPoints.Count == 0) {  __result = 0; return; }

            if (pAmount == 1)
            {
                if (cw_city_data.popPoints.Count > 1000) pAmount += cw_city_data.popPoints.Count / 100;
                else if (cw_city_data.popPoints.Count > 100) pAmount += cw_city_data.popPoints.Count / 10;
            }
            int count = 0; int tmp;
            while(count < pAmount && cw_city_data.popPoints.Count > 0)
            {
                count++;
                tmp = Toolbox.randomInt(0, cw_city_data.popPoints.Count);

                Library.CW_Asset_CultiBook cultibook = Library.CW_Library_Manager.instance.cultibooks.get(cw_city_data.cw_pop_points[tmp].cultibook_id);
                if (cultibook != null)
                {
                    cultibook.cur_culti_nr--;
                    if (cultibook.cur_culti_nr <= 0) cultibook.try_deprecate();
                }

                cw_city_data.popPoints.Swap(tmp, cw_city_data.popPoints.Count - 1);
                cw_city_data.cw_pop_points.Swap(tmp, cw_city_data.popPoints.Count - 1);
                cw_city_data.popPoints.RemoveAt(cw_city_data.popPoints.Count - 1);
                cw_city_data.cw_pop_points.RemoveAt(cw_city_data.popPoints.Count);
            }
            __result = count;
        }
        private static void __city_updatePopPoints(City __instance)
        {
            CW_CityData cw_city_data = (CW_CityData)CW_City.get_data(__instance);
            if (cw_city_data.popPoints.Count == 0) return;
            int idx = 0;
            while(idx < cw_city_data.popPoints.Count)
            {
                if (W_Harmony_Actor.__new_updateAge(cw_city_data.popPoints[idx].status, cw_city_data.cw_pop_points[idx].status))
                {
                    idx++;
                }
                else
                {
                    Library.CW_Asset_CultiBook cultibook = Library.CW_Library_Manager.instance.cultibooks.get(cw_city_data.cw_pop_points[idx].cultibook_id);
                    if (cultibook != null)
                    {
                        cultibook.cur_culti_nr--;
                        if (cultibook.cur_culti_nr <= 0) cultibook.try_deprecate();
                    }

                    cw_city_data.popPoints.RemoveAt(idx);
                    cw_city_data.cw_pop_points.RemoveAt(idx);
                }
            }
        }
        private static bool __city_beh_produceNewCitizen(Building building, City city)
        {
            ModState.instance.load_unit_reason = Load_Unit_Reason.CITY_SPAWN;
            List<Actor> units = city.units.getSimpleList();
            if (units.Count < 2) return false;


            CW_Actor main_parent = get_random_parent(units);
            
            if (main_parent == null || (main_parent.haveTrait("infected") && Toolbox.randomBool())) return false;
            CW_Actor second_parent = get_random_parent(units, main_parent);

            if (second_parent == null || (second_parent.haveTrait("infected") && Toolbox.randomBool())) return false;

            CW_CityData cw_city_data = (CW_CityData)CW_City.get_data(city);

            ResourceAsset foodItem = CW_City.func_getFoodItem(city, main_parent.fast_data.favoriteFood);

            main_parent.consumeCityFoodItem(foodItem);
            city.status.housingFree--;
            cw_city_data.born++;
            Kingdom kingdom = MapBox.instance.kingdoms.getKingdomByID(cw_city_data.kingdomID);
            if (kingdom != null) kingdom.born++;

            ActorStats stats = main_parent.stats;

            ActorData actorData = new ActorData();
            actorData.cityID = cw_city_data.cityID;

            actorData.status = new ActorStatus();
            actorData.status.statsID = stats.id;

            Race race = AssetManager.raceLibrary.get(main_parent.stats.race);
            ActorBase.generateCivUnit(main_parent.stats, actorData.status, race);

            actorData.status.generateTraits(stats, race);
            CW_ActorStatus.actorstatus_inheritTraits(actorData.status, main_parent.fast_data.traits);
            CW_ActorStatus.actorstatus_inheritTraits(actorData.status, second_parent.fast_data.traits);

            actorData.status.hunger = stats.maxHunger / 2;

            actorData.status.skin = ai.ActorTool.getBabyColor(main_parent, second_parent);
            actorData.status.skin_set = main_parent.fast_data.skin_set;

            Culture babyCulture = getBabyCulture(main_parent, second_parent);
            if (babyCulture != null)
            {
                actorData.status.culture = babyCulture.id;
                actorData.status.level = babyCulture.getBornLevel();
            }
            city.addPopPoint(actorData);
            cw_city_data.cw_pop_points.Add(CW_Actor.procrete(main_parent, second_parent));

            main_parent.add_child(actorData.status);
            second_parent.add_child(actorData.status);
            return true;
        }
        private static CW_Actor get_random_parent(List<Actor> pList, CW_Actor actor_to_ignore = null)
        {
            if (parent_idx > pList.Count) parent_idx = 0;

            for (; parent_idx < pList.Count; parent_idx++)
            {
                pList.ShuffleOne(parent_idx);
                CW_Actor actor = (CW_Actor)pList[parent_idx];
                if (actor.fast_data.alive && !(actor == actor_to_ignore) && (actor_to_ignore==null || actor.fast_data.gender != actor_to_ignore.fast_data.gender)&& !actor.haveTrait("plague") && actor.fast_data.age > actor.stats.maxAge * 18 / 150)
                {
                    return actor;
                }
            }
            return null;
        }
        private static Culture getBabyCulture(CW_Actor main_parent, CW_Actor second_parent)
        {
            string text = main_parent.fast_data.culture;
            string text2 = second_parent.fast_data.culture;
            if (string.IsNullOrEmpty(text))
            {
                City city = main_parent.city;
                text = ((city != null) ? city.getCulture().id : null);
            }
            if (string.IsNullOrEmpty(text2))
            {
                City city2 = second_parent.city;
                text2 = ((city2 != null) ? city2.getCulture().id : null);
            }
            Culture culture = main_parent.currentTile.zone.culture;

            if (culture != null && culture.race == main_parent.stats.race && Toolbox.randomChance(culture.stats.culture_spread_convert_chance.value))
            {
                text = culture.id;
            }
            if (Toolbox.randomBool())
            {
                return MapBox.instance.cultures.get(text);
            }
            return MapBox.instance.cultures.get(text2);
        }
    }
}
