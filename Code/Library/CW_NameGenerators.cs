using System.Collections.Generic;
using System.IO;
using Cultivation_Way.Core;
using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
#if 一米_中文名
using Chinese_Name;
#endif

namespace Cultivation_Way.Library;

internal static class CW_NameGenerators
{
    public static void init()
    {
#if 一米_中文名
        ParameterGetters.PutCustomParameterGetter<GetCultibookNameParameters>("default",
            default_cultibook_name_parameters);
        ParameterGetters.PutCustomParameterGetter<GetCWItemNameParameters>("default", default_cw_item_name_parameters);

        WordLibraryManager.SubmitDirectoryToLoad(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath,
            "GameResources/chinese_name/word_libraries/inmny"));
        CN_NameGeneratorLibrary.SubmitDirectoryToLoad(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath,
            "GameResources/chinese_name/name_generators/inmny"));
#endif
    }

    [Hotfixable]
    private static void default_cultibook_name_parameters(Cultibook pCultibook, CW_Actor pEditor,
        Dictionary<string, string> pParameters)
    {
        _ = pEditor.getName();

        pEditor.data.get("chinese_family_name", out string creator_family_name, "");

        pParameters["creator_family_name"] = creator_family_name;

        if (pCultibook.spells.Count > 0)
        {
            int[] elements = new int[Constants.Core.element_type_nr];
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                elements[i] = 0;
            }

            foreach (string spell_id in pCultibook.spells)
            {
                CW_SpellAsset spell_asset = Manager.spells.get(spell_id);

                if (spell_asset == null)
                {
                    continue;
                }

                for (int i = 0; i < Constants.Core.element_type_nr; i++)
                {
                    elements[i] += spell_asset.element.BaseElements[i];
                }
            }

            CW_Element element = new(elements, true);
            pParameters["spell_element"] = LM.Get(element.GetElementType().id);
        }
        else
        {
            pParameters["spell_element"] = "";
        }

        string main_bonus_stat = "";
        float main_bonus_stat_value = 0;
        foreach (var container in pCultibook.bonus_stats.mods_list)
        {
            if (container.value > main_bonus_stat_value)
            {
                main_bonus_stat_value = container.value;
                main_bonus_stat = container.id;
            }
        }

        pParameters["main_bonus_stat"] = main_bonus_stat;
    }

    [Hotfixable]
    private static void default_cw_item_name_parameters(CW_ItemData pItemData, CW_ItemAsset pItemAsset,
        CW_Actor pCreator,
        Dictionary<string, string> pParameters)
    {
        if (pItemData.Spells.Count > 0)
        {
            int[] elements = new int[Constants.Core.element_type_nr];
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                elements[i] = 0;
            }

            foreach (string spell_id in pItemData.Spells)
            {
                CW_SpellAsset spell_asset = Manager.spells.get(spell_id);

                if (spell_asset == null)
                {
                    continue;
                }

                for (int i = 0; i < Constants.Core.element_type_nr; i++)
                {
                    elements[i] += spell_asset.element.BaseElements[i];
                }
            }

            CW_Element element = new(elements, true);
            pParameters["spell_element"] = LM.Get(element.GetElementType().id);
        }
        else
        {
            pParameters["spell_element"] = "";
        }

        string main_bonus_stat = "";
        float main_bonus_stat_value = 0;
        if (pItemData.addition_stats.mods_list != null) // 当物品等级为1时, addition_stats.mods_list可能为null, 这不是bug
        {
            foreach (var container in pItemData.addition_stats.mods_list)
            {
                if (!(container.value > main_bonus_stat_value)) continue;
                main_bonus_stat_value = container.value;
                main_bonus_stat = container.id;
            }
        }

        pParameters["main_bonus_stat"] = main_bonus_stat;
    }
}