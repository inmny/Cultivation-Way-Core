using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI
{
    internal class CW_TipButton : MonoBehaviour
    {
        public Button button;
        public Image image;
        private Action<GameObject> tooltip_action;
        private Vector3 origin_scale = new(-1, -1);
        private void Awake()
        {
            this.button = GetComponent<Button>();
            this.image = GetComponent<Image>();
        }
        private void Start()
        {
            this.button.onClick.AddListener(show_tooltip);
            this.button.OnHover(show_tooltip);
            this.button.OnHoverOut(Tooltip.hideTooltip);
        }
        private void show_tooltip()
        {
            tooltip_action?.Invoke(this.gameObject);

            if (this.origin_scale.x < 0 && this.origin_scale.y < 0)
            {
                this.origin_scale = transform.localScale;
            }
            transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y * 1.25f, 1f);
            transform.DOKill(true);
            transform.DOScale(origin_scale, 0.1f).SetEase(Ease.InBack);
        }
        public void load(string icon, Action<GameObject> show_tooltip)
        {
            this.button = GetComponent<Button>();
            this.image = GetComponent<Image>();
            this.tooltip_action = show_tooltip;
            this.image.sprite = Resources.Load<Sprite>("ui/Icons/" + icon);
        }
    }
    internal static class Prefabs
    {
        public static CW_TipButton tip_button_prefab;
        private static Dictionary<string, UnityEngine.Object> resources_dict;
        public static void init()
        {
            resources_dict = ReflectionUtility.Reflection.GetField(typeof(NCMS.Utils.ResourcesPatch), null, "modsResources") as Dictionary<string, UnityEngine.Object>;

            set_tip_button_prefab();
            add_tooltip_element_prefab();
            add_tooltip_cultibook_prefab();
            add_tooltip_blood_nodes_prefab();
            add_tooltip_cultisys_prefab();
        }

        private static void add_tooltip_element_prefab()
        {
            Tooltip tooltip = UnityEngine.Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
            tooltip.gameObject.name = Constants.Core.mod_prefix+"element";

            resources_dict["tooltips/tooltip_"+tooltip.gameObject.name] = tooltip;
        }
        private static void add_tooltip_cultibook_prefab()
        {
            Tooltip tooltip = UnityEngine.Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
            tooltip.gameObject.name = Constants.Core.mod_prefix + "cultibook";

            resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
        }
        private static void add_tooltip_blood_nodes_prefab()
        {
            Tooltip tooltip = UnityEngine.Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
            tooltip.gameObject.name = Constants.Core.mod_prefix + "blood_nodes";

            resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
        }
        private static void add_tooltip_cultisys_prefab()
        {
            Tooltip tooltip = UnityEngine.Object.Instantiate(Resources.Load<Tooltip>("tooltips/tooltip_normal"), CW_Core.prefab_library);
            tooltip.gameObject.name = Constants.Core.mod_prefix + "cultisys";

            resources_dict["tooltips/tooltip_" + tooltip.gameObject.name] = tooltip;
        }

        private static void set_tip_button_prefab()
        {
            GameObject _obj = new("Tip_Button_Prefab");
            _obj.SetActive(false);
            _obj.transform.SetParent(CW_Core.prefab_library);
            _obj.AddComponent<Button>();
            _obj.AddComponent<Image>();
            _obj.GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
            tip_button_prefab = _obj.AddComponent<CW_TipButton>();
        }
    }
}
