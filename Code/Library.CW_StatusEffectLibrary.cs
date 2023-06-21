using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
using Cultivation_Way.Others;

namespace Cultivation_Way.Library
{
    public class CW_StatusEffect : Asset
    {
        /// <summary>
        /// 动画id
        /// </summary>
        public string anim_id;
        /// <summary>
        /// 状态标签, 以32位二进制表示, 具体参考 <see cref="StatusEffectTag"/>
        /// </summary>
        public uint tags;
        /// <summary>
        /// 持续时间, 单位秒, 时间结束后会强制结束动画, 如果动画存在.
        /// </summary>
        public float duration;
        /// <summary>
        /// update_action执行间隔
        /// </summary>
        public float action_interval;
        /// <summary>
        /// 状态的属性加成/减少
        /// </summary>
        public BaseStats bonus_stats = new();
        /// <summary>
        /// 冲突状态, 如果冲突状态已存在, 则该状态不会被添加
        /// </summary>
        public List<string> opposite_statuses = new();
        /// <summary>
        /// 获取状态时执行的action, 可为null
        /// <para>参数1: <see cref="StatusEffectData"/> status_effect 产生的状态实例</para>
        /// <para>参数2: <see cref="BaseSimObject"/> @object1 状态来源, 可能为null</para>
        /// <para>参数3: <see cref="BaseSimObject"/> @object2 状态作用目标</para>
        /// </summary>
        public StatusAction action_on_get;
        /// <summary>
        /// 状态结束时执行的action, 可为null
        /// <para>参数1: <see cref="StatusEffectData"/> status_effect 即将结束的状态实例</para>
        /// <para>参数2: <see cref="BaseSimObject"/> @object1 状态来源, 可能为null</para>
        /// <para>参数3: <see cref="BaseSimObject"/> @object2 状态作用目标</para>
        /// </summary>
        public StatusAction action_on_end;
        /// <summary>
        /// 每action_interval执行的action, 可为null
        /// <para>参数1: <see cref="StatusEffectData"/> status_effect 正在更新的状态实例</para>
        /// <para>参数2: <see cref="BaseSimObject"/> @object1 状态来源, 可能为null</para>
        /// <para>参数3: <see cref="BaseSimObject"/> @object2 状态作用目标</para>
        /// </summary>
        public StatusAction action_on_update;
        /// <summary>
        /// 状态所有者受到攻击时执行的action, 可为null
        /// <para>参数1: <see cref="StatusEffectData"/> status_effect 状态实例</para>
        /// <para>参数2: <see cref="BaseSimObject"/> @object1 攻击来源, 可能为null</para>
        /// <para>参数3: <see cref="BaseSimObject"/> @object2 状态所有者</para>
        /// </summary>
        public StatusAction action_on_get_hit;
        /// <summary>
        /// 状态所有者发起攻击时执行的action, 可为null
        /// <para>参数1: <see cref="StatusEffectData"/> status_effect 状态实例</para>
        /// <para>参数2: <see cref="BaseSimObject"/> @object1 攻击目标</para>
        /// <para>参数3: <see cref="BaseSimObject"/> @object2 状态所有者</para>
        /// </summary>
        public StatusAction action_on_attack;
        /// <summary>
        /// 自定义设置
        /// </summary>
        private readonly BaseSystemData _setting_data = new();

    }
    public class CW_StatusEffectLibrary : CW_Library<CW_StatusEffect>
    {

    }
}
