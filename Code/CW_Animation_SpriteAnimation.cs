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

        internal Vector2 src_vec;
        internal Vector2 dst_vec;
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
            throw new NotImplementedException();
        }
    }
}
