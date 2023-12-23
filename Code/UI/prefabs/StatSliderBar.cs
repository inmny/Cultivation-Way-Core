﻿using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.Code.UI.prefabs;

public class StatSliderBar : APrefab<StatSliderBar>
{
    private void Awake()
    {
        if (!Initialized) Init();
    }
    protected override void Init()
    {
        if(Initialized) return;
        base.Init();
        slider_bar = transform.Find("SliderBar").GetComponent<SliderBar>();
        stat_name = transform.Find("Title/Name").GetComponent<Text>();
        stat_value = transform.Find("Title/Value").GetComponent<Text>();
    }
    [Hotfixable]
    public void Setup(BaseStatsContainer stats, float min, float max, Vector2 size = default)
    {
        Init();

        SetSize(size);
        slider_bar.Setup(stats.value, min, max, [Hotfixable](val)=> 
        {
            stats.value = val;
            stat_value.text = val.ToString();
        }, default);
        stat_name.text = LM.Get(AssetManager.base_stats_library.get(stats.id).translation_key);
        if (string.IsNullOrEmpty(stat_name.text))
        {
            stat_name.text = "Key: " + AssetManager.base_stats_library.get(stats.id).translation_key;
            if (string.IsNullOrEmpty(AssetManager.base_stats_library.get(stats.id).translation_key))
            {
                stat_name.text = "ID: "+stats.id;
            }
        }
        stat_value.text = stats.value.ToString();
    }
    [Hotfixable]
    public void SetSize(Vector2 size)
    {
        Init();
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = size;
        stat_name.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x / 2 * 0.95f, size.y /2 * 0.9f);
        stat_value.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x / 2 * 0.95f, size.y /2 * 0.9f);

        slider_bar.SetSize(new Vector2(size.x * 0.95f, size.y / 2 * 0.9f));
    }
    private SliderBar slider_bar;
    private Text stat_name;
    private Text stat_value;
    internal static void _init()
    {
        GameObject obj = new GameObject("StatSliderBar", typeof(Image), typeof(VerticalLayoutGroup));
        obj.transform.SetParent(CW_Core.ui_prefab_library.transform);
        obj.GetComponent<Image>().sprite = FastVisit.get_window_inner_sliced();
        obj.GetComponent<Image>().type = Image.Type.Sliced;

        VerticalLayoutGroup root_layout = obj.GetComponent<VerticalLayoutGroup>();
        root_layout.childControlHeight = false;
        root_layout.childControlWidth = false;
        root_layout.childForceExpandHeight = false;
        root_layout.childForceExpandWidth = false;
        root_layout.childAlignment = TextAnchor.UpperCenter;
        root_layout.padding = new RectOffset(2, 2, 2, 2);
        root_layout.spacing = 2;

        GameObject title = new GameObject("Title", typeof(RectTransform), typeof(ContentSizeFitter), typeof(HorizontalLayoutGroup));
        title.transform.SetParent(obj.transform);
        title.transform.localScale = Vector3.one;
        ContentSizeFitter title_fitter = title.GetComponent<ContentSizeFitter>();
        title_fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        title_fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        HorizontalLayoutGroup title_layout = title.GetComponent<HorizontalLayoutGroup>();
        title_layout.childControlHeight = false;
        title_layout.childControlWidth = false;
        title_layout.childForceExpandHeight = false;
        title_layout.childForceExpandWidth = false;
        title_layout.childAlignment = TextAnchor.MiddleCenter;
        title_layout.spacing = 2;

        GameObject stat_name = new GameObject("Name", typeof(Text));
        Text stat_name_text = stat_name.GetComponent<Text>();
        stat_name_text.text = "StatName";
        stat_name_text.resizeTextForBestFit = true;
        stat_name_text.resizeTextMaxSize = 10;
        stat_name_text.resizeTextMinSize = 1;
        stat_name_text.alignment = TextAnchor.MiddleLeft;
        stat_name_text.font = LocalizedTextManager.currentFont;
        stat_name.transform.SetParent(title.transform);
        stat_name.transform.localScale = Vector3.one;
        stat_name.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);

        GameObject stat_value = Instantiate(stat_name, title.transform);
        stat_value.name = "Value";
        stat_value.GetComponent<Text>().text = "0";
        stat_value.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        stat_value.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);


        SliderBar bar = Instantiate(SliderBar.Prefab, obj.transform);
        bar.name = "SliderBar";
        bar.transform.SetParent(obj.transform);
        bar.transform.localScale = Vector3.one;

        Prefab = obj.AddComponent<StatSliderBar>();
    }
}
