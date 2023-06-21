using Cultivation_Way.Core;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Others
{
    /// <summary>
    /// <list type="table">
    /// <item> <term>参数1</term><description>生物</description></item>
    /// <item> <term>参数2</term><description>需要判定的修炼体系</description></item>
    /// </list>
    /// </summary>
    public delegate bool CultisysJudge(CW_Actor actor, CultisysAsset cultisys);

    /// <summary>
    /// 动画结束时的委托
    /// </summary>
    public delegate void AnimEndAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim);
    /// <summary>
    /// 动画帧更新时的委托
    /// </summary>
    public delegate void AnimFrameAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim);
    /// <summary>
    /// 动画的轨迹更新函数
    /// </summary>
    public delegate void AnimTraceUpdate(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim, ref float delta_x, ref float delta_y);

    /// <summary>
    /// 法术相关行为
    /// </summary>
    public delegate void SpellAction(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target, WorldTile target_tile, float cost);
    /// <summary>
    /// 法术消耗和修习相关检查, 一般返回负数表示false
    /// </summary>
    public delegate float SpellCheck(CW_SpellAsset spell_asset, BaseSimObject user);

    /// <summary>
    /// 状态相关行为
    /// </summary>
    public delegate void StatusAction(CW_StatusEffectData status_effect, BaseSimObject @object1, BaseSimObject @object2);
}
