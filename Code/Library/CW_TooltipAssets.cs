using System;
using System.Collections.Generic;
using System.Text;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
namespace Cultivation_Way.Library;

internal static class CW_TooltipAssets
{
    public static void init()
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
        var equipment_tooltip = AssetManager.tooltips.get("equipment");
        equipment_tooltip.callback =
            (TooltipShowAction)Delegate.Combine(equipment_tooltip.callback, new TooltipShowAction(show_cw_item));
    }

    [Hotfixable]
    private static void show_cw_item(Tooltip pTooltip, string pType, TooltipData pData = default)
    {
        pTooltip.transform.Find("HeadDescription")?.gameObject.SetActive(false);
        pTooltip.transform.Find("Spells")?.gameObject.SetActive(false);
        if (pData.item is not CW_ItemData cw_item)
        {
            return;
        }

        CW_ItemAsset cw_item_asset = Manager.items.get(cw_item.id);

        if (pTooltip.transform.Find("HeadDescription") == null)
        {
            add_head_description(pTooltip);
        }

        if (pTooltip.transform.Find("Spells") == null)
        {
            add_spell_stats(pTooltip);
        }

        Text head_description_text = pTooltip.transform.Find("HeadDescription").GetComponent<Text>();
        if (LocalizedTextManager.stringExists($"item_desc_{cw_item.id}"))
        {
            head_description_text.text = LM.Get($"item_desc_{cw_item.id}");
            head_description_text.gameObject.SetActive(true);
        }
        else
        {
            head_description_text.text = string.Empty;
            head_description_text.gameObject.SetActive(false);
        }


        var spells_description = pTooltip.transform.Find("Spells/StatsDescription").GetComponent<Text>();
        var spells_values = pTooltip.transform.Find("Spells/StatsValues").GetComponent<Text>();
        var spells_container = pTooltip.transform.Find("Spells").gameObject;

        spells_description.text = string.Empty;
        spells_values.text = string.Empty;

        void addSpell(string spell_id)
        {
            spells_description.text += LocalizedTextManager.getText("2_immortal_spell") + "\n";
            spells_values.text += Toolbox.coloredText(LocalizedTextManager.getText($"spell_{spell_id}"),
                Toolbox.colorToHex(Manager.spells.get(spell_id).element.GetColor())) + "\n";

            spells_container.SetActive(true);
        }

        foreach (var spell in cw_item.Spells)
        {
            addSpell(spell);
        }

        spells_description.text = spells_description.text.TrimEnd('\n');
        spells_values.text = spells_values.text.TrimEnd('\n');


        LocalizedText item_level_text =
            pTooltip.transform.Find("Equipment Type/EquipmentText").GetComponent<LocalizedText>();
        item_level_text.setKeyAndUpdate($"cw_{cw_item_asset.VanillaAsset.name_class}");
        item_level_text.text.text = item_level_text.text.text
            .Replace("$item_stage$", LM.Get($"item_stage_{cw_item.Level / Constants.Core.item_level_per_stage}"))
            .Replace("$item_level$", LM.Get($"item_level_{cw_item.Level % Constants.Core.item_level_per_stage}"));
    }

    [Hotfixable]
    private static void add_spell_stats(Tooltip pTooltip)
    {
        GameObject spell_stats = Object.Instantiate(pTooltip.stats_container, pTooltip.transform);
        spell_stats.name = "Spells";
        spell_stats.transform.SetAsLastSibling();
    }

    private static void add_head_description(Tooltip pTooltip)
    {
        GameObject head_description = new("HeadDescription", typeof(Text), typeof(LayoutElement));
        head_description.transform.SetParent(pTooltip.transform);
        head_description.transform.localScale = Vector3.one;
        head_description.transform.SetSiblingIndex(1);

        Text head_description_text = head_description.GetComponent<Text>();
        head_description_text.font = pTooltip.name.font;
        head_description_text.fontSize = 6;
        head_description_text.color = Colors.default_color;
    }

    [Hotfixable]
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
            $"{LM.Get(cultisys_asset.culti_energy_id)} {(int)cultisys_asset.curr_progress(actor, cultisys_asset, level)}/{(int)cultisys_asset.max_progress(actor, cultisys_asset, level)}");

        HashSet<string> spells = new(actor.cur_spells);
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
        CW_Element element = actor.data.GetElement();

        tooltip.name.text = LocalizedTextManager.getText(element.GetElementType().id);

        StringBuilder str_builder = new();
        for (int i = 0; i < Constants.Core.element_type_nr; i++)
        {
            str_builder.AppendLine(
                $"{LocalizedTextManager.getText(Constants.Core.element_str[i])}\t{element.BaseElements[i]}%");
        }

        tooltip.addDescription(str_builder.ToString());

        tooltip.showBaseStats(element.ComputeBonusStats());
    }

    private static void show_cultibook(Tooltip tooltip, string type, TooltipData data = default)
    {
        Cultibook cultibook;
        if (data.actor != null)
        {
            cultibook = data.actor.data.GetCultibook();
        }
        else
        {
            cultibook = Manager.cultibooks.get(data.tip_name);
        }

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
            Color color = Manager.spells.get(spell_id).element.GetColor();
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

        if (cultibook.spells.Count == 0)
        {
            tooltip.transform.Find("Spells").gameObject.SetActive(false);
        }
        else
        {
            tooltip.transform.Find("Spells").gameObject.SetActive(true);
        }


        tooltip.showBaseStats(cultibook.bonus_stats);
    }

    private static void show_blood_nodes(Tooltip tooltip, string type, TooltipData data = default)
    {
        CW_Actor actor = (CW_Actor)data.actor;
        // 可以确定actor的blood_nodes不为空
        Dictionary<string, float> blood_nodes = actor.data.GetBloodNodes();
        BloodNodeAsset main_blood = actor.data.GetMainBlood();

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
}