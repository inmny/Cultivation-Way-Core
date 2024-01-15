using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class BloodMergePanel : APrefab<BloodMergePanel>
{
    protected override void Init()
    {
        if (Initialized) return;
        base.Init();
    }

    public void Setup()
    {
    }

    private static void _init()
    {
        var obj = new GameObject("BloodMergePanel", typeof(Image));
        obj.transform.SetParent(CW_Core.ui_prefab_library);

        var bg = obj.GetComponent<Image>();
        bg.sprite = SpriteTextureLoader.getSprite("ui/special/windowEmptyFrame");
        bg.type = Image.Type.Sliced;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 200);

        var input_rect = new GameObject("InputRectLeft", typeof(Image));
        input_rect.transform.SetParent(obj.transform);
        input_rect.transform.localPosition = new Vector3(-32, 50);
        input_rect.transform.localScale = Vector3.one;
        var input_rect_img = input_rect.GetComponent<Image>();
        input_rect_img.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        input_rect_img.type = Image.Type.Sliced;
        input_rect_img.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);

        var input_rect2 = new GameObject("InputRectRight", typeof(Image));
        input_rect2.transform.SetParent(obj.transform);
        input_rect2.transform.localPosition = new Vector3(32, 50);
        input_rect2.transform.localScale = Vector3.one;
        input_rect_img = input_rect2.GetComponent<Image>();
        input_rect_img.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        input_rect_img.type = Image.Type.Sliced;
        input_rect_img.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);

        var output_rect = new GameObject("OutputRect", typeof(Image));
        output_rect.transform.SetParent(obj.transform);
        output_rect.transform.localPosition = new Vector3(0, -50);
        output_rect.transform.localScale = Vector3.one;
        input_rect_img = output_rect.GetComponent<Image>();
        input_rect_img.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        input_rect_img.type = Image.Type.Sliced;
        input_rect_img.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);

        Prefab = obj.AddComponent<BloodMergePanel>();
    }
}