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

    internal static void init()
    {
        base_init(Constants.Core.tops_window);

        background_transform.Find("Title").GetComponent<Text>().font = Fonts.STLiti;

        #region 八个榜单的按钮

        GameObject button_prefab =
            Instantiate(Prefabs.tip_button_with_bg_game_obj_prefab.gameObject, CW_Core.prefab_library);
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
    }

    private void switch_to_creature()
    {
    }
}