using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_CityData : CityData
    {
        public List<CW_ActorData> cw_pop_points;
        [NonSerialized]
        internal string least_unit_id;
        [NonSerialized]
        internal string most_unit_id;
        [NonSerialized]
        internal int tmp_wakan_total;
        [NonSerialized]
        internal int tmp_avg_level;
        public CW_CityData(City pCity) : base(pCity)
        {
            this.cw_pop_points = new List<CW_ActorData>();
        }
        public void set_origin_data(CityData origin, bool clear = true)
        {
            this.age = origin.age;
            this.born = origin.born;
            this.cityID = origin.cityID;
            this.cityName = origin.cityName;
            this.culture = origin.culture;
            this.deaths = origin.deaths;
            this.kingdomID = origin.kingdomID;
            this.leaderID = origin.leaderID;
            this.popPoints = origin.popPoints;
            if(clear) this.popPoints = new List<ActorData>();
            this.race = origin.race;
            this.storage = origin.storage;
            this.timer_revolt = origin.timer_revolt;
            this.timer_supply = origin.timer_supply;
            this.timer_trade = origin.timer_trade;
            this.zones = origin.zones;
            foreach (ItemData weapon in origin.storage.items_weapons) storage.items_weapons.Add(new CW_ItemData(weapon));
            foreach (ItemData armor in origin.storage.items_armor) storage.items_armor.Add(new CW_ItemData(armor));
            foreach (ItemData amulet in origin.storage.items_amulets) storage.items_amulets.Add(new CW_ItemData(amulet));
            foreach (ItemData ring in origin.storage.items_rings) storage.items_rings.Add(new CW_ItemData(ring));
            foreach (ItemData boot in origin.storage.items_boots) storage.items_boots.Add(new CW_ItemData(boot));
            foreach (ItemData helmet in origin.storage.items_helmets) storage.items_helmets.Add(new CW_ItemData(helmet));
        }
        internal void comp_avg_level()
        {
            throw new NotImplementedException();
        }
        internal void comp_wakan()
        {
            tmp_wakan_total = 0;
            List<TileZone> zones = ReflectionUtility.Reflection.GetField(typeof(City), MapBox.instance.getCityByID(this.cityID), "zones") as List<TileZone>;
            foreach(TileZone zone in zones)
            {
                tmp_wakan_total += (int)((World_Data.instance.map_chunk_manager.get_chunk(zone.chunk.x, zone.chunk.y).wakan_level)*100);
            }
            tmp_wakan_total /= zones.Count == 0?1:zones.Count;
            tmp_wakan_total -= 99;
            if (tmp_wakan_total < 0) tmp_wakan_total = 0;
        }
        internal UnityEngine.Sprite get_race_icon()
        {
            if (this.race != "yao") return UnityEngine.Resources.Load<UnityEngine.Sprite>(AssetManager.raceLibrary.get(this.race).path_icon);
            else
            {
                return UnityEngine.Resources.Load<UnityEngine.Sprite>("ui/Icons/"+AssetManager.unitStats.get(string.IsNullOrEmpty(this.most_unit_id) ? "_yao" : this.most_unit_id).icon);
            }
        }
    }
}
