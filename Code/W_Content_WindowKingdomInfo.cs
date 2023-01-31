using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal class CultiInfoElement : MonoBehaviour
    {

    }
    internal class W_Content_WindowKingdomInfo
    {
        internal static bool initialized = false;
        internal static GameObject origin_inspect_unit_gameobject;
        internal static void patch_to_origin_window()
        {
            // 获取原版窗口
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            origin_inspect_unit_gameobject = NCMS.Utils.Windows.GetWindow("inspect_unit").gameObject;

            origin_inspect_unit_gameobject.SetActive(false);
            WindowCreatureInfo origin_wci = origin_inspect_unit_gameobject.GetComponent<WindowCreatureInfo>();
        }
    }
}
