using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Others;
using Cultivation_Way.Constants;
namespace Cultivation_Way.Library
{
    /// <summary>
    /// 修炼体系
    /// </summary>
    public class CultisysAsset : Asset
    {
        /// <summary>
        /// 体系类型
        /// </summary>
        public CultisysType type { get; internal set; }
        /// <summary>
        /// 体系图标路径, 从根目录开始
        /// </summary>
        public string sprite_path;
        /// <summary>
        /// 最大等级
        /// </summary>
        public int max_level;
        /// <summary>
        /// 力量层次, b^l
        /// </summary>
        public float[] power_level { get; internal set; }
        /// <summary>
        /// 力量基数, b^l
        /// </summary>
        public float power_base = 1;
        /// <summary>
        /// 基础属性加成, 单境界, 不累加
        /// </summary>
        public BaseStats[] bonus_stats { get; internal set; }
        /// <summary>
        /// 能否修炼判定
        /// </summary>
        [NonSerialized]
        public CultisysJudge allow;
        /// <summary>
        /// 能否升级判定
        /// </summary>
        [NonSerialized]
        public CultisysJudge level_up;
        public CultisysAsset(string id, CultisysType type, int max_level)
        {
            this.id = id;
            this.type = type;
            this.max_level = max_level;
            power_level = new float[max_level];
            bonus_stats = new BaseStats[max_level];
        }
    }
    public class CultisysLibrary : CW_Library<CultisysAsset>
    {
        /// <summary>
        /// 默认allow和level_up返回false, 默认图标为"ui/Icons/iconCultiBook_immortal"
        /// </summary>
        public override void post_init()
        {
            base.post_init();
            foreach(CultisysAsset cultisys in this.list)
            {
                cultisys.allow ??= (actor, culti) => false;
                cultisys.level_up ??= (actor, culti) => false;
                cultisys.sprite_path ??= "ui/Icons/iconCultiBook_immortal";
            }
        }
    }
}
