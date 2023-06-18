using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extension
{
    public static class BaseSystemDataTools
    {
        public static void clear(this BaseSystemData data)
        {
            data.custom_data_bool?.dict?.Clear();
            data.custom_data_float?.dict?.Clear();
            data.custom_data_int?.dict?.Clear();
            data.custom_data_string?.dict?.Clear();
            data.custom_data_flags?.Clear();
        }
    }
}
