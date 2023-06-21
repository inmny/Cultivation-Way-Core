using Cultivation_Way.Others.DataStructs;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way.Animation
{
    public class EffectController
    {
        internal int anim_limit;
        internal string id;

        internal GameObject prefab;
        internal CW_ForwardLinkedList<SpriteAnimation> active_anims;
        internal Stack<SpriteAnimation> inactive_anims;
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
        public AnimationSetting default_setting;
        internal EffectController(string id, int anim_limit, AnimationSetting setting, Sprite[] anim, GameObject default_prefab, float base_scale, Vector2 base_offset)
        {
            this.base_offset = base_offset;
            prefab = UnityEngine.Object.Instantiate(default_prefab, CW_Core.instance.transform);
            prefab.name = "prefab_" + id;
            this.id = id;
            prefab.transform.localScale = new Vector3(base_scale, base_scale, prefab.transform.localScale.z);
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = setting.layer_name;
            renderer.transform.localPosition = new Vector3(base_offset.x + renderer.transform.localPosition.x, base_offset.y + renderer.transform.localPosition.y, renderer.transform.localPosition.z);

            this.active_anims = new CW_ForwardLinkedList<SpriteAnimation>();
            this.inactive_anims = new Stack<SpriteAnimation>((int)Mathf.Sqrt(anim_limit));

            default_setting = setting.__deepcopy();
            default_setting.possible_referenced = true;
            this.sprites = anim;
            this.anim_limit = anim_limit;
        }
        public string get_id() { return id; }
        internal void recycle_memory()
        {
            int target_num = anim_limit * 3 / 4;
            while (inactive_anims.Count > target_num)
            {
                SpriteAnimation anim = inactive_anims.Pop();
                anim.kill();
            }
        }
        internal void update(float elapsed)
        {
            active_anims.SetToFirst();

            SpriteAnimation anim = active_anims.GetCurrent();
            SpriteAnimation _anim_to_clear;
            //int count = 0;
            while (anim != null)
            {
                anim.update(elapsed);

                if (!anim.isOn)
                {
                    //count++;
                    _anim_to_clear = active_anims.RemoveCurrent();
                    _anim_to_clear.clear();
                    inactive_anims.Push(_anim_to_clear);
                }

                active_anims.MoveNext();

                anim = active_anims.GetCurrent();
            }
            //if(count>0)Debug.LogFormat("Inactivate {0}/{1}", count, active_anims.Count);
        }
        public void clear()
        {
            active_anims.SetToFirst();

            SpriteAnimation anim = active_anims.GetCurrent();
            SpriteAnimation _anim_to_clear;
            //int count = 0;
            while (anim != null)
            {
                anim.force_stop(false);
                //count++;
                _anim_to_clear = active_anims.RemoveCurrent();
                _anim_to_clear.clear();
                inactive_anims.Push(_anim_to_clear);

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

        internal SpriteAnimation spawn_on(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj, BaseSimObject dst_obj, float scale)
        {
            SpriteAnimation new_anim;
            if (inactive_anims.Count > 0)
            {
                new_anim = inactive_anims.Pop();
                new_anim.set(default_setting, sprites, prefab, src_vec, dst_vec, src_obj, dst_obj);
                active_anims.Add(new_anim);
            }
            else if (active_anims.Count <= this.anim_limit)
            {
                new_anim = new SpriteAnimation(default_setting, sprites, prefab, src_vec, dst_vec, src_obj, dst_obj);
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
