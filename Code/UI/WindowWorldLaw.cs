using System;
using System.Collections.Generic;
using Cultivation_Way.Others;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowWorldLaw : AbstractWindow<WindowWorldLaw>
{
    private readonly Dictionary<string, Transform> grids = new();
    private WorldLawElement switch_law_prefab;

    internal static void init()
    {
        base_init(Constants.Core.worldlaw_window);

        ScrollWindow.checkWindowExist("world_laws");
        GameObject vanllia_worldlaw_window = Windows.GetWindow("world_laws").gameObject;
        vanllia_worldlaw_window.SetActive(false);

        instance.switch_law_prefab = vanllia_worldlaw_window.transform
            .Find("Background/Scroll View/Viewport/Content/Civ/Grid/world_law_diplomacy")
            .GetComponent<WorldLawElement>();

        VerticalLayoutGroup layout = content_transform.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 16;
        layout.padding = new RectOffset(0, 0, 16, 0);

        ContentSizeFitter fitter = content_transform.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    internal void add_switch_law_button(string id, string grid_id, string icon_path)
    {
        if (!grids.ContainsKey(grid_id))
        {
            create_grid(grid_id);
        }

        Transform grid = grids[grid_id];
        WorldLawElement element = Instantiate(switch_law_prefab, grid.Find("Grid"));
        element.name = id;
        element.icon.sprite = SpriteTextureLoader.getSprite(icon_path);
    }

    internal void add_setting_law_button(string id, string grid_id, string icon_path, string window_id,
        Action addition_action = null)
    {
        if (!grids.ContainsKey(grid_id))
        {
            create_grid(grid_id);
        }

        Transform grid = grids[grid_id];
        PowerButton button = CWTab.create_button(id, icon_path, () =>
        {
            addition_action?.Invoke();
            ScrollWindow.showWindow(window_id);
        });
        button.transform.SetParent(grid.Find("Grid"));
    }

    internal void create_grid(string id)
    {
        GameObject grid = new(id, typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        grid.GetComponent<Image>().sprite = FastVisit.get_window_inner_sliced();
        grid.GetComponent<Image>().type = Image.Type.Sliced;
        grid.transform.SetParent(content_transform);
        grid.transform.localScale = new Vector3(1, 1, 1);

        VerticalLayoutGroup layout = grid.GetComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 8;
        layout.padding = new RectOffset(5, 0, -15, 5);

        ContentSizeFitter fitter = grid.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        grids.Add(id, grid.transform);

        GameObject title = new("Title", typeof(LocalizedText), typeof(Text), typeof(Shadow));
        title.transform.SetParent(grid.transform);
        LocalizedText localized_text = title.GetComponent<LocalizedText>();
        Text text = title.GetComponent<Text>();
        localized_text.text = text;
        localized_text.key = id;
        localized_text.autoField = true;

        text.font = LocalizedTextManager.currentFont;
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextMaxSize = 9;
        text.resizeTextMaxSize = 1;
        text.resizeTextForBestFit = true;
        text.fontStyle = FontStyle.Bold;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;

        title.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 12);
        title.transform.localScale = new Vector3(1, 1, 1);

        GameObject container = new("Grid", typeof(RectTransform), typeof(GridLayoutGroup));
        GridLayoutGroup grid_layout = container.GetComponent<GridLayoutGroup>();
        grid_layout.cellSize = new Vector2(32, 32);
        grid_layout.spacing = new Vector2(4, 4);
        grid_layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid_layout.constraintCount = 5;

        container.transform.SetParent(grid.transform);
        container.transform.localScale = new Vector3(1, 1, 1);
    }

    internal static void post_init()
    {
        foreach (Transform grid in instance.grids.Values)
        {
            if (grid.Find("Grid").childCount == 0)
            {
                grid.gameObject.SetActive(false);
            }
        }
    }
}