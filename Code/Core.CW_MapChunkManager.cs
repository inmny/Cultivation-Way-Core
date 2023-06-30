using System;
using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Core;

/// <summary>
///     对区块中的能量进行描述
/// </summary>
public struct CW_EnergyMapChunk
{
    /// <summary>
    ///     量
    /// </summary>
    public float value;

    /// <summary>
    ///     密度
    /// </summary>
    public float density;

    internal Color32 color;

    internal bool need_redraw;
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
    public CW_EnergyMapChunk[,] map { get; private set; }

    private CW_EnergyMapChunk[,] _tmp_map;

    private static readonly List<KeyValuePair<int, int>> _forward_dirs = new()
    {
        new(0, 1),
        new(1, 0)
    };

    internal void init(int width, int height)
    {
        if (map == null || width > 0 || height > 0)
        {
            map = new CW_EnergyMapChunk[height, width];
            _tmp_map = new CW_EnergyMapChunk[height, width];
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j].value = Toolbox.randomFloat(0, 1000);
                map[i, j].density = Toolbox.randomFloat(1, 2);
                map[i, j].color = Color.white;
            }
        }
    }

    internal void update(int width, int height)
    {
        float delta_value;
        for (int i = 0; i < height - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                foreach (var dir in _forward_dirs)
                {
                    delta_value = energy.get_spread_grad(
                        map[i, j].value, map[i, j].density,
                        map[i + dir.Key, j + dir.Value].value,
                        map[i + dir.Key, j + dir.Value].density,
                        World.world.tilesMap[i, j],
                        World.world.tilesMap[i + dir.Key, j + dir.Value]
                    );
                    _tmp_map[i, j].value = map[i, j].value + delta_value;
                    _tmp_map[i + dir.Key, j + dir.Value].value =
                        map[i + dir.Key, j + dir.Value].value - delta_value;
                }
            }
        }

        for (int i = 0; i < height - 1; i++)
        {
            var dir = _forward_dirs[1];
            delta_value = energy.get_spread_grad(
                map[i, width - 1].value, map[i, width - 1].density,
                map[i + dir.Key, width - 1 + dir.Value].value,
                map[i + dir.Key, width - 1 + dir.Value].density,
                World.world.tilesMap[i, width - 1],
                World.world.tilesMap[i + dir.Key, width - 1 + dir.Value]
            );
            _tmp_map[i, width - 1].value = map[i, width - 1].value + delta_value;
            _tmp_map[i + dir.Key, width - 1 + dir.Value].value =
                map[i + dir.Key, width - 1 + dir.Value].value - delta_value;
        }

        for (int j = 0; j < width - 1; j++)
        {
            var dir = _forward_dirs[0];
            delta_value = energy.get_spread_grad(
                map[height - 1, j].value, map[height - 1, j].density,
                map[height - 1 + dir.Key, j + dir.Value].value,
                map[height - 1 + dir.Key, j + dir.Value].density,
                World.world.tilesMap[height - 1, j],
                World.world.tilesMap[height - 1 + dir.Key, j + dir.Value]
            );
            _tmp_map[height - 1, j].value = map[height - 1, j].value + delta_value;
            _tmp_map[height - 1 + dir.Key, j + dir.Value].value =
                map[height - 1 + dir.Key, j + dir.Value].value - delta_value;
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j].value = _tmp_map[i, j].value;
                map[i, j].density = Math.Max(1, Mathf.Log(map[i, j].value + 1, energy.power_base_value));
                Color32 new_color = energy.get_color(map[i, j].value, map[i, j].density, 1);
                if (Math.Abs(new_color.a - map[i, j].color.a) >= 0.02f ||
                    Math.Abs(new_color.r - map[i, j].color.r) >= 0.02f ||
                    Math.Abs(new_color.g - map[i, j].color.g) >= 0.02f ||
                    Math.Abs(new_color.b - map[i, j].color.b) >= 0.02f)
                {
                    map[i, j].color = new_color;
                    map[i, j].need_redraw = true;
                }
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