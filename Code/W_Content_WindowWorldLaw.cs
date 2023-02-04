using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
using UnityEngine.UI;

namespace Cultivation_Way.Content
{
    internal enum CW_WorldLaw_Type
    {
        World_Setting
    }
    internal static class W_Content_WindowWorldLaw
    {
        private static GameObject law_button_prefab;
        private static GameObject law_grid_prefab;
        private static List<Transform> grids = new List<Transform>();
        internal static void init()
        {
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "world_laws");
            GameObject cw_wwl = GameObject.Instantiate(NCMS.Utils.Windows.GetWindow("world_laws").gameObject, CanvasMain.instance.transformWindows);
            NCMS.Utils.Windows.GetWindow("world_laws").gameObject.SetActive(false);
            cw_wwl.name = "cw_world_laws";
            
            ScrollWindow cw_wwl_sw = cw_wwl.GetComponent<ScrollWindow>();
            NCMS.Utils.Windows.AllWindows.Add(cw_wwl.name, cw_wwl_sw);

            cw_wwl_sw.CallMethod("create", true);
            cw_wwl.transform.Find("Background/Scroll View").gameObject.SetActive(true);

            // 设置标题
            GameObject title_object = cw_wwl.transform.Find("Background/Title").gameObject;
            title_object.GetComponent<LocalizedText>().autoField = false;
            Text title = title_object.GetComponent<Text>();
            title.text = LocalizedTextManager.getText("CW_WorldLaw_title");
            title.font = W_Content_Helper.font_STLiti;
            title.fontSize = 15;
            title.resizeTextMaxSize = 15;

            Transform content_transform = cw_wwl.transform.Find("Background/Scroll View/Viewport/Content");

            GameObject.Destroy(content_transform.Find("Units").gameObject);
            GameObject.Destroy(content_transform.Find("Mobs").gameObject);
            GameObject.Destroy(content_transform.Find("Nature").gameObject);

            law_button_prefab = GameObject.Instantiate(content_transform.Find("Civ/Grid/world_law_diplomacy").gameObject);
            law_button_prefab.SetActive(false);

            int children_to_destroy_nr = content_transform.Find("Civ/Grid").childCount;
            while (children_to_destroy_nr > 0)
            {
                GameObject.Destroy(content_transform.Find("Civ/Grid").GetChild(children_to_destroy_nr - 1).gameObject);
                children_to_destroy_nr--;
            }
            law_grid_prefab = GameObject.Instantiate(content_transform.Find("Civ").gameObject);
            law_grid_prefab.SetActive(false);

            Transform world_setting = content_transform.Find("Civ");
            world_setting.name = "world_setting";
            world_setting.Find("Title").GetComponent<LocalizedText>().key = "cw_world_setting";
            grids.Add(world_setting);
        }
        internal static void add_world_law(string id, bool default_val, CW_WorldLaw_Type type)
        {
            Transform grid_transform = grids[(int)type];
            GameObject law_button = GameObject.Instantiate(law_button_prefab, grid_transform);
            law_button.name = id;
            law_button.transform.localPosition = get_button_pos(grid_transform.childCount-3);
            law_button.GetComponent<WorldLawElement>().icon.sprite = Resources.Load<Sprite>("ui/Icons/iconCheckWakan");
            law_button.SetActive(true);
        }
        private const float start_x = -74f;
        private const float start_y = 18.5f;
        private static float step_x = 37f;
        private static float step_y = -37;
        private static int row_count = 5;
        private static Vector3 get_button_pos(int childCount)
        {
            int x = childCount % row_count;
            int y = childCount / row_count;
            return new Vector3(start_x + x * step_x, start_y + y * step_y);
        }
    }
}
