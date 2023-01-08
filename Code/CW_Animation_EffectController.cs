using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way.Animation
{
    public class CW_EffectController
    {
        internal int cur_anim_nr;

        internal GameObject prefab;
        internal CW_SpriteAnimation[] animations;
        /// <summary>
        /// 缩放
        /// </summary>
        public float scale;
        /// <summary>
        /// 动图
        /// </summary>
        public Sprite[] anim;
        /// <summary>
        /// 生成的动画的默认设置
        /// </summary>
        public CW_AnimationSetting default_setting;
        internal CW_EffectController(int anim_limit, CW_AnimationSetting setting, Sprite[] anim, GameObject default_prefab)
        {
            prefab = UnityEngine.Object.Instantiate(default_prefab);
            prefab.transform.localScale = new Vector3(scale, scale, prefab.transform.localScale.z);
            prefab.GetComponent<SpriteRenderer>().sortingLayerName = setting.layer_name;
            animations = new CW_SpriteAnimation[anim_limit];
            for(int i = 0; i < animations.Length; i++)
            {
                animations[i] = null;
            }
            default_setting = setting.__deepcopy();
            default_setting.possible_associated = true;
            this.anim = anim;
            cur_anim_nr = 0;
        }
        internal void update(float elapsed)
        {
            int i;
            for(i = 0; i < cur_anim_nr; i++)
            {
                if (animations[i]!=null && animations[i].isOn) animations[i].update(elapsed);
            }
            for (i = 0; i < cur_anim_nr; i++)
            {
                if (animations[i] != null && !animations[i].isOn) 
                {
                    animations[i].kill();
                    animations[i] = animations[cur_anim_nr - 1];
                    animations[cur_anim_nr - 1] = null;
                    cur_anim_nr--;
                }
            }
        }

        internal CW_SpriteAnimation spawn_on(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj, BaseSimObject dst_obj)
        {
            CW_SpriteAnimation new_anim = new CW_SpriteAnimation(default_setting, anim, prefab, src_vec, dst_vec, src_obj, dst_obj);
            if (!new_anim.isOn) return null;
            animations[cur_anim_nr++] = new_anim;
            return new_anim;
        }
    }
}
