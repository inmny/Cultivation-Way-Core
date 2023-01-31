using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Actions
{
    public static class CW_NameTemplateActions
    {
        public static bool random_select(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            return Toolbox.randomChance(name_template_elm.free_val);
        }
        public static bool must_select(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            return true;
        }
        public static string random_decode(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            return CW_Library_Manager.instance.words_libraries.get(name_template_elm.words_id).words.GetRandom();
        }
        public static string descend_family_name(CW_Template_Elm name_template_elm, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            if (cw_actor!=null && !string.IsNullOrEmpty(cw_actor.cw_data.family_name) && Toolbox.randomChance(1 - Others.CW_Constants.new_family_name_chance)) return cw_actor.cw_data.family_name;
            string family_name = CW_Library_Manager.instance.words_libraries.get(name_template_elm.words_id).words.GetRandom();
            cw_actor.cw_data.family_name = family_name;
            return family_name;
        }
    }
}
