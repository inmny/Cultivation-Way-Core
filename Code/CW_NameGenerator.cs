using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way
{
    public class CW_NameGenerator
    {
        // Harmony拦截getName与generateNameFromTemplate
        // 则有getName->__gen_name->(产生后直接返回),(找不到)->原版getName->generateNameFromTemplate->__gen_name->找不到->原版generateNameFromTemplate
        public static string gen_name(string template_id, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            string result = __gen_name(template_id, cw_actor, city, kingdom, culture, tile, cultibook, special_body);
            if(!string.IsNullOrEmpty(result)) return result;
            return NameGenerator.generateNameFromTemplate(AssetManager.nameGenerator.get(template_id), cw_actor);
        }
        internal static string __gen_name(string template_id, CW_Actor cw_actor=null, City city = null, Kingdom kingdom=null, Culture culture=null, WorldTile tile = null, CW_Asset_CultiBook cultibook = null, CW_Asset_SpecialBody special_body = null)
        {
            if (ModState.instance.cur_language != "cz")
            {
                //return "";
            }
            CW_Asset_NameGenerator name_generator;
            if(CW_Library_Manager.instance.name_generators.dict.TryGetValue(template_id, out name_generator))
            {
                return name_generator.gen_name(null, cw_actor, city, kingdom, culture, tile, cultibook, special_body);
            }
            return "";
        }
    }
}
