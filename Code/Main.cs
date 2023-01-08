using System;
using NCMS;
using UnityEngine;
namespace Cultivation_Way{
    [ModEntry]
    class Main : MonoBehaviour{
        public static Main Instance { get; private set; }
        private bool initialized = false;
        void Awake(){
            
        }
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
            }
        }
    }
}