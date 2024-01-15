using System.Collections.Generic;
using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class SubSelectWindow : APrefab<SubSelectWindow>
{
    private bool auto_clean;
    private Button close_button;
    public Text introduction { get; private set; }
    public Text title { get; private set; }
    public RectTransform grid { get; private set; }

    private void OnDisable()
    {
        if (!Initialized || !auto_clean) return;
        var child_count = grid.childCount;
        for (var i = 0; i < child_count; i++) Destroy(grid.GetChild(i).gameObject, 0.3f);
    }

    protected override void Init()
    {
        if (Initialized) return;
        close_button = transform.Find("Close").GetComponent<Button>();
        introduction = transform.Find("Introduction").GetComponent<Text>();
        title = transform.Find("Title").GetComponent<Text>();
        grid = transform.Find("Grid").GetComponent<RectTransform>();
        close_button.onClick.AddListener(() => { gameObject.SetActive(false); });
        base.Init();
    }

    public void Setup(List<RectTransform> pContent, string pTitle, string pIntroduction, Vector2 pSize = default,
        bool pAutoClean = false)
    {
        Init();
        title.text = pTitle;
        introduction.text = pIntroduction;
        foreach (var child in pContent)
        {
            child.SetParent(grid);
            child.localScale = Vector3.one;
        }

        if (pSize == default)
            SetSize(new Vector2(320, 220));
        else
            SetSize(pSize);
        auto_clean = pAutoClean;
    }

    [Hotfixable]
    public override void SetSize(Vector2 pSize)
    {
        base.SetSize(pSize);
        var bg_border = GetComponent<Image>().sprite.border;
        grid.sizeDelta = pSize - new Vector2(bg_border.x + bg_border.z, bg_border.y + bg_border.w);
        introduction.GetComponent<RectTransform>().sizeDelta = new Vector2(grid.sizeDelta.x, 50);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x / 320 * 100, 24);
        close_button.transform.localPosition = new Vector2(142.25f + (pSize.x - 320) / 2, 98.4f + (pSize.y - 220) / 2);
    }

    private static void _init()
    {
        var obj = new GameObject("SubSelectWindow", typeof(Image), typeof(VerticalLayoutGroup));
        obj.transform.SetParent(CW_Core.ui_prefab_library);
        obj.GetComponent<Image>().sprite = FastVisit.get_window_library();
        obj.GetComponent<Image>().type = Image.Type.Sliced;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 220);

        var layout = obj.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 5;

        var close_button = new GameObject("Close", typeof(Image), typeof(Button), typeof(LayoutElement));
        close_button.transform.SetParent(obj.transform);
        close_button.transform.localScale = Vector3.one;
        close_button.transform.localPosition = new Vector3(142.25f, 98.4f);
        close_button.GetComponent<RectTransform>().sizeDelta = new Vector2(28, 28);
        close_button.GetComponent<LayoutElement>().ignoreLayout = true;
        close_button.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/icons/iconClose");


        var title = new GameObject("Title", typeof(Text));
        title.transform.SetParent(obj.transform);
        title.transform.localScale = Vector3.one;
        var title_text = title.GetComponent<Text>();
        title_text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 24);
        title_text.font = LocalizedTextManager.currentFont;
        title_text.text = "Title";
        title_text.alignment = TextAnchor.MiddleCenter;
        title_text.color = Colors.default_color;
        title_text.fontSize = 10;

        var button_container = new GameObject("Grid", typeof(Image), typeof(GridLayoutGroup));
        button_container.transform.SetParent(obj.transform);
        button_container.transform.localScale = Vector3.one;
        button_container.GetComponent<Image>().enabled = false;

        var grid = button_container.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(32, 32);
        grid.spacing = new Vector2(4, 4);
        grid.padding = new RectOffset(5, 5, 5, 5);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 9;
        grid.childAlignment = TextAnchor.UpperLeft;

        var intro = new GameObject("Introduction", typeof(Text));
        intro.transform.SetParent(obj.transform);
        intro.transform.localScale = Vector3.one;
        var intro_text = intro.GetComponent<Text>();
        intro_text.font = LocalizedTextManager.currentFont;
        intro_text.text =
            "IntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroductionIntroduction";
        intro_text.alignment = TextAnchor.UpperCenter;
        intro_text.color = Colors.default_color;
        intro_text.fontSize = 12;

        Prefab = obj.AddComponent<SubSelectWindow>();
    }
}