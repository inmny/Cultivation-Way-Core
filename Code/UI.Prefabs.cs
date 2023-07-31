using System;
using System.Collections.Generic;
using DG.Tweening;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cultivation_Way.UI;

internal class CW_TipButton : MonoBehaviour
{
    public Button button;
    public Image image;
    private Vector3 origin_scale = new(-1, -1);
    private Action<GameObject> tooltip_action;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        button.onClick.AddListener(show_tooltip);
        button.OnHover(show_tooltip);
        button.OnHoverOut(Tooltip.hideTooltip);
    }

    private void show_tooltip()
    {
        tooltip_action?.Invoke(gameObject);

        if (origin_scale is { x: < 0, y: < 0 })
        {
            origin_scale = transform.localScale;
        }

        transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y * 1.25f, 1f);
        transform.DOKill(true);
        transform.DOScale(origin_scale, 0.1f).SetEase(Ease.InBack);
    }

    public void load(string icon, Action<GameObject> show_tooltip)
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        tooltip_action = show_tooltip;
        image.sprite = Resources.Load<Sprite>("ui/Icons/" + icon);
    }
}

internal static class Prefabs
{
    public static CW_TipButton tip_button_prefab;
    public static GameObject tip_button_with_bg_game_obj_prefab;
    private static Dictionary<string, Object> resources_dict;

    public static void init()
    {
        resources_dict =
            Reflection.GetField(typeof(ResourcesPatch), null, "modsResources") as Dictionary<string, Object>;

        set_tip_button_prefab();
        set_tip_button_with_bg_prefab();
        add_tooltip_element_prefab();
        add_tooltip_cultibook_prefab();
        add_tooltip_blood_nodes_prefab();
        add_tooltip_cultisys_prefab();
    }

    private static void set_tip_button_with_bg_prefab()
    {
        tip_button_with_bg_game_obj_prefab = new GameObject("Tip_Button_With_BG_Prefab");
        tip_button_with_bg_game_obj_prefab.transform.SetParent(CW_Core.prefab_library);
        GameObject bg = new("BG", typeof(Image));
        bg.transform.SetParent(tip_button_with_bg_game_obj_prefab.transform);

        GameObject tip_button = Object.Instantiate(tip_button_prefab.gameObject,
            tip_button_with_bg_game_obj_prefab.transform);
        tip_button.name = "Tip_Button";
    }

    private static void add_tooltip_element_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "element";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_cultibook_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "cultibook";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_blood_nodes_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "blood_nodes";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_cultisys_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "cultisys";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void set_tip_button_prefab()
    {
        GameObject _obj = new("Tip_Button_Prefab");
        _obj.SetActive(false);
        _obj.transform.SetParent(CW_Core.prefab_library);
        _obj.AddComponent<Button>();
        _obj.AddComponent<Image>();
        _obj.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
        tip_button_prefab = _obj.AddComponent<CW_TipButton>();
    }
}