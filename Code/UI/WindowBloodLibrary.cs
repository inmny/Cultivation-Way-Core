using System.Collections.Generic;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.UI.prefabs;
using Cultivation_Way.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Window;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowBloodLibrary : AutoLayoutWindow<WindowBloodLibrary>, ILibraryWindow<Dictionary<string, float>>
{
    private readonly Dictionary<string, BloodLibraryGrid> blood_grids = new();
    private readonly Dictionary<string, List<Dictionary<string, float>>> blood_group = new();
    private readonly Dictionary<string, BloodNodeAsset> node_dict = new();
    private ObjectPoolGenericMono<BloodLibraryGrid> blood_grid_pool;
    private BloodMergePanel blood_merge_panel;
    private GameObject create_group_obj;

    private BloodLibraryGrid DefaultGroup;

    private int group_code;

    private bool is_dirty;
    public static WindowBloodLibrary Instance { get; private set; }

    private void Update()
    {
        if (!Initialized) return;
        if (is_dirty)
        {
            if (layout.enabled)
            {
                layout.enabled = false;
            }
            else
            {
                layout.enabled = true;
                is_dirty = false;
            }
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

            node_dict.Add(copy.id, copy);
        }

        Data.Add(pData);
        DefaultGroup.AddBloodNode(pData);
    }

    [Hotfixable]
    private void CreateGroup()
    {
        var group = blood_grid_pool.getNext(create_group_obj.transform.GetSiblingIndex());
        group.Setup($"group_{group_code++}");

        blood_group.Add(group.name, new List<Dictionary<string, float>>());
        blood_grids.Add(group.name, group);

        create_group_obj.transform.SetAsLastSibling();
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

    public override void OnNormalEnable()
    {
        if (CW_Core.mod_state.is_awarding)
            blood_merge_panel.gameObject.SetActive(false);
        else
            blood_merge_panel.gameObject.SetActive(true);
    }

    protected override void Init()
    {
        blood_grid_pool = new ObjectPoolGenericMono<BloodLibraryGrid>(BloodLibraryGrid.Prefab, ContentTransform);
        var default_grid = blood_grid_pool.getNext(0);
        default_grid.Setup(LM.Get("default"));
        default_grid.clear_button.Button.enabled = false;
        DefaultGroup = default_grid;
        AddChild(default_grid.gameObject);

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
        Instance = this;
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}