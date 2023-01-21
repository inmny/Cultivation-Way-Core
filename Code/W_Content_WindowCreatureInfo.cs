using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Cultivation_Way.Library;
using ReflectionUtility;
using DG.Tweening;

namespace Cultivation_Way.Content
{
    internal class CW_TipButton : MonoBehaviour
    {
        public Button button;
        public Image image;
        public string title;
        public string icon;
        public string type;
        public string description;
        public bool tooltip_enabled = true;
        private Vector3 origin_scale = new Vector3(-1,-1);
        private void Awake()
        {
            this.button = GetComponent<Button>();
            this.image = transform.Find("icon").GetComponent<Image>();
        }
        private void Start()
        {
            this.button.onClick.AddListener(show_tooltip);
            this.button.OnHover(show_tooltip);
            this.button.OnHoverOut(Tooltip.hideTooltip);
        }
        private void show_tooltip()
        {
            if (!tooltip_enabled) return;
            string title = this.title;
            string description = this.description;
            if (!LocalizedTextManager.stringExists(description)) description = null;
            Tooltip.instance.show(gameObject, type, title, description);
            if(this.origin_scale.x<0 && this.origin_scale.y < 0)
            {
                this.origin_scale = transform.localScale;
            }
            transform.localScale = new Vector3(transform.localScale.x*1.25f, transform.localScale.y*1.25f, 1f);
            transform.DOKill(true);
            transform.DOScale(origin_scale, 0.1f).SetEase(Ease.InBack);
        }
        public void load(string title, string description, string icon, string type = "cw_custom")
        {
            this.title = title;
            this.description = description;
            this.type = type;
            this.image.sprite = Resources.Load<Sprite>("ui/Icons/" + icon);
        }
    }
    internal class W_Content_WindowCreatureInfo : MonoBehaviour
    {
		public Image favoriteFoodSprite;
		public Image favoriteFoodBg;
		public Image moodSprite;
		public Image moodBG;
		public TraitButton prefabTrait;
		public EquipmentButton prefabEquipment;
        public CW_TipButton prefab_tip_button;
		public Transform traitsParent;
		public Transform equipmentParent;
		public StatBar health;
		public StatBar hunger;
		public StatBar shied;
		public StatBar wakan;
		public CityIcon damage;
		public CityIcon speed;
		public CityIcon armor;
		public CityIcon attackSpeed;
		public CityIcon crit;
		public CityIcon diplomacy;
		public CityIcon warfare;
		public CityIcon stewardship;
		public CityIcon intelligence;
		public NameInput nameInput;
		public Image icon;
		public UnitAvatarLoader avatarLoader;
		public Image iconFavorite;
		public Text text_description;
		public Text text_values;
		public GameObject buttonKingdom;
		public GameObject buttonCity;
		public GameObject backgroundCiv;
		public GameObject buttonPope;
		public GameObject buttonChildren;
		public GameObject buttonCultures;
		public GameObject buttonTraitEditor;
		private CW_Actor cw_actor;
		private List<ItemData> temp_equipment = new List<ItemData>();
        internal static bool initialized = false;
        internal static GameObject origin_inspect_unit_gameobject;
		internal static GameObject create_window_gameobject()
        {
            initialized = true;
            // 获取原版窗口
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            origin_inspect_unit_gameobject = NCMS.Utils.Windows.GetWindow("inspect_unit").gameObject;

            origin_inspect_unit_gameobject.SetActive(false);
			WindowCreatureInfo origin_wci = origin_inspect_unit_gameobject.GetComponent<WindowCreatureInfo>();
            // 初始化原始窗口
            //GameObject cw_inspect_unit_gameobject = UnityEngine.Object.Instantiate(origin_inspect_unit_gameobject);

            origin_inspect_unit_gameobject.name = "cw_inspect_unit";
			NCMS.Utils.Windows.AllWindows.Add(origin_inspect_unit_gameobject.name, origin_inspect_unit_gameobject.GetComponent<ScrollWindow>());

            //ScrollWindow cw_window = cw_inspect_unit_gameobject.GetComponent<ScrollWindow>();

            
			W_Content_WindowCreatureInfo cw_wci = origin_inspect_unit_gameobject.AddComponent<W_Content_WindowCreatureInfo>();
            WorldBoxConsole.Console.print(origin_inspect_unit_gameobject.GetComponent<W_Content_WindowCreatureInfo>() != null);
			// 拷贝数据
			cw_wci.armor = origin_wci.armor;
			cw_wci.attackSpeed = origin_wci.attackSpeed;
			cw_wci.avatarLoader = origin_wci.avatarLoader;
			cw_wci.backgroundCiv = origin_wci.backgroundCiv;
			cw_wci.buttonChildren = null;
			cw_wci.buttonCity = origin_wci.buttonCity;
			cw_wci.buttonCultures = origin_wci.buttonCultures;
			cw_wci.buttonKingdom = origin_wci.buttonKingdom;
			cw_wci.buttonPope = null;
			cw_wci.buttonTraitEditor = origin_wci.buttonTraitEditor;
			cw_wci.crit = origin_wci.crit;
			cw_wci.damage = origin_wci.damage;
			cw_wci.diplomacy = origin_wci.diplomacy;
			cw_wci.equipmentParent = origin_wci.equipmentParent;
            cw_wci.favoriteFoodBg = origin_wci.favoriteFoodBg;
            cw_wci.favoriteFoodSprite = origin_wci.favoriteFoodSprite;
			cw_wci.health = origin_wci.health;
			cw_wci.hunger = origin_wci.hunger;
			cw_wci.icon = origin_wci.icon;
			cw_wci.iconFavorite = origin_wci.iconFavorite;
			cw_wci.intelligence = origin_wci.intelligence;
			cw_wci.moodBG = origin_wci.moodBG;
			cw_wci.moodSprite = origin_wci.moodSprite;
			cw_wci.nameInput = origin_wci.nameInput;
			cw_wci.prefabEquipment = origin_wci.prefabEquipment;
            cw_wci.prefabTrait = origin_wci.prefabTrait;
            cw_wci.shied = null;
			cw_wci.speed = origin_wci.speed;
			cw_wci.stewardship = origin_wci.stewardship;
			cw_wci.temp_equipment = new List<ItemData>();
			cw_wci.text_description = origin_wci.text_description;
			cw_wci.text_values = origin_wci.text_values;
			cw_wci.traitsParent = origin_wci.traitsParent;
			cw_wci.wakan = null;
            cw_wci.warfare = origin_wci.warfare;
            Destroy(origin_wci);
            WorldBoxConsole.Console.print(origin_inspect_unit_gameobject.GetComponent<W_Content_WindowCreatureInfo>() != null);
            // 默认界面调整
            Transform content_transform = origin_inspect_unit_gameobject.transform.Find("Background/Scroll View/Viewport/Content");
            Transform stat_icons_transform = content_transform.Find("StatIcons");
            Transform inner_bg_transform = content_transform.Find("InnerBG");
            stat_icons_transform.localPosition = new Vector3(129.22f, -105.85f);
            inner_bg_transform.localPosition = new Vector3(129.22f, -175.05f);
            RectTransform inner_bg_rect_transform = inner_bg_transform.GetComponent<RectTransform>();
            inner_bg_rect_transform.anchorMin = new Vector2(1, 0.95f);
            inner_bg_rect_transform.offsetMin = new Vector2(-224.84f, -220.64f);
            inner_bg_rect_transform.offsetMax = new Vector2(95.49f, -120.46f);
            inner_bg_rect_transform.sizeDelta = new Vector2(318.87f, 100.18f);
            // 按键功能
            Transform button_kingdom = origin_inspect_unit_gameobject.transform.Find("Background/BackgroundLeftBottom/ButtonKingdom");
            Button tmp_button = button_kingdom.GetComponent<Button>();
            tmp_button.onClick.AddListener(cw_wci.clickKingdom);

            Transform button_village = origin_inspect_unit_gameobject.transform.Find("Background/BackgroundLeftBottom/ButtonVillage");
            tmp_button = button_village.GetComponent<Button>();
            tmp_button.onClick.AddListener(cw_wci.clickVillage);

            Transform button_make_favorite = origin_inspect_unit_gameobject.transform.Find("Background/BackgroundRightBottom/Make Favorite");
            tmp_button = button_make_favorite.GetComponent<Button>();
            tmp_button.onClick.AddListener(cw_wci.pressFavorite);

            Transform button_culture = origin_inspect_unit_gameobject.transform.Find("Background/ButtonCulturesContainer/Button");
            tmp_button = button_culture.GetComponent<Button>();
            tmp_button.onClick.AddListener(cw_wci.clickCulture);

            Transform button_edit_traits = origin_inspect_unit_gameobject.transform.Find("Background/ButtonContainerTraits/Button");
            tmp_button =button_edit_traits.GetComponent<Button>();
            tmp_button.onClick.AddListener(cw_wci.clickTraitEditor);
            // 设置新stat bar
            GameObject ShiedBar = Instantiate<GameObject>(content_transform.Find("HealthBar").gameObject);
            ShiedBar.transform.SetParent(content_transform);
            cw_wci.shied = ShiedBar.GetComponent<StatBar>();
            cw_wci.shied.transform.localPosition = new Vector3(79.55f, -85.30f);
            cw_wci.shied.transform.localScale = Vector3.one;
            ShiedBar.name = "ShiedBar";
            Image shied_bar_image = ShiedBar.transform.Find("Mask/Bar").GetComponent<Image>();
            shied_bar_image.color = new Color(1, 1, 1, 0.79f);
            ShiedBar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconShied");
            ShiedBar.GetComponent<TipButton>().textOnClick = "shied";

            GameObject WakanBar = Instantiate<GameObject>(content_transform.Find("HealthBar").gameObject);
            WakanBar.transform.SetParent(content_transform);
            cw_wci.wakan = WakanBar.GetComponent<StatBar>();
            cw_wci.wakan.transform.localPosition = new Vector3(178.89f, -85.30f);
            cw_wci.wakan.transform.localScale = Vector3.one;
            WakanBar.name = "WakanBar";
            Image wakan_bar_image = WakanBar.transform.Find("Mask/Bar").GetComponent<Image>();
            wakan_bar_image.color = new Color(0.38f, 0.71f, 1, 0.75f);
            WakanBar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
            WakanBar.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.35f, 0.35f);
            WakanBar.GetComponent<TipButton>().textOnClick = "wakan";

            // 更多信息
            GameObject TipButtonPrefab = Instantiate<GameObject>(cw_wci.prefabTrait.gameObject, cw_wci.prefabTrait.transform.parent);
            Destroy(TipButtonPrefab.transform.GetComponent<TraitButton>());
            cw_wci.prefab_tip_button = TipButtonPrefab.AddComponent<CW_TipButton>();
            cw_wci.prefab_tip_button.transform.localScale = new Vector3(0.80f, 0.80f);

            return origin_inspect_unit_gameobject;
		}
		private void Awake()
        {
            nameInput.addListener(applyInputName);
        }
        private void applyInputName(string input)
        {
            cw_actor.fast_data.setName(input);
        }
        private void Update()
        {
            if (favoriteFoodBg.gameObject.activeSelf)
            {
                favoriteFoodBg.transform.Rotate(Vector3.forward * 70f * Time.deltaTime, Space.Self);
            }
        }

        private void OnEnable()
        {
            if (Config.selectedUnit == null)
            {
                return;
            }

            AchievementLibrary.achievementTheBroken.check();
            AchievementLibrary.achievementTheDemon.check();
            AchievementLibrary.achievementTheKing.check();
            AchievementLibrary.achievementTheAccomplished.check();
            cw_actor = (CW_Actor)Config.selectedUnit;

            buttonTraitEditor.SetActive(cw_actor.stats.can_edit_traits);

            nameInput.setText(cw_actor.getName());
            health.setBar(cw_actor.fast_data.health, cw_actor.cw_cur_stats.base_stats.health, "/" + cw_actor.cw_cur_stats.base_stats.health);
            shied.setBar(cw_actor.cw_status.shied, cw_actor.cw_cur_stats.shied, "/" + cw_actor.cw_cur_stats.shied);
            wakan.setBar(cw_actor.cw_status.wakan, cw_actor.cw_cur_stats.wakan, "/" + cw_actor.cw_cur_stats.wakan);
            if (cw_actor.stats.needFood || cw_actor.stats.unit)
            {
                hunger.gameObject.SetActive(value: true);
                int num = (int)((float)cw_actor.fast_data.hunger / (float)cw_actor.stats.maxHunger * 100f);
                hunger.setBar(num, 100f, "%");
            }
            else
            {
                hunger.gameObject.SetActive(value: false);
            }


            diplomacy.gameObject.SetActive(cw_actor.stats.unit);
            warfare.gameObject.SetActive(cw_actor.stats.unit);
            stewardship.gameObject.SetActive(cw_actor.stats.unit);
            intelligence.gameObject.SetActive(cw_actor.stats.unit);

            damage.gameObject.SetActive(cw_actor.stats.inspect_stats);
            armor.gameObject.SetActive(cw_actor.stats.inspect_stats);
            speed.gameObject.SetActive(cw_actor.stats.inspect_stats);
            diplomacy.gameObject.SetActive(cw_actor.stats.inspect_stats);
            attackSpeed.gameObject.SetActive(cw_actor.stats.inspect_stats);
            crit.gameObject.SetActive(cw_actor.stats.inspect_stats);

            damage.setValue(cw_actor.cw_cur_stats.base_stats.damage);
            armor.setValue(cw_actor.cw_cur_stats.base_stats.armor);
            speed.setValue(cw_actor.cw_cur_stats.base_stats.speed);
            crit.setValue(cw_actor.cw_cur_stats.base_stats.crit, "%");
            attackSpeed.setValue(cw_actor.cw_cur_stats.base_stats.attackSpeed);
            showAttribute(diplomacy, cw_actor.cw_cur_stats.base_stats.diplomacy);
            showAttribute(stewardship, cw_actor.cw_cur_stats.base_stats.stewardship);
            showAttribute(intelligence, cw_actor.cw_cur_stats.base_stats.intelligence);
            showAttribute(warfare, cw_actor.cw_cur_stats.base_stats.warfare);

            Sprite sprite = (Sprite)Resources.Load("ui/Icons/" + cw_actor.stats.icon, typeof(Sprite));
            icon.sprite = sprite;
            avatarLoader.load(cw_actor);
            
            iconFavorite.transform.parent.gameObject.SetActive(!cw_actor.stats.hideFavoriteIcon);

            text_description.text = "";
            text_values.text = "";
            showStat("creature_statistics_age", cw_actor.fast_data.age + "/"+cw_actor.cw_status.max_age);
            if (cw_actor.stats.inspect_kills) showStat("creature_statistics_kills", cw_actor.fast_data.kills);

            if (cw_actor.stats.inspect_experience) showStat("creature_statistics_character_experience", cw_actor.fast_data.experience + "/" + cw_actor.getExpToLevelup());

            if (cw_actor.stats.inspect_experience) showStat("creature_statistics_character_level", cw_actor.fast_data.level);

            if (cw_actor.stats.inspect_children) showStat("creature_statistics_children", cw_actor.fast_data.children);

            moodBG.gameObject.SetActive(value: false);
            favoriteFoodBg.gameObject.SetActive(value: false);
            favoriteFoodSprite.gameObject.SetActive(value: false);
            if (cw_actor.stats.unit && !cw_actor.stats.baby)
            {
                string pValue = "??";
                if (!string.IsNullOrEmpty(cw_actor.fast_data.favoriteFood))
                {
                    pValue = LocalizedTextManager.getText(cw_actor.fast_data.favoriteFood);
                    favoriteFoodBg.gameObject.SetActive(value: true);
                    favoriteFoodSprite.gameObject.SetActive(value: true);
                    favoriteFoodSprite.sprite = AssetManager.resources.get(cw_actor.fast_data.favoriteFood).getSprite();
                }

                showStat("creature_statistics_favorite_food", pValue);
            }

            if (cw_actor.stats.unit)
            {
                moodBG.gameObject.SetActive(value: true);
                showStat("creature_statistics_mood", LocalizedTextManager.getText("mood_" + cw_actor.fast_data.mood));
                MoodAsset moodAsset = AssetManager.moods.get(cw_actor.fast_data.mood);
                moodSprite.sprite = moodAsset.getSprite();
                if (CW_Actor.get_s_personality(cw_actor) != null)
                {
                    showStat("creature_statistics_personality", LocalizedTextManager.getText("personality_" + CW_Actor.get_s_personality(cw_actor).id));
                }
            }

            text_description.text += "\n";
            text_values.text += "\n";
            if (cw_actor.stats.inspect_home)
            {
                showStat("creature_statistics_homeVillage", (cw_actor.city != null) ? CW_City.get_data(cw_actor.city).cityName : "??", (cw_actor.kingdom!=null && Reflection.GetField(typeof(Kingdom), cw_actor.kingdom, "kingdomColor")!=null)?((KingdomColor)Reflection.GetField(typeof(Kingdom), cw_actor.kingdom, "kingdomColor")).colorBorderInside32 : Toolbox.color_clear);
            }

            if (cw_actor.kingdom != null && cw_actor.kingdom.isCiv())
            {
                showStat("kingdom", cw_actor.kingdom.name, (cw_actor.kingdom != null && Reflection.GetField(typeof(Kingdom), cw_actor.kingdom, "kingdomColor") != null) ? ((KingdomColor)Reflection.GetField(typeof(Kingdom), cw_actor.kingdom, "kingdomColor")).colorBorderInside32 : Toolbox.color_clear);
            }

            Culture culture = MapBox.instance.cultures.get(cw_actor.fast_data.culture);
            if (culture != null)
            {
                string text = culture.name + "[" + culture.followers + "]";
                text = Toolbox.coloredString(text, culture.color32_text);
                showStat("culture", text);
                buttonCultures.SetActive(value: true);
            }
            else
            {
                buttonCultures.SetActive(value: false);
            }

            if (cw_actor.stats.isBoat)
            {
                Boat component = cw_actor.GetComponent<Boat>();
                showStat("passengers", ((HashSet<Actor>)Reflection.GetField(typeof(Boat), component, "unitsInside")).Count);
                if ((bool)component.CallMethod("isState",BoatState.TransportDoLoading))
                {
                    showStat("status", LocalizedTextManager.getText("status_waiting_for_passengers"));
                }
            }

            text_description.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            text_values.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            text_description.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            text_values.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            if (LocalizedTextManager.isRTLLang())
            {
                text_description.alignment = (TextAnchor)2;
                text_values.alignment = (TextAnchor)0;
            }
            else
            {
                text_description.alignment = (TextAnchor)0;
                text_values.alignment = (TextAnchor)2;
            }

            buttonCity.SetActive(cw_actor.city != null);

            buttonKingdom.SetActive(cw_actor.kingdom != null && cw_actor.kingdom.isCiv());

            backgroundCiv.SetActive(buttonCity.activeSelf || buttonKingdom.activeSelf);

            updateFavoriteIconFor(cw_actor);
            clearPrevButtons();
            loadTraits();
            loadEquipment();
            load_cultisys();
            load_cultibook();
            load_special_body();
            load_element();
            load_spells();
        }

        private void showAttribute(CityIcon pText, int pValue)
        {
            if (pValue < 4)
            {
                pText.setValue(pValue, "", Toolbox.color_negative);
            }
            else if (pValue >= 20)
            {
                pText.setValue(pValue, "", Toolbox.color_positive);
            }
            else
            {
                pText.setValue(pValue);
            }
        }

        private void showStat(string pID, object pValue)
        {
            Text text = text_description;
            text.text = text.text + LocalizedTextManager.getText(pID) + "\n";
            Text text2 = text_values;
            text2.text = text2.text + pValue?.ToString() + "\n";
        }

        private void showStat(string pID, object pValue, Color32? pColor)
        {
            Text text = text_description;
            text.text = text.text + LocalizedTextManager.getText(pID) + "\n";
            Text text2 = text_values;
            text2.text = text2.text + Toolbox.coloredString(pValue.ToString(), pColor) + "\n";
        }

        private void clearPrevButtons()
        {
            for (int i = 0; i < traitsParent.childCount; i++)
            {
                Transform child = traitsParent.GetChild(i);
                if (!(child.name == "Title"))
                {
                    Destroy(child.gameObject);
                }
            }

            for (int j = 0; j < equipmentParent.childCount; j++)
            {
                Transform child = equipmentParent.GetChild(j);
                if (!(child.name == "Title"))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void loadTraits()
        {
            int base_num = 4; int num = 0;
            int total_count = base_num + ((cw_actor.fast_data.traits == null) ? 0 : cw_actor.fast_data.traits.Count);

            CW_TipButton button_element = Instantiate(prefab_tip_button, traitsParent);
            string tmp_description = cw_actor.cw_data.element.__to_string();
            for(int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                string replace_text = "$base_element_" + i + "$";
                tmp_description = tmp_description.Replace(replace_text, LocalizedTextManager.getText(replace_text));
            }
            NCMS.Utils.Localization.Set("element_info", tmp_description);
            button_element.load(cw_actor.cw_data.element.comp_type(), "element_info", "iconElement", "normal");
            set_position_on_traits(button_element.GetComponent<RectTransform>(), num++, total_count);


            if (cw_actor.cw_status.can_culti && !string.IsNullOrEmpty(cw_actor.cw_data.cultibook_id))
            {
                CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(cw_actor.cw_data.cultibook_id);
                if (cultibook != null)
                {
                    CW_TipButton button_cultibook = Instantiate(prefab_tip_button, traitsParent);
                    NCMS.Utils.Localization.Set("CW_cultibook_name", cultibook.name);
                    NCMS.Utils.Localization.Set("CW_cultibook_info", cultibook.get_info_without_name());
                    button_cultibook.load("CW_cultibook_name", "CW_cultibook_info", (cw_actor.cw_data.cultisys & Others.CW_Constants.cultisys_immortol_tag) != 0 ? "iconCultiBook_immortal":"iconCultiBook_bushido", "normal");
                    set_position_on_traits(button_cultibook.GetComponent<RectTransform>(), num++, total_count);
                }
            }

            if (cw_actor.cw_data.cultisys != 0)
            {
                CW_TipButton button_cultisys = Instantiate(prefab_tip_button, traitsParent);
                string cultisys_info = CW_Library_Manager.instance.cultisys.parse_cultisys(cw_actor.cw_data);
                StringBuilder string_builder = new StringBuilder();
                for(int i = 0; i < cw_actor.cw_data.spells.Count; i++)
                {
                    string_builder.AppendLine(String.Format("法术[{0}]\t\t{1}", i, LocalizedTextManager.getText("spell_" + cw_actor.cw_data.spells[i])));
                }
                NCMS.Utils.Localization.Set("CW_cultisys_info", cultisys_info+"\n"+string_builder.ToString());
                button_cultisys.load("cultisys", "CW_cultisys_info", "iconCultiSys", "normal");
                button_cultisys.transform.localScale = new Vector3(0.65f, 0.80f);
                
                set_position_on_traits(button_cultisys.GetComponent<RectTransform>(), num++, total_count);
            }
            if (!string.IsNullOrEmpty(cw_actor.cw_data.special_body_id))
            {
                CW_Asset_SpecialBody body = CW_Library_Manager.instance.special_bodies.get(cw_actor.cw_data.special_body_id);
                if (body != null)
                {
                    CW_TipButton button_special_body = Instantiate(prefab_tip_button, traitsParent);
                    NCMS.Utils.Localization.Set("CW_special_body_name", body.name);
                    NCMS.Utils.Localization.Set("CW_special_body_info", body.get_info_without_name());
                    button_special_body.load("CW_special_body_name", "CW_special_body_info", "iconSpecialBody", "normal");
                    set_position_on_traits(button_special_body.GetComponent<RectTransform>(), num++, total_count);
                }
            }
            if (cw_actor.fast_data.traits != null)
            {
                int count = cw_actor.fast_data.traits.Count;
                for (int i = 0; i < count; i++)
                {
                    loadTraitButton(cw_actor.fast_data.traits[i], num, total_count);
                    num++;
                }
            }
        }

        private void loadEquipment()
        {
            Actor selectedUnit = Config.selectedUnit;
            temp_equipment.Clear();
            equipmentParent.gameObject.SetActive(value: false);
            if (selectedUnit.equipment == null)
            {
                return;
            }

            equipmentParent.gameObject.SetActive(value: true);
            if (selectedUnit.equipment.weapon.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.weapon.data);
            }

            if (selectedUnit.equipment.helmet.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.helmet.data);
            }

            if (selectedUnit.equipment.armor.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.armor.data);
            }

            if (selectedUnit.equipment.boots.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.boots.data);
            }

            if (selectedUnit.equipment.ring.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.ring.data);
            }

            if (selectedUnit.equipment.amulet.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.amulet.data);
            }

            int num = 0;
            int count = temp_equipment.Count;
            if (temp_equipment.Count > 0)
            {
                for (int i = 0; i < temp_equipment.Count; i++)
                {
                    loadEquipmentButton(temp_equipment[i], num, count);
                    num++;
                }
            }
        }

        private void loadEquipmentButton(ItemData pData, int pIndex, int pTotal)
        {
            EquipmentButton equipmentButton = UnityEngine.Object.Instantiate(prefabEquipment, equipmentParent);
            equipmentButton.CallMethod("load", pData);
            RectTransform component = equipmentButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 136f - num * 1.5f;
            float num3 = 22.4f * 0.8f;
            if ((float)pTotal * num3 >= num2)
            {
                num3 = num2 / (float)pTotal;
            }

            float x = num + num3 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        private void set_position_on_traits(RectTransform component, int pIndex, int pTotal)
        {
            float num = 10f;
            float num2 = 136f - num * 1.5f;
            float num3 = 22.4f * 0.7f;
            if ((float)pTotal * num3 >= num2)
            {
                num3 = num2 / (float)pTotal;
            }

            float x = num + num3 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        private void loadTraitButton(string pID, int pIndex, int pTotal)
        {
            TraitButton traitButton = UnityEngine.Object.Instantiate(prefabTrait, traitsParent);
            traitButton.CallMethod("load", pID);
            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 136f - num * 1.5f;
            float num3 = 22.4f * 0.7f;
            if ((float)pTotal * num3 >= num2)
            {
                num3 = num2 / (float)pTotal;
            }

            float x = num + num3 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }

        private void updateFavoriteIconFor(Actor pUnit)
        {
            if (cw_actor.fast_data.favorite)
            {
                iconFavorite.color = Color.white;
            }
            else
            {
                iconFavorite.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        public void pressFavorite()
        {
            if (!(cw_actor == null))
            {
                cw_actor.fast_data.favorite = !cw_actor.fast_data.favorite;
                updateFavoriteIconFor(cw_actor);
                if (cw_actor.fast_data.favorite)
                {
                    WorldTip.showNowTop("tip_favorite_icon");
                }
            }
        }

        private void OnDisable()
        {
            nameInput.inputField.DeactivateInputField();
        }
        private void load_cultisys() { }
        private void load_cultibook() { }
        private void load_special_body() { }
        private void load_element() { }
        private void load_spells() { }

        public void clickChildren()
        {
			return;
        }
		public void clickPope()
        {
			return;
        }
        public void clickKingdom()
        {
            Config.selectedKingdom = Config.selectedUnit.kingdom;
            ScrollWindow.moveAllToLeftAndRemove(true);
            ScrollWindow.showWindow("kingdom");
            ScrollWindow.get("kingdom").GetComponent<KingdomWindow>().showInfo();
        }

        // Token: 0x06001544 RID: 5444 RVA: 0x00011019 File Offset: 0x0000F219
        public void clickVillage()
        {
            Config.selectedCity = Config.selectedUnit.city;
            ScrollWindow.moveAllToLeftAndRemove(true);
            ScrollWindow.get("village").clickShow();
            ScrollWindow.get("village").GetComponent<CityWindow>().CallMethod("showMainInfo");
        }

        // Token: 0x06001545 RID: 5445 RVA: 0x000B5608 File Offset: 0x000B3808
        public void clickCulture()
        {
            if (string.IsNullOrEmpty(cw_actor.fast_data.culture))
            {
                return;
            }
            Config.selectedCulture = MapBox.instance.cultures.get(cw_actor.fast_data.culture);
            ScrollWindow.showWindow("culture");
        }

        // Token: 0x06001546 RID: 5446 RVA: 0x00011053 File Offset: 0x0000F253
        public void clickTraitEditor()
        {
            ScrollWindow.showWindow("trait_editor");
        }
    }
}
