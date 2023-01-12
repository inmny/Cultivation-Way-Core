using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
namespace Cultivation_Way
{
	public class CW_ActorStatus
	{
		public int wakan;
		public int shied;
		public float culti_velo;
		public bool can_culti;
		public float wakan_level;
		public int max_age;

		public static Func<ActorStatus, HashSet<string>> get_s_traits_ids = CW_ReflectionHelper.create_getter<ActorStatus, HashSet<string>>("s_traits_ids");
		public static void actorstatus_inheritTraits(ActorStatus origin_status, List<string> traits)
		{
			for (int i = 0; i < traits.Count; i++)
			{
				ActorTrait actorTrait = AssetManager.traits.get(traits[i]);
				if (actorTrait != null && actorTrait.inherit != 0f)
				{
					if (actorTrait.inherit >= Toolbox.randomFloat(0f, 100f) && !origin_status.traits.Contains(actorTrait.id) && !actorstatus_haveOppositeTrait(origin_status, actorTrait)) origin_status.addTrait(actorTrait.id);
				}
			}
		}
		public static bool actorstatus_haveOppositeTrait(ActorStatus origin_status, ActorTrait pTraitMain)
		{
			if (pTraitMain == null || pTraitMain.oppositeArr == null) return false;
			for (int i = 0; i < pTraitMain.oppositeArr.Length; i++)
			{
				if (origin_status.traits.Contains(pTraitMain.oppositeArr[i])) return true;
			}
			return false;
		}
		public static void actorstatus_updateAttributes(ActorStatus origin_status, ActorStats pStats, Race pRace, bool pForce = false)
		{
			if (!pStats.unit) return;
			if ((origin_status.age % 3 == 0 && origin_status.age <= 100) || pForce)
			{
                switch (pRace.preferred_attribute.GetRandom())
                {
					case "intelligence":origin_status.intelligence++;	return;
					case "diplomacy":	origin_status.diplomacy++;		return;
					case "warfare":		origin_status.warfare++;		return;
					case "stewardship":	origin_status.stewardship++;	return;
					default: return;
				}
			}
		}
	}
}
