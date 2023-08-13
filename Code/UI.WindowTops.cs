using System;
using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowTops : AbstractWindow<WindowTops>
{
    private GameObject creature_entry;
    private GameObject cultibook_entry;
    private GameObject blood_entry;
    private GameObject item_entry;
    private GameObject pope_entry;
    private GameObject city_entry;
    private GameObject kingdom_entry;
    private GameObject clan_entry;
    private GameObject no_item;

    private GameObject sort_key;
    private GameObject filter;

    private TopValueCalc curr_value_calc;
    private TopValueShow curr_value_show;
    private TopFilterCheck curr_filter;
    private string curr_icon;
    private SimpleInfo curr_info_prefab;
    private readonly int show_count = 10;
    private readonly List<object> curr_list = new();

    private readonly Dictionary<string, TopValueCalc> calcs = new();
    private readonly Dictionary<string, TopValueShow> shows = new();
    private readonly Dictionary<string, string> icons = new();
    private readonly Dictionary<string, TopFilterCheck> filter_funcs = new();

    private readonly string[] top_ids =
    {
        "creature",
        "cultibook",
        "blood",
        "item",
        "pope",
        "city",
        "kingdom",
        "clan"
    };

    internal static void init()
    {
        base_init(Constants.Core.tops_window);

        background_transform.Find("Title").GetComponent<Text>().font = Fonts.STLiti;

        #region 八个榜单的按钮

        GameObject button_prefab =
            Instantiate(Prefabs.tip_button_with_bg_game_obj_prefab.gameObject, CW_Core.ui_prefab_library);
        button_prefab.name = "Top_Button_Prefab";
        Transform button_prefab_bg = button_prefab.transform.Find("BG");
        button_prefab_bg.GetComponent<RectTransform>().sizeDelta = new Vector2(42, 42);
        button_prefab_bg.GetComponent<Image>().sprite = FastVisit.get_window_big_close();
        button_prefab_bg.eulerAngles = new Vector3(0, 0, 270);

        void add_entry(string entry_name, string icon, Vector3 pos, bool bg_rotate = false)
        {
            GameObject top_entry = Instantiate(button_prefab, background_transform);
            top_entry.name = entry_name;
            top_entry.transform.localPosition = pos;
            top_entry.transform.Find("Tip_Button").GetComponent<CW_TipButton>().load(icon, obj =>
            {
                Tooltip.show(obj, "normal", new TooltipData
                {
                    tip_name = $"{Constants.Core.mod_prefix}{entry_name}_top"
                });
            });
            if (bg_rotate)
            {
                top_entry.transform.Find("BG").eulerAngles = new Vector3(0, 0, 90);
            }

            top_entry.transform.Find("Tip_Button").GetComponent<CW_TipButton>().button.onClick
                .AddListener(() => { instance.CallMethod($"switch_to_{entry_name}", null); });
            top_entry.transform.Find("Tip_Button").gameObject.SetActive(true);
            Reflection.SetField(instance, $"{entry_name}_entry", top_entry);
        }

        add_entry("creature", "iconEastern_Humans", new Vector3(119, 86.5f));
        add_entry("cultibook", "iconCultiBook_immortal", new Vector3(119, 30.5f));
        add_entry("blood", "iconCultiBook_immortal", new Vector3(119, -50.5f));
        add_entry("item", "iconCultiBook_immortal", new Vector3(117, -100.5f));
        add_entry("pope", "iconCultiBook_immortal", new Vector3(-119, 86.5f), true);
        add_entry("city", "iconCityList", new Vector3(-119, 30.5f), true);
        add_entry("kingdom", "iconKingdomList", new Vector3(-119, -50.5f), true);
        add_entry("clan", "iconClan", new Vector3(-117, -100.5f), true);

        #endregion

        #region 设置默认显示内容

        ScrollWindow.checkWindowExist("favorites");
        GameObject origin_favorites_gameobject = Windows.GetWindow("favorites").gameObject;
        instance.no_item =
            Instantiate(
                origin_favorites_gameobject.transform.Find("Background/Scroll View/Viewport/Content/No Items")
                    .gameObject, content_transform);
        instance.no_item.name = "No_Items";
        origin_favorites_gameobject.SetActive(false);
        Sprite no_item_sprite = Resources.Load<Sprite>("ui/Icons/iconTop");
        instance.no_item.transform.Find("BG").GetComponent<Image>().sprite = no_item_sprite;
        instance.no_item.transform.Find("InnerBG/Left").GetComponent<Image>().sprite = no_item_sprite;
        instance.no_item.transform.Find("InnerBG/Right").GetComponent<Image>().sprite = no_item_sprite;
        instance.no_item.transform.Find("InnerBG/Description").GetComponent<LocalizedText>().key =
            "cw_Top_No_Items_Description";
        instance.no_item.SetActive(true);

        #endregion

        #region 排序关键词选择器

        instance.sort_key = new GameObject("Sort_Key");
        instance.sort_key.transform.SetParent(background_transform);
        instance.sort_key.transform.localPosition = new Vector3(0, 0);
        instance.sort_key.transform.localScale = new Vector3(1, 1);

        void add_sort_key_container(string id)
        {
            GameObject sort_key_container = new(id, typeof(GridLayoutGroup));
            sort_key_container.transform.SetParent(instance.sort_key.transform);
            sort_key_container.transform.localPosition = new Vector3(240, 5);
            sort_key_container.transform.localScale = new Vector3(1, 1);
            sort_key_container.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);

            GridLayoutGroup layout = sort_key_container.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(24, 24);
            layout.spacing = new Vector2(4, 0);
            sort_key_container.SetActive(false);
        }

        foreach (string top_id in instance.top_ids)
        {
            add_sort_key_container(top_id);
        }

        #endregion

        #region 筛选器

        instance.filter = new GameObject("Filter");
        instance.filter.transform.SetParent(background_transform);

        void add_filter_container(string id)
        {
            GameObject _filter = new(id);
            _filter.transform.SetParent(instance.filter.transform);
            _filter.transform.localPosition = new Vector3(-200, 55);
            _filter.transform.localScale = new Vector3(1, 1);

            // 筛选器类型, 上方的按钮
            GameObject filter_type_select_part =
                new("Detailed Filters", typeof(HorizontalLayoutGroup), typeof(TopFilter));
            filter_type_select_part.transform.SetParent(_filter.transform);
            filter_type_select_part.transform.localPosition = new Vector3(0, 0);
            filter_type_select_part.transform.localScale = new Vector3(1, 1);
            filter_type_select_part.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 20);

            HorizontalLayoutGroup filter_type_select_part_layout =
                filter_type_select_part.GetComponent<HorizontalLayoutGroup>();
            filter_type_select_part_layout.childControlHeight = false;
            filter_type_select_part_layout.childControlWidth = false;
            filter_type_select_part_layout.childForceExpandHeight = false;
            filter_type_select_part_layout.childForceExpandWidth = false;
            filter_type_select_part_layout.childAlignment = TextAnchor.MiddleCenter;
            filter_type_select_part_layout.spacing = 4;

            // 筛选器容器, 下方的具体内容
            GameObject filter_container = new("Container", typeof(TopFilterContainer));
            filter_container.transform.SetParent(_filter.transform);

            _filter.SetActive(false);
        }

        foreach (string top_id in instance.top_ids)
        {
            add_filter_container(top_id);
        }

        #endregion

        instance.filter_funcs["default"] = _ => true;

        #region 列表整理

        VerticalLayoutGroup content_layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();

        content_layout.childControlHeight = false;
        content_layout.childControlWidth = false;
        content_layout.childForceExpandHeight = false;
        content_layout.childForceExpandWidth = false;
        content_layout.childAlignment = TextAnchor.UpperCenter;
        content_layout.spacing = 8;
        content_layout.padding = new RectOffset(0, 0, 8, 8);

        ContentSizeFitter content_fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        content_fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        content_fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        #endregion

        initialized = true;
    }

    internal static void post_init()
    {
        foreach (string top_id in instance.top_ids)
        {
            try
            {
                instance.CallMethod($"add_{top_id}_sort_keys", instance.sort_key.transform.Find(top_id));
            }
            catch (Exception)
            {
                // 忽略, 仅说明该分榜未实现
            }
        }
    }

    private void OnEnable()
    {
        if (!initialized) return;
        clear_content();
        if (curr_value_calc != null)
        {
            foreach (string top_id in top_ids)
            {
                if (!sort_key.transform.Find(top_id).gameObject.activeSelf) continue;
                this.CallMethod($"load_{top_id}_list");
                break;
            }

            show();
        }
    }

    private void set_sort_key(string id)
    {
        curr_value_calc = calcs[id];
        curr_value_show = shows[id];
        curr_icon = icons.TryGetValue(id, out string icon) ? icon : null;
    }

    private void switch_to_creature()
    {
        clear();
        load_creature_list();
        curr_info_prefab = Prefabs.simple_creature_info_prefab;
        instance.sort_key.transform.Find("creature").gameObject.SetActive(true);
        foreach (Transform child in instance.sort_key.transform.Find("creature"))
        {
            child.gameObject.SetActive(true);
        }

        set_sort_key("actor_age");
        curr_filter = filter_funcs["default"];
        show();
    }

    private void switch_to_cultibook()
    {
        clear();
        load_cultibook_list();
        curr_info_prefab = Prefabs.simple_cultibook_info_prefab;
        instance.sort_key.transform.Find("cultibook").gameObject.SetActive(true);
        foreach (Transform child in instance.sort_key.transform.Find("cultibook"))
        {
            child.gameObject.SetActive(true);
        }

        set_sort_key("cultibook_level");
        curr_filter = filter_funcs["default"];
        show();
    }

    private void switch_to_blood()
    {
    }

    private void switch_to_item()
    {
    }

    private void switch_to_pope()
    {
    }

    private void switch_to_city()
    {
    }

    private void switch_to_kingdom()
    {
    }

    private void switch_to_clan()
    {
    }

    private void load_creature_list()
    {
        curr_list.Clear();
        List<Actor> simple_list = World.world.units.getSimpleList();
        foreach (Actor actor in simple_list)
        {
            if (!actor.isAlive()) continue;
            curr_list.Add(actor);
        }
    }

    private void load_cultibook_list()
    {
        curr_list.Clear();
        foreach (Cultibook cultibook in Manager.cultibooks.list)
        {
            if (cultibook.cur_users < 1) continue;
            curr_list.Add(cultibook);
        }
    }

    private void load_blood_list()
    {
    }

    private void load_item_list()
    {
    }

    private void load_pope_list()
    {
    }

    private void load_city_list()
    {
    }

    private void load_kingdom_list()
    {
    }

    private void load_clan_list()
    {
    }

    private void add_sort_key(string id, string icon, string tip_name, TopValueCalc calc, TopValueShow show,
        Transform container)
    {
        CW_TipButton key_button = Instantiate(Prefabs.tip_button_prefab, container);
        GameObject arrow_obj = new("Arrow", typeof(Image));
        arrow_obj.transform.SetParent(key_button.transform);
        arrow_obj.transform.localPosition = new Vector3(4, 4);
        arrow_obj.transform.localScale = new Vector3(1, 1);
        arrow_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(16, 16);
        arrow_obj.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/Icons/iconArrowUP");
        key_button.load(icon, obj =>
        {
            Tooltip.show(obj, "normal", new TooltipData
            {
                tip_name = tip_name
            });
        });
        key_button.button.onClick.AddListener(() =>
        {
            clear_content();
            set_sort_key(id);
            instance.show();
        });
        calcs[id] = calc;
        shows[id] = show;
        if (icon.StartsWith("../../"))
        {
            icons[id] = icon.Replace("../../", "");
        }
        else
        {
            icons[id] = $"ui/Icons/{icon}";
        }
    }

    private void add_creature_sort_keys(Transform container)
    {
        add_sort_key("actor_age", "iconClock", "cw_top_creature_sort_key_age", o => ((CW_Actor)o).data.getAge(),
            o => ((CW_Actor)o).data.getAge().ToString(), container);
        add_sort_key("actor_kills", "iconSkulls", "cw_top_creature_sort_key_kills", o => ((CW_Actor)o).data.kills,
            o => ((CW_Actor)o).data.kills.ToString(), container);
        add_sort_key("actor_level", "iconLevels", "cw_top_creature_sort_key_level",
            o => ((CW_Actor)o).data.level * 1e9f + ((CW_Actor)o).data.experience,
            o => ((CW_Actor)o).data.level.ToString(), container);
        foreach (CultisysAsset cultisys in Manager.cultisys.list)
        {
            add_sort_key("actor_" + cultisys.id, "../../" + cultisys.sprite_path,
                $"cw_top_creature_sort_key_{cultisys.id}",
                o => ((CW_Actor)o).data.get_cultisys_level()[cultisys.pid],
                o =>
                {
                    if (((CW_Actor)o).data.get_cultisys_level()[cultisys.pid] < 0)
                    {
                        return Localization.Get("cw_no_cultisys");
                    }

                    return Localization.Get($"{cultisys.id}_{((CW_Actor)o).data.get_cultisys_level()[cultisys.pid]}");
                }, container);
        }

        add_sort_key("actor_element", "iconElement",
            "cw_top_creature_sort_key_element",
            o => ((CW_Actor)o).data.get_element().get_type().rarity,
            o => Toolbox.coloredString(
                Localization.Get(((CW_Actor)o).data.get_element().get_type().id),
                ((CW_Actor)o).data.get_element().get_color()), container);
    }

    private void add_cultibook_sort_keys(Transform container)
    {
        add_sort_key("cultibook_level", "iconLevels", "cw_top_cultibook_sort_key_level", o => ((Cultibook)o).level,
            o => ((Cultibook)o).level.ToString(), container);
        add_sort_key("cultibook_curr_users", "iconPopulation", "cw_top_cultibook_sort_key_curr_users",
            o => ((Cultibook)o).cur_users,
            o => ((Cultibook)o).cur_users.ToString(), container);
        add_sort_key("cultibook_spells_nr", "iconPopulation", "cw_top_cultibook_sort_key_spells_nr",
            o => ((Cultibook)o).spells.Count,
            o => ((Cultibook)o).spells.Count.ToString(), container);
        add_sort_key("cultibook_cultivelo", "iconCultiSys", "cw_top_cultibook_sort_key_cultivelo",
            o => ((Cultibook)o).bonus_stats[CW_S.mod_cultivelo],
            o => (int)(100 * ((Cultibook)o).bonus_stats[CW_S.mod_cultivelo]) + "%", container);
        add_sort_key("cultibook_histroy", "iconClock", "cw_top_cultibook_sort_key_histroy",
            o => -((Cultibook)o).creation_time,
            o => World.world.getYearsSince(((Cultibook)o).creation_time).ToString(), container);
    }

    private void show()
    {
        content_transform.Find("No_Items").gameObject.SetActive(false);
        List<object> show_list = curr_list.FindAll(o => curr_filter(o));
        show_list.Sort((o1, o2) => { return curr_value_calc(o2).CompareTo(curr_value_calc(o1)); });
        for (int idx = 0; idx < Math.Min(show_count, show_list.Count); idx++)
        {
            SimpleInfo info = Instantiate(curr_info_prefab, content_transform);
            info.load_obj(show_list[idx], curr_value_show(show_list[idx]), curr_icon);
        }
    }

    private void clear()
    {
        curr_list.Clear();
        foreach (Transform child in sort_key.transform)
        {
            child.gameObject.SetActive(false);
        }

        clear_content();
    }

    private void clear_content()
    {
        content_transform.Find("No_Items").gameObject.SetActive(true);
        foreach (Transform child in content_transform)
        {
            if (child.name == "No_Items") continue;
            Destroy(child.gameObject);
        }
    }
}