using NCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Core
{
    public class ModState
    {
        public bool initialized { get; internal set; }
    }
    public class ModData
    {
        public Library.Manager library_manager { get; internal set; }
    }
    [ModEntry]
    public class CW_Core : MonoBehaviour
    {
        public static CW_Core instance;
        public ModState state;
        public ModData data;
        void Awake() 
        {
            instance = this;
            state = new ModState();
            data = new ModData();
        }
        void OnEnable() { }
        void Start() { }
        void Update() 
        {
            if (!state.initialized)
            {
                state.initialized = true;
                this.initialize();
                
            }
        }

        void OnDisable() { }

        private void initialize()
        {
            data.library_manager = new Library.Manager();

            data.library_manager.init();

            Others.FastVisit.init();
            HarmonySpace.Manager.init();
        }
    }
}
