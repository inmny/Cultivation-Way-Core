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
        public List<CityData> cities = new List<CityData>(); // 实际的数据为CW_CityData
        public List<ActorData> actor_datas = new List<ActorData>();
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
            origin_save.cities = cities;
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
            foreach (ActorData actorData in this.actor_datas)
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
            foreach (CityData cityData in this.cities)
            {
                num += cityData.popPoints.Count;
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
