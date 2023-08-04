using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Core;

/// <summary>
///     对区块中的能量进行描述
/// </summary>
public class CW_EnergyMapTile
{
    /// <summary>
    ///     总量
    /// </summary>
    public float value;

    /// <summary>
    ///     评估得到的密度
    /// </summary>
    public float density;

    internal Color32 color;

    internal int x;
    internal int y;

    public void update(EnergyAsset energy_asset)
    {
        density = Mathf.Log(Mathf.Max(value, energy_asset.power_base_value),
            energy_asset.power_base_value);
        Color32 new_color = energy_asset.get_color(value, density);
        if (Math.Abs(new_color.a - color.a) >= 0.02f ||
            Math.Abs(new_color.r - color.r) >= 0.02f ||
            Math.Abs(new_color.g - color.g) >= 0.02f ||
            Math.Abs(new_color.b - color.b) >= 0.02f)
        {
            color = new_color;
            var tiles_to_redraw = CW_Core.mod_state.map_chunk_manager
                .maps[CW_Core.mod_state.map_chunk_manager.current_map_id].tiles_to_redraw;
            Monitor.Enter(tiles_to_redraw);
            tiles_to_redraw.Add(this);
            Monitor.Exit(tiles_to_redraw);
        }
    }
}

/// <summary>
///     某种能量的地图
/// </summary>
public class CW_EnergyMap
{
    /// <summary>
    ///     表示能量的Asset
    /// </summary>
    private EnergyAsset energy;

    public CW_EnergyMap(EnergyAsset energy)
    {
        this.energy = energy;
    }

    /// <summary>
    ///     地图的数组
    /// </summary>
    public CW_EnergyMapTile[,] map { get; private set; }

    private CW_EnergyMapTile[,] _tmp_map;
    internal HashSet<CW_EnergyMapTile> tiles_to_redraw = new();
    internal static bool redraw_lock = false;

    private static readonly List<KeyValuePair<int, int>> _forward_dirs = new()
    {
        new(0, 1),
        new(1, 0)
    };

    internal void init(int width, int height)
    {
        if (map == null || width > 0 || height > 0)
        {
            map = new CW_EnergyMapTile[width, height];
            _tmp_map = new CW_EnergyMapTile[width, height];
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new CW_EnergyMapTile
                {
                    value = Toolbox.randomFloat(0, 10000),
                    density = Toolbox.randomFloat(1, 3),
                    x = x,
                    y = y
                };
                map[x, y].update(energy);
                _tmp_map[x, y] = new CW_EnergyMapTile();
            }
        }
    }

    internal void update(int width, int height)
    {
        if (World.world.worldLaws.dict == null ||
            (World.world.worldLaws.dict.ContainsKey($"{energy.id}_spread_limit") &&
             World.world.worldLaws.dict[$"{energy.id}_spread_limit"].boolVal))
        {
            return;
        }

        float delta_value;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tmp_map[x, y].value = map[x, y].value;
            }
        }

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                foreach (var dir in _forward_dirs)
                {
                    delta_value = energy.get_spread_grad(
                        map[x, y].value, map[x, y].density,
                        map[x + dir.Key, y + dir.Value].value,
                        map[x + dir.Key, y + dir.Value].density,
                        World.world.tilesMap[x, y],
                        World.world.tilesMap[x + dir.Key, y + dir.Value]
                    );
                    _tmp_map[x, y].value += delta_value;
                    _tmp_map[x + dir.Key, y + dir.Value].value -= delta_value;
                }
            }
        }

        for (int x = 0; x < width - 1; x++)
        {
            var dir = _forward_dirs[1];
            delta_value = energy.get_spread_grad(
                map[x, height - 1].value, map[x, height - 1].density,
                map[x + dir.Key, height - 1 + dir.Value].value,
                map[x + dir.Key, height - 1 + dir.Value].density,
                World.world.tilesMap[x, height - 1],
                World.world.tilesMap[x + dir.Key, height - 1 + dir.Value]
            );
            _tmp_map[x, height - 1].value += delta_value;
            _tmp_map[x + dir.Key, height - 1 + dir.Value].value -= delta_value;
        }

        for (int y = 0; y < height - 1; y++)
        {
            var dir = _forward_dirs[0];
            delta_value = energy.get_spread_grad(
                map[width - 1, y].value, map[width - 1, y].density,
                map[width - 1 + dir.Key, y + dir.Value].value,
                map[width - 1 + dir.Key, y + dir.Value].density,
                World.world.tilesMap[width - 1, y],
                World.world.tilesMap[width - 1 + dir.Key, y + dir.Value]
            );
            _tmp_map[width - 1, y].value += delta_value;
            _tmp_map[width - 1 + dir.Key, y + dir.Value].value -= delta_value;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y].value = _tmp_map[x, y].value;
                map[x, y].update(energy);
            }
        }
    }
}

// TODO: 更多注释，懒得写了
public class CW_MapChunkManager
{
    public readonly Dictionary<string, CW_EnergyMap> maps = new();

    /// <summary>
    ///     当前应用的地图ID
    /// </summary>
    public string current_map_id => PlayerConfig.dict[Constants.Core.energy_maps_toggle_name].stringVal;

    public int height;
    public int width;
    internal bool paused = false;

    internal void init(int width, int height)
    {
        this.width = width;
        this.height = height;
        foreach (EnergyAsset energy in Manager.energies.list.Where(energy => energy.is_dissociative))
        {
            maps.Add(energy.id, new CW_EnergyMap(energy));
        }

        foreach (CW_EnergyMap map in maps.Values)
        {
            map.init(width, height);
        }
    }

    internal void reset(int width, int height)
    {
        if (width == this.width && height == this.height)
        {
            foreach (CW_EnergyMap map in maps.Values)
            {
                map.init(0, 0);
            }
        }
        else
        {
            this.width = width;
            this.height = height;
            foreach (CW_EnergyMap map in maps.Values)
            {
                map.init(width, height);
            }
        }
    }

    internal void update_per_year()
    {
        foreach (CW_EnergyMap map in maps.Values)
        {
            map.update(width, height);
        }
    }
}