using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way.Animation
{
    internal class NewEffectController
    {
        public Sprite[] defaultFrames;
        public Action[] defaultFrameActions;
        public List<NewSpriteAnimation> anims;
        public GameObject prefab;
        public int limit;
        public float xOffset;
        public float yOffset;
        public float defaultInterval;
        public void create(Sprite[] frames, int pLimit, float frameInterval, string layer)
        {
            prefab = UnityEngine.Object.Instantiate(NewEffectManager.prefab);
            prefab.GetComponent<SpriteRenderer>().sortingLayerName = layer;
            limit = pLimit;
            defaultFrames = frames;
            defaultInterval = frameInterval;
            anims = new List<NewSpriteAnimation>(pLimit + 5);
        }
        public NewSpriteAnimation add(WorldTile pTile, Vector3 scale)
        {
            if (anims.Count >= limit)
            {
                return null;
            }
            NewSpriteAnimation anim = new NewSpriteAnimation();
            anim.create(this);
            anim.setFrames(defaultFrames, defaultFrameActions);
            anim.frameInterval = defaultInterval;
            anim.m_gameobject.transform.localScale = scale;
            anim.m_gameobject.transform.position = pTile.posV3;
            anim.start();
            anims.Add(anim);
            return anim;
        }
        public NewSpriteAnimation add(Vector3 pos, Vector3 scale)
        {
            if (anims.Count >= limit)
            {
                return null;
            }
            NewSpriteAnimation anim = new NewSpriteAnimation();
            anim.create(this);
            anim.setFrames(defaultFrames, defaultFrameActions);
            anim.m_gameobject.transform.localScale = scale;
            anim.m_gameobject.transform.position = pos;
            anim.start();
            anims.Add(anim);
            return anim;
        }
        public NewSpriteAnimation add(Transform parent, Vector3 scale)
        {
            return null;
        }
        public void setDefaultFrames(Sprite[] frames,Action[] actions = null)
        {
            defaultFrames = frames;
            defaultFrameActions = actions;
        }
        public void update(float elapse)
        {
            for (int i = 0; i < anims.Count; i++)
            {
                anims[i].update(elapse);
                if (!anims[i].isOn)
                {
                    anims[i].kill();
                    anims.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
