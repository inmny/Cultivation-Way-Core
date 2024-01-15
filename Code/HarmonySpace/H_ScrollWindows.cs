using Cultivation_Way.UI;
using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal static class H_ScrollWindows
{
    /// <summary>
    ///     在checkWindowExist后添加patch用于初始化部分窗口
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScrollWindow), nameof(ScrollWindow.checkWindowExist))]
    public static void ScrollWindow_checkWindowExist_postfix(ScrollWindow __instance, string pWindowID)
    {
        switch (pWindowID)
        {
            case "inspect_unit":
            {
                WindowCreatureInfoHelper.init(ScrollWindow.allWindows[pWindowID]);
                break;
            }
            case "village":
            {
                if (ScrollWindow.allWindows[pWindowID].GetComponent<CityWindowAddition>() == null)
                    ScrollWindow.allWindows[pWindowID].gameObject.AddComponent<CityWindowAddition>().Init();
                break;
            }
        }
    }
}