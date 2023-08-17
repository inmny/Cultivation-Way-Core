using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Save;

// 0.14中, 存档数据为完整游戏数据, 需要单独加载
internal class SavedDataVer1 : AbstractSavedData
{
    public class OldBaseStats
    {
        public float personality_aggression;
        public float personality_administration;
        public float personality_diplomatic;
        public float personality_rationality;
        public int diplomacy;
        public int warfare;
        public int stewardship;
        public int intelligence;
        public int army;
        public int cities;
        public int zones;
        public int bonus_towers;
        public float s_crit_chance;
        public int damage;
        public float speed;
        public int health;
        public int armor;
        public float dodge;
        public float accuracy;
        public int targets;
        public int projectiles;
        public float crit;
        public float damageCritMod;
        public float range;
        public float size;
        public float areaOfEffect;
        public float attackSpeed;
        public float knockback;
        public int loyalty_traits;
        public int loyalty_mood;
        public int opinion;
        public float knockbackReduction;
        public float mod_health;
        public float mod_damage;
        public float mod_armor;
        public float mod_crit;
        public float mod_diplomacy;
        public float mod_speed;
        public float mod_attackSpeed;
        public float scale;
        public float mod_supply_timer;
    }

    public class CW_BaseStats
    {
        public int shield;
        public float mod_shield;
        public int shield_regen;
        public float mod_shield_regen;
        public float soul;
        public float mod_soul;
        public float soul_regen;
        public float mod_soul_regen;
        public int age_bonus;
        public float mod_age;
        public float spell_range;
        public float mod_spell_range;
        public float vampire;
        public float anti_injury;
        public int spell_armor;
        public float mod_spell_armor;
        public int health_regen;
        public float mod_health_regen;
        public int wakan;
        public float mod_wakan;
        public int wakan_regen;
        public float mod_wakan_regen;
        public float mod_cultivation;
        public OldBaseStats base_stats;
    }

    public class CW_ActorData
    {
        public int[] cultisys_level;
        public uint cultisys;
        public string cultibook_id;
        public CW_Element element;
        public List<string> spells;
        public int cultisys_to_save;
    }

    public class CW_MapChunkData
    {
        public float wakan;
        public float wakan_level;
    }

    public class CW_CultiBookAsset
    {
        public string id;
        public string name = "无名";
        public string content;
        public string author_name = "佚名";
        public string author_id;
        public int cur_culti_nr;
        public int histroy_culti_nr;
        public int order;
        public int level;
        public float culti_promt;
        public string[] spells;
        public CW_BaseStats bonus_stats;
    }

    public int world_width;
    public int world_height;
    public MapStats map_stats;
    public WorldLaws world_laws;
    public string tile_str;
    public List<string> tile_map = new();
    public int[][] tile_array;
    public int[][] tile_amounts;
    public List<int> fire = new();
    public List<int> conway_eater = new();
    public List<int> conway_creator = new();
    public List<WorldTileData> tiles = new();
    public List<CityData> cities = new();
    public List<ActorDataObsolete> actor_datas = new();
    public List<CW_ActorData> cw_actor_datas = new();
    public List<BuildingData> building_datas = new();
    public List<KingdomData> kingdoms = new();
    public List<DiplomacyRelationData> relations = new();
    public List<CultureData> cultures = new();
    public List<CW_MapChunkData> chunks = new();
    public List<CW_CultiBookAsset> cultibooks = new();

    public override void load_to_world(SaveManager save_manager, SavedMap origin_data)
    {
        SavedMap converted_data = convert();
        save_manager.data = converted_data;
        if (save_manager.data.saveVersion < 12)
        {
            save_manager.convertOldAges();
            load_cw_actor_data(converted_data.actors_data);
        }

        if (save_manager.data.saveVersion < 13)
        {
            save_manager.checkOldBuildingID();
        }

        World.world.addClearWorld(origin_data.width, origin_data.height);
        SmoothLoader.add(delegate { clear_cw_world(); }, "Clearing CW Map");
        SmoothLoader.add(delegate
        {
            World.world.setMapSize(origin_data.width, origin_data.height);
            World.world.mapStats = origin_data.mapStats;
            World.world.mapStats.load();
            World.world.worldLaws = origin_data.worldLaws;
            World.world.eraManager.loadEra(origin_data.mapStats.era_id, origin_data.mapStats.era_next_id);
        }, "Setting Map Size");
        if (origin_data.saveVersion > 0 && origin_data.saveVersion < 8)
        {
            SmoothLoader.add(delegate { save_manager.loadTiles(origin_data.tileString); },
                "Loading Very Old Tiles. Like super old. Maybe you should like, re-save your world?");
        }
        else if (origin_data.saveVersion > 7)
        {
            SmoothLoader.add(delegate { save_manager.loadTileArray(origin_data); }, "Loading Tiles");
            SmoothLoader.add(delegate { save_manager.loadFrozen(origin_data.frozen_tiles); }, "Loading Frozen");
            SmoothLoader.add(delegate { save_manager.loadFire(origin_data.fire); }, "Loading Fires");
            SmoothLoader.add(delegate { save_manager.loadConway(origin_data.conwayEater, origin_data.conwayCreator); },
                "Loading Conway");
        }
        else
        {
            SmoothLoader.add(save_manager.loadOldTiles, "Loading Old Tiles");
        }

        SmoothLoader.add(delegate
        {
            CW_EnergyMapManager manager = CW_Core.mod_state.energy_map_manager;
            int width = World.world.tilesMap.GetLength(0);
            int height = World.world.tilesMap.GetLength(1);
            foreach (string energy_id in manager.maps.Keys)
            {
                manager.maps[energy_id].init(width, height);
                if (energy_id != "cw_energy_wakan") continue;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        manager.maps[energy_id].map[x, y].value = chunks[x / 16 + y / 16 * (width / 16)].wakan;
                        manager.maps[energy_id].map[x, y].density = chunks[x / 16 + y / 16 * (width / 16)].wakan_level;
                    }
                }
            }
        }, "Loading Energy Maps");
        SmoothLoader.add(delegate
        {
            foreach (CW_CultiBookAsset cultibook in cultibooks)
            {
                Manager.cultibooks.add(new Cultibook
                {
                    author_name = cultibook.author_name,
                    bonus_stats = convert_stats(cultibook.bonus_stats),
                    cur_users = 0,
                    description = cultibook.content,
                    editor_name = cultibook.author_name,
                    id = cultibook.id,
                    level = cultibook.level + cultibook.order * 10,
                    max_users = cultibook.histroy_culti_nr,
                    max_spell_nr = Math.Max((cultibook.level + cultibook.order * 10) / 4, cultibook.spells.Length),
                    name = cultibook.name,
                    spells = cultibook.spells.ToList()
                });
            }
        }, "Loading Cultibooks");
        SmoothLoader.add(save_manager.loadCultures, "Loading Cultures");
        SmoothLoader.add(save_manager.loadKingdoms, "Loading Kingdoms");
        SmoothLoader.add(save_manager.loadCities, "Loading Cities");
        SmoothLoader.add(save_manager.loadActors, "Finish Loading Actors");
        SmoothLoader.add(save_manager.checkOldCityZones, "Check Old City Zones");
        SmoothLoader.add(save_manager.loadBuildings, "Load Buildings");
        SmoothLoader.add(save_manager.setHomeBuildings, "Set Home Buildings");
        SmoothLoader.add(save_manager.loadCivs02, "Loading Civs");
        SmoothLoader.add(save_manager.loadLeaders, "Loading Leaders");
        SmoothLoader.add(save_manager.loadClans, "Loading Clans");
        SmoothLoader.add(save_manager.loadAlliances, "Loading Alliances");
        SmoothLoader.add(save_manager.loadWars, "Loading Wars");
        SmoothLoader.add(save_manager.loadPlots, "Loading Plots");
        SmoothLoader.add(save_manager.loadDiplomacy, "Loading Diplomacy");
        World.world.addUnloadResources();
        SmoothLoader.add(delegate { World.world.mapChunkManager.allDirty(); }, "Map Chunk Manager (1/2)");
        SmoothLoader.add(delegate { World.world.mapChunkManager.update(0f, true); }, "Map Chunk Manager (2/2)");
        SmoothLoader.add(save_manager.loadBoatStates, "Loading Boats. Ahoy Ahoy");
        SmoothLoader.add(delegate { World.world.finishMakingWorld(); }, "Tidying Up the World");
        World.world.addKillAllUnits();
        SmoothLoader.add(delegate { save_manager.data = null; }, "Finishing up...", false, 0.2f);
    }

    private void load_cw_actor_data(List<ActorData> actors_data)
    {
        if (actors_data == null) return;
        for (int i = 0; i < actors_data.Count; i++)
        {
            ActorData actor_data = actors_data[i];
            CW_ActorData cw_actor_data = cw_actor_datas[i];
            actor_data.set_element(cw_actor_data.element);
            if (cw_actor_data.spells != null)
            {
                foreach (string spell_id in cw_actor_data.spells)
                {
                    if (Manager.spells.contains(spell_id))
                    {
                        actor_data.add_spell(spell_id);
                    }
                }
            }

            if (!string.IsNullOrEmpty(cw_actor_data.cultibook_id) &&
                Manager.cultibooks.contains(cw_actor_data.cultibook_id))
            {
                // 由于前面将Cultibook的当前人数都设置为0, 在这里重新设置
                actor_data.set_cultibook(Manager.cultibooks.get(cw_actor_data.cultibook_id));
            }

            if (cw_actor_data.cultisys_level != null)
            {
                for (int cultisys_idx = 0; cultisys_idx < cw_actor_data.cultisys_level.Length; cultisys_idx++)
                {
                    if (cultisys_idx >= Manager.cultisys.size) break;
                    cw_actor_data.cultisys = (uint)cw_actor_data.cultisys_to_save;
                    if ((cw_actor_data.cultisys & (1 << cultisys_idx)) == 0) continue;
                    CultisysAsset cultisys = Manager.cultisys.list[cultisys_idx];
                    actor_data.set(cultisys.id,
                        Math.Min(cultisys.max_level - 1, cw_actor_data.cultisys_level[cultisys_idx]));
                }
            }
        }
    }

    private SavedMap convert()
    {
        SavedMap converted_data = new();
        // 由于该版本的actor_datas同样过时, 故无视过时警告
        converted_data.actors = actor_datas;
        converted_data.actors_data = new List<ActorData>();
        converted_data.alliances = new List<AllianceData>();
        converted_data.buildings = building_datas;
        converted_data.cities = cities;
        foreach (CityData city_data in cities)
        {
            if (city_data.race == "EasternHuman")
            {
                city_data.race = "eastern_human";
            }
            else if (city_data.race == "Yao")
            {
                city_data.race = "yao";
            }
        }

        converted_data.clans = new List<ClanData>();
        converted_data.conwayCreator = conway_creator;
        converted_data.conwayEater = conway_eater;
        converted_data.cultures = cultures;
        foreach (CultureData culture in cultures)
        {
            if (culture.race == "EasternHuman")
            {
                culture.race = "eastern_human";
            }
            else if (culture.race == "Yao")
            {
                culture.race = "yao";
            }

            Race race = AssetManager.raceLibrary.get(culture.race);
            if (culture.banner_decor_id >= race.culture_decors.Count) culture.banner_decor_id = 0;
            if (culture.banner_element_id >= race.culture_elements.Count) culture.banner_element_id = 0;
        }

        converted_data.fire = fire;
        converted_data.frozen_tiles = new List<int>();
        converted_data.height = world_height;
        converted_data.kingdoms = kingdoms;
        foreach (KingdomData kingdom_data in kingdoms)
        {
            if (kingdom_data.raceID == "EasternHuman")
            {
                kingdom_data.raceID = "eastern_human";
            }
            else if (kingdom_data.raceID == "Yao")
            {
                kingdom_data.raceID = "yao";
            }
        }

        converted_data.mapStats = map_stats;
        converted_data.plots = new List<PlotData>();
        converted_data.relations = relations;
        converted_data.saveVersion = origin_save_version;
        converted_data.tiles = tiles;
        converted_data.tileAmounts = tile_amounts;
        converted_data.tileArray = tile_array;
        converted_data.tileMap = tile_map;
        converted_data.tileString = tile_str;
        converted_data.wars = new List<WarData>();
        converted_data.width = world_width;
        converted_data.worldLaws = world_laws;
        return converted_data;
    }

    private BaseStats convert_stats(CW_BaseStats cw_base_stats)
    {
        BaseStats base_stats = new();

        base_stats[CW_S.health_regen] = cw_base_stats.health_regen;
        base_stats[CW_S.mod_cultivelo] = cw_base_stats.mod_cultivation;
        base_stats[CW_S.mod_age] = cw_base_stats.mod_age;
        base_stats[CW_S.mod_health_regen] = cw_base_stats.mod_health_regen;
        base_stats[CW_S.mod_shield_regen] = cw_base_stats.mod_shield_regen;
        base_stats[CW_S.mod_shield] = cw_base_stats.mod_shield;
        base_stats[CW_S.mod_soul] = cw_base_stats.soul;
        base_stats[CW_S.mod_soul_regen] = cw_base_stats.mod_soul_regen;
        base_stats[CW_S.mod_spell_armor] = cw_base_stats.mod_spell_armor;
        base_stats[CW_S.mod_wakan] = cw_base_stats.mod_wakan;
        base_stats[CW_S.mod_wakan_regen] = cw_base_stats.mod_wakan_regen;
        base_stats[CW_S.shield] = cw_base_stats.shield;
        base_stats[CW_S.shield_regen] = cw_base_stats.shield_regen;
        base_stats[CW_S.soul] = cw_base_stats.soul;
        base_stats[CW_S.soul_regen] = cw_base_stats.soul_regen;
        base_stats[CW_S.spell_armor] = cw_base_stats.spell_armor;
        base_stats[CW_S.spell_range] = cw_base_stats.spell_range;
        base_stats[CW_S.throns] = cw_base_stats.anti_injury;
        base_stats[CW_S.vampire] = cw_base_stats.vampire;
        base_stats[CW_S.wakan] = cw_base_stats.wakan;
        base_stats[CW_S.wakan_regen] = cw_base_stats.wakan_regen;

        FieldInfo[] fields = cw_base_stats.base_stats.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            try
            {
                base_stats[field.Name] = (float)field.GetValue(cw_base_stats.base_stats);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        return base_stats;
    }
}