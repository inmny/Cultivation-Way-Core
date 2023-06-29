using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Library;

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
    public CW_EnergyMapChunk[,] chunks { get; private set; }

    private CW_EnergyMapChunk[,] _tmp_chunks;

    private static readonly List<KeyValuePair<int, int>> _forward_dirs = new()
    {
        new(0, 1),
        new(1, 0)
    };

    internal void init(int width, int height)
    {
        if (chunks == null || width > 0 || height > 0)
        {
            chunks = new CW_EnergyMapChunk[width, height];
            _tmp_chunks = new CW_EnergyMapChunk[width, height];
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                chunks[i, j].value = 0;
                chunks[i, j].density = 0;
            }
        }
    }

    internal void update(int width, int height)
    {
        float delta_value;
        for (int i = 0; i < width - 1; i++)
        {
            for (int j = 0; j < height - 1; j++)
            {
                foreach (var dir in _forward_dirs)
                {
                    delta_value = energy.get_spread_grad(
                        chunks[i, j].value, chunks[i, j].density,
                        chunks[i + dir.Key, j + dir.Value].value,
                        chunks[i + dir.Key, j + dir.Value].density,
                        World.world.mapChunkManager.map[i, j],
                        World.world.mapChunkManager.map[i + dir.Key, j + dir.Value]
                    );
                    _tmp_chunks[i, j].value += delta_value;
                    _tmp_chunks[i + dir.Key, j + dir.Value].value -= delta_value;
                }
            }
        }

        for (int i = 0; i < width - 1; i++)
        {
            var dir = _forward_dirs[0];
            delta_value = energy.get_spread_grad(
                chunks[i, height - 1].value, chunks[i, height - 1].density,
                chunks[i + dir.Key, height - 1 + dir.Value].value,
                chunks[i + dir.Key, height - 1 + dir.Value].density,
                World.world.mapChunkManager.map[i, height - 1],
                World.world.mapChunkManager.map[i + dir.Key, height - 1 + dir.Value]
            );
            _tmp_chunks[i, height - 1].value += delta_value;
            _tmp_chunks[i + dir.Key, height - 1 + dir.Value].value -= delta_value;
        }

        for (int j = 0; j < height - 1; j++)
        {
            var dir = _forward_dirs[1];
            delta_value = energy.get_spread_grad(
                chunks[width - 1, j].value, chunks[width - 1, j].density,
                chunks[width - 1 + dir.Key, j + dir.Value].value,
                chunks[width - 1 + dir.Key, j + dir.Value].density,
                World.world.mapChunkManager.map[width - 1, j],
                World.world.mapChunkManager.map[width - 1 + dir.Key, j + dir.Value]
            );
            _tmp_chunks[width - 1, j].value += delta_value;
            _tmp_chunks[width - 1 + dir.Key, j + dir.Value].value -= delta_value;
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                chunks[i, j].value = _tmp_chunks[i, j].value;
                chunks[i, j].density = _tmp_chunks[i, j].density;
            }
        }
    }
}

// TODO: 更多注释，懒得写了
public class CW_MapChunkManager
{
    public readonly Dictionary<string, CW_EnergyMap> maps = new();
    public int height;
    public int width;

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