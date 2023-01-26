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
        public static float transform_wakan(float wakan, float from_level, float to_level)
        {
            return wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, from_level - to_level);
        }
        public static float get_raw_wakan(float wakan, float wakan_level)
        {
            return wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, wakan_level - 1);
        }
        public static float compress_raw_wakan(float raw_wakan, float target_level)
        {
            return raw_wakan / Mathf.Pow(Others.CW_Constants.wakan_level_co, target_level - 1);
        }
        public static float get_seconds_by_month(int month)
        {
            return month * Others.CW_Constants.seconds_per_month;
        }
        public static int sum_of(int[] ints)
        {
            int ret = 0;
            foreach (int elm in ints)
            {
                ret += elm;
            }
            return ret;
        }
        public static int max_of(int[] ints)
        {
            int ret = int.MinValue;
            foreach(int elm in ints)
            {
                if (elm > ret) ret = elm; 
            }
            return ret;
        }
    }
}
