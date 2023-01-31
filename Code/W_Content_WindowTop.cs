using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Cultivation_Way.Content
{
    internal abstract class CW_Sim_Info_Elm : MonoBehaviour
    {
        public GameObject bg;
        public GameObject disp;
        public GameObject info;
        public Text object_name;
    }
    internal class CW_Sim_Creature_Info_Elm : CW_Sim_Info_Elm
    {
        public StatBar health_bar;
        public StatBar wakan_bar;
        public StatBar shield_bar;
        public GameObject damage;
        public GameObject level;
        public GameObject kills;
        public UnitAvatarLoader avatar_loader;
        public Text text_damage;
        public Text text_level;
        public Text text_kills;
        public CW_Actor actor;
        public Button locate_button;
        public Button inspect_button;
        public void show(CW_Actor actor)
        {
            this.actor = actor;
            this.object_name.text = actor.coloredName;
            this.health_bar.setBar(actor.base_data.health, actor.cw_cur_stats.base_stats.health, "",false);
            this.wakan_bar.setBar(actor.cw_status.wakan, actor.cw_cur_stats.wakan, "",false);
            this.shield_bar.setBar(actor.cw_status.shield, actor.cw_cur_stats.shield, "",false);
            this.avatar_loader.load(actor);
            this.text_damage.text = Toolbox.formatNumber(actor.cw_cur_stats.base_stats.damage);
            this.text_level.text = actor.fast_data.level.ToString();
            this.text_kills.text = Toolbox.formatNumber(actor.fast_data.kills);

            Button inspect_button = transform.Find("inspect/Open").GetComponent<Button>();
            inspect_button.onClick.RemoveAllListeners();
            inspect_button.onClick = new Button.ButtonClickedEvent();
            inspect_button.onClick.AddListener(inspect);

            Button locate_button = transform.Find("locate/Locate").GetComponent<Button>();
            locate_button.onClick.RemoveAllListeners();
            locate_button.onClick = new Button.ButtonClickedEvent();
            locate_button.onClick.AddListener(locate);
        }
        public void inspect() 
        {
            Config.selectedUnit = this.actor;
            ScrollWindow.moveAllToLeftAndRemove(true);
            ScrollWindow.showWindow("inspect_unit");
        }
        public void locate()
        {
            WorldLog.locationFollow(this.actor);
        }
    }
    internal class CW_Sim_City_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_Kingdom_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_Clan_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_Pope_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_Item_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_S_B_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal class CW_Sim_CultiBook_Info_Elm : CW_Sim_Info_Elm
    {

    }
    internal enum Top_Type
    {
        NONE,
        CREATURE,
        CULTIBOOK,
        S_B,
        ITEM,
        CLAN,
        CITY,
        KINGDOM,
        POPE
    }
    internal class Sort_Setting
    {
        internal int top_k = 10;

    }
    internal class W_Content_WindowTop : MonoBehaviour
    {
        internal static bool initialized = false;
        internal Top_Type cur_top_type = Top_Type.NONE;
        internal Sort_Setting sort_setting = new Sort_Setting();
        internal List<CW_Sim_Info_Elm> elements = new List<CW_Sim_Info_Elm>();
        internal GameObject no_item;
        internal GameObject prefab_top_type;
        internal GameObject prefab_top_switch_button;
        internal GameObject prefab_city;
        internal GameObject prefab_clan;
        internal GameObject prefab_unit;
        internal GameObject prefab_cultibook;
        internal GameObject prefab_item;
        internal GameObject prefab_kingdom;
        internal GameObject prefab_pope;
        internal GameObject prefab_S_B;
        internal GameObject creature_top_entry;
        internal GameObject cultibook_top_entry;
        internal GameObject S_B_top_entry;
        internal GameObject item_top_entry;
        internal GameObject clan_top_entry;
        internal GameObject city_top_entry;
        internal GameObject kingdom_top_entry;
        internal GameObject pope_top_entry;
        internal Transform content_transform;
        internal static GameObject window_top_gameobject;
        internal static GameObject origin_favorites_gameobject;
        internal static GameObject create_window_gameobject()
        {
            #region 基础初始化
            initialized = true;
            // 获取最爱列表窗口
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "favorites");
            origin_favorites_gameobject = NCMS.Utils.Windows.GetWindow("favorites").gameObject;

            origin_favorites_gameobject.SetActive(false);
            window_top_gameobject = Instantiate(Resources.Load<ScrollWindow>("windows/empty"), CanvasMain.instance.transformWindows).gameObject;

            window_top_gameobject.name = "cw_window_top";
            NCMS.Utils.Windows.AllWindows.Add(window_top_gameobject.name, window_top_gameobject.GetComponent<ScrollWindow>());

            window_top_gameobject.GetComponent<ScrollWindow>().CallMethod("create", true);
            window_top_gameobject.transform.Find("Background/Scroll View").gameObject.SetActive(true);

            Transform content_transform = window_top_gameobject.transform.Find("Background/Scroll View/Viewport/Content").transform;
            
            // 去除无用元素
            //window_top_gameobject.transform.Find("HoveringIconBgManager").gameObject.SetActive(false);
            //Destroy(content_transform.Find("No Items").gameObject);
            //Destroy(window_top_gameobject.GetComponent<WindowFavorites>());
            // 设置标题
            GameObject title_object = window_top_gameobject.transform.Find("Background/Title").gameObject;
            title_object.GetComponent<LocalizedText>().autoField = false;
            Text title = title_object.GetComponent<Text>();
            title.text = LocalizedTextManager.getText("CW_Top_title");
            title.font = W_Content_Helper.font_STLiti;
            title.fontSize = 15;
            title.resizeTextMaxSize = 15;
            // 添加界面控制器
            W_Content_WindowTop cw_wt = window_top_gameobject.AddComponent<W_Content_WindowTop>();
            cw_wt.content_transform = content_transform;
            #endregion
            #region 添加八个榜单的进入按钮
            Transform background_transform = window_top_gameobject.transform.Find("Background");
            GameObject button_prefab = new GameObject("button_prefab");
            button_prefab.SetActive(false);
            button_prefab.AddComponent<RectTransform>();
            button_prefab.transform.localScale = new Vector3(0.45f, 0.45f);
            GameObject button_bg = new GameObject("BG");
            button_bg.transform.SetParent(button_prefab.transform);
            Image bg = button_bg.AddComponent<Image>();
            bg.transform.localScale = background_transform.Find("CloseBackgound").GetComponent<Image>().transform.localScale;
            bg.sprite = background_transform.Find("CloseBackgound").GetComponent<Image>().sprite;

            GameObject button_icon = new GameObject("icon");
            button_icon.transform.SetParent(button_prefab.transform);
            button_icon.transform.localScale = new Vector3(0.5f, 0.5f, 0);

            CW_TipButton tmp_tip_button = button_prefab.AddComponent<CW_TipButton>();
            tmp_tip_button.button = tmp_tip_button.gameObject.AddComponent<Button>();
            tmp_tip_button.image = button_icon.gameObject.AddComponent<Image>();
            

            button_bg.transform.eulerAngles = new Vector3(0, 0, 270);

            // 生物排序
            GameObject top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.creature_top_entry = top_entry;
            top_entry.name = "Creature_Background";
            top_entry.transform.localPosition = new Vector3(119, 86.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_Creature_Top", null, "iconEastern_Humans", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_creature);
            top_entry.SetActive(true);
            // 功法排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.cultibook_top_entry = top_entry;
            top_entry.name = "CultiBook_Background";
            top_entry.transform.localPosition = new Vector3(119, 30.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_CultiBook_Top", null, "iconCultiBook_immortal", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_cultibook);
            top_entry.SetActive(true);
            // 体质排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.S_B_top_entry = top_entry;
            top_entry.name = "S_B_Background";
            top_entry.transform.localPosition = new Vector3(119, -50.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_S_B_Top", null, "iconCultiBook_immortal", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_S_B);
            top_entry.SetActive(true);
            // 物品排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.item_top_entry = top_entry;
            top_entry.name = "Item_Background";
            top_entry.transform.localPosition = new Vector3(116, -100.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_Item_Top", null, "iconCultiBook_immortal", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_item);
            top_entry.SetActive(true);

            button_bg.transform.eulerAngles = new Vector3(0, 0, 90);
            // 宗门排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.pope_top_entry = top_entry;
            top_entry.name = "Pope_Background";
            top_entry.transform.localPosition = new Vector3(-119, 86.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_Pope_Top", null, "iconCultiBook_immortal", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_pope);
            top_entry.SetActive(true);
            // 城市排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.city_top_entry = top_entry;
            top_entry.name = "City_Background";
            top_entry.transform.localPosition = new Vector3(-119, 30.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_City_Top", null, "iconCityList", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_city);
            top_entry.SetActive(true);
            // 国家排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.kingdom_top_entry = top_entry;
            top_entry.name = "Kingdom_Background";
            top_entry.transform.localPosition = new Vector3(-119, -50.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_Kingdom_Top", null, "iconKingdomList", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_kingdom);
            top_entry.SetActive(true);
            // 家族排序
            top_entry = Instantiate(button_prefab, background_transform);
            cw_wt.clan_top_entry = top_entry;
            top_entry.name = "Clan_Background";
            top_entry.transform.localPosition = new Vector3(-116, -100.5f);
            top_entry.GetComponent<CW_TipButton>().load("CW_Clan_Top", null, "iconClan", "normal");
            top_entry.GetComponent<CW_TipButton>().button.onClick.AddListener(cw_wt.switch_to_clan);
            top_entry.SetActive(true);
            #endregion
            // 设置默认显示内容
            cw_wt.no_item = Instantiate(origin_favorites_gameobject.transform.Find("Background/Scroll View/Viewport/Content/No Items").gameObject, content_transform);
            Sprite no_item_sprite = Resources.Load<Sprite>("ui/Icons/iconTop");
            cw_wt.no_item.transform.Find("BG").GetComponent<Image>().sprite = no_item_sprite;
            cw_wt.no_item.transform.Find("InnerBG/Left").GetComponent<Image>().sprite = no_item_sprite;
            cw_wt.no_item.transform.Find("InnerBG/Right").GetComponent<Image>().sprite = no_item_sprite;
            cw_wt.no_item.transform.Find("InnerBG/Description").GetComponent<LocalizedText>().key = "CW_Top_No_Items_Description";
            #region 创造预制体
            GameObject prefab_top_switch = new GameObject("prefab_top_switch");
            #region 榜单切换按钮
            cw_wt.prefab_top_switch_button = prefab_top_switch;
            prefab_top_switch.AddComponent<Image>();
            prefab_top_switch.AddComponent<Button>();
            #endregion
            GameObject prefab_top_type = new GameObject("prefab_top_type");
            #region 榜单挂件
            cw_wt.prefab_top_type = prefab_top_type;
            #endregion
            GameObject prefab_sim_info_elm = new GameObject("prefab_sim_info_elm");
            GameObject prefab_bar = null;
            GameObject prefab_stat = null;
            #region 基础预制体
            WindowElementFavoriteUnit __origin_creature_info = Instantiate(origin_favorites_gameobject.GetComponent<WindowFavorites>().elementPrefab);
            RectTransform rect = prefab_sim_info_elm.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.sizeDelta = new Vector2(192, 48);

            GameObject __sim_info_elm_bg = new GameObject("bg", typeof(Image));
            __sim_info_elm_bg.transform.SetParent(prefab_sim_info_elm.transform);
            GameObject __sim_info_elm_title = Instantiate(__origin_creature_info.transform.Find("Text").gameObject, __sim_info_elm_bg.transform);
            __sim_info_elm_title.name = "title";
            __sim_info_elm_title.GetComponent<Text>().fontSize = 5;
            __sim_info_elm_title.GetComponent<Text>().resizeTextMaxSize = 8;
            __sim_info_elm_title.GetComponent<Text>().text = "TITLE TEXT";
            __sim_info_elm_title.transform.localScale = new Vector3(0.5f, 2.4f);
            __sim_info_elm_title.transform.localPosition = new Vector3(8, 30);
            //GameObject __sim_info_elm_disp = new GameObject("disp");
            //__sim_info_elm_disp.transform.SetParent(prefab_sim_info_elm.transform);
            GameObject __sim_info_elm_info = new GameObject("info");
            __sim_info_elm_info.transform.SetParent(prefab_sim_info_elm.transform);
            __sim_info_elm_bg.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/sim_info_bg");

            prefab_sim_info_elm.transform.SetParent(content_transform);
            prefab_sim_info_elm.transform.localScale = new Vector3(1, 1);
            __sim_info_elm_bg.transform.localScale = new Vector3(2.1f, 0.5f);
            __sim_info_elm_bg.transform.localPosition = new Vector3(-14, 0);
            // 起始于(137.4f, -25.95f) 单项高度55
            prefab_sim_info_elm.transform.localPosition = new Vector3(137.4f, -30.95f);
#if false
            prefab_sim_info_elm.SetActive(true);
#else
            prefab_sim_info_elm.SetActive(false);
#endif

            prefab_bar = Instantiate(__origin_creature_info.health_bar).gameObject;
            prefab_stat = Instantiate(__origin_creature_info.transform.Find("Icons/Damage").gameObject);
            prefab_stat.transform.GetComponent<Image>().color = new Color(0.83f, 0.83f, 0.83f, 1f);
            #endregion
            cw_wt.prefab_unit = Instantiate(prefab_sim_info_elm);
            #region 生物信息
            cw_wt.prefab_unit.name = "prefab_unit_info_elm";
            CW_Sim_Creature_Info_Elm __sim_unit_info_elm = cw_wt.prefab_unit.AddComponent<CW_Sim_Creature_Info_Elm>();
#if true
            __sim_unit_info_elm.transform.SetParent(content_transform);
#endif
            cw_wt.prefab_unit.SetActive(false);
            cw_wt.prefab_unit.transform.localScale = prefab_sim_info_elm.transform.localScale;
            cw_wt.prefab_unit.transform.localPosition = prefab_sim_info_elm.transform.localPosition;
            cw_wt.prefab_unit.transform.Find("bg").localPosition = __sim_info_elm_bg.transform.localPosition;
            
            __sim_unit_info_elm.transform.localScale = new Vector3(1, 1);
            __sim_unit_info_elm.bg = cw_wt.prefab_unit.transform.Find("bg").gameObject;
            //__sim_unit_info_elm.disp = cw_wt.prefab_unit.transform.Find("disp").gameObject;
            __sim_unit_info_elm.info = cw_wt.prefab_unit.transform.Find("info").gameObject;
            __sim_unit_info_elm.object_name = cw_wt.prefab_unit.transform.Find("bg/title").GetComponent<Text>();
            GridLayoutGroup __unit_info_layout_group = __sim_unit_info_elm.info.AddComponent<GridLayoutGroup>();
            __unit_info_layout_group.cellSize = new Vector2(29, 12);
            __unit_info_layout_group.startAxis = GridLayoutGroup.Axis.Horizontal;
            __unit_info_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            __unit_info_layout_group.constraintCount = 3;
            __sim_unit_info_elm.info.transform.localPosition = new Vector3(50, -44);

            __sim_unit_info_elm.health_bar = Instantiate(prefab_bar, __sim_unit_info_elm.info.transform).GetComponent<StatBar>();
            __sim_unit_info_elm.health_bar.gameObject.name = "Health Bar";
            __sim_unit_info_elm.health_bar.transform.localPosition = new Vector3(2-50, -6+44);
            __sim_unit_info_elm.health_bar.GetComponent<TipButton>().textOnClick = "health";

            __sim_unit_info_elm.wakan_bar = Instantiate(prefab_bar, __sim_unit_info_elm.info.transform).GetComponent<StatBar>();
            __sim_unit_info_elm.wakan_bar.gameObject.name = "Wakan Bar";
            __sim_unit_info_elm.wakan_bar.transform.Find("Mask/Bar").GetComponent<Image>().color = new Color(0.38f, 0.71f, 1, 0.75f);
            __sim_unit_info_elm.wakan_bar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
            __sim_unit_info_elm.wakan_bar.GetComponent<TipButton>().textOnClick = "wakan";
            __sim_unit_info_elm.wakan_bar.transform.localPosition = new Vector3(76-50, -21+44);
            __sim_unit_info_elm.wakan_bar.transform.Find("Icon").localScale = new Vector3(0.35f, 0.35f);

            __sim_unit_info_elm.shield_bar = Instantiate(prefab_bar, __sim_unit_info_elm.info.transform).GetComponent<StatBar>();
            __sim_unit_info_elm.shield_bar.gameObject.name = "Shield Bar";
            __sim_unit_info_elm.shield_bar.transform.Find("Mask/Bar").GetComponent<Image>().color = new Color(1, 1, 1, 0.79f);
            __sim_unit_info_elm.shield_bar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconShield");
            __sim_unit_info_elm.shield_bar.GetComponent<TipButton>().textOnClick = "shield";
            __sim_unit_info_elm.shield_bar.transform.localPosition = new Vector3(2-50, -21+44);

            __sim_unit_info_elm.damage = Instantiate(prefab_stat, __sim_unit_info_elm.info.transform).gameObject;
            __sim_unit_info_elm.damage.gameObject.name = "Damage";
            __sim_unit_info_elm.damage.transform.localPosition = new Vector3(12, 0);
            __sim_unit_info_elm.text_damage = __sim_unit_info_elm.damage.transform.Find("GameObject/Text").GetComponent<Text>();

            __sim_unit_info_elm.level = Instantiate(prefab_stat, __sim_unit_info_elm.info.transform).gameObject;
            __sim_unit_info_elm.level.gameObject.name = "Level";
            __sim_unit_info_elm.level.transform.localPosition = new Vector3(42, 0);
            __sim_unit_info_elm.level.transform.Find("GameObject/Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconLevels");
            __sim_unit_info_elm.text_level = __sim_unit_info_elm.level.transform.Find("GameObject/Text").GetComponent<Text>();

            __sim_unit_info_elm.kills = Instantiate(prefab_stat, __sim_unit_info_elm.info.transform).gameObject;
            __sim_unit_info_elm.kills.gameObject.name = "Kills";
            __sim_unit_info_elm.kills.transform.localPosition = new Vector3(72, 0);
            __sim_unit_info_elm.kills.transform.Find("GameObject/Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconSkulls");
            __sim_unit_info_elm.text_kills = __sim_unit_info_elm.kills.transform.Find("GameObject/Text").GetComponent<Text>();

            __sim_unit_info_elm.disp = Instantiate(__origin_creature_info.transform.Find("BackgroundAvatar").gameObject, __sim_unit_info_elm.transform);
            __sim_unit_info_elm.disp.gameObject.name = "disp";
            __sim_unit_info_elm.disp.transform.localPosition = new Vector3(-85.24f, 13.5f);
            __sim_unit_info_elm.avatar_loader = __sim_unit_info_elm.disp.transform.Find("Mask/AvatarLoader").GetComponent<UnitAvatarLoader>();

            GameObject inspect = Instantiate(__origin_creature_info.transform.Find("OpenContainer").gameObject, cw_wt.prefab_unit.transform);
            GameObject locate = Instantiate(__origin_creature_info.transform.Find("LocateContainer").gameObject, cw_wt.prefab_unit.transform);
            inspect.name = "inspect"; locate.name = "locate";
            inspect.transform.localPosition = new Vector3(30, 6.96f);
            locate.transform.localPosition = new Vector3(70.46f, 6.96f);
            
            
#if false
            cw_wt.prefab_unit.SetActive(true);
#else
            cw_wt.prefab_unit.SetActive(false);
#endif
            #endregion

            #endregion

            #region 生物榜不同排序
            GameObject creature_top_type = Instantiate(prefab_top_type, background_transform);
            creature_top_type.name = "creature_top_type";
            creature_top_type.transform.localPosition = new Vector3(180f,0);
            GameObject kill_top = Instantiate(prefab_top_switch, creature_top_type.transform);
            kill_top.name = "kill_top";
            kill_top.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconSkulls");
            kill_top.GetComponent<Button>().onClick.AddListener(cw_wt.__sort_by_kills);
            kill_top.transform.localScale = new Vector3(0.3f, 0.3f);
            GameObject cutivelo_top = Instantiate(prefab_top_switch, creature_top_type.transform);
            cutivelo_top.name = "cultivelo_top";
            cutivelo_top.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconCultisys");
            cutivelo_top.GetComponent<Button>().onClick.AddListener(cw_wt.__sort_by_cultivelo);
            cutivelo_top.transform.localScale = new Vector3(0.24f, 0.3f);
            cutivelo_top.transform.localPosition = new Vector3(0, 100);

            #endregion
            WorldBoxConsole.Console.print("INIT WINDOW TOP");
            return window_top_gameobject;
        }
        void Start()
        {
            clear();
            // W_Content_WindowTop_Helper.clear_tmp_lists();
            if (no_item) no_item.SetActive(true);
        }
        void OnEnable()
        {
            
        }
        void OnDisable()
        {
            W_Content_WindowTop_Helper.clear_tmp_lists();
            clear();
        }
        private void clear()  
        {
            int i;
            while(elements.Count>0)
            {
                CW_Sim_Info_Elm __to_destroy = elements[elements.Count-1];
                elements.RemoveAt(elements.Count - 1);
                Destroy(__to_destroy.gameObject);
            }
            cur_top_type = Top_Type.NONE;
            if(no_item) no_item.SetActive(false);
        }
        private void scroll_resize()
        {
            this.content_transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60 * elements.Count + 40f);
        }
        private void update_window_type()
        {
            //throw new NotImplementedException();
        }
        private void switch_to_creature()
        {
            clear();
            cur_top_type = Top_Type.CREATURE;
            List<CW_Actor> list = W_Content_WindowTop_Helper.sort_creatures_by_level(sort_setting.top_k);
            foreach(CW_Actor actor in list) add_creature_info(actor);
            scroll_resize();
            update_window_type();
        }

        private void switch_to_cultibook()
        {
            clear();
            cur_top_type = Top_Type.CULTIBOOK;


            update_window_type();
        }
        private void switch_to_kingdom()
        {
            clear();
            cur_top_type = Top_Type.KINGDOM;


            update_window_type();
        }
        private void switch_to_city()
        {
            clear();
            cur_top_type = Top_Type.CITY;


            update_window_type();
        }
        private void switch_to_clan()
        {
            clear();
            cur_top_type = Top_Type.CLAN;


            update_window_type();
        }
        private void switch_to_S_B()
        {
            clear();
            cur_top_type = Top_Type.S_B;


            update_window_type();
        }
        private void switch_to_item()
        {
            clear();
            cur_top_type = Top_Type.ITEM;


            update_window_type();
        }
        private void switch_to_pope()
        {
            clear();
            cur_top_type = Top_Type.POPE;


            update_window_type();
        }
        private void __sort_by_health_level()
        {
            clear();
            cur_top_type = Top_Type.CREATURE;
            List<CW_Actor> list = W_Content_WindowTop_Helper.sort_creatures_by_health_level(sort_setting.top_k);
            foreach (CW_Actor actor in list) add_creature_info(actor);
            scroll_resize();
            update_window_type();
        }
        private void __sort_by_kills()
        {
            clear();
            cur_top_type = Top_Type.CREATURE;
            List<CW_Actor> list = W_Content_WindowTop_Helper.sort_creatures_by_kills(sort_setting.top_k);
            foreach (CW_Actor actor in list) add_creature_info(actor);
            scroll_resize();
            update_window_type();
        }
        private void __sort_by_cultivelo()
        {
            clear();
            cur_top_type = Top_Type.CREATURE;
            List<CW_Actor> list = W_Content_WindowTop_Helper.sort_creatures_by_cultivelo(sort_setting.top_k);
            foreach (CW_Actor actor in list) add_creature_info(actor);
            scroll_resize();
            update_window_type();
        }
        private void add_creature_info(CW_Actor actor)
        {
            CW_Sim_Creature_Info_Elm actor_info = Instantiate(prefab_unit, content_transform).GetComponent<CW_Sim_Creature_Info_Elm>();
            this.elements.Add(actor_info);
            actor_info.wakan_bar.transform.Find("Mask/Bar").GetComponent<Image>().color = Utils.CW_Utils_Others.get_wakan_color(actor.cw_status.wakan_level, actor.cw_cur_stats.wakan);
            actor_info.GetComponent<RectTransform>().anchoredPosition = new Vector3(7, 20 - 60f * elements.Count);
            actor_info.show(actor);
            actor_info.gameObject.SetActive(true);
        }


    }
}
