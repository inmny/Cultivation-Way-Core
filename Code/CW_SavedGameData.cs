using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Newtonsoft.Json;
using UnityEngine;

namespace Cultivation_Way
{
    [Serializable]
    public class ActorData_For_Save
    {
        public List<CW_ItemData> items;
        public ActorBag inventory;
        public ActorStatus status;
        public string cityID = "";
        public int x;
        public int y;
        public ActorData_For_Save()
        {

        }
        public ActorData_For_Save(ActorData origin_data)
        {
            x = origin_data.x;
            y = origin_data.y;
            cityID = origin_data.cityID;
            status = origin_data.status;
            inventory = origin_data.inventory;
            if(origin_data.items!=null && origin_data.items.Count > 0)
            {
                items = new List<CW_ItemData>();
                foreach (CW_ItemData item in origin_data.items) items.Add(item);
            }
        }
        public ActorData get_data_for_load()
        {
            ActorData ret = new ActorData
            {
                x = x,
                y = y,
                cityID = cityID,
                status = status,
                inventory = inventory
            };
            if(items != null && items.Count > 0)
            {
                ret.items = new List<ItemData>();
                foreach (CW_ItemData item in items)
                {
                    ret.items.Add(item);
                }
            }
            return ret;
        }
    }
    public class CityStorage_For_Save
    {
        public List<CW_ItemData> items_weapons = new List<CW_ItemData>();
        public List<CW_ItemData> items_helmets = new List<CW_ItemData>();
        public List<CW_ItemData> items_armor = new List<CW_ItemData>();
        public List<CW_ItemData> items_boots = new List<CW_ItemData>();
        public List<CW_ItemData> items_rings = new List<CW_ItemData>();
        public List<CW_ItemData> items_amulets = new List<CW_ItemData>();
        public List<CityStorageSlot> savedResources = new List<CityStorageSlot>();
    }
    public class CityData_For_Save
    {
        public string cityName = "NEW_CITY";
        public CityStorage_For_Save storage;
        public string cityID = "";
        public string kingdomID = "";
        public string leaderID = "";
        public List<ZoneData> zones = new List<ZoneData>();
        public string culture = string.Empty;
        public string race = "null";
        public int age;
        public int deaths;
        public int born;
        public float timer_supply;
        public float timer_trade;
        public float timer_revolt;
        public List<ActorData_For_Save> pop_points = new List<ActorData_For_Save>();
        public List<CW_ActorData> cw_pop_points = new List<CW_ActorData>();
        public void set(CW_CityData cw_data)
        {
            age = cw_data.age;
            born = cw_data.born;
            cityID = cw_data.cityID;
            cityName = cw_data.cityName;
            culture = cw_data.culture;
            cw_pop_points = cw_data.cw_pop_points;
            deaths = cw_data.deaths;
            kingdomID = cw_data.kingdomID;
            leaderID = cw_data.leaderID;
            pop_points = new List<ActorData_For_Save>();
            foreach (ActorData actor_data in cw_data.popPoints) pop_points.Add(new ActorData_For_Save(actor_data));
            race = cw_data.race;
            timer_revolt = cw_data.timer_revolt;
            timer_supply = cw_data.timer_supply;
            timer_trade = cw_data.timer_trade;
            zones = cw_data.zones;
            storage = new CityStorage_For_Save();
            storage.savedResources = cw_data.storage.savedResources;
            try
            {
                foreach (CW_ItemData weapon in cw_data.storage.items_weapons) storage.items_weapons.Add(weapon);
                foreach (CW_ItemData armor in cw_data.storage.items_armor) storage.items_armor.Add(armor);
                foreach (CW_ItemData amulet in cw_data.storage.items_amulets) storage.items_amulets.Add(amulet);
                foreach (CW_ItemData ring in cw_data.storage.items_rings) storage.items_rings.Add(ring);
                foreach (CW_ItemData boot in cw_data.storage.items_boots) storage.items_boots.Add(boot);
                foreach (CW_ItemData helmet in cw_data.storage.items_helmets) storage.items_helmets.Add(helmet);
            }
            catch (InvalidCastException)
            {
                foreach (ItemData weapon in cw_data.storage.items_weapons) storage.items_weapons.Add(new CW_ItemData(weapon));
                foreach (ItemData armor in cw_data.storage.items_armor) storage.items_armor.Add(new CW_ItemData(armor));
                foreach (ItemData amulet in cw_data.storage.items_amulets) storage.items_amulets.Add(new CW_ItemData(amulet));
                foreach (ItemData ring in cw_data.storage.items_rings) storage.items_rings.Add(new CW_ItemData(ring));
                foreach (ItemData boot in cw_data.storage.items_boots) storage.items_boots.Add(new CW_ItemData(boot));
                foreach (ItemData helmet in cw_data.storage.items_helmets) storage.items_helmets.Add(new CW_ItemData(helmet));
            }
        }
        public CW_CityData get_cw_data_for_load()
        {
            CW_CityData cw_data = new CW_CityData(null);
            cw_data.age = age;
            cw_data.born = born;
            cw_data.cityID = cityID;
            cw_data.cityName = cityName;
            cw_data.culture = culture;
            cw_data.cw_pop_points = cw_pop_points;
            cw_data.deaths = deaths;
            cw_data.kingdomID = kingdomID;
            cw_data.leaderID = leaderID;
            cw_data.popPoints = new List<ActorData>();
            foreach (ActorData_For_Save actor_data in pop_points) cw_data.popPoints.Add(actor_data.get_data_for_load());
            cw_data.race = race;
            cw_data.timer_revolt = timer_revolt;
            cw_data.timer_supply = timer_supply;
            cw_data.timer_trade = timer_trade;
            cw_data.zones = zones;
            foreach (CW_ItemData weapon in storage.items_weapons) cw_data.storage.items_weapons.Add(weapon);
            foreach (CW_ItemData armor in storage.items_armor) cw_data.storage.items_armor.Add(armor);
            foreach (CW_ItemData amulet in storage.items_amulets) cw_data.storage.items_amulets.Add(amulet);
            foreach (CW_ItemData ring in storage.items_rings) cw_data.storage.items_rings.Add(ring);
            foreach (CW_ItemData boot in storage.items_boots) cw_data.storage.items_boots.Add(boot);
            foreach (CW_ItemData helmet in storage.items_helmets) cw_data.storage.items_helmets.Add(helmet);
            cw_data.storage.savedResources = storage.savedResources;
            return cw_data;
        }
    }
    [Serializable]
    public class CW_SavedGameData
    {
        public int origin_save_version;
        public int cw_save_version;
        public int world_width;
        public int world_height;
        public MapStats map_stats;
        public WorldLaws world_laws;
        public string tile_str;
        public List<string> tile_map = new List<string>();
        public int[][] tile_array;
        public int[][] tile_amounts;
        public List<int> fire = new List<int>();
        public List<int> conway_eater = new List<int>();
        public List<int> conway_creator = new List<int>();
        public List<WorldTileData> tiles = new List<WorldTileData>();
        public List<CityData_For_Save> cities = new List<CityData_For_Save>(); // 实际的数据为CW_CityData
        public List<ActorData_For_Save> actor_datas = new List<ActorData_For_Save>();
        public List<CW_ActorData> cw_actor_datas = new List<CW_ActorData>();
        public List<BuildingData> building_datas = new List<BuildingData>();
        public List<CW_BuildingData> cw_building_datas = new List<CW_BuildingData>();
        public List<Kingdom> kingdoms = new List<Kingdom>();
        public List<DiplomacyRelation> relations = new List<DiplomacyRelation>();
        public List<Culture> cultures = new List<Culture>();
        public List<CW_MapChunk_Data> chunks = new List<CW_MapChunk_Data>();
        public List<CW_Asset_CultiBook> cultibooks = new List<CW_Asset_CultiBook>();
        public List<CW_Asset_SpecialBody> special_bodies = new List<CW_Asset_SpecialBody>();
        internal SavedMap get_origin_format()
        {
            SavedMap origin_save = new SavedMap();
            origin_save.saveVersion = origin_save_version;
            origin_save.width = world_width;
            origin_save.height = world_height;
            origin_save.mapStats = map_stats;
            origin_save.worldLaws = world_laws;
            origin_save.tileMap = tile_map;
            origin_save.tileArray = tile_array;
            origin_save.tileAmounts = tile_amounts;
            origin_save.fire = fire;
            origin_save.conwayCreator = conway_creator;
            origin_save.conwayEater = conway_eater;
            origin_save.tiles = tiles;
            origin_save.cities = new List<CityData>();
            foreach (var c in cities) origin_save.cities.Add(c.get_cw_data_for_load());
            origin_save.kingdoms = kingdoms;
            origin_save.relations = relations;
            origin_save.cultures = cultures;
            return origin_save;
        }
        public MapMetaData getMeta()
        {
            MapMetaData mapMetaData = new MapMetaData();
            List<string> list = new List<string>();
            int num = 0;
            int num2 = 0;
            foreach (ActorData_For_Save actorData in this.actor_datas)
            {
                ActorStats actorStats = AssetManager.unitStats.get(actorData.status.statsID);
                if (actorStats != null)
                {
                    if (actorStats.unit)
                    {
                        num++;
                        if (!list.Contains(actorStats.race))
                        {
                            list.Add(actorStats.race);
                        }
                    }
                    else
                    {
                        num2++;
                    }
                }
            }
            foreach (CityData_For_Save cityData in this.cities)
            {
                num += cityData.pop_points.Count;
            }
            mapMetaData.saveVersion = this.cw_save_version;
            mapMetaData.width = this.world_width;
            mapMetaData.height = this.world_height;
            mapMetaData.mapStats = this.map_stats;
            mapMetaData.cities = this.cities.Count;
            mapMetaData.units = this.actor_datas.Count;
            mapMetaData.population = num;
            mapMetaData.cultures = this.cultures.Count;
            mapMetaData.mobs = num2;
            int num3 = 0;
            int num4 = 0;
            using (List<BuildingData>.Enumerator enumerator3 = this.building_datas.GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.cityID != "")
                    {
                        num3++;
                    }
                    num4++;
                }
            }
            mapMetaData.buildings = num3;
            mapMetaData.structures = num4;
            mapMetaData.kingdoms = this.kingdoms.Count;
            mapMetaData.races = list;
            return mapMetaData;
        }
        public string toJson()
        {
            string text = "";
            try
            {
                text = JsonConvert.SerializeObject(this, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
                if (string.IsNullOrEmpty(text) || text.Length < 20)
                {
                    text = JsonUtility.ToJson(this);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                text = JsonUtility.ToJson(this);
            }
            if (string.IsNullOrEmpty(text) || text.Length < 20)
            {
                throw new Exception("Error while creating json");
            }
            return text;
        }
        public void init()
        {
            this.world_width = Config.ZONE_AMOUNT_X_DEFAULT;
            this.world_height = Config.ZONE_AMOUNT_Y_DEFAULT;
            this.world_laws = new WorldLaws();
            this.world_laws.init();
        }
        public int get_tile_map_id(string last_tile_whole_id)
        {
            if (!this.tile_map.Contains(last_tile_whole_id))
            {
                this.tile_map.Add(last_tile_whole_id);
            }
            return this.tile_map.IndexOf(last_tile_whole_id);
        }
    }
}
