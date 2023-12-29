using Cultivation_Way.Core;
using Cultivation_Way.Others;
using Cultivation_Way.Utils.General.AboutItem;
using DG.Tweening;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;
namespace Cultivation_Way.UI.prefabs;

/// <summary>
///     这是用在WindowItemLibrary中的装备按钮
/// </summary>
public class SimpleEquipmentButton : APrefab<SimpleEquipmentButton>
{
    public Text text { get; private set; }
    public Image background { get; private set; }
    public Image icon { get; private set; }
    public IconOutline outline { get; private set; }
    public TipButton tip_button { get; private set; }
    public ItemData item_data { get; private set; }
    protected override void Init()
    {
        if (Initialized) return;
        text = transform.Find("Title").GetComponent<Text>();
        background = GetComponent<Image>();
        tip_button = transform.Find("Icon").GetComponent<TipButton>();
        icon = transform.Find("Icon").GetComponent<Image>();
        outline = transform.Find("Outline").GetComponent<IconOutline>();
        tip_button.type = "equipment";
        tip_button.hoverAction = showTooltip;
        tip_button.clickAction = showTooltip;
        base.Init();
    }
    [Hotfixable]
    public void Setup(ItemData pItemData, Vector2 pSize = default)
    {
        Init();
        SetSize(pSize == default ? new Vector2(32, 36) : pSize);
        item_data = pItemData;
        icon.sprite = AssetManager.items.get(pItemData.id).getSprite(pItemData);

        int level = 0;
        if (pItemData is CW_ItemData cw_item)
        {
            level = cw_item.Level % Constants.Core.item_level_per_stage;
        }
        text.text = LM.Get($"item_level_{level}");

        outline.show(Colors.GetContainerItemColor(Toolbox.colorToHex(ItemIconConstructor.GetItemQualityColor(pItemData))));
    }
    public override void SetSize(Vector2 pSize)
    {
        base.SetSize(pSize);
        GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, (int)(pSize.y * 0.05f), (int)(pSize.y * 0.05f));
        GetComponent<VerticalLayoutGroup>().spacing = pSize.y * 0.02f;
        icon.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x * 0.8f, pSize.x * 0.8f);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x * 0.9f, pSize.y * 0.15f);
    }
    private void showTooltip()
    {
        if (item_data == null) return;
        Tooltip.show(icon.gameObject, tip_button.type, new TooltipData
        {
            item = item_data
        });
        icon.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        icon.transform.DOKill();
        icon.transform.DOScale(1f, 0.1f).SetEase(Ease.InBack);
    }
    private static void _init()
    {
        GameObject obj = new("SimpleEquipmentButton", typeof(Image), typeof(VerticalLayoutGroup));
        obj.transform.SetParent(CW_Core.ui_prefab_library);
        obj.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        obj.GetComponent<Image>().type = Image.Type.Sliced;
        VerticalLayoutGroup layout = obj.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.spacing = 3;

        GameObject outline = new("Outline", typeof(Image), typeof(IconOutline));
        outline.transform.SetParent(obj.transform);
        outline.transform.localScale = Vector3.one;


        GameObject button = new("Icon", typeof(Button), typeof(Image), typeof(TipButton));
        button.transform.SetParent(obj.transform);
        button.transform.localScale = Vector3.one;

        IconOutline outlineComponent = outline.GetComponent<IconOutline>();
        outlineComponent.parent_image = button.GetComponent<Image>();
        outlineComponent.image = outline.GetComponent<Image>();
        /*
        GameObject icon = new("Icon", typeof(Image), typeof(Outline));
        icon.transform.SetParent(button.transform);
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = Vector3.zero;
        */
        GameObject text = new("Title", typeof(Text));
        text.transform.SetParent(obj.transform);
        text.transform.localScale = Vector3.one;
        Text textComponent = text.GetComponent<Text>();
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.resizeTextForBestFit = true;
        textComponent.resizeTextMinSize = 1;
        textComponent.resizeTextMaxSize = 10;
        textComponent.text = "";
        textComponent.color = Color.white;
        textComponent.font = LocalizedTextManager.currentFont;

        Prefab = obj.AddComponent<SimpleEquipmentButton>();
    }
}