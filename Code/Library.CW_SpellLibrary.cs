using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Others;

namespace Cultivation_Way.Library
{
    public class CW_SpellAsset : Asset
    {
        /// <summary>
        /// 法术触发标签, 以32位的二进制表示, 具体参考 <see cref="SpellTriggerTag"/>
        /// </summary>
        private uint trigger_tags;

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
        /// 法术的动画行为, 当 <see cref="anim_type"/> 非 CUSTOM 时, 将在核心与附属完成初始化后自动填写
        /// </summary>
        public SpellAction anim_action;
        /// <summary>
        /// 法术的额外行为, 常空
        /// </summary>
        public SpellAction spell_action;


        /// <summary>
        /// 法术消耗, 在核心*action执行时调用以计算法术消耗
        /// </summary>
        public float spell_cost;
        /// <summary>
        /// 法术消耗行为, 在申请释放法术时调用以检查法术是否可释放
        /// </summary>
        public SpellCheck spell_cost_action;


        /// <summary>
        /// 修炼体系要求, 以32位二进制表示, 具体见 <see cref="Constants.CultisysType"/>
        /// <para>当修炼体系完全包含此, 才能够习得该法术</para>
        /// <para>采用 CultisysType.LIMIT 作为无法习得标记</para>
        /// <para>为安全起见, 该字段在<see cref="spell_learn_check"/>之外生效</para>
        /// </summary>
        public uint cultisys_require;
        /// <summary>
        /// 法术稀有度, 用于法术习得概率计算, 值越大越稀有
        /// <para>取值范围:[0,\infty)</para>
        /// </summary>
        public int rarity;
        /// <summary>
        /// 法术元素, 用于法术习得概率计算 和 伤害计算(未实现)
        /// </summary>
        public CW_Element element;
        /// <summary>
        /// 法术修习检查, 在申请修习法术时调用以检查法术修习可行性
        /// <para>非正数部分表示不可修习, 正数部分表示概率</para>
        /// </summary>
        /// <returns>(-\infty, 1]</returns>
        public SpellCheck spell_learn_check;
    }
    public class CW_SpellLibrary: CW_Library<CW_SpellAsset>
    {

    }
}
