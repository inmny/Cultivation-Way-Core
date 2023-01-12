using System;
using NCMS;
using UnityEngine;
using Cultivation_Way.Content;
using Cultivation_Way.Library;
namespace Cultivation_Way{
    public enum Load_Unit_Reason
    {
        CITY_SPAWN,
        LOAD_SAVES
    }
    public class ModState
    {
        public static ModState instance;
        public CW_Library_Manager library_manager;
        public Load_Unit_Reason load_unit_reason = Load_Unit_Reason.CITY_SPAWN;
    }
    [ModEntry]
    class Main : MonoBehaviour{
        public static Main instance { get; private set; }
        public ModState mod_state;
        public CW_WorldConfig world_config;
        private bool initialized = false;
        void Awake(){
            instance = this;
        }
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
                CW_Library_Manager.create();
                mod_state = new ModState();
                world_config = new CW_WorldConfig();
                ModState.instance = mod_state;
                mod_state.library_manager = CW_Library_Manager.instance;
                mod_state.library_manager.init();

                W_Content_Manager.add_content();

                mod_state.library_manager.register();
            }
        }
    }
}