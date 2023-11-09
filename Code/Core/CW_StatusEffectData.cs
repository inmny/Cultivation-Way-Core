using Cultivation_Way.Library;

namespace Cultivation_Way.Core;

public class CW_StatusEffectData
{
    /// <summary>
    ///     action_on_update计时器
    /// </summary>
    internal float _update_action_timer;

    /// <summary>
    ///     该状态的动画, 可能为null
    /// </summary>
    public Animation.SpriteAnimation anim;

    /// <summary>
    ///     属性加成, 修改时注意深拷贝
    /// </summary>
    public BaseStats bonus_stats;

    /// <summary>
    ///     结束标记, 准备回收
    /// </summary>
    public bool finished;

    /// <summary>
    ///     id
    /// </summary>
    public string id;

    /// <summary>
    ///     剩余时间
    /// </summary>
    public float left_time;

    /// <summary>
    ///     该状态的施加者, 可能为null
    /// </summary>
    public BaseSimObject source;

    /// <summary>
    ///     Asset访问
    /// </summary>
    public CW_StatusEffect status_asset;

    /// <summary>
    ///     效果值, 一个自由变量
    /// </summary>
    public float effect_val;

    /// <summary>
    ///     创建状态数据, 注意动画需要自行创建
    /// </summary>
    /// <param name="status_asset"></param>
    /// <param name="source"></param>
    public CW_StatusEffectData(CW_StatusEffect status_asset, BaseSimObject source)
    {
        this.status_asset = status_asset;
        this.source = source;
        id = status_asset.id;
        left_time = status_asset.duration;
        bonus_stats = status_asset.bonus_stats;
        anim = null;
        finished = false;
        _update_action_timer = 0;
    }

    internal void update_timer(float delta_time)
    {
        if (finished)
            return;
        if (_update_action_timer > 0)
        {
            _update_action_timer -= delta_time;
        }

        left_time -= delta_time;
        if (left_time <= 0)
        {
            left_time = 0;
            finished = true;
        }

        if (finished && anim != null)
        {
            anim.force_stop(true);
        }
    }
}