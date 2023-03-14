using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_ActorAsset : Asset
    {
        /// <summary>
        /// 原版的生物信息
        /// </summary>
        public ActorAsset vanllia_asset;
        /// <summary>
        /// 此类生物修炼速度
        /// </summary>
        public float culti_velo;
        /// <summary>
        /// 偏好元素
        /// </summary>
        public int[] prefer_element;
        /// <summary>
        /// 元素偏好系数
        /// </summary>
        public float prefer_element_scale;
        /// <summary>
        /// 自带法术
        /// </summary>
        public List<string> born_spells;
        /// <summary>
        /// 反时间停止（非游戏暂停
        /// </summary>
        public bool anti_time_stop = false;
        /// <summary>
        /// 预设名
        /// </summary>
        public string fixed_name = null;
        /// <summary>
        /// 允许的修炼体系
        /// </summary>
        internal uint allow_cultisys = 0;
        /// <summary>
        /// 强制的修炼体系，或关系
        /// </summary>
        internal uint force_cultisys = 0;
    }
    public class CW_ActorAssetLibrary : CW_Library<CW_ActorAsset>
    {
        internal List<string> added_actors = new List<string>();
    }
}
