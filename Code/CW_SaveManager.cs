using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using Assets.SimpleZip;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way
{
    public class CW_SaveManager
    {
        internal static CW_ActorData tmp_loaded_actor_data;
        internal static CW_BuildingData tmp_loaded_building_data;
        private static List<int> banner_icon_buffer = new List<int>();
        //private static Dictionary<CityData, List<CW_ActorData>> cw_actor_data_buffer = new Dictionary<CityData, List<CW_ActorData>>();
        public static SavedMap save_to(string folder_name, bool only_compressed_data = false)
        {
            banner_icon_buffer.Clear();
            //cw_actor_data_buffer.Clear();
            Debug.Log("[CW Core]: Start Saving the World (0/12)");
            // 存储原版数据
            SavedMap origin_save = create_origin_save();
            Debug.Log("[CW Core]: Get the Origin Save Data (1/12)");
            MapMetaData origin_meta = origin_save.getMeta();
            Debug.Log("[CW Core]: Get the Origin Save Meta Data (2/12)");
            SaveManager.saveMetaData(origin_meta, folder_name);
            Debug.Log("[CW Core]: Save the Origin Save Meta Data to File (3/12)");
            string origin_save_json = origin_save.toJson();
            Debug.Log("[CW Core]: Convert the Origin Save Data to Json (4/12)");
            string path = Reflection.CallStaticMethod(typeof(SaveManager), "folderPath", folder_name) as string;
            if (!only_compressed_data)
            {
                File.WriteAllText(path + "map.wbax", origin_save_json);
            }
            File.WriteAllBytes(path + "map.wbox", Zip.Compress(origin_save_json));
            Debug.Log("[CW Core]: Save the Origin Save Data to File (5/12)");
            // 恢复城市未出生人口数据
            /**
            foreach(CityData city_data in cw_actor_data_buffer.Keys)
            {
                List<CW_ActorData> cw_pop_points = new List<CW_ActorData>();
                for(int i = 0; i < cw_actor_data_buffer[city_data].Count; i++)
                {
                    cw_pop_points.Add(cw_pop_points[i]);
                }
                ((CW_CityData)city_data).cw_pop_points = cw_pop_points;
            }
            cw_actor_data_buffer.Clear();
            */
            Debug.Log("[CW Core]: Restore City CW Pop points (6/12)");
            // 恢复至模组安装时的旗帜
            List<Kingdom> kingdoms_to_restore_banner_icon = MapBox.instance.kingdoms.list_civs;
            for(int i=0;i<kingdoms_to_restore_banner_icon.Count;i++)
            {
                kingdoms_to_restore_banner_icon[i].banner_icon_id = banner_icon_buffer[i];
            }
            banner_icon_buffer.Clear();
            Debug.Log("[CW Core]: Restore Kingdom Banner Icons (7/12)");
            // 存储修仙安装时的完整数据
            CW_SavedGameData cw_save = create_cw_save();
            Debug.Log("[CW Core]: Get the CW Save Data (8/12)");
            MapMetaData cw_meta = cw_save.getMeta();
            Debug.Log("[CW Core]: Get the CW Save Meta Data (9/12)");
            cw_meta.prepareForSave();
            File.WriteAllText(SaveManager.generateMetaPath(folder_name).Replace
                ("map.meta", "cw_map.meta"), cw_meta.toJson());
            Debug.Log("[CW Core]: Save the CW Save Meta Data to File (10/12)");
            string cw_save_json = cw_save.toJson();
            Debug.Log("[CW Core]: Covert the CW Save Data to Json (11/12)");
            if (!only_compressed_data)
            {
                File.WriteAllText(path + "cw_map.wbax", cw_save_json);
            }
            File.WriteAllBytes(path + "cw_map.wbox", Zip.Compress(cw_save_json));
            Debug.Log("[CW Core]: Save the CW Save Data to File (12/12)");
            return origin_save;
        }
        public static void load_origin_data(SavedMap origin_save)
        {
            ModState.instance.loading_save_type = Loading_Save_Type.ORIGIN;
            Reflection.SetField(MapBox.instance.saveManager, "data", origin_save);
            origin_save.worldLaws.check();
            #region 按照原版动作加载地图
            SmoothLoader.add(delegate
            {
                MapBox.instance.clearWorld();
                MapBox.instance.setMapSize(origin_save.width, origin_save.height);
                MapBox.instance.mapStats = origin_save.mapStats;
                MapBox.instance.worldLaws = origin_save.worldLaws;
            }, "Clear World");
            SmoothLoader.add(delegate
            {
                if (origin_save.saveVersion > 0 && origin_save.saveVersion < 8)
                {
                    MapBox.instance.saveManager.CallMethod("loadTiles", origin_save.tileString);
                }
                else if (origin_save.saveVersion > 7)
                {
                    MapBox.instance.saveManager.CallMethod("loadTileArray", origin_save);
                    MapBox.instance.saveManager.CallMethod("loadFire", origin_save.fire);
                    MapBox.instance.saveManager.CallMethod("loadConway", origin_save.conwayEater, origin_save.conwayCreator);
                }
                else
                {
                    MapBox.instance.saveManager.CallMethod("loadOldTiles", origin_save);
                }
            }, "Load Tiles");
            #endregion
            #region 加载默认动态库（功法、体质）
            SmoothLoader.add(delegate
            {
                CW_Library_Manager.instance.cultibooks.reset();
                CW_Library_Manager.instance.special_bodies.reset();
            }, "Set Dynamic Libraries to default");
            #endregion
            #region 正常加载文化和国家
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadCultures");
                load_origin_kingdoms(origin_save.kingdoms);
            }, "Load Cultures and Kingdoms");

            #endregion
            #region 转换并加载城市数据
            SmoothLoader.add(delegate
            {
                origin_save.cities = convert_origin_city_data(origin_save.cities);
                load_origin_cities(origin_save.cities, origin_save);
                MapBox.instance.saveManager.CallMethod("checkOldCityZones");
            }, "Convert and Load Cities' Data");
            #endregion
            int i;
            
            #region 加载并拓展人物数据, TODO: 将人物等级转换为目前修炼等阶
            int amount_to_load_each_time = 100;
            int times_to_load = Mathf.CeilToInt((float)origin_save.actors.Count / amount_to_load_each_time);
            Debug.Log(string.Format("Load time:{0}, count: {1}", times_to_load, origin_save.actors.Count));
            int cur_idx = 0;
            for (i = 0; i < times_to_load; i++)
            {
                int amount_to_load_this_time = Mathf.Min(amount_to_load_each_time, origin_save.actors.Count - cur_idx);
                int start_idx = cur_idx;
                cur_idx += amount_to_load_this_time;
                SmoothLoader.add(delegate
                {
                    ModState.instance.load_object_reason = Load_Object_Reason.LOAD_SAVES;
                    //Debug.Log(string.Format("start_idx:{0}, amount:{1}", start_idx, amount_to_load_this_time));
                    load_origin_actors(origin_save.actors, start_idx, amount_to_load_this_time);
                    ModState.instance.load_object_reason = Load_Object_Reason.SPAWN;
                }, String.Format("Load Actors ({0}/{1})", cur_idx, origin_save.actors.Count), true, 0.001f);
            }
            SmoothLoader.add(delegate
            {
                MapBox.instance.units.checkAddRemove();
            }, "Finish Loading Actors");
            #endregion
            #region 加载建筑数据 TODO: 完成数据拓展
            SmoothLoader.add(delegate
            {
                ModState.instance.load_object_reason = Load_Object_Reason.LOAD_SAVES;
                load_origin_buildings(origin_save.buildings);
                ModState.instance.load_object_reason = Load_Object_Reason.SPAWN;
            }, "Load Buildings");
            SmoothLoader.add(delegate
            {
                foreach(City city in MapBox.instance.citiesList)
                {
                    city.buildings.checkAddRemove();
                }
                Debug.LogWarning("Before Set Home Buildings");
                MapBox.instance.saveManager.CallMethod("setHomeBuildings");
            }, "Set Home Buildings");
            #endregion
            
            #region 城市与国家数据收尾
            SmoothLoader.add(delegate
            {
                Debug.LogWarning("Finish Cities and Kingdoms");
                MapBox.instance.saveManager.CallMethod("loadCivs02");
                MapBox.instance.saveManager.CallMethod("loadLeaders");
                MapBox.instance.saveManager.CallMethod("loadDiplomacy");
            }, "Finish Loading Cities and Kingdoms");
            #endregion
            MapBox.instance.addUnloadResources();
            SmoothLoader.add(delegate
            {
                ((MapChunkManager)Reflection.GetField(typeof(MapBox),MapBox.instance,"mapChunkManager")).allDirty();
            }, "Map Chunk Manager (1/2)");
            SmoothLoader.add(delegate
            {
                ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).updateDirty();
            }, "Map Chunk Manager (2/2)");
            SmoothLoader.add(delegate
            {
                World_Data.instance.map_chunk_manager.update();
            }, "World Chunk Wakan");
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadBoatStates");
            }, "Load Boat States");
            SmoothLoader.add(delegate
            {
                MapBox.instance.finishMakingWorld(true);
            }, "finishMakingWorld", false, 0.001f);
            SmoothLoader.add(delegate
            {
            }, "Finishing up...", false, 0.4f);
        }
        public static void load_cw_data(CW_SavedGameData cw_save)
        {
            ModState.instance.loading_save_type = Loading_Save_Type.CW;
            SavedMap origin_save = cw_save.get_origin_format();
            origin_save.worldLaws.check();
            Reflection.SetField(MapBox.instance.saveManager, "data", origin_save);
            
            #region 按照原版动作加载地图
            SmoothLoader.add(delegate
            {
                MapBox.instance.clearWorld();
                MapBox.instance.setMapSize(cw_save.world_width, cw_save.world_height);
                MapBox.instance.mapStats = cw_save.map_stats;
                MapBox.instance.worldLaws = cw_save.world_laws;
                Content.Harmony.W_Harmony_WorldLaw.worldLaws_init(MapBox.instance.worldLaws);
            }, "Clear World");
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadTileArray", origin_save);
                MapBox.instance.saveManager.CallMethod("loadFire", origin_save.fire);
                MapBox.instance.saveManager.CallMethod("loadConway", origin_save.conwayEater, origin_save.conwayCreator);
            }, "Load Tiles");
            #endregion
            #region 加载动态库（功法、体质）
            SmoothLoader.add(delegate
            {
                CW_Library_Manager.instance.cultibooks.load_as(cw_save.cultibooks);
                CW_Library_Manager.instance.special_bodies.load_as(cw_save.special_bodies);
            }, "Load Dynamic Libraries");
            
            #endregion
            #region 正常加载文化和国家
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadCultures");
                MapBox.instance.saveManager.CallMethod("loadKingdoms");
            }, "Load Cultures and Kingdoms");

            #endregion
            #region 加载城市数据
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadCities");
            }, "Convert and Load Cities' Data");
            #endregion
            int i;
            
            #region 加载人物数据
            int amount_to_load_each_time = 100;
            int times_to_load = Mathf.CeilToInt((float)cw_save.actor_datas.Count / amount_to_load_each_time);
            Debug.Log(string.Format("Load time:{0}, count: {1}", times_to_load, cw_save.actor_datas.Count));
            int cur_idx = 0;
            for (i = 0; i < times_to_load; i++)
            {
                int amount_to_load_this_time = Mathf.Min(amount_to_load_each_time, cw_save.actor_datas.Count - cur_idx);
                int start_idx = cur_idx;
                cur_idx += amount_to_load_this_time;
                SmoothLoader.add(delegate
                {
                    ModState.instance.load_object_reason = Load_Object_Reason.LOAD_SAVES;
                    //Debug.Log(string.Format("start_idx:{0}, amount:{1}", start_idx, amount_to_load_this_time));
                    load_actors(cw_save.actor_datas, cw_save.cw_actor_datas, start_idx, amount_to_load_this_time);
                    ModState.instance.load_object_reason = Load_Object_Reason.SPAWN;
                }, String.Format("Load Actors ({0}/{1})", cur_idx, cw_save.actor_datas.Count), true, 0.001f);
            }
            SmoothLoader.add(delegate
            {
                MapBox.instance.units.checkAddRemove();
            }, "Finish Loading Actors");
            #endregion
            #region 加载建筑数据
            SmoothLoader.add(delegate
            {
                ModState.instance.load_object_reason = Load_Object_Reason.LOAD_SAVES;
                load_buildings(cw_save.building_datas, cw_save.cw_building_datas);
                ModState.instance.load_object_reason = Load_Object_Reason.SPAWN;
            }, "Load Buildings");
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("setHomeBuildings");
            }, "Set Home Buildings");
            #endregion
            
            #region 城市与国家数据收尾
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadCivs02");
                MapBox.instance.saveManager.CallMethod("loadLeaders");
                MapBox.instance.saveManager.CallMethod("loadDiplomacy");
            }, "Finish Loading Cities and Kingdoms");
            #endregion
            MapBox.instance.addUnloadResources();
            SmoothLoader.add(delegate
            {
                ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).allDirty();
            }, "Map Chunk Manager (1/2)");
            SmoothLoader.add(delegate
            {
                ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).updateDirty();
            }, "Map Chunk Manager (2/2)");
            SmoothLoader.add(delegate
            {
                World_Data.instance.map_chunk_manager.load(cw_save.chunks);
                World_Data.instance.map_chunk_manager.update();
            }, "World Chunk Wakan");
            SmoothLoader.add(delegate
            {
                MapBox.instance.saveManager.CallMethod("loadBoatStates");
            }, "Load Boat States");
            SmoothLoader.add(delegate
            {
                MapBox.instance.finishMakingWorld(true);
            }, "finishMakingWorld", false, 0.001f);
            SmoothLoader.add(delegate
            {
            }, "Finishing up...", false, 0.4f);
        }
        private static SavedMap create_origin_save()
        {
            SavedMap origin_save = new SavedMap();
            origin_save.init();

            MapBox instance = MapBox.instance;
            origin_save.width = Config.ZONE_AMOUNT_X;
            origin_save.height = Config.ZONE_AMOUNT_Y;
            origin_save.saveVersion = Config.WORLD_SAVE_VERSION;
            origin_save.mapStats = instance.mapStats;
            origin_save.worldLaws = instance.worldLaws;
            origin_save.mapStats.population = instance.units.Count;
            List<Culture> world_cultures = instance.cultures.save();
            List<Kingdom> world_kingdoms = instance.kingdoms.save();
            List<DiplomacyRelation> world_relations = instance.kingdoms.diplomacyManager.save();
            origin_save.cultures = new List<Culture>();
            origin_save.kingdoms = new List<Kingdom>();
            origin_save.relations = new List<DiplomacyRelation>();
            int i, j;
            List<Culture> cultures_to_ignore = new List<Culture>();
            List<Kingdom> kingdoms_to_ignore = new List<Kingdom>();
            List<int> cities_to_ignore = new List<int>();
            List<DiplomacyRelation> relations_to_ignore = new List<DiplomacyRelation>();
            foreach(Culture culture in world_cultures)
            {
                if (CW_Library_Manager.instance.races.added_races.Contains(culture.race))
                {
                    cultures_to_ignore.Add(culture);
                    continue;
                }
                origin_save.cultures.Add(culture);
            }
            foreach(Kingdom kingdom in world_kingdoms)
            {
                banner_icon_buffer.Add(kingdom.banner_icon_id);
                if (cultures_to_ignore.Contains(kingdom.getCulture()) || CW_Library_Manager.instance.races.added_races.Contains(kingdom.raceID)|| CW_Library_Manager.instance.kingdoms.added_kingdoms.Contains(kingdom.id))
                {
                    kingdoms_to_ignore.Add(kingdom);
                    continue;
                }
                origin_save.kingdoms.Add(kingdom);
                if (kingdom.banner_icon_id == 0) kingdom.banner_icon_id = 1;
            }
            foreach(DiplomacyRelation relation in world_relations)
            {
                if(kingdoms_to_ignore.Contains(relation.kingdom1) || kingdoms_to_ignore.Contains(relation.kingdom2))
                {
                    relations_to_ignore.Add(relation);
                    continue;
                }
                origin_save.relations.Add(relation);
            }
            for (i = 0; i < instance.citiesList.Count; i++)
            {
                CityData data = CW_City.get_data(instance.citiesList[i]);
                bool city_to_ignore = false;
                if (CW_Library_Manager.instance.races.added_races.Contains(data.race)) city_to_ignore = true;
                foreach(Culture culture_to_ignore in cultures_to_ignore)
                {
                    if (data.culture == culture_to_ignore.id || city_to_ignore)
                    {
                        city_to_ignore = true;
                        break;
                    }
                }
                foreach(Kingdom kingdom_to_ignore in kingdoms_to_ignore)
                {
                    if(data.kingdomID == kingdom_to_ignore.id || city_to_ignore)
                    {
                        city_to_ignore = true;
                        break;
                    }
                }
                if (city_to_ignore)
                {
                    cities_to_ignore.Add(instance.citiesList[i].city_hash_id);
                    continue;
                }
                data.storage.save();
                data.zones.Clear();
                /**
                cw_actor_data_buffer[data] = new List<CW_ActorData>();
                List<CW_ActorData> pop_points = ((CW_CityData)data).cw_pop_points;
                for (j=0;j< pop_points.Count; j++)
                {
                    cw_actor_data_buffer[data].Add(pop_points[j]);
                }
                pop_points.Clear();
                ((CW_CityData)data).cw_pop_points = null;
                */
                List<TileZone> zones = Reflection.GetField(typeof(City), instance.citiesList[i], "zones") as List<TileZone>;
                for (j = 0; j < zones.Count; j++)
                {
                    data.zones.Add(new ZoneData
                    {
                        x = zones[j].x,
                        y = zones[j].y
                    });
                }
                origin_save.cities.Add(data);
            }
            origin_save.tileMap.Clear();
            origin_save.fire.Clear();
            origin_save.conwayCreator.Clear();
            origin_save.conwayEater.Clear();
            // 手动进行数据压缩，将相同类型的地块压缩成一个地块加对应的数量
            List<int[]> tile_array_list = new List<int[]>();
            List<int[]> tile_amount_list = new List<int[]>();
            int cur_amount = 0; int cur_y = 0; int cur_len = 0;
            int max_len = origin_save.width * 64;
            string last_tile_whole_id = string.Empty;
            tile_amount_list.Add(new int[max_len]);
            tile_array_list.Add(new int[max_len]);
            for (i = 0; i < instance.tilesList.Count; i++)
            {
                WorldTile tile = instance.tilesList[i];
                string cur_tile_whole_id = get_whole_tile_id(tile);
                if(cur_tile_whole_id != last_tile_whole_id || cur_y != tile.pos.y)
                {
                    if (cur_amount > 0)
                    {
                        tile_amount_list[cur_y][cur_len] = cur_amount;
                        tile_array_list[cur_y][cur_len++] = origin_save.getTileMapID(last_tile_whole_id);
                        cur_amount = 0;
                    }
                    last_tile_whole_id = cur_tile_whole_id;
                    // 切换至新行
                    if(cur_y != tile.pos.y)
                    {
                        tile_array_list[cur_y] = Toolbox.resizeArray(tile_array_list[cur_y], cur_len);
                        tile_amount_list[cur_y] = Toolbox.resizeArray(tile_amount_list[cur_y], cur_len);
                        cur_y = tile.pos.y;
                        tile_array_list.Add(new int[max_len]);
                        tile_amount_list.Add(new int[max_len]);
                        cur_len = 0;
                    }
                }
                cur_amount++;
                if (tile.data.fire) origin_save.fire.Add(tile.data.tile_id);
                if (tile.data.conwayType == ConwayType.Eater) origin_save.conwayEater.Add(tile.data.tile_id);
                if(tile.data.conwayType == ConwayType.Creator) origin_save.conwayCreator.Add(tile.data.tile_id);
            }
            // 地块数据压缩收尾
            // if(cur_amount > 0) 逻辑上cur_amount必定大于0
            tile_amount_list[cur_y][cur_len] = cur_amount;
            tile_array_list[cur_y][cur_len++] = origin_save.getTileMapID(last_tile_whole_id);
            tile_array_list[cur_y] = Toolbox.resizeArray(tile_array_list[cur_y], cur_len);
            tile_amount_list[cur_y] = Toolbox.resizeArray(tile_amount_list[cur_y], cur_len);
            origin_save.tileArray = tile_array_list.ToArray();
            origin_save.tileAmounts = tile_amount_list.ToArray();
            tile_array_list.Clear(); tile_amount_list.Clear();
            // simpleList可以重复获取，并且在游戏内容无更新的情况下保持不变
            List<Actor> actors = instance.units.getSimpleList();
            for (i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                if (!actor.base_data.alive || actor.stats.skipSave || CW_Library_Manager.instance.units.added_actors.Contains(actor.stats.id)) continue;
                if (kingdoms_to_ignore.Contains(actor.kingdom)) continue;
                if (cultures_to_ignore.Contains(actor.getCulture())) continue;
                if (actor.city != null && cities_to_ignore.Contains(actor.city.city_hash_id)) continue;
                
                ActorData actorData = new ActorData
                {
                    status = ((CW_Actor)actor).fast_data,
                    x = actor.currentTile.pos.x,
                    y = actor.currentTile.pos.y,
                    inventory = actor.inventory
                };
                if (actor.equipment != null)
                {
                    List<ItemData> dataForSave = actor.equipment.getDataForSave();
                    if (dataForSave.Count > 0)
                    {
                        actorData.items = dataForSave;
                    }
                }
                if (actor.city != null) actorData.cityID = CW_City.get_data(actor.city).cityID;
                origin_save.actors.Add(actorData);
            }

            List<Building> buildings = instance.buildings.getSimpleList();
            for (i = 0; i < buildings.Count; i++)
            {
                CW_Building building = (CW_Building)buildings[i];
                if (CW_Library_Manager.instance.buildings.added_buildings.Contains(building.cw_stats.origin_stats.id)) continue;

                if (kingdoms_to_ignore.Contains(building.kingdom)) continue;
                if (building.city != null && cities_to_ignore.Contains(building.city.city_hash_id)) continue;
                building.prepareForSave();
                origin_save.buildings.Add(((CW_Building)building).fast_data);
            }
            return origin_save;
        }
        private static CW_SavedGameData create_cw_save()
        {
            CW_SavedGameData save = new CW_SavedGameData();
            save.init();
            MapBox instance = MapBox.instance;
            save.world_width = Config.ZONE_AMOUNT_X;
            save.world_height = Config.ZONE_AMOUNT_Y;
            save.origin_save_version = Config.WORLD_SAVE_VERSION;
            save.cw_save_version = Others.CW_Constants.save_version;
            save.map_stats = instance.mapStats;
            save.world_laws = instance.worldLaws;
            save.map_stats.population = instance.units.Count;
            save.cultures = instance.cultures.list;
            save.kingdoms = instance.kingdoms.list_civs;
            save.relations = instance.kingdoms.diplomacyManager.save();
            int i, j;
            for (i = 0; i < instance.citiesList.Count; i++)
            {
                CW_CityData data = (CW_CityData)CW_City.get_data(instance.citiesList[i]);
                data.storage.save();
                data.zones.Clear();
                List<TileZone> zones = Reflection.GetField(typeof(City), instance.citiesList[i], "zones") as List<TileZone>;
                for (j = 0; j < zones.Count; j++)
                {
                    data.zones.Add(new ZoneData
                    {
                        x = zones[j].x,
                        y = zones[j].y
                    });
                }
                CityData_For_Save saved_city_data = new CityData_For_Save();
                saved_city_data.set(data);
                save.cities.Add(saved_city_data);
            }
            save.tile_map.Clear();
            save.fire.Clear();
            save.conway_eater.Clear();
            save.conway_creator.Clear();
            // 手动进行数据压缩，将相同类型的地块压缩成一个地块加对应的数量
            List<int[]> tile_array_list = new List<int[]>();
            List<int[]> tile_amount_list = new List<int[]>();
            int cur_amount = 0; int cur_y = 0; int cur_len = 0;
            int max_len = save.world_width * 64;
            string last_tile_whole_id = string.Empty;
            tile_amount_list.Add(new int[max_len]);
            tile_array_list.Add(new int[max_len]);
            for (i = 0; i < instance.tilesList.Count; i++)
            {
                WorldTile tile = instance.tilesList[i];
                string cur_tile_whole_id = get_whole_tile_id(tile);
                if (cur_tile_whole_id != last_tile_whole_id || cur_y != tile.pos.y)
                {
                    if (cur_amount > 0)
                    {
                        tile_amount_list[cur_y][cur_len] = cur_amount;
                        tile_array_list[cur_y][cur_len++] = save.get_tile_map_id(last_tile_whole_id);
                        cur_amount = 0;
                    }
                    last_tile_whole_id = cur_tile_whole_id;
                    // 切换至新行
                    if (cur_y != tile.pos.y)
                    {
                        tile_array_list[cur_y] = Toolbox.resizeArray(tile_array_list[cur_y], cur_len);
                        tile_amount_list[cur_y] = Toolbox.resizeArray(tile_amount_list[cur_y], cur_len);
                        cur_y = tile.pos.y;
                        tile_array_list.Add(new int[max_len]);
                        tile_amount_list.Add(new int[max_len]);
                        cur_len = 0;
                    }
                }
                cur_amount++;
                if (tile.data.fire) save.fire.Add(tile.data.tile_id);
                if (tile.data.conwayType == ConwayType.Eater) save.conway_eater.Add(tile.data.tile_id);
                if (tile.data.conwayType == ConwayType.Creator) save.conway_creator.Add(tile.data.tile_id);
            }
            // 地块数据压缩收尾
            // if(cur_amount > 0) 逻辑上cur_amount必定大于0
            tile_amount_list[cur_y][cur_len] = cur_amount;
            tile_array_list[cur_y][cur_len++] = save.get_tile_map_id(last_tile_whole_id);
            tile_array_list[cur_y] = Toolbox.resizeArray(tile_array_list[cur_y], cur_len);
            tile_amount_list[cur_y] = Toolbox.resizeArray(tile_amount_list[cur_y], cur_len);
            save.tile_array = tile_array_list.ToArray();
            save.tile_amounts = tile_amount_list.ToArray();
            tile_array_list.Clear(); tile_amount_list.Clear();
            // simpleList可以重复获取，并且在游戏内容无更新的情况下保持不变
            List<Actor> actors = instance.units.getSimpleList();
            for (i = 0; i < actors.Count; i++)
            {
                CW_Actor actor = (CW_Actor)actors[i];
                if (actor.stats.skipSave) continue;
                if (!actor.base_data.alive)
                {
                    MapBox.instance.destroyActor(actor);
                    continue;
                }
                actor.prepare_cw_data_for_save();
                ActorData_For_Save actorData = new ActorData_For_Save
                {
                    status = actor.fast_data,
                    x = actor.currentTile.pos.x,
                    y = actor.currentTile.pos.y,
                    inventory = actor.inventory
                };
                if (actor.equipment != null)
                {
                    List<ItemData> dataForSave = actor.equipment.getDataForSave();
                    if (dataForSave.Count > 0)
                    {
                        actorData.items = new List<CW_ItemData>();
                        foreach (ItemData item in dataForSave) actorData.items.Add((CW_ItemData)item);
                    }
                }
                if (actor.city != null) actorData.cityID = CW_City.get_data(actor.city).cityID;
                save.actor_datas.Add(actorData);
                save.cw_actor_datas.Add(actor.cw_data);
            }

            List<Building> buildings = instance.buildings.getSimpleList();
            for (i = 0; i < buildings.Count; i++)
            {
                CW_Building building = (CW_Building)buildings[i];
                building.prepare_cw_data_for_save();

                building.prepareForSave();
                save.building_datas.Add(building.fast_data);
                save.cw_building_datas.Add(building.cw_data);
            }
            save.chunks = World_Data.instance.map_chunk_manager.save();
            save.cultibooks = CW_Library_Manager.instance.cultibooks.list;
            save.special_bodies = CW_Library_Manager.instance.special_bodies.list;
            return save;
        }
        private static string get_whole_tile_id(WorldTile tile)
        {
            if (tile.top_type == null) return tile.main_type.id;
            return tile.main_type.id + ":" + tile.top_type.id;
        }
        private static List<CityData> convert_origin_city_data(List<CityData> origin_city_datas)
        {
            List<CityData> new_city_datas= new List<CityData>();
            foreach(CityData origin_data in origin_city_datas)
            {
                CW_CityData new_data = new CW_CityData(null);
                foreach (ItemData weapon in origin_data.storage.items_weapons) new_data.storage.items_weapons.Add(new CW_ItemData(weapon));
                foreach (ItemData armor in origin_data.storage.items_armor) new_data.storage.items_armor.Add(new CW_ItemData(armor));
                foreach (ItemData amulet in origin_data.storage.items_amulets) new_data.storage.items_amulets.Add(new CW_ItemData(amulet));
                foreach (ItemData ring in origin_data.storage.items_rings) new_data.storage.items_rings.Add(new CW_ItemData(ring));
                foreach (ItemData boot in origin_data.storage.items_boots) new_data.storage.items_boots.Add(new CW_ItemData(boot));
                foreach (ItemData helmet in origin_data.storage.items_helmets) new_data.storage.items_helmets.Add(new CW_ItemData(helmet));
                new_data.set_origin_data(origin_data);
                new_city_datas.Add(new_data);
            }
            return new_city_datas;
        }
        private static void load_actors(List<ActorData_For_Save> origin_datas, List<CW_ActorData> cw_datas, int start_idx, int amount)
        {
            int end_idx = start_idx + amount;
            int i;
            for (i = start_idx; i < end_idx; i++)
            {
                //Debug.Log(string.Format("Load Actor[{0}]", i));
                ActorData origin_data = origin_datas[i].get_data_for_load();
                tmp_loaded_actor_data = cw_datas[i];

                if (!origin_data.status.alive)
                {
                    clear_actor_data(origin_data, cw_datas[i]);
                    continue;
                }
                if (origin_data.status.gender == ActorGender.Unknown) origin_data.status.gender = Toolbox.randomBool() ? ActorGender.Male : ActorGender.Female;

                if ((!(origin_data.status.statsID == "livingPlants") && !(origin_data.status.statsID == "livingHouse")) || !string.IsNullOrEmpty(origin_data.status.special_graphics))
                {
                    if (!CW_Library_Manager.instance.cultibooks.has_asset(tmp_loaded_actor_data.cultibook_id)) tmp_loaded_actor_data.cultibook_id = null;
                    if (!CW_Library_Manager.instance.special_bodies.has_asset(tmp_loaded_actor_data.special_body_id)) tmp_loaded_actor_data.special_body_id = null;
                    for(int j = 0; j < tmp_loaded_actor_data.spells.Count; j++)
                    {
                        if (CW_Library_Manager.instance.spells.has_asset(tmp_loaded_actor_data.spells[j])) continue;
                        tmp_loaded_actor_data.spells.RemoveAt(j);
                        j--;
                    }
                    tmp_loaded_actor_data.cultisys &= ~((1u << CW_Library_Manager.instance.cultisys.list.Count) - 1);

                    MapBox.instance.spawnAndLoadUnit(origin_data.status.statsID, origin_data, MapBox.instance.GetTile(origin_data.x, origin_data.y));
                }
                else
                {
                    clear_actor_data(origin_data, cw_datas[i]);
                }
            }
        }
        private static void load_buildings(List<BuildingData> origin_datas, List<CW_BuildingData> cw_datas)
        {
            for (int i = 0; i < origin_datas.Count; i++)
            {
                BuildingData buildingData = origin_datas[i];
                tmp_loaded_building_data = cw_datas[i];
                if (AssetManager.buildings.get(buildingData.templateID) != null)
                {
                    MapBox.instance.CallMethod("loadBuilding", buildingData);
                }
            }
            MapBox.instance.buildings.checkAddRemove();
        }
        private static void load_origin_actors(List<ActorData> actor_datas, int start_idx, int amount)
        {
            int end_idx = start_idx + amount;
            int i, j;
            for (i = start_idx; i < end_idx; i++)
            {
                //Debug.Log(string.Format("Load Actor[{0}]", i));
                ActorData origin_data = actor_datas[i];

                if (!origin_data.status.alive) continue;
                if (origin_data.status.gender == ActorGender.Unknown) origin_data.status.gender = Toolbox.randomBool() ? ActorGender.Male : ActorGender.Female;
                if ((!(origin_data.status.statsID == "livingPlants") && !(origin_data.status.statsID == "livingHouse")) || !string.IsNullOrEmpty(origin_data.status.special_graphics))
                {
                    switch (origin_data.status.statsID)
                    {
                        case "EasternHuman":
                        case "unit_EasternHuman":
                            origin_data.status.statsID = "unit_eastern_human";
                            break;
                        case "baby_EasternHuman":
                            origin_data.status.statsID = "baby_eastern_human";
                            break;
                        default:
                            if(AssetManager.unitStats.get(origin_data.status.statsID)==null)
                            {
                                continue;
                            }
                            break;
                    }

                    CW_Actor actor = (CW_Actor)MapBox.instance.spawnAndLoadUnit(origin_data.status.statsID, origin_data, MapBox.instance.GetTile(origin_data.x, origin_data.y));
                    if(actor==null) continue;
                    j = (int)Mathf.Sqrt(origin_data.status.level);
                    while(j-->0 && (actor.cw_data.cultisys &0x1)==0)
                    {
                        actor.cw_data.element.re_random();
                        CW_Library_Manager.instance.cultisys.set_cultisys(actor);
                    }
                    if((actor.cw_data.cultisys & 0x1) != 0)
                    {
                        if(actor.fast_data.level <= 10) actor.cw_data.cultisys_level[0]  = actor.fast_data.level - 1;
                        else
                        {
                            actor.cw_data.cultisys_level[0] = 8 + (actor.fast_data.level + 9) / 10;
                        }
                        for (j = 1; j <= actor.cw_data.cultisys_level[0]; j++)
                        {
                            actor.level_up_bonus(1, 0, j);
                        }
                        actor.setStatsDirty();
                    }
                }
            }
        }
        private static void clear_actor_data(ActorData origin_data, CW_ActorData cw_data)
        {
            
        }
        private static void load_origin_kingdoms(List<Kingdom> kingdoms)
        {
            for (int i = 0; i < kingdoms.Count; i++)
            {
                Kingdom kingdom = kingdoms[i];
                kingdom.banner_background_id = -1;
                kingdom.banner_icon_id = -1;
                MonoBehaviour.print(kingdom.raceID);
                MonoBehaviour.print("Start fixing");
                switch (kingdom.raceID)
                {
                    case "EasternHuman":
                        kingdom.raceID = "eastern_human";
                        break;
                    case "Yao":
                        kingdom.raceID = "yao";
                        break;
                    case "Ming":
                        kingdom.raceID = "";
                        break;
                    case "Tian":
                        kingdom.raceID = "";
                        break;
                    default:
                        if(AssetManager.raceLibrary.get(kingdom.raceID)==null) continue;
                        break;
                }
                kingdom.asset = AssetManager.kingdoms.get(kingdom.raceID);
                Reflection.SetField(kingdom, "race", AssetManager.raceLibrary.get(kingdom.raceID));

                MapBox.instance.kingdoms.addKingdom(kingdom, true);

                kingdom.CallMethod("load1");
            }
        }
        private static void load_origin_cities(List<CityData> cities, SavedMap save)
        {
            MonoBehaviour.print("City Data Count:" + cities.Count.ToString());
            for (int i = 0; i < cities.Count; i++)
            {
                CityData cityData = cities[i];
                if (cityData.zones.Count != 0)
                {
                    if (cityData.race == "ork")
                    {
                        cityData.race = "orc";
                    }
                    else if(cityData.race == "EasternHuman")
                    {
                        cityData.race = "eastern_human";
                    }
                    else if(cityData.race == "Ming")
                    {
                        cityData.race = "ming";
                    }
                    else if(cityData.race == "Yao")
                    {
                        cityData.race = "yao";
                    }
                    else if(cityData.race == "Tian")
                    {
                        cityData.race = "tian";
                    }
                    TileZone tileZone = Content.W_Content_Helper.zone_calculator.getZone(cityData.zones[0].x, cityData.zones[0].y);
                    if (save.saveVersion < 7)
                    {
                        tileZone = MapBox.instance.saveManager.CallMethod("findZoneViaBuilding", cityData.cityID, save.buildings) as TileZone;
                    }
                    if (tileZone != null)
                    {
                        City city = MapBox.instance.buildNewCity(tileZone, cityData, null, true, null);
                        if (city != null && save.saveVersion >= 7)
                        {
                            //MonoBehaviour.print("Build City:" + cityData.cityID + ":" + city.data.cityID);
                            for (int j = 1; j < cityData.zones.Count; j++)
                            {
                                ZoneData zoneData = cityData.zones[j];
                                TileZone zone = Content.W_Content_Helper.zone_calculator.getZone(zoneData.x, zoneData.y);
                                if (zone != null)
                                {
                                    city.CallMethod("addZone", zone);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void load_origin_buildings(List<BuildingData> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                BuildingData buildingData = buildings[i];
                if (buildingData.templateID.Contains("ork"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("ork", "orc");
                }
                else if (buildingData.templateID.Contains("EasternHuman"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("EasternHuman", "eastern_human");
                }
                else if (buildingData.templateID.Contains("Ming"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("Ming", "ming");
                }
                else if (buildingData.templateID.Contains("Tian"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("Tian", "tian");
                }
                else if (buildingData.templateID.Contains("Yao"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("Yao", "yao");
                }
                
                if (AssetManager.buildings.get(buildingData.templateID) != null)
                {
                    MapBox.instance.CallMethod("loadBuilding", buildingData);
                }
            }
            MapBox.instance.buildings.checkAddRemove();
            Debug.LogWarning("Building Loaded");
        }
    }
}
