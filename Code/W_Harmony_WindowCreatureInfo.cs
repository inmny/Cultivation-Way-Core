using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_WindowCreatureInfo
	{
		static bool first = true;
		[HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), "inspectUnit")]
        public static bool action_inspectUnit(WorldTile pTile, ref bool __result)
        {
			Actor actor;
			if (pTile == null)
			{
				actor = MapBox.instance.getActorNearCursor();
			}
			else
			{
				actor = ActionLibrary.getActorFromTile(pTile);
			}
			if (actor == null)
			{
				__result = false;
				return false;
			}

			Config.selectedUnit = actor;
			if(!W_Content_WindowCreatureInfo.initialized) W_Content_WindowCreatureInfo.create_window_gameobject();
			ScrollWindow.showWindow("cw_inspect_unit");
			__result = false;
			return false;
		}
    }
}
