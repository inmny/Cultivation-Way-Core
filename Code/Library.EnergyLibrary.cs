using Cultivation_Way.Constants;
using Cultivation_Way.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Library
{
    public class EnergyAsset : Asset
    {
        /// <summary>
        /// 类型
        /// </summary>
        public CultisysType type;
        /// <summary>
        /// 基础值, 与其他能量的转换时的汇率
        /// </summary>
        public float base_value = 1;
        /// <summary>
        /// 层次提升的幂乘值
        /// </summary>
        public float power_base_value = 100;
        /// <summary>
        /// 是否游离, 游离能量将会显示在地图上
        /// </summary>
        public bool is_dissociative = false;
        /// <summary>
        /// 颜色计算
        /// </summary>
        public EnergyColorCalc color_calc;
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="density">密度</param>
        /// <param name="power_level">层次</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color get_color(float value, float density, float power_level)
        {
            return color_calc==null? Color.white: color_calc(this, value, density, power_level);
        }
    }
    public class EnergyLibrary : CW_Library<EnergyAsset>
    {

    }
}
