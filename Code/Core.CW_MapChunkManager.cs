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

    internal void init(int width, int height)
    {
        if (chunks == null || width > 0 || height > 0)
        {
            chunks = new CW_EnergyMapChunk[width, height];
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

    internal void update()
    {
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

    internal void update()
    {
        foreach (CW_EnergyMap map in maps.Values)
        {
            map.update();
        }
    }
}