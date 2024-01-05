using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class BloodMergePanel : APrefab<BloodMergePanel>
{
    private static void _init()
    {
        var obj = new GameObject("BloodMergePanel", typeof(Image));
        obj.transform.SetParent(CW_Core.ui_prefab_library);

        var bg = obj.GetComponent<Image>();
        bg.sprite = SpriteTextureLoader.getSprite("ui/special/windowEmptyFrame");

        Prefab = obj.AddComponent<BloodMergePanel>();
    }
}