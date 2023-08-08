using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cultivation_Way.Core;
using Cultivation_Way.Others;
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
        if (icon.StartsWith("../../"))
        {
            image.sprite = SpriteTextureLoader.getSprite(icon.Replace("../../", ""));
        }
        else
        {
            image.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + icon);
        }
    }
}

/// <summary>
///     在天榜的筛选器组件中添加便于逻辑处理的组件
/// </summary>
internal class TopFilter : MonoBehaviour
{
    private TopFilterContainer curr_filter_container;

    public void select_container(string id)
    {
        curr_filter_container = transform.Find(id).GetComponent<TopFilterContainer>();
    }

    public TopFilterContainer add_container(string id)
    {
        return null;
    }

    public TopFilterCheck get_curr_filter_check()
    {
        return curr_filter_container.get_filter_check();
    }
}

internal class TopFilterContainer : MonoBehaviour
{
    public TopFilterCheck get_filter_check()
    {
        return _ => true;
    }
}

internal abstract class SimpleInfo : MonoBehaviour
{
    public GameObject disp;
    public GameObject info;
    public Text object_name;

    public virtual void load_obj(object obj, string value, string icon_path = "")
    {
        name = object_name.text;
    }
}

internal class SimpleCreatureInfo : SimpleInfo
{
    public StatBar health_bar;
    public Text value_text;
    public Image value_icon;

    public override void load_obj(object obj, string value, string icon_path = "")
    {
        CW_Actor actor = (CW_Actor)obj;
        object_name.text = actor.getName();
        disp.GetComponent<UiUnitAvatarElement>().show_banner_clan = false;
        disp.GetComponent<UiUnitAvatarElement>().show_banner_kingdom = false;
        disp.GetComponent<UiUnitAvatarElement>().show(actor);
        value_text.text = value;
        health_bar.setBar(actor.data.health, actor.getMaxHealth(), $"/{Toolbox.formatNumber(actor.getMaxHealth())}");

        if (string.IsNullOrEmpty(icon_path))
        {
            value_icon.sprite =
                SpriteTextureLoader.getSprite("ui/icons/iconFavoriteStar");
        }
        else
        {
            value_icon.sprite = SpriteTextureLoader.getSprite(icon_path);
        }

        base.load_obj(obj, value);
    }
}

[SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
internal static class Prefabs
{
    public static CW_TipButton tip_button_prefab;
    public static GameObject tip_button_with_bg_game_obj_prefab;
    public static GameObject cultisys_level_edit_prefab;

    public static SimpleCreatureInfo simple_creature_info_prefab;

    private static GameObject edit_bar_prefab;
    private static GameObject input_field_prefab;
    private static Dictionary<string, Object> resources_dict;

    public static void init()
    {
        resources_dict =
            Reflection.GetField(typeof(ResourcesPatch), null, "modsResources") as Dictionary<string, Object>;

        set_input_field_prefab();
        set_tip_button_prefab();
        set_tip_button_with_bg_prefab();
        set_cultisys_level_edit_prefab();
        set_edit_bar_prefab();

        set_simple_creature_info_prefab();

        add_tooltip_element_prefab();
        add_tooltip_cultibook_prefab();
        add_tooltip_blood_nodes_prefab();
        add_tooltip_cultisys_prefab();
    }

    private static void set_simple_creature_info_prefab()
    {
        ScrollWindow.checkWindowExist("favorites");
        GameObject vanllia_favorites_window = Windows.GetWindow("favorites").gameObject;
        vanllia_favorites_window.SetActive(false);
        PrefabUnitElement vanllia_favorite_element_prefab =
            vanllia_favorites_window.GetComponent<WindowFavorites>().element_prefab;

        simple_creature_info_prefab =
            new GameObject("Simple_Creature_Info_Prefab", typeof(SimpleCreatureInfo), typeof(Image))
                .GetComponent<SimpleCreatureInfo>();
        simple_creature_info_prefab.transform.SetParent(CW_Core.ui_prefab_library);
        simple_creature_info_prefab.transform.localScale = new Vector3(1, 1, 1);
        simple_creature_info_prefab.transform.localPosition = new Vector3(0, 0);
        simple_creature_info_prefab.GetComponent<Image>().sprite = FastVisit.get_info_bg();
        simple_creature_info_prefab.GetComponent<Image>().type = Image.Type.Sliced;
        simple_creature_info_prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(193, 31);

        GameObject name = new("Name", typeof(Text));
        name.transform.SetParent(simple_creature_info_prefab.transform);
        name.transform.localPosition = new Vector3(0, 7);
        name.transform.localScale = new Vector3(1, 1);
        name.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 15);
        simple_creature_info_prefab.object_name = name.GetComponent<Text>();
        simple_creature_info_prefab.object_name.alignment = TextAnchor.MiddleCenter;
        simple_creature_info_prefab.object_name.font = LocalizedTextManager.currentFont;
        simple_creature_info_prefab.object_name.resizeTextForBestFit = true;
        simple_creature_info_prefab.object_name.resizeTextMinSize = 1;
        simple_creature_info_prefab.object_name.resizeTextMaxSize = 10;
        simple_creature_info_prefab.object_name.horizontalOverflow = HorizontalWrapMode.Wrap;

        simple_creature_info_prefab.disp =
            Object.Instantiate(vanllia_favorite_element_prefab.avatarElement, simple_creature_info_prefab.transform)
                .gameObject;
        simple_creature_info_prefab.disp.transform.localPosition = new Vector3(-80, 0);
        simple_creature_info_prefab.disp.transform.localScale = new Vector3(1, 1);
        simple_creature_info_prefab.disp.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
        simple_creature_info_prefab.disp.transform.Find("PrefabBanner").gameObject.SetActive(false);
        simple_creature_info_prefab.disp.transform.Find("PrefabBannerClan").gameObject.SetActive(false);

        simple_creature_info_prefab.info = new GameObject("Info");
        simple_creature_info_prefab.info.transform.SetParent(simple_creature_info_prefab.transform);
        simple_creature_info_prefab.info.transform.localPosition = new Vector3(0, -13.67f);
        simple_creature_info_prefab.info.transform.localScale = new Vector3(1, 1);

        GameObject health_bar = Object
            .Instantiate(vanllia_favorite_element_prefab.health_bar, simple_creature_info_prefab.info.transform)
            .gameObject;
        health_bar.transform.localPosition = new Vector3(1f, 0);
        health_bar.transform.localScale = new Vector3(1, 1);
        simple_creature_info_prefab.health_bar = health_bar.GetComponent<StatBar>();

        GameObject value_obj = Object.Instantiate(
            vanllia_favorite_element_prefab.transform.Find("Icons/Kills").gameObject,
            simple_creature_info_prefab.info.transform);
        value_obj.transform.localPosition = new Vector3(50, 6);
        value_obj.transform.localScale = new Vector3(1, 1);
        value_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(64, 12);

        value_obj.transform.Find("Container").GetComponent<HorizontalLayoutGroup>().spacing = 2;

        simple_creature_info_prefab.value_text = value_obj.transform.Find("Container/Text").GetComponent<Text>();
        simple_creature_info_prefab.value_icon = value_obj.transform.Find("Container/Icon").GetComponent<Image>();

        simple_creature_info_prefab.value_text.supportRichText = true;
    }

    private static void set_edit_bar_prefab()
    {
        edit_bar_prefab = new GameObject("Edit_Bar_Prefab", typeof(Image));
        edit_bar_prefab.transform.SetParent(CW_Core.ui_prefab_library);
        edit_bar_prefab.GetComponent<Image>().type = Image.Type.Sliced;
        edit_bar_prefab.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        edit_bar_prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 30);

        Text text_component;

        GameObject comment = new("Comment", typeof(Text), typeof(LocalizedText));
        comment.transform.SetParent(edit_bar_prefab.transform);
        comment.transform.localPosition = new Vector3(-56, 0);
        comment.transform.localScale = new Vector3(1, 1);
        comment.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);

        text_component = comment.GetComponent<Text>();
        text_component.alignment = TextAnchor.MiddleCenter;
        text_component.font = LocalizedTextManager.currentFont;
        text_component.resizeTextForBestFit = true;
        text_component.resizeTextMinSize = 1;
        text_component.resizeTextMaxSize = 10;

        comment.GetComponent<LocalizedText>().text = text_component;

        GameObject name_input = instantiate_input_field(FastVisit.get_window_inner_sliced(), new Vector2(100, 14),
            new Vector2(4, 1),
            edit_bar_prefab.transform);
        name_input.name = "Name_Input";
        name_input.transform.localPosition = new Vector3(32, 0);
    }

    private static void set_cultisys_level_edit_prefab()
    {
        cultisys_level_edit_prefab = new GameObject("Cultisys_Level_Edit_Prefab", typeof(Image));
        cultisys_level_edit_prefab.transform.SetParent(CW_Core.ui_prefab_library);
        cultisys_level_edit_prefab.GetComponent<Image>().type = Image.Type.Sliced;
        cultisys_level_edit_prefab.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        cultisys_level_edit_prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 30);

        Text text_component;

        GameObject comment = new("Comment", typeof(Text));
        comment.transform.SetParent(cultisys_level_edit_prefab.transform);
        comment.transform.localPosition = new Vector3(-75, 0);
        comment.transform.localScale = new Vector3(1, 1);

        text_component = comment.GetComponent<Text>();
        text_component.alignment = TextAnchor.MiddleCenter;
        text_component.font = LocalizedTextManager.currentFont;
        text_component.resizeTextForBestFit = true;
        text_component.resizeTextMinSize = 1;
        text_component.resizeTextMaxSize = 10;

        GameObject name_input = instantiate_input_field(FastVisit.get_window_inner_sliced(), new Vector2(100, 14),
            new Vector2(4, 1),
            cultisys_level_edit_prefab.transform);
        name_input.name = "Name_Input";

        GameObject edit_button = new("Edit", typeof(Image), typeof(Button));
        edit_button.transform.SetParent(cultisys_level_edit_prefab.transform);
        edit_button.transform.localPosition = new Vector3(75, -0.9f);
        edit_button.GetComponent<Image>().sprite = FastVisit.get_red_button();
        edit_button.GetComponent<Image>().type = Image.Type.Sliced;
        edit_button.GetComponent<RectTransform>().sizeDelta = new Vector2(17, 17);

        GameObject edit_button_icon = new("Icon", typeof(Image));
        edit_button_icon.transform.SetParent(edit_button.transform);
        edit_button_icon.transform.localPosition = new Vector3(0, 0);
        edit_button_icon.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/icons/iconCustomWorld");
        edit_button_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(16, 16);
    }

    private static void set_input_field_prefab()
    {
        input_field_prefab = new GameObject("Input_Field_Prefab", typeof(Image));
        input_field_prefab.transform.SetParent(CW_Core.ui_prefab_library);
        input_field_prefab.GetComponent<Image>().type = Image.Type.Sliced;
        input_field_prefab.GetComponent<Image>().sprite = FastVisit.get_window_inner_sliced();

        GameObject input_field = new("Input_Field", typeof(InputField), typeof(Text));
        input_field.transform.SetParent(input_field_prefab.transform);

        InputField input_field_component = input_field.GetComponent<InputField>();
        Text text_component = input_field.GetComponent<Text>();

        input_field_component.textComponent = text_component;

        text_component.alignment = TextAnchor.MiddleCenter;
        text_component.horizontalOverflow = HorizontalWrapMode.Wrap;
        text_component.font = LocalizedTextManager.currentFont;
        text_component.resizeTextForBestFit = true;
        text_component.resizeTextMinSize = 1;
        text_component.resizeTextMaxSize = 10;
    }

    public static GameObject instantiate_input_field(Sprite sprite, Vector2 field_size, Vector2 padding,
        Transform parent)
    {
        GameObject obj = Object.Instantiate(input_field_prefab, parent);
        obj.transform.GetComponent<Image>().sprite = sprite;
        obj.transform.GetComponent<RectTransform>().sizeDelta = field_size + 2 * padding;
        obj.transform.Find("Input_Field").GetComponent<RectTransform>().sizeDelta = field_size;

        return obj;
    }

    public static GameObject instantiate_edit_bar(string comment_key, Transform parent)
    {
        GameObject obj = Object.Instantiate(edit_bar_prefab, parent);
        obj.transform.Find("Comment").GetComponent<LocalizedText>().key = comment_key;
        return obj;
    }

    private static void set_tip_button_with_bg_prefab()
    {
        tip_button_with_bg_game_obj_prefab = new GameObject("Tip_Button_With_BG_Prefab");
        tip_button_with_bg_game_obj_prefab.transform.SetParent(CW_Core.ui_prefab_library);
        GameObject bg = new("BG", typeof(Image));
        bg.transform.SetParent(tip_button_with_bg_game_obj_prefab.transform);

        GameObject tip_button = Object.Instantiate(tip_button_prefab.gameObject,
            tip_button_with_bg_game_obj_prefab.transform);
        tip_button.name = "Tip_Button";
    }

    private static void add_tooltip_element_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.ui_prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "element";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_cultibook_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.ui_prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "cultibook";

        GameObject spells = Object.Instantiate(tooltip.transform.Find("Stats").gameObject,
            tooltip.transform.Find("Stats").parent);
        spells.transform.localScale = new Vector3(1, 1);
        spells.transform.SetSiblingIndex(2);
        spells.name = "Spells";
        Text text;
        text = spells.transform.Find("StatsDescription").GetComponent<Text>();
        text.fontStyle = FontStyle.Normal;
        text.font = LocalizedTextManager.currentFont;

        text = spells.transform.Find("StatsValues").GetComponent<Text>();
        text.fontStyle = FontStyle.Normal;
        text.font = LocalizedTextManager.currentFont;

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_blood_nodes_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.ui_prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "blood_nodes";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void add_tooltip_cultisys_prefab()
    {
        Tooltip tooltip =
            Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.ui_prefab_library);
        tooltip.gameObject.name = Constants.Core.mod_prefix + "cultisys";

        resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
    }

    private static void set_tip_button_prefab()
    {
        GameObject _obj = new("Tip_Button_Prefab");
        _obj.SetActive(false);
        _obj.transform.SetParent(CW_Core.ui_prefab_library);
        _obj.AddComponent<Button>();
        _obj.AddComponent<Image>();
        _obj.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
        tip_button_prefab = _obj.AddComponent<CW_TipButton>();
    }
}