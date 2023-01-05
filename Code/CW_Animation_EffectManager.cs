using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Animation
{
    internal class CW_EffectManager : MonoBehaviour
    {
        private bool initialized = false;
        public static CW_EffectManager instance;
        public static GameObject prefab = new GameObject();

        public List<CW_EffectController> list;
        public Dictionary<string, CW_EffectController> controllers = new Dictionary<string, CW_EffectController>();

        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                instance = this;
                instance.transform.SetParent(MapBox.instance.transform);
                prefab.transform.name = "basePrefab";
                prefab.transform.SetParent(this.transform);
                prefab.AddComponent<SpriteRenderer>();
                prefab.SetActive(false);
                

                // Load Resources
                list = controllers.Values.ToList();
            }
        }

        private void Update()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update(Time.fixedDeltaTime);
            }
        }
        private CW_EffectController load(string id, float frameInterval, string layerOrder, int limit,string extendPath="")
        {
            CW_EffectController controller = new CW_EffectController();
            controller.create(Resources.LoadAll<Sprite>("effects/" + extendPath+id), limit, frameInterval, layerOrder);
            controllers[id] = controller;
            controller.prefab.transform.name = id;
            return controller;
        }
        private void loadToStackEffects(string id,bool useBasicPrefab=true,float frameInterval = 0.1f, string layerOrder="EffectsBack", int limit=60)
        {
            MapBox.instance.stackEffects.CallMethod("loadEffect", id, useBasicPrefab, frameInterval, layerOrder, limit);
        }
        public static CW_SpriteAnimation spawnOn(string id,WorldTile pTile, Vector3 scale)
        {
            CW_EffectController controller;
            if(instance.controllers.TryGetValue(id,out controller))
            {
                return controller.add(pTile, scale);
            }
            return null;
        }
        public static CW_SpriteAnimation spawnOn(string id,BaseSimObject pObject, Vector3 scale)
        {
            CW_EffectController controller;
            if (instance.controllers.TryGetValue(id, out controller))
            {
                return controller.add(pObject.gameObject.transform,scale);
            }
            return null;
        }
        public static CW_SpriteAnimation spawnOn(string id, Vector3 pos, Vector3 scale)
        {
            CW_EffectController controller;
            if (instance.controllers.TryGetValue(id, out controller))
            {
                return controller.add(pos, scale);
            }
            return null;
        }
        public static CW_SpriteAnimation spawnOn(string id, WorldTile pTile, float scale)
        {
            return spawnOn(id, pTile, new Vector3(scale, scale, scale));
        }
        public static CW_SpriteAnimation spawnOn(string id, BaseSimObject pObject, float scale)
        {
            return spawnOn(id, pObject, new Vector3(scale, scale, scale));
        }
        public static CW_SpriteAnimation spawnOn(string id, Vector3 pos, float scale)
        {
            return spawnOn(id, pos, new Vector3(scale, scale, scale));
        }
        public void public_load(string id, float frameInterval, string layerOrder, int limit)
        {
            list.Add(load(id, frameInterval, layerOrder, limit));
        }
    }
}
