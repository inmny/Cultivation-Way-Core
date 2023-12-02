using Cultivation_Way.Library;
using Cultivation_Way.Others;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowCultisysConfig : AbstractWindow<WindowCultisysConfig>
{
    private Image cultisys_icon;
    private InputField cultisys_name_input;
    private CultisysAsset editing_cultisys_asset;
    private Transform level_list;

    private void OnEnable()
    {
        if (!initialized)
        {
            return;
        }

        cultisys_icon.sprite = SpriteTextureLoader.getSprite(editing_cultisys_asset.sprite_path);
        cultisys_name_input.text = LocalizedTextManager.getText(editing_cultisys_asset.id);

        foreach (Transform child in level_list)
        {
            Destroy(child.gameObject);
        }

        for (int level = 0; level < editing_cultisys_asset.max_level; level++)
        {
            int tmp = level;
            GameObject level_obj = Instantiate(Prefabs.cultisys_level_edit_prefab, level_list);
            level_obj.name = $"Level {level}";
            level_obj.transform.Find("Comment").GetComponent<Text>().text = level.ToString();
            level_obj.transform.Find("Name_Input/Input_Field").GetComponent<InputField>().text =
                LocalizedTextManager.getText($"{editing_cultisys_asset.id}_{level}");
            level_obj.transform.Find("Name_Input/Input_Field").GetComponent<InputField>().onEndEdit.AddListener(
                str => { Localization.Set($"{editing_cultisys_asset.id}_{tmp}", str); }
            );
            level_obj.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() =>
            {
                WindowCultisysLevelConfig.select_cultisys_level(instance.editing_cultisys_asset, tmp);
                ScrollWindow.showWindow(Constants.Core.cultisys_level_config_window);
            });
        }
    }

    internal static void init()
    {
        base_init(Constants.Core.cultisys_config_window);

        content_transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        VerticalLayoutGroup content_layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();
        content_layout.childControlHeight = false;
        content_layout.childControlWidth = false;
        content_layout.childForceExpandHeight = false;
        content_layout.childForceExpandWidth = false;
        content_layout.childAlignment = TextAnchor.UpperCenter;
        content_layout.spacing = 0;
        ContentSizeFitter content_fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        content_fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        content_fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        GameObject top_part = new("Top", typeof(RectTransform));
        top_part.transform.SetParent(content_transform);
        top_part.transform.localScale = Vector3.one;

        instance.cultisys_icon = new GameObject("Icon", typeof(Image)).GetComponent<Image>();
        instance.cultisys_icon.transform.SetParent(top_part.transform);
        instance.cultisys_icon.transform.localPosition = new Vector3(-70, 0);
        instance.cultisys_icon.transform.localScale = new Vector3(1, 1);
        instance.cultisys_icon.rectTransform.sizeDelta = new Vector2(32, 32);

        GameObject cultisys_name_input_obj = Prefabs.instantiate_input_field(FastVisit.get_window_inner_sliced(),
            new Vector2(100, 20),
            new Vector2(4, 2), top_part.transform);
        cultisys_name_input_obj.transform.localPosition = new Vector3(22, 0);
        instance.cultisys_name_input = cultisys_name_input_obj.transform.Find("Input_Field").GetComponent<InputField>();


        instance.level_list =
            new GameObject("Level List", typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter))
                .transform;
        instance.level_list.SetParent(content_transform);
        instance.level_list.localScale = Vector3.one;
        instance.level_list.localPosition = new Vector3(0, -60);
        instance.level_list.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 0);
        instance.level_list.GetComponent<Image>().sprite = FastVisit.get_window_inner_sliced();
        instance.level_list.GetComponent<Image>().type = Image.Type.Sliced;

        VerticalLayoutGroup level_layout = instance.level_list.GetComponent<VerticalLayoutGroup>();
        level_layout.childControlHeight = false;
        level_layout.childControlWidth = false;
        level_layout.childForceExpandHeight = false;
        level_layout.childForceExpandWidth = false;
        level_layout.childAlignment = TextAnchor.UpperCenter;
        level_layout.spacing = 8;
        level_layout.padding = new RectOffset(0, 0, 8, 8);

        ContentSizeFitter fitter = instance.level_list.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        initialized = true;
    }

    public static void select_cultisys(CultisysAsset cultisys_asset)
    {
        instance.editing_cultisys_asset = cultisys_asset;
    }
}