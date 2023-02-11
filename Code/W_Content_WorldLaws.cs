using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content
{
    internal static class W_Content_WorldLaws
    {
        internal static List<string> added_world_laws = new List<string>();
        internal static List<bool> default_world_law_bool_val = new List<bool>();
        private const string wakan_tide_trigger = "wakan_tide_trigger";
        private const string no_wakan_spread = "no_wakan_spread";
        private const string enpowered_wakan_increase = "enpowered_wakan_increase";
        internal static void add_world_laws()
        {
            add_wakan_tide_trigger();
            add_no_wakan_spread();
            add_enpowered_wakan_increase();
        }


        internal static void add_world_settings()
        {
            add_cultisys_name_setting();
        }

        private static void add_cultisys_name_setting()
        {
            W_Content_WindowWorldLaw.add_world_setting("cultisys_setting", "iconCultiSys", new UnityEngine.Vector3(0.6f, 0.75f), CW_WorldLaw_Type.World_Setting, delegate
            {
                ScrollWindow.showWindow("cultisys_setting");
            });
        }
        private static void add_enpowered_wakan_increase()
        {
            add_law(enpowered_wakan_increase, false, "iconWakan_Increase", CW_WorldLaw_Type.World_Option);
        }

        private static void add_wakan_tide_trigger()
        {
            add_law(wakan_tide_trigger, true, "iconCheckWakan", CW_WorldLaw_Type.World_Option);
        }

        private static void add_no_wakan_spread()
        {
            add_law(no_wakan_spread, false, "iconNo_Wakan_Spread", CW_WorldLaw_Type.World_Option);
        }
        private static void add_law(string id, bool default_val, string icon_name, CW_WorldLaw_Type type)
        {
            added_world_laws.Add(id);
            default_world_law_bool_val.Add(default_val);
            W_Content_WindowWorldLaw.add_world_law(id, default_val, icon_name, type);
        }
        public static bool is_wakan_tide_working() { return MapBox.instance.worldLaws.dict[wakan_tide_trigger].boolVal; }
        public static bool is_no_wakan_spread_working() { return MapBox.instance.worldLaws.dict[no_wakan_spread].boolVal; }
        public static bool is_wakan_increase_enpowered() { return MapBox.instance.worldLaws.dict[enpowered_wakan_increase].boolVal; }
    }
}
