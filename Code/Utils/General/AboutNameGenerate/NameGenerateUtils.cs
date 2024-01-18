using System.Collections.Generic;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using NeoModLoader.api.attributes;
#if 一米_中文名
using Chinese_Name;
#endif

namespace Cultivation_Way.Utils.General.AboutNameGenerate;

public static class NameGenerateUtils
{
#if 一米_中文名
    [Hotfixable]
    public static string GenerateCultibookName(Cultibook pCultibook, CW_Actor pEditor)
    {
        CN_NameGeneratorAsset generator = CN_NameGeneratorLibrary.Get("cultibook_name");

        var para = new Dictionary<string, string>();
        ParameterGetters.GetCustomParameterGetter<GetCultibookNameParameters>(generator.parameter_getter)(pCultibook,
            pEditor, para);

        return generator.GenerateName(para);
    }

    [Hotfixable]
    public static string GenerateItemName(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator)
    {
        CN_NameGeneratorAsset generator = CN_NameGeneratorLibrary.Get(pItemAsset.GetTypeName());

        var para = new Dictionary<string, string>();
        ParameterGetters.GetCustomParameterGetter<GetCWItemNameParameters>(generator.parameter_getter)(pItemData,
            pItemAsset, pCreator, para);

        return generator.GenerateName(para);
    }
#else
    public static string GenerateCultibookName(Cultibook pCultibook, CW_Actor pEditor)
    {
        return "";
    }

    public static string GenerateItemName(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator)
    {
        return "";
    }
#endif
}