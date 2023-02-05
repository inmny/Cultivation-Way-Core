using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal static class W_Content_WindowTop_Helper
    {
        private static List<CW_Actor> _actors = new List<CW_Actor>();
        private static List<CW_CityData> _cities = new List<CW_CityData>();
        internal static void clear_tmp_lists()
        {
            _actors.Clear();
            _cities.Clear();
        }
        private static void collect_actors()
        {
            if (_actors.Count != 0) return;

            List<Actor> units = MapBox.instance.units.getSimpleList();
            foreach(Actor actor in units)
            {
                if (actor.object_destroyed || !actor.base_data.alive) continue;
                _actors.Add((CW_Actor)actor);
            }
        }
        private static void collect_cities()
        {
            if (_cities.Count != 0) return;

            List<City> cities = MapBox.instance.citiesList;
            foreach(City city in cities)
            {
                if (!(bool)Reflection.GetField(typeof(City), city, "alive")) continue;
                city.CallMethod("updateCityStatus");
                CW_CityData cw_data = CW_City.get_data(city) as CW_CityData;
                cw_data.tmp_avg_level = 0;
                cw_data.comp_wakan();
                _cities.Add(cw_data);
            }
        }
        internal static List<CW_Actor> sort_creatures_by_level(int amount)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = Utils.CW_Utils_Others.max_of(left.cw_data.cultisys_level) * left.cw_status.health_level * left.cw_status.wakan_level;
                float l_2 = Utils.CW_Utils_Others.max_of(right.cw_data.cultisys_level) * right.cw_status.health_level * right.cw_status.wakan_level;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for(int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }
        internal static List<CW_Actor> sort_by_cultisys_level(int amount, int tag)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = (left.cw_data.cultisys & (1ul<<tag)) > 0 ? left.cw_data.cultisys_level[tag] : -1;
                float l_2 = (right.cw_data.cultisys & (1ul<<tag)) > 0 ? right.cw_data.cultisys_level[tag] : -1;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for (int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }
        internal static List<CW_Actor> sort_by_bushido_level(int amount)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = (left.cw_data.cultisys&0x2)>0?left.cw_data.cultisys_level[1]:-1;
                float l_2 = (right.cw_data.cultisys & 0x2) > 0 ? right.cw_data.cultisys_level[1] : -1;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for (int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }
        internal static List<CW_Actor> sort_creatures_by_kills(int amount)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = left.fast_data.kills;
                float l_2 = right.fast_data.kills;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for (int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }
        internal static List<CW_Actor> sort_creatures_by_cultivelo(int amount)
        {
            collect_actors();
            List<CW_Actor> ret = new List<CW_Actor>();
            _actors.Sort((left, right) =>
            {
                float l_1 = left.cw_status.culti_velo;
                float l_2 = right.cw_status.culti_velo;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });
            amount = Math.Min(amount, _actors.Count);
            for (int i = 0; i < amount; i++)
            {
                ret.Add(_actors[i]);
            }
            return ret;
        }

        internal static List<CW_CityData> sort_cities_by_wakan(int amount)
        {
            if (_cities.Count == 0) collect_cities();
            List<CW_CityData> ret = new List<CW_CityData>();
            _cities.Sort((left, right) =>
            {
                float l_1 = left.tmp_wakan_total;
                float l_2 = right.tmp_wakan_total;
                if (l_1 == l_2) return 0;
                return l_1 < l_2 ? 1 : -1;
            });

            amount = Math.Min(amount, _cities.Count);
            for (int i = 0; i < amount; i++)
            {
                ret.Add(_cities[i]);
            }
            return ret;
        }
    }
}
