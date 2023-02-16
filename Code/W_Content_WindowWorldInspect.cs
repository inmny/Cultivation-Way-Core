using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal class WindowWorldInspect:MonoBehaviour
    {
        private Transform content_transform;
        private static WindowWorldInspect instance;
        private bool initialized = false;
        private bool first_open = true;

        public static void init()
        {
            ScrollWindow scroll_window = GameObject.Instantiate(Resources.Load<ScrollWindow>("windows/empty"), CanvasMain.instance.transformWindows);
            scroll_window.titleText.GetComponent<LocalizedText>().key = "CW_Inspect_title";
            scroll_window.screen_id = "cw_window_inspect_world";
            scroll_window.name = "cw_window_inspect_world";
            scroll_window.CallMethod("create", false);
            NCMS.Utils.Windows.AllWindows[scroll_window.name] = scroll_window;

            instance = scroll_window.gameObject.AddComponent<WindowWorldInspect>();
            instance.transform.Find("Background/Scroll View").gameObject.SetActive(true);
            instance.content_transform = instance.transform.Find("Background/Scroll View/Viewport/Content");
            instance.gameObject.SetActive(false);
        }
    }
}
