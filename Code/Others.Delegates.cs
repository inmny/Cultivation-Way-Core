using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Core;
using Cultivation_Way.Library;

namespace Cultivation_Way.Others
{
    /// <summary>
    /// <list type="table">
    /// <item> <term>参数1</term><description>生物</description></item>
    /// <item> <term>参数2</term><description>需要判定的修炼体系</description></item>
    /// </list>
    /// </summary>
    public delegate bool CultisysJudge(CW_Actor actor, CultisysAsset cultisys);
}
