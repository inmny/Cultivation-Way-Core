using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI
{
    internal class CultiProgress : MonoBehaviour
    {
        private RectTransform mask;
        private RectTransform bar;
        private Image bar_image;
        private float bar_height;
        private float set_height;
        private CW_TipButton tip_button;
        void Awake()
        {
            mask = transform.Find("Mask") as RectTransform;
            bar = transform.Find("Mask/Bar") as RectTransform;
            bar_image = bar.GetComponent<Image>();
            bar_height = bar.sizeDelta.y;
            GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                clear(); mask.DOSizeDelta(new(mask.sizeDelta.x, set_height), set_height / bar_height, false);
            }));
            tip_button = transform.Find("Info").GetComponent<CW_TipButton>();
            tip_button.transform.localScale = new(1, 1);
            tip_button.transform.localPosition = new(0, -60, 0);
        }
        public void clear()
        {
            mask.sizeDelta = new(mask.sizeDelta.x, 0.1f);
            tip_button.gameObject.SetActive(false);
        }
        void Update()
        {
            bar.localPosition = new(0, 0);
        }
        public void load_cultisys(CultisysAsset cultisys, int cultisys_level, CultisysType type, CW_Actor actor)
        {
            float max = cultisys.max_progress(actor, cultisys, cultisys_level);
            float curr = cultisys.curr_progress(actor, cultisys, cultisys_level);
            bar_image.color = type switch
            {
                CultisysType.BODY => Color.red,
                CultisysType.SOUL => Color.gray,
                CultisysType.WAKAN => Color.blue,
                _ => Color.white,
            };
            set_height = bar_height * curr / max;
            mask.DOSizeDelta(new(mask.sizeDelta.x, set_height), set_height / bar_height, false);

            tip_button.gameObject.SetActive(true);
            tip_button.transform.localPosition = new(0, 0);
            tip_button.load(cultisys.sprite_path.Replace("ui/Icons/",""), (GameObject go) =>
            {
                Tooltip.show(go, Constants.Core.mod_prefix + "cultisys", new TooltipData
                {
                    actor = actor,
                    tip_name = cultisys.id,
                });
            });
        }
    }
    internal class WindowCreatureInfoHelper
    {
        public static CityIcon spell_armor;
        public static CityIcon crit_damage_mod;
        public static CityIcon throns;
        public static CityIcon knockback_reduction;
        public static CityIcon health_regen;
        public static CityIcon shield_regen;
        public static CityIcon wakan_regen;
        public static CityIcon culti_velo_co;

        public static CW_TipButton element;
        public static CW_TipButton cultibook;
        public static CW_TipButton blood;

        public static CultiProgress body_progress;
        public static CultiProgress soul_progress;
        public static CultiProgress wakan_progress;

        public static Transform content_transform;
        public static Transform background_transform;
        public static Transform stat_icons_transform;
        private static bool initialized = false;
        private static bool first_open = true;
        public static void init(ScrollWindow scroll_window)
        {
            if (initialized) return;
            background_transform = scroll_window.transform.Find("Background");
            content_transform = scroll_window.transform.Find("Background/Scroll View/Viewport/Content");

            #region StatIcons
            stat_icons_transform = content_transform.Find("StatIcons");
            GridLayoutGroup grid_layout_group = stat_icons_transform.GetComponent<GridLayoutGroup>();
            grid_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid_layout_group.constraintCount = 9;

            GameObject new_stats;
            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "spell_armor";
            spell_armor = new_stats.GetComponent<CityIcon>();
            spell_armor.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconSpell_Armor");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.spell_armor).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "crit_damage_mod";
            crit_damage_mod = new_stats.GetComponent<CityIcon>();
            crit_damage_mod.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconBloodlust");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(S.critical_damage_multiplier).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "anti_injury";
            throns = new_stats.GetComponent<CityIcon>();
            throns.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconAnti_Injury");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.throns).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "knockback_reduction";
            knockback_reduction = new_stats.GetComponent<CityIcon>();
            knockback_reduction.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconKnockback_Reduction");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(S.knockback_reduction).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "health_regen";
            health_regen = new_stats.GetComponent<CityIcon>();
            health_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconRegeneration");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.health_regen).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "shield_regen";
            shield_regen = new_stats.GetComponent<CityIcon>();
            shield_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/tech/icon_tech_defense_strategy");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.shield_regen).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "wakan_regen";
            wakan_regen = new_stats.GetComponent<CityIcon>();
            wakan_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.wakan_regen).translation_key;

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "culti_velo_co";
            culti_velo_co = new_stats.GetComponent<CityIcon>();
            culti_velo_co.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconCultiSys");
            new_stats.GetComponent<TipButton>().textOnClick = AssetManager.base_stats_library.get(CW_S.mod_cultivelo).translation_key;
            new_stats.GetComponent<TipButton>().textOnClickDescription = "tip_culti_velo_co";
            #endregion

            content_transform.Find("Part 1/MoodBG").localPosition = new Vector3(-91.9f, 15);
            background_transform.Find("Scroll View/Viewport").GetComponent<Mask>().enabled = true;
            background_transform.Find("Scroll View/Viewport").GetComponent<Image>().enabled = true;
            content_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            background_transform.Find("Scroll View").GetComponent<ScrollRect>().enabled = true;
            stat_icons_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;

            #region Left
            GameObject left_part = new("Left", typeof(Image), typeof(GridLayoutGroup));
            left_part.transform.SetParent(background_transform);
            left_part.transform.localScale = new(1, 1);
            left_part.transform.localPosition = new(-150, 0, 0);
            left_part.GetComponent<Image>().sprite = Others.FastVisit.get_square_frame();
            left_part.GetComponent<Image>().type = Image.Type.Sliced;
            left_part.GetComponent<RectTransform>().sizeDelta = new(40, 128);
            element = UnityEngine.Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
            cultibook = UnityEngine.Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
            blood = UnityEngine.Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
            element.name = "element";
            cultibook.name = "cultibook";
            blood.name = "blood";
            grid_layout_group = left_part.GetComponent<GridLayoutGroup>();
            grid_layout_group.cellSize = new(32, 32);
            grid_layout_group.spacing = new(4, 4);
            grid_layout_group.padding = new(4, 4, 8, 4);
            grid_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid_layout_group.constraintCount = 1;
            #endregion

            #region Right
            GameObject body_culti_progress = new("BodyCultiProgress", typeof(Button));
            body_culti_progress.transform.SetParent(background_transform);
            body_culti_progress.transform.localScale = new(0.5f, 0.5f);
            //body_culti_progress.transform.rotation = new(0, 0, -90, 0);
            GameObject body_progress_mask = new("Mask", typeof(Image), typeof(Mask));
            body_progress_mask.transform.SetParent(body_culti_progress.transform);
            body_progress_mask.transform.localScale = new(1f, 1f);
            body_progress_mask.transform.localPosition = new(0, -99f);
            body_progress_mask.GetComponent<RectTransform>().sizeDelta = new(30, 198);
            body_progress_mask.GetComponent<RectTransform>().pivot = new(0.5f, 0);
            body_progress_mask.GetComponent<Mask>().showMaskGraphic = false;
            GameObject body_progress_bar = new("Bar", typeof(Image));
            body_progress_bar.transform.SetParent(body_progress_mask.transform);
            body_progress_bar.transform.localScale = new(1, 1);
            body_progress_bar.transform.localPosition = new(0, 0);
            body_progress_bar.GetComponent<RectTransform>().sizeDelta = new(30, 198);
            body_progress_bar.GetComponent<RectTransform>().pivot = new(0.5f, 0);
            body_progress_bar.GetComponent<Image>().sprite = Others.FastVisit.get_window_bar_90();
            body_progress_bar.GetComponent<Image>().type = Image.Type.Sliced;
            GameObject body_culti_progress_bg = new("BG", typeof(Image));
            body_culti_progress_bg.transform.SetParent(body_culti_progress.transform);
            body_culti_progress_bg.transform.localScale = new(1f, 1f);
            body_culti_progress_bg.transform.localPosition = new(0, 0, 0);
            body_culti_progress_bg.GetComponent<RectTransform>().sizeDelta = new(40, 200);
            body_culti_progress_bg.GetComponent<Image>().sprite = Others.FastVisit.get_square_frame_only();
            body_culti_progress_bg.GetComponent<Image>().type = Image.Type.Sliced;
            CW_TipButton body_culti_info = UnityEngine.Object.Instantiate(Prefabs.tip_button_prefab, body_culti_progress.transform);
            body_culti_info.transform.localScale = new(1f, 1f);
            body_culti_info.transform.localPosition = new(0, 0, 0);
            body_culti_info.name = "Info";

            body_progress = body_culti_progress.AddComponent<CultiProgress>();

            wakan_progress = UnityEngine.Object.Instantiate(body_culti_progress, background_transform).GetComponent<CultiProgress>();
            wakan_progress.name = "WakanCultiProgress";

            soul_progress = UnityEngine.Object.Instantiate(body_culti_progress, background_transform).GetComponent<CultiProgress>();
            soul_progress.name = "SoulCultiProgress";

            body_progress.transform.localPosition = new(160, 50, 0);
            wakan_progress.transform.localPosition = new(185, 50, 0);
            soul_progress.transform.localPosition = new(210, 50, 0);
            #endregion
            init_tooltip_assets();
            initialized = true;
        }
        private static void init_tooltip_assets()
        {
            AssetManager.tooltips.add(new TooltipAsset
            {
                id = Constants.Core.mod_prefix + "element",
                prefab_id = "tooltips/tooltip_"+Constants.Core.mod_prefix + "element",
                callback = show_element
            });
            AssetManager.tooltips.add(new TooltipAsset
            {
                id = Constants.Core.mod_prefix + "cultibook",
                prefab_id = "tooltips/tooltip_" + Constants.Core.mod_prefix + "cultibook",
                callback = show_cultibook
            });
            AssetManager.tooltips.add(new TooltipAsset
            {
                id = Constants.Core.mod_prefix + "blood_nodes",
                prefab_id = "tooltips/tooltip_" + Constants.Core.mod_prefix + "blood_nodes",
                callback = show_blood_nodes
            });
            AssetManager.tooltips.add(new TooltipAsset
            {
                id = Constants.Core.mod_prefix + "cultisys",
                prefab_id = "tooltips/tooltip_" + Constants.Core.mod_prefix + "cultisys",
                callback = show_cultisys
            });
        }

        private static void show_cultisys(Tooltip tooltip, string type, TooltipData data = default)
        {
            CW_Actor actor = (CW_Actor)data.actor;
            CultisysAsset cultisys_asset = Library.Manager.cultisys.get(data.tip_name);
            actor.data.get(cultisys_asset.id, out int level, -1);

            tooltip.name.text = LocalizedTextManager.getText(data.tip_name, null);

            StringBuilder str_builder = new();
            str_builder.AppendLine($"{LocalizedTextManager.getText($"{cultisys_asset.id}_{level}")}({level + 1}境)");
            str_builder.AppendLine($"{cultisys_asset.curr_progress(actor, cultisys_asset, level)}/{cultisys_asset.max_progress(actor, cultisys_asset, level)}");

            tooltip.addDescription(str_builder.ToString());

            if (CW_Core.mod_state.editor_inmny)
            {
                tooltip.showBaseStats(cultisys_asset.get_bonus_stats(actor, level));
            }
        }

        private static void show_element(Tooltip tooltip, string type, TooltipData data = default)
        {
            CW_Actor actor = (CW_Actor)data.actor;
            // 可以确定actor的element不为空
            CW_Element element = actor.data.get_element();

            tooltip.name.text = LocalizedTextManager.getText(element.get_type().id, null);

            StringBuilder str_builder = new();
            for(int i=0;i<Constants.Core.element_type_nr;i++)
            {
                str_builder.AppendLine($"{LocalizedTextManager.getText(Constants.Core.element_str[i])}\t{element.base_elements[i]}%");
            }
            tooltip.addDescription(str_builder.ToString());

            tooltip.showBaseStats(element.comp_bonus_stats());
        }
        private static void show_cultibook(Tooltip tooltip, string type, TooltipData data = default)
        {
            CW_Actor actor = (CW_Actor)data.actor;
            // 可以确定actor的cultibook不为空
            Cultibook cultibook = actor.data.get_cultibook();

            tooltip.name.text = cultibook.name;
            StringBuilder str_builder = new();
            str_builder.AppendLine($"{cultibook.author_name} 著");
            str_builder.AppendLine($"{cultibook.editor_name} 编");
            str_builder.AppendLine(cultibook.description);
            tooltip.addDescription(str_builder.ToString());
           

            tooltip.showBaseStats(cultibook.bonus_stats);
        }
        private static void show_blood_nodes(Tooltip tooltip, string type, TooltipData data = default)
        {
            CW_Actor actor = (CW_Actor)data.actor;
            // 可以确定actor的blood_nodes不为空
            Dictionary<string, float> blood_nodes = actor.data.get_blood_nodes();
            BloodNodeAsset main_blood = actor.data.get_main_blood();

            tooltip.name.text = "血脉";
            StringBuilder str_builder = new();
            str_builder.AppendLine($"占优血脉\t {main_blood.ancestor_data.name}({(int)(blood_nodes[main_blood.id]*100)}%)");
            str_builder.AppendLine($"{main_blood.alive_descendants_count}/{main_blood.max_descendants_count}");
            foreach(string blood_id in blood_nodes.Keys)
            {
                if (blood_id == main_blood.id) continue;
                BloodNodeAsset blood = Library.Manager.bloods.get(blood_id);
                str_builder.AppendLine($"{blood.ancestor_data.name}({(int)(blood_nodes[blood_id] * 100)}%)");
            }
            tooltip.addDescription(str_builder.ToString());

            if (CW_Core.mod_state.editor_inmny)
            {
                //tooltip.showBaseStats(main_blood.ancestor_stats);
            }
        }
        public static void OnEnable_postfix(WindowCreatureInfo window_creature_info)
        {
            if (!initialized) return;
            CW_Actor actor = (CW_Actor)window_creature_info.actor;
            #region StatIcons
            spell_armor.gameObject.SetActive(true);
            crit_damage_mod.gameObject.SetActive(true);
            throns.gameObject.SetActive(true);
            knockback_reduction.gameObject.SetActive(true);
            health_regen.gameObject.SetActive(true);
            shield_regen.gameObject.SetActive(true);
            wakan_regen.gameObject.SetActive(true);
            culti_velo_co.gameObject.SetActive(true);
            if (!actor.asset.inspect_stats)
            {
                spell_armor.gameObject.SetActive(false);
                crit_damage_mod.gameObject.SetActive(false);
                throns.gameObject.SetActive(false);
                knockback_reduction.gameObject.SetActive(false);
                health_regen.gameObject.SetActive(false);
                shield_regen.gameObject.SetActive(false);
                wakan_regen.gameObject.SetActive(false);
                culti_velo_co.gameObject.SetActive(false);
            }
            window_creature_info.armor.setValue(actor.stats[S.armor], "", "", false);
            spell_armor.setValue(actor.stats[CW_S.spell_armor], "", "", false);
            crit_damage_mod.setValue(actor.stats[S.critical_damage_multiplier], "", "", false);
            throns.setValue(actor.stats[CW_S.throns], "", "", false);
            knockback_reduction.setValue(actor.stats[S.knockback_reduction], "", "", false);
            health_regen.setValue(actor.stats[CW_S.health_regen], "", "", false);
            shield_regen.setValue(actor.stats[CW_S.shield_regen], "", "", false);
            wakan_regen.setValue(0, "", "", false);
            culti_velo_co.setValue((1+actor.stats[CW_S.mod_cultivelo]) * actor.cw_asset.culti_velo, "", "", false);

            #endregion
            element.gameObject.SetActive(false);
            cultibook.gameObject.SetActive(false);
            blood.gameObject.SetActive(false);


            if (actor.data.get_element() != null) load_element(actor);

            if (actor.data.get_cultibook() != null) load_cultibook(actor);

            if (actor.data.get_blood_nodes() != null) load_blood(actor);

            body_progress.clear();
            wakan_progress.clear();
            soul_progress.clear();

            //body_progress.load_cultisys(null, 0, CultisysType.BODY, actor);
            int[] cultisys_level = actor.data.get_cultisys_level();
            for(int i = 0; i < Library.Manager.cultisys.size; i++)
            {
                if (cultisys_level[i] < 0) continue;
                CultisysAsset cultisys = Library.Manager.cultisys.list[i];
                switch (cultisys.type)
                {
                    case CultisysType.BODY:
                        body_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.BODY, actor);
                        break;
                    case CultisysType.SOUL:
                        soul_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.SOUL, actor);
                        break;
                    case CultisysType.WAKAN:
                        wakan_progress.load_cultisys(cultisys, cultisys_level[i], CultisysType.WAKAN, actor);
                        break;
                    default:
                        break;
                }
            }
        }
        public static void Update_postfix(WindowCreatureInfo window_creature_info)
        {
            if (!initialized) return;
            if (first_open)
            {
                first_open = false;
                OnEnable_postfix(window_creature_info);
            }
        }
        private static void load_element(CW_Actor actor)
        {
            element.gameObject.SetActive(true);
            element.load("iconElement", (GameObject obj) =>
            {
                Tooltip.show(obj, Constants.Core.mod_prefix + "element", new TooltipData
                {
                    actor = actor
                });
            });
        }
        private static void load_cultibook(CW_Actor actor)
        {
            cultibook.gameObject.SetActive(true);
            cultibook.load("iconCultiBook_immortal", (GameObject obj) =>
            {
                Tooltip.show(obj, Constants.Core.mod_prefix + "cultibook", new TooltipData
                {
                    actor = actor
                });
            });
        }
        private static void load_blood(CW_Actor actor)
        {
            blood.gameObject.SetActive(true);
            blood.load(actor.asset.icon, (GameObject obj) =>
            {
                Tooltip.show(obj, Constants.Core.mod_prefix + "blood_nodes", new TooltipData
                {
                    actor = actor
                });
            });
        }
    }
}
