using System.Collections.Generic;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Utils.General.AboutSpell;

public static class LearnChecks
{
    // 无需检查修炼体系, 已在上一级函数learn_check中检查

    public static float default_learn_check(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target)
    {
        int[] cultisys_levels = user.a.data.GetAllCultisysLevels();
        foreach (KeyValuePair<string, int> cultisys_level_require in spell_asset.spell_cultisys_level_require)
        {
            CultisysAsset cultisys = Manager.cultisys.get(cultisys_level_require.Key);
            if (cultisys == null) return -1;
            if (cultisys_levels[cultisys.pid] < cultisys_level_require.Value) return -1;
        }

        return CW_Element.get_similarity(user.a.data.GetElement(), spell_asset.element) / (spell_asset.rarity + 1);
    }
}