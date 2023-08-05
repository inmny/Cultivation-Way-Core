using Cultivation_Way.Others;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowActorAssetConfig : AbstractWindow<WindowActorAssetConfig>
{
    private ActorAsset editing_actor_asset;

    internal static void init()
    {
        base_init(Constants.Core.actorasset_config_window);
        content_transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        VerticalLayoutGroup content_layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();
        content_layout.childControlHeight = false;
        content_layout.childControlWidth = false;
        content_layout.childForceExpandHeight = false;
        content_layout.childForceExpandWidth = false;
        content_layout.childAlignment = TextAnchor.UpperCenter;
        content_layout.spacing = 0;
        content_layout.padding = new RectOffset(0, 0, 4, 0);
        ContentSizeFitter content_fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        content_fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        content_fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

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
    }

    internal static void post_init()
    {
        General.AboutUI.WorldLaws.add_setting_law(Constants.Core.actorasset_config_window, "worldlaw_creature_grid",
            "ui/icons/iconYaos", Constants.Core.actorasset_config_window);
    }

    private void select_actor_asset(ActorAsset actor_asset)
    {
    }
}