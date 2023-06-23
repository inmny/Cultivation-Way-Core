using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content
{
    internal class Content_Constants
    {
        public const string cultisys_prefix = Constants.Core.mod_prefix + "cultisys_";
        public const string immortal_id = cultisys_prefix +"immortal";
        public const int immortal_max_level = 20;
        public const float immortal_max_wakan_regen = 0.6f;
        public const float immortal_base_cultivelo = 12;


        public const string bushido_id = cultisys_prefix + "bushido";
        public const string soul_id = cultisys_prefix + "soul";
        public const int bushido_max_level = 20;
        public const int soul_max_level = 20;
    }
}
