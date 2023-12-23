using Cultivation_Way.Library;
using NCMS.Utils;
using NeoModLoader.General;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowCultisysLevelConfig : AbstractWindow<WindowCultisysLevelConfig>
{
    private CultisysAsset editing_cultisys_asset;
    private int editing_cultisys_level;

    private void OnEnable()
    {
        if (!initialized)
        {
            return;
        }

        LM.AddToCurrentLocale(Constants.Core.cultisys_level_config_window + Constants.Core.title_suffix,
            LocalizedTextManager.getText($"{editing_cultisys_asset.id}_{editing_cultisys_level}"));
        
        instance.level_limit_field.text = editing_cultisys_asset.number_limit_per_level[editing_cultisys_level].ToString();
    }
    private InputField level_limit_field;
    internal static void init()
    {
        base_init(Constants.Core.cultisys_level_config_window);
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

        // 人数限制
        instance.level_limit_field = Prefabs.instantiate_edit_bar("cw_cultisys_level_number_limit", content_transform).transform.Find("Name_Input/Input_Field").GetComponent<InputField>();
        instance.level_limit_field.onValueChanged.AddListener((string value) =>
        {
            if (int.TryParse(value, out int result))
            {
                if(result < 0)
                {
                    instance.level_limit_field.textComponent.color = Color.red;
                    return;
                }
                instance.level_limit_field.textComponent.color = Color.white;
                instance.editing_cultisys_asset.number_limit_per_level[instance.editing_cultisys_level] = result;
                return;
            }
            instance.level_limit_field.textComponent.color = Color.red;
        });
        // 破镜难度
        // 属性设置
        initialized = true;
    }
    
    internal static void select_cultisys_level(CultisysAsset cultisys_asset, int level)
    {
        instance.editing_cultisys_asset = cultisys_asset;
        instance.editing_cultisys_level = level;
    }
}