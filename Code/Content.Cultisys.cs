using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal static class Cultisys
    {
        public static void init()
        {
            add_immortal();
            add_bushido();
            add_soul_cultisys();
        }
        private static void add_immortal()
        {
            int max_level = 20;
            CultisysAsset cultisys = new("cw_cultisys_immortal", Constants.CultisysType.WAKAN, max_level)
            {
                sprite_path = "ui/Icons/iconCultiBook_immortal",
                curr_progress = (actor, culti, level) => { 
                    actor.data.get(Constants.DataS.wakan, out float wakan, 0);
                    return wakan; 
                },
                max_progress = (actor, culti, level) => actor.stats[Constants.CW_S.wakan]
            };
            for(int i=0; i< max_level; i++)
            {
                cultisys.power_level[i] = 1+i*0.1f;
            }
            Library.Manager.cultisys.add(cultisys);
        }
        private static void add_bushido()
        {
            int max_level = 20;
            CultisysAsset cultisys = new("cw_cultisys_bushido", Constants.CultisysType.BODY, max_level)
            {
                sprite_path = "ui/Icons/iconCultiBook_bushido"
            };
            for (int i = 0; i < max_level; i++)
            {
                cultisys.power_level[i] = 1 + i * 0.1f;
            }
            Library.Manager.cultisys.add(cultisys);
        }
        private static void add_soul_cultisys()
        {
            int max_level = 20;
            CultisysAsset cultisys = new("cw_cultisys_soul", Constants.CultisysType.SOUL, max_level)
            {
                sprite_path = "ui/Icons/iconCultiBook_immortal"
            };
            for (int i = 0; i < max_level; i++)
            {
                cultisys.power_level[i] = 1 + i * 0.1f;
            }
            Library.Manager.cultisys.add(cultisys);
        }
    }
}
