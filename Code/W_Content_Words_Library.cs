using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Words_Library
    {
        internal static void add_words_libraries()
        {
            load_words("hundreds_of_family_names",null);
            load_words("thousand_common_words", null);
            load_words("eh_city_postfix_name", "cities/");
            load_words("eh_city_fixed_name", "cities/");
            load_words("eh_kingdom_fixed_name", "kingdoms/");
            load_words("eh_kingdom_main_name", "kingdoms/");
            load_words("eh_kingdom_postfix_name", "kingdoms/");
            load_words("eh_culture_postfix_name", "cultures/");
            load_words("eh_culture_fixed_name", "cultures/");
            load_words("cultibook_postfix_name", "cultibooks/");
            load_words("cultibook_random_main_name", "cultibooks/");
            load_words("S_B_postfix_name", "special_bodies/");
            load_words("S_B_random_main_name", "special_bodies/");
            load_words("kingdom_mottos", "kingdoms/");
        }
        private static void load_words(string id, string dir)
        {
            CW_Library_Manager.instance.words_libraries.load_words(id, string.Format("cw_words_libraries/{0}{1}",dir,id));
        }
    }
}
