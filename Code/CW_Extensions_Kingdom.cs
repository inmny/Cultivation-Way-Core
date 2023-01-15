using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extensions
{
    public static class CW_Extensions_Kingdom
    {
        public static bool isEnemy(this Kingdom kingdom, Kingdom target)
        {
			if (target == null) return true;
			if (kingdom.isCiv() && target.isCiv())
			{
				return target != kingdom && !kingdom.allies.ContainsKey(target);
			}
			return kingdom.asset.isFoe(target.asset);

		}
    }
}
