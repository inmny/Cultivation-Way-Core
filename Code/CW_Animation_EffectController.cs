using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way.Animation
{
    public class CW_EffectController
    {
        CW_SpriteAnimation[] animations;
        /// <summary>
        /// 生成的动画的默认设置
        /// </summary>
        public CW_AnimationSetting default_setting;
        internal CW_EffectController(int anim_limit, CW_AnimationSetting setting, Sprite[] anim)
        {
            animations = new CW_SpriteAnimation[anim_limit];
            for(int i = 0; i < animations.Length; i++)
            {
                animations[i] = null;
            }
            default_setting = setting.__deepcopy();
            default_setting.is_default = true;
        }
        internal void update(float elapsed)
        {
            for(int i = 0; i < animations.Length; i++)
            {
                if (animations[i]!=null && animations[i].isOn) animations[i].update(elapsed);
            }
        }
        
    }
}
