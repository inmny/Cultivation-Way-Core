using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Animation
{
    public class CW_EffectManager : MonoBehaviour
    {
        public static CW_EffectManager instance;
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
            }
        }

        private void Update()
        {
            int i;
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
        public CW_EffectController load_as_controller(string id, string path_to_anim, Vector2 base_offset, int anim_limit = 100, float base_scale = 1.0f, CW_AnimationSetting controller_setting = null)
        {
            if (controllers_dict.ContainsKey(id))
            {
                if (Others.CW_Constants.is_debugging) throw new System.Exception(string.Format("Repeated Controller id with {0}", id));
                return null;
            }
            Sprite[] sprites = Resources.LoadAll<Sprite>(path_to_anim);
            if (sprites == null) { throw new System.Exception("No found sprites under:" + path_to_anim); return null; }
            CW_EffectController controller = new CW_EffectController(anim_limit, controller_setting==null?new CW_AnimationSetting():controller_setting, sprites, default_prefab, base_scale, base_offset);
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
        private bool test_anim(string anim_id, string src_id, string dst_id, int src_x, int src_y, int dst_x, int dst_y, float scale)
        {
            
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            
            Vector2 src_vec = (src_actor == null ? new Vector2(src_x, src_y) : src_actor.currentPosition);
            Vector2 dst_vec = (dst_actor == null ? new Vector2(dst_x, dst_y) : dst_actor.currentPosition);
            WorldBoxConsole.Console.print(string.Format("src_vec:{0}, dst_vec:{1}", src_vec, dst_vec));
            ModState.instance.effect_manager.spawn_anim(anim_id, src_vec, dst_vec, MapBox.instance.getActorByID(src_id), MapBox.instance.getActorByID(dst_id), scale);
            return true;
        }
    }
}
