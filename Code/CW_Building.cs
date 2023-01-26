using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
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
        public BuildingData fast_data;

        internal static Action<Building, float, bool, AttackType, BaseSimObject, bool> func_getHit = (Action<Building, float, bool, AttackType, BaseSimObject, bool>)CW_ReflectionHelper.get_method<Building>("getHit");
        public static Action<Building, bool> func_startDestroyBuilding = (Action<Building, bool>)CW_ReflectionHelper.get_method<Building>("startDestroyBuilding");
        public void get_hit(float damage, bool flash = true, Others.CW_Enums.CW_AttackType type = Others.CW_Enums.CW_AttackType.Other, BaseSimObject attacker = null, bool skip_if_shake = true)
        {
            func_getHit(this, damage, flash, (AttackType)type, attacker, skip_if_shake);
        }
        internal bool __get_hit(float damage, Others.CW_Enums.CW_AttackType attack_type, BaseSimObject attacker, bool pSkipIfShake)
        {
            throw new NotImplementedException();
            return true;
        }
    }
}
