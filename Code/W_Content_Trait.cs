using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Trait
    {
        internal static void add_traits()
        {
            CW_Asset_Trait trait = new CW_Asset_Trait();
            trait.id = "asylum";
            trait.origin_asset = new ActorTrait();
            trait.origin_asset.id = trait.id;
            trait.origin_asset.path_icon = "ui/Icons/iconTop";
            trait.origin_asset.needs_to_be_explored = false;
            trait.origin_asset.birth = 0f;
            // 效果实现分别在W_Harmony_Actor.actor_killHimself, CW_Actor.__get_hit
            CW_Library_Manager.instance.traits.add(trait);
        }
    }
}
