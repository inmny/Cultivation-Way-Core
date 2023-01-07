using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.Animation
{
    public class CW_SpriteAnimation
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        internal bool isOn;
        /// <summary>
        /// 是否为默认图像集
        /// </summary>
        internal bool is_default_sprites;
        /// <summary>
        /// 图像集
        /// </summary>
        internal Sprite[] sprites;
        /// <summary>
        /// 运行时间
        /// </summary>
        internal float play_time;
        /// <summary>
        /// 循环次数
        /// </summary>
        internal int loop_nr;
        /// <summary>
        /// 路径长度
        /// </summary>
        internal float trace_length;
        /// <summary>
        /// 距离下一帧剩余时间
        /// </summary>
        internal float next_frame_time;
        /// <summary>
        /// 当前帧
        /// </summary>
        internal int cur_frame_idx;
        /// <summary>
        /// 渲染器
        /// </summary>
        internal SpriteRenderer renderer;
        /// <summary>
        /// 组件
        /// </summary>
        internal GameObject gameObject;

        internal Vector2 src_vec;
        internal Vector2 dst_vec;
        internal BaseSimObject src_object;
        internal BaseSimObject dst_object;
        /// <summary>
        /// 动画设置
        /// </summary>
        internal CW_AnimationSetting setting;

        internal CW_SpriteAnimation(CW_AnimationSetting setting, Sprite[] sprites, GameObject prefab, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
        {
            isOn = true; is_default_sprites = true;
            if (setting.possible_associated) { this.setting = setting; }
            else { this.setting = setting.__deepcopy(); }
            this.apply_setting(src_vec, dst_vec, src_object, dst_object);
            if (!isOn) return;

            this.sprites = sprites;

            gameObject = UnityEngine.Object.Instantiate(prefab);
            renderer = gameObject.GetComponent<SpriteRenderer>();
            gameObject.transform.SetParent(CW_EffectManager.instance.transform);
        }
        // TODO: complete it
        internal void apply_setting(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
        {
            this.src_vec = src_vec;
            this.dst_vec = dst_vec;
            this.src_object = src_object;
            this.dst_object = dst_object;
            if (setting.trace_type==AnimationTraceType.TRACK)
            {
                if (dst_object == null)
                {
                    isOn = false;
                    if (Others.CW_Constants.is_debugging) throw new Exception("Null dst_object");
                    return;
                }
                dst_vec = dst_object.currentPosition;
            }
            this.renderer.sortingLayerName = setting.layer_name;
        }
        internal void update(float elapsed)
        {
            if (!isOn)
            {
                return;
            }
            play_time += elapsed;
            if (elapsed < next_frame_time)
            {
                next_frame_time -= elapsed;
                return;
            }
            next_frame_time += setting.frame_interval; // TODO: 可能直接设置为frame_interval更合理
            
            // 切换至下一帧图像
            /** Original Version
            int change = setting.play_direction == AnimationPlayDirection.FORWARD ? 1 : -1;
            if (setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) change = 0-change;
            **/
            int change = ((setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) ^ (setting.play_direction == AnimationPlayDirection.FORWARD)) ? 1 : -1;
            int next_frame_idx = (cur_frame_idx + change + sprites.Length) % sprites.Length;
            renderer.sprite = sprites[next_frame_idx];
            cur_frame_idx = next_frame_idx;
            if (cur_frame_idx == 0) loop_nr++;
            // 检测到目标不存在后停止
            if (setting.trace_type == AnimationTraceType.TRACK && dst_object == null)
            {
                isOn = false;
                return;
            }
            float src_x, src_y; float dst_x = 0, dst_y = 0;
            src_x = src_vec.x; src_y = src_vec.y;
            // 轨迹更新
            if (setting.trace_type != AnimationTraceType.NONE)
            {
                float next_x = gameObject.transform.position.x;
                float next_y = gameObject.transform.position.y;
                if(setting.trace_type == AnimationTraceType.TRACK)
                {
                    dst_x = dst_object.currentPosition.x; dst_y = dst_object.currentPosition.y;
                    setting.trace_updater(src_vec.x, src_vec.y, dst_x, dst_y, play_time, ref next_x, ref next_y, setting.trace_grad);
                }
                else
                {
                    dst_x = dst_vec.x; dst_y = dst_vec.y;
                    setting.trace_updater(src_vec.x, src_vec.y, dst_x, dst_y, play_time, ref next_x, ref next_y, setting.trace_grad);
                }
                float delta_x = next_x - gameObject.transform.position.x;
                float delta_y = next_y - gameObject.transform.position.y;
                trace_length += Mathf.Sqrt(delta_x * delta_x + delta_y * delta_y);
                gameObject.transform.position = new Vector3(next_x, next_y);
            }
            // 路径行为
            if (setting.frame_action != null) setting.frame_action(cur_frame_idx, src_x, src_y, dst_x, dst_y, play_time, gameObject.transform.position.x, gameObject.transform.position.y, src_object);

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
                        if (Toolbox.Dist(dst_x,dst_y,gameObject.transform.position.x,gameObject.transform.position.y) < setting.trace_grad) end = true;
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
                isOn = false;
                if (setting.end_action != null) setting.end_action(cur_frame_idx, src_x, src_y, dst_x, dst_y, play_time, gameObject.transform.position.x, gameObject.transform.position.y, src_object, dst_object);
            }
        }
        internal void kill()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        /// <summary>
        /// 修改动画设置，请在知道你在做什么的情况下谨慎操作
        /// </summary>
        /// <param name="setting">新的动画设置</param>
        /// <param name="deepcopy">是否进行深拷贝，默认是</param>
        public void change_setting(CW_AnimationSetting setting, bool deepcopy = true)
        {
            if (deepcopy) { this.setting = setting.__deepcopy(); this.setting.possible_associated = false; }
            else { this.setting = setting; this.setting.possible_associated = true; }
            apply_setting(src_vec, dst_vec, src_object, dst_object);
        }
        /// <summary>
        /// 获取动画设置原本/拷贝，请在知道你在做什么的情况下谨慎操作
        /// </summary>
        /// <param name="safety">是否保证安全，此处安全是指是否与其他处共用setting对象</param>
        /// <returns>动画设置</returns>
        public CW_AnimationSetting get_setting(bool safety = true)
        {
            if(safety || !this.setting.possible_associated) return this.setting.__deepcopy();
            return this.setting;
        }
        /// <summary>
        /// 获取运行状态
        /// </summary>
        public bool is_playing()
        {
            return isOn;
        }
        /// <summary>
        /// 强制更新
        /// </summary>
        /// <param name="elapsed">更新时间间隔</param>
        public void force_update(float elapsed)
        {
            if (isOn) update(elapsed);
        }
        /// <summary>
        /// 强制停止
        /// </summary>
        /// <param name="stop_with_end_action">是否执行动画终止函数</param>
        public void force_stop(bool stop_with_end_action = false)
        {
            if (!isOn) return;
            isOn = false;
            if (stop_with_end_action && setting.end_action != null)
            {
                float dst_x = 0, dst_y = 0;
                if (setting.trace_type == AnimationTraceType.TRACK)
                {
                    dst_x = dst_object.currentPosition.x; dst_y = dst_object.currentPosition.y;
                }
                else if(setting.trace_type!=AnimationTraceType.NONE)
                {
                    dst_x = dst_vec.x; dst_y = dst_vec.y;
                }
                setting.end_action(cur_frame_idx, src_vec.x, src_vec.y, dst_x, dst_y, play_time, gameObject.transform.position.x, gameObject.transform.position.y, src_object, dst_object);
            }
        }
    }
}
