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
        World_Option,
        World_Setting
    }
    internal static class W_Content_WindowWorldLaw
    {
        private static GameObject law_button_prefab;
        private static GameObject law_grid_prefab;
        private static GameObject setting_button_prefab;
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

            law_button_prefab = GameObject.Instantiate(content_transform.Find("Civ/Grid/world_law_diplomacy").gameObject, content_transform);
            law_button_prefab.name = "law_button_prefab";
            law_button_prefab.SetActive(false);

            Transform __civ_grid = content_transform.Find("Civ/Grid");
            int children_to_destroy_nr = __civ_grid.childCount;
            while (children_to_destroy_nr > 0)
            {
                Transform __transform_to_destroy = __civ_grid.GetChild(children_to_destroy_nr - 1);
                GameObject.DestroyImmediate(__transform_to_destroy.gameObject);
                children_to_destroy_nr--;
            }

            setting_button_prefab = new GameObject("setting_button_prefab");
            setting_button_prefab.SetActive(false);
            setting_button_prefab.transform.SetParent(content_transform);
            GameObject setting_button_prefab_button_object = new GameObject("Button", typeof(Button), typeof(TipButton), typeof(Image));
            setting_button_prefab_button_object.transform.SetParent(setting_button_prefab.transform);
            setting_button_prefab_button_object.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/button");
            GameObject setting_button_prefab_button_image = new GameObject("Icon", typeof(Image));
            setting_button_prefab_button_image.transform.SetParent(setting_button_prefab_button_object.transform);
            setting_button_prefab_button_image.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconCultiSys");

            Transform world_option = content_transform.Find("Civ");
            world_option.name = "world_option";
            GameObject.Destroy(world_option.Find("Grid").GetComponent<GridLayoutGroup>());
            world_option.Find("Title").GetComponent<LocalizedText>().key = "cw_world_option";
            grids.Add(world_option);

            Transform world_setting = create_new_grid("world_setting", content_transform).transform;
            world_setting.Find("Title").GetComponent<LocalizedText>().key = "cw_world_setting";
            grids.Add(world_setting);
            world_setting.gameObject.SetActive(true);
        }
        private static GameObject create_new_grid(string id, Transform content_transform)
        {
            GameObject grid = GameObject.Instantiate(content_transform.Find("world_option").gameObject, content_transform);
            grid.name = id;
            grid.SetActive(true);
            Transform __grid_content = grid.transform.Find("Grid");
            int children_to_destroy_nr = __grid_content.childCount;
            while (children_to_destroy_nr > 0)
            {
                Transform __transform_to_destroy = __grid_content.GetChild(children_to_destroy_nr - 1);
                GameObject.DestroyImmediate(__transform_to_destroy.gameObject);
                children_to_destroy_nr--;
            }
            return grid;
        }
        internal static void add_world_setting(string id, string icon, Vector3 icon_scale, CW_WorldLaw_Type type, Action action)
        {
            Transform grid_transform = grids[(int)type];
            GameObject setting_button = GameObject.Instantiate(setting_button_prefab, grid_transform.Find("Grid"));
            set_grid_size(grid_transform, grid_transform.Find("Grid").childCount - 1);
            setting_button.name = id;
            setting_button.transform.Find("Button").GetComponent<TipButton>().textOnClick = id+"_title";
            setting_button.transform.Find("Button").GetComponent<TipButton>().textOnClickDescription = id + "_description";
            setting_button.transform.Find("Button").GetComponent<Button>().onClick.AddListener((UnityEngine.Events.UnityAction)delegate
            {
                action();
            });
            setting_button.transform.Find("Button/Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/" + icon);
            setting_button.transform.Find("Button/Icon").transform.localScale = icon_scale;
            setting_button.transform.localPosition = get_button_pos(grid_transform.Find("Grid").childCount - 1);
            setting_button.SetActive(true);
        }
        internal static void add_world_law(string id, bool default_val, CW_WorldLaw_Type type)
        {
            Transform grid_transform = grids[(int)type];
            GameObject law_button = GameObject.Instantiate(law_button_prefab, grid_transform.Find("Grid"));
            set_grid_size(grid_transform, grid_transform.Find("Grid").childCount - 1);
            law_button.name = id;
            law_button.transform.localPosition = get_button_pos(grid_transform.Find("Grid").childCount-1);
            law_button.GetComponent<WorldLawElement>().icon.sprite = Resources.Load<Sprite>("ui/Icons/iconCheckWakan");
            law_button.SetActive(true);
        }
        private const float start_x = -74f;
        private const float start_y = 0f;
        private static float step_x = 37f;
        private static float step_y = -37;
        private static int row_count = 5;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="childCount">按钮加入前的按钮量</param>
        /// <returns></returns>
        private static Vector3 get_button_pos(int childCount)
        {
            int x = childCount % row_count;
            int y = childCount / row_count;
            return new Vector3(start_x + x * step_x, start_y + y * step_y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childCount">按钮加入前的按钮量</param>
        private static void set_grid_size(Transform transform, int childCount)
        {
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(190.0f, 41.31f + 37f * (childCount/ 5));
        }
    }
}
