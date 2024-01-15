using Cultivation_Way.UI.prefabs;
using NeoModLoader.General;
using NeoModLoader.General.UI.Window;
using UnityEngine;

namespace Cultivation_Way.UI;

public class WindowModInfo : MultiTabWindow<WindowModInfo>
{
    protected override void Init()
    {
        InitMainPage();
        InitElementPage();
        InitEnergyPage();
        InitCultisysPage();
        InitCultibookPage();
        InitBloodPage();
        InitItemPage();
        InitSpellPage();
        InitElixirPage();
        InitStatusPage();
        InitOtherPage();
    }

    private void InitOtherPage()
    {
        var page = CreateTab("Other", SpriteTextureLoader.getSprite("ui/icons/iconAbout"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitStatusPage()
    {
        var page = CreateTab("Status", SpriteTextureLoader.getSprite("ui/icons/iconElement"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitEnergyPage()
    {
        var page = CreateTab("Energy", SpriteTextureLoader.getSprite("ui/icons/iconWakan"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitElixirPage()
    {
        var page = CreateTab("Elixir", SpriteTextureLoader.getSprite("ui/icons/elixirs/iconNormal"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitSpellPage()
    {
        var page = CreateTab("Spell", SpriteTextureLoader.getSprite("ui/cw_icons/天师"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitItemPage()
    {
        var page = CreateTab("Item", SpriteTextureLoader.getSprite("ui/icons/items/icon_紫金葫芦_violet_gold"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitBloodPage()
    {
        var page = CreateTab("Blood", SpriteTextureLoader.getSprite("ui/icons/iconWus"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitCultibookPage()
    {
        var page = CreateTab("Cultibook", SpriteTextureLoader.getSprite("ui/icons/iconCultiBook_Immortal"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitCultisysPage()
    {
        var page = CreateTab("Cultisys", SpriteTextureLoader.getSprite("ui/icons/iconCultisysSquare"));


        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitElementPage()
    {
        var page = CreateTab("Element", SpriteTextureLoader.getSprite("ui/icons/iconElement"));

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }

    private void InitMainPage()
    {
        var page = this;

        var sub_title = Instantiate(SimpleText.Prefab, null);
        page.AddChild(sub_title.gameObject);
        sub_title.Setup(LM.Get("placeholder"), TextAnchor.MiddleCenter);
        sub_title.background.enabled = false;
    }
}