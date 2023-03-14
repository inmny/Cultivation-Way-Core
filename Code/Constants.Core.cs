using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Constants
{
    public static class Core
    {
        public const string uniform_type = "CW_uniform";
        public const int BASE_TYPE_WATER = 0;
        public const int BASE_TYPE_FIRE = 1;
        public const int BASE_TYPE_WOOD = 2;
        public const int BASE_TYPE_IRON = 3;
        public const int BASE_TYPE_GROUND = 4;
        public const int element_type_nr = 5;
        public readonly static string[] element_str = new string[element_type_nr] { "cw_water", "cw_fire", "cw_wood", "cw_iron", "cw_ground" };
    }
}
