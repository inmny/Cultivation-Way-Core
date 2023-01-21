using NCMS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cultivation_Way.Content
{
    public enum CW_Tab_Button_Type
    {
        INFO,
        TOOL,
        ACTOR,
        BUILDING,
        BOSS,
        OTHERS
    }
    public static class W_Content_Tab
    {
        public static GameObject cw_tab;
        public static PowersTab cw_powers_tab;
        private static Dictionary<CW_Tab_Button_Type, List<PowerButton>> buttons_to_apply;
        private static float cur_x = 72f;
        private static float step_x = 36f;
        private const float up_y = 18f;
        private const float down_y = -18f;
        private const float line_step = 23f;
        private static bool to_add_to_up = true;
        private static GameObject origin_line;

        internal static void create_tab()
        {
            GameObject origin_tab_button = GameObjects.FindEvenInactive("Button_Other");
            GameObject cw_tab_button = GameObject.Instantiate(origin_tab_button);
            cw_tab_button.name = "Button_Cultivation_Way";
            cw_tab_button.transform.SetParent(origin_tab_button.transform.parent);
            cw_tab_button.transform.localScale = new Vector3(1f, 1f);
            cw_tab_button.transform.localPosition = new Vector3(-150f, 49.62f);//x轴待调整
            cw_tab_button.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconTab");
            //设置栏内元素

            GameObject origin_tab = GameObjects.FindEvenInactive("Tab_Other");
            //暂时禁用copyTab内元素
            foreach (Transform transform in origin_tab.transform)
            {
                transform.gameObject.SetActive(false);
            }

            cw_tab = GameObject.Instantiate(origin_tab);
            //删除复制来的无用元素
            foreach (Transform transform in cw_tab.transform)
            {
                if (transform.gameObject.name == "tabBackButton" || transform.gameObject.name == "-space")
                {
                    transform.gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Destroy(transform.gameObject);
                }
            }
            //恢复copyTab内元素
            foreach (Transform transform in origin_tab.transform)
            {
                transform.gameObject.SetActive(true);
            }

            cw_tab.name = "Tab_Cultivation_Way";
            cw_tab.transform.SetParent(origin_tab.transform.parent);

            //子内容设置
            Button buttonComponent = cw_tab_button.GetComponent<Button>();
            cw_powers_tab = cw_tab.GetComponent<PowersTab>();
            cw_powers_tab.powerButton = buttonComponent;
            cw_powers_tab.powerButton.onClick = new Button.ButtonClickedEvent();
            cw_powers_tab.powerButton.onClick.AddListener(tab_button_click);
            cw_powers_tab.tipKey = "CW_Tab_title";
            ReflectionUtility.Reflection.SetField<GameObject>(cw_powers_tab, "parentObj", origin_tab.transform.parent.parent.gameObject);
            cw_tab.SetActive(true);
            buttons_to_apply = new Dictionary<CW_Tab_Button_Type, List<PowerButton>>();
            buttons_to_apply[CW_Tab_Button_Type.INFO] = new List<PowerButton>();
            buttons_to_apply[CW_Tab_Button_Type.TOOL] = new List<PowerButton>();
            buttons_to_apply[CW_Tab_Button_Type.ACTOR] = new List<PowerButton>();
            buttons_to_apply[CW_Tab_Button_Type.BUILDING] = new List<PowerButton>();
            buttons_to_apply[CW_Tab_Button_Type.BOSS] = new List<PowerButton>();
            buttons_to_apply[CW_Tab_Button_Type.OTHERS] = new List<PowerButton>();
            origin_line = GameObject.Find("CanvasBottom/BottomElements/BottomElementsMover/CanvasScrollView/Scroll View/Viewport/Content/buttons/Tab_Other/LINE");
        }
        internal static void add_buttons()
        {
            PowerButton button;
            add_button(
                create_button(
                    "CW_AboutThis", "ui/Icons/iconabout",
                    show_window_about_this
                    ), 
                CW_Tab_Button_Type.INFO
                );
            add_button(
                create_button(
                    "CW_Wiki", "ui/Icons/iconWiki",
                    jump_to_wiki
                    ),
                CW_Tab_Button_Type.INFO
                );
            add_button(
                create_button(
                    "CW_WorldLaw", "ui/Icons/iconworldlaws",
                    show_window_world_law
                    ),
                CW_Tab_Button_Type.TOOL
                );
            add_button(
                create_button(
                    "CW_Top", "ui/Icons/iconTop",
                    show_window_top
                    ),
                CW_Tab_Button_Type.TOOL
                );
            button = 
            add_button(
                create_button(
                    "CW_CheckWakan", "ui/Icons/iconCheckWakan",
                    null, ButtonType.GodPower
                ),
                CW_Tab_Button_Type.TOOL
                );
            add_button(
                create_button(
                    "spawnEasternHuman", "ui/Icons/iconEasternHuman",
                    null, ButtonType.GodPower
                ),
                CW_Tab_Button_Type.ACTOR
                );
            add_button(
                create_button(
                    "spawnYao", "ui/Icons/iconYao",
                    null, ButtonType.GodPower
                ),
                CW_Tab_Button_Type.ACTOR
                );
        }
        internal static PowerButton create_button(string id, string sprite_path, UnityAction action, ButtonType button_type = ButtonType.Click)
        {
            return PowerButtons.CreateButton(id, Resources.Load<Sprite>(sprite_path), LocalizedTextManager.getText(id + "_title"), LocalizedTextManager.stringExists(id + "_description") ? LocalizedTextManager.getText(id + "_description") : "", Vector2.zero, button_type, cw_tab.transform, action);
        }
        public static PowerButton add_button(PowerButton button, CW_Tab_Button_Type button_type = CW_Tab_Button_Type.OTHERS)
        {
            buttons_to_apply[button_type].Add(button);
            return button;
        }
        internal static void apply_buttons()
        {
            foreach(List<PowerButton> buttons in buttons_to_apply.Values)
            {
                to_add_to_up = true;
                foreach(PowerButton button in buttons)
                {
                    button.transform.localScale = Vector3.one;
                    button.transform.localPosition = new Vector3(cur_x, to_add_to_up ? up_y : down_y);
                    to_add_to_up = !to_add_to_up;
                    if (to_add_to_up) cur_x += step_x;
                }
                add_line(to_add_to_up);
            }
        }
        private static void add_line(bool to_add_to_up)
        {
            GameObject line = GameObject.Instantiate(origin_line, cw_powers_tab.transform);
            line.transform.localPosition = new Vector3(cur_x + line_step - (to_add_to_up ? step_x : 0), line.transform.localPosition.y);
            cur_x += 2 * line_step - (to_add_to_up ? step_x : 0);
        }
        private static void tab_button_click()
        {
            GameObject tab = GameObjects.FindEvenInactive("Tab_Cultivation_Way");
            PowersTab powersTab = tab.GetComponent<PowersTab>();
            powersTab.showTab(powersTab.powerButton);
        }
        private static void show_window_about_this()
        {
            Windows.ShowWindow("cw_window_about_this");
        }
        private static void show_window_world_law()
        {
            Windows.ShowWindow("cw_window_world_law");
        }
        private static void show_window_top()
        {
            Windows.ShowWindow("cw_window_top");
        }
        private static void jump_to_wiki()
        {
            Application.OpenURL(@"https://github.com/inmny/Cultivation-Way-Core/wiki");
        }
    }
}
