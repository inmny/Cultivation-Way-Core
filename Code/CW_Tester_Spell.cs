using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Tester
{
    internal static class CW_Spell_Tester
    {
        private static bool cast_spell(string spell_id, string user_id, string target_id, int x, int y)
        {
            CW_Actor user = MapBox.instance.getActorByID(user_id) as CW_Actor;
            CW_Actor target = MapBox.instance.getActorByID(target_id) as CW_Actor;
            return CW_Spell.cast(spell_id, user, target, MapBox.instance.GetTile(x, y));
        }
        private static void fake_hit(string actor_id, float damage)
        {
            ((CW_Actor)MapBox.instance.getActorByID(actor_id)).get_hit(damage, true, Others.CW_Enums.CW_AttackType.Spell, null, false);

        }
    }
}
