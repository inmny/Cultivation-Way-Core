using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Extensions;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    public class CW_SpellHelper
    {
		internal static List<List<BaseSimObject>> temp_list_objects_enemies = new List<List<BaseSimObject>>();
		private static Kingdom temp_list_objects_enemies_kingdom;
		private static MapChunk temp_list_objects_enemies_chunk;
        private static int[][] dir_for_find_tiles_in_circle = new int[4][]{
                new int[2]{ 1, 3}, //右上
                new int[2]{ 1, 2}, //右下
                new int[2]{ 0, 3}, //左上
                new int[2]{ 0, 2} //左下
            };
        public static bool is_enemy(BaseSimObject o_1, BaseSimObject o_2)
        {
			if (o_1==null||o_2==null||o_1.kingdom == null || o_2.kingdom == null) return true;
			return ((!o_1.kingdom.asset.mobs && !o_2.kingdom.asset.mobs) || !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && o_2.kingdom.isEnemy(o_1.kingdom);
		}
		public static bool is_enemy(Kingdom k_1, Kingdom k_2)
		{
			if (k_1 == null || k_2 == null) return true;
			return ((!k_1.asset.mobs && !k_2.asset.mobs) || !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && k_2.isEnemy(k_1);
		}
		public static List<List<BaseSimObject>> find_kingdom_enemies_in_chunk(MapChunk pChunk, Kingdom pMainKingdom)
        {
			__find_kingdom_enemies_in_chunk(pChunk, pMainKingdom);
			return temp_list_objects_enemies;
        }
		internal static void __find_kingdom_enemies_in_chunk(MapChunk pChunk, Kingdom pMainKingdom)
		{
			if (pChunk == temp_list_objects_enemies_chunk && pMainKingdom == temp_list_objects_enemies_kingdom) return;

			temp_list_objects_enemies_chunk = pChunk;
			temp_list_objects_enemies_kingdom = pMainKingdom;

			temp_list_objects_enemies.Clear();
            if (pChunk.k_list_objects.Count == 0) return;

            int count = pChunk.k_list_objects.Count;
			for (int i = 0; i < count; i++)
			{
				Kingdom kingdom = pChunk.k_list_objects[i];
				if (is_enemy(kingdom, pMainKingdom))
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
        public static List<BaseSimObject> find_enemies_in_circle(WorldTile center_tile, Kingdom kingdom, int radius)
        {
            return find_enemies_in_tiles(get_circle_tiles(center_tile, radius), kingdom);
        }

        public static List<BaseSimObject> find_enemies_in_tiles(List<WorldTile> tiles, Kingdom kingdom)
        {
            List<BaseSimObject> enemies = new List<BaseSimObject>();
            foreach (WorldTile tile in tiles)
            {
                if (tile.building != null && is_enemy(tile.building.kingdom, kingdom)) enemies.Add(tile.building);
                foreach (Actor unit in tile.units)
                {
                    if (is_enemy(unit.kingdom, kingdom)) enemies.Add(unit);
                }
            }
            return enemies;
        }
        public static List<WorldTile> get_circle_tiles(WorldTile center, float range)
        {
            //改用寻找圆周1/4边界，进行翻转获取
            List<WorldTile> tiles = new List<WorldTile>();
            if (center == null) return tiles;
            //获取边界
            List<int> right = new List<int>();
            int x = (int)range;
            int y = 0;
            float aPerTile = 1f;
            float distance;
            while (y < range)
            {
                distance = Mathf.Sqrt((x * x * aPerTile) + (y * y * aPerTile));
                while (distance >= range)
                {
                    x--;
                    distance = Mathf.Sqrt((x * x * aPerTile) + (y * y * aPerTile));
                }
                right.Add(x);
                y++;
            }
            //添加tile
            //WorldTile.neighbours中0-3对应left,right,down,up
            //确定方向
            
            //添加至tiles，但原点未添加，四条轴各存在一次重复，采用去重，不采用加入时判断
            for (int i = 0; i < 4; i++)
            {
                WorldTile readyToAdd = center;//水平移动用于添加
                WorldTile yLine = center;     //竖直移动，以校准x=0
                for (int yPos = 0; yPos < right.Count; yPos++)
                {
                    for (int xPos = 0; xPos < right[yPos]; xPos++)
                    {
                        tiles.Add(readyToAdd);
                        if (readyToAdd.world_edge)
                        {
                            break;
                        }
                        readyToAdd = readyToAdd.neighbours[dir_for_find_tiles_in_circle[i][0]];
                    }
                    if (yLine.world_edge)
                    {
                        break;
                    }
                    yLine = yLine.neighbours[dir_for_find_tiles_in_circle[i][1]];
                    readyToAdd = yLine;
                }
            }
            //去重
            int rightLim = 1;
            int leftLim = 1;
            int upLim = 1;
            int downLim = 1;
            int centerLim = 3;
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].x == center.x && tiles[i].y == center.y && centerLim > 0)
                {
                    centerLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
                else if (tiles[i].x == center.x)
                {
                    if (tiles[i].y < center.y && downLim > 0)
                    {
                        downLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                    else if (tiles[i].y > center.y && upLim > 0)
                    {
                        upLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                }
                else if (tiles[i].y == center.y)
                {
                    if (tiles[i].x < center.x && leftLim > 0)
                    {
                        leftLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                    else if (tiles[i].x > center.x && rightLim > 0)
                    {
                        rightLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                }
            }
            return tiles;
        }
        public static CW_StatusEffectData add_status_to_target(BaseSimObject user, BaseSimObject target, string status_id, string as_id = null)
        {
            if (target.objectType != MapObjectType.Actor) return null;
            return ((CW_Actor)target).add_status_effect(status_id, as_id, user);
        }
        public static bool cause_damage_to_target(BaseSimObject user, BaseSimObject target, float damage, Others.CW_Enums.CW_AttackType attack_type = Others.CW_Enums.CW_AttackType.Spell, bool ignore_user_alive = true)
        {
			if (target == null || !target.base_data.alive || (!ignore_user_alive && (user == null || !user.base_data.alive))) return false;
			if (target.objectType == MapObjectType.Actor)
			{
				((CW_Actor)target).get_hit(damage, true, attack_type, user, false);
			}
			else if (target.objectType == MapObjectType.Building)
			{
				CW_Building.func_getHit((Building)target, damage, true, (AttackType)attack_type, user, false);
			}
            return true;
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
