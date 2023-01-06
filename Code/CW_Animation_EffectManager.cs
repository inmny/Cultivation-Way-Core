using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Animation
{
    public class CW_EffectManager : MonoBehaviour
    {
        private bool initialized = false;

        private List<CW_EffectController> controllers = new List<CW_EffectController>();
        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
            }
        }

        private void Update()
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].update(Time.fixedDeltaTime);
            }
        }
    }
}
