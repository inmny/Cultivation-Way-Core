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
        public float base_scale;
        /// <summary>
        /// 动图
        /// </summary>
        public Sprite[] anim;
        /// <summary>
        /// 图像基础偏移
        /// </summary>
        public Vector2 base_offset;
        /// <summary>
        /// 生成的动画的默认设置
        /// </summary>
        public CW_AnimationSetting default_setting;
        internal CW_EffectController(int anim_limit, CW_AnimationSetting setting, Sprite[] anim, GameObject default_prefab, float base_scale)
        {
            this.base_offset = Vector2.zero;
            prefab = UnityEngine.Object.Instantiate(default_prefab);
            prefab.transform.localScale = new Vector3(base_scale, base_scale, prefab.transform.localScale.z);
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = setting.layer_name;
            renderer.transform.localPosition = new Vector3(base_offset.x + renderer.transform.localPosition.x, base_offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);
            animations = new CW_SpriteAnimation[anim_limit];
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i] = null;
            }
            default_setting = setting.__deepcopy();
            default_setting.possible_associated = true;
            this.anim = anim;
            cur_anim_nr = 0;
        }
        internal CW_EffectController(int anim_limit, CW_AnimationSetting setting, Sprite[] anim, GameObject default_prefab, float base_scale, Vector2 base_offset)
        {
            this.base_offset = base_offset;
            prefab = UnityEngine.Object.Instantiate(default_prefab);
            prefab.transform.localScale = new Vector3(base_scale, base_scale, prefab.transform.localScale.z);
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = setting.layer_name;
            renderer.transform.localPosition = new Vector3(base_offset.x + renderer.transform.localPosition.x, base_offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);
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
        public void offset(Vector2 offset)
        {
            this.base_offset += offset;
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            renderer.transform.localPosition = new Vector3(base_offset.x + renderer.transform.localPosition.x, base_offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);
        }

        internal CW_SpriteAnimation spawn_on(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj, BaseSimObject dst_obj, float scale)
        {
            CW_SpriteAnimation new_anim = new CW_SpriteAnimation(default_setting, anim, prefab, src_vec, dst_vec, src_obj, dst_obj);
            if (!new_anim.isOn) return null;
            animations[cur_anim_nr++] = new_anim;
            new_anim.change_scale(scale);
            return new_anim;
        }
        internal CW_SpriteAnimation spawn_on(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj, BaseSimObject dst_obj, float scale, Vector2 offset)
        {
            CW_SpriteAnimation new_anim = new CW_SpriteAnimation(default_setting, anim, prefab, src_vec, dst_vec, src_obj, dst_obj);
            if (!new_anim.isOn) return null;
            animations[cur_anim_nr++] = new_anim;
            new_anim.change_scale(scale);
            new_anim.offset(offset);
            return new_anim;
        }
    }
}
