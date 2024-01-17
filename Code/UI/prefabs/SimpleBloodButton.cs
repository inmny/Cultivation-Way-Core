using System.Collections.Generic;
using Cultivation_Way.Utils;
using Cultivation_Way.Utils.General.AboutBlood;
using NeoModLoader.api.attributes;
using NeoModLoader.General.UI.Prefabs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cultivation_Way.UI.prefabs;

public class SimpleBloodButton : APrefab<SimpleBloodButton>
{
    private SimpleButton Button;
    public Dictionary<string, float> Blood { get; private set; }

    [Hotfixable]
    public void Setup(Dictionary<string, float> pBlood)
    {
        Init();
        Blood = pBlood;
        var node = WindowBloodLibrary.Instance.node_dict[MiscUtils.MaxBlood(Blood).Key];

        var actor_asset = AssetManager.actor_library.get(node.ancestor_data.asset_id);


        Button.Setup(() => { },
            SpriteTextureLoader.getSprite($"ui/icons/{actor_asset.icon}"),
            pSize: new Vector2(36, 36),
            pTipType: Constants.Core.mod_prefix + "library_blood_node",
            pTipData: new TooltipData
            {
                tip_name = GeneralHelper.to_json(Blood)
            });

        var event_trigger = Button.GetComponent<EventTrigger>();
        if (event_trigger == null) event_trigger = Button.gameObject.AddComponent<EventTrigger>();

        var begin_drag = new EventTrigger.Entry();
        begin_drag.eventID = EventTriggerType.BeginDrag;
        var on_drag = new EventTrigger.Entry();
        on_drag.eventID = EventTriggerType.Drag;
        var end_drag = new EventTrigger.Entry();
        end_drag.eventID = EventTriggerType.EndDrag;

        Transform last_parent = null;
        var last_position = Vector3.zero;
        begin_drag.callback.AddListener(data =>
        {
            last_parent = transform.parent;
            last_position = transform.localPosition;
            transform.SetParent(WindowBloodLibrary.Instance.background_transform, true);
        });
        on_drag.callback.AddListener(data =>
        {
            if (data is not PointerEventData pointerEventData) return;

            transform.position = pointerEventData.position;
        });
        end_drag.callback.AddListener(data =>
        {
            if (!WindowBloodLibrary.Instance.CheckOnReceiver(this))
            {
                transform.SetParent(last_parent);
                transform.localPosition = last_position;
                transform.localScale = Vector3.one;
            }
            else
            {
                WindowBloodLibrary.Instance.SetButtonToReceiver(this);
            }
        });
        event_trigger.triggers.Add(begin_drag);
        event_trigger.triggers.Add(on_drag);
        event_trigger.triggers.Add(end_drag);
    }

    protected override void Init()
    {
        if (Initialized) return;
        base.Init();
        Button = GetComponent<SimpleButton>();
    }

    private static void _init()
    {
        var obj = Instantiate(SimpleButton.Prefab, CW_Core.ui_prefab_library).gameObject;

        var button = obj.GetComponent<SimpleButton>();
        button.Setup(() => { }, SpriteTextureLoader.getSprite("ui/icons/iconWus"), pSize: new Vector2(36, 36),
            pTipType: Constants.Core.mod_prefix + "library_blood_node");


        Prefab = obj.AddComponent<SimpleBloodButton>();
    }
}