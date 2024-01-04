using Cultivation_Way.Library;
using Cultivation_Way.Utils;
using DG.Tweening;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class SimpleCultibookButton : APrefab<SimpleCultibookButton>
{
    public Text text { get; private set; }
    public Image background { get; private set; }
    public Image icon { get; private set; }
    public TipButton tip_button { get; private set; }
    public Cultibook cultibook { get; private set; }

    protected override void Init()
    {
        if (Initialized) return;
        text = transform.Find("Title").GetComponent<Text>();
        background = GetComponent<Image>();
        tip_button = transform.Find("Icon").GetComponent<TipButton>();
        icon = transform.Find("Icon").GetComponent<Image>();
        tip_button.type = Constants.Core.mod_prefix + "cultibook";
        tip_button.hoverAction = showTooltip;
        tip_button.clickAction = showTooltip;
        icon.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!CW_Core.mod_state.is_awarding) return;
            if (Config.selectedUnit == null || !Config.selectedUnit.isAlive()) return;
            if (cultibook == null) return;

            WindowCultibookLibrary.Instance.ShowConfirmAwardWindow(cultibook, this);
        });
        base.Init();
    }

    private void showTooltip()
    {
        if (cultibook == null) return;
        Tooltip.show(icon.gameObject, tip_button.type, new TooltipData
        {
            tip_description = GeneralHelper.to_json(cultibook, true)
        });
        icon.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        icon.transform.DOKill();
        icon.transform.DOScale(1f, 0.1f).SetEase(Ease.InBack);
    }

    public void Setup(Cultibook pCultibook, Vector2 pSize = default)
    {
        Init();
        cultibook = pCultibook;
        icon.sprite = pCultibook.GetSprite();
        text.text = pCultibook.name;
        SetSize(pSize == default ? new Vector2(32, 36) : pSize);
    }

    public override void SetSize(Vector2 pSize)
    {
        base.SetSize(pSize);
        GetComponent<VerticalLayoutGroup>().padding =
            new RectOffset(0, 0, (int)(pSize.y * 0.05f), (int)(pSize.y * 0.05f));
        GetComponent<VerticalLayoutGroup>().spacing = pSize.y * 0.02f;
        icon.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x * 0.8f, pSize.x * 0.8f);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x * 0.9f, pSize.y * 0.15f);
    }

    private static void _init()
    {
        GameObject obj = new("SimpleCultibookButton", typeof(Image), typeof(VerticalLayoutGroup));
        obj.transform.SetParent(CW_Core.ui_prefab_library);
        obj.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        obj.GetComponent<Image>().type = Image.Type.Sliced;
        var layout = obj.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 3;

        GameObject button = new("Icon", typeof(Button), typeof(Image), typeof(TipButton));
        button.transform.SetParent(obj.transform);
        button.transform.localScale = Vector3.one;

        GameObject text = new("Title", typeof(Text));
        text.transform.SetParent(obj.transform);
        text.transform.localScale = Vector3.one;
        var textComponent = text.GetComponent<Text>();
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.resizeTextForBestFit = true;
        textComponent.resizeTextMinSize = 1;
        textComponent.resizeTextMaxSize = 10;
        textComponent.text = "";
        textComponent.color = Color.white;
        textComponent.font = LocalizedTextManager.currentFont;

        Prefab = obj.AddComponent<SimpleCultibookButton>();
    }
}