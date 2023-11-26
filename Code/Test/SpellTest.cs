using Cultivation_Way.Library;
using System.Collections.Generic;
using System.Text;
using Cultivation_Way.Others;

namespace Cultivation_Way.Test;

internal static class SpellTest
{
    public static Dictionary<SpellCheck, KeyValuePair<string, float>[]> SpellCostDict = new();
    public static void LogAllSpellCost()
    {
        StringBuilder sb = new();
        
        HashSet<string> cost_attr_set = new();
        foreach (CW_SpellAsset spell_asset in Manager.spells.list)
        {
            if (spell_asset.spell_cost_action == null) continue;
            
            if(!SpellCostDict.TryGetValue(spell_asset.spell_cost_action, out var cost_list)) continue;
            CW_Core.LogInfo($"------{spell_asset.id}------");
            foreach (KeyValuePair<string, float> key_cost_pair in cost_list)
            {
                CW_Core.LogInfo($"{key_cost_pair.Key}:{key_cost_pair.Value}");
            }
        }
    }
}