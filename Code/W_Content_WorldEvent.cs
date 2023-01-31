using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Content
{
    internal static class W_Content_WorldEvent
    {
        internal static void add_events()
        {
            add_wakan_tide();

        }

        private static void add_wakan_tide()
        {
            CW_Asset_WorldEvent _event = new CW_Asset_WorldEvent(
                id: "wakan_tide",
                trigger_type: CW_Event_Trigger_Type.CUSTOM,
                trigger_val: 2000f,
                action: wakan_tide,
                action_val: 3f
                );
            CW_Library_Manager.instance.events.add(_event);
        }
        private static void wakan_tide(CW_Asset_WorldEvent event_asset)
        {
            int num = Toolbox.randomInt(1, (int)Mathf.Sqrt(Mathf.Sqrt(World_Data.instance.map_chunk_manager.width * World_Data.instance.map_chunk_manager.height)));
            while (num --> 0)
            {
                World_Data.instance.map_chunk_manager.chunks[Toolbox.randomInt(0, World_Data.instance.map_chunk_manager.width), Toolbox.randomInt(0, World_Data.instance.map_chunk_manager.height)].wakan_level = event_asset.action_val;
            }
        }
    }
}
