using System.Linq;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class BloodMergePanel : APrefab<BloodMergePanel>
{
    public RectTransform input_rect_left;
    public RectTransform input_rect_right;
    public RectTransform output_rect;

    public SimpleBloodButton input_left;
    public SimpleBloodButton input_right;
    public SimpleBloodButton output;
    private SimpleButton merge_button;

    protected override void Init()
    {
        if (Initialized) return;
        base.Init();
        input_rect_left = transform.Find("InputRectLeft").GetComponent<RectTransform>();
        input_rect_right = transform.Find("InputRectRight").GetComponent<RectTransform>();
        output_rect = transform.Find("OutputRect").GetComponent<RectTransform>();
        merge_button = transform.Find("MergeButton").GetComponent<SimpleButton>();

        merge_button.Setup(() =>
            {
                if (input_left == null || input_right == null || output != null) return;
                var blood_left = input_left.Blood;
                var blood_right = input_right.Blood;

                var blood_output = blood_left.ToDictionary(blood => blood.Key, blood => blood.Value);
                foreach (var blood in blood_right)
                    if (!blood_output.ContainsKey(blood.Key))
                        blood_output.Add(blood.Key, blood.Value);
                    else
                        blood_output[blood.Key] += blood.Value;


                var keys = blood_output.Keys.ToList();

                var sum_at_first = keys.Sum(key => blood_output[key]);

                var curr_sum = sum_at_first;


                foreach (var key in keys
                             .Where(key => !(blood_output[key] / sum_at_first >= Constants.Others.blood_ignore_line)))
                {
                    curr_sum -= blood_output[key];
                    blood_output.Remove(key);
                }

                keys.Clear();
                keys.AddRange(blood_output.Keys);
                foreach (var key in keys) blood_output[key] /= curr_sum;

                var obj = input_left.gameObject;
                input_left = null;
                obj.transform.SetParent(null);
                Destroy(obj);
                obj = input_right.gameObject;
                input_right = null;
                obj.transform.SetParent(null);
                Destroy(obj);

                output = Instantiate(SimpleBloodButton.Prefab, output_rect);
                output.Setup(blood_output);
            }, SpriteTextureLoader.getSprite("ui/icons/iconClock"), pSize: new Vector2(32, 32), pTipType: "normal",
            pTipData: new TooltipData
            {
                tip_name = "MergeBlood"
            });
    }

    public void Setup()
    {
        Init();
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

        var merge_button = Instantiate(SimpleButton.Prefab, obj.transform);
        merge_button.name = "MergeButton";
        merge_button.transform.SetParent(obj.transform);
        merge_button.transform.localPosition = new Vector3(0, 0);
        merge_button.transform.localScale = Vector3.one;

        Prefab = obj.AddComponent<BloodMergePanel>();
    }
}