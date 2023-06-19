using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
using Cultivation_Way.Others;

namespace Cultivation_Way.Library
{
    public class CW_SpellAsset : Asset
    {
        /// <summary>
        /// 法术触发标签, 以32位的二进制表示, 具体参考 Constants.SpellTriggerTag , 0表示核心不提供自动触发
        /// </summary>
        public uint trigger_tags;

        /// <summary>
        /// 对应的动画id, 仅当 *action 采用核心提供委托时必填
        /// </summary>
        public string anim_id;
        /// <summary>
        /// 对应的动画类型, 仅当 *action 采用核心提供委托时必填
        /// </summary>
        public SpellAnimType anim_type;

        /// <summary>
        /// 法术目标的阵营
        /// <para>使用场景:法术释放前检查法术合法性; 范围法术在核心*action执行时查找作用目标</para>
        /// </summary>
        public SpellTargetCamp target_camp;
        /// <summary>
        /// 法术目标的类型
        /// <para>使用场景:法术释放前检查法术合法性; 范围法术在核心*action执行时查找作用目标</para>
        /// </summary>
        public SpellTargetType target_type;

        /**法术实例管理器取出法术
         * 
         *      检查法术释放者是否存活, 或其他合法性检查
         *      
         *      调用核心相关函数
         *          
         *          若 anim_action 非空, 调用 anim_action
         * 
         *          若 spell_action 非空, 调用 spell_action
         *          
         */
        /// <summary>
        /// 法术的动画行为, 当 anim_type 非 CUSTOM 时, 将在核心与附属完成初始化后自动填写
        /// </summary>
        public SpellAction anim_action;
        /// <summary>
        /// 法术的额外行为, 常空
        /// </summary>
        public SpellAction spell_action;
    }
    public class CW_SpellLibrary: CW_Library<CW_SpellAsset>
    {

    }
}
