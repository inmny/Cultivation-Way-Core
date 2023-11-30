using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Cultivation_Way.Constants;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

    private static void SelfUpload(string changelog)
    {
        var mod_info = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(
            File.ReadAllText(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath, "mod.json")));
        string version = (string)mod_info["version"];
        mod_info["version"] = next_version(version);
        File.WriteAllText(Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath, "mod.json"),
            Newtonsoft.Json.JsonConvert.SerializeObject(mod_info, Formatting.Indented));

        Type type = AccessTools.TypeByName("ModWorkshopService");
        MethodInfo method = type.GetMethod("TryEditMod");
        method.Invoke(null, new object[] { 3072913057, CW_Core.Instance, changelog });
        
        return;

        string next_version(string pVersion)
        {
            string[] version = pVersion.Split('.');
            return $"{version[0]}.{version[1]}.{int.Parse(version[2]) + 1}";
        }
    }
    static JsonSerializerSettings private_members_visit_settings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver()
        {
            // 反正不改版本, 就用这个吧
#pragma warning disable 618
            DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
#pragma warning restore 618
        }
    };
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string to_json(object obj, bool private_members_included = false)
    {
        if (private_members_included)
        {
            return JsonConvert.SerializeObject(obj, private_members_visit_settings);
        }
        return JsonConvert.SerializeObject(obj);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T from_json<T>(string json, bool private_members_included = false)
    {
        //return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        if (private_members_included)
        {
            return JsonConvert.DeserializeObject<T>(json, private_members_visit_settings);
        }
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    ///     获取以center_tile为中心, radius为半径的圆内的所有WorldTile
    /// </summary>
    public static List<WorldTile> get_tiles_in_circle(WorldTile center_tile, float radius)
    {
        List<WorldTile> tiles = new();
        if (radius <= 0 || center_tile == null)
        {
            tiles.Add(center_tile);
            return tiles;
        }

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

    /// <summary>
    ///     获取以center_tile为中心, radius为半径的圆内的所有WorldTile上的敌人
    /// </summary>
    public static List<BaseSimObject> find_enemies_in_circle(WorldTile center_tile, Kingdom kingdom, float radius,
        bool building_included = true)
    {
        List<WorldTile> tiles = get_tiles_in_circle(center_tile, radius);
        List<BaseSimObject> enemies = new();
        foreach (WorldTile tile in tiles)
        {
            if (building_included && tile.building != null && is_enemy(tile.building.kingdom, kingdom))
                enemies.Add(tile.building);
            enemies.AddRange(tile._units.Where(actor => is_enemy(actor.kingdom, kingdom)));
        }

        return enemies;
    }

    /// <summary>
    ///     获取以center_tile为中心, 向四个方向延伸half_edge距离的方形区域中所有WorldTile
    /// </summary>
    public static List<WorldTile> get_tiles_in_square(WorldTile center_tile, float half_edge)
    {
        List<WorldTile> tiles = new();
        if (half_edge <= 0 || center_tile == null)
        {
            tiles.Add(center_tile);
            return tiles;
        }

        int rt_x = (int)(center_tile.x + half_edge);
        int rt_y = (int)(center_tile.y + half_edge);

        int edge = 2 * (int)half_edge;

        for (int x = rt_x - edge; x <= rt_x; x++)
        {
            for (int y = rt_y - edge; y <= rt_y; y++)
            {
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    tiles.Add(tile);
                }
            }
        }

        return tiles;
    }

    /// <summary>
    ///     获取以center_tile为中心, 向四个方向延伸half_edge距离的方形区域中所有WorldTile上的敌人
    /// </summary>
    public static List<BaseSimObject> find_enemies_in_square(WorldTile center_tile, Kingdom kingdom, float half_edge,
        bool building_included = true)
    {
        List<WorldTile> tiles = get_tiles_in_square(center_tile, half_edge);
        List<BaseSimObject> enemies = new();
        foreach (WorldTile tile in tiles)
        {
            if (building_included && tile.building != null && is_enemy(tile.building.kingdom, kingdom))
                enemies.Add(tile.building);
            enemies.AddRange(tile._units.Where(actor => is_enemy(actor.kingdom, kingdom)));
        }

        return enemies;
    }

    public static void damage_to_tiles(List<WorldTile> tiles, float damage, BaseSimObject attacker,
        CW_AttackType attack_type)
    {
        foreach (WorldTile tile in tiles)
        {
            if (tile.building != null && is_enemy(tile.building, attacker))
                tile.building.getHit(damage, pType: (AttackType)attack_type, pAttacker: attacker);
            foreach (Actor actor in tile._units)
            {
                if (!is_enemy(actor, attacker)) continue;
                actor.getHit(damage, pAttackType: (AttackType)attack_type, pAttacker: attacker);
            }
        }
    }

    public static bool is_enemy(Kingdom k1, Kingdom k2)
    {
        if (k1 == null || k2 == null) return true;
        return ((!k1.asset.mobs && !k2.asset.mobs) ||
                !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && k2.isEnemy(k1);
    }

    public static bool is_enemy(BaseSimObject obj_1, BaseSimObject obj_2)
    {
        if (obj_1 == null || obj_2 == null || obj_1.kingdom == null || obj_2.kingdom == null) return true;
        return ((!obj_1.kingdom.asset.mobs && !obj_2.kingdom.asset.mobs) ||
                !MapBox.instance.worldLaws.world_law_peaceful_monsters.boolVal) && obj_2.kingdom.isEnemy(obj_1.kingdom);
    }
}