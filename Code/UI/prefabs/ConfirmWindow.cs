using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class ConfirmWindow : APrefab<ConfirmWindow>
{
    private UnityAction<bool> callback;
    private SimpleButton cancel_button;
    private SimpleButton confirm_button;
    public Text introduction { get; private set; }
    public Text title { get; private set; }

    protected override void Init()
    {
        if (Initialized) return;
        introduction = transform.Find("Introduction").GetComponent<Text>();
        title = transform.Find("Title").GetComponent<Text>();
        confirm_button = transform.Find("ButtonGroup/Confirm").GetComponent<SimpleButton>();
        cancel_button = transform.Find("ButtonGroup/Cancel").GetComponent<SimpleButton>();

        confirm_button.Setup(() =>
        {
            callback?.Invoke(true);
            gameObject.SetActive(false);
        }, null, LM.Get("confirm"), new Vector2(150, 32));
        cancel_button.Setup(() =>
        {
            callback?.Invoke(false);
            gameObject.SetActive(false);
        }, null, LM.Get("confirm"), new Vector2(150, 32));


        base.Init();
    }

    public void Setup(string pTitle, string pIntroduction, UnityAction<bool> pCallback)
    {
        Init();
        title.text = pTitle;
        introduction.text = pIntroduction;
        confirm_button.Text.text = LM.Get("confirm");
        cancel_button.Text.text = LM.Get("cancel");

        callback = pCallback;
    }

    [Hotfixable]
    public override void SetSize(Vector2 pSize)
    {
    }

    private static void _init()
    {
        var obj = new GameObject("ConfirmWindow", typeof(Image), typeof(VerticalLayoutGroup));
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
        intro.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 140);

        var button_group = new GameObject("ButtonGroup", typeof(HorizontalLayoutGroup));
        button_group.transform.SetParent(obj.transform);
        button_group.transform.localScale = Vector3.one;
        button_group.GetComponent<HorizontalLayoutGroup>().childControlHeight = false;
        button_group.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
        button_group.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = false;
        button_group.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
        button_group.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        button_group.GetComponent<HorizontalLayoutGroup>().spacing = 20;
        button_group.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(5, 5, 0, 5);
        button_group.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 42);

        var confirm_button = Instantiate(SimpleButton.Prefab, button_group.transform);
        confirm_button.Setup(null, null, LM.Get("confirm"), new Vector2(120, 28));
        confirm_button.name = "Confirm";

        var cancel_button = Instantiate(SimpleButton.Prefab, button_group.transform);
        cancel_button.Setup(null, null, LM.Get("cancel"), new Vector2(120, 28));
        cancel_button.name = "Cancel";

        Prefab = obj.AddComponent<ConfirmWindow>();
    }
}