using System;
using Cultivation_Way.Extension;
using NeoModLoader.api.attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cultivation_Way.Animation;

/// <summary>
///     自定义的动画, 与原版SpriteAnimation同名
/// </summary>
public class SpriteAnimation
{
    /// <summary>
    ///     本次更新间隔
    /// </summary>
    public float cur_elapsed;

    /// <summary>
    ///     当前帧
    /// </summary>
    public int cur_frame_idx;

    /// <summary>
    ///     更多的数据
    /// </summary>
    public BaseSystemData data;

    /// <summary>
    ///     目标对象
    /// </summary>
    public BaseSimObject dst_object;

    /// <summary>
    ///     目标位置
    /// </summary>
    public Vector2 dst_vec;

    /// <summary>
    ///     结束后冻结时间
    /// </summary>
    public float end_froze_time;

    /// <summary>
    ///     组件
    /// </summary>
    public GameObject gameObject;

    /// <summary>
    ///     是否已经到达终点
    /// </summary>
    public bool has_end;

    /// <summary>
    ///     是否为默认图像集
    /// </summary>
    internal bool is_default_sprites;

    /// <summary>
    ///     运行状态
    /// </summary>
    internal bool isOn;

    /// <summary>
    ///     循环次数
    /// </summary>
    public int loop_nr;

    /// <summary>
    ///     距离下一帧剩余时间
    /// </summary>
    public float next_frame_time;

    /// <summary>
    ///     运行时间
    /// </summary>
    public float play_time;

    /// <summary>
    ///     渲染器
    /// </summary>
    public SpriteRenderer renderer;

    /// <summary>
    ///     动画设置
    /// </summary>
    internal AnimationSetting setting;

    /// <summary>
    ///     图像集
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    ///     起始对象
    /// </summary>
    public BaseSimObject src_object;

    /// <summary>
    ///     起始位置
    /// </summary>
    public Vector2 src_vec;

    /// <summary>
    ///     路径长度
    /// </summary>
    public float trace_length;

    internal SpriteAnimation(AnimationSetting setting, Sprite[] sprites, GameObject prefab, Vector2 src_vec,
        Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
    {
        isOn = true;
        is_default_sprites = true;
        data = new BaseSystemData();
        this.setting = setting;

        this.sprites = sprites;

        gameObject = Object.Instantiate(prefab, EffectManager.instance.transform);
        gameObject.layer = 0;
        default_scale = gameObject.transform.localScale.x;
        renderer = gameObject.GetComponent<SpriteRenderer>();

        apply_setting(src_vec, dst_vec, src_object, dst_object);
    }

    /// <summary>
    ///     创建时设定的大小
    /// </summary>
    public float default_scale { get; private set; }

    // TODO: complete it
    internal void apply_setting(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
    {
        this.src_vec = src_vec;
        this.dst_vec = dst_vec;
        this.src_object = src_object;
        this.dst_object = dst_object;
        if (setting.trace_type == AnimationTraceType.TRACK)
        {
            if (dst_object != null)
            {
                this.dst_vec = dst_object.currentPosition;
            }
        }

        //WorldBoxConsole.Console.print("Is renderer null?>" + (renderer == null)+"\nIs setting null?>"+(setting==null));
        renderer.sortingLayerName = setting.layer_name;

        gameObject.transform.localPosition = this.src_vec;

        if (setting.point_to_dst)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f,
                Toolbox.getAngle(gameObject.transform.position.x, gameObject.transform.position.y, this.dst_vec.x,
                    this.dst_vec.y) * 57.29578f));
    }

    internal void set(AnimationSetting setting, Sprite[] sprites, GameObject prefab, Vector2 src_vec, Vector2 dst_vec,
        BaseSimObject src_object, BaseSimObject dst_object)
    {
        isOn = true;
        is_default_sprites = true;
        this.setting = setting;

        this.sprites = sprites;

        gameObject.transform.localScale = prefab.transform.localScale;

        apply_setting(src_vec, dst_vec, src_object, dst_object);

        //this.gameObject.SetActive(true);
        show();
    }

    internal void clear()
    {
        hide();
        gameObject.transform.localPosition = new Vector3(-9999999, -999999, -9999);
        //this.gameObject.SetActive(false);
        cur_frame_idx = 0;
        dst_object = null;
        dst_vec.x = 0;
        dst_vec.y = 0;
        end_froze_time = -1;
        has_end = false;
        loop_nr = 0;
        next_frame_time = 0;
        play_time = 0;
        src_object = null;
        src_vec.x = 0;
        src_vec.y = 0;
        trace_length = 0;
        data.Clear();
    }

    private void hide()
    {
        //this.gameObject.layer
        renderer.enabled = false;
        //renderer.sprite = null;
        gameObject.layer = 3;
        //this.gameObject.transform.localPosition = new Vector3(-100000, -10000, -10000);
    }

    private void show()
    {
        gameObject.layer = 0;
        renderer.enabled = true;
        //renderer.sprite = sprites[cur_frame_idx];
    }

    [Hotfixable]
    internal void update(float elapsed)
    {
        if (!isOn)
        {
            return;
        }

        if (renderer.enabled &&
            ((!setting.visible_in_low_res && EffectManager.instance.low_res) || IsOutScreen())) hide();
        if (!renderer.enabled && setting.visible_in_low_res && !EffectManager.instance.low_res &&
            !IsOutScreen()) show();

        //if (!CW_EffectManager.instance.low_res && renderer.sprite == null) renderer.sprite = sprites[cur_frame_idx];
        play_time += elapsed;
        cur_elapsed = elapsed;
        if (play_time > Constants.Others.max_anim_time)
        {
            isOn = false;
            return;
        }

        if (has_end)
        {
            end_froze_time += elapsed;
            if (end_froze_time < setting.froze_time_after_end) return;
            isOn = false;
            return;
        }

        if (elapsed < next_frame_time)
        {
            next_frame_time -= elapsed;
            if (Config.timeScale >= 3)
            {
                return;
            }
        }
        else
        {
            next_frame_time += setting.frame_interval; // TODO: 可能直接设置为frame_interval更合理

            // 切换至下一帧图像
            /* Original Version
            int change = setting.play_direction == AnimationPlayDirection.FORWARD ? 1 : -1;
            if (setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) change = 0-change;
            */
            if (cur_frame_idx != setting.anim_froze_frame_idx)
            {
                int change = (setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) ^
                             (setting.play_direction == AnimationPlayDirection.FORWARD)
                    ? 1
                    : -1;
                int next_frame_idx = (cur_frame_idx + change + sprites.Length) % sprites.Length;
                if (renderer.enabled)
                    renderer.sprite = sprites[next_frame_idx];
                cur_frame_idx = next_frame_idx;
                if (cur_frame_idx == 0) loop_nr++;
            }
            else
            {
                loop_nr++;
            }
        }

        //始终旋转
        if (setting.always_roll)
        {
            gameObject.transform.Rotate(setting.always_roll_axis * (elapsed * setting.roll_angle_per_frame));
        }

        // 检测到目标不存在后停止
        /*
        if (setting.trace_type == AnimationTraceType.TRACK && dst_object == null)
        {
            isOn = false;
            return;
        }
        */
        // 轨迹更新
        if (setting.trace_type != AnimationTraceType.NONE)
        {
            float delta_x = 0;
            float delta_y = 0;

            if (setting.trace_type == AnimationTraceType.TRACK)
            {
                var position = gameObject.transform.position;
                Vector2 tmp_src_vec = new(position.x, position.y);
                if (dst_object != null)
                {
                    dst_vec = dst_object.currentPosition;
                }

                setting.trace_updater(ref tmp_src_vec, ref dst_vec, this, ref delta_x, ref delta_y);
            }
            else if (setting.trace_type == AnimationTraceType.ATTACH)
            {
                if (dst_object == null || !dst_object.isAlive())
                {
                    isOn = false;
                    return;
                }

                gameObject.transform.position = dst_object.currentPosition;
                goto NO_TRACE_COMP;
            }
            else
            {
                setting.trace_updater(ref src_vec, ref dst_vec, this, ref delta_x, ref delta_y);
            }

            delta_x *= elapsed;
            delta_y *= elapsed;
            var curr_pos = gameObject.transform.position;
            float next_x = curr_pos.x + delta_x;
            float next_y = curr_pos.y + delta_y;
            //WorldBoxConsole.Console.print(string.Format("delta x:{0},y:{1}", delta_x, delta_y));
            if (setting.loop_limit_type == AnimationLoopLimitType.TRACE_LIMIT ||
                setting.loop_limit_type == AnimationLoopLimitType.DST_LIMIT)
                trace_length += Mathf.Sqrt(delta_x * delta_x + delta_y * delta_y);

            // 指向终点
            if (setting.always_point_to_dst)
            {
                var position = gameObject.transform.position;
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f,
                    Toolbox.getAngle(position.x, position.y, dst_vec.x,
                        dst_vec.y) * 57.29578f));
            }

            gameObject.transform.position = new Vector3(next_x, next_y, next_y);
        }

        NO_TRACE_COMP:
        // 路径行为
        setting.frame_action?.Invoke(cur_frame_idx, ref src_vec, ref dst_vec, this);

        // 按照设置进行判断是否结束
        bool end = false;
        switch (setting.loop_limit_type)
        {
            case AnimationLoopLimitType.NUMBER_LIMIT:
            {
                if (loop_nr >= setting.loop_nr_limit) end = true;
                break;
            }
            case AnimationLoopLimitType.TIME_LIMIT:
            {
                if (play_time >= setting.loop_time_limit) end = true;
                break;
            }
            case AnimationLoopLimitType.DST_LIMIT:
            {
                if (Toolbox.Dist(dst_vec.x, dst_vec.y, gameObject.transform.position.x,
                        gameObject.transform.position.y) < Constants.Others.anim_dst_error ||
                    trace_length >= Constants.Others.max_anim_trace_length) end = true;
                break;
            }
            case AnimationLoopLimitType.TRACE_LIMIT:
            {
                if (trace_length >= setting.loop_trace_limit) end = true;
                break;
            }
            case AnimationLoopLimitType.NO_LIMIT:
            {
                break;
            }
            default:
            {
                throw new Exception("Unexpected Animation Loop Limit Type");
            }
        }

        if (setting.stop_frame_idx == cur_frame_idx) end = true;
        if (end)
        {
            has_end = true;
            if (setting.end_action != null) setting.end_action(cur_frame_idx, ref src_vec, ref dst_vec, this);
            if (end_froze_time >= setting.froze_time_after_end + 0.01f) isOn = false;
        }
    }

    [Hotfixable]
    private bool IsOutScreen()
    {
        return false;
        var position = gameObject.transform.position;

        return position.x > EffectManager.camera_range_1.x && position.x < EffectManager.camera_range_2.x &&
               position.y > EffectManager.camera_range_1.y && position.y < EffectManager.camera_range_2.y;
    }

    internal void kill()
    {
        Object.Destroy(gameObject, 5);
    }

    /// <summary>
    ///     设置大小, 相较于预制体的大小
    /// </summary>
    /// <param name="scale">大小系数</param>
    public void set_scale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale * default_scale, scale * default_scale,
            gameObject.transform.localScale.z);
    }

    /// <summary>
    ///     缩放
    /// </summary>
    /// <param name="scale">缩放比例</param>
    public void change_scale(float scale)
    {
        var local_scale = gameObject.transform.localScale;
        local_scale = new Vector3(local_scale.x * scale,
            local_scale.y * scale, local_scale.z);
        gameObject.transform.localScale = local_scale;
    }

    /// <summary>
    ///     获取当前localScale
    /// </summary>
    /// <returns></returns>
    public float get_scale()
    {
        return gameObject.transform.localScale.x;
    }

    /// <summary>
    ///     设置绝对位置
    /// </summary>
    /// <param name="pos"></param>
    public void set_position(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    /// <summary>
    ///     以输入进行偏移
    /// </summary>
    /// <param name="offset"></param>
    public void offset(Vector2 offset)
    {
        renderer.transform.localPosition = new Vector3(offset.x + renderer.transform.localPosition.x,
            offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);
    }

    /// <summary>
    ///     修改动画设置，请在知道你在做什么的情况下谨慎操作
    /// </summary>
    /// <param name="setting">新的动画设置</param>
    /// <param name="deepcopy">是否进行深拷贝，默认是</param>
    public void change_setting(AnimationSetting setting, bool deepcopy = true)
    {
        if (deepcopy)
        {
            this.setting = setting.__deepcopy();
            this.setting.possible_referenced = false;
        }
        else
        {
            this.setting = setting;
            this.setting.possible_referenced = true;
        }

        apply_setting(src_vec, dst_vec, src_object, dst_object);
    }

    /// <summary>
    ///     设置透明度
    /// </summary>
    /// <param name="alpha">透明度(0,1)</param>
    public void set_alpha(float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    /// <summary>
    ///     获取动画设置原本/拷贝，请在知道你在做什么的情况下谨慎操作
    /// </summary>
    /// <param name="safety">是否保证安全，此处安全是指是否与其他处共用setting对象</param>
    /// <returns>动画设置</returns>
    public AnimationSetting get_setting(bool safety = true)
    {
        if (safety || !setting.possible_referenced) return setting.__deepcopy();
        return setting;
    }

    /// <summary>
    ///     获取运行状态
    /// </summary>
    public bool is_playing()
    {
        return isOn;
    }

    /// <summary>
    ///     强制更新
    /// </summary>
    /// <param name="elapsed">更新时间间隔</param>
    public void force_update(float elapsed)
    {
        if (isOn) update(elapsed);
    }

    /// <summary>
    ///     强制停止
    /// </summary>
    /// <param name="stop_with_end_action">是否执行动画终止函数</param>
    public void force_stop(bool stop_with_end_action = false)
    {
        if (!isOn) return;
        isOn = false;
        if (stop_with_end_action && setting.end_action != null)
        {
            if (setting.trace_type == AnimationTraceType.TRACK && dst_object != null)
            {
                dst_vec = dst_object.currentPosition;
            }

            setting.end_action(cur_frame_idx, ref src_vec, ref dst_vec, this);
        }
    }
}