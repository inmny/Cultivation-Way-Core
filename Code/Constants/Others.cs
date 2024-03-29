﻿namespace Cultivation_Way.Constants;

public static class Others
{
    public const string harmony_id = "harmony.cw.inmny";

    /// <summary>
    ///     对于意料之外的情况是否严格处理
    /// </summary>
    public const bool strict_mode = false;

    public const float common_map_icon_duration = 2f;

    /// <summary>
    ///     状态效果的最大持续时间
    /// </summary>
    public const float max_status_effect_duration = 3600;

    /// <summary>
    ///     继承父母灵根时随机灵根的概率
    /// </summary>
    public const float random_element_when_inherit_chance = 0.3f;

    /// <summary>
    ///     调用newCreature创建的生物是否自动创建血脉
    /// </summary>
    public const bool new_creature_create_blood = true;

    /// <summary>
    ///     功法改良时增加新法术槽的概率
    /// </summary>
    public const float new_spell_slot_for_cultibook = 0.2f;

    /// <summary>
    ///     创造/改良功法要求的等级
    /// </summary>
    public const int create_cultibook_every_level = 5;

    /// <summary>
    ///     功法锁定线(历史最大人数)
    /// </summary>
    public const int cultibook_lock_line = 10000;

    /// <summary>
    ///     血脉锁定线(历史最大人数)
    /// </summary>
    public const int blood_node_lock_line = 1000;

    /// <summary>
    ///     血脉忽略线
    /// </summary>
    public const float blood_ignore_line = 0.05f;

    /// <summary>
    ///     动画对象回收间隔
    /// </summary>
    public const float anim_recycle_interval = 60f;

    /// <summary>
    ///     默认的动画移动速度
    /// </summary>
    public const float default_anim_trace_grad = 2f;

    /// <summary>
    ///     动画最大持续时间
    /// </summary>
    public const float max_anim_time = 600f;

    /// <summary>
    ///     动画最大移动距离
    /// </summary>
    public const float max_anim_trace_length = 1000f;

    /// <summary>
    ///     判定动画移动结束的误差
    /// </summary>
    public const float anim_dst_error = 0.5f;

    /// <summary>
    ///     默认的动画图层, 仅高于人物
    /// </summary>
    public const string default_anim_layer_name = "EffectsTop";
}

/// <summary>
///     修炼体系类型
/// </summary>
public enum CultisysType
{
    /// <summary>
    ///     肉身
    /// </summary>
    BODY = 1,

    /// <summary>
    ///     元神
    /// </summary>
    SOUL = 2,

    /// <summary>
    ///     灵气等外物
    /// </summary>
    WAKAN = 4,

    /// <summary>
    ///     血脉
    /// </summary>
    BLOOD = 8,

    /// <summary>
    ///     隐藏, 不推荐使用
    /// </summary>
    HIDDEN = 16,

    /// <summary>
    ///     用作限制表示非任意一种体系, 不推荐为CultisysAsset的type使用
    /// </summary>
    LIMIT = 32
}

/// <summary>
///     法术动画类型
/// </summary>
public enum SpellAnimType
{
    /// <summary>
    ///     作用于使用单位/点
    /// </summary>
    ON_USER,

    /// <summary>
    ///     作用于目标单位/点
    /// </summary>
    ON_TARGET,

    /// <summary>
    ///     从使用单位移动到目标单位/点
    /// </summary>
    USER_TO_TARGET,

    /// <summary>
    ///     从目标单位/点移动到使用单位/点
    /// </summary>
    TARGET_TO_USER,

    /// <summary>
    ///     自定义
    /// </summary>
    CUSTOM
}

/// <summary>
///     法术触发标签, 决定法术是否可以在某些情况下触发, 每次到达该情况随机获取一个法术，检查是否可以触发，不可触发则跳过
/// </summary>
public enum SpellTriggerTag
{
    /// <summary>
    ///     主动进行攻击
    /// </summary>
    ATTACK = 1,

    /// <summary>
    ///     受到可见单位的攻击
    /// </summary>
    NAMED_DEFEND = 2,

    /// <summary>
    ///     受到规则/不可见单位的攻击
    /// </summary>
    UNNAMED_DEFEND = 4,

    /// <summary>
    ///     在移动时
    /// </summary>
    MOVE = 8,

    /// <summary>
    ///     时常触发
    /// </summary>
    REGULAR = 16
}

/// <summary>
///     法术目标类型, TILE为地块, 大于TILE的为单位/建筑
/// </summary>
public enum SpellTargetType
{
    /// <summary>
    ///     法术作用的中心地块
    /// </summary>
    TILE,

    /// <summary>
    ///     法术作用的中心单位
    /// </summary>
    ACTOR,

    /// <summary>
    ///     法术作用的中心建筑
    /// </summary>
    BUILDING
}

/// <summary>
///     法术目标阵营
/// </summary>
public enum SpellTargetCamp
{
    /// <summary>
    ///     不作用于单位
    /// </summary>
    NONE,

    /// <summary>
    ///     仅作用于友方单位
    /// </summary>
    ALIAS,

    /// <summary>
    ///     仅作用于敌方单位
    /// </summary>
    ENEMY,

    /// <summary>
    ///     作用于所有单位
    /// </summary>
    BOTH
}

/// <summary>
///     状态效果标签
/// </summary>
public enum StatusEffectTag
{
    /// <summary>
    ///     正面
    /// </summary>
    POSITIVE = 1,

    /// <summary>
    ///     负面
    /// </summary>
    NEGATIVE = 2,

    /// <summary>
    ///     束缚
    /// </summary>
    FETTER = 4,

    /// <summary>
    ///     强化
    /// </summary>
    REINFORCE = 8,

    /// <summary>
    ///     弱化
    /// </summary>
    WEAKEN = 16
}

/// <summary>
///     拓展后的攻击类型
/// </summary>
public enum CW_AttackType
{
    Acid,
    Fire,
    Plague,
    Infection,
    Tumor,
    Other,
    AshFever,
    Transformation,
    Hunger,
    Eaten,
    Age,
    Weapon,
    None,
    Poison,
    Block,
    Spell,
    Soul,
    RealSpell,
    RealWeapon,
    God
}

public enum CW_ZoneDisplayMode
{
    None,
    CityBorders,
    Cultures,
    KingdomBorders,
    Clans,
    Alliance,
    Energy
}

// TODO: 补充注释, 懒得写了
public enum ButtonContainerType
{
    INFO,
    DEBUG,
    TOOL,
    RACE,
    ACTOR,
    BUILDING,
    BOSS,
    LIBRARY,
    OTHERS
}

/// <summary>
///     世界法则类型
/// </summary>
public enum WorldLawType
{
    /// <summary>
    ///     开关
    /// </summary>
    SWITCH,

    /// <summary>
    ///     复杂设置
    /// </summary>
    SETTING,

    /// <summary>
    ///     其他
    /// </summary>
    OTHERS
}