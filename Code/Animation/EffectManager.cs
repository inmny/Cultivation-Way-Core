using System;
using System.Collections.Generic;
using NeoModLoader.api.attributes;
using UnityEngine;

namespace Cultivation_Way.Animation;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    private static QualityChanger quality_changer;
    internal static Vector2 camera_range_1;
    internal static Vector2 camera_range_2;
    private readonly List<EffectController> controllers = new();
    private readonly Dictionary<string, EffectController> controllers_dict = new();
    private readonly GameObject default_prefab = new();
    private readonly List<SpriteAnimation> single_anims = new();
    private bool initialized;
    internal bool low_res;
    private float timer;

    private void Awake()
    {
        if (initialized) return;

        initialized = true;
        instance = this;
        default_prefab.AddComponent<SpriteRenderer>();

        quality_changer = MapBox.instance.qualityChanger;
    }

    [Hotfixable]
    private void Update()
    {
        if (Config.paused || ScrollWindow.isWindowActive()) return;
        int i = 0;
        int time;
        camera_range_1 = World.world.camera.ViewportToWorldPoint(new Vector3(0, 0, World.world.camera.nearClipPlane));
        camera_range_2 = World.world.camera.ViewportToWorldPoint(new Vector3(1, 1, World.world.camera.nearClipPlane));
        low_res = quality_changer.isFullLowRes();
        Toolbox.bench("EffectManager.Update");
        float elapsed = World.world.getCurElapsed();
        try
        {
            /*
            for (time = 0; time < Config.timeScale; time++)
            {*/
            for (i = 0; i < controllers.Count; i++)
            {
                Toolbox.bench(controllers[i].id, "EffectManager.Update");
                controllers[i].update(elapsed, (int)Config.timeScale);

                Toolbox.benchEnd(controllers[i].id, "EffectManager.Update");
            }

            for (i = 0; i < single_anims.Count; i++)
            {
                for (time = 0; time < Config.timeScale; time++)
                {
                    single_anims[i].update(elapsed);
                }

                if (single_anims[i].isOn) continue;
                single_anims.RemoveAt(i);
                i--;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("An error happen in Update '{0}'", controllers[i].id));
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }

        Toolbox.benchEnd("EffectManager.Update");

        if (timer > 0)
        {
            timer -= elapsed;
        }
        else
        {
            timer = Constants.Others.anim_recycle_interval;
            for (i = 0; i < controllers.Count; i++)
            {
                controllers[i].recycle_memory();
            }
        }
    }

    /// <summary>
    ///     加载一个动画控制器并返回
    /// </summary>
    /// <param name="id">控制器id</param>
    /// <param name="path_to_anim">动画文件夹路径</param>
    /// <param name="base_offset">加载时偏移</param>
    /// <param name="anim_limit">动画数量软限制</param>
    /// <param name="base_scale">基础缩放(与动画实例缩放相乘)</param>
    /// <param name="controller_setting">动画设置</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public EffectController load_as_controller(string id, string path_to_anim, Vector2 base_offset,
        int anim_limit = 100, float base_scale = 1.0f, AnimationSetting controller_setting = null)
    {
        if (controllers_dict.ContainsKey(id))
        {
            if (Constants.Others.strict_mode) throw new Exception(string.Format("Repeated Controller id with {0}", id));
            return null;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(path_to_anim);
        if (sprites == null || sprites.Length == 0) throw new Exception("No found sprites under:" + path_to_anim);
        EffectController controller = new(id, anim_limit, controller_setting ?? new AnimationSetting(), sprites,
            default_prefab, base_scale, base_offset);
        controllers.Add(controller);
        controllers_dict.Add(id, controller);
        return controller;
    }

    public EffectController load_as_controller(string id, string path_to_anim, int anim_limit = 100,
        float base_scale = 1.0f, AnimationSetting controller_setting = null)
    {
        return load_as_controller(id, path_to_anim, Vector2.zero, anim_limit, base_scale, controller_setting);
    }

    public EffectController get_controller(string id)
    {
        return controllers_dict.TryGetValue(id, out EffectController controller) ? controller : null;
    }

    public SpriteAnimation spawn_anim(string id, WorldTile src_tile, WorldTile dst_tile, BaseSimObject src_obj = null,
        BaseSimObject dst_obj = null, float scale = 1.0f)
    {
        if (controllers_dict.TryGetValue(id, out EffectController controller))
        {
            return controller.spawn_on(src_tile.pos, dst_tile?.pos ?? src_tile.pos, src_obj, dst_obj,
                scale);
        }

        throw new Exception("No found animations controller for id:" + id);
    }

    /// <summary>
    ///     生成动画并返回, 如果已有动画数量超过限制则会返回null
    /// </summary>
    /// <param name="id">控制器id</param>
    /// <param name="src_vec">起始点</param>
    /// <param name="dst_vec">终点</param>
    /// <param name="src_obj">起始对象</param>
    /// <param name="dst_obj">目标对象</param>
    /// <param name="scale">动画实例缩放(与控制器缩放相乘)</param>
    /// <returns></returns>
    /// <exception cref="Exception">没有找到对应的控制器则抛出异常</exception>
    public SpriteAnimation spawn_anim(string id, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj = null,
        BaseSimObject dst_obj = null, float scale = 1.0f)
    {
        if (controllers_dict.TryGetValue(id, out EffectController controller))
        {
            return controller.spawn_on(src_vec, dst_vec, src_obj, dst_obj, scale);
        }

        throw new Exception("No found animations controller for id:" + id);
    }

    public void clear()
    {
        foreach (EffectController controller in controllers)
        {
            controller.clear();
        }

        int idx = single_anims.Count - 1;
        while (idx >= 0)
        {
            SpriteAnimation anim = single_anims[idx];
            single_anims[idx] = null;
            anim.kill();
            idx--;
        }
    }
}