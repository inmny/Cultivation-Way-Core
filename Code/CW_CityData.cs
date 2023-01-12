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
            
        }
    }
}
