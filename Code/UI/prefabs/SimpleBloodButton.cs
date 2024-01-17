using System.Collections.Generic;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;

namespace Cultivation_Way.UI.prefabs;

public class SimpleBloodButton : APrefab<SimpleBloodButton>
{
    public Dictionary<string, float> Blood { get; private set; }

    public void Setup(Dictionary<string, float> pBlood)
    {
        Blood = pBlood;
    }

    private static void _init()
    {
        var obj = Instantiate(SimpleButton.Prefab, CW_Core.ui_prefab_library).gameObject;

        var button = obj.GetComponent<SimpleButton>();
        button.Setup(() => { }, SpriteTextureLoader.getSprite("ui/icons/iconWus"), pSize: new Vector2(36, 36));


        Prefab = obj.AddComponent<SimpleBloodButton>();
    }
}