using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutSpell;

namespace Cultivation_Way.Content
{
    internal static class Spell
    {
        public static void init()
        {
            add_track_projectile_spells();
        }

        private static void add_track_projectile_spells()
        {
            // 金剑
            FormatSpells.create_track_projectile_spell(
                id: "gold_sword",
                anim_id: "single_gold_sword_anim", anim_path: "effects/single_gold_sword",
                rarity: 3,
                spell_cost_list: new KeyValuePair<string, float>[]
                {
                    new(DataS.wakan, 30f) 
                }
            );
        }
    }
}
