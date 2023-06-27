using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace Cultivation_Way.Utils;

public static class GeneralHelper
{
    private static readonly int[][] dir_for_find_tiles_in_circle = new int[4][]
    {
        new int[2] { 1, 3 }, //右上
        new int[2] { 1, 2 }, //右下
        new int[2] { 0, 3 }, //左上
        new int[2] { 0, 2 } //左下
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string to_json(object obj)
    {
        //return System.Text.Json.JsonSerializer.Serialize(obj);
        return JsonConvert.SerializeObject(obj);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T from_json<T>(string json)
    {
        //return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    ///     获取以center_tile为中心, radius为半径的圆内的所有WorldTile
    /// </summary>
    public static List<WorldTile> get_tiles_in_circle(WorldTile center_tile, float radius)
    {
        List<WorldTile> tiles = new();
        if (center_tile == null) return tiles;

        #region 寻找圆周1/4边界, 翻转获取整个圆面

        // 获取边界
        List<int> right = new();
        int x = (int)radius;
        int y = 0;
        float aPerTile = 1f;
        float distance;
        while (y < radius)
        {
            distance = Mathf.Sqrt(x * x * aPerTile + y * y * aPerTile);
            while (distance >= radius)
            {
                x--;
                distance = Mathf.Sqrt(x * x * aPerTile + y * y * aPerTile);
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
            WorldTile readyToAdd = center_tile; //水平移动用于添加
            WorldTile yLine = center_tile; //竖直移动，以校准x=0
            foreach (int right_bound in right)
            {
                for (int xPos = 0; xPos < right_bound; xPos++)
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
            if (tiles[i].x == center_tile.x && tiles[i].y == center_tile.y && centerLim > 0)
            {
                centerLim--;
                tiles.RemoveAt(i);
                i--;
            }
            else if (tiles[i].x == center_tile.x)
            {
                if (tiles[i].y < center_tile.y && downLim > 0)
                {
                    downLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
                else if (tiles[i].y > center_tile.y && upLim > 0)
                {
                    upLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
            }
            else if (tiles[i].y == center_tile.y)
            {
                if (tiles[i].x < center_tile.x && leftLim > 0)
                {
                    leftLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
                else if (tiles[i].x > center_tile.x && rightLim > 0)
                {
                    rightLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion

        return tiles;
    }

    public static bool is_enemy(BaseSimObject obj_1, BaseSimObject obj_2)
    {
        if (obj_1 == null || obj_2 == null || obj_1.kingdom == null || obj_2.kingdom == null) return true;
        return ((!obj_1.kingdom.asset.mobs && !obj_2.kingdom.asset.mobs) ||
                !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && obj_2.kingdom.isEnemy(obj_1.kingdom);
    }
}