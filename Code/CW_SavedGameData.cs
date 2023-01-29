using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way
{
    [Serializable]
    public class CW_SavedGameData
    {
        public int save_version;
        public int world_width;
        public int world_height;
        public MapStats map_stats;
        public WorldLaws world_laws;
        public string tile_str;
        public List<string> tile_map;
        public int[][] tile_array;
        public int[][] tile_amounts;
        public List<int> fire = new List<int>();
        public List<int> conway_eater = new List<int>();
        public List<int> conway_creator = new List<int>();
        public List<WorldTileData> tiles = new List<WorldTileData>();
        public List<CW_CityData> cities = new List<CW_CityData>();
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
    }
}
