using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.UI.prefabs;
using Cultivation_Way.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Window;
using NeoModLoader.General.UI.Window.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowBloodLibrary : AutoLayoutWindow<WindowBloodLibrary>, ILibraryWindow<Dictionary<string, float>>
{
    private readonly Dictionary<string, BloodLibraryGrid> blood_grids = new();
    private readonly Dictionary<string, List<Dictionary<string, float>>> blood_group = new();
    public readonly Dictionary<string, BloodNodeAsset> node_dict = new();
    private bool all_enabled = true;

    private ObjectPoolGenericMono<BloodLibraryGrid> blood_grid_pool;
    private BloodMergePanel blood_merge_panel;
    private GameObject create_group_obj;

    private BloodLibraryGrid DefaultGroup;

    private int group_code;

    private bool is_dirty;
    private int skip_frame = 4;
    public RectTransform background_transform => BackgroundTransform as RectTransform;
    public static WindowBloodLibrary Instance { get; private set; }

    private void Update()
    {
        if (!Initialized) return;
        if (is_dirty)
        {
            if (skip_frame > 0)
            {
                skip_frame--;
                return;
            }

            all_enabled = !all_enabled;
            UpdateLayoutState();

            if (!all_enabled) return;

            skip_frame = 4;
            is_dirty = false;
        }
    }

    public List<Dictionary<string, float>> Data { get; set; } = new();

    public void SaveData()
    {
    }

    public void LoadData()
    {
    }

    public void PushData(Dictionary<string, float> pData)
    {
        if (pData == null || pData.Count == 0) return;

        foreach (var blood_id in pData.Keys)
        {
            var node = Manager.bloods.get(blood_id);
            if (node == null) continue;

            var copy = GeneralHelper.from_json<BloodNodeAsset>(GeneralHelper.to_json(node, true), true);
            copy.ancestor_stats.AfterDeserialize();

            node_dict[copy.id] = copy;
        }

        Data.Add(pData);
        DefaultGroup.AddBloodNode(pData);
    }

    private void UpdateLayoutState()
    {
        var layoutGroups = gameObject.GetComponentsInChildren<LayoutGroup>().Reverse();
        foreach (var layoutGroup in layoutGroups) layoutGroup.enabled = all_enabled;
        layout.enabled = all_enabled;
    }

    [Hotfixable]
    private void CreateGroup()
    {
        var group = blood_grid_pool.getNext();
        group.Setup($"group_{group_code++}");
        group.transform.SetAsLastSibling();

        blood_group.Add(group.name, new List<Dictionary<string, float>>());
        blood_grids.Add(group.name, group);

        is_dirty = true;
    }

    internal void DissolveGroup(BloodLibraryGrid pGroup)
    {
        if (pGroup == DefaultGroup) return;

        blood_grids.Remove(pGroup.name);

        var bloods = blood_group[pGroup.name];
        blood_group.Remove(pGroup.name);

        blood_grid_pool.InactiveObj(pGroup);

        blood_group[DefaultGroup.name].AddRange(bloods);

        is_dirty = true;
    }

    internal void RenameGroup(BloodLibraryGrid pGroup, string pNewName)
    {
        blood_grids.Remove(pGroup.name);
        blood_grids.Add(pNewName, pGroup);

        var bloods = blood_group[pGroup.name];
        blood_group.Remove(pGroup.name);
        blood_group.Add(pNewName, bloods);

        pGroup.name = pNewName;
    }

    internal bool CheckOnReceiver(SimpleBloodButton pButton)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(blood_merge_panel.input_rect_left,
                pButton.transform.position)
            || RectTransformUtility.RectangleContainsScreenPoint(blood_merge_panel.input_rect_right,
                pButton.transform.position))
            return true;

        foreach (var grid in blood_grids.Values)
            if (RectTransformUtility.RectangleContainsScreenPoint(grid.GetComponent<RectTransform>(),
                    pButton.transform.position))
                return true;
        return false;
    }

    [Hotfixable]
    internal void SetButtonToReceiver(SimpleBloodButton pButton)
    {
        if (blood_merge_panel.input_left == pButton)
            blood_merge_panel.input_left = null;
        else if (blood_merge_panel.input_right == pButton)
            blood_merge_panel.input_right = null;
        else if (blood_merge_panel.output == pButton) blood_merge_panel.output = null;
        var set = false;

        if (RectTransformUtility.RectangleContainsScreenPoint(blood_merge_panel.input_rect_left,
                pButton.transform.position))
        {
            blood_merge_panel.input_left = pButton;
            pButton.transform.SetParent(blood_merge_panel.input_rect_left);
            set = true;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(blood_merge_panel.input_rect_right,
                     pButton.transform.position))
        {
            blood_merge_panel.input_right = pButton;
            pButton.transform.SetParent(blood_merge_panel.input_rect_right);
            set = true;
        }
        else
        {
            foreach (var grid_id in blood_grids.Keys.Where(grid_id => RectTransformUtility.RectangleContainsScreenPoint(
                         blood_grids[grid_id].GetComponent<RectTransform>(),
                         pButton.transform.position)))
            {
                if (!blood_group[grid_id].Contains(pButton.Blood))
                {
                    foreach (var group in blood_group.Values.Where(group => group.Contains(pButton.Blood)))
                    {
                        group.Remove(pButton.Blood);
                        break;
                    }

                    blood_group[grid_id].Add(pButton.Blood);
                }

                pButton.transform.SetParent(blood_grids[grid_id].GridTransform);
                set = true;
                break;
            }
        }

        if (set)
        {
            pButton.transform.localPosition = Vector3.zero;
            pButton.transform.localScale = Vector3.one;
        }

        is_dirty = true;
    }

    public override void OnNormalEnable()
    {
        is_dirty = true;
        if (CW_Core.mod_state.is_awarding)
            blood_merge_panel.gameObject.SetActive(false);
        else
            blood_merge_panel.gameObject.SetActive(true);
    }

    protected override void Init()
    {
        var grid_part = this.BeginVertGroup();
        blood_grid_pool = new ObjectPoolGenericMono<BloodLibraryGrid>(BloodLibraryGrid.Prefab, grid_part.transform);
        var default_grid = blood_grid_pool.getNext(0);
        default_grid.Setup(LM.Get("default"));
        default_grid.clear_button.Button.enabled = false;
        DefaultGroup = default_grid;
        grid_part.AddChild(default_grid.gameObject);

        blood_group.Add(default_grid.name, new List<Dictionary<string, float>>());
        blood_grids.Add(default_grid.name, default_grid);

        var create_group = new GameObject("CreateGroup", typeof(Image), typeof(Button));
        AddChild(create_group);
        create_group.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 24);
        create_group.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/button");
        create_group.GetComponent<Image>().type = Image.Type.Sliced;
        create_group.GetComponent<Button>().onClick.AddListener(CreateGroup);
        create_group_obj = create_group;

        var create_group_text = new GameObject("Text", typeof(Text), typeof(LocalizedText));
        create_group_text.transform.SetParent(create_group.transform);
        create_group_text.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 24);
        create_group_text.GetComponent<Text>().font = LocalizedTextManager.currentFont;
        create_group_text.GetComponent<Text>().fontSize = 16;
        create_group_text.GetComponent<Text>().color = Colors.default_color;
        create_group_text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        create_group_text.GetComponent<LocalizedText>().autoField = true;
        create_group_text.GetComponent<LocalizedText>().setKeyAndUpdate("CreateBloodGroup");
        LocalizedTextManager.addTextField(create_group_text.GetComponent<LocalizedText>());

        blood_merge_panel = Instantiate(BloodMergePanel.Prefab, BackgroundTransform);
        blood_merge_panel.Setup();
        var panel_transform = blood_merge_panel.transform;
        panel_transform.localPosition = new Vector3(-200, 0);
        panel_transform.localScale = Vector3.one;
        Instance = this;
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}