using Chinese_Name;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Others;

namespace Cultivation_Way.General.AboutNameGenerate;

public static class NameGenerateUtils
{
#if 一米_中文名
    public static string GenerateCultibookName(Cultibook pCultibook, CW_Actor pEditor)
    {
        CN_NameGeneratorAsset generator = CN_NameGeneratorLibrary.Get("cultibook_name");

        GetCultibookNameParameters para_getter =
            ParameterGetters.GetCustomParameterGetter<GetCultibookNameParameters>(generator.parameter_getter);
        int max_try = 10;
        while (max_try-- > 0)
        {
            CN_NameTemplate template = generator.GetRandomTemplate();
            var parameters = template.GetParametersToFill();
            
            para_getter(pCultibook, pEditor, parameters);
            
            string name = template.GenerateName(parameters);
            
            if(string.IsNullOrWhiteSpace(name)) continue;

            return name;
        }
        
        return "";
    }

    public static string GenerateItemName(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator)
    {
        CN_NameGeneratorAsset generator = CN_NameGeneratorLibrary.Get(pItemAsset.VanillaAsset.name_class);

        GetCWItemNameParameters para_getter =
            ParameterGetters.GetCustomParameterGetter<GetCWItemNameParameters>(generator.parameter_getter);
        int max_try = 10;
        while (max_try-- > 0)
        {
            CN_NameTemplate template = generator.GetRandomTemplate();
            var parameters = template.GetParametersToFill();
            
            para_getter(pItemData, pItemAsset, pCreator, parameters);
            
            string name = template.GenerateName(parameters);
            
            if(string.IsNullOrWhiteSpace(name)) continue;

            return name;
        }
        
        return "";
    }
#else
    public static string GenerateCultibookName(Cultibook pCultibook, CW_Actor pCreator, CW_Actor pEditor)
    {
        return "";
    }

    public static string GenerateItemName(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator)
    {
        return "";
    }
#endif
}