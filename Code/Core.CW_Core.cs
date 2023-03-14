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
        public long update_nr { get; internal set; }
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
            Factories.init();
        }
        void OnEnable() { }
        void Start() { }
        void Update() 
        {
            if (!state.initialized)
            {
                state.initialized = true;
                state.update_nr = 0;
                this.initialize();
            }
            if(state.update_nr % 1 == 0)
            {
                Factories.recycle_items();
                if(state.update_nr % 100 == 0)
                {
                    Factories.recycle_memory();
                }
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
