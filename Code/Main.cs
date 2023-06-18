﻿using NCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    [ModEntry]
    public class Main : MonoBehaviour
    {
        public struct ModState
        {
            public bool initialized;
            public Library.Manager library_manager;
        };
        public static Main instance;

        public ModState state = new ModState()
        {
            initialized = false,
            library_manager = null
        };

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if(!state.initialized)
            {
                state.initialized = true;
                this.initialize();
            }
        }
        void initialize()
        {
            state.library_manager = new Library.Manager();

            HarmonySpace.Manager.init();
            state.library_manager.init();
        }
    }
}
