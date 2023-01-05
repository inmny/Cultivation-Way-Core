using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.Animation
{
    internal class CW_SpriteAnimation
    {
        public float frameInterval = 0.1f;
        public float nextFrameTime = 0.1f;
        public int currFrameIndex = 0;
        public int stopFrameIndex;
        public bool stopTriggered = false;
        public bool loop;
        public bool isOn;
        public bool move = false;
        public float moveSpeed;
        public Vector3 end;
        public AnimPlayType direction;
        internal Sprite[] frames;
        internal Action[] frameActions;
        public SpriteRenderer currFrame;
        public GameObject m_gameobject;
        public Action callback;
        public Action reachAction;
        public CW_EffectController controller;
        public void create(CW_EffectController pController)
        {
            controller = pController;
            m_gameobject = UnityEngine.Object.Instantiate(controller.prefab);
            currFrame = m_gameobject.GetComponent<SpriteRenderer>();
            m_gameobject.transform.SetParent(CW_EffectManager.instance.transform);
            direction = AnimPlayType.Forward;
        }
        public void start()
        {
            isOn = true;
            m_gameobject.SetActive(true);
        }
        public void setMove(Vector3 pEnd,float pSpeed=10f,bool includeElapse=false,Action pReachAction = null)
        {
            end = pEnd;
            move = true;
            if (includeElapse)
            {
                moveSpeed = pSpeed;
            }
            else
            {
                moveSpeed = pSpeed * MapBox.instance.getCurElapsed();
            }
            if (pReachAction != null)
            {
                reachAction = pReachAction;
            }
            return;
        }
        public void clearReachAction()
        {
            reachAction = null;
        }
        private void moveUpdate()
        {
            m_gameobject.transform.position = Vector3.MoveTowards(m_gameobject.transform.position, end, moveSpeed);
        }
        public void setParent(Transform parent)
        {
            m_gameobject.transform.SetParent(parent);
        }
        public void stop(bool force = true)
        {
            if (!force && callback != null)
            {
                callback();
            }
            isOn = false;
        }
        public void stopAt(int stopIndex)
        {
            stopFrameIndex = stopIndex;
            stopTriggered = true;
        }
        public WorldTile getCurrentTile()
        {
            return MapBox.instance.GetTile((int)(m_gameobject.transform.position.x + 0.5f), (int)(m_gameobject.transform.position.y + 0.5f));
        }
        public void setFrames(Sprite[] newFrames,Action[] actions=null, bool restart = false)
        {
            frames = newFrames;
            frameActions = actions;
            if (restart)
            {
                currFrameIndex = 0;
                updateFrame();
            }
        }
        public void setFrame(int index)
        {
            if (index < 0 || index >= frames.Length)
            {
                Debug.LogWarning("Unexpected frame index");
                return;
            }
            currFrameIndex = index;
            updateFrame();
        }
        public void update(float elapse)
        {
            if(!isOn || (stopTriggered && stopFrameIndex == currFrameIndex)|| Config.paused)
            {
                return;
            }
            if (move && (m_gameobject.transform.position - end).sqrMagnitude > 1E-6f)
            {
                moveUpdate();
            }
            else
            {
                move = false; 

                if (reachAction != null)
                {
                    reachAction();
                }
            }
            if ((nextFrameTime -= elapse) > 0f)
            {
                return;
            }
            nextFrameTime = frameInterval;
            nextFrame();
        }
        public void nextFrame(int forceFrameIndex = -1)
        {
            if (forceFrameIndex < 0)
            {
                if (direction == AnimPlayType.Forward)
                {
                    currFrameIndex++;
                    if (currFrameIndex == frames.Length)
                    {
                        if (!loop)
                        {
                            stop(false);
                            return;
                        }
                        currFrameIndex = 0;
                    }
                }
                else
                {
                    currFrameIndex--;
                    if (currFrameIndex == -1)
                    {
                        if (!loop)
                        {
                            stop(false);
                            return;
                        }
                        currFrameIndex = frames.Length - 1;
                    }
                }
            }
            else
            {
                currFrameIndex = forceFrameIndex;
            }
            if (stopTriggered && currFrameIndex == stopFrameIndex)
            {
                stop(false);
            }
            updateFrame();
        }
        /// <summary>
        /// 若frameIndex<0，则为所有帧加上action
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="action"></param>
        public void setFrameAction(int frameIndex,Action action)
        {
            if (frameActions==null|| frameIndex >= frames.Length)
            {
                return;
            }
            Action[] newActions = new Action[frameActions.Length];
            if (frameIndex < 0)
            {
                for(int i = 0; i < frameActions.Length; i++)
                {
                    newActions[i] = action;
                }
            }
            else
            {
                frameActions.CopyTo(newActions, 0);
                newActions[frameIndex] = action;
                frameActions = newActions;
            }
        }
        public void updateFrame()
        {
            currFrame.sprite = frames[currFrameIndex];
            if (frameActions!=null&&frameActions[currFrameIndex] != null)
            {
                frameActions[currFrameIndex]();
            }
        }
        public void kill()
        {
            UnityEngine.Object.Destroy(m_gameobject);
        }
    }
}
