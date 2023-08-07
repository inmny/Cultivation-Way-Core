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
    private TopFilter curr_filter;
    private SimpleInfo curr_info_prefab;
    private readonly int show_count = 10;
    private readonly List<object> curr_list = new();

    private readonly Dictionary<string, TopValueCalc> calcs = new();
    private readonly Dictionary<string, TopFilter> filter_funcs = new();

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
        instance.sort_key.transform.localPosition = new Vector3(0, 0);
        instance.sort_key.transform.localScale = new Vector3(1, 1);
        instance.sort_key.transform.SetParent(background_transform);

        void add_sort_key_container(string id)
        {
            GameObject sort_key_container = new(id, typeof(GridLayoutGroup));
            sort_key_container.transform.SetParent(instance.sort_key.transform);
            sort_key_container.transform.localPosition = new Vector3(200, 55);
            sort_key_container.transform.localScale = new Vector3(1, 1);

            GridLayoutGroup layout = sort_key_container.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(24, 24);
            sort_key_container.SetActive(false);
        }

        add_sort_key_container("creature");
        add_sort_key_container("cultibook");
        add_sort_key_container("blood");
        add_sort_key_container("item");
        add_sort_key_container("pope");
        add_sort_key_container("city");
        add_sort_key_container("kingdom");
        add_sort_key_container("clan");

        instance.add_creature_sort_keys(instance.sort_key.transform.Find("creature"));

        #endregion

        #region 筛选器

        instance.filter = new GameObject("Filter");
        instance.filter.transform.SetParent(background_transform);

        void add_filter_container(string id)
        {
            GameObject filter_container = new(id, typeof(GridLayoutGroup));
            filter_container.transform.SetParent(instance.filter.transform);
            filter_container.transform.localPosition = new Vector3(-200, 55);
            filter_container.transform.localScale = new Vector3(1, 1);

            GridLayoutGroup layout = filter_container.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(24, 24);
            filter_container.SetActive(false);
        }

        add_filter_container("creature");
        add_filter_container("cultibook");
        add_filter_container("blood");
        add_filter_container("item");
        add_filter_container("pope");
        add_filter_container("city");
        add_filter_container("kingdom");
        add_filter_container("clan");

        #endregion

        instance.filter_funcs["default"] = _ => true;
    }

    private void switch_to_creature()
    {
        clear();
        curr_list.AddRange(World.world.units.getSimpleList());
        curr_info_prefab = Prefabs.simple_creature_info_prefab;
        curr_value_calc = calcs["age"];
        curr_filter = filter_funcs["default"];
        show();
    }

    private void switch_to_cultibook()
    {
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

    private void add_creature_sort_keys(Transform container)
    {
        CW_TipButton key_button;

        key_button = Instantiate(Prefabs.tip_button_prefab, container);
        key_button.load("iconClock", obj =>
        {
            Tooltip.show(obj, "normal", new TooltipData
            {
                tip_name = "cw_top_creature_sort_key_age"
            });
        });
        key_button.button.onClick.AddListener(() =>
        {
            clear_content();
            curr_value_calc = calcs["age"];
            show();
        });
        calcs["age"] = o =>
        {
            CW_Actor actor = (CW_Actor)o;
            return actor.data.getAge();
        };
    }

    private void show()
    {
        List<object> show_list = curr_list.FindAll(o => curr_filter(o));
        show_list.Sort((o1, o2) => { return curr_value_calc(o1).CompareTo(curr_value_calc(o2)); });
        for (int idx = 0; idx < Math.Min(show_count, show_list.Count); idx++)
        {
            SimpleInfo info = Instantiate(curr_info_prefab, content_transform);
            info.load_obj(show_list[idx], curr_value_show(show_list[idx]));
        }
    }

    private void clear()
    {
        curr_list.Clear();
    }

    private void clear_content()
    {
    }
}