using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal class W_Content_Cultisys
    {
        public static void add_cultisys()
        {
            CW_Asset_CultiSys immortal = CW_Library_Manager.instance.cultisys.add(
                new CW_Asset_CultiSys()
                {
                    id = "immortal",
                    sprite_name = "icon_immortal",
                    judge = immortal_judge
                }
            );

        }
        private static bool immortal_judge(CW_ActorData cw_actor_data)
        {
            return cw_actor_data.status.can_culti;
        }
        private static bool bushido_judge(CW_ActorData cw_actor_data)
        {
            return true;
        }
    }
}
