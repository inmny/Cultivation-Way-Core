using System.Collections.Generic;
using System.Text;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using UnityEngine;
using UnityEngine.UI;

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
            $"{(int)cultisys_asset.curr_progress(actor, cultisys_asset, level)}/{(int)cultisys_asset.max_progress(actor, cultisys_asset, level)}");

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
        Cultibook cultibook;
        if (data.actor != null)
        {
            cultibook = data.actor.data.get_cultibook();
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
}