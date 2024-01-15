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
    public List<ActorDataObsolete> actor_datas = new();
    public List<BuildingData> building_datas = new();
    public List<CW_MapChunkData> chunks = new();
    public List<CityData> cities = new();
    public List<int> conway_creator = new();
    public List<int> conway_eater = new();
    public List<CW_CultiBookAsset> cultibooks = new();
    public List<CultureData> cultures = new();
    public List<CW_ActorData> cw_actor_datas = new();
    public List<int> fire = new();
    public List<KingdomData> kingdoms = new();
    public MapStats map_stats;
    public List<DiplomacyRelationData> relations = new();
    public int[][] tile_amounts;
    public int[][] tile_array;
    public List<string> tile_map = new();
    public string tile_str;
    public List<WorldTileData> tiles = new();
    public int world_height;
    public WorldLaws world_laws;

    public int world_width;

    public override void BeforeAll(SaveManager pSaveManager, SavedMap pVanillaData)
    {
        base.BeforeAll(pSaveManager, pVanillaData);
        // Load Cultibooks
        foreach (var cultibook in cultibooks)
        {
            cultibook.cur_culti_nr = 0;
            var new_spells = cultibook.spells.ToList();
            new_spells.RemoveAll(spell => !Manager.spells.contains(spell));
            cultibook.spells = new_spells.ToArray();
            Manager.cultibooks.add(new Cultibook
            {
                author_name = cultibook.author_name,
                bonus_stats = convert_stats(cultibook.bonus_stats),
                cur_users = cultibook.cur_culti_nr,
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

        // Convert Actor Data
        ConvertToNew(pVanillaData);
        pSaveManager.convertOldAges();
        pSaveManager.checkOldBuildingID();

        GenerateCW_ActorData(pVanillaData.actors_data);
    }

    private void ConvertToNew(SavedMap pVanillaData)
    {
        // 由于该版本的actor_datas同样过时, 故无视过时警告
        pVanillaData.actors = actor_datas;
        pVanillaData.actors_data = new List<ActorData>();
        pVanillaData.alliances = new List<AllianceData>();
        pVanillaData.buildings = building_datas;
        pVanillaData.cities = cities;
        foreach (var city_data in cities)
        {
            city_data.race = city_data.race switch
            {
                "EasternHuman" => "eastern_human",
                "Yao" => "yao",
                _ => city_data.race
            };
            city_data.pop_points.Clear();
        }

        pVanillaData.clans = new List<ClanData>();
        pVanillaData.conwayCreator = conway_creator;
        pVanillaData.conwayEater = conway_eater;
        pVanillaData.cultures = cultures;
        foreach (var culture in cultures)
        {
            culture.race = culture.race switch
            {
                "EasternHuman" => "eastern_human",
                "Yao" => "yao",
                _ => culture.race
            };

            var race = AssetManager.raceLibrary.get(culture.race);
            if (culture.banner_decor_id >= race.culture_decors.Count) culture.banner_decor_id = 0;
            if (culture.banner_element_id >= race.culture_elements.Count) culture.banner_element_id = 0;
        }

        pVanillaData.fire = fire;
        pVanillaData.frozen_tiles = new List<int>();
        pVanillaData.height = world_height;
        pVanillaData.kingdoms = kingdoms;
        foreach (var kingdom_data in kingdoms)
            kingdom_data.raceID = kingdom_data.raceID switch
            {
                "EasternHuman" => "eastern_human",
                "Yao" => "yao",
                _ => kingdom_data.raceID
            };

        pVanillaData.mapStats = map_stats;
        pVanillaData.plots = new List<PlotData>();
        pVanillaData.relations = relations;
        pVanillaData.tiles = tiles;
        pVanillaData.tileAmounts = tile_amounts;
        pVanillaData.tileArray = tile_array;
        pVanillaData.tileMap = tile_map;
        pVanillaData.tileString = tile_str;
        pVanillaData.wars = new List<WarData>();
        pVanillaData.width = world_width;
        pVanillaData.worldLaws = world_laws;
        world_laws.init();
    }

    public override void LoadWorld()
    {
        // Load Energy Maps
        Dictionary<string, EnergyTileData[,]> energy_tiles = new();
        energy_tiles["cw_energy_wakan"] = new EnergyTileData[MapBox.width, MapBox.height];
        for (var x = 0; x < MapBox.width; x++)
        for (var y = 0; y < MapBox.height; y++)
            energy_tiles["cw_energy_wakan"][x, y] = new EnergyTileData
            {
                value = chunks[x / 16 + y / 16 * (MapBox.width / 16)].wakan,
                density = chunks[x / 16 + y / 16 * (MapBox.width / 16)].wakan_level
            };
        CW_Core.mod_state.energy_map_manager.replace_new_map(energy_tiles, MapBox.width, MapBox.height);
    }

    private void GenerateCW_ActorData(List<ActorData> actors_data)
    {
        if (actors_data == null) return;
        for (int i = 0; i < actors_data.Count; i++)
        {
            ActorData actor_data = actors_data[i];
            CW_ActorData cw_actor_data = cw_actor_datas[i];
            cw_actor_data.element.ComputeType();
            actor_data.SetElement(cw_actor_data.element);
            if (cw_actor_data.spells != null)
            {
                foreach (var spell_id in cw_actor_data.spells.Where(spell_id => Manager.spells.contains(spell_id)))
                    actor_data.AddSpell(spell_id);
            }

            if (!string.IsNullOrEmpty(cw_actor_data.cultibook_id) &&
                Manager.cultibooks.contains(cw_actor_data.cultibook_id))
            {
                // 由于前面将Cultibook的当前人数都设置为0, 在这里重新设置
                actor_data.SetCultibook(Manager.cultibooks.get(cw_actor_data.cultibook_id));
            }

            if (cw_actor_data.cultisys_level == null) continue;

            for (var cultisys_idx = 0; cultisys_idx < cw_actor_data.cultisys_level.Length; cultisys_idx++)
            {
                if (cultisys_idx >= Manager.cultisys.size) break;
                var cultisys = Manager.cultisys.list[cultisys_idx];
                actor_data.set(cultisys.id,
                    Math.Min(cultisys.max_level - 1, cw_actor_data.cultisys_level[cultisys_idx] - 1));
            }
        }
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

    public class OldBaseStats
    {
        public float accuracy;
        public float areaOfEffect;
        public int armor;
        public int army;
        public float attackSpeed;
        public int bonus_towers;
        public int cities;
        public float crit;
        public int damage;
        public float damageCritMod;
        public int diplomacy;
        public float dodge;
        public int health;
        public int intelligence;
        public float knockback;
        public float knockbackReduction;
        public int loyalty_mood;
        public int loyalty_traits;
        public float mod_armor;
        public float mod_attackSpeed;
        public float mod_crit;
        public float mod_damage;
        public float mod_diplomacy;
        public float mod_health;
        public float mod_speed;
        public float mod_supply_timer;
        public int opinion;
        public float personality_administration;
        public float personality_aggression;
        public float personality_diplomatic;
        public float personality_rationality;
        public int projectiles;
        public float range;
        public float s_crit_chance;
        public float scale;
        public float size;
        public float speed;
        public int stewardship;
        public int targets;
        public int warfare;
        public int zones;
    }

    public class CW_BaseStats
    {
        public int age_bonus;
        public float anti_injury;
        public OldBaseStats base_stats;
        public int health_regen;
        public float mod_age;
        public float mod_cultivation;
        public float mod_health_regen;
        public float mod_shield;
        public float mod_shield_regen;
        public float mod_soul;
        public float mod_soul_regen;
        public float mod_spell_armor;
        public float mod_spell_range;
        public float mod_wakan;
        public float mod_wakan_regen;
        public int shield;
        public int shield_regen;
        public float soul;
        public float soul_regen;
        public int spell_armor;
        public float spell_range;
        public float vampire;
        public int wakan;
        public int wakan_regen;
    }

    public class CW_ActorData
    {
        public string cultibook_id;
        public uint cultisys;
        public int[] cultisys_level;
        public int cultisys_to_save;
        public CW_Element element;
        public List<string> spells;
    }

    public class CW_MapChunkData
    {
        public float wakan;
        public float wakan_level;
    }

    public class CW_CultiBookAsset
    {
        public string author_id;
        public string author_name = "佚名";
        public CW_BaseStats bonus_stats;
        public string content;
        public float culti_promt;
        public int cur_culti_nr;
        public int histroy_culti_nr;
        public string id;
        public int level;
        public string name = "无名";
        public int order;
        public string[] spells;
    }
}