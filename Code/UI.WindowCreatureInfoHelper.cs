using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI
{
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
        public static Transform content_transform;
        public static Transform background_transform;
        public static Transform stat_icons_transform;
        private static bool initialized = false;
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
            new_stats.GetComponent<TipButton>().textOnClick = "spell_armor";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "crit_damage_mod";
            crit_damage_mod = new_stats.GetComponent<CityIcon>();
            crit_damage_mod.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconBloodlust");
            new_stats.GetComponent<TipButton>().textOnClick = "crit_damage_mod";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "anti_injury";
            throns = new_stats.GetComponent<CityIcon>();
            throns.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconAnti_Injury");
            new_stats.GetComponent<TipButton>().textOnClick = "anti_injury";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "knockback_reduction";
            knockback_reduction = new_stats.GetComponent<CityIcon>();
            knockback_reduction.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconKnockback_Reduction");
            new_stats.GetComponent<TipButton>().textOnClick = "knockback_reduction";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "health_regen";
            health_regen = new_stats.GetComponent<CityIcon>();
            health_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconRegeneration");
            new_stats.GetComponent<TipButton>().textOnClick = "health_regen";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "shield_regen";
            shield_regen = new_stats.GetComponent<CityIcon>();
            shield_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/tech/icon_tech_defense_strategy");
            new_stats.GetComponent<TipButton>().textOnClick = "shield_regen";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "wakan_regen";
            wakan_regen = new_stats.GetComponent<CityIcon>();
            wakan_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
            new_stats.GetComponent<TipButton>().textOnClick = "wakan_regen";

            new_stats = UnityEngine.Object.Instantiate<GameObject>(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
            new_stats.name = "culti_velo_co";
            culti_velo_co = new_stats.GetComponent<CityIcon>();
            culti_velo_co.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconCultiSys");
            new_stats.GetComponent<TipButton>().textOnClick = "culti_velo_co";
            new_stats.GetComponent<TipButton>().textOnClickDescription = "tip_culti_velo_co";
            #endregion

            content_transform.Find("Part 1/MoodBG").localPosition = new Vector3(-91.9f, 15);
            background_transform.Find("Scroll View/Viewport").GetComponent<Mask>().enabled = true;
            background_transform.Find("Scroll View/Viewport").GetComponent<Image>().enabled = true;
            content_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            background_transform.Find("Scroll View").GetComponent<ScrollRect>().enabled = true;
            stat_icons_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;


            initialized = true;
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
            culti_velo_co.setValue(actor.stats[CW_S.mod_cultivelo], "", "", false);
            
            #endregion
        }
        public static void Update_postfix()
        {
            if (!initialized) return;

        }
    }
}
