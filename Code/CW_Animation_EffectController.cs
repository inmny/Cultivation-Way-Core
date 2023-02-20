using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cultivation_Way.Utils;
namespace Cultivation_Way.Animation
{
    public class CW_EffectController
    {
        internal int anim_limit;
        internal string id;

        internal GameObject prefab;
        internal CW_ForwardLinkedList<CW_SpriteAnimation> active_anims;
        internal Stack<CW_SpriteAnimation> inactive_anims;
        /// <summary>
        /// 缩放
        /// </summary>
        public float base_scale;
        /// <summary>
        /// 动图
        /// </summary>
        public Sprite[] sprites;
        /// <summary>
        /// 图像基础偏移
        /// </summary>
        public Vector2 base_offset;
        /// <summary>
        /// 生成的动画的默认设置
        /// </summary>
        public CW_AnimationSetting default_setting;
        internal CW_EffectController(string id, int anim_limit, CW_AnimationSetting setting, Sprite[] anim, GameObject default_prefab, float base_scale, Vector2 base_offset)
        {
            this.base_offset = base_offset;
            prefab = UnityEngine.Object.Instantiate(default_prefab, Main.instance.transform);
            prefab.name = "prefab_" + id;
            this.id = id;
            prefab.transform.localScale = new Vector3(base_scale, base_scale, prefab.transform.localScale.z);
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = setting.layer_name;
            renderer.transform.localPosition = new Vector3(base_offset.x + renderer.transform.localPosition.x, base_offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);

            this.active_anims = new CW_ForwardLinkedList<CW_SpriteAnimation>();
            this.inactive_anims = new Stack<CW_SpriteAnimation>((int)Mathf.Sqrt(anim_limit));

            default_setting = setting.__deepcopy();
            default_setting.possible_associated = true;
            this.sprites = anim;
            this.anim_limit = anim_limit;
        }
        public string get_id() { return id; }

        internal void update(float elapsed)
        {
            active_anims.SetToFirst();

            CW_SpriteAnimation anim = active_anims.GetCurrent();
            while(anim != null)
            {
                if(anim.isOn) anim.update(elapsed);

                if (!anim.isOn)
                {
                    CW_SpriteAnimation _anim_to_clear = active_anims.RemoveCurrent();
                    _anim_to_clear.clear();
                    inactive_anims.Push(_anim_to_clear);
                }

                active_anims.MoveNext();

                anim = active_anims.GetCurrent();
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
            CW_SpriteAnimation new_anim;
            if (inactive_anims.Count > 0)
            {
                new_anim = inactive_anims.Pop();
                new_anim.set(default_setting, sprites, prefab, src_vec, dst_vec, src_obj, dst_obj);
                active_anims.Add(new_anim);
            }
            else if(active_anims.Count <= this.anim_limit)
            {
                new_anim = new CW_SpriteAnimation(default_setting, sprites, prefab, src_vec, dst_vec, src_obj, dst_obj);
                active_anims.Add(new_anim);
            }
            else
            {
                return null;
            }
            new_anim.change_scale(scale);
            return new_anim;
        }
    }
}
