using System.Runtime.CompilerServices;
using Cultivation_Way.Constants;
using Cultivation_Way.Others;
using UnityEngine;

namespace Cultivation_Way.Library;

public class EnergyAsset : Asset
{
    /// <summary>
    ///     基础值, 与其他能量的转换时的汇率
    /// </summary>
    public float base_value = 1;

    /// <summary>
    ///     颜色计算
    /// </summary>
    public EnergyColorCalc color_calc;

    /// <summary>
    ///     扩散计算
    /// </summary>
    public EnergySpreadGradCalc spread_grad_calc;

    /// <summary>
    ///     是否游离, 游离能量将会显示在地图上
    /// </summary>
    public bool is_dissociative = false;

    /// <summary>
    ///     层次提升的幂乘值
    /// </summary>
    public float power_base_value = 100;

    /// <summary>
    ///     类型
    /// </summary>
    public CultisysType type;


    /// <summary>
    ///     获取颜色
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="density">密度</param>
    /// <param name="power_level">层次</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color get_color(float value, float density, float power_level)
    {
        return color_calc?.Invoke(this, value, density, power_level) ?? Color.white;
    }

    /// <summary>
    ///     能量的扩散计算
    /// </summary>
    /// <param name="curr_value">当前区块该能量的量</param>
    /// <param name="curr_density">当前区块该能量的密度</param>
    /// <param name="target_value">相邻一区块该能量的量</param>
    /// <param name="target_density">相邻一区块该能量的密度</param>
    /// <param name="curr_tile">当前区块</param>
    /// <param name="target_tile">相邻一区块</param>
    /// <returns>当前区块能量的增量/相邻区块能量的减量</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float get_spread_grad(
        float curr_value, float curr_density,
        float target_value, float target_density,
        WorldTile curr_tile, WorldTile target_tile
    )
    {
        return spread_grad_calc?.Invoke(
            curr_value, curr_density,
            target_value, target_density,
            curr_tile, target_tile
        ) ?? 0;
    }
}

public class EnergyLibrary : CW_Library<EnergyAsset>
{
    public override void post_init()
    {
        base.post_init();
        foreach (EnergyAsset energy_asset in list)
        {
            if (energy_asset.spread_grad_calc == null)
            {
                Logger.Warn(
                    $"The spread_grad_calc of Energy Asset {energy_asset.id} is null, so it will not spread to other chunks."
                );
            }

            if (energy_asset.color_calc == null)
            {
                Logger.Warn(
                    $"The color_calc of Energy Asset {energy_asset.id} is null, all colors of it will be white."
                );
            }
        }
    }
}