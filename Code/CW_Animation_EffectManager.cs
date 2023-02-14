using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
using UnityEngine.UI;
using System;

namespace Cultivation_Way.Animation
{
    public class CW_EffectManager : MonoBehaviour
    {
        // TODO: MapBox.QualityChanger在低画质情况下隐藏法术动画
        public static CW_EffectManager instance;
        internal bool low_res;
        internal static QualityChanger quality_changer;
        private bool initialized = false;
        private GameObject default_prefab = new GameObject();
        private CW_AnimationSetting default_setting = new CW_AnimationSetting();
        private List<CW_SpriteAnimation> single_anims = new List<CW_SpriteAnimation>();
        private List<CW_EffectController> controllers = new List<CW_EffectController>();
        private Dictionary<string, CW_EffectController> controllers_dict = new Dictionary<string,CW_EffectController>();
        
        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                instance = this;
                default_prefab.AddComponent<SpriteRenderer>();
                quality_changer = Reflection.GetField(typeof(MapBox), MapBox.instance, "qualityChanger") as QualityChanger;
            }
        }

        private void Update()
        {
            if (!Others.CW_Constants.is_debugging && (Config.paused || ScrollWindow.isWindowActive())) return;
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
            }catch(Exception e)
            {
                Debug.LogError(string.Format("An error happen in update '{0}'", controllers[i].id));
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
        }
        public CW_EffectController load_as_controller(string id, string path_to_anim, Vector2 base_offset, int anim_limit = 100, float base_scale = 1.0f, CW_AnimationSetting controller_setting = null)
        {
            if (controllers_dict.ContainsKey(id))
            {
                if (Others.CW_Constants.is_debugging) throw new System.Exception(string.Format("Repeated Controller id with {0}", id));
                return null;
            }
            Sprite[] sprites = Resources.LoadAll<Sprite>(path_to_anim);
            if (sprites == null || sprites.Length == 0) { throw new System.Exception("No found sprites under:" + path_to_anim); return null; }
            CW_EffectController controller = new CW_EffectController(id, anim_limit, controller_setting==null?new CW_AnimationSetting():controller_setting, sprites, default_prefab, base_scale, base_offset);
            this.controllers.Add(controller);
            this.controllers_dict.Add(id, controller);
            return controller;
        }
        public CW_EffectController load_as_controller(string id, string path_to_anim, int anim_limit = 100, float base_scale = 1.0f, CW_AnimationSetting controller_setting = null)
        {
            return load_as_controller(id,path_to_anim,Vector2.zero,anim_limit,base_scale,controller_setting);
        }
        public CW_EffectController get_controller(string id)
        {
            CW_EffectController controller;
            if (controllers_dict.TryGetValue(id, out controller)) return controller;
            return null;
        }
        public CW_SpriteAnimation spawn_anim(string path_to_anim, WorldTile src_tile, WorldTile dst_tile = null, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, CW_AnimationSetting anim_setting = null, float scale = 1.0f)
        {
            Sprite[] sprites = Utils.CW_ResourceHelper.load_sprites(path_to_anim);
            if (sprites == null) return null;

            CW_SpriteAnimation anim = new CW_SpriteAnimation(anim_setting == null ? default_setting.__deepcopy() : anim_setting, sprites, default_prefab, src_tile.pos, dst_tile==null?src_tile.pos : dst_tile.pos, src_obj, dst_obj);

            if (!anim.isOn) { anim.kill(); return null; }
            anim.set_scale(scale);
            single_anims.Add(anim);
            return anim;
        }
        public CW_SpriteAnimation spawn_anim(string path_to_anim, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, CW_AnimationSetting anim_setting = null, float scale = 1.0f)
        {
            Sprite[] sprites = Utils.CW_ResourceHelper.load_sprites(path_to_anim);
            if (sprites == null) return null;

            CW_SpriteAnimation anim = new CW_SpriteAnimation(anim_setting==null?default_setting.__deepcopy():anim_setting, sprites, default_prefab, src_vec, dst_vec, src_obj, dst_obj);

            if (!anim.isOn) { anim.kill(); return null; }
            anim.set_scale(scale);
            single_anims.Add(anim);
            return anim;
        }
        public CW_SpriteAnimation spawn_anim(string id, WorldTile src_tile, WorldTile dst_tile = null, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, float scale = 1.0f)
        {
            CW_EffectController controller;
            if (controllers_dict.TryGetValue(id, out controller))
            {
                return controller.spawn_on(src_tile.pos, dst_tile==null?src_tile.pos:dst_tile.pos, src_obj, dst_obj, scale);
            }
            throw new System.Exception("No found animations controller for id:" + id);
            return null;
        }
        public CW_SpriteAnimation spawn_anim(string id, Vector2 src_vec, Vector2 dst_vec, BaseSimObject src_obj = null, BaseSimObject dst_obj = null, float scale = 1.0f)
        {
            CW_EffectController controller;
            if(controllers_dict.TryGetValue(id, out controller))
            {
                return controller.spawn_on(src_vec, dst_vec, src_obj, dst_obj, scale);
            }
            throw new System.Exception("No found animations controller for id:" + id);
            return null;
        }
        public void clear()
        {

        }
    }
}
