using NeoModLoader.api.attributes;
using NeoModLoader.General;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class CityWindowAddition : AbstractWindowAddition
{
    private CityWindow city_window;
    private RectTransform drag_receiver;
    private Text drag_receiver_text;

    private bool first_update = true;

    private void Update()
    {
        if (first_update)
        {
            patch_equipment_buttons_as_draggable();
            first_update = false;
        }
    }

    private void OnEnable()
    {
        if (!initialized) return;
        first_update = true;
    }

    protected override void Initialize()
    {
        city_window = GetComponent<CityWindow>();
        GameObject receiver = new("Drag Receiver", typeof(Image));
        receiver.transform.SetParent(Background);
        receiver.transform.localPosition = new Vector3(-191, -109);
        receiver.transform.localScale = Vector3.one;
        receiver.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        receiver.GetComponent<Image>().type = Image.Type.Sliced;

        drag_receiver = receiver.GetComponent<RectTransform>();
        drag_receiver.sizeDelta = new Vector2(150, 60);
        drag_receiver.gameObject.SetActive(false);

        GameObject receiver_text = new("Text", typeof(Text));
        receiver_text.transform.SetParent(receiver.transform);
        receiver_text.transform.localPosition = Vector3.zero;
        receiver_text.transform.localScale = Vector3.one;
        var text = receiver_text.GetComponent<Text>();
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextForBestFit = false;
        text.font = LocalizedTextManager.currentFont;
        text.fontSize = 12;
        text.GetComponent<RectTransform>().sizeDelta = drag_receiver.sizeDelta * 0.95f;
        drag_receiver_text = text;
    }

    [Hotfixable]
    private void patch_equipment_buttons_as_draggable()
    {
        drag_receiver_text.text = LM.Get("DragItemOrActorHere");
        foreach (var component in Content.GetComponentsInChildren(typeof(EquipmentButton), true))
        {
            var equip_button = (EquipmentButton)component;
            var button = equip_button.GetComponent<EventTrigger>();
            if (button == null) button = equip_button.gameObject.AddComponent<EventTrigger>();
            var need_add_trigger = true;
            foreach (var trigger in button.triggers)
                if (trigger.eventID == EventTriggerType.EndDrag)
                {
                    need_add_trigger = false;
                    break;
                }

            if (!need_add_trigger) continue;
            EventTrigger.Entry entry;
            Transform old_transform = null;
            var old_position = Vector2.zero;
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData) return;
                drag_receiver_text.color = Color.white;
                drag_receiver.gameObject.SetActive(true);
                old_transform = equip_button.transform.parent;
                old_position = equip_button.transform.position;
                equip_button.transform.SetParent(Background);
                equip_button.transform.localScale = Vector3.one;
                equip_button.transform.position = pointerEventData.position;
            });
            button.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData) return;
                if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                    drag_receiver_text.color = Color.yellow;
                else
                    drag_receiver_text.color = Color.white;
                equip_button.transform.position = pointerEventData.position;
            });
            button.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener([Hotfixable](data) =>
            {
                if (data is not PointerEventData pointerEventData) return;
                if (RectTransformUtility.RectangleContainsScreenPoint(drag_receiver, pointerEventData.position))
                {
                    equip_button.gameObject.SetActive(false);


                    var item_asset = AssetManager.items.get(equip_button.item_data.id);
                    city_window.city.data.storage.items_dicts[item_asset.equipmentType].Remove(equip_button.item_data);

                    var idx = 0;
                    var total = city_window.city.data.storage.items_dicts[item_asset.equipmentType].Count;
                    foreach (RectTransform button in old_transform)
                    {
                        if (!button.gameObject.activeSelf) continue;
                        if (button.GetComponent<EquipmentButton>() == null) continue;
                        var num2 = old_transform.GetComponent<RectTransform>().rect.width - 10 * 1.5f;
                        var num3 = 22.4f * 0.8f;
                        if (total * num3 >= num2) num3 = num2 / total;
                        var x = 10 + num3 * idx;
                        var y = -11f;
                        button.anchoredPosition = new Vector2(x, -11);

                        idx++;
                    }

                    WindowItemLibrary.CollectItem(equip_button.item_data);
                }
                else
                {
                    equip_button.transform.SetParent(old_transform);
                    equip_button.transform.position = old_position;
                }

                drag_receiver.gameObject.SetActive(false);
            });
            button.triggers.Add(entry);
        }
    }
}