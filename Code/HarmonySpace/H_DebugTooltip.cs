using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal static class H_DebugTooltip
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TooltipLibrary), nameof(TooltipLibrary.showActor))]
    public static void showActor_postfix(string pTitle, Tooltip pTooltip, TooltipData pData)
    {
        CW_Actor actor = (CW_Actor)pData.actor;

        CW_Element element = actor.data.GetElement();
        if (element != null)
        {
            pTooltip.addLineText("cw_element", LocalizedTextManager.getText(element.GetElementType().id),
                Toolbox.colorToHex(element.GetColor()));
        }
    }
}