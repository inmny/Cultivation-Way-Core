using Cultivation_Way.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.General.AboutSpell
{
    /// <summary>
    /// 提供一般法术的spell_cost_action
    /// </summary>
    public static class CostChecks
    {
        public static SpellCheck generate_cost_check_action(KeyValuePair<string, float>[] cost_list){
            return (SpellCheck)delegate (SpellAsset spell_asset, BaseSimObject user)
            {
                
            };
        }
    }
}
