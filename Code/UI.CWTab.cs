using System;
using System.Collections.Generic;
using Cultivation_Way.Constants;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cultivation_Way.UI;

internal static class CWTab
{
    private static readonly Dictionary<ButtonContainerType, HashSet<PowerButton>> button_containers = new();
    private static GameObject tab;
    private static PowersTab powers_tab;
    private static GameObject origin_line;
    private static float cur_x = 72f;
    private static readonly float step_x = 36f;
    private const float up_y = 18f;
    private const float down_y = -18f;
    private const float line_step = 23f;
    private static bool to_add_to_up = true;

    public static void init()
    {
        create_tab();
        foreach (ButtonContainerType container_type in Enum.GetValues(typeof(ButtonContainerType)))
        {
            button_containers[container_type] = new HashSet<PowerButton>();
        }

        add_buttons();
    }

    public static void post_init()
    {
        foreach (HashSet<PowerButton> buttons in button_containers.Values)
        {
            to_add_to_up = true;
            foreach (PowerButton button in buttons)
            {
                button.transform.localScale = Vector3.one;
                button.transform.localPosition = new Vector3(cur_x, to_add_to_up ? up_y : down_y);
                to_add_to_up = !to_add_to_up;
                if (to_add_to_up) cur_x += step_x;
            }

            add_line();
        }

        tab.SetActive(true);
    }

    private static void create_tab()
    {
        GameObject origin_tab_button = GameObjects.FindEvenInactive("Button_Other");
        GameObject tab_button = Object.Instantiate(origin_tab_button, origin_tab_button.transform.parent, true);
        tab_button.name = "Button_CW";
        tab_button.transform.localScale = new Vector3(1f, 1f);
        tab_button.transform.localPosition = new Vector3(-150f, 49.62f); //x轴待调整
        tab_button.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconTab");
        //设置栏内元素

        GameObject origin_tab = GameObjects.FindEvenInactive("Tab_Other");
        //暂时禁用copyTab内元素
        foreach (Transform transform in origin_tab.transform)
        {
            transform.gameObject.SetActive(false);
        }

        tab = Object.Instantiate(origin_tab, origin_tab.transform.parent, true);
        tab.SetActive(false);
        //删除复制来的无用元素
        foreach (Transform transform in tab.transform)
        {
            if (transform.gameObject.name == "tabBackButton" || transform.gameObject.name == "-space")
            {
                transform.gameObject.SetActive(true);
            }
            else
            {
                Object.Destroy(transform.gameObject);
            }
        }

        //恢复copyTab内元素
        foreach (Transform transform in origin_tab.transform)
        {
            transform.gameObject.SetActive(true);
        }

        tab.name = "Tab_CW";

        //子内容设置
        Button button_component = tab_button.GetComponent<Button>();
        powers_tab = tab.GetComponent<PowersTab>();
        powers_tab.powerButton = button_component;
        powers_tab.powerButton.onClick = new Button.ButtonClickedEvent();
        powers_tab.powerButton.onClick.AddListener(() =>
        {
            PowersTab component = GameObjects.FindEvenInactive(tab.name)
                .GetComponent<PowersTab>();
            component.showTab(component.powerButton);
        });
        powers_tab.parentObj = origin_tab.transform.parent.parent.gameObject;
        powers_tab.powerButtons.Clear();

        tab_button.GetComponent<TipButton>().textOnClick = "CW";
        tab_button.GetComponent<TipButton>().text_description_2 = "";

        origin_line =
            GameObject.Find(
                "CanvasBottom/BottomElements/BottomElementsMover/CanvasScrollView/Scroll View/Viewport/Content/buttons/Tab_Other/LINE"
            );
    }

    private static void add_buttons()
    {
    }

    private static void add_line()
    {
        GameObject line = Object.Instantiate(origin_line, powers_tab.transform);
        line.transform.localPosition =
            new Vector3(cur_x + line_step - (to_add_to_up ? step_x : 0), line.transform.localPosition.y);
        cur_x += 2 * line_step - (to_add_to_up ? step_x : 0);
    }

    private static void create_and_add_button(string id, string sprite_path, UnityAction action,
        ButtonType button_type = ButtonType.Click, ButtonContainerType container_type = ButtonContainerType.OTHERS)
    {
        add_button(create_button(id, sprite_path, action, button_type));
    }

    /// <summary>
    ///     将button添加到指定的容器中
    /// </summary>
    /// <param name="button"></param>
    /// <param name="container_type"></param>
    public static void add_button(PowerButton button, ButtonContainerType container_type = ButtonContainerType.OTHERS)
    {
        button_containers[container_type].Add(button);
    }

    /// <summary>
    ///     创建按钮
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sprite_path"></param>
    /// <param name="action"></param>
    /// <param name="button_type"></param>
    /// <returns></returns>
    public static PowerButton create_button(string id, string sprite_path, UnityAction action,
        ButtonType button_type = ButtonType.Click)
    {
        return PowerButtons.CreateButton(
            id,
            Resources.Load<Sprite>(sprite_path),
            LocalizedTextManager.stringExists(id + Constants.Core.title_suffix)
                ? LocalizedTextManager.getText(id + Constants.Core.title_suffix)
                : id,
            LocalizedTextManager.stringExists(id + Constants.Core.desc_suffix)
                ? LocalizedTextManager.getText(id + Constants.Core.desc_suffix)
                : "",
            Vector2.zero,
            button_type,
            tab.transform,
            action ?? (() => { })
        );
    }
}