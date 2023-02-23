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
        public Sprite[] sprites;
        /// <summary>
        /// 运行时间
        /// </summary>
        public float play_time;
        /// <summary>
        /// 循环次数
        /// </summary>
        public int loop_nr;
        /// <summary>
        /// 路径长度
        /// </summary>
        public float trace_length;
        /// <summary>
        /// 距离下一帧剩余时间
        /// </summary>
        public float next_frame_time;
        /// <summary>
        /// 当前帧
        /// </summary>
        public int cur_frame_idx;
        /// <summary>
        /// 渲染器
        /// </summary>
        public SpriteRenderer renderer;
        /// <summary>
        /// 组件
        /// </summary>
        public GameObject gameObject;
        public float end_froze_time;
        public bool has_end;
        public Vector2 src_vec;
        public Vector2 dst_vec;
        public BaseSimObject src_object;
        public BaseSimObject dst_object;
        public float free_val;
        public string free_str;
        public float cur_elapsed;
        /// <summary>
        /// 动画设置
        /// </summary>
        internal CW_AnimationSetting setting;
        public float cost_for_spell;

        internal CW_SpriteAnimation(CW_AnimationSetting setting, Sprite[] sprites, GameObject prefab, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
        {
            isOn = true; is_default_sprites = true;
            this.setting = setting;
            
            this.sprites = sprites;

            gameObject = UnityEngine.Object.Instantiate(prefab, CW_EffectManager.instance.transform);
            gameObject.layer = 0;
            renderer = gameObject.GetComponent<SpriteRenderer>();

            this.apply_setting(src_vec, dst_vec, src_object, dst_object);
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
                if (dst_object != null)
                {
                    this.dst_vec = dst_object.currentPosition;
                }
            }
            //WorldBoxConsole.Console.print("Is renderer null?>" + (renderer == null)+"\nIs setting null?>"+(setting==null));
            renderer.sortingLayerName = setting.layer_name;

            gameObject.transform.localPosition = this.src_vec;

            if(setting.point_to_dst) gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Toolbox.getAngle(gameObject.transform.position.x, gameObject.transform.position.y, this.dst_vec.x, this.dst_vec.y) * 57.29578f));
        }
        internal void set(CW_AnimationSetting setting, Sprite[] sprites, GameObject prefab, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_object, BaseSimObject dst_object)
        {
            isOn = true; is_default_sprites = true;
            this.setting = setting;

            this.sprites = sprites;

            this.gameObject.transform.localScale = prefab.transform.localScale;

            this.apply_setting(src_vec, dst_vec, src_object, dst_object);

            //this.gameObject.SetActive(true);
            show();
        }
        internal void clear()
        {
            //this.gameObject.transform.localPosition = new Vector3(9999999, 999999);
            hide();
            //this.gameObject.SetActive(false);
            this.cost_for_spell = 0;
            this.cur_frame_idx = 0;
            this.dst_object = null;
            this.dst_vec.x = 0;
            this.dst_vec.y = 0;
            this.end_froze_time = -1;
            this.has_end = false;
            this.loop_nr = 0;
            this.next_frame_time = 0;
            this.play_time = 0;
            this.src_object = null;
            this.src_vec.x = 0;
            this.src_vec.y = 0;
            this.trace_length = 0;
            this.free_val = 0;
            this.free_str = "";
        }
        private void hide()
        {
            //this.gameObject.layer
            renderer.enabled = false;
            //renderer.sprite = null;
            this.gameObject.layer = 1<<3;
        }
        private void show()
        {
            this.gameObject.layer = 0;
            renderer.enabled = true;
            //renderer.sprite = sprites[cur_frame_idx];
        }
        internal void update(float elapsed)
        {
            if (!isOn)
            {
                return;
            }

            if (renderer.enabled&&!setting.visible_in_low_res&&CW_EffectManager.instance.low_res) hide();
            else if (!renderer.enabled) show();

            //if (!CW_EffectManager.instance.low_res && renderer.sprite == null) renderer.sprite = sprites[cur_frame_idx];
            play_time += elapsed;
            cur_elapsed = elapsed;
            if ( play_time > Others.CW_Constants.max_anim_time)
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
            }
            else
            {
                next_frame_time += setting.frame_interval; // TODO: 可能直接设置为frame_interval更合理

                // 切换至下一帧图像
                /** Original Version
                int change = setting.play_direction == AnimationPlayDirection.FORWARD ? 1 : -1;
                if (setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) change = 0-change;
                **/
                if (cur_frame_idx != setting.anim_froze_frame_idx)
                {
                    int change = ((setting.loop_type == AnimationLoopType.ETOE && (loop_nr & 0x1) == 1) ^ (setting.play_direction == AnimationPlayDirection.FORWARD)) ? 1 : -1;
                    int next_frame_idx = (cur_frame_idx + change + sprites.Length) % sprites.Length;
                    if(!CW_EffectManager.instance.low_res) renderer.sprite = sprites[next_frame_idx];
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
                gameObject.transform.Rotate(setting.always_roll_axis * elapsed * setting.roll_angle_per_frame);
            }
            // 检测到目标不存在后停止
            /**
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
                    Vector2 tmp_src_vec = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                    if (dst_object != null)
                    {
                        dst_vec = dst_object.currentPosition;
                    }
                    setting.trace_updater(ref tmp_src_vec, ref dst_vec, this, ref delta_x, ref delta_y);
                }
                else if (setting.trace_type == AnimationTraceType.ATTACH)
                {
                    if (!this.dst_object.base_data.alive)
                    {
                        this.isOn = false; return;
                    }
                
                    gameObject.transform.position = this.dst_object.currentPosition;
                }
                else
                {
                    setting.trace_updater(ref src_vec, ref dst_vec, this, ref delta_x, ref delta_y);
                }
                delta_x *= elapsed; delta_y *= elapsed;
                float next_x = gameObject.transform.position.x + delta_x;
                float next_y = gameObject.transform.position.y + delta_y;
                //WorldBoxConsole.Console.print(string.Format("delta x:{0},y:{1}", delta_x, delta_y));
                if(setting.loop_limit_type==AnimationLoopLimitType.TRACE_LIMIT || setting.loop_limit_type == AnimationLoopLimitType.DST_LIMIT) trace_length += Mathf.Sqrt(delta_x * delta_x + delta_y * delta_y);

                // 指向终点
                if (setting.always_point_to_dst)
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Toolbox.getAngle(gameObject.transform.position.x, gameObject.transform.position.y, dst_vec.x, dst_vec.y) * 57.29578f));
                }
                gameObject.transform.position = new Vector3(next_x, next_y, next_y);
            }
            // 路径行为
            if (setting.frame_action != null) setting.frame_action(cur_frame_idx, ref src_vec, ref dst_vec, this);

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
                        if (Toolbox.Dist(dst_vec.x,dst_vec.y,gameObject.transform.position.x,gameObject.transform.position.y) < Others.CW_Constants.anim_dst_error || trace_length >= Others.CW_Constants.max_anim_trace_length) end = true;
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
                if (end_froze_time>=setting.froze_time_after_end+0.01f) isOn = false;
            }
        }
        internal void kill()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="scale">大小系数</param>
        public void set_scale(float scale)
        {
            gameObject.transform.localScale = new Vector3(scale, scale, gameObject.transform.localScale.z);
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale">缩放比例</param>
        public void change_scale(float scale)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * scale, gameObject.transform.localScale.y * scale, gameObject.transform.localScale.z);
        }
        public Vector3 get_scale()
        {
            return gameObject.transform.localScale;
        }
        public void set_position(Vector3 pos)
        {
            this.gameObject.transform.position = pos;
        }
        public void offset(Vector2 offset)
        {
            this.renderer.transform.localPosition = new Vector3(offset.x + renderer.transform.localPosition.x, offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);
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
        public void set_alpha(float alpha)
        {
            Color color = this.renderer.color;
            color.a = alpha;
            renderer.color = color;
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
                if (setting.trace_type == AnimationTraceType.TRACK && this.dst_object!=null)
                {
                    dst_vec = dst_object.currentPosition;
                }
                setting.end_action(cur_frame_idx, ref src_vec, ref dst_vec, this);
            }
        }
    }
}
