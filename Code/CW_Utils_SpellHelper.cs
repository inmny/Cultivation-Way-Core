using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Extensions;
namespace Cultivation_Way.Utils
{
    public class CW_SpellHelper
    {
		internal static List<List<BaseSimObject>> temp_list_objects_enemies = new List<List<BaseSimObject>>();
		private static Kingdom temp_list_objects_enemies_kingdom;
		private static MapChunk temp_list_objects_enemies_chunk;
		public static bool is_enemy(BaseSimObject o_1, BaseSimObject o_2)
        {
			if (o_1.kingdom == null || o_2.kingdom == null) return false;
			return ((!o_1.kingdom.asset.mobs && !o_2.kingdom.asset.mobs) || !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && o_2.kingdom.isEnemy(o_1.kingdom);
		}
		public static bool is_enemy(Kingdom k_1, Kingdom k_2)
		{
			if (k_1 == null || k_2 == null) return false;
			return ((!k_1.asset.mobs && !k_2.asset.mobs) || !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && k_2.isEnemy(k_1);
		}
		public static List<List<BaseSimObject>> find_kingdom_enemies_in_chunk(MapChunk pChunk, Kingdom pMainKingdom)
        {
			__find_kingdom_enemies_in_chunk(pChunk, pMainKingdom);
			return temp_list_objects_enemies;
        }
		internal static void __find_kingdom_enemies_in_chunk(MapChunk pChunk, Kingdom pMainKingdom)
		{
			if (pChunk.k_list_objects.Count == 0 || pMainKingdom == null) return;
			if (pChunk == temp_list_objects_enemies_chunk && pMainKingdom == temp_list_objects_enemies_kingdom) return;

			temp_list_objects_enemies_chunk = pChunk;
			temp_list_objects_enemies_kingdom = pMainKingdom;

			temp_list_objects_enemies.Clear();
			int count = pChunk.k_list_objects.Count;
			for (int i = 0; i < count; i++)
			{
				Kingdom kingdom = pChunk.k_list_objects[i];
				if (kingdom != null && ((!kingdom.asset.mobs && !pMainKingdom.asset.mobs) || !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && pMainKingdom.isEnemy(kingdom))
				{
					temp_list_objects_enemies.Add(pChunk.k_dict_objects[kingdom]);
				}
			}
		}
		public static List<BaseSimObject> find_enemies_in_square(WorldTile center_tile, Kingdom kingdom, int edge_length)
        {
			int i, j;
			List<BaseSimObject> list = new List<BaseSimObject>();
            for (i = center_tile.x - edge_length / 2; i <= center_tile.x + edge_length / 2; i++)
            {
				if (i < 0) continue;
				for(j=center_tile.y - edge_length / 2; j <= center_tile.y + edge_length / 2; j++)
				{
					WorldTile tile = MapBox.instance.GetTile(i, j);
					if (tile == null) continue;
					if (tile.building != null && is_enemy(tile.building.kingdom, kingdom)) list.Add(tile.building);
					foreach(Actor unit in tile.units)
                    {
						if(is_enemy(unit.kingdom, kingdom)) list.Add(unit);
                    }
                }
            }
			return list;
        }
		internal static void cause_damage_to_target(BaseSimObject user, BaseSimObject target, float damage)
        {
			if (target == null || !target.base_data.alive || user == null || !user.base_data.alive) return;
			if (target.objectType == MapObjectType.Actor)
			{
				((CW_Actor)target).get_hit(damage, true, Others.CW_Enums.CW_AttackType.Spell, user, true);
			}
			else if (target.objectType == MapObjectType.Building)
			{
				CW_Building.func_getHit((Building)target, damage, true, (AttackType)Others.CW_Enums.CW_AttackType.Spell, user, true);
			}
		}
		public static TileIsland get_random_ground_island(bool pMinRegions = true)
        {
			if (Content.W_Content_Helper.islands_calculator.islands_ground.Count == 0)
			{
				return null;
			}
			if (!pMinRegions)
			{
				return Content.W_Content_Helper.islands_calculator.islands_ground.GetRandom();
			}
			for (int i = 0; i < Content.W_Content_Helper.islands_calculator.islands_ground.Count; i++)
			{
				Content.W_Content_Helper.islands_calculator.islands_ground.ShuffleOne(i);
				if (Content.W_Content_Helper.islands_calculator.islands_ground[i].regions.Count >= 4)
				{
					return Content.W_Content_Helper.islands_calculator.islands_ground[i];
				}
			}
			return null;
		}
	}
}
