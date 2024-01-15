using System.Collections.Generic;
using Cultivation_Way.Library;
using Cultivation_Way.UI.prefabs;
using NeoModLoader.General;
using UnityEngine;
using UnityEngine.UI;
namespace Cultivation_Way.UI;

public class WindowCultisysLevelConfig : AbstractWindow<WindowCultisysLevelConfig>
{
    private CultisysAsset editing_cultisys_asset;
    private int editing_cultisys_level;
    private List<GameObject> invisible_obj_at_level_one = new List<GameObject>();
    private InputField level_difficulty_field;
    private InputField level_limit_field;
    private ObjectPoolGenericMono<StatSliderBar> slider_bar_pool;
    private void OnEnable()
    {
        if (!initialized)
        {
            return;
        }

        foreach (GameObject obj in invisible_obj_at_level_one)
        {
            obj.SetActive(editing_cultisys_level != 0);
        }

        LM.AddToCurrentLocale(Constants.Core.cultisys_level_config_window,
            LocalizedTextManager.getText($"{editing_cultisys_asset.id}_{editing_cultisys_level}"));

        transform.Find("Background/Title").GetComponent<LocalizedText>().updateText();

        var last_stats = editing_cultisys_level == 0 ? null : editing_cultisys_asset.bonus_stats[editing_cultisys_level - 1];
        var next_stats = editing_cultisys_level == editing_cultisys_asset.bonus_stats.Length - 1 ? null : editing_cultisys_asset.bonus_stats[editing_cultisys_level + 1];

        foreach (BaseStatsContainer stats in editing_cultisys_asset.bonus_stats[editing_cultisys_level].stats_list)
        {
            StatSliderBar bar = slider_bar_pool.getNext(0);
            float min = last_stats == null ? 0 : last_stats[stats.id];
            float max = next_stats == null ? Mathf.Min(int.MaxValue, 10 * stats.value) : next_stats[stats.id];
            bar.Setup(stats, min, max, new Vector2(190, 40));
        }

        level_limit_field.text = editing_cultisys_asset.number_limit_per_level[editing_cultisys_level].ToString();
        level_difficulty_field.text = editing_cultisys_asset.difficulty_per_level[editing_cultisys_level].ToString();
    }
    private void OnDisable()
    {
        if (!initialized) return;
        slider_bar_pool.clear();
        var list = World.world.units.getSimpleList();
        foreach (var unit in list)
        {
            if (unit == null || !unit.isAlive()) continue;
            unit.data.get(editing_cultisys_asset.id, out int level, -1);
            if (level != editing_cultisys_level) continue;
            unit.setStatsDirty();
        }
    }
    internal static void init()
    {
        base_init(Constants.Core.cultisys_level_config_window);
        content_transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        VerticalLayoutGroup layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 5;
        layout.padding = new RectOffset(0, 0, 4, 4);
        ContentSizeFitter fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // 人数限制
        GameObject edit_level_limit = Prefabs.instantiate_edit_bar("cw_cultisys_level_number_limit", content_transform);
        instance.level_limit_field = edit_level_limit.transform.Find("Name_Input/Input_Field").GetComponent<InputField>();
        instance.level_limit_field.onValueChanged.AddListener((string value) =>
        {
            if (int.TryParse(value, out int result))
            {
                if (result < 0)
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
        instance.invisible_obj_at_level_one.Add(edit_level_limit);
        // 破镜难度
        GameObject edit_level_difficulty = Prefabs.instantiate_edit_bar("cw_cultisys_level_difficulty", content_transform);
        instance.level_difficulty_field = edit_level_difficulty.transform.Find("Name_Input/Input_Field").GetComponent<InputField>();
        instance.level_difficulty_field.onValueChanged.AddListener((string value) =>
        {
            if (float.TryParse(value, out float result))
            {
                if (result < 1 || result > 100)
                {
                    instance.level_difficulty_field.textComponent.color = Color.red;
                    return;
                }
                instance.level_difficulty_field.textComponent.color = Color.white;
                instance.editing_cultisys_asset.difficulty_per_level[instance.editing_cultisys_level] = result;
                return;
            }
            instance.level_difficulty_field.textComponent.color = Color.red;
        });
        instance.invisible_obj_at_level_one.Add(edit_level_difficulty);
        // 属性设置
        GameObject stats_grid = new GameObject("StatsGrid", typeof(RectTransform), typeof(ContentSizeFitter), typeof(VerticalLayoutGroup));
        layout = stats_grid.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 5;
        layout.padding = new RectOffset(0, 0, 4, 4);
        fitter = stats_grid.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        stats_grid.transform.SetParent(content_transform);
        stats_grid.transform.localScale = Vector3.one;

        instance.slider_bar_pool = new ObjectPoolGenericMono<StatSliderBar>(StatSliderBar.Prefab, stats_grid.transform);
        initialized = true;
    }

    internal static void select_cultisys_level(CultisysAsset cultisys_asset, int level)
    {
        instance.editing_cultisys_asset = cultisys_asset;
        instance.editing_cultisys_level = level;
    }
}