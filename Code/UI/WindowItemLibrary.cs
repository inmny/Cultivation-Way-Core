using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
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

public class WindowItemLibrary : AutoLayoutWindow<WindowItemLibrary>, ILibraryWindow<ItemData>
{
    private SimpleEquipmentButton _button;
    private ConfirmWindow _confirm_window;

    private ItemData _item_data;
    private ObjectPoolGenericMono<SimpleEquipmentButton>[] _item_pools;
    private AutoGridLayoutGroup[] _item_stage_groups;
    public static WindowItemLibrary Instance { get; private set; }
    public List<ItemData> Data { get; set; } = new();

    public void SaveData()
    {
        if (Data.Count > 0)
            File.WriteAllText(Paths.ItemLibraryPath, GeneralHelper.to_json(Data, true));
        else
            try
            {
                File.Delete(Paths.ItemLibraryPath);
            }
            catch (Exception e)
            {
                CW_Core.LogError(e.Message);
                CW_Core.LogError(e.StackTrace);
            }
    }

    public void LoadData()
    {
        if (!File.Exists(Paths.ItemLibraryPath)) return;
        var read_data = GeneralHelper.from_json<List<CW_ItemData>>(File.ReadAllText(Paths.ItemLibraryPath));
        Data.Clear();
        foreach (var data in read_data)
            if (Manager.items.dict.ContainsKey(data.id))
            {
                data.addition_stats.AfterDeserialize();
                Data.Add(data);
            }
            else
            {
                Data.Add(GeneralHelper.from_json<ItemData>(GeneralHelper.to_json(data)));
            }
    }

    public void PushData(ItemData pData)
    {
        Data.Add(pData);
        SaveData();
    }

    protected override void Init()
    {
        GetLayoutGroup().spacing = 3;
        _item_stage_groups =
            new AutoGridLayoutGroup[Constants.Core.item_level_count / Constants.Core.item_level_per_stage];
        _item_pools = new ObjectPoolGenericMono<SimpleEquipmentButton>[_item_stage_groups.Length];
        for (int i = 0; i < _item_stage_groups.Length; i++)
        {
            SimpleText item_group_title = Instantiate(SimpleText.Prefab, null);
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
            auto_localized_text.key = $"item_stage_{i}";
            auto_localized_text.autoField = true;
            LocalizedTextManager.addTextField(auto_localized_text);

            AddChild(item_group_title.gameObject);
            _item_stage_groups[i] = this.BeginGridGroup(
                5, GridLayoutGroup.Constraint.FixedColumnCount,
                default, new Vector2(32, 36), new Vector2(4, 4)
            );
            _item_pools[i] =
                new ObjectPoolGenericMono<SimpleEquipmentButton>(SimpleEquipmentButton.Prefab,
                    _item_stage_groups[i].transform);

            _item_stage_groups[i].layout.padding = new RectOffset(5, 5, 5, 5);
            Image bg = _item_stage_groups[i].gameObject.AddComponent<Image>();
            bg.sprite = i switch
            {
                0 => SpriteTextureLoader.getSprite("ui/special/windowInnerSliced"),
                1 => SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty"),
                2 => SpriteTextureLoader.getSprite("ui/special/darkInputFieldEmpty"),
                3 => SpriteTextureLoader.getSprite("ui/special/button"),
                4 => SpriteTextureLoader.getSprite("ui/special/button2"),
                _ => SpriteTextureLoader.getSprite("ui/special/button2")
            };
            if (i == 0)
            {
                bg.color = new Color(0, 0, 0, 0.455f);
            }

            bg.type = Image.Type.Sliced;
        }

        _confirm_window = Instantiate(ConfirmWindow.Prefab, BackgroundTransform);
        _confirm_window.Setup(
            LM.Get("confirm_award"),
            LM.Get("confirm_award_intro"),
            result =>
            {
                if (result) GiveItem();
            }
        );
        _confirm_window.gameObject.SetActive(false);
        Instance = this;
    }

    public static void CollectItem(ItemData pItemData)
    {
        Instance.PushData(pItemData);
    }

    public override void OnNormalEnable()
    {
        base.OnNormalEnable();
        foreach (var item in Manager.items.list)
        {
            if (Manager.items.IsCreatable(item)) continue;

            CW_ItemData item_data = new(item, null, item.MainMaterials.Keys.First());
            SimpleEquipmentButton button = _item_pools[item_data.Level / Constants.Core.item_level_per_stage]
                .getNext(0);
            button.Setup(item_data, new Vector2(32, 36));
        }

        foreach (var item in Data)
        {
            SimpleEquipmentButton button;
            if (item is not CW_ItemData cw_item)
            {
                button = _item_pools[0].getNext(0);
                button.Setup(item, new Vector2(32, 36));
                continue;
            }

            button = _item_pools[cw_item.Level / Constants.Core.item_level_per_stage].getNext();
            button.Setup(cw_item, new Vector2(32, 36));
        }
    }

    public override void OnNormalDisable()
    {
        base.OnNormalDisable();
        foreach (var pool in _item_pools)
        {
            pool.clear();
        }

        SaveData();
        CW_Core.mod_state.is_awarding = false;
    }

    [Hotfixable]
    public void ShowConfirmAwardWindow(ItemData pItemData, SimpleEquipmentButton pButton)
    {
        _item_data = pItemData;
        _button = pButton;
        _confirm_window.title.text = LM.Get("confirm_award");

        var item_name = _item_data.name;
        if (string.IsNullOrEmpty(item_name)) item_name = LM.Get(_item_data.id);
        _confirm_window.introduction.text = LM.Get("confirm_award_item_info").Replace("$item$", item_name);
        _confirm_window.gameObject.SetActive(true);
    }

    [Hotfixable]
    private void GiveItem()
    {
        var asset = AssetManager.items.get(_item_data.id);

        Config.selectedUnit.equipment.getSlot(asset.equipmentType).setItem(_item_data);
        Config.selectedUnit.setStatsDirty();
        Config.selectedUnit.dirty_sprite_item = true;
        Data.Remove(_item_data);

        ScrollWindowComponent.clickBack();
    }
}