using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
namespace Cultivation_Way
{
    public class CW_ActorStatus
    {
        public int wakan;
        public int shied;
        public float culti_velo;
        public bool can_culti;
        public float wakan_level;


        public static Func<ActorStatus, HashSet<string>> get_s_traits_ids = CW_ReflectionHelper.create_getter<ActorStatus, HashSet<string>>("s_traits_ids");

    }
}
