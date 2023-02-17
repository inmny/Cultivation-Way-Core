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
            try
            {
                this.stats = this.transform.Find("stats_window_entry/Button").GetComponent<Button>();
                this.transform.Find("stats_window_entry/Button").localPosition = new Vector3(0, 0);
                this.transform.Find("stats_window_entry/Button").localScale = new Vector3(1, 1);
            }
            catch(Exception e)
            {
                this.stats = null;
            }
            this.transform.Find("level_number").localPosition = new Vector3(-80, 0);
            this.transform.Find("level_name").localPosition = new Vector3(0, 0);
        }
        internal void set_level_name(string name)
        {
            Window_Cultisys_Name_Setting.set_localized("cultisys_" + cultisys_id + "_" + level.text.Replace("境",""), name);
        }
    }
    internal class Window_Cultisys_Name_Setting : MonoBehaviour
    {
        private string cur_cultisys;
        private InputField name_input_field;
        private Cultisys_Level_Element[] level_settings;
        private Text description;
        private Button reset;
        private Transform content_transform;
        private GameObject level_setting_prefab;
        internal static Window_Cultisys_Name_Setting wcs;
        private static string path_to_save = Application.streamingAssetsPath + "/cw/cw_cultisys_name.json";
        private Dictionary<string, string> changed_name = new Dictionary<string, string>();
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

            wcs = scroll_window.gameObject.AddComponent<Window_Cultisys_Name_Setting>();
            //wcs.load_from_file();
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

            GameObject setting_description = new GameObject("Description", typeof(Text));
            setting_description.transform.SetParent(wcs.content_transform);
            setting_description.GetComponent<RectTransform>().sizeDelta = new Vector2(162, 100);
            setting_description.GetComponent<Text>().text = "调整始终有效\n如需恢复请删除\nworldbox_Data/StreamingAssets/cw/cw_cultisys_name.json";
            wcs.description = setting_description.GetComponent<Text>();
            wcs.description.alignment = TextAnchor.UpperCenter;
            wcs.description.font = W_Content_Helper.font_STKaiti;
            wcs.description.fontSize = 12;
            wcs.description.transform.localScale = new Vector3(1, 1);

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
            this.description.transform.localPosition = new Vector3(130, -78f);
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
                    Window_Cultisys_Stats_Setting.instance.set_cultisys(cur_cultisys);
                    ScrollWindow.showWindow("cultisys_stats_setting");
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
            set_localized("CW_cultisys_" + cur_cultisys, new_name);
        }
        internal static void set_localized(string key, string text)
        {
            NCMS.Utils.Localization.Set(key, text);
            wcs.changed_name[key] = text;
        }
        internal void load_from_file()
        {
            if (!System.IO.File.Exists(path_to_save)) return;
            wcs.changed_name = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(System.IO.File.ReadAllText(path_to_save));
            if (wcs.changed_name == null)
            {
                wcs.changed_name = new Dictionary<string, string>();
                return;
            }
            foreach (string key in wcs.changed_name.Keys)
            {
                NCMS.Utils.Localization.Set(key, wcs.changed_name[key]);
            }
        }
        internal static void save_to_file()
        {
            if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/cw"))
            {
                System.IO.Directory.CreateDirectory(Application.streamingAssetsPath+"/cw");
            }
            System.IO.File.WriteAllText(path_to_save, Newtonsoft.Json.JsonConvert.SerializeObject(wcs.changed_name));
        }
    }
    internal class Window_Cultisys_Stats_Setting : MonoBehaviour
    {
        public static Window_Cultisys_Stats_Setting instance;
        private Text cultisys_name;
        private CW_Asset_CultiSys cultisys_to_change;
        private Cultisys_Level_Element[] cultisys_levels;
        private GameObject prefab_cultisys_level_elm;
        private Transform content_transform;
        public bool initialized = false;
        private bool first_open = true;
        private bool enable = false;
        private string[] stats_can_modify = new string[] { "power_level","age_bonus", "soul", "soul_regen", "spell_armor", "shield", "shield_regen", "wakan", "wakan_regen", "health_regen", "armor", "health", "damage", "knockbackReduction" };
        private Image[] stats_is_on;
        private const int base_stats_split_idx = 10;
        private static string path_to_save = Application.streamingAssetsPath + "/cw/cw_cultisys_stats.json";
        private static List<CW_Asset_CultiSys> cultisys_changed = new List<CW_Asset_CultiSys>();
        private int cur_stats_idx = 0;
        internal static void init()
        {
            ScrollWindow scroll_window = GameObject.Instantiate(Resources.Load<ScrollWindow>("windows/empty"), CanvasMain.instance.transformWindows);
            scroll_window.titleText.GetComponent<LocalizedText>().key = "cultisys_stats_setting_title";
            scroll_window.screen_id = "cultisys_stats_setting";
            scroll_window.name = "cultisys_stats_setting";
            scroll_window.CallMethod("create", false);
            NCMS.Utils.Windows.AllWindows[scroll_window.name] = scroll_window;

            instance = scroll_window.gameObject.AddComponent<Window_Cultisys_Stats_Setting>();
            //instance.load_from_file();
            instance.transform.Find("Background/Scroll View").gameObject.SetActive(true);
            instance.content_transform = instance.transform.Find("Background/Scroll View/Viewport/Content");
            instance.gameObject.SetActive(false);

            GameObject cultisys_name_setting = new GameObject("name_setting",typeof(Image));
            cultisys_name_setting.transform.SetParent(instance.content_transform);
            cultisys_name_setting.transform.localScale = new Vector3(1.2f, 1.2f);
            cultisys_name_setting.transform.localPosition = new Vector3(130, -10.51f);
            cultisys_name_setting.GetComponent<Image>().enabled = false;
            cultisys_name_setting.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);

            GameObject cultisys_name_input_field = new GameObject("cultisys_name", typeof(Text));
            cultisys_name_input_field.transform.SetParent(cultisys_name_setting.transform);
            cultisys_name_input_field.transform.localPosition = new Vector3(0, 0);
            cultisys_name_input_field.transform.localScale = new Vector3(0.8f, 0.8f);
            cultisys_name_input_field.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);

            Text cultisys_name_text_component = cultisys_name_input_field.GetComponent<Text>();
            cultisys_name_text_component.font = W_Content_Helper.font_STLiti;
            cultisys_name_text_component.text = "体系名";
            cultisys_name_text_component.alignment = TextAnchor.UpperCenter;
            instance.cultisys_name = cultisys_name_text_component;

            GameObject stats_grid_object = new GameObject("stats_grid", typeof(Image));
            stats_grid_object.transform.SetParent(instance.content_transform);
            stats_grid_object.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/windowInnerSliced");
            stats_grid_object.transform.localScale = new Vector3(1, 1);
            stats_grid_object.transform.localPosition = new Vector3(130, -80);
            stats_grid_object.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 100);


            GameObject stats_grid = new GameObject("grid_group", typeof(GridLayoutGroup));
            stats_grid.transform.SetParent(stats_grid_object.transform);
            stats_grid.transform.localPosition = new Vector3(-62.12f, 27.51f);
            GridLayoutGroup grid_group = stats_grid.GetComponent<GridLayoutGroup>();
            grid_group.cellSize = new Vector2(150, 50);
            grid_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid_group.constraintCount = 3;
            grid_group.spacing = new Vector2(15, 5);

            GameObject stats_prefab = new GameObject("stats_prefab", typeof(Image));
            #region stats_prefab setting
            stats_prefab.transform.SetParent(stats_grid.transform);
            stats_prefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/windowInnerSliced");

            GameObject stats_name = new GameObject("name", typeof(Text));
            stats_name.transform.SetParent(stats_prefab.transform);
            stats_name.transform.localPosition = new Vector3(-20, -40);
            stats_name.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 100);
            Text stats_name_text = stats_name.GetComponent<Text>();
            stats_name_text.text = "属性名";
            stats_name_text.font = W_Content_Helper.font_STKaiti;
            stats_name_text.resizeTextForBestFit = true;
            stats_name_text.resizeTextMaxSize = 20;
            stats_name_text.resizeTextMinSize = 10;

            GameObject stats_button = new GameObject("button", typeof(Button), typeof(Image));
            stats_button.transform.SetParent(stats_prefab.transform);
            stats_button.transform.localPosition = new Vector3(50, 0);
            stats_button.transform.localScale = new Vector3(0.3f, 0.3f);
            stats_button.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/button");
            GameObject stats_on_image = new GameObject("is_on", typeof(Image));
            stats_on_image.transform.SetParent(stats_button.transform);
            stats_on_image.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconOn");
            stats_on_image.GetComponent<Image>().enabled = false;
            stats_on_image.transform.localPosition = new Vector3(0, 0);
            stats_on_image.transform.localScale = new Vector3(1, 1);
            #endregion
            stats_prefab.SetActive(false);
            instance.stats_is_on = new Image[instance.stats_can_modify.Length];
            for(int i = 0; i < instance.stats_can_modify.Length; i++)
            {
                GameObject __stats = Instantiate(stats_prefab, stats_grid.transform);
                __stats.transform.Find("name").GetComponent<Text>().text = LocalizedTextManager.getText(instance.stats_can_modify[i]);
                Button __button = __stats.transform.Find("button").GetComponent<Button>();
                int __index = i;
                instance.stats_is_on[i] = __button.transform.Find("is_on").GetComponent<Image>();
                __button.onClick.AddListener(new UnityEngine.Events.UnityAction(delegate
                {
                    instance.switch_to_stats(__index);
                }));
                instance.stats_is_on[i].transform.localPosition = new Vector3(0, 0);
                __stats.SetActive(true);
            }

            GameObject setting_description = new GameObject("Description", typeof(Text));
            setting_description.transform.SetParent(instance.content_transform);
            setting_description.GetComponent<RectTransform>().sizeDelta = new Vector2(162, 100);
            setting_description.GetComponent<Text>().text = "调整始终有效\n如需恢复请删除\nworldbox_Data/StreamingAssets/cw/cw_cultisys_stats.json";
            Text text_setting_description = setting_description.GetComponent<Text>();
            text_setting_description.alignment = TextAnchor.UpperCenter;
            text_setting_description.font = W_Content_Helper.font_STKaiti;
            text_setting_description.fontSize = 12;
            text_setting_description.transform.localScale = new Vector3(1, 1);

            instance.prefab_cultisys_level_elm = new GameObject("level_setting_prefab", typeof(Image), typeof(Cultisys_Level_Element));
            GameObject level_setting_prefab = instance.prefab_cultisys_level_elm;
            level_setting_prefab.SetActive(false);
            #region 预制体初始化
            Cultisys_Level_Element __cle = level_setting_prefab.GetComponent<Cultisys_Level_Element>();

            level_setting_prefab.transform.SetParent(instance.content_transform);
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
            level_name_setting.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 15);
            level_name_setting.transform.localScale = new Vector3(1, 1);
            level_name_setting.transform.localPosition = new Vector3(30, 0);

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
            level_name_text_component.text = "数值";
            level_name_text_component.alignment = TextAnchor.UpperCenter;
            level_name_input_field_component.textComponent = level_name_text_component;
            __cle.input_field = level_name_input_field_component;

            //level_setting_prefab.SetActive(false);
            #endregion
            instance.initialized = true;
        }
        private void postfix_init()
        {
            set_cultisys("immortal");
            int length = cultisys_to_change.bonus_stats.Length;
            int i;
            cultisys_levels = new Cultisys_Level_Element[Others.CW_Constants.max_cultisys_level];
            for (i = 0; i < length; i++)
            {
                Cultisys_Level_Element level_setting = Instantiate(this.prefab_cultisys_level_elm, this.content_transform).GetComponent<Cultisys_Level_Element>();
                cultisys_levels[i] = level_setting;
                level_setting.init();
                level_setting.cultisys_id = cultisys_to_change.id;
                level_setting.level.text = i + "境";
                level_setting.name = level_setting.level.text;
                //level_setting.level_name.text = LocalizedTextManager.getText("cultisys_" + cur_cultisys + "_" + i);
                int cur_level = i;
                level_setting.input_field.onEndEdit = new InputField.SubmitEvent();
                level_setting.input_field.onEndEdit.AddListener(new UnityEngine.Events.UnityAction<string>((text) =>
                {
                    this.set_val(text, cur_level);
                }));
                level_setting.gameObject.SetActive(true);

            }
            List<CW_Asset_CultiSys> cultisyses = CW_Library_Manager.instance.cultisys.list;
            for (i = 0; i < cultisyses.Count; i++)
            {

                GameObject cultisys_switch = new GameObject("switch_to_" + cultisyses[i].id);
                GameObject button = new GameObject("Button", typeof(Image), typeof(Button), typeof(TipButton));
                cultisys_switch.transform.SetParent(this.transform.Find("Background"));
                button.transform.SetParent(cultisys_switch.transform);

                cultisys_switch.transform.localPosition = new Vector3(150, 100 - i * 40);
                button.transform.localScale = new Vector3(1, 1);

                button.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/" + cultisyses[i].sprite_name);

                string cultisys_id = cultisyses[i].id;
                button.GetComponent<TipButton>().textOnClick = LocalizedTextManager.getText("cultisys_name_setting").Replace("$cultisys_name$", LocalizedTextManager.getText("CW_cultisys_" + cultisys_id));

                Button button_button = button.GetComponent<Button>();
                button_button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                {
                    this.set_cultisys(cultisys_id);
                }));
            }
            resize();
        }
        private void switch_to_stats(int idx)
        {
            stats_is_on[cur_stats_idx].enabled = false;
            cur_stats_idx = idx;
            stats_is_on[cur_stats_idx].enabled = true;
            if(enable) show_stats();
        }
        internal void load_from_file()
        {
            if (!System.IO.File.Exists(path_to_save)) return;

            try
            {
                cultisys_changed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CW_Asset_CultiSys>>(System.IO.File.ReadAllText(path_to_save));
            }
            catch(Exception e)
            {
                cultisys_changed = new List<CW_Asset_CultiSys>();
            }

            if (cultisys_changed == null) cultisys_changed=new List<CW_Asset_CultiSys>();

            foreach(CW_Asset_CultiSys cultisys in cultisys_changed)
            {
                if (!CW_Library_Manager.instance.cultisys.dict.ContainsKey(cultisys.id)) continue;
                CW_Asset_CultiSys cultisys_to_be_overlayed = CW_Library_Manager.instance.cultisys.get(cultisys.id);
                cultisys_to_be_overlayed.power_level = cultisys.power_level;
                cultisys_to_be_overlayed.bonus_stats = cultisys.bonus_stats;
            }
        }
        internal static void save_to_file()
        {
            if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/cw"))
            {
                System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + "/cw");
            }
            System.IO.File.WriteAllText(path_to_save, Newtonsoft.Json.JsonConvert.SerializeObject(cultisys_changed));
        }
        private void resize()
        {
            RectTransform rect = this.content_transform.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 180 + (this.cultisys_levels == null ? 0 : this.cultisys_levels.Length * 30));
            float cur_y = 15.5f - rect.sizeDelta.y;
            for(int i = 19; i >= 0; i--)
            {
                cultisys_levels[i].transform.localPosition = new Vector3(129.4f, cur_y);
                cur_y += 30f;
            }
            cur_y += 70;
            this.content_transform.Find("stats_grid").localPosition = new Vector3(130, cur_y);
            cur_y += 80;
            this.content_transform.Find("name_setting").localPosition = new Vector3(130, cur_y);
        }
        public void set_cultisys(string cultisys)
        {
            cultisys_to_change = CW_Library_Manager.instance.cultisys.get(cultisys);
            switch_to_stats(cur_stats_idx);
            // 设置体系名字
            cultisys_name.text = LocalizedTextManager.getText("CW_cultisys_" + cultisys_to_change.id);
        }
        void OnEnable()
        {
            if (!initialized) return;
            if (first_open)
            {
                first_open = false;
                postfix_init();
            }
            enable = true;

            // 展示各个境界的数值
            show_stats();
        }
        void OnDisable()
        {
            enable = false;
            apply_changes_to_units_immediately();
        }

        private void apply_changes_to_units_immediately()
        {
            List<Actor> units = MapBox.instance.units.getSimpleList();
            foreach(Actor unit in units)
            {
                unit.setStatsDirty();
            }
        }

        private void set_val(string val_str, int level)
        {
            if (string.IsNullOrEmpty(val_str)) return;
            if (!cultisys_changed.Contains(cultisys_to_change)) cultisys_changed.Add(cultisys_to_change);
            if (cur_stats_idx == 0)
            {
                cultisys_to_change.power_level[level] = Convert.ToSingle(val_str);
            }
            else if (cur_stats_idx < base_stats_split_idx)
            {
                if(typeof(CW_BaseStats).GetField(stats_can_modify[cur_stats_idx]).FieldType == typeof(int))
                {
                    Reflection.SetField(cultisys_to_change.bonus_stats[level], stats_can_modify[cur_stats_idx],Convert.ToInt32(val_str));
                }
                else
                {
                    Reflection.SetField(cultisys_to_change.bonus_stats[level], stats_can_modify[cur_stats_idx], Convert.ToSingle(val_str));
                }
            }
            else
            {
                if (typeof(BaseStats).GetField(stats_can_modify[cur_stats_idx]).FieldType == typeof(int))
                {
                    Reflection.SetField(cultisys_to_change.bonus_stats[level].base_stats, stats_can_modify[cur_stats_idx], Convert.ToInt32(val_str));
                }
                else
                {
                    Reflection.SetField(cultisys_to_change.bonus_stats[level].base_stats, stats_can_modify[cur_stats_idx], Convert.ToSingle(val_str));
                }
            }
        }
        private void show_stats()
        {
            if (cur_stats_idx == 0)
            {
                for (int i = 0; i < cultisys_levels.Length; i++)
                {
                    cultisys_levels[i].input_field.text = cultisys_to_change.power_level[i].ToString();
                }
            }
            else if (cur_stats_idx < base_stats_split_idx)
            {
                for (int i = 0; i < cultisys_levels.Length; i++)
                {
                    cultisys_levels[i].input_field.text = ReflectionUtility.Reflection.GetField(typeof(CW_BaseStats), cultisys_to_change.bonus_stats[i], stats_can_modify[cur_stats_idx]).ToString();
                }
            }
            else
            {
                for (int i = 0; i < cultisys_levels.Length; i++)
                {
                    cultisys_levels[i].input_field.text = ReflectionUtility.Reflection.GetField(typeof(BaseStats), cultisys_to_change.bonus_stats[i].base_stats, stats_can_modify[cur_stats_idx]).ToString();
                }
            }
        }
    }
}
