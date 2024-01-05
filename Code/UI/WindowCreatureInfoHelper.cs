using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.UI.prefabs;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

internal class CultiProgress : MonoBehaviour
{
    private StatBar bar;
    private Image bar_image;
    private TipButton tip_button;

    private void Awake()
    {
        init();
    }

    private void init()
    {
        bar = GetComponent<StatBar>();
        bar_image = transform.Find("Mask/Bar").GetComponent<Image>();
        bar_image.color = Color.white;

        bar_image.enabled = false;
        transform.Find("Background").gameObject.SetActive(false);
        transform.Find("Text").gameObject.SetActive(false);
        transform.Find("Icon").gameObject.SetActive(false);

        tip_button = GetComponent<TipButton>();
        tip_button.hoverAction = null;
    }

    public void clear()
    {
        if (bar_image != null) bar_image.enabled = false;
        transform.Find("Background").gameObject.SetActive(false);
        transform.Find("Text").gameObject.SetActive(false);
        transform.Find("Icon").gameObject.SetActive(false);
        if (tip_button != null)
        {
            tip_button = GetComponent<TipButton>();
            tip_button.hoverAction = null;
        }
    }

    public void load_cultisys(CultisysAsset cultisys, int cultisys_level, CultisysType type, CW_Actor actor)
    {
        if (bar_image == null) init();
        bar_image.enabled = true;
        transform.Find("Background").gameObject.SetActive(true);
        transform.Find("Text").gameObject.SetActive(true);
        transform.Find("Icon").gameObject.SetActive(true);
        transform.Find("Icon").GetComponent<Image>().sprite = SpriteTextureLoader.getSprite(cultisys.sprite_path);

        float max = (int)cultisys.max_progress(actor, cultisys, cultisys_level);
        float curr = (int)cultisys.curr_progress(actor, cultisys, cultisys_level);
        bar.setBar(curr, max, $"/{max}");

        bar_image.color = type switch
        {
            CultisysType.BODY => Color.red,
            CultisysType.SOUL => Color.gray,
            CultisysType.WAKAN => Color.blue,
            _ => Color.white
        };
        if (cultisys.culti_energy != null)
        {
            bar_image.color = cultisys.culti_energy.get_color(curr, cultisys.power_level[cultisys_level]);
        }

        tip_button.hoverAction = () =>
        {
            Tooltip.show(gameObject, Constants.Core.mod_prefix + "cultisys", new TooltipData
            {
                actor = actor,
                tip_name = cultisys.id
            });
        };
    }
}

internal class WindowCreatureInfoHelper
{
    public static CityIcon spell_armor;
    public static CityIcon crit_damage_mod;
    public static CityIcon throns;
    public static CityIcon knockback_reduction;
    public static CityIcon health_regen;
    public static CityIcon shield_regen;
    public static CityIcon wakan_regen;
    public static CityIcon culti_velo_co;

    public static CW_TipButton element;
    public static CW_TipButton cultibook;
    public static CW_TipButton blood;

    public static CultiProgress body_progress;
    public static CultiProgress soul_progress;
    public static CultiProgress wakan_progress;

    public static Transform content_transform;
    public static Transform background_transform;
    public static Transform stat_icons_transform;
    private static bool initialized;
    private static bool first_open = true;
    public static RectTransform drag_receiver;
    private static Text drag_receiver_text;

    private static SubSelectWindow award_select_window;

    public static void init(ScrollWindow scroll_window)
    {
        if (initialized) return;
        background_transform = scroll_window.transform.Find("Background");
        content_transform = scroll_window.transform.Find("Background/Scroll View/Viewport/Content");

        #region StatIcons

        stat_icons_transform = content_transform.Find("StatIcons");
        GridLayoutGroup grid_layout_group = stat_icons_transform.GetComponent<GridLayoutGroup>();
        grid_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid_layout_group.constraintCount = 9;

        GameObject new_stats;
        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "spell_armor";
        spell_armor = new_stats.GetComponent<CityIcon>();
        spell_armor.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconSpell_Armor");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.spell_armor).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "crit_damage_mod";
        crit_damage_mod = new_stats.GetComponent<CityIcon>();
        crit_damage_mod.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconBloodlust");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(S.critical_damage_multiplier).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "anti_injury";
        throns = new_stats.GetComponent<CityIcon>();
        throns.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconAnti_Injury");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.throns).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "knockback_reduction";
        knockback_reduction = new_stats.GetComponent<CityIcon>();
        knockback_reduction.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconKnockback_Reduction");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(S.knockback_reduction).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "health_regen";
        health_regen = new_stats.GetComponent<CityIcon>();
        health_regen.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconRegeneration");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.health_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "shield_regen";
        shield_regen = new_stats.GetComponent<CityIcon>();
        shield_regen.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/tech/icon_tech_defense_strategy");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.shield_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "wakan_regen";
        wakan_regen = new_stats.GetComponent<CityIcon>();
        wakan_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.wakan_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "culti_velo_co";
        culti_velo_co = new_stats.GetComponent<CityIcon>();
        culti_velo_co.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconCultiSys");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.mod_cultivelo).translation_key;
        new_stats.GetComponent<TipButton>().textOnClickDescription = "tip_culti_velo_co";

        #endregion

        content_transform.Find("Part 1/MoodBG").localPosition = new Vector3(-91.9f, 15);
        background_transform.Find("Scroll View/Viewport").GetComponent<Mask>().enabled = true;
        background_transform.Find("Scroll View/Viewport").GetComponent<Image>().enabled = true;
        content_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;
        background_transform.Find("Scroll View").GetComponent<ScrollRect>().enabled = true;
        stat_icons_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.MinSize;

        #region Left

        GameObject left_part = new("Left", typeof(Image), typeof(GridLayoutGroup));
        left_part.transform.SetParent(background_transform);
        left_part.transform.localScale = new Vector3(1, 1);
        left_part.transform.localPosition = new Vector3(-250, 0, 0);
        left_part.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        left_part.GetComponent<Image>().type = Image.Type.Sliced;
        left_part.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 128);
        element = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        cultibook = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        blood = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        element.name = "element";
        cultibook.name = "cultibook";
        blood.name = "blood";
        grid_layout_group = left_part.GetComponent<GridLayoutGroup>();
        grid_layout_group.cellSize = new Vector2(32, 32);
        grid_layout_group.spacing = new Vector2(4, 4);
        grid_layout_group.padding = new RectOffset(4, 4, 8, 4);
        grid_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid_layout_group.constraintCount = 1;

        #endregion

        body_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        body_progress.name = "BodyCultiProgress";
        body_progress.transform.localPosition = new Vector3(-165, 50, 0);
        body_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        body_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        body_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        body_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        body_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //body_progress.transform.Find("Text").localPosition += new Vector3(7, 0);
        wakan_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        wakan_progress.name = "WakanCultiProgress";
        wakan_progress.transform.localPosition = new Vector3(-165, 0, 0);
        wakan_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        wakan_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        wakan_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        wakan_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        wakan_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //wakan_progress.transform.Find("Text").localPosition += new Vector3(7, 0);
        soul_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        soul_progress.name = "SoulCultiProgress";
        soul_progress.transform.localPosition = new Vector3(-165, -50, 0);
        soul_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        soul_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        soul_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        soul_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        soul_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //soul_progress.transform.Find("Text").localPosition += new Vector3(7, 0);

        GameObject receiver = new("Drag Receiver", typeof(Image));
        receiver.transform.SetParent(background_transform);
        receiver.transform.localPosition = new Vector3(-191, -109);
        receiver.transform.localScale = Vector3.one;
        receiver.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        receiver.GetComponent<Image>().type = Image.Type.Sliced;

        drag_receiver = receiver.GetComponent<RectTransform>();
        drag_receiver.sizeDelta = new Vector2(150, 60);
        drag_receiver.gameObject.SetActive(false);

        GameObject receiver_text = new("Text", typeof(Text));
        receiver_text.transform.SetParent(receiver.transform);
        receiver_text.transform.localPosition = Vector3.zero;
        receiver_text.transform.localScale = Vector3.one;
        Text text = receiver_text.GetComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextForBestFit = false;
        text.font = LocalizedTextManager.currentFont;
        text.fontSize = 12;
        text.GetComponent<RectTransform>().sizeDelta = drag_receiver.sizeDelta * 0.95f;
        drag_receiver_text = text;


        award_select_window = Object.Instantiate(SubSelectWindow.Prefab, background_transform);
        award_select_window.transform.localScale = Vector3.one;
        award_select_window.transform.localPosition = Vector3.zero;
        award_select_window.gameObject.SetActive(false);

        var element_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var cultibook_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var equipment_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var elixir_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var blood_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var cultisys_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var spell_award_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var possession_entry = Object.Instantiate(SimpleButton.Prefab, null);
        var child_born_entry = Object.Instantiate(SimpleButton.Prefab, null);

        element_award_entry.Setup([Hotfixable]() =>
        {
            var element = Config.selectedUnit.data.GetElement();
            element.ReRandom();
            Config.selectedUnit.data.SetElement(element);
            scroll_window.GetComponent<WindowCreatureInfo>().OnEnable();
            WorldTip.showNow(
                LM.Get("ElementAdjust Result").Replace("$element$",
                    LM.Get(Config.selectedUnit.data.GetElement().GetElementType().id)), false, "top");
        }, SpriteTextureLoader.getSprite("ui/icons/iconElement"), pSize: new Vector2(32, 32), pTipData: new TooltipData
        {
            tip_name = "ElementAdjust",
            tip_description = "ElementAdjust Description"
        }, pTipType: "normal");
        cultibook_award_entry.Setup(() =>
            {
                ScrollWindow.moveAllToLeftAndRemove();
                ScrollWindow.showWindow(nameof(WindowCultibookLibrary));
                CW_Core.mod_state.is_awarding = true;
            }, SpriteTextureLoader.getSprite("ui/icons/iconCultiBook_immortal"), pSize: new Vector2(32, 32),
            pTipData: new TooltipData
            {
                tip_name = nameof(WindowCultibookLibrary),
                tip_description = nameof(WindowCultibookLibrary) + Constants.Core.new_desc_suffix
            }, pTipType: "normal");
        equipment_award_entry.Setup(() =>
            {
                ScrollWindow.moveAllToLeftAndRemove();
                ScrollWindow.showWindow(nameof(WindowItemLibrary));
                CW_Core.mod_state.is_awarding = true;
            }, SpriteTextureLoader.getSprite("ui/icons/items/icon_紫金葫芦_violet_gold"), pSize: new Vector2(32, 32),
            pTipData: new TooltipData
            {
                tip_name = nameof(WindowItemLibrary),
                tip_description = nameof(WindowItemLibrary) + Constants.Core.new_desc_suffix
            }, pTipType: "normal");
        elixir_award_entry.Setup(() =>
        {
            ScrollWindow.moveAllToLeftAndRemove();
            ScrollWindow.showWindow(nameof(WindowElixirLibrary));
            CW_Core.mod_state.is_awarding = true;
        }, SpriteTextureLoader.getSprite("ui/icons/elixirs/iconNormal"), pSize: new Vector2(32, 32));
        blood_award_entry.Setup(() =>
            {
                ScrollWindow.moveAllToLeftAndRemove();
                ScrollWindow.showWindow(nameof(WindowBloodLibrary));
                CW_Core.mod_state.is_awarding = true;
            }, SpriteTextureLoader.getSprite("ui/icons/iconWus"), pSize: new Vector2(32, 32),
            pTipData: new TooltipData
            {
                tip_name = nameof(WindowBloodLibrary),
                tip_description = nameof(WindowBloodLibrary) + Constants.Core.new_desc_suffix
            }, pTipType: "normal");
        cultisys_award_entry.Setup(() =>
        {
            ScrollWindow.moveAllToLeftAndRemove();
            ScrollWindow.showWindow(nameof(WindowCultiConfig));
            CW_Core.mod_state.is_awarding = true;
        }, SpriteTextureLoader.getSprite("ui/icons/iconCultiSys"), pSize: new Vector2(32, 32));
        cultisys_award_entry.Icon.GetComponent<RectTransform>().sizeDelta = new Vector2(22.4f, 28);
        spell_award_entry.Setup(() =>
        {
            ScrollWindow.moveAllToLeftAndRemove();
            ScrollWindow.showWindow(nameof(WindowSpellLibrary));
            CW_Core.mod_state.is_awarding = true;
        }, SpriteTextureLoader.getSprite("ui/cw_icons/天师"), pSize: new Vector2(32, 32));
        possession_entry.Setup(() =>
        {
            ScrollWindow.moveAllToLeftAndRemove();
            ScrollWindow.showWindow(nameof(WindowActorLibrary));
            CW_Core.mod_state.is_awarding = true;
        }, SpriteTextureLoader.getSprite("ui/cw_icons/iconPossession"), pSize: new Vector2(32, 32));
        child_born_entry.Setup(() =>
        {
            ScrollWindow.moveAllToLeftAndRemove();
            ScrollWindow.showWindow(nameof(WindowChildConfig));
            CW_Core.mod_state.is_awarding = true;
        }, SpriteTextureLoader.getSprite("ui/icons/worldrules/icon_lastofus"), pSize: new Vector2(32, 32));


        award_select_window.Setup(new List<RectTransform>
        {
            element_award_entry.GetComponent<RectTransform>(),
            cultisys_award_entry.GetComponent<RectTransform>(),
            spell_award_entry.GetComponent<RectTransform>(),
            cultibook_award_entry.GetComponent<RectTransform>(),
            blood_award_entry.GetComponent<RectTransform>(),
            equipment_award_entry.GetComponent<RectTransform>(),
            elixir_award_entry.GetComponent<RectTransform>(),
            possession_entry.GetComponent<RectTransform>(),
            child_born_entry.GetComponent<RectTransform>()
        }, LM.Get("Award"), LM.Get("Award Description"), new Vector2(364, 140));
        award_select_window.GetComponent<VerticalLayoutGroup>().spacing = 8;


        var award_entry = Object.Instantiate(SimpleButton.Prefab, background_transform);
        award_entry.transform.localPosition = new Vector3(-125, 100);
        award_entry.transform.localScale = Vector3.one;
        award_entry.Setup([Hotfixable]() =>
        {
            award_select_window.transform.SetAsLastSibling();
            award_select_window.gameObject.SetActive(true);
        }, SpriteTextureLoader.getSprite("ui/cw_icons/iconAwardUnit"), pSize: new Vector2(28, 28));
        award_entry.TipButton.enabled = true;
        award_entry.TipButton.textOnClick = "Award";
        award_entry.TipButton.textOnClickDescription = "Award Description";
        var anim = award_entry.gameObject.AddComponent<IconRotationAnimation>();
        anim.image = award_entry.Background;
        anim.delay = 1.5f;
        anim.initScale = new Vector3(1, 1, 1);
        anim.scaleTo = new Vector3(1.1f, 1.1f, 1.1f);


        EventTrigger.Entry entry;
        EventTrigger event_trigger;

        #region Cultibook Draggable

        event_trigger = cultibook.GetComponent<EventTrigger>();
        if (event_trigger == null) event_trigger = cultibook.gameObject.AddComponent<EventTrigger>();

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            drag_receiver_text.color = Color.white;
            drag_receiver.gameObject.SetActive(true);
            cultibook.transform.SetParent(background_transform);
            cultibook.transform.localScale = Vector3.one;
            cultibook.transform.position = pointerEventData.position;
        });
        event_trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                drag_receiver_text.color = Color.yellow;
            else
                drag_receiver_text.color = Color.white;

            cultibook.transform.position = pointerEventData.position;
        });
        event_trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
            {
                var copied = new Cultibook();
                copied.copy_from(Config.selectedUnit.data.GetCultibook());

                WindowCultibookLibrary.Instance.PushData(copied);
            }

            cultibook.transform.SetParent(left_part.transform);
            cultibook.transform.SetSiblingIndex(1);
            drag_receiver.gameObject.SetActive(false);
        });
        event_trigger.triggers.Add(entry);

        #endregion

        #region Blood Draggable

        event_trigger = blood.GetComponent<EventTrigger>();
        if (event_trigger == null) event_trigger = blood.gameObject.AddComponent<EventTrigger>();

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            drag_receiver_text.color = Color.white;
            drag_receiver.gameObject.SetActive(true);
            blood.transform.SetParent(background_transform);
            blood.transform.localScale = Vector3.one;
            blood.transform.position = pointerEventData.position;
        });
        event_trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                drag_receiver_text.color = Color.yellow;
            else
                drag_receiver_text.color = Color.white;

            blood.transform.position = pointerEventData.position;
        });
        event_trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener([Hotfixable](data) =>
        {
            if (data is not PointerEventData pointerEventData) return;

            if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                WindowBloodLibrary.Instance.PushData(Config.selectedUnit.data.GetBloodNodes());

            blood.transform.SetParent(left_part.transform);
            blood.transform.SetSiblingIndex(2);
            drag_receiver.gameObject.SetActive(false);
        });
        event_trigger.triggers.Add(entry);

        #endregion

        initialized = true;
    }

    [Hotfixable]
    public static void OnEnable_postfix(WindowCreatureInfo window_creature_info)
    {
        if (!initialized) return;

        award_select_window.gameObject.SetActive(false);

        CW_Actor actor = (CW_Actor)window_creature_info.actor;

        actor.setStatsDirty();
        actor.updateStats();

        #region StatIcons

        spell_armor.gameObject.SetActive(true);
        crit_damage_mod.gameObject.SetActive(true);
        throns.gameObject.SetActive(true);
        knockback_reduction.gameObject.SetActive(true);
        health_regen.gameObject.SetActive(true);
        shield_regen.gameObject.SetActive(true);
        wakan_regen.gameObject.SetActive(true);
        culti_velo_co.gameObject.SetActive(true);
        if (!actor.asset.inspect_stats)
        {
            spell_armor.gameObject.SetActive(false);
            crit_damage_mod.gameObject.SetActive(false);
            throns.gameObject.SetActive(false);
            knockback_reduction.gameObject.SetActive(false);
            health_regen.gameObject.SetActive(false);
            shield_regen.gameObject.SetActive(false);
            wakan_regen.gameObject.SetActive(false);
            culti_velo_co.gameObject.SetActive(false);
        }

        window_creature_info.armor.setValue(actor.stats[S.armor]);
        spell_armor.setValue(actor.stats[CW_S.spell_armor]);
        crit_damage_mod.setValue(actor.stats[S.critical_damage_multiplier]);
        throns.setValue(actor.stats[CW_S.throns]);
        knockback_reduction.setValue(actor.stats[S.knockback_reduction]);
        health_regen.setValue(actor.stats[CW_S.health_regen]);
        shield_regen.setValue(actor.stats[CW_S.shield_regen]);
        wakan_regen.setValue(actor.stats[CW_S.wakan_regen]);
        culti_velo_co.setValue((1 + actor.stats[CW_S.mod_cultivelo]) * actor.cw_asset.culti_velo);

        #endregion

        element.gameObject.SetActive(false);
        cultibook.gameObject.SetActive(false);
        blood.gameObject.SetActive(false);


        if (actor.data.GetElement() != null) load_element(actor);

        if (actor.data.GetCultibook() != null) load_cultibook(actor);

        if (actor.data.GetBloodNodes() != null) load_blood(actor);

        //body_progress.load_cultisys(null, 0, CultisysType.BODY, actor);
        body_progress.clear();
        wakan_progress.clear();
        soul_progress.clear();
        int[] cultisys_level = actor.data.GetAllCultisysLevels();
        for (int i = 0; i < Manager.cultisys.size; i++)
        {
            if (cultisys_level[i] < 0) continue;
            CultisysAsset cultisys = Manager.cultisys.list[i];
            switch (cultisys.type)
            {
                case CultisysType.BODY:
                    body_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.BODY, actor);
                    break;
                case CultisysType.SOUL:
                    soul_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.SOUL, actor);
                    break;
                case CultisysType.WAKAN:
                    wakan_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.WAKAN, actor);
                    break;
            }
        }

        drag_receiver_text.text = LM.Get("DragItemOrActorHere");
        load_cw_statuses(actor, window_creature_info);
        patch_equipment_buttons_as_draggable(window_creature_info);
    }

    private static void patch_equipment_buttons_as_draggable(WindowCreatureInfo pWindowCreatureInfo)
    {
        foreach (var component in pWindowCreatureInfo.equipmentParent.GetComponentsInChildren(typeof(EquipmentButton),
                     false))
        {
            var equip_button = (EquipmentButton)component;
            EventTrigger button = equip_button.GetComponent<EventTrigger>();
            if (button == null)
            {
                button = equip_button.gameObject.AddComponent<EventTrigger>();
            }

            bool need_add_trigger = true;
            foreach (var trigger in button.triggers)
            {
                if (trigger.eventID == EventTriggerType.EndDrag)
                {
                    need_add_trigger = false;
                    break;
                }
            }

            if (!need_add_trigger) continue;
            EventTrigger.Entry entry;

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData)
                {
                    return;
                }

                drag_receiver_text.color = Color.white;
                drag_receiver.gameObject.SetActive(true);
                equip_button.transform.SetParent(background_transform);
                equip_button.transform.localScale = Vector3.one;
                equip_button.transform.position = pointerEventData.position;
            });
            button.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData)
                {
                    return;
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                {
                    drag_receiver_text.color = Color.yellow;
                }
                else
                {
                    drag_receiver_text.color = Color.white;
                }

                equip_button.transform.position = pointerEventData.position;
            });
            button.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData)
                {
                    return;
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                {
                    equip_button.gameObject.SetActive(false);
                    pWindowCreatureInfo.pool_equipment._elements_inactive.Push(equip_button);

                    ItemData item_data = equip_button.item_data;
                    ItemAsset item_asset = AssetManager.items.get(item_data.id);

                    WindowItemLibrary.CollectItem(item_data);

                    pWindowCreatureInfo.actor.equipment.getSlot(item_asset.equipmentType).emptySlot();
                    pWindowCreatureInfo.actor.setStatsDirty();
                }

                equip_button.transform.SetParent(pWindowCreatureInfo.equipmentParent);
                drag_receiver.gameObject.SetActive(false);
            });
            button.triggers.Add(entry);
        }
    }

    private static void load_cw_statuses(CW_Actor actor, WindowCreatureInfo window_creature_info)
    {
        if (actor.statuses == null || actor.statuses.Count == 0) return;

        foreach (CW_StatusEffectData status_effect_data in actor.statuses.Values)
        {
            if (status_effect_data.finished) continue;
            StatusEffectData fake_status_effect_data =
                new(actor, AssetManager.status.get(status_effect_data.status_asset.id));
            fake_status_effect_data.setTimer(status_effect_data.left_time);

            window_creature_info.loadStatusButton(fake_status_effect_data);
        }
    }

    public static void Update_postfix(WindowCreatureInfo window_creature_info)
    {
        if (!initialized) return;
        if (first_open)
        {
            first_open = false;
            OnEnable_postfix(window_creature_info);
        }
    }

    private static void load_element(CW_Actor actor)
    {
        element.gameObject.SetActive(true);
        element.load("iconElement", obj =>
        {
            Tooltip.show(obj, Constants.Core.mod_prefix + "element", new TooltipData
            {
                actor = actor
            });
        });
    }

    private static void load_cultibook(CW_Actor actor)
    {
        cultibook.gameObject.SetActive(true);
        cultibook.load("iconCultiBook_immortal", obj =>
        {
            Tooltip.show(obj, Constants.Core.mod_prefix + "cultibook", new TooltipData
            {
                actor = actor
            });
        });
    }

    private static void load_blood(CW_Actor actor)
    {
        blood.gameObject.SetActive(true);
        blood.load(actor.asset.icon, obj =>
        {
            Tooltip.show(obj, Constants.Core.mod_prefix + "blood_nodes", new TooltipData
            {
                actor = actor
            });
        });
    }
}