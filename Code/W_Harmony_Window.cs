using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Window
	{
		[HarmonyPrefix]
        [HarmonyPatch(typeof(ScrollWindow), "showWindow")]
        public static bool action_inspectUnit(ref string pWindowID)
        {
            switch (pWindowID)
            {
				case "inspect_unit":
					if(!W_Content_WindowCreatureInfo.initialized) W_Content_WindowCreatureInfo.create_window_gameobject();
					pWindowID = "cw_inspect_unit";
					break;
				default:
					break;
			}
			return true;
		}
    }
}
