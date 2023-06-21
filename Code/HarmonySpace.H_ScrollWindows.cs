using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.HarmonySpace
{
    internal static class H_ScrollWindows
    {
        /// <summary>
        /// 在checkWindowExist后添加patch用于初始化部分窗口
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ScrollWindow), nameof(ScrollWindow.checkWindowExist))]
        public static void ScrollWindow_checkWindowExist_postfix(ScrollWindow __instance, string pWindowID)
        {
            switch (pWindowID)
            {
                case "inspect_unit":
                    {
                        UI.WindowCreatureInfoHelper.init(ScrollWindow.allWindows[pWindowID]);
                        break;
                    }
            }
        }
    }
}
