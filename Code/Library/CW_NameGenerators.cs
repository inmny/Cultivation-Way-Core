using System.Collections.Generic;
using System.IO;
#if 一米_中文名
using Chinese_Name;
#endif
using Cultivation_Way.Core;
using Cultivation_Way.Others;

namespace Cultivation_Way.Library;

internal static class CW_NameGenerators
{
    public static void init()
    {
#if 一米_中文名
        ParameterGetters.PutCustomParameterGetter<GetCultibookNameParameters>("default", default_cultibook_name_parameters);
        ParameterGetters.PutCustomParameterGetter<GetCWItemNameParameters>("default", default_cw_item_name_parameters);
        
        WordLibraryManager.SubmitDirectoryToLoad(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath,
            "GameResources/chinese_name/word_libraries/inmny"));
        CN_NameGeneratorLibrary.SubmitDirectoryToLoad(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath,
            "GameResources/chinese_name/name_generators/inmny"));
#endif
    }

    private static void default_cultibook_name_parameters(Cultibook pCultibook, CW_Actor pEditor,
        Dictionary<string, string> pParameters)
    {
    }
    private static void default_cw_item_name_parameters(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator,
        Dictionary<string, string> pParameters)
    {
    }
}