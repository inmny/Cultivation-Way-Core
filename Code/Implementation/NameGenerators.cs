#if 一米_中文名
using Chinese_Name;
#endif

namespace Cultivation_Way.Implementation;

internal static class NameGenerators
{
    public static void init()
    {
        #if 一米_中文名
        
        WordLibraryManager.SubmitDirectoryToLoad("chinese_name/word_libraries/inmny");
        CN_NameGeneratorLibrary.SubmitDirectoryToLoad("chinese_name/name_generators/inmny");
        
        #endif
    }
}