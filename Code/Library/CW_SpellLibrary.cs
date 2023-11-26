using System;
using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Others;

namespace Cultivation_Way.Library;

public class CW_SpellAsset : Asset
{
    /// 法术实例管理器取出法术
    ///  
    /// 检查法术释放者是否存活, 或其他合法性检查
    ///       
    /// 调用核心相关函数
    ///           
    /// 若 anim_action 非空, 调用 anim_action
    ///  
    /// 若 spell_action 非空, 调用 spell_action
    /// <summary>
    ///     法术的动画行为, 当 <see cref="anim_type" /> 非 CUSTOM 时, 将在核心与附属完成初始化后自动填写
    /// </summary>
    public SpellAction anim_action;

    /// <summary>
    ///     对应的动画id, 仅当 *action 采用核心提供委托时必填
    /// </summary>
    public string anim_id;

    /// <summary>
    ///     对应的动画类型, 仅当 *action 采用核心提供委托时必填
    /// </summary>
    public SpellAnimType anim_type;


    /// <summary>
    ///     修炼体系要求, 以32位二进制表示, 具体见 <see cref="Constants.CultisysType" />
    ///     <para>当修炼体系完全包含此, 才能够习得该法术</para>
    ///     <para>采用 CultisysType.LIMIT 作为无法习得标记</para>
    ///     <para>为安全起见, 该字段在<see cref="spell_learn_check" />之外生效</para>
    /// </summary>
    internal uint cultisys_require;

    /// <summary>
    ///     法术元素, 用于法术习得概率计算 和 伤害计算(未实现)
    /// </summary>
    public CW_Element element;

    /// <summary>
    ///     法术稀有度, 用于法术习得概率计算, 值越大越稀有
    ///     <para>取值范围:[0,\infty)</para>
    /// </summary>
    public int rarity;

    /// <summary>
    ///     法术的额外行为, 常空
    /// </summary>
    public SpellAction spell_action;


    /// <summary>
    ///     法术修习的等级要求
    /// </summary>
    public List<KeyValuePair<string, int>> spell_cultisys_level_require;

    /// <summary>
    ///     法术消耗行为, 在申请释放法术时调用以检查法术是否可释放
    /// </summary>
    public SpellCheck spell_cost_action;
    /// <summary>
    ///     最低消耗, 在申请释放法术时检查法术是否可释放
    /// </summary>
    public float minimum_cost = 1;

    /// <summary>
    ///     法术修习检查, 在申请修习法术时调用以检查法术修习可行性
    ///     <para>非正数部分表示不可修习, 正数部分表示概率</para>
    /// </summary>
    /// <returns>(-\infty, 1]</returns>
    public SpellCheck spell_learn_check;

    /// <summary>
    ///     法术目标的阵营
    ///     <para>使用场景:法术释放前检查法术合法性; 范围法术在核心*action执行时查找作用目标</para>
    /// </summary>
    public SpellTargetCamp target_camp;

    /// <summary>
    ///     法术目标的类型
    ///     <para>使用场景:法术释放前检查法术合法性; 范围法术在核心*action执行时查找作用目标</para>
    /// </summary>
    public SpellTargetType target_type;

    /// <summary>
    ///     法术触发标签, 以32位的二进制表示, 具体参考 <see cref="SpellTriggerTag" />
    /// </summary>
    private uint _trigger_tags;


    /// <summary>
    ///     添加法术触发标签
    /// </summary>
    /// <param name="tag">添加的标签</param>
    public void add_trigger_tag(SpellTriggerTag tag)
    {
        _trigger_tags |= (uint)tag;
    }

    /// <summary>
    ///     添加一组法术触发标签
    /// </summary>
    /// <param name="tags">可迭代的一组标签</param>
    public void add_trigger_tags(IEnumerable<SpellTriggerTag> tags)
    {
        foreach (SpellTriggerTag tag in tags) add_trigger_tag(tag);
    }

    /// <summary>
    ///     添加修炼体系类型要求
    /// </summary>
    /// <param name="type">添加的修炼类型</param>
    public void add_cultisys_require(CultisysType type)
    {
        cultisys_require |= (uint)type;
    }

    /// <summary>
    ///     添加一组修炼体系类型要求
    /// </summary>
    /// <param name="types">可迭代的一组修炼体系类型要求</param>
    public void add_cultisys_requires(IEnumerable<CultisysType> types)
    {
        foreach (CultisysType cultisys_type in types) add_cultisys_require(cultisys_type);
    }

    /// <summary>
    ///     检查能否修习该法术, 以及修习概率; 同时用于判断法术能否加入功法
    /// </summary>
    /// <param name="actor">检查目标</param>
    public float learn_check(CW_Actor actor, uint given_cultisys = 0)
    {
        if (given_cultisys == 0)
        {
            int[] cultisys_level = actor.data.get_all_cultisys_levels();
            for (int i = 0; i < Manager.cultisys.size; i++)
            {
                if (cultisys_level[i] == -1) continue;

                given_cultisys |= (uint)Manager.cultisys.list[i].type;
            }
        }

        if ((given_cultisys | cultisys_require) != given_cultisys)
        {
            return -1;
        }

        return spell_learn_check(this, actor);
    }

    /// <summary>
    ///     能否在指定情况下触发
    /// </summary>
    /// <param name="tag">指定的触发情况</param>
    public bool can_trigger(SpellTriggerTag tag)
    {
        return (_trigger_tags & (uint)tag) > 0;
    }
}

public class CW_SpellLibrary : CW_Library<CW_SpellAsset>
{
    private readonly Dictionary<uint, List<CW_SpellAsset>> cultisys_spells = new();

    /// <summary>
    ///     将法术按照修炼体系分类
    /// </summary>
    public override void post_init()
    {
        base.post_init();
        uint max_cultisys = 0;
        foreach (CultisysType cultisys in Enum.GetValues(typeof(CultisysType)))
        {
            max_cultisys |= (uint)cultisys;
        }

        for (uint i = 0; i <= max_cultisys; i++)
        {
            cultisys_spells.Add(i, new List<CW_SpellAsset>());
            for (int j = 0; j < size; j++)
            {
                if ((list[j].cultisys_require | i) == i)
                {
                    cultisys_spells[i].Add(list[j]);
                }
            }
        }

        foreach (CW_SpellAsset spell in list)
        {
            spell.spell_cultisys_level_require ??= new List<KeyValuePair<string, int>>();
        }
    }

    /// <summary>
    ///     按照修炼体系类别获取法术
    /// </summary>
    public List<CW_SpellAsset> get_spells_by_cultisys(uint cultisys)
    {
        List<CW_SpellAsset> ret = new();
        ret.AddRange(cultisys_spells[cultisys]);
        return ret;
    }
}