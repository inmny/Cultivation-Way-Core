using Cultivation_Way.Library;
using Cultivation_Way.Others;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowActorAssetConfig : AbstractWindow<WindowActorAssetConfig>
{
    private ActorAsset editing_actor_asset;
    private Transform cultisys_select_grid;

    internal static void init()
    {
        base_init(Constants.Core.actorasset_config_window);
        content_transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        VerticalLayoutGroup layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 20;
        layout.padding = new RectOffset(0, 0, 4, 0);
        ContentSizeFitter fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        #region 搜索框

        GameObject search_part = new("Search", typeof(RectTransform));
        search_part.transform.SetParent(content_transform);
        search_part.transform.localScale = new Vector3(1, 1);
        search_part.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        GameObject search_result = new("Search_Result", typeof(Text), typeof(LocalizedText));
        search_result.transform.SetParent(search_part.transform);
        search_result.GetComponent<Text>().font = LocalizedTextManager.currentFont;
        search_result.GetComponent<Text>().resizeTextMaxSize = 10;
        search_result.GetComponent<Text>().resizeTextMinSize = 1;
        search_result.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        search_result.GetComponent<Text>().resizeTextForBestFit = true;
        search_result.GetComponent<Text>().color = Color.red;
        search_result.GetComponent<LocalizedText>().key = "no_found";
        search_result.GetComponent<LocalizedText>().text = search_result.GetComponent<Text>();
        search_result.transform.localPosition = new Vector3(-75, 0);
        search_result.transform.localScale = new Vector3(1, 1);
        search_result.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 20);

        GameObject search_input = Prefabs.instantiate_input_field(FastVisit.get_window_inner_sliced(),
            new Vector2(110, 20),
            new Vector2(4, 2), search_part.transform);
        search_input.name = "Search_Input";
        search_input.transform.localPosition = new Vector3(5, 0);
        search_input.transform.localScale = new Vector3(1, 1);
        search_input.transform.Find("Input_Field").GetComponent<InputField>().onValueChanged.AddListener(
            str =>
            {
                ActorAsset actor_asset = AssetManager.actor_library.get(str);
                if (actor_asset == null)
                {
                    search_result.GetComponent<Text>().color = Color.red;
                    search_result.GetComponent<LocalizedText>().key = "no_found";
                }
                else
                {
                    search_result.GetComponent<Text>().color = Color.white;
                    search_result.GetComponent<LocalizedText>().key = actor_asset.nameLocale;
                }

                search_result.GetComponent<LocalizedText>().updateText();
            }
        );
        search_input.transform.Find("Input_Field").GetComponent<InputField>().onEndEdit.AddListener(
            str =>
            {
                ActorAsset actor_asset = AssetManager.actor_library.get(str);
                if (actor_asset == null)
                {
                    if (instance.editing_actor_asset == null)
                    {
                        search_result.GetComponent<Text>().color = Color.red;
                        search_result.GetComponent<LocalizedText>().key = "no_found";
                        search_result.GetComponent<LocalizedText>().updateText();
                    }
                    else
                    {
                        search_input.transform.Find("Input_Field").GetComponent<InputField>().text =
                            instance.editing_actor_asset.id;
                    }
                }
                else
                {
                    instance.select_actor_asset(actor_asset);
                }
            }
        );
        GameObject search_help = new("Search_Help", typeof(Image), typeof(Button));
        search_help.transform.SetParent(search_part.transform);
        search_help.transform.localPosition = new Vector3(82, 0);
        search_help.transform.localScale = new Vector3(1, 1);
        search_help.GetComponent<Image>().sprite = FastVisit.get_red_button();
        search_help.GetComponent<Image>().type = Image.Type.Sliced;
        search_help.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
        GameObject search_help_icon = new("Icon", typeof(Image));
        search_help_icon.transform.SetParent(search_help.transform);
        search_help_icon.transform.localScale = new Vector3(1, 1);
        search_help_icon.transform.localPosition = new Vector3(0, 0);
        search_help_icon.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/icons/iconAbout");
        search_help_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);

        #endregion

        #region 可修炼体系选择

        GameObject cultisys_select_part = new("Cultisys_Select_Grid", typeof(Image), typeof(VerticalLayoutGroup),
            typeof(ContentSizeFitter));
        cultisys_select_part.transform.SetParent(content_transform);
        cultisys_select_part.transform.localScale = new Vector3(1, 1);
        cultisys_select_part.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 0);
        cultisys_select_part.GetComponent<Image>().sprite = FastVisit.get_window_inner_sliced();
        cultisys_select_part.GetComponent<Image>().type = Image.Type.Sliced;

        fitter = cultisys_select_part.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        layout = cultisys_select_part.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 4;
        layout.padding = new RectOffset(0, 0, -20, 4);

        GameObject cultisys_select_title = new("Title", typeof(Text), typeof(LocalizedText));
        cultisys_select_title.transform.SetParent(cultisys_select_part.transform);
        cultisys_select_title.transform.localScale = new Vector3(1, 1);
        cultisys_select_title.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
        cultisys_select_title.GetComponent<Text>().resizeTextMaxSize = 10;
        cultisys_select_title.GetComponent<Text>().resizeTextMinSize = 1;
        cultisys_select_title.GetComponent<Text>().resizeTextForBestFit = true;
        cultisys_select_title.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        cultisys_select_title.GetComponent<Text>().font = LocalizedTextManager.currentFont;
        cultisys_select_title.GetComponent<LocalizedText>().key = "cw_allowed_cultisys_select";


        GameObject cultisys_select_grid =
            new("Grid", typeof(RectTransform), typeof(GridLayoutGroup), typeof(ContentSizeFitter));
        cultisys_select_grid.transform.SetParent(cultisys_select_part.transform);
        cultisys_select_grid.transform.localScale = new Vector3(1, 1);
        cultisys_select_grid.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 0);
        GridLayoutGroup grid_layout = cultisys_select_grid.GetComponent<GridLayoutGroup>();
        grid_layout.cellSize = new Vector2(50, 20);
        grid_layout.spacing = new Vector2(12, 4);
        grid_layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid_layout.constraintCount = 3;
        grid_layout.childAlignment = TextAnchor.UpperLeft;

        fitter = cultisys_select_grid.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;


        instance.cultisys_select_grid = cultisys_select_grid.transform;

        #endregion
    }

    internal static void post_init()
    {
        General.AboutUI.WorldLaws.add_setting_law(Constants.Core.actorasset_config_window, "worldlaw_creature_grid",
            "ui/icons/iconYaos", Constants.Core.actorasset_config_window);
        foreach (CultisysAsset cultisys in Manager.cultisys.list)
        {
            GameObject cultisys_select_obj = new(cultisys.id, typeof(RectTransform));
            cultisys_select_obj.transform.SetParent(instance.cultisys_select_grid);
            cultisys_select_obj.transform.localScale = new Vector3(1, 1);
            GameObject cultisys_name = new("Title", typeof(Text), typeof(LocalizedText));
            cultisys_name.transform.SetParent(cultisys_select_obj.transform);
            cultisys_name.transform.localScale = new Vector3(1, 1);
            cultisys_name.GetComponent<Text>().resizeTextMaxSize = 10;
            cultisys_name.GetComponent<Text>().resizeTextMinSize = 1;
            cultisys_name.GetComponent<Text>().resizeTextForBestFit = true;
            cultisys_name.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            cultisys_name.GetComponent<Text>().font = LocalizedTextManager.currentFont;
            cultisys_name.GetComponent<LocalizedText>().key = cultisys.id;
            cultisys_name.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 20);
            GameObject select_button = new("Button", typeof(Image), typeof(Button));
            select_button.transform.SetParent(cultisys_select_obj.transform);
            select_button.transform.localScale = new Vector3(1, 1);
            select_button.transform.localPosition = new Vector3(24, 0);
            select_button.GetComponent<Image>().sprite = FastVisit.get_button_1();
            select_button.GetComponent<Image>().type = Image.Type.Sliced;
            select_button.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            GameObject select_button_icon = new("Icon", typeof(Image));
            select_button_icon.transform.SetParent(select_button.transform);
            select_button_icon.transform.localScale = new Vector3(1, 1);
            select_button_icon.transform.localPosition = new Vector3(0, 0);
            select_button_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            select_button_icon.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/icons/iconOn");
            select_button_icon.SetActive(false);
            select_button.GetComponent<Button>().onClick.AddListener(() =>
            {
                select_button_icon.SetActive(!select_button_icon.activeSelf);
                if (instance.editing_actor_asset == null) return;

                CW_ActorAsset cw_actor_asset = Manager.actors.get(instance.editing_actor_asset.id);
                if (select_button_icon.activeSelf)
                {
                    cw_actor_asset.allowed_cultisys.Add(Manager.cultisys.get(cultisys_select_obj.name));
                }
                else
                {
                    cw_actor_asset.allowed_cultisys.Remove(Manager.cultisys.get(cultisys_select_obj.name));
                }
            });
        }
    }

    private void select_actor_asset(ActorAsset actor_asset)
    {
        instance.editing_actor_asset = actor_asset;
        if (actor_asset == null) return;
        CW_ActorAsset cw_actor_asset = Manager.actors.get(actor_asset.id);
        // 更新允许的修炼体系
        foreach (Transform cultisys_select_tsf in cultisys_select_grid)
        {
            GameObject select_button = cultisys_select_tsf.Find("Button").gameObject;
            GameObject select_button_icon = select_button.transform.Find("Icon").gameObject;
            select_button_icon.SetActive(
                cw_actor_asset.allowed_cultisys.Contains(Manager.cultisys.get(cultisys_select_tsf.name)));
        }
    }
}