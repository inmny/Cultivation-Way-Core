using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_Actor : Actor
    {
        public Library.CW_ActorStats cw_stats;
        public CW_BaseStats cw_cur_stats;
        public CW_ActorData cw_data;
        public CW_ActorStatus cw_status;
        public List<CW_Actor> compose_actors;
        public List<CW_Building> compose_buildings;
        /// <summary>
        /// 仅提供高效访问，待权限开放后删除
        /// </summary>
        public ActorStatus data;
    }
}
