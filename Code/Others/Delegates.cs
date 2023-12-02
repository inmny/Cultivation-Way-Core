using System.Collections.Generic;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Others;

/// <summary>
///     <list type="table">
///         <item>
///             <term>参数1</term><description>生物</description>
///         </item>
///         <item>
///             <term>参数2</term><description>需要判定的修炼体系</description>
///         </item>
///     </list>
/// </summary>
public delegate bool CultisysJudge(CW_Actor actor, CultisysAsset cultisys);

/// <summary>
///     获取修炼体系的属性加成
/// </summary>
public delegate BaseStats CultisysStats(CW_Actor actor, CultisysAsset cultisys);

/// <summary>
///     检查actor在cultisys上的相关信息
/// </summary>
public delegate float CultisysCheck(CW_Actor actor, CultisysAsset cultisys, int cultisys_level);

/// <summary>
///     能量的颜色计算
/// </summary>
public delegate Color EnergyColorCalc(EnergyAsset energy, float value, float density);

/// <summary>
///     能量的扩散计算
/// </summary>
/// <param name="curr_value">当前地块该能量的量</param>
/// <param name="curr_density">当前地块该能量的密度</param>
/// <param name="target_value">相邻一地块该能量的量</param>
/// <param name="target_density">相邻一地块该能量的密度</param>
/// <param name="curr_tile">当前地块</param>
/// <param name="target_tile">相邻一地块</param>
/// <returns>当前地块能量的增量/相邻地块能量的减量</returns>
public delegate float EnergySpreadGradCalc(
    float curr_value, float curr_density,
    float target_value, float target_density,
    WorldTile curr_tile, WorldTile target_tile
);

public delegate void EnergyMapInitialize(CW_EnergyMapTile tiles, int x, int y, int width, int height);

/// <summary>
///     动画结束时的委托
/// </summary>
public delegate void AnimEndAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
    Animation.SpriteAnimation anim);

/// <summary>
///     动画帧更新时的委托
/// </summary>
public delegate void AnimFrameAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec,
    Animation.SpriteAnimation anim);

/// <summary>
///     动画的轨迹更新函数
/// </summary>
public delegate void AnimTraceUpdate(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
    ref float delta_x, ref float delta_y);

/// <summary>
///     法术相关行为
/// </summary>
public delegate void SpellAction(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
    WorldTile target_tile, float cost);

/// <summary>
///     法术消耗和修习相关检查, 一般返回负数表示false
/// </summary>
public delegate float SpellCheck(CW_SpellAsset spell_asset, BaseSimObject user);

/// <summary>
///     状态相关行为
/// </summary>
public delegate void StatusAction(CW_StatusEffectData status_effect, BaseSimObject object1, BaseSimObject object2);

/// <summary>
///     丹药效果相关行为, 部分情形下返回值表示丹药效果
/// </summary>
public delegate float ElixirAction(CW_Actor user, Elixir elixir_instance, ElixirAsset elixir_asset);

/// <summary>
///     用于天榜展示评估结果
/// </summary>
public delegate string TopValueShow(object obj);

/// <summary>
///     用于天榜计算评估值
/// </summary>
public delegate float TopValueCalc(object obj);

/// <summary>
///     用于天榜筛选
/// </summary>
public delegate bool TopFilterCheck(object obj);

public delegate void GetCultibookNameParameters(Cultibook pCultibook, CW_Actor pEditor,
    Dictionary<string, string> pParameters);

public delegate void GetCWItemNameParameters(CW_ItemData pItemData, CW_ItemAsset pItemAsset, CW_Actor pCreator,
    Dictionary<string, string> pParameters);