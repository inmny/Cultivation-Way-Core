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
        private static Color level_0 = Color.white;
        private static Color level_1 = new Color(0.38f, 0.71f, 1);
        private static Color level_2 = Color.magenta;
        private static Color level_3 = Color.yellow;
        public static Color get_wakan_color(float wakan_level, float wakan)
        {
            Color color_to_ret = level_3;
            if (wakan_level == 1)
            {
                color_to_ret = Toolbox.blendColor(level_0, level_1, 100 / (100 + wakan));
            }
            else if (wakan_level <= 2)
            {
                color_to_ret = Toolbox.blendColor(level_1, level_2, 2 - wakan_level);
            }
            else if (wakan_level <= 3)
            {
                color_to_ret = Toolbox.blendColor(level_2, level_3, 3 - wakan_level);
            }
            return color_to_ret;
        }
        public static bool is_map_mode_active(string mode_id)
        {
            return ModState.instance.map_mode == mode_id && PlayerConfig.dict[mode_id].boolVal;
        }
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
