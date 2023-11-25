using System;
using System.Linq;
using System.Collections.Generic;
using Cultivation_Way.Constants;
using NCMS.Utils;
using NeoModLoader.General;
using NeoModLoader.General.UI.Tab;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cultivation_Way.UI;

internal static class CWTab
{
    private static GameObject _tab;
    private static PowersTab _powers_tab;

    public static void init()
    {
        create_tab();

        add_buttons();
    }

    public static void post_init()
    {
        _powers_tab.UpdateLayout();
    }

    private static void create_tab()
    {
        _powers_tab = TabManager.CreateTab("CW", "cw_Tab_title", null,
            SpriteTextureLoader.getSprite("ui/cw_window/iconTab"));
        _tab = _powers_tab.gameObject;
        _powers_tab.SetLayout(Enum.GetNames(typeof(ButtonContainerType)).ToList());
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
        // 世界法则窗口
        create_and_add_button(
            Constants.Core.worldlaw_window,
            "ui/Icons/iconworldlaws",
            () => ScrollWindow.showWindow(Constants.Core.worldlaw_window),
            ButtonType.Click,
            ButtonContainerType.TOOL
        );
        // 模组设置窗口
        create_and_add_button(
            Constants.Core.modconfig_window,
            "ui/Icons/iconOptions",
            () => ScrollWindow.showWindow(Constants.Core.modconfig_window),
            ButtonType.Click,
            ButtonContainerType.TOOL
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

    public static PowerButton create_and_add_button(string id, string sprite_path, UnityAction action,
        ButtonType button_type = ButtonType.Click, ButtonContainerType container_type = ButtonContainerType.OTHERS)
    {
        var ret = create_button_new(id, sprite_path, action, button_type);
        add_button(ret, container_type);
        return ret;
    }

    /// <summary>
    ///     将button添加到指定的容器中
    /// </summary>
    /// <param name="button"></param>
    /// <param name="container_type"></param>
    public static void add_button(PowerButton button, ButtonContainerType container_type = ButtonContainerType.OTHERS)
    {
        _powers_tab.AddPowerButton(container_type.ToString(), button);
    }
    /// <summary>
    ///     创建按钮
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sprite_path"></param>
    /// <param name="action"></param>
    /// <param name="button_type"></param>
    /// <returns></returns>
    public static PowerButton create_button_new(string id, string sprite_path, UnityAction action,
        ButtonType button_type = ButtonType.Click)
    {
        PowerButton ret = null;
        switch (button_type)
        {
            case ButtonType.Click:
                ret = PowerButtonCreator.CreateSimpleButton(id, action, SpriteTextureLoader.getSprite(sprite_path));
                break;
            case ButtonType.GodPower:
                ret = PowerButtonCreator.CreateGodPowerButton(id, SpriteTextureLoader.getSprite(sprite_path));
                break;
            case ButtonType.Toggle:
                ret = PowerButtonCreator.CreateToggleButton(id, SpriteTextureLoader.getSprite(sprite_path), pNoAutoSetToggleAction: true);
                break;
        }

        return ret;
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
            ret.godPower = AssetManager.powers.get(id);
            ToggleIcon toggle_icon = ret.transform.Find("ToggleIcon").gameObject.GetComponent<ToggleIcon>();
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