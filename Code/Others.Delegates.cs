using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
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

    public delegate void AnimEndAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim);
    public delegate void AnimFrameAction(int cur_frame_idx, ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim);
    public delegate void AnimTraceUpdate(ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim, ref float delta_x, ref float delta_y);
}
