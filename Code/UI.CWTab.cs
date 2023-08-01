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
    private static GameObject _tab;
    private static PowersTab _powers_tab;
    private static GameObject _origin_line;
    private static float _cur_x = 72f;
    private const float step_x = 36f;
    private const float up_y = 18f;
    private const float down_y = -18f;
    private const float line_step = 23f;
    private static bool _to_add_to_up = true;
    private static GameObject _last_line;

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
        foreach (ButtonContainerType container_type in Enum.GetValues(typeof(ButtonContainerType)))
        {
            HashSet<PowerButton> buttons = button_containers[container_type];
            if (buttons.Count == 0) continue;
            _to_add_to_up = true;

            foreach (PowerButton button in buttons)
            {
                button.transform.localScale = Vector3.one;
                button.transform.localPosition = new Vector3(_cur_x, _to_add_to_up ? up_y : down_y);
                _to_add_to_up = !_to_add_to_up;
                if (_to_add_to_up) _cur_x += step_x;
            }

            add_line();
        }

        if (_last_line != null) _last_line.SetActive(false);
        _tab.SetActive(true);
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

        _tab = Object.Instantiate(origin_tab, origin_tab.transform.parent, true);
        _tab.SetActive(false);
        //删除复制来的无用元素
        foreach (Transform transform in _tab.transform)
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

        _tab.name = "Tab_CW";

        //子内容设置
        Button button_component = tab_button.GetComponent<Button>();
        _powers_tab = _tab.GetComponent<PowersTab>();
        _powers_tab.powerButton = button_component;
        _powers_tab.powerButton.onClick = new Button.ButtonClickedEvent();
        _powers_tab.powerButton.onClick.AddListener(() =>
        {
            PowersTab component = GameObjects.FindEvenInactive(_tab.name)
                .GetComponent<PowersTab>();
            component.showTab(component.powerButton);
        });
        _powers_tab.parentObj = origin_tab.transform.parent.parent.gameObject;
        _powers_tab.powerButtons.Clear();

        tab_button.GetComponent<TipButton>().textOnClick = "CW";
        tab_button.GetComponent<TipButton>().text_description_2 = "";

        _origin_line =
            GameObject.Find(
                "CanvasBottom/BottomElements/BottomElementsMover/CanvasScrollView/Scroll View/Viewport/Content/buttons/Tab_Other/LINE"
            );
    }

    private static void add_buttons()
    {
        // 模组信息窗口
        create_and_add_button(
            Constants.Core.modinfo_window,
            "ui/Icons/iconAbout",
            () => ScrollWindow.showWindow(Constants.Core.modinfo_window),
            ButtonType.Click,
            ButtonContainerType.INFO
        );
        // 天榜窗口
        create_and_add_button(
            Constants.Core.tops_window,
            "ui/Icons/iconTop",
            () => ScrollWindow.showWindow(Constants.Core.tops_window),
            ButtonType.Click,
            ButtonContainerType.TOOL
        );
        // 能量地图
        create_and_add_button(
            Constants.Core.energy_maps_toggle_name,
            "ui/Icons/iconCheckWakan",
            null,
            ButtonType.Toggle,
            ButtonContainerType.TOOL
        );
        // 拔高能量
        create_and_add_button(
            Constants.Core.power_energy_increase,
            "ui/Icons/iconWakan_Increase",
            null,
            ButtonType.GodPower,
            ButtonContainerType.TOOL
        );
        // 降低能量
        create_and_add_button(
            Constants.Core.power_energy_decrease,
            "ui/Icons/iconWakan_Decrease",
            null,
            ButtonType.GodPower,
            ButtonContainerType.TOOL
        );
    }

    private static void add_line()
    {
        GameObject line = Object.Instantiate(_origin_line, _powers_tab.transform);
        line.transform.localPosition =
            new Vector3(_cur_x + line_step - (_to_add_to_up ? step_x : 0), line.transform.localPosition.y);
        _cur_x += 2 * line_step - (_to_add_to_up ? step_x : 0);
        _last_line = line;
    }

    public static void create_and_add_button(string id, string sprite_path, UnityAction action,
        ButtonType button_type = ButtonType.Click, ButtonContainerType container_type = ButtonContainerType.OTHERS)
    {
        add_button(create_button(id, sprite_path, action, button_type), container_type);
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
        PowerButton ret = PowerButtons.CreateButton(
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
            _tab.transform,
            action ??
            (button_type == ButtonType.Click ? () => { } : null)
        );
        if (button_type == ButtonType.Toggle)
        {
            ret.transform.Find("toggleIcon").gameObject.name = "ToggleIcon";
            ret.godPower = AssetManager.powers.get(id);
            ToggleIcon toggle_icon = ret.transform.Find("ToggleIcon").gameObject.AddComponent<ToggleIcon>();
            toggle_icon.spriteON = SpriteTextureLoader.getSprite("ui/cw_window/buttonToggleIndicator_0");
            toggle_icon.spriteOFF = SpriteTextureLoader.getSprite("ui/cw_window/buttonToggleIndicator_1");
            Button button = ret.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(
                () =>
                {
                    ret.godPower?.toggle_action(ret.godPower.id);
                    toggle_icon.updateIcon(PlayerConfig.optionBoolEnabled(id));
                }
            );
            PowerButton.toggleButtons.Add(ret);
        }

        return ret;
    }
}