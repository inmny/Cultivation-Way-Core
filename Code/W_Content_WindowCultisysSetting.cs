using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    class Cultisys_Level_Element : MonoBehaviour
    {
        internal string cultisys_id;
        internal Image bg;
        internal Text level;
        internal Text level_name;
        internal InputField input_field;
        internal Button stats;
        internal void init()
        {
            this.bg = this.gameObject.GetComponent<Image>();
            this.level = this.transform.Find("level_number").GetComponent<Text>();
            this.level_name = this.transform.Find("level_name/input_field").GetComponent<Text>();
            this.input_field = this.transform.Find("level_name/input_field").GetComponent<InputField>();
            this.stats = this.transform.Find("stats_window_entry/Button").GetComponent<Button>();
            this.transform.Find("level_number").localPosition = new Vector3(-80, 0);
            this.transform.Find("level_name").localPosition = new Vector3(0, 0);
            this.transform.Find("stats_window_entry/Button").localPosition = new Vector3(0, 0);
            this.transform.Find("stats_window_entry/Button").localScale = new Vector3(1, 1);
        }
        internal void set_level_name(string name)
        {
            NCMS.Utils.Localization.Set("cultisys_" + cultisys_id + "_" + level.text.Replace("境",""), name);
        }
    }
    internal class W_Content_WindowCultisysSetting : MonoBehaviour
    {
        private string cur_cultisys;
        private InputField name_input_field;
        private Cultisys_Level_Element[] level_settings;
        private Button reset;
        private Transform content_transform;
        private GameObject level_setting_prefab;
        private static W_Content_WindowCultisysSetting wcs;
        private bool initialized = false;
        private bool first_open = true;
        internal static void init()
        {
            ScrollWindow scroll_window = GameObject.Instantiate(Resources.Load<ScrollWindow>("windows/empty"), CanvasMain.instance.transformWindows);
            scroll_window.titleText.GetComponent<LocalizedText>().key = "cultisys_setting_title";
            scroll_window.screen_id = "cultisys_setting";
            scroll_window.name = "cultisys_setting";
            scroll_window.CallMethod("create", false);
            NCMS.Utils.Windows.AllWindows[scroll_window.name] = scroll_window;

            wcs = scroll_window.gameObject.AddComponent<W_Content_WindowCultisysSetting>();
            wcs.transform.Find("Background/Scroll View").gameObject.SetActive(true);
            wcs.content_transform = wcs.transform.Find("Background/Scroll View/Viewport/Content");
            wcs.gameObject.SetActive(false);

            GameObject cultisys_name_setting = new GameObject("name_setting", typeof(Image));
            cultisys_name_setting.transform.SetParent(wcs.content_transform);
            cultisys_name_setting.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/windowNameEdit");
            cultisys_name_setting.transform.localScale = new Vector3(1.2f, 1.2f);
            cultisys_name_setting.transform.localPosition = new Vector3(130, -10.51f);
            cultisys_name_setting.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);
            
            GameObject cultisys_name_input_field = new GameObject("input_field", typeof(Text), typeof(InputField));
            cultisys_name_input_field.transform.SetParent(cultisys_name_setting.transform);
            cultisys_name_input_field.transform.localPosition = new Vector3(0, 0);
            cultisys_name_input_field.transform.localScale = new Vector3(0.8f, 0.8f);
            cultisys_name_input_field.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);

            InputField cultisys_name_input_field_component = cultisys_name_input_field.GetComponent<InputField>();
            Text cultisys_name_text_component = cultisys_name_input_field.GetComponent<Text>();
            cultisys_name_text_component.font = W_Content_Helper.font_STLiti;
            cultisys_name_text_component.text = "体系名";
            cultisys_name_text_component.alignment = TextAnchor.UpperCenter;
            cultisys_name_input_field_component.textComponent = cultisys_name_text_component;
            cultisys_name_input_field_component.onEndEdit = new InputField.SubmitEvent();
            cultisys_name_input_field_component.onEndEdit.AddListener(new UnityEngine.Events.UnityAction<string>((text) =>
            {
                wcs.set_cultisys_name(text);
            }));
            wcs.name_input_field = cultisys_name_input_field_component;

            wcs.level_setting_prefab = new GameObject("level_setting_prefab", typeof(Image), typeof(Cultisys_Level_Element));
            GameObject level_setting_prefab = wcs.level_setting_prefab;
            level_setting_prefab.SetActive(false);
            #region 预制体初始化
            Cultisys_Level_Element __cle = level_setting_prefab.GetComponent<Cultisys_Level_Element>();

            level_setting_prefab.transform.SetParent(wcs.content_transform);
            level_setting_prefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/windowInnerSliced");
            level_setting_prefab.transform.localPosition = new Vector3(129.8f, -80);
            level_setting_prefab.transform.localScale = new Vector3(1, 1);
            level_setting_prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 20);
            __cle.bg = level_setting_prefab.GetComponent<Image>();

            GameObject level_number_text = new GameObject("level_number", typeof(Text));
            level_number_text.transform.SetParent(level_setting_prefab.transform);
            level_number_text.GetComponent<Text>().font = LocalizedTextManager.currentFont;
            level_number_text.GetComponent<Text>().text = "NR";
            level_number_text.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 15);
            level_number_text.transform.localScale = new Vector3(1, 1);
            level_number_text.transform.localPosition = new Vector3(-80, 0);
            __cle.level = level_number_text.GetComponent<Text>();
            __cle.level.fontSize = 10;
            __cle.level.alignment = TextAnchor.UpperRight;

            GameObject level_name_setting = new GameObject("level_name", typeof(Image));
            level_name_setting.transform.SetParent(level_setting_prefab.transform);
            level_name_setting.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/windowNameEdit");
            level_name_setting.GetComponent<RectTransform>().sizeDelta = new Vector2(100,15);
            level_name_setting.transform.localScale = new Vector3(1, 1);

            GameObject level_name_input_field = new GameObject("input_field", typeof(Text), typeof(InputField));
            level_name_input_field.transform.SetParent(level_name_setting.transform);
            level_name_input_field.transform.localPosition = new Vector3(0, 0);
            level_name_input_field.transform.localScale = new Vector3(0.8f, 0.8f);
            level_name_input_field.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);
            

            InputField level_name_input_field_component = level_name_input_field.GetComponent<InputField>();
            Text level_name_text_component = level_name_input_field.GetComponent<Text>();
            __cle.level_name = level_name_text_component;
            __cle.level_name.fontSize = 12;
            level_name_text_component.font = LocalizedTextManager.currentFont;
            level_name_text_component.text = "境界名";
            level_name_text_component.alignment = TextAnchor.UpperCenter;
            level_name_input_field_component.textComponent = level_name_text_component;
            __cle.input_field = level_name_input_field_component;
            /**
            level_name_input_field_component.onEndEdit = new InputField.SubmitEvent();
            level_name_input_field_component.onEndEdit.AddListener(new UnityEngine.Events.UnityAction<string>((text) =>
            {
                wcs.set_level_name(text);
            }));
            */
            GameObject stats_window_entry = new GameObject("stats_window_entry", typeof(Image));
            stats_window_entry.transform.SetParent(level_setting_prefab.transform);
            stats_window_entry.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/red_button");
            stats_window_entry.transform.localPosition = new Vector3(80, 0);
            stats_window_entry.transform.localScale = new Vector3(0.15f, 0.15f);

            GameObject stats_entry_button = new GameObject("Button", typeof(Image), typeof(Button));
            stats_entry_button.transform.SetParent(stats_window_entry.transform);
            __cle.stats = stats_entry_button.GetComponent<Button>();
            stats_entry_button.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconSpectate");

            //level_setting_prefab.SetActive(false);
            #endregion
            wcs.initialized = true;
        }
        void OnEnable()
        {
            if (!initialized) return;
            if (first_open) postfix_init();
            if (string.IsNullOrEmpty(cur_cultisys))
            {
                cur_cultisys = "immortal";
            }
            try
            {
                switch_cultisys(cur_cultisys);
            }
            catch(KeyNotFoundException e)
            {
                Debug.Log("Not found " + cur_cultisys + " now");
            }
        }

        private void postfix_init()
        {
            first_open = false;
            List<CW_Asset_CultiSys> cultisys_list = CW_Library_Manager.instance.cultisys.list;
            int i;
            for (i = 0; i < cultisys_list.Count; i++)
            {
                GameObject cultisys_switch = new GameObject("switch_to_" + cultisys_list[i].id);
                GameObject button = new GameObject("Button", typeof(Image), typeof(Button), typeof(TipButton));
                cultisys_switch.transform.SetParent(this.transform.Find("Background"));
                button.transform.SetParent(cultisys_switch.transform);

                cultisys_switch.transform.localPosition = new Vector3(150, 100 - i * 40);
                button.transform.localScale = new Vector3(1, 1);

                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/" + cultisys_list[i].sprite_name);

                string cultisys_id = cultisys_list[i].id;
                button.GetComponent<TipButton>().textOnClick = LocalizedTextManager.getText("cultisys_name_setting").Replace("$cultisys_name$", LocalizedTextManager.getText("CW_cultisys_" + cultisys_id));

                Button button_button = button.GetComponent<Button>();
                button_button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                {
                    this.switch_cultisys(cultisys_id);
                }));
            }
        }

        void OnDisable()
        {
            clear();
        }
        private void clear()
        {
            if (level_settings == null) return;
            int cur_idx = level_settings.Length-1;
            while (cur_idx >= 0)
            {
                if(level_settings[cur_idx] == null)
                {
                    cur_idx--; continue;
                }
                GameObject object_to_destroy = level_settings[cur_idx].gameObject;
                level_settings[cur_idx] = null;
                GameObject.Destroy(object_to_destroy);
                cur_idx--;
            }
            level_settings = null;
        }
        private void resize()
        {
            RectTransform rect = this.content_transform.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 80 + (this.level_settings==null?0:this.level_settings.Length * 30));
            this.content_transform.Find("name_setting").localPosition = new Vector3(130, -17.5f);
        }
        private void switch_cultisys(string new_id)
        {
            clear();
            cur_cultisys = new_id;
            this.content_transform.Find("name_setting/input_field").GetComponent<InputField>().text = LocalizedTextManager.getText("CW_cultisys_" + cur_cultisys);
            //this.name_input_field.text = LocalizedTextManager.getText("CW_cultisys_" + cur_cultisys);
            CW_Asset_CultiSys cultisys = CW_Library_Manager.instance.cultisys.get(cur_cultisys);
            int length = cultisys.bonus_stats.Length;
            int i;
            level_settings = new Cultisys_Level_Element[Others.CW_Constants.max_cultisys_level];
            for (i = 0; i < length; i++)
            {
                level_settings[i] = GameObject.Instantiate(this.level_setting_prefab, content_transform).GetComponent<Cultisys_Level_Element>();
                level_settings[i].init();
                level_settings[i].cultisys_id = cur_cultisys;
                level_settings[i].level.text = i + "境";
                level_settings[i].name = level_settings[i].level.text;
                level_settings[i].level_name.text = LocalizedTextManager.getText("cultisys_" + cur_cultisys + "_" + i);
                level_settings[i].stats.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                {
                    return;
                }));
                level_settings[i].input_field.text = level_settings[i].level_name.text;
                level_settings[i].input_field.onEndEdit = new InputField.SubmitEvent();
                int cur_idx = i;
                level_settings[i].input_field.onEndEdit.AddListener(new UnityEngine.Events.UnityAction<string>((text) =>
                {
                    level_settings[cur_idx].set_level_name(text);
                }));
                level_settings[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 240 - 30 * i);
                level_settings[i].gameObject.SetActive(true);
            }
            resize();
        }
        private void set_cultisys_name(string new_name)
        {
            if (string.IsNullOrEmpty(cur_cultisys)) return;
            NCMS.Utils.Localization.Set("CW_cultisys_" + cur_cultisys, new_name);
        }
    }
}
