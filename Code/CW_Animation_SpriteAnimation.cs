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
        /// 动画设置
        /// </summary>
        public CW_AnimationSetting setting;
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
        public CW_SpriteAnimation(CW_AnimationSetting setting, Sprite[] sprites)
        {
            isOn = true; is_default_sprites = true;
            if (setting.is_default) { this.setting = setting; }
            else { this.setting = setting.__deepcopy(); }

            this.sprites = sprites;
        }
        internal void update(float elapsed)
        {
            if (!isOn)
            {
                kill();
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
                        if (loop_nr == setting.loop_nr_limit) end = true;
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
            throw new NotImplementedException();
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
        /// <param name="elapsed"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void force_update(float elapsed)
        {
            if (isOn) update(elapsed);
        }
        public void force_stop(bool stop_with_end_action = false)
        {
            if (!isOn) return;
            isOn = false;
            if (stop_with_end_action)
            {
                throw new NotImplementedException();
            }
        }
    }
}
