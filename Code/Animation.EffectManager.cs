using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Animation
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;
        internal bool low_res;
        internal static QualityChanger quality_changer;
        private bool initialized = false;
        private readonly GameObject default_prefab = new();
        private readonly List<SpriteAnimation> single_anims = new();
        private readonly List<EffectController> controllers = new();
        private readonly Dictionary<string, EffectController> controllers_dict = new();
        private float timer = 0;

        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                instance = this;
                default_prefab.AddComponent<SpriteRenderer>();

                quality_changer = MapBox.instance.qualityChanger;
            }
        }

        private void Update()
        {
            if (Config.paused || ScrollWindow.isWindowActive()) return;
            int i = 0; int time;
            low_res = quality_changer.isFullLowRes();
            try
            {
                for (time = 0; time < Config.timeScale; time++)
                {
                    for (i = 0; i < controllers.Count; i++)
                    {
                        controllers[i].update(Time.fixedDeltaTime);
                    }
                    for (i = 0; i < single_anims.Count; i++)
                    {
                        single_anims[i].update(Time.fixedDeltaTime);
                        if (!single_anims[i].isOn)
                        {
                            single_anims.RemoveAt(i); i--;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("An error happen in update '{0}'", controllers[i].id));
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    timer = Constants.Others.anim_recycle_interval;
                    for (i = 0; i < controllers.Count; i++)
                    {
                        controllers[i].recycle_memory();
                    }
                }
            }
        }
        public EffectController load_as_controller(string id, string path_to_anim, Vector2 base_offset, int anim_limit = 100, float base_scale = 1.0f, AnimationSetting controller_setting = null)
        {
            if (controllers_dict.ContainsKey(id))
            {
                if (Constants.Others.strict_mode) throw new System.Exception(string.Format("Repeated Controller id with {0}", id));
                return null;
            }
            Sprite[] sprites = Resources.LoadAll<Sprite>(path_to_anim);
            if (sprites == null || sprites.Length == 0) throw new System.Exception("No found sprites under:" + path_to_anim); 
            EffectController controller = new(id, anim_limit, controller_setting == null ? new AnimationSetting() : controller_setting, sprites, default_prefab, base_scale, base_offset);
            this.controllers.Add(controller);
            this.controllers_dict.Add(id, controller);
            return controller;
        }
        public EffectController load_as_controller(string id, string path_to_anim, int anim_limit = 100, float base_scale = 1.0f, AnimationSetting controller_setting = null)
        {
            return load_as_controller(id, path_to_anim, Vector2.zero, anim_limit, base_scale, controller_setting);
        }
        public EffectController get_controller(string id)
        {
            EffectController controller;
            if (controllers_dict.TryGetValue(id, out controller)) return controller;
            return null;
        }
        public SpriteAnimation spawn_anim(string id, WorldTile src_tile, WorldTile dst_tile = null, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, float scale = 1.0f)
        {
            EffectController controller;
            if (controllers_dict.TryGetValue(id, out controller))
            {
                return controller.spawn_on(src_tile.pos, dst_tile == null ? src_tile.pos : dst_tile.pos, src_obj, dst_obj, scale);
            }
            throw new System.Exception("No found animations controller for id:" + id);
        }
        public SpriteAnimation spawn_anim(string id, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, float scale = 1.0f)
        {
            EffectController controller;
            if (controllers_dict.TryGetValue(id, out controller))
            {
                return controller.spawn_on(src_vec, dst_vec, src_obj, dst_obj, scale);
            }
            throw new System.Exception("No found animations controller for id:" + id);
        }
        public void clear()
        {
            foreach (EffectController controller in this.controllers)
            {
                controller.clear();
            }
            int idx = this.single_anims.Count - 1;
            while (idx >= 0)
            {
                SpriteAnimation anim = this.single_anims[idx];
                single_anims[idx] = null;
                anim.kill();
                idx--;
            }
        }
    }
}
