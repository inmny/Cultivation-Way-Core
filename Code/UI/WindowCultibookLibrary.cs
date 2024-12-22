using System;
using System.Collections.Generic;
using System.IO;
using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.UI.prefabs;
using Cultivation_Way.Utils;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.UI.Window;
using NeoModLoader.General.UI.Window.Layout;
using NeoModLoader.General.UI.Window.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class WindowCultibookLibrary : AutoLayoutWindow<WindowCultibookLibrary>, ILibraryWindow<Cultibook>
{
    private SimpleCultibookButton _button;
    private ConfirmWindow _confirm_window;

    private       Cultibook                                      _cultibook;
    private       ObjectPoolGenericMono<SimpleCultibookButton>[] _cultibook_pools;
    private       AutoGridLayoutGroup[]                          _cultibook_stage_groups;
    public static WindowCultibookLibrary                         Instance { get; private set; }
    public        List<Cultibook>                                Data     { get; set; } = new();

    public void SaveData()
    {
        if (Data.Count > 0)
            File.WriteAllText(Paths.CultibookLibraryPath, GeneralHelper.to_json(Data, true));
        else
            try
            {
                File.Delete(Paths.CultibookLibraryPath);
            }
            catch (Exception e)
            {
                CW_Core.LogError(e.Message);
                CW_Core.LogError(e.StackTrace);
            }
    }

    public void LoadData()
    {
        if (!File.Exists(Paths.CultibookLibraryPath)) return;
        var read_data = GeneralHelper.from_json<List<Cultibook>>(File.ReadAllText(Paths.CultibookLibraryPath), true);
        Data.Clear();
        foreach (var data in read_data)
        {
            data.bonus_stats.AfterDeserialize();
            Data.Add(data);
        }
    }

    public void PushData(Cultibook pData)
    {
        Data.Add(pData);
        SaveData();
    }

    protected override void Init()
    {
        GetLayoutGroup().spacing = 3;
        _cultibook_stage_groups =
            new AutoGridLayoutGroup[Constants.Core.cultibook_level_count / Constants.Core.cultibook_level_per_stage];
        _cultibook_pools = new ObjectPoolGenericMono<SimpleCultibookButton>[_cultibook_stage_groups.Length];
        for (var i = 0; i < _cultibook_stage_groups.Length; i++)
        {
            var item_group_title = Instantiate(SimpleText.Prefab, null);
            item_group_title.Setup(
                "",
                TextAnchor.MiddleCenter,
                new Vector2(200, 10)
            );
            item_group_title.text.resizeTextMaxSize = 10;
            item_group_title.text.color = i switch
            {
                0 => Color.white,
                1 => Color.green,
                2 => Color.blue,
                3 => Color.magenta,
                4 => Color.yellow,
                _ => Color.red
            };
            item_group_title.text.supportRichText = true;
            item_group_title.background.enabled = false;
            var auto_localized_text = item_group_title.text.gameObject.AddComponent<LocalizedText>();
            auto_localized_text.key = $"cultibook_order_{i}";
            auto_localized_text.autoField = true;
            LocalizedTextManager.addTextField(auto_localized_text);

            AddChild(item_group_title.gameObject);
            _cultibook_stage_groups[i] = this.BeginGridGroup(
                5, GridLayoutGroup.Constraint.FixedColumnCount,
                default, new Vector2(32, 36), new Vector2(4, 4)
            );
            _cultibook_pools[i] =
                new ObjectPoolGenericMono<SimpleCultibookButton>(SimpleCultibookButton.Prefab,
                    _cultibook_stage_groups[i].transform);

            _cultibook_stage_groups[i].layout.padding = new RectOffset(5, 5, 5, 5);
            var bg = _cultibook_stage_groups[i].gameObject.AddComponent<Image>();
            bg.sprite = i switch
            {
                0 => SpriteTextureLoader.getSprite("ui/special/windowInnerSliced"),
                1 => SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty"),
                2 => SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty"),
                3 => SpriteTextureLoader.getSprite("ui/special/button"),
                4 => SpriteTextureLoader.getSprite("ui/special/button2"),
                _ => SpriteTextureLoader.getSprite("ui/special/button2")
            };
            if (i == 0) bg.color = new Color(0, 0, 0, 0.455f);

            bg.type = Image.Type.Sliced;
        }

        _confirm_window = Instantiate(ConfirmWindow.Prefab, BackgroundTransform);
        _confirm_window.Setup(
            LM.Get("confirm_award"),
            LM.Get("confirm_award_intro"),
            result =>
            {
                if (result) GiveCultibook();
            }
        );
        _confirm_window.gameObject.SetActive(false);
        Instance = this;
        LoadData();
    }

    private void GiveCultibook()
    {
        Cultibook copy = new();
        copy.copy_from(_cultibook);
        copy.id = Guid.NewGuid().ToString();

        Manager.cultibooks.add(copy);

        Config.selectedUnit.data.SetCultibook(copy);
        Config.selectedUnit.setStatsDirty();
        Config.selectedUnit.updateStats();

        ScrollWindowComponent.clickBack();
    }

    public override void OnNormalEnable()
    {
        base.OnNormalEnable();
        foreach (var item in Manager.cultibooks.static_list)
        {
            SimpleCultibookButton button;
            var pool = _cultibook_pools[item.Stage];
            button = pool.getNext(0);
            button.Setup(item, new Vector2(32, 36));
        }

        foreach (var item in Data)
        {
            SimpleCultibookButton button;
            var pool = _cultibook_pools[item.Stage];
            button = pool.getNext(0);
            button.Setup(item, new Vector2(32, 36));
        }
    }

    [Hotfixable]
    public override void OnNormalDisable()
    {
        base.OnNormalDisable();
        foreach (var pool in _cultibook_pools) pool.clear();
        CW_Core.mod_state.is_awarding = false;
        SaveData();
    }

    public void ShowConfirmAwardWindow(Cultibook pCultibook, SimpleCultibookButton pButton)
    {
        _cultibook = pCultibook;
        _button = pButton;
        _confirm_window.title.text = LM.Get("confirm_award");

        var item_name = _cultibook.name;

        _confirm_window.introduction.text = LM.Get("confirm_award_cultibook_info").Replace("$cultibook$", item_name);
        _confirm_window.gameObject.SetActive(true);
    }
}