using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_Building : Building
    {
        public CW_BaseStats cw_cur_stats;
        public CW_BuildingData cw_data;
        public Library.CW_Asset_Building cw_stats;
        public List<CW_Actor> compose_actors;
        public List<CW_Building> compose_buildings;
        /// <summary>
        /// 仅提供高效访问，待权限开放后删除
        /// </summary>
        public BuildingData data;
    }
}
