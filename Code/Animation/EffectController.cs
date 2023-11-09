using System.Collections.Generic;
using Cultivation_Way.Others.DataStructs;
using UnityEngine;

namespace Cultivation_Way.Animation;

public class EffectController
{
    private readonly CW_ForwardLinkedList<SpriteAnimation> _active_anims = new();
    private readonly Stack<SpriteAnimation> _inactive_anims;

    private readonly GameObject _prefab;

    /// <summary>
    ///     生成的动画的默认设置
    /// </summary>
    public readonly AnimationSetting default_setting;

    /// <summary>
    ///     图像基础偏移
    /// </summary>
    private Vector2 _base_offset;

    private float _base_scale;
    internal int anim_limit;

    internal string id;

    /// <summary>
    ///     动图
    /// </summary>
    public Sprite[] sprites;

    internal EffectController(string id, int anim_limit, AnimationSetting setting, Sprite[] anim,
        GameObject default_prefab, float base_scale, Vector2 base_offset)
    {
        _base_offset = base_offset;
        _prefab = GameObject.Instantiate(default_prefab, CW_Core.anim_prefab_library);
        _prefab.name = "prefab_" + id;
        this.id = id;
        this.base_scale = base_scale;

        SpriteRenderer renderer = _prefab.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = setting.layer_name;

        var transform = renderer.transform;
        var local_position = transform.localPosition;
        local_position = new Vector3(base_offset.x + local_position.x,
            base_offset.y + local_position.y, local_position.z);
        transform.localPosition = local_position;

        _inactive_anims = new Stack<SpriteAnimation>((int)Mathf.Sqrt(anim_limit));

        default_setting = setting.__deepcopy();
        default_setting.possible_referenced = true;
        sprites = anim;
        this.anim_limit = anim_limit;
    }

    /// <summary>
    ///     缩放
    /// </summary>
    public float base_scale
    {
        get => _base_scale;
        set
        {
            _prefab.transform.localScale = new Vector3(value, value, _prefab.transform.localScale.z);
            _base_scale = value;
        }
    }

    public string get_id()
    {
        return id;
    }

    internal void recycle_memory()
    {
        int target_num = anim_limit * 3 / 4;
        while (_inactive_anims.Count > target_num)
        {
            SpriteAnimation anim = _inactive_anims.Pop();
            anim.kill();
        }
    }

    internal void update(float elapsed)
    {
        _active_anims.SetToFirst();

        SpriteAnimation anim = _active_anims.GetCurrent();
        SpriteAnimation _anim_to_clear;
        //int count = 0;
        while (anim != null)
        {
            anim.update(elapsed);

            if (!anim.isOn)
            {
                //count++;
                _anim_to_clear = _active_anims.RemoveCurrent();
                _anim_to_clear.clear();
                _inactive_anims.Push(_anim_to_clear);
            }

            _active_anims.MoveNext();

            anim = _active_anims.GetCurrent();
        }
        //if(count>0)Debug.LogFormat("Inactivate {0}/{1}", count, active_anims.Count);
    }

    public void clear()
    {
        _active_anims.SetToFirst();

        SpriteAnimation anim = _active_anims.GetCurrent();
        SpriteAnimation _anim_to_clear;
        //int count = 0;
        while (anim != null)
        {
            anim.force_stop();
            //count++;
            _anim_to_clear = _active_anims.RemoveCurrent();
            _anim_to_clear.clear();
            _inactive_anims.Push(_anim_to_clear);

            _active_anims.MoveNext();

            anim = _active_anims.GetCurrent();
        }
    }

    public void offset(Vector2 offset)
    {
        _base_offset += offset;
        SpriteRenderer renderer = _prefab.GetComponent<SpriteRenderer>();
        var transform = renderer.transform;
        var local_position = transform.localPosition;
        local_position = new Vector3(_base_offset.x + local_position.x,
            _base_offset.y + local_position.y, local_position.z);
        transform.localPosition = local_position;
    }

    internal SpriteAnimation spawn_on(Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj, BaseSimObject dst_obj,
        float scale)
    {
        SpriteAnimation new_anim;
        if (_inactive_anims.Count > 0)
        {
            new_anim = _inactive_anims.Pop();
            new_anim.set(default_setting, sprites, _prefab, src_vec, dst_vec, src_obj, dst_obj);
            _active_anims.Add(new_anim);
        }
        else if (_active_anims.Count <= anim_limit)
        {
            new_anim = new SpriteAnimation(default_setting, sprites, _prefab, src_vec, dst_vec, src_obj, dst_obj);
            _active_anims.Add(new_anim);
        }
        else
        {
            return null;
        }

        new_anim.change_scale(scale);
        return new_anim;
    }
}