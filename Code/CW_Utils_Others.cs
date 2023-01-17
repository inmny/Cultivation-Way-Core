using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    public class CW_Utils_Others
    {
        public static float get_raw_wakan(float wakan, float wakan_level)
        {
            return wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, wakan_level-1);
        }
        public static float compress_raw_wakan(float raw_wakan, float target_level)
        {
            return raw_wakan / Mathf.Pow(Others.CW_Constants.wakan_level_co, target_level - 1);
        }
    }
}
