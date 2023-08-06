﻿using System.Collections.Generic;
using System.Text;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

internal class CultiProgress : MonoBehaviour
{
    private StatBar bar;
    private Image bar_image;
    private TipButton tip_button;

    private void Awake()
    {
        bar = GetComponent<StatBar>();
        bar_image = transform.Find("Mask/Bar").GetComponent<Image>();
        bar_image.color = Color.white;

        bar_image.enabled = false;
        transform.Find("Background").gameObject.SetActive(false);
        transform.Find("Text").gameObject.SetActive(false);
        transform.Find("Icon").gameObject.SetActive(false);

        tip_button = GetComponent<TipButton>();
        tip_button.hoverAction = null;
    }

    public void clear()
    {
        bar_image.enabled = false;
        transform.Find("Background").gameObject.SetActive(false);
        transform.Find("Text").gameObject.SetActive(false);
        transform.Find("Icon").gameObject.SetActive(false);

        tip_button = GetComponent<TipButton>();
        tip_button.hoverAction = null;
    }

    public void load_cultisys(CultisysAsset cultisys, int cultisys_level, CultisysType type, CW_Actor actor)
    {
        bar_image.enabled = true;
        transform.Find("Background").gameObject.SetActive(true);
        transform.Find("Text").gameObject.SetActive(true);
        transform.Find("Icon").gameObject.SetActive(true);
        transform.Find("Icon").GetComponent<Image>().sprite = SpriteTextureLoader.getSprite(cultisys.sprite_path);

        float max = cultisys.max_progress(actor, cultisys, cultisys_level);
        float curr = cultisys.curr_progress(actor, cultisys, cultisys_level);
        bar.setBar(curr, max, $"/{max}");

        bar_image.color = type switch
        {
            CultisysType.BODY => Color.red,
            CultisysType.SOUL => Color.gray,
            CultisysType.WAKAN => Color.blue,
            _ => Color.white
        };
        if (cultisys.culti_energy != null)
        {
            bar_image.color = cultisys.culti_energy.get_color(curr, cultisys.power_level[cultisys_level]);
        }

        tip_button.hoverAction = () =>
        {
            Tooltip.show(gameObject, Constants.Core.mod_prefix + "cultisys", new TooltipData
            {
                actor = actor,
                tip_name = cultisys.id
            });
        };
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
    private static bool initialized;
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
        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "spell_armor";
        spell_armor = new_stats.GetComponent<CityIcon>();
        spell_armor.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconSpell_Armor");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.spell_armor).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "crit_damage_mod";
        crit_damage_mod = new_stats.GetComponent<CityIcon>();
        crit_damage_mod.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconBloodlust");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(S.critical_damage_multiplier).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "anti_injury";
        throns = new_stats.GetComponent<CityIcon>();
        throns.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconAnti_Injury");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.throns).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "knockback_reduction";
        knockback_reduction = new_stats.GetComponent<CityIcon>();
        knockback_reduction.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconKnockback_Reduction");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(S.knockback_reduction).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "health_regen";
        health_regen = new_stats.GetComponent<CityIcon>();
        health_regen.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconRegeneration");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.health_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "shield_regen";
        shield_regen = new_stats.GetComponent<CityIcon>();
        shield_regen.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/tech/icon_tech_defense_strategy");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.shield_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "wakan_regen";
        wakan_regen = new_stats.GetComponent<CityIcon>();
        wakan_regen.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.wakan_regen).translation_key;

        new_stats = Object.Instantiate(stat_icons_transform.Find("damage").gameObject, stat_icons_transform);
        new_stats.name = "culti_velo_co";
        culti_velo_co = new_stats.GetComponent<CityIcon>();
        culti_velo_co.transform.Find("Icon").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("ui/Icons/iconCultiSys");
        new_stats.GetComponent<TipButton>().textOnClick =
            AssetManager.base_stats_library.get(CW_S.mod_cultivelo).translation_key;
        new_stats.GetComponent<TipButton>().textOnClickDescription = "tip_culti_velo_co";

        #endregion

        content_transform.Find("Part 1/MoodBG").localPosition = new Vector3(-91.9f, 15);
        background_transform.Find("Scroll View/Viewport").GetComponent<Mask>().enabled = true;
        background_transform.Find("Scroll View/Viewport").GetComponent<Image>().enabled = true;
        content_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;
        background_transform.Find("Scroll View").GetComponent<ScrollRect>().enabled = true;
        stat_icons_transform.gameObject.AddComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.MinSize;

        #region Left

        GameObject left_part = new("Left", typeof(Image), typeof(GridLayoutGroup));
        left_part.transform.SetParent(background_transform);
        left_part.transform.localScale = new Vector3(1, 1);
        left_part.transform.localPosition = new Vector3(-250, 0, 0);
        left_part.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        left_part.GetComponent<Image>().type = Image.Type.Sliced;
        left_part.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 128);
        element = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        cultibook = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        blood = Object.Instantiate(Prefabs.tip_button_prefab, left_part.transform);
        element.name = "element";
        cultibook.name = "cultibook";
        blood.name = "blood";
        grid_layout_group = left_part.GetComponent<GridLayoutGroup>();
        grid_layout_group.cellSize = new Vector2(32, 32);
        grid_layout_group.spacing = new Vector2(4, 4);
        grid_layout_group.padding = new RectOffset(4, 4, 8, 4);
        grid_layout_group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid_layout_group.constraintCount = 1;

        #endregion

        body_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        body_progress.name = "BodyCultiProgress";
        body_progress.transform.localPosition = new Vector3(-165, 50, 0);
        body_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        body_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        body_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        body_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        body_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //body_progress.transform.Find("Text").localPosition += new Vector3(7, 0);
        wakan_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        wakan_progress.name = "WakanCultiProgress";
        wakan_progress.transform.localPosition = new Vector3(-165, 0, 0);
        wakan_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        wakan_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        wakan_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        wakan_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        wakan_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //wakan_progress.transform.Find("Text").localPosition += new Vector3(7, 0);
        soul_progress = Object.Instantiate(content_transform.Find("Part 3/HealthBar").gameObject, background_transform)
            .AddComponent<CultiProgress>();
        soul_progress.name = "SoulCultiProgress";
        soul_progress.transform.localPosition = new Vector3(-165, -50, 0);
        soul_progress.GetComponent<Image>().sprite = FastVisit.get_square_frame();
        soul_progress.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 21);
        soul_progress.transform.Find("Background").localPosition += new Vector3(7, 0);
        soul_progress.transform.Find("Mask").localPosition += new Vector3(7, 0);
        soul_progress.transform.Find("Icon").localPosition += new Vector3(7, 0);
        //soul_progress.transform.Find("Text").localPosition += new Vector3(7, 0);

        init_tooltip_assets();
        initialized = true;
    }

    private static void init_tooltip_assets()
    {
        AssetManager.tooltips.add(new TooltipAsset
        {
            id = Constants.Core.mod_prefix + "element",
            prefab_id = "tooltips/tooltip_" + Constants.Core.mod_prefix + "element",
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
        CultisysAsset cultisys_asset = Manager.cultisys.get(data.tip_name);
        if (cultisys_asset == null)
        {
            return;
        }

        actor.data.get(cultisys_asset.id, out int level, -1);

        tooltip.name.text = LocalizedTextManager.getText(data.tip_name);

        StringBuilder str_builder = new();
        str_builder.AppendLine($"{LocalizedTextManager.getText($"{cultisys_asset.id}_{level}")}({level + 1}境)");
        str_builder.AppendLine(
            $"{cultisys_asset.curr_progress(actor, cultisys_asset, level)}/{cultisys_asset.max_progress(actor, cultisys_asset, level)}");

        HashSet<string> spells = actor.data.get_spells();
        spells ??= new HashSet<string>();
        foreach (string spell_id in spells)
        {
            str_builder.AppendLine(LocalizedTextManager.getText($"spell_{spell_id}"));
        }

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

        tooltip.name.text = LocalizedTextManager.getText(element.get_type().id);

        StringBuilder str_builder = new();
        for (int i = 0; i < Constants.Core.element_type_nr; i++)
        {
            str_builder.AppendLine(
                $"{LocalizedTextManager.getText(Constants.Core.element_str[i])}\t{element.base_elements[i]}%");
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

        int idx = 0;
        Text spell_idx_text = tooltip.transform.Find("Spells/StatsDescription").GetComponent<Text>();
        Text spell_name_text = tooltip.transform.Find("Spells/StatsValues").GetComponent<Text>();

        StringBuilder spell_idx_builder = new();
        StringBuilder spell_name_builder = new();
        foreach (string spell_id in cultibook.spells)
        {
            idx++;
            Color color = Manager.spells.get(spell_id).element.get_color();
            spell_idx_builder.Append(Toolbox.coloredString($"[{idx}]", color));
            spell_name_builder.Append(Toolbox.coloredString(LocalizedTextManager.getText("spell_" + spell_id),
                color));
            if (idx != cultibook.spells.Count)
            {
                spell_idx_builder.AppendLine();
                spell_name_builder.AppendLine();
            }
        }

        spell_idx_text.text = spell_idx_builder.ToString();
        spell_name_text.text = spell_name_builder.ToString();


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
        str_builder.AppendLine($"占优血脉\t {main_blood.ancestor_data.name}({(int)(blood_nodes[main_blood.id] * 100)}%)");
        str_builder.AppendLine($"{main_blood.alive_descendants_count}/{main_blood.max_descendants_count}");
        foreach (string blood_id in blood_nodes.Keys)
        {
            if (blood_id == main_blood.id) continue;
            BloodNodeAsset blood = Manager.bloods.get(blood_id);
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

        window_creature_info.armor.setValue(actor.stats[S.armor]);
        spell_armor.setValue(actor.stats[CW_S.spell_armor]);
        crit_damage_mod.setValue(actor.stats[S.critical_damage_multiplier]);
        throns.setValue(actor.stats[CW_S.throns]);
        knockback_reduction.setValue(actor.stats[S.knockback_reduction]);
        health_regen.setValue(actor.stats[CW_S.health_regen]);
        shield_regen.setValue(actor.stats[CW_S.shield_regen]);
        wakan_regen.setValue(0);
        culti_velo_co.setValue((1 + actor.stats[CW_S.mod_cultivelo]) * actor.cw_asset.culti_velo);

        #endregion

        element.gameObject.SetActive(false);
        cultibook.gameObject.SetActive(false);
        blood.gameObject.SetActive(false);


        if (actor.data.get_element() != null) load_element(actor);

        if (actor.data.get_cultibook() != null) load_cultibook(actor);

        if (actor.data.get_blood_nodes() != null) load_blood(actor);

        //body_progress.load_cultisys(null, 0, CultisysType.BODY, actor);
        body_progress.clear();
        wakan_progress.clear();
        soul_progress.clear();
        int[] cultisys_level = actor.data.get_cultisys_level();
        for (int i = 0; i < Manager.cultisys.size; i++)
        {
            if (cultisys_level[i] < 0) continue;
            CultisysAsset cultisys = Manager.cultisys.list[i];
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
            }
        }

        load_cw_statuses(actor, window_creature_info);
    }

    private static void load_cw_statuses(CW_Actor actor, WindowCreatureInfo window_creature_info)
    {
        if (actor.statuses == null || actor.statuses.Count == 0) return;

        foreach (CW_StatusEffectData status_effect_data in actor.statuses.Values)
        {
            if (status_effect_data.finished) continue;
            StatusEffectData fake_status_effect_data =
                new(actor, AssetManager.status.get(status_effect_data.status_asset.id));
            fake_status_effect_data.setTimer(status_effect_data.left_time);

            window_creature_info.loadStatusButton(fake_status_effect_data);
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
        element.load("iconElement", obj =>
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
        cultibook.load("iconCultiBook_immortal", obj =>
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
        blood.load(actor.asset.icon, obj =>
        {
            Tooltip.show(obj, Constants.Core.mod_prefix + "blood_nodes", new TooltipData
            {
                actor = actor
            });
        });
    }
}