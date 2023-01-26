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
        }
    }
}
