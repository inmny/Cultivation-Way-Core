using System.Collections.Generic;
using NeoModLoader.General.UI.Prefabs;
using NeoModLoader.General.UI.Window.Layout;
using NeoModLoader.General.UI.Window.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI.prefabs;

public class BloodLibraryGrid : APrefab<BloodLibraryGrid>
{
    public  SimpleButton                             clear_button;
    public  RectTransform                            GridTransform;
    private ObjectPoolGenericMono<SimpleBloodButton> blood_button_pool;
    private InputField                               title_input;
    public  string                                   id { get; private set; }

    public void AddBloodNode(Dictionary<string, float> pBlood)
    {
        Init();
        var button = blood_button_pool.getNext(0);
        button.Setup(pBlood);
    }

    protected override void Init()
    {
        if (Initialized) return;

        clear_button = transform.Find("Top/ClearButton").GetComponent<SimpleButton>();
        clear_button.Setup(() => { WindowBloodLibrary.Instance.DissolveGroup(this); },
                           SpriteTextureLoader.getSprite("ui/icons/worldrules/icon_rebellion"), pTipType: "normal",
                           pTipData: new TooltipData
                           {
                               tip_name = "dissolve",
                               tip_description = "dissolve_group"
                           }, pSize: new Vector2(18, 18));

        title_input = transform.Find("Top/Title/InputField").GetComponent<InputField>();
        transform.Find("Top/Title").GetComponent<TextInput>().Setup("Group Name",
                                                                    new_title =>
                                                                    {
                                                                        WindowBloodLibrary.Instance.RenameGroup(
                                                                            this, new_title);
                                                                    });
        GridTransform = transform.Find("Grid").GetComponent<RectTransform>();
        blood_button_pool =
            new ObjectPoolGenericMono<SimpleBloodButton>(SimpleBloodButton.Prefab, GridTransform);
        base.Init();
    }

    public void Setup(string pTitle)
    {
        Init();
        id = pTitle;
        name = pTitle;
        title_input.text = pTitle;
        blood_button_pool.clear();
    }

    public void Rename(string pNewName)
    {
        name = pNewName;
        title_input.text = pNewName;
    }

    private static void _init()
    {
        var root = Instantiate(AutoVertLayoutGroup.Prefab, null);
        root.name = "BloodLibraryGrid";
        root.transform.SetParent(CW_Core.ui_prefab_library);
        root.Setup();
        root.fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        var top = root.BeginHoriGroup(new Vector2(200, 18), TextAnchor.MiddleRight, 24);
        top.name = "Top";

        var title = Instantiate(TextInput.Prefab, null);
        title.name = "Title";
        title.Setup("Group Name", _ => { });
        title.SetSize(new Vector2(100, 18));
        title.text.alignment = TextAnchor.MiddleCenter;
        top.AddChild(title.gameObject);

        var clear_button = Instantiate(SimpleButton.Prefab, null);
        clear_button.name = "ClearButton";
        top.AddChild(clear_button.gameObject);

        var grid = root.BeginGridGroup(5, pSize: default, pSpacing: new Vector2(4, 4),
                                       pCellSize: new Vector2(36,                  36));
        grid.name = "Grid";
        grid.fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        grid.fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        grid.layout.padding = new RectOffset(4, 4, 4, 4);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 10);

        var grid_bg = grid.gameObject.AddComponent<Image>();
        grid_bg.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        grid_bg.type = Image.Type.Sliced;

        root.SetSize(new Vector2(200, 39));
        Prefab = root.gameObject.AddComponent<BloodLibraryGrid>();
    }
}