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
        internal static void add_world_laws()
        {
            add_wakan_tide_trigger();
        }

        private static void add_wakan_tide_trigger()
        {
            add_law(wakan_tide_trigger, true, CW_WorldLaw_Type.World_Setting);
        }
        private static void add_law(string id, bool default_val, CW_WorldLaw_Type type)
        {
            added_world_laws.Add(id);
            default_world_law_bool_val.Add(default_val);
            W_Content_WindowWorldLaw.add_world_law(id, default_val, type);
        }
        public static bool is_wakan_tide_working() { return MapBox.instance.worldLaws.dict[wakan_tide_trigger].boolVal; }
    }
}
